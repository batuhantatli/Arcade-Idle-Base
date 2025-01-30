using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CashRegisterManager : Singleton<CashRegisterManager>
{
    public List<CashRegister> cashRegisterList = new List<CashRegister>();

    public CashRegister GetCashRegister()
    {
        var productStands = cashRegisterList.OrderByDescending(t => t.GetQueueCustomerCount()).ToList();
        return productStands.First();
    }
}
