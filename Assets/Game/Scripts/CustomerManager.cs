using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Serialization;
using Task = System.Threading.Tasks.Task;

public class CustomerManager : MonoBehaviour
{
    private ObjectPool _objectPool;
    private ProductStandManager _productStandManager;
    private CashRegisterManager _cashRegisterManager;
    private CashRegister _selectedCashRegister;
    private int _currentActiveCustomerCount;

    [SerializeField] private Customer customerPrefab;
    [SerializeField] private Transform customerSpawnPoint;
    [SerializeField] private float customerWaitRateForFull;
    [SerializeField] private float customerSpawnRate;
    [SerializeField] private int maxCustomerCount;
    
    public  Action<Customer> OnCustomerTaskDone;
    
    
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
    }

    public void SpawnCustomer()
    {
        Customer customer = ObjectPool.Instance.customerPool.Get();
        _selectedCashRegister = _cashRegisterManager.GetCashRegister();
        customer.Initialize(this  , _selectedCashRegister , _productStandManager.GetEmptyProductStand() , customerSpawnPoint);
        
        _currentActiveCustomerCount++;
    }

    public IEnumerator SpawnCustomerLoop()
    {
        while (true)
        {
            if ( CustomerSpawnController())
            {
                SpawnCustomer();
                yield return new WaitForSeconds(customerSpawnRate);
            }

            yield return new WaitForSeconds(customerWaitRateForFull);

        }
    }

    public bool CustomerSpawnController()
    {
        return _productStandManager.EmptyProductStandCount() > 0 && maxCustomerCount > _currentActiveCustomerCount;
    }
    

    public void CustomerExit(Customer customer)
    {
        _currentActiveCustomerCount--;
        _objectPool.productPool.Relase(customer.stackedProduct.gameObject);
        _objectPool.customerPool.Relase(customer.gameObject);
    }

    
}
