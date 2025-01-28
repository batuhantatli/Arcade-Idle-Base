using Sirenix.OdinInspector;
using UnityEngine;

public partial class ObjectPool : Singleton<ObjectPool>
{
    public ObjectPoolStruct<Customer> customerPool;
    public ObjectPoolStruct<Product> productPool;
    public ObjectPoolStruct<MoneyPiece> moneyPool;
}
