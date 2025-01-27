using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class ProductResourceManager : MonoBehaviour
{
    [Serializable]
    public struct Resource
    {
        public ProductResource resource;
        public ProductSO ProductSo;
        public int capacity;
        public float respawnTime;
    }

    [SerializeField] public List<Resource> resources;
    private ResourceData[] resourceDatas;

    void Start()
    {
        InitializeResource();
    }

    public void InitializeResource()
    {
        resourceDatas = new ResourceData[resources.Count];

        for (int i = 0; i < resources.Count; i++)
        {
            resourceDatas[i] = new ResourceData
            {
                productPrefab = resources[i].ProductSo.prefab,
                capacity = resources[i].capacity,
                respawnTime = resources[i].respawnTime
            };

            resources[i].resource.ResourceData = resourceDatas[i];

            resources[i].resource.InitializeResource();
        }
    }


}