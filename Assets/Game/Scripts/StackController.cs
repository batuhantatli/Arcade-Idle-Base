using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    public int stackLimitCount;
    public Transform stackPoint;
    private Stack<Product> _stackedProducts = new Stack<Product>();

    private Coroutine _stackCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ProductResource productResource) && IsReadyForPush())
        {
            _stackCoroutine = StartCoroutine(CollectProduct(productResource));
        }

        if (other.TryGetComponent(out ProductStand stand))
        {
            if (stand.IsReadyForPush() && IsReadyForPop())
            {
                _stackCoroutine = StartCoroutine(DropProduct(stand));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.TryGetComponent(out ProductResource productResource)
            || other.TryGetComponent(out ProductStand stand))
            && _stackCoroutine != null)
        {
            Debug.Log("stopped");
            StopCoroutine(_stackCoroutine);
            _stackCoroutine = null;
        }
    }

    public Vector3 GetProductMovePoint()
    {
        Vector3 point = new Vector3(0, _stackedProducts.Count, 0);
        return point;
    }

    public IEnumerator CollectProduct(ProductResource productResource)
    {
        while (true)
        {
            var product = productResource.TakeItem();
            if (product != null)
            {
                product.Jump(stackPoint, GetProductMovePoint(), () =>
                {
                    PushProduct(product);
                });
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    public IEnumerator DropProduct(ProductStand stand)
    {
        while (true)
        {
            if (stand.IsReadyForPush() && IsReadyForPop())
            {
                stand.PushToStack(PopProduct());
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    public void PushProduct(Product product)
    {
        _stackedProducts.Push(product);
        if (!IsReadyForPush() && _stackCoroutine!= null )
        {
            StopCoroutine(_stackCoroutine);
            _stackCoroutine = null;
        }
    }

    public Product PopProduct()
    {
        if (_stackedProducts.Count > 0)
        {
            return _stackedProducts.Pop();
        }

        Debug.LogWarning("Stack is empty. Cannot remove any product.");
        return null;
    }

    public bool IsReadyForPush()
    {
        return _stackedProducts.Count < stackLimitCount;
    }    
    public bool IsReadyForPop()
    {
        return _stackedProducts.Count > 0;
    }    
}
