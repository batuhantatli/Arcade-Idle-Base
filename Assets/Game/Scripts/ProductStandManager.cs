using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental;
using UnityEngine;

public class ProductStandManager : Singleton<ProductStandManager>
{
    public List<StandOrderProcessor> standList = new List<StandOrderProcessor>();

    public StandOrderProcessor.CustomerTarget GetEmptyProductStand()
    {
        var g = standList.OrderByDescending(t => t.GetEmptyWaitCount()).ToList();
        return g.First().GetRandomEmptyPoint();
    }

    public int EmptyProductStandCount()
    {
        return standList.Count(t => t.GetEmptyWaitCount() > 0);
    }
}
