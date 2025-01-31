using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainCanvasManager : MonoBehaviour
{
    private MoneyControlManager _moneyControlManager;
    
    [SerializeField] private List<TMP_Text> moneyTexts = new List<TMP_Text>();
    
    private void Awake()
    {
        _moneyControlManager = MoneyControlManager.Instance;
    }

    private void OnEnable()
    {
        SetMoneyTexts();
    }

    private void SetMoneyTexts()
    {
        foreach (var text in moneyTexts)
        {
            _moneyControlManager.coinTexts.Add(text);
        }
    }
    
}
