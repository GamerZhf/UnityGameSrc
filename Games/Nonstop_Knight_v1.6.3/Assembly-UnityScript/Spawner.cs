using Boo.Lang.Runtime;
using System;
using System.Collections;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Spawner : MonoBehaviour
{
    public Hashtable activeCachedObjects;
    public ObjectCache[] caches;
    [NonSerialized]
    public static Spawner spawner;

    public override void Awake()
    {
        spawner = this;
        int capacity = 0;
        for (int i = 0; i < Extensions.get_length(this.caches); i++)
        {
            this.caches[i].Initialize();
            capacity += this.caches[i].cacheSize;
        }
        this.activeCachedObjects = new Hashtable(capacity);
    }

    public static void Destroy(GameObject objectToDestroy)
    {
        if ((spawner != null) && spawner.activeCachedObjects.ContainsKey(objectToDestroy))
        {
            objectToDestroy.SetActive(false);
            spawner.activeCachedObjects[objectToDestroy] = false;
        }
        else
        {
            UnityEngine.Object.Destroy(objectToDestroy);
        }
    }

    public override void Main()
    {
    }

    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        ObjectCache lhs = null;
        GameObject nextObjectInCache;
        if (spawner != null)
        {
            for (int i = 0; i < Extensions.get_length(spawner.caches); i++)
            {
                if (spawner.caches[i].prefab == prefab)
                {
                    lhs = spawner.caches[i];
                }
            }
            nextObjectInCache = lhs.GetNextObjectInCache();
            nextObjectInCache.transform.position = position;
            nextObjectInCache.transform.rotation = rotation;
            nextObjectInCache.SetActive(true);
        }
        return (!RuntimeServices.EqualityOperator(lhs, null) ? nextObjectInCache : (((GameObject) UnityEngine.Object.Instantiate(prefab, position, rotation)) as GameObject));
    }
}

