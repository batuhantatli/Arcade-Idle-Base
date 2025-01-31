using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    private const float WaitRefreshRate = .2f;

    private NavMeshAgent _navMeshAgent;
    private CashRegister _cashRegister;
    private CustomerManager _customerManager;
    private ProductStand.CustomerTarget _customerTarget;
    private Transform _startPoint;
    private WaitForSeconds _productWaitRefreshRate;
    [SerializeField] private Transform productStackPoint;

    public Product stackedProduct;
    public bool isPaying;
    
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
        _productWaitRefreshRate = new WaitForSeconds(WaitRefreshRate);

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


    private void MoveStand()
    {
        Debug.Log("Customer moving stand");
        MovePosition(_customerTarget.moveTarget, (OnProductProcess));
    }

    private void OnProductProcess()
    {
        if (_customerTarget.selectedStand.IsReadyForPop())
        {
            Product product = _customerTarget.selectedStand.TakeItem();
            Debug.Log("Customer taking product");
            product.Jump(productStackPoint, Vector3.zero, (() =>
            {
                _customerTarget.isEmpty = true;
                MoveCashRegister();
            }));
            stackedProduct = product;
        }
        else
        {
            Debug.Log("Customer waiting product");
            StartCoroutine(WaitForProduct());
        }
    }

    private IEnumerator WaitForProduct()
    {
        while (!_customerTarget.selectedStand.IsReadyForPop())
        {
            Debug.Log("Waiting for product...");
            yield return _productWaitRefreshRate;
        }

        OnProductProcess(); 
    }

    private void MoveCashRegister()
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


    private void SetStandPoint(ProductStand.CustomerTarget customerTarget)
    {
        _customerTarget = customerTarget;
        _customerTarget.isEmpty = false;
        MoveStand();
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

    private bool HasReachedDestination()
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