using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TypedObjectPool<T, V> : IGenericObjectPool, ITypedObjectPool<T, V>
{
    [CompilerGenerated]
    private ObjectPoolExpansionMethod <ExpansionMethod>k__BackingField;
    [CompilerGenerated]
    private int <InitialCapacityGlobal>k__BackingField;
    [CompilerGenerated]
    private Dictionary<V, int> <InitialCapacityPerType>k__BackingField;
    [CompilerGenerated]
    private ITypedInstanceProvider<T, V> <InstanceProvider>k__BackingField;
    private Dictionary<V, List<T>> m_all;
    private Dictionary<V, Stack<T>> m_free;
    private Dictionary<V, bool> m_prewarmed;

    public TypedObjectPool()
    {
        this.m_all = new Dictionary<V, List<T>>();
        this.m_free = new Dictionary<V, Stack<T>>();
        this.m_prewarmed = new Dictionary<V, bool>();
    }

    public TypedObjectPool(ITypedInstanceProvider<T, V> instanceProvider, int initialCapacityGlobal, Dictionary<V, int> initialCapacityPerType, ObjectPoolExpansionMethod expansionMethod, bool preWarm)
    {
        this.m_all = new Dictionary<V, List<T>>();
        this.m_free = new Dictionary<V, Stack<T>>();
        this.m_prewarmed = new Dictionary<V, bool>();
        this.InstanceProvider = instanceProvider;
        this.InitialCapacityGlobal = initialCapacityGlobal;
        this.ExpansionMethod = expansionMethod;
        this.InitialCapacityPerType = initialCapacityPerType;
        if (preWarm && (this.InitialCapacityPerType != null))
        {
            foreach (KeyValuePair<V, int> pair in this.InitialCapacityPerType)
            {
                V key = pair.Key;
                this.preWarm(key);
            }
        }
    }

    public void destroy()
    {
        foreach (KeyValuePair<V, List<T>> pair in this.m_all)
        {
            V key = pair.Key;
            if (this.m_all[key].Count != this.m_free[key].Count)
            {
                Debug.LogWarning(string.Concat(new object[] { "Destroying TypedObjectPool without returning all objects first: ", this, " - ", this.InstanceProvider, " - ", key }));
            }
            for (int i = 0; i < pair.Value.Count; i++)
            {
                this.InstanceProvider.onDestroy(pair.Value[i]);
            }
        }
        this.InstanceProvider.reset();
        this.m_all.Clear();
        this.m_free.Clear();
        this.m_prewarmed.Clear();
    }

    public void forceReturnAll()
    {
        foreach (KeyValuePair<V, List<T>> pair in this.m_all)
        {
            V key = pair.Key;
            List<T> list = pair.Value;
            for (int i = 0; i < list.Count; i++)
            {
                T t = list[i];
                if (!this.m_free[key].Contains(t))
                {
                    this.returnObject(t, key);
                }
            }
        }
    }

    public T getObject(V type)
    {
        if (!this.m_all.ContainsKey(type))
        {
            int initialCapacityGlobal;
            if ((this.InitialCapacityPerType != null) && this.InitialCapacityPerType.ContainsKey(type))
            {
                initialCapacityGlobal = this.InitialCapacityPerType[type];
            }
            else
            {
                initialCapacityGlobal = this.InitialCapacityGlobal;
            }
            this.m_all.Add(type, new List<T>(initialCapacityGlobal));
            this.m_free.Add(type, new Stack<T>(initialCapacityGlobal));
            this.m_prewarmed.Add(type, false);
        }
        if (this.m_free[type].Count == 0)
        {
            int num2 = 0;
            if (this.m_all[type].Count == 0)
            {
                if (Application.isEditor && this.m_prewarmed[type])
                {
                    Debug.LogWarning(string.Concat(new object[] { "Expanding object pool: '", this, "' with instance provider: '", this.InstanceProvider, "' and type '", type, "'" }));
                }
                if ((this.InitialCapacityPerType != null) && this.InitialCapacityPerType.ContainsKey(type))
                {
                    num2 = this.InitialCapacityPerType[type];
                }
                else
                {
                    num2 = this.InitialCapacityGlobal;
                }
            }
            else
            {
                if (Application.isEditor)
                {
                    Debug.LogWarning(string.Concat(new object[] { "Expanding object pool: '", this, "' with instance provider: '", this.InstanceProvider, "' and type '", type, "'" }));
                }
                if (this.ExpansionMethod == ObjectPoolExpansionMethod.DOUBLE)
                {
                    num2 = this.m_all[type].Count * 2;
                }
                else if (this.ExpansionMethod == ObjectPoolExpansionMethod.SINGLE)
                {
                    num2++;
                }
            }
            for (int i = 0; i < num2; i++)
            {
                T item = this.InstanceProvider.instantiate(type);
                this.m_all[type].Add(item);
                this.m_free[type].Push(item);
            }
        }
        return this.m_free[type].Pop();
    }

    public void preWarm(V type)
    {
        T local = this.getObject(type);
        this.returnObject(local, type);
        this.m_prewarmed[type] = true;
    }

    public void returnObject(T obj, V type)
    {
        if (!this.m_all[type].Contains(obj))
        {
            Debug.LogError(string.Concat(new object[] { "Trying to return object of type '", type, "' to pool without existing in m_all container: ", this, " - ", this.InstanceProvider }));
        }
        else if (!this.m_free[type].Contains(obj))
        {
            this.InstanceProvider.onReturn(obj);
            if (obj is IPoolable)
            {
                ((IPoolable) obj).cleanUpForReuse();
            }
            this.m_free[type].Push(obj);
        }
    }

    public ObjectPoolExpansionMethod ExpansionMethod
    {
        [CompilerGenerated]
        get
        {
            return this.<ExpansionMethod>k__BackingField;
        }
        [CompilerGenerated]
        set
        {
            this.<ExpansionMethod>k__BackingField = value;
        }
    }

    public int InitialCapacityGlobal
    {
        [CompilerGenerated]
        get
        {
            return this.<InitialCapacityGlobal>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<InitialCapacityGlobal>k__BackingField = value;
        }
    }

    public Dictionary<V, int> InitialCapacityPerType
    {
        [CompilerGenerated]
        get
        {
            return this.<InitialCapacityPerType>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<InitialCapacityPerType>k__BackingField = value;
        }
    }

    public ITypedInstanceProvider<T, V> InstanceProvider
    {
        [CompilerGenerated]
        get
        {
            return this.<InstanceProvider>k__BackingField;
        }
        [CompilerGenerated]
        set
        {
            this.<InstanceProvider>k__BackingField = value;
        }
    }
}

