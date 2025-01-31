using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackController : MonoBehaviour
{
    public int stackLimitCount;
    public Transform stackPoint;
    private List<Product> _stackedProducts = new List<Product>();

    private Coroutine _stackCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ProductResource productResource) && IsReadyForAdd())
        {
            _stackCoroutine = StartCoroutine(CollectProduct(productResource));
        }

        if (other.TryGetComponent(out ProductStand stand))
        {
            if (stand.IsReadyForPush() && IsReadyForRemove(stand.type))
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
            if (IsReadyForAdd())
            {
                var product = productResource.RequestProduct();
                if (product != null)
                {
                    product.Jump(stackPoint, GetProductMovePoint(), () =>
                    {
                        AddProduct(product);
                    });
                }
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    public IEnumerator DropProduct(ProductStand stand)
    {
        while (true)
        {
            if (stand.IsReadyForPush() && IsReadyForRemove(stand.type))
            {
                stand.PushToStack(RemoveProduct(stand.type));
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    public void AddProduct(Product product)
    {
        _stackedProducts.Add(product);
        if (!IsReadyForAdd() && _stackCoroutine!= null )
        {
            StopCoroutine(_stackCoroutine);
            _stackCoroutine = null;
        }
    }

    public Product RemoveProduct(ProductType type)
    {
        if (IsReadyForRemove(type))
        {
            Product product = _stackedProducts.LastOrDefault(t => t.data.Type == type);
            _stackedProducts.Remove(product);
            return product;
        }

        Debug.LogWarning("Stack is empty. Cannot remove any product.");
        return null;
    }

    public bool IsReadyForAdd()
    {
        return _stackedProducts.Count < stackLimitCount;
    }    
    public bool IsReadyForRemove(ProductType type)
    {
        return _stackedProducts.Count(t =>t.data.Type == type) > 0;
    }    
}
