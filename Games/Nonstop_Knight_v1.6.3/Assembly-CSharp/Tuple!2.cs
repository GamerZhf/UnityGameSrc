using System;

public class Tuple<K, V>
{
    public K Key;
    public V Value;

    public Tuple()
    {
    }

    public Tuple(K key, V value)
    {
        this.Key = key;
        this.Value = value;
    }
}

