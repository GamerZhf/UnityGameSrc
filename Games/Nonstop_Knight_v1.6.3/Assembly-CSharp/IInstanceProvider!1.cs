using System;

public interface IInstanceProvider<T>
{
    T instantiate();
    void onDestroy(T obj);
    void onReset();
    void onReturn(T obj);
}

