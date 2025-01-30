using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProductResource : MonoBehaviour
{
    public float respawnTime;
    public int capacity;
    private Stack<int> _availableIndices;  // Changed from Queue to Stack
    private List<Product> _spawnedObjects; 
    private ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = ObjectPool.Instance;
    }

    private void Start()
    {
        InitializeResource();
    }

    public void InitializeResource()
    {
        _availableIndices = new Stack<int>(); 
        _spawnedObjects = new List<Product>();

        for (int i = 0; i < capacity; i++)
        {
            SpawnObject(i);
            _availableIndices.Push(i); 
        }
    }

    private void SpawnObject(int index)
    {
        // Obje oluştur ve listeye ekle
        Product product = _objectPool.productPool.Get();
        product.SetInitializeTransform(SetProductSpawnPoint(index), SetProductRotation());
        
        if (index < _spawnedObjects.Count)
        {
            _spawnedObjects[index] = product;
        }
        else
        {
            _spawnedObjects.Add(product);
        }
    }

    public Vector3 SetProductSpawnPoint(int index)
    {
        return transform.position + Vector3.right * (index+1);
    }

    public Quaternion SetProductRotation()
    {
        return new Quaternion(0, 0, 0,0);
    }

    [Button]
    public Product TakeItem()
    {
        if (_availableIndices.Count == 0)
        {
            Debug.Log("No items available to take!");
            return null;
        }

        int index = _availableIndices.Pop();  

        if (_spawnedObjects[index] != null)
        {
            Product product = _spawnedObjects[index]; 
            _spawnedObjects[index] = null;

            _ = RespawnItem(index);

            return product; 
        }

        return null;
    }

    
    private async Task RespawnItem(int index)
    {
        await Task.Delay((int)(respawnTime*1000)); // convert sec for design 

        SpawnObject(index);
        _availableIndices.Push(index);  
    }
}
