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
        var productStands = standList.Where(t => t.GetEmptyWaitCount() > 0 ).OrderByDescending(t=>t.GetProductCount());
        return productStands.First().GetEmptyPoint();
    }

    public int EmptyProductStandCount()
    {
        return standList.Count(t => t.GetEmptyWaitCount() > 0);
    }
}
