using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProductResource : MonoBehaviour
{
    public ResourceData ResourceData; 
    private Stack<int> _availableIndices;  // Changed from Queue to Stack
    private List<Product> _spawnedObjects; 
    private Dictionary<int, Coroutine> _respawnCoroutines;
    private ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = ObjectPool.Instance;
    }

    public void InitializeResource()
    {
        _availableIndices = new Stack<int>();  // Initialize as a Stack
        _spawnedObjects = new List<Product>();
        _respawnCoroutines = new Dictionary<int, Coroutine>();

        for (int i = 0; i < ResourceData.capacity; i++)
        {
            SpawnObject(i);
            _availableIndices.Push(i);  // Use Push instead of Enqueue
        }
    }

    private void SpawnObject(int index)
    {
        if (ResourceData.productPrefab == null)
        {
            Debug.LogError("Prefab is not assigned in StandSO!");
            return;
        }
        // Obje oluştur ve listeye ekle
        Product product = _objectPool.productPool.Get();
        product.transform.position = SetProductSpawnPoint(index);

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

    [Button]
    public Product TakeItem()
    {
        if (_availableIndices.Count == 0)
        {
            Debug.Log("No items available to take!");
            return null;
        }

        int index = _availableIndices.Pop();  // Use Pop instead of Dequeue

        if (_spawnedObjects[index] != null)
        {
            Product product = _spawnedObjects[index]; // Store the item before it's "destroyed"
            _spawnedObjects[index] = null;

            // Respawn başlat
            Coroutine respawnCoroutine = StartCoroutine(RespawnItem(index));
            _respawnCoroutines[index] = respawnCoroutine;

            return product; // Return the item instead of destroying it
        }

        return null;
    }

    
    private IEnumerator RespawnItem(int index)
    {
        yield return new WaitForSeconds(ResourceData.respawnTime);

        SpawnObject(index);
        _availableIndices.Push(index);  // Use Push to add the index back to the stack

        if (_respawnCoroutines.ContainsKey(index))
        {
            _respawnCoroutines.Remove(index);
        }
    }
}
