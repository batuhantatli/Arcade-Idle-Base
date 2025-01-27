using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ProductStand : MonoBehaviour
{
    public int capacity;
    
    private Stack<Product> _stackedProducts = new Stack<Product>();
    
    // Yığına veri ekleme
    public void PushToStack(Product product)
    {
        MoveProductToPosition(product , () =>
        {
            _stackedProducts.Push(product);
        });
    }

    // Yığından veri çıkarma
    public Product PopFromStack()
    {
        if (_stackedProducts.Count > 0)
        {
            Product item = _stackedProducts.Pop();
            return item;
        }

        return null;

    }
    
    public Product TakeItem()
    {
        Product index = PopFromStack(); // Use Pop instead of Dequeue
        
        return index;
    }
    
    public Vector3 CalculateProductPosition()
    {
        return new Vector3(0, 0, _stackedProducts.Count);
    }

    public void MoveProductToPosition(Product product, Action onMoveComplete)
    {
        Vector3 targetPosition = CalculateProductPosition();
        product.Jump(transform, targetPosition, onMoveComplete);
    }
    
    public bool IsReadyForPush()
    {
        return _stackedProducts.Count < capacity;
    }    
    public bool IsReadyForPop()
    {
        return _stackedProducts.Count > 0;
    }
    
    
    
    
    
    
}
