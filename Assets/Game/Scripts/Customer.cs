using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Customer : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private CashRegister _cashRegister;
    private CustomerManager _customerManager;
    
    private ProductStand.CustomerTarget _customerTarget;
    public Product stackedProduct;
    private Transform _startPoint;

    public bool isPaying;
    
    
    [SerializeField] private Transform productStackPoint;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnDisable()
    {
        isPaying = false;
        var product = stackedProduct;
        if (product != null)
        {
            stackedProduct = null;
        }
    }
    
    private void Start()
    {
        _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    public void Initialize(CustomerManager customerManager, CashRegister cashRegister,
        ProductStand.CustomerTarget customerTarget, Transform startPoint)
    {
        _customerManager = customerManager ?? throw new ArgumentNullException(nameof(customerManager));
        _cashRegister = cashRegister ?? throw new ArgumentNullException(nameof(cashRegister));

        _startPoint = startPoint;

        transform.position = _startPoint.position;

        SetStandPoint(customerTarget);
    }

    public void SetStandPoint(ProductStand.CustomerTarget customerTarget)
    {
        _customerTarget = customerTarget;
        _customerTarget.isEmpty = false;
        MoveStand();
    }

    public void MoveStand()
    {
        Debug.Log("Customer moving stand");
        MovePosition(_customerTarget.moveTarget, (() => { StartCoroutine(ProductProcess()); }));
    }

    public IEnumerator ProductProcess()
    {
        while (true)
        {
            if (_customerTarget.selectedStand.IsReadyForPop())
            {
                Product product = _customerTarget.selectedStand.TakeItem();
                Debug.Log("Customer taking product");
                product.Jump(productStackPoint, Vector3.zero, ((() =>
                {
                    _customerTarget.isEmpty = true;
                    MoveCashRegister();
                })));
                stackedProduct = product;

                yield break;
            }

            Debug.Log("Customer waiting product");
            yield return new WaitForSeconds(.2f);
        }
    }

    public void MoveCashRegister()
    {
        Debug.Log("Customer moving cash register");
        _cashRegister.NewCustomer(this);
        MovePosition(_cashRegister.SetCustomerPos(this), () => { _cashRegister.MoveCustomerNewCashRegisterPos(this); });
    }

    [Button]
    public void MoveExit()
    {
        Debug.Log("Customer moving exit.");
        MovePosition(_startPoint.position, (() =>
        {
            Debug.Log("Customer Exited");
            _customerManager.OnCustomerTaskDone?.Invoke(this);
        }));
    }

    public void MovePosition(Vector3 moveTarget, Action onCompleteMove = null)
    {
        _navMeshAgent.SetDestination(moveTarget);
        StartCoroutine(CheckIfReachedDestination((() => { onCompleteMove?.Invoke(); })));
    }

    private IEnumerator CheckIfReachedDestination(Action onComplateMove)
    {
        while (true)
        {
            if (HasReachedDestination())
            {
                onComplateMove?.Invoke();
                yield break; 
            }

            yield return null;
        }
    }

    public bool HasReachedDestination()
    {
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }
}