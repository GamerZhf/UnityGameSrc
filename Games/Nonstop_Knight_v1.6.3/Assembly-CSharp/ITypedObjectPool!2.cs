using System;

public interface ITypedObjectPool<T, V>
{
    void destroy();
    T getObject(V type);
    void preWarm(V type);
    void returnObject(T obj, V type);

    ObjectPoolExpansionMethod ExpansionMethod { get; set; }

    ITypedInstanceProvider<T, V> InstanceProvider { get; set; }
}

