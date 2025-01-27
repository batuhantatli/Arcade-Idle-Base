using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    private List<Customer> _customers = new List<Customer>();
    public Transform productPos;


    public IEnumerator Sale()
    {
        Customer customer = _customers.First();
        Debug.Log("Customer paying");

         yield return customer.stackedProduct.transform.DOJump(productPos.position, 1f, 1, 1f).SetLoops(2, LoopType.Yoyo).WaitForCompletion();

        Debug.Log("Customer paid product");
        _customers.Remove(customer);
        
        customer.MoveExit();
        OnPaid();
    }

    public void MoveCustomerNewCashRegisterPos(Customer customer)
    {
        customer.MovePosition(
            SetCustomerPos(customer), (() =>
            {
                if ( _customers.Count>0 &&_customers.First() == customer)
                {
                    StartCoroutine(Sale());
                }
            }));
    }

    public Vector3 SetCustomerPos(Customer customer)
    {
        return new Vector3(transform.position.x, transform.position.y, transform.position.z + (_customers.IndexOf(customer) +1));
    }

    public void OnPaid()
    {
        for (var i = 0; i < _customers.Count; i++)
        {
            var customer = _customers[i];
            customer.MovePosition(
                SetCustomerPos(customer), (() =>
                {
                    if (_customers.Count > 0 && _customers.First() == customer)
                    {
                        StartCoroutine(Sale());
                    }
                }));
        }
    }


    public void NewCustomer(Customer customer)
    {
        _customers.Add(customer);
    }
}