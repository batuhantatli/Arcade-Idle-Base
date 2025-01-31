using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CashRegisterManager : Singleton<CashRegisterManager>
{
    [SerializeField] private List<CashRegister> cashRegisterList = new List<CashRegister>();

    #region Public Methods
    public CashRegister GetCashRegister()
    {
        return cashRegisterList.OrderBy(t => t.GetQueueCustomerCount()).First();
    }
    #endregion

 
}