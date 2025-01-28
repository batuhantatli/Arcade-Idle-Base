using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyControlManager : Singleton<MoneyControlManager>
{
    public Action OnMoneyChange;

    public List<TMP_Text> coinTexts = new List<TMP_Text>() ;
    public float currentMoney;

    [Header("Debug Mode")] public float money = 0;
    public KeyCode keyCode = KeyCode.L;

    private void Start()
    {
        MoneyTextUpdate();
    }

    [Button]
    public bool ChangeMoney(float money)
    {
        if (!CheckCanBuy(money))
        {
            return false;
        }

        currentMoney += money;
        MoneyTextUpdate();
        OnMoneyChange?.Invoke();
        return true;
    }

    public bool CheckCanBuy(float money)
    {
        return !(currentMoney + money < 0);
    }

    public void IncreaseMoney()
    {
        ChangeMoney(money);
    }

    public void DecreaseMoney()
    {
        ChangeMoney(-money);
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            IncreaseMoney();
        }
    }

    private void MoneyTextUpdate()
    {
        foreach (TMP_Text text in coinTexts)
        {
            text.text = $"{Formatter.Format(currentMoney)}";
            text.transform.localScale = Vector3.one;
            text.transform.DOKill();
            text.transform.DOPunchScale(Vector3.one * 0.25f, .25f, 1, 0.1f).SetEase(Ease.InOutBounce);
        }
    }
}