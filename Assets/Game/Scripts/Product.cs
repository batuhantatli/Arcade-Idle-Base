using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Product : MonoBehaviour
{
    private void OnEnable()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void Jump(Transform parent,Vector3 movePoint , Action onComplete = null)
    {
        transform.SetParent(parent);
        transform.rotation = new Quaternion(0,0,0,0);
        transform.DOLocalJump(movePoint, 1f, 1, .5f);
        onComplete?.Invoke();
    }
}
