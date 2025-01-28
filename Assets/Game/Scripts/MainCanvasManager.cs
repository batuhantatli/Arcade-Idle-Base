using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainCanvasManager : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> moneyTexts = new List<TMP_Text>();
    private MoneyControlManager _moneyControlManager;
    private void Awake()
    {
        _moneyControlManager = MoneyControlManager.Instance;
    }

    private void OnEnable()
    {
        SetMoneyTexts();
    }

    public void SetMoneyTexts()
    {
        foreach (var text in moneyTexts)
        {
            _moneyControlManager.coinTexts.Add(text);
        }
    }
    
}
