using System;

public interface ITypedInstanceProvider<T, V>
{
    T instantiate(V type);
    void onDestroy(T obj);
    void onReturn(T obj);
    void reset();
}

