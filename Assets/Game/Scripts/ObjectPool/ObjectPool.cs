using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using Sirenix.OdinInspector;
using Object = UnityEngine.Object;

public partial class ObjectPool : Singleton<ObjectPool>
{
    public List<IObjectPoolStruct> ObjectPools = new List<IObjectPoolStruct>(); 

    #region Initilaze

    private void Init()
    {
        FindInterfacesAndSet();
        // DontDestroyOnLoad(this);
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Init();
    }
    #endregion

    public T RetrieveObjectByTag<T>(string tag) where T : Component
    {
        foreach (var pool in ObjectPools)
        {
            if (pool is ObjectPoolStruct<T> typedPool && typedPool.Tag == tag)
            {
                return typedPool.Get();
            }
        }

        return null;
    }

    

    private void FindInterfacesAndSet()
    {
        FieldInfo[] fields = GetType().GetFields();

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType.GetInterfaces().Contains(typeof(IObjectPoolStruct)))
            {
                IObjectPoolStruct objectPoolStruct = (IObjectPoolStruct)field.GetValue(this);
                objectPoolStruct.Spawn();
                ObjectPools.Add(objectPoolStruct);
            }
        }
    }

}

[System.Serializable]
public class ObjectPoolStruct<T> : IObjectPoolStruct where T : UnityEngine.Component
{
    public string Tag;
    public T referenceComponent;
    public int poolSize;
    public Transform container;

    public HashSet<T> generic = new HashSet<T>();

    public void Spawn()
    {
        for (int i = 0; i < poolSize; i++)
        {
            T s = GameObject.Instantiate(referenceComponent) as T;
            AfterSpawn(s);
        }
    }

    public void AfterSpawn(T obj)
    {
        container = container == null ? ObjectPool.Instance.transform : container;

        obj.transform.parent = container;
        obj.gameObject.SetActive(false);
        generic.Add(obj);
    }


    public T Get()
    {
        foreach (T pooledObject in generic)
        {
            if (!pooledObject.gameObject.activeSelf)
            {
                pooledObject.gameObject.SetActive(true);
                return pooledObject;
            }
        }

        T o = Object.Instantiate(referenceComponent) as T;
        AfterSpawn(o);
        o.gameObject.SetActive(true);

        return o;
    }


    public void Relase(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.parent = container;
    }
}