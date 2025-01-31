using System;
using System.Collections;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private ObjectPool _objectPool;
    private ProductStandManager _productStandManager;
    private CashRegisterManager _cashRegisterManager;
    private CashRegister _selectedCashRegister;
    private int _currentActiveCustomerCount;
    private WaitForSeconds _customerSpawnWait;
    private WaitForSeconds _customerFullWait;

    [SerializeField] private Customer customerPrefab;
    [SerializeField] private Transform customerSpawnPoint;
    [SerializeField] private float customerWaitRateForFull;
    [SerializeField] private float customerSpawnRate;
    [SerializeField] private int maxCustomerCount;

    public Action<Customer> OnCustomerTaskDone;


    private void Awake()
    {
        _cashRegisterManager = CashRegisterManager.Instance;
        _productStandManager = ProductStandManager.Instance;
        _objectPool = ObjectPool.Instance;
    }

    private void OnEnable()
    {
        OnCustomerTaskDone += CustomerExit;
    }

    private void OnDisable()
    {
        OnCustomerTaskDone -= CustomerExit;
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomerLoop());
        _customerSpawnWait = new WaitForSeconds(customerSpawnRate);
        _customerFullWait = new WaitForSeconds(customerWaitRateForFull);
    }

    public void SpawnCustomer()
    {
        Customer customer = _objectPool.customerPool.Get();
        _selectedCashRegister = _cashRegisterManager.GetCashRegister();
        customer.Initialize(this, _selectedCashRegister, _productStandManager.GetEmptyProductStand(),
            customerSpawnPoint);

        _currentActiveCustomerCount++;
    }

    public IEnumerator SpawnCustomerLoop()
    {
        while (true)
        {
            if (CustomerSpawnController())
            {
                SpawnCustomer();
                yield return _customerSpawnWait;
            }

            yield return _customerFullWait;
        }
    }

    public bool CustomerSpawnController()
    {
        return _productStandManager.EmptyProductStandCount() > 0 && maxCustomerCount > _currentActiveCustomerCount;
    }


    public void CustomerExit(Customer customer)
    {
        _currentActiveCustomerCount--;
        _objectPool.GetProductPool(customer.stackedProduct.data.Type).productPool
            .Relase(customer.stackedProduct.gameObject);
        _objectPool.customerPool.Relase(customer.gameObject);
    }
}