using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T: MonoBehaviour
{
    protected static T sm_instance;

    public static T Instance
    {
        get
        {
            if (MonoBehaviourSingleton<T>.sm_instance == null)
            {
                MonoBehaviourSingleton<T>.sm_instance = (T) Object.FindObjectOfType(typeof(T));
            }
            return MonoBehaviourSingleton<T>.sm_instance;
        }
    }
}

