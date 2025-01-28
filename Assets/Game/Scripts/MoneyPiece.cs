using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoneyPiece : MonoBehaviour
{
    public int increaseCount;

    private void OnEnable()
    {
        transform.rotation = new Quaternion(0, 0, 0,0);
    }

    public void Move(Transform movePoint , ObjectPool pool)
    {
        transform.SetParent(movePoint);
        transform.DOLocalJump(Vector3.zero, 1, 1, .5f).OnComplete((() =>
        {
            pool.moneyPool.Relase(this.gameObject);
        }));
    }
}
