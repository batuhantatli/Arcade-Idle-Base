using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    [SerializeField] private Transform productPos;

    private CashRegisterMoneyArea _moneyArea;
    private Customer _firstCustomer;
    private readonly Queue<Customer> _customers = new Queue<Customer>();

    private void Awake()
    {
        _moneyArea = GetComponent<CashRegisterMoneyArea>();
    }


    private async Task Sale()
    {
        if (_customers.Count == 0) return;

        Debug.Log("Customer paying");
        _firstCustomer = _customers.Peek();

        await _firstCustomer.stackedProduct.transform.DOJump(productPos.position, 1f, 1, 1f).SetLoops(2, LoopType.Yoyo)
            .AsyncWaitForCompletion();

        Debug.Log("Customer paid product");

        _firstCustomer.MoveExit();
        _customers.Dequeue();
        _moneyArea.SpawnMoney(_firstCustomer.stackedProduct.data.Price);

        SoldProduct();
    }

    private void SoldProduct()
    {
        foreach (var t in _customers)
        {
            MoveCustomerNewCashRegisterPos(t);
        }
    }


    public void MoveCustomerNewCashRegisterPos(Customer customer)
    {
        customer.MovePosition(
            SetCustomerPos(customer), (() =>
            {
                if (_customers.Count > 0 && _customers.Peek() == customer && !customer.isPaying)
                {
                    customer.isPaying = true;
                    _ = Sale();
                }
            }));
    }


    public Vector3 SetCustomerPos(Customer customer)
    {
        int index = 0;
        foreach (var c in _customers)
        {
            if (c == customer)
                break;
            index++;
        }

        return new Vector3(transform.position.x, transform.position.y, transform.position.z + (index + 1));
    }


    public void NewCustomer(Customer customer)
    {
        _customers.Enqueue(customer);
    }

    public int GetQueueCustomerCount()
    {
        return _customers.Count;
    }
}