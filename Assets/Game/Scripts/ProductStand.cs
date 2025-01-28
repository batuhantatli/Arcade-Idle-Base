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
        public Transform moveTarget;
        public bool isEmpty = true;
    }
    
    private Stack<Product> _stackedProducts = new Stack<Product>();
    private List<CustomerTarget> _customerTargets = new List<CustomerTarget>();
    
    [SerializeField] private List<Transform> waitPoints = new List<Transform>();
    [SerializeField] private int capacity;
    
    private void Start()
    {
        SetCustomerWaitPoints();
    }
    
    public void PushToStack(Product product)
    {
        MoveProductToPosition(product , () =>
        {
            _stackedProducts.Push(product);
        });
    }

    private Product PopFromStack()
    {
        if (_stackedProducts.Count > 0)
        {
            Product item = _stackedProducts.Pop();
            return item;
        }

        return null;

    }
    
    public Product TakeItem()
    {
        Product index = PopFromStack(); // Use Pop instead of Dequeue
        
        return index;
    }
    
    public Vector3 CalculateProductPosition()
    {
        return new Vector3(0, 0, _stackedProducts.Count);
    }

    public void MoveProductToPosition(Product product, Action onMoveComplete)
    {
        Vector3 targetPosition = CalculateProductPosition();
        product.Jump(transform, targetPosition, onMoveComplete);
    }
    
    public bool IsReadyForPush()
    {
        return _stackedProducts.Count < capacity;
    }    
    public bool IsReadyForPop()
    {
        return _stackedProducts.Count > 0;
    }



    public void SetCustomerWaitPoints()
    {
        for (int i = 0; i < waitPoints.Count; i++)
        {
            CustomerTarget point = new CustomerTarget
            {
                selectedStand = this,
                moveTarget = waitPoints[i]
                
            };
            _customerTargets.Add(point);
        }
    }

    public CustomerTarget GetRandomEmptyPoint()
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
        return _customerTargets.Count(t=> t.isEmpty);
    }
    
    
    
    
    
}
