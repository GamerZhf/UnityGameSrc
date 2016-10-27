using System;

public interface IObjectPool<T>
{
    void destroy();
    T getObject();
    void prewarm();
    void returnObject(T obj);

    ObjectPoolExpansionMethod ExpansionMethod { get; set; }

    int InitialCapacity { get; }

    IInstanceProvider<T> InstanceProvider { get; set; }
}

