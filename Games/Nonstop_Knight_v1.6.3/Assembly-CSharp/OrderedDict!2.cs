using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;
using System.Reflection;

public class OrderedDict<K, V>
{
    [JsonIgnore]
    public Dictionary<K, int> IndexDict;
    public List<K> Keys;
    public List<V> Values;

    public OrderedDict()
    {
        this.Keys = new List<K>();
        this.Values = new List<V>();
        this.IndexDict = new Dictionary<K, int>();
    }

    public OrderedDict(OrderedDict<K, V> another)
    {
        this.Keys = new List<K>();
        this.Values = new List<V>();
        this.IndexDict = new Dictionary<K, int>();
        this.Keys = new List<K>(another.Keys);
        this.Values = new List<V>(another.Values);
        this.IndexDict = new Dictionary<K, int>(another.IndexDict);
    }

    public void add(K key, V val)
    {
        this.Keys.Add(key);
        this.Values.Add(val);
        this.IndexDict.Add(key, this.Keys.Count - 1);
    }

    public void clear()
    {
        this.Keys.Clear();
        this.Values.Clear();
        this.IndexDict.Clear();
    }

    public bool contains(K key)
    {
        return this.Keys.Contains(key);
    }

    public V get(K key)
    {
        if (this.Keys.Count != this.IndexDict.Count)
        {
            this.refreshIndexDict();
        }
        return this.Values[this.IndexDict[key]];
    }

    public K keyAt(int idx)
    {
        return this.Keys[idx];
    }

    private void refreshIndexDict()
    {
        this.IndexDict.Clear();
        for (int i = 0; i < this.Keys.Count; i++)
        {
            this.IndexDict.Add(this.Keys[i], i);
        }
    }

    public void remove(K key)
    {
        int index = this.Keys.IndexOf(key);
        this.Keys.RemoveAt(index);
        this.Values.RemoveAt(index);
        this.IndexDict.Remove(key);
    }

    public void set(K key, V val)
    {
        if (this.IndexDict.ContainsKey(key))
        {
            this.Values[this.IndexDict[key]] = val;
        }
        else
        {
            this.add(key, val);
        }
    }

    public V valueAt(int idx)
    {
        return this.Values[idx];
    }

    public int Count
    {
        get
        {
            return this.Keys.Count;
        }
    }

    public V this[K key]
    {
        get
        {
            return this.get(key);
        }
    }
}

