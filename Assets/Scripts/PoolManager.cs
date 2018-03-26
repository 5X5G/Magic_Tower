using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager :Singleton<PoolManager>{

    public bool logStatus;
    public Transform root;

    private Dictionary<GameObject, ObjectPool<GameObject>> prefabLookup;
    private Dictionary<GameObject, ObjectPool<GameObject>> instanceLookup;

    private bool dirty =false;

    private void Awake()
    {
        prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
        instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
    }

    public void WarmPool(GameObject prefab, int size)
    {
        if (prefabLookup.ContainsKey(prefab))
        {
            throw new Exception("Pool for prefab " + prefab.name + " has already been created");
        }
        var pool = new ObjectPool<GameObject>(() => { return InstantiatePrefab(prefab); }, size);
        prefabLookup[prefab] = pool;

        dirty = true;
    }

    private GameObject InstantiatePrefab(GameObject prefab)
    {
        var go = Instantiate(prefab) as GameObject;        
        if (root != null) go.transform.parent = root;
        go.transform.localScale = Vector3.one;
        return go;
    }

    #region Static API

    public static void Warm(GameObject prefab, int size)
    {
        Instance.WarmPool(prefab, size);
    }

    #endregion
}
