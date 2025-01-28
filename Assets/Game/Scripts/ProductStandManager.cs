using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental;
using UnityEngine;

public class ProductStandManager : Singleton<ProductStandManager>
{
    public List<ProductStand> standList = new List<ProductStand>();

    public ProductStand.CustomerTarget GetEmptyProductStand()
    {
        var productStands = standList.OrderByDescending(t => t.GetEmptyWaitCount()).ToList();
        return productStands.First().GetRandomEmptyPoint();
    }

    public int EmptyProductStandCount()
    {
        return standList.Count(t => t.GetEmptyWaitCount() > 0);
    }
}
