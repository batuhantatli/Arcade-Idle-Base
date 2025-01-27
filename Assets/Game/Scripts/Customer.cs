using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public CashRegister _cashRegister;
    private StandOrderProcessor.CustomerTarget _customerTarget;
    private NavMeshAgent _navMeshAgent;
    private Transform _startPoint;
    public Transform _productStackPoint;
    public Product stackedProduct;
    private CustomerManager _customerManager;
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetCashRegister(CashRegister cashRegister)
    {
        _cashRegister = cashRegister;
    }    
    public void SetCustomerManager(CustomerManager customerManager)
    {
        _customerManager = customerManager ;
    }

    private void Start()
    {
        _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    public void SetWaitPoint(StandOrderProcessor.CustomerTarget customerTarget)
    {
        _customerTarget = customerTarget;
        _customerTarget.isEmpty = false;
        MoveStand();
    }

    public void SetStartPoint(Transform transform)
    {
        _startPoint = transform;
    }

    public void MoveStand()
    {
        Debug.Log("Customer moving stand");
        MovePosition(_customerTarget.moveTarget.position,(() =>
        {
            StartCoroutine(ProductProcess());
        } ));

    }

    public IEnumerator ProductProcess()
    {
        while (true)
        {
            if (_customerTarget.selectedStand.IsReadyForPop())
            {
                Product product = _customerTarget.selectedStand.TakeItem();
                Debug.Log("Customer taking product");
                product.Jump(_productStackPoint,Vector3.zero, ((() =>
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
        MovePosition(_cashRegister.SetCustomerPos(this) , () =>
        {
            _cashRegister.MoveCustomerNewCashRegisterPos(this);
        });
    }

    [Button]
    public void MoveExit()
    {
        Debug.Log("Customer moving exit.");
        MovePosition(_startPoint.position, (() =>
        {
            Debug.Log("Exited");
            _customerManager.OnCustomerTaskDone?.Invoke();
            gameObject.SetActive(false);
        }));
    }
    
    public void MovePosition(Vector3 moveTarget, Action onCompleteMove = null)
    {
        _navMeshAgent.SetDestination(moveTarget);
        StartCoroutine(CheckIfReachedDestination((() =>
        {
            onCompleteMove?.Invoke();
        })));
    }
    
    private IEnumerator CheckIfReachedDestination(Action onComplateMove)
    {
        while (true)
        {
            if (HasReachedDestination())
            {
                onComplateMove?.Invoke();
                yield break; // Coroutine sonlandır
            }

            yield return null;
        }
    }
    public bool HasReachedDestination()
    {
        // Hedefe ulaşılıp ulaşılmadığını kontrol et
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
