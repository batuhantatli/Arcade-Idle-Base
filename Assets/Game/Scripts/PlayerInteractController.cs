using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    private MoneyControlManager moneyControlManager;
    private void Awake()
    {
        moneyControlManager = MoneyControlManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CashRegisterMoneyArea moneyArea))
        {
            moneyArea.GiveMoney(transform , moneyControlManager);
        }
    }
}
