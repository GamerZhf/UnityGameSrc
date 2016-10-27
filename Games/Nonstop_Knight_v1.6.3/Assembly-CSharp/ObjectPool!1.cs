using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectPool<T> : IGenericObjectPool, IObjectPool<T>
{
    [CompilerGenerated]
    private ObjectPoolExpansionMethod <ExpansionMethod>k__BackingField;
    [CompilerGenerated]
    private int <InitialCapacity>k__BackingField;
    [CompilerGenerated]
    private IInstanceProvider<T> <InstanceProvider>k__BackingField;
    [CompilerGenerated]
    private bool <Prewarmed>k__BackingField;
    private List<T> m_all;
    private Stack<T> m_free;

    public ObjectPool()
    {
        this.m_all = new List<T>();
        this.m_free = new Stack<T>();
    }

    public ObjectPool(IInstanceProvider<T> instanceProvider, int initialCapacity, ObjectPoolExpansionMethod expansionMethod, bool preWarm)
    {
        this.m_all = new List<T>();
        this.m_free = new Stack<T>();
        this.InstanceProvider = instanceProvider;
        this.InitialCapacity = initialCapacity;
        this.ExpansionMethod = expansionMethod;
        if (preWarm)
        {
            this.prewarm();
        }
    }

    public void destroy()
    {
        if (this.m_all.Count != this.m_free.Count)
        {
            Debug.LogWarning(string.Concat(new object[] { "Destroying ObjectPool without returning all objects first: ", this, " - ", this.InstanceProvider }));
        }
        for (int i = 0; i < this.m_all.Count; i++)
        {
            this.InstanceProvider.onDestroy(this.m_all[i]);
        }
        this.InstanceProvider.onReset();
        this.m_all.Clear();
        this.m_free.Clear();
    }

    public void forceReturnAll()
    {
        for (int i = 0; i < this.m_all.Count; i++)
        {
            T t = this.m_all[i];
            if (!this.m_free.Contains(t))
            {
                this.returnObject(t);
            }
        }
    }

    public T getObject()
    {
        if (this.m_free.Count == 0)
        {
            int initialCapacity = 0;
            if (this.m_all.Count == 0)
            {
                if (Application.isEditor && this.Prewarmed)
                {
                    Debug.LogWarning(string.Concat(new object[] { "Expanding object pool: '", this, "' with instance provider: '", this.InstanceProvider, "'" }));
                }
                initialCapacity = this.InitialCapacity;
            }
            else
            {
                if (Application.isEditor)
                {
                    Debug.LogWarning(string.Concat(new object[] { "Expanding object pool: '", this, "' with instance provider: '", this.InstanceProvider, "'" }));
                }
                if (this.ExpansionMethod == ObjectPoolExpansionMethod.DOUBLE)
                {
                    initialCapacity = this.m_all.Count * 2;
                }
                else if (this.ExpansionMethod == ObjectPoolExpansionMethod.SINGLE)
                {
                    initialCapacity++;
                }
            }
            for (int i = 0; i < initialCapacity; i++)
            {
                T item = this.InstanceProvider.instantiate();
                this.m_all.Add(item);
                this.m_free.Push(item);
            }
        }
        return this.m_free.Pop();
    }

    public void prewarm()
    {
        T local = this.getObject();
        this.returnObject(local);
        this.Prewarmed = true;
    }

    public void returnObject(T obj)
    {
        if (!this.m_all.Contains(obj))
        {
            Debug.LogError(string.Concat(new object[] { "Trying to return object to pool without existing in m_all container: ", this, " - ", this.InstanceProvider }));
        }
        else if (!this.m_free.Contains(obj))
        {
            this.InstanceProvider.onReturn(obj);
            if (obj is IPoolable)
            {
                ((IPoolable) obj).cleanUpForReuse();
            }
            this.m_free.Push(obj);
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

    public int InitialCapacity
    {
        [CompilerGenerated]
        get
        {
            return this.<InitialCapacity>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<InitialCapacity>k__BackingField = value;
        }
    }

    public IInstanceProvider<T> InstanceProvider
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

    public bool Prewarmed
    {
        [CompilerGenerated]
        get
        {
            return this.<Prewarmed>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<Prewarmed>k__BackingField = value;
        }
    }
}

