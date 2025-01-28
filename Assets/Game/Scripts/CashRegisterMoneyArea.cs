using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegisterMoneyArea : MonoBehaviour
{
    public struct GridCell
    {
        public int X;
        public int Y;
        public int Z;

        public GridCell(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

    }
    private ObjectPool _objectPool;
    private GridCell[,,] grid;
    public List<MoneyPiece> moneyPieces = new List<MoneyPiece>();
    public Transform moneySpawnPoint;
    int width = 5;  // X ekseni boyutu
    int height = 5; // Y ekseni boyutu
    int depth = 5;  // Z ekseni boyutu
    public float xSpaceLength;
    public float ySpaceLength;
    public float zSpaceLength;


    private void Awake()
    {
        _objectPool = ObjectPool.Instance;
    }

    int currentX = 0; // Başlangıç X koordinatı
    int currentY = 0; // Başlangıç Y koordinatı
    int currentZ = 0; // Başlangıç Z koordinatı

    public void SpawnMoney()
    {
        for (int i = 0; i < 3; i++)
        {
            // Eğer grid dışına çıkıyorsa, yeni spawnları durdur
            if (currentX >= width || currentZ >= depth)
            {
                Debug.LogWarning("Grid doldu, daha fazla nesne yerleştirilemez!");
                break;
            }

            var g = _objectPool.moneyPool.Get();
            moneyPieces.Add(g);
            g.transform.SetParent(moneySpawnPoint);

            Vector3 spawnPosition = new Vector3(currentX * xSpaceLength, currentY * ySpaceLength, currentZ * zSpaceLength);
            g.transform.localPosition = spawnPosition;


            currentX++;

            if (currentX >= width)
            {
                currentX = 0;
                currentZ++;
            }

            if (currentZ >= depth)
            {
                currentZ = 0;
                currentY++;
            }
        }
    }

    public void GiveMoney(Transform player , MoneyControlManager moneyControlManager)
    {
        for (int i = 0; i < moneyPieces.Count; i++)
        {
            var money = moneyPieces[i];
            money.Move(player.transform,_objectPool);
            moneyPieces.Remove(money);
            moneyControlManager.ChangeMoney(money.increaseCount);
        }

        currentX = 0;
        currentY = 0;
        currentZ = 0;
    }

    
    
}
