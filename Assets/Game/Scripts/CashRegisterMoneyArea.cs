using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegisterMoneyArea : MonoBehaviour
{
    private ObjectPool _objectPool;
    private int _currentPrice;
    private readonly List<MoneyPiece> _moneyPieces = new List<MoneyPiece>();
    private int _currentX = 0; // Başlangıç X koordinatı
    private int _currentY = 0; // Başlangıç Y koordinatı
    private int _currentZ = 0; // Başlangıç Z koordinatı
    
    [Header("Money Spawn Settings")]
    [SerializeField] private Transform moneySpawnPoint;
    [SerializeField] private int countPerProduct;
    [SerializeField] private int width = 5; 
    [SerializeField] private int depth = 5; 
    [SerializeField] private float xSpaceLength;
    [SerializeField] private float ySpaceLength;
    [SerializeField] private float zSpaceLength;


    private void Awake()
    {
        _objectPool = ObjectPool.Instance;
    }

    public void SpawnMoney(int price)
    {
        SetHoldPrice(price);
        for (int i = 0; i < countPerProduct; i++)
        {
            var g = _objectPool.moneyPool.Get();
            _moneyPieces.Add(g);
            g.transform.SetParent(moneySpawnPoint);

            Vector3 spawnPosition = new Vector3(_currentX * xSpaceLength, _currentY * ySpaceLength, _currentZ * zSpaceLength);
            g.transform.localPosition = spawnPosition;

            _currentX++;

            if (_currentX >= width)
            {
                _currentX = 0;
                _currentZ++;
            }

            if (_currentZ >= depth)
            {
                _currentZ = 0;
                _currentY++;
            }
        }
    }

    public void SetHoldPrice(int price)
    {
        _currentPrice += price;
    }

    public void GiveMoney(Transform player , MoneyControlManager moneyControlManager)
    {
        for (int i = 0; i < _moneyPieces.Count; i++)
        {
            var money = _moneyPieces[i];
            money.Move(player.transform,_objectPool);
            _moneyPieces.Remove(money);
        }

        moneyControlManager.ChangeMoney(_currentPrice);
        
        _currentPrice = 0;

        _currentX = 0;
        _currentY = 0;
        _currentZ = 0;
    }

    
    
}
