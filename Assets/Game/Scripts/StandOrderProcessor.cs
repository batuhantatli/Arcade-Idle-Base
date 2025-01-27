using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class StandOrderProcessor : MonoBehaviour
{
    [Serializable]
    public class CustomerTarget
    {
        public ProductStand selectedStand;
        public Transform moveTarget;
        public bool isEmpty = true;
    }
    
    private ProductStand _stand;
    
    private List<CustomerTarget> customerTargets = new List<CustomerTarget>();

    [SerializeField]
    private List<Transform> waitPoints = new List<Transform>();
    private void Awake()
    {
        _stand = GetComponent<ProductStand>();
    }

    private void Start()
    {
        SetCustomerWaitPoints();
    }

    public void SetCustomerWaitPoints()
    {
        for (int i = 0; i < waitPoints.Count; i++)
        {
            CustomerTarget point = new CustomerTarget
            {
                selectedStand = _stand,
                moveTarget = waitPoints[i]
                
            };
            customerTargets.Add(point);
        }
    }

    public CustomerTarget GetRandomEmptyPoint()
    {
        var customerWaitPoint = customerTargets.FirstOrDefault(t => t.isEmpty);
        if (customerWaitPoint != null)
        {
            return customerWaitPoint;
        }

        return null;
    }

    public int GetEmptyWaitCount()
    {
        return customerTargets.Count(t=> t.isEmpty);
    }
}
