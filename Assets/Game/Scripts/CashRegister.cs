using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    private CashRegisterMoneyArea _moneyArea;
    private List<Customer> _customers = new List<Customer>();
    public Transform productPos;
    public Customer firsCustomer;

    private void Awake()
    {
        _moneyArea = GetComponent<CashRegisterMoneyArea>();
    }

    public async Task Sale()
    {
        // Customer customer = _customers.First();
        Debug.Log("Customer paying");

        await firsCustomer.stackedProduct.transform.DOJump(productPos.position, 1f, 1, 1f).SetLoops(2, LoopType.Yoyo).AsyncWaitForCompletion();

        Debug.Log("Customer paid product");
        _customers.Remove(firsCustomer);

        firsCustomer.MoveExit();
        
        _moneyArea.SpawnMoney(firsCustomer.stackedProduct.data.Price);

        OnPaid();
    }

    public void MoveCustomerNewCashRegisterPos(Customer customer)
    {
        customer.MovePosition(
            SetCustomerPos(customer), (() =>
            {
                if (_customers.Count > 0 &&_customers.First() == customer && !customer.isPaying )
                {
                    customer.isPaying = true;
                    firsCustomer = customer;
                    _ = Sale();
                }
            }));
    }

    public void OnPaid()
    {
        foreach (var t in _customers)
        {
            MoveCustomerNewCashRegisterPos(t);
        }
    }

    public Vector3 SetCustomerPos(Customer customer)
    {
        return new Vector3(transform.position.x, transform.position.y,
            transform.position.z + (_customers.IndexOf(customer) + 1));
    }

    public void NewCustomer(Customer customer)
    {
        _customers.Add(customer);
    }

    public int GetQueueCustomerCount()
    {
        return _customers.Count;
    }
}