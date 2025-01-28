using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Product", menuName = "ResourceProducts/Product", order = 1)]
public class ProductSO : ScriptableObject
{
    public string productID;
    public Image image;
    public Product prefab;
    
}
