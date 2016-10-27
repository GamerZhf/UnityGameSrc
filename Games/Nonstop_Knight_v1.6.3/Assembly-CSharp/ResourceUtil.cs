using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class ResourceUtil
{
    public static bool Exists(string path)
    {
        return (Resources.Load(path) != null);
    }

    public static bool Exists(string path, System.Type type)
    {
        UnityEngine.Object obj2 = Resources.Load(path);
        return ((obj2 != null) && obj2.GetType().Equals(type));
    }

    public static UnityEngine.Object Instantiate(string path)
    {
        UnityEngine.Object original = Resources.Load(path);
        if (original == null)
        {
            Debug.LogError("Cannot instantiate resource at path: '" + path + "'.");
            return null;
        }
        string name = original.name;
        UnityEngine.Object obj3 = UnityEngine.Object.Instantiate(original);
        obj3.name = name;
        return obj3;
    }

    public static T Instantiate<T>(string path) where T: UnityEngine.Object
    {
        return (T) Instantiate(path);
    }

    public static UnityEngine.Object Instantiate(UnityEngine.Object res)
    {
        UnityEngine.Object obj2 = UnityEngine.Object.Instantiate(res);
        obj2.name = res.name;
        return obj2;
    }

    public static T Instantiate<T>(UnityEngine.Object res) where T: UnityEngine.Object
    {
        T local = (T) UnityEngine.Object.Instantiate(res);
        local.name = res.name;
        return local;
    }

    public static T Instantiate<T>(T res) where T: UnityEngine.Object
    {
        T local = UnityEngine.Object.Instantiate<T>(res);
        local.name = res.name;
        return local;
    }

    public static GameObject InstantiateGameObject(string path, Vector3 position, Quaternion rotation)
    {
        GameObject original = (GameObject) Resources.Load(path);
        if (original == null)
        {
            Debug.LogError("Cannot instantiate GameObject at path: '" + path + "'.");
            return null;
        }
        string name = original.name;
        GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(original, position, rotation);
        obj3.name = name;
        return obj3;
    }

    public static UnityEngine.Object[] LoadResourcesAtPath(string path)
    {
        return Resources.LoadAll(path);
    }

    public static T[] LoadResourcesAtPath<T>(string path) where T: UnityEngine.Object
    {
        return (T[]) Resources.LoadAll(path, typeof(T));
    }

    public static UnityEngine.Object LoadSafe(string path, [Optional, DefaultParameterValue(false)] bool doNotLogError)
    {
        UnityEngine.Object obj2 = Resources.Load(path);
        if ((obj2 is ScriptableObject) && Application.isPlaying)
        {
            Debug.LogError("Trying to load ScriptableObject at path '" + path + "'!\nThis cannot be done during runtime. Use ResourceUtil.Instantiate() instead.");
            return null;
        }
        if (obj2 != null)
        {
            return obj2;
        }
        if (Application.isPlaying)
        {
            if (doNotLogError)
            {
                Debug.LogWarning("Cannot load resource at path: '" + path + "'.");
            }
            else
            {
                Debug.LogError("Cannot load resource at path: '" + path + "'.");
            }
        }
        return null;
    }

    public static T LoadSafe<T>(string path, [Optional, DefaultParameterValue(false)] bool doNotLogError) where T: UnityEngine.Object
    {
        T local = (T) Resources.Load(path, typeof(T));
        if ((local is ScriptableObject) && Application.isPlaying)
        {
            Debug.LogError("Trying to load ScriptableObject at path '" + path + "'!\nThis cannot be done during runtime. Use ResourceUtil.Instantiate() instead.");
            return null;
        }
        if (local != null)
        {
            return local;
        }
        if (Application.isPlaying)
        {
            if (doNotLogError)
            {
                Debug.LogWarning("Cannot load resource at path: '" + path + "'.");
            }
            else
            {
                Debug.LogError("Cannot load resource at path: '" + path + "'.");
            }
        }
        return null;
    }

    public static UnityEngine.Object LoadUnsafe(string path)
    {
        return Resources.Load(path);
    }

    public static T LoadUnsafe<T>(string path) where T: UnityEngine.Object
    {
        return (T) Resources.Load(path, typeof(T));
    }
}

