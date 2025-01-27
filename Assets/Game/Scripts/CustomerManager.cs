using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class CustomerManager : MonoBehaviour
{
    public float customerSpawnRate;
    public float customerWaitRateForFull;
    private ProductStandManager _productStandManager;
    public CashRegister c;
    public Action OnCustomerTaskDone;
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private Transform customerSpawnPoint;
    [SerializeField] private int maxCustomerCount;
    public int currentActiveCustomerCount;
    
    private void Awake()
    {
        _productStandManager = ProductStandManager.Instance;
    }

    private void OnEnable()
    {
        OnCustomerTaskDone += DestroyCustomer;
    }

    private void OnDisable()
    {
        OnCustomerTaskDone -= DestroyCustomer;
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomerLoop());
    }

    public void SpawnCustomer()
    {
        Customer customer = Instantiate(customerPrefab, customerSpawnPoint.position, Quaternion.identity);
        customer.SetStartPoint(customerSpawnPoint);
        customer.SetWaitPoint(_productStandManager.GetEmptyProductStand());
        customer.SetCustomerManager(this);
        currentActiveCustomerCount++;
        customer._cashRegister = c;
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
        return _productStandManager.EmptyProductStandCount() > 0 && maxCustomerCount > currentActiveCustomerCount;
    }

    public void DestroyCustomer()
    {
        currentActiveCustomerCount--;
    }

    
}
