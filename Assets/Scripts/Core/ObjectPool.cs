using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    private List<ObjectPoolContainer<T>> list;
    private Dictionary<T, ObjectPoolContainer<T>> lookup;
    private Func<T> factoryFunc;
    private int lastIndex = 0;

    public ObjectPool(Func<T> factoryFunc, int initialSize)
    {
        this.factoryFunc = factoryFunc;

        list = new List<ObjectPoolContainer<T>>(initialSize);
        lookup = new Dictionary<T, ObjectPoolContainer<T>>(initialSize);

        Warm(initialSize);
    }

    private void Warm(int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            CreateContainer();
        }
    }

    private ObjectPoolContainer<T> CreateContainer()
    {
        var container = new ObjectPoolContainer<T>();
        container.Item = factoryFunc();
        list.Add(container);
        return container;
    }   
}
