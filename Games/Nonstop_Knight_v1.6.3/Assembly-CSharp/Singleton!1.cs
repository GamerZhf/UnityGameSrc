using System;

public class Singleton<T> where T: class, new()
{
    protected static T m_instance;

    public static T Instance
    {
        get
        {
            if (Singleton<T>.m_instance == null)
            {
                Singleton<T>.m_instance = Activator.CreateInstance<T>();
            }
            return Singleton<T>.m_instance;
        }
    }
}

