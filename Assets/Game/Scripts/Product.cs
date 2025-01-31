using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Product : MonoBehaviour
{
    public ProductData data;
    
    public void SetProductData(ProductData _data)
    {
        data.Price = _data.Price;
        data.Type = _data.Type;
    }
    public void Jump(Transform parent,Vector3 movePoint , Action onComplete = null)
    {
        transform.SetParent(parent);
        transform.rotation = quaternion.identity;
        transform.DOLocalJump(movePoint, 2f,1 , .2f);
        onComplete?.Invoke();
    }

    public void SetInitializeTransform(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
}
