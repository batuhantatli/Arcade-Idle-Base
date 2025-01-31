using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ProductStand : MonoBehaviour
{
    [Serializable]
    public class CustomerTarget
    {
        public ProductStand selectedStand;
        public Vector3 moveTarget;
        public bool isEmpty = true;
    }

    private readonly Stack<Product> _stackedProducts = new Stack<Product>();
    private readonly List<CustomerTarget> _customerTargets = new List<CustomerTarget>();

    [SerializeField] private List<Transform> waitPoints = new List<Transform>();
    [SerializeField] private int productCapacity;
    public ProductType type;

    private void Start()
    {
        SetCustomerWaitPoints();
    }

    public void PushToStack(Product product)
    {
        Vector3 targetPosition = CalculateProductPosition();
        product.Jump(transform, targetPosition, (() => { _stackedProducts.Push(product); }));
    }

    private Product PopFromStack()
    {
        if (_stackedProducts.Count <= 0) return null;
        Product item = _stackedProducts.Pop();
        return item;
    }

    public Product TakeItem()
    {
        Product index = PopFromStack(); // Use Pop instead of Dequeue

        return index;
    }

    public Vector3 CalculateProductPosition()
    {
        return new Vector3(0, 0, _stackedProducts.Count+1);
    }
    public bool IsReadyForPush()
    {
        return _stackedProducts.Count < productCapacity;
    }

    public bool IsReadyForPop()
    {
        return _stackedProducts.Count > 0;
    }

    public int GetProductCount()
    {
        return _stackedProducts.Count;
    }


    public void SetCustomerWaitPoints()
    {
        for (int i = 0; i < waitPoints.Count; i++)
        {
            CustomerTarget point = new CustomerTarget
            {
                selectedStand = this,
                moveTarget = waitPoints[i].position
            };
            _customerTargets.Add(point);
        }
    }

    public CustomerTarget GetEmptyPoint()
    {
        var customerWaitPoint = _customerTargets.FirstOrDefault(t => t.isEmpty);
        if (customerWaitPoint != null)
        {
            return customerWaitPoint;
        }

        return null;
    }

    public int GetEmptyWaitCount()
    {
        return _customerTargets.Count(t => t.isEmpty);
    }
}