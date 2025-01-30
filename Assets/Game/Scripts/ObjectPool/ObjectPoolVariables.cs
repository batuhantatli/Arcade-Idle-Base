using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public partial class ObjectPool : Singleton<ObjectPool>
{
    [System.Serializable]
    public class ProductPool
    {
        public ObjectPoolStruct<Product> productPool;
        public ProductType type;
        public int price;
    }
    
    [SerializeField] private List<ProductPool> productPools = new List<ProductPool>();
    public ObjectPoolStruct<Customer> customerPool;
    public ObjectPoolStruct<MoneyPiece> moneyPool;

    public ProductPool GetProductPool(ProductType type)
    {
        var g = productPools.FirstOrDefault(t => t.type == type);
        return g;
    }

}
