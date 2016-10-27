using System;
using UnityEngine;

[Serializable]
public class ObjectCache
{
    private int cacheIndex;
    public int cacheSize = 10;
    private GameObject[] objects;
    public GameObject prefab;

    public override GameObject GetNextObjectInCache()
    {
        GameObject context = null;
        for (int i = 0; i < this.cacheSize; i++)
        {
            context = this.objects[this.cacheIndex];
            if (!context.activeSelf)
            {
                break;
            }
            this.cacheIndex = (this.cacheIndex + 1) % this.cacheSize;
        }
        if (context.activeSelf)
        {
            Debug.LogWarning(((("Spawn of " + this.prefab.name) + " exceeds cache size of ") + this.cacheSize) + "! Reusing already active object.", context);
            Spawner.Destroy(context);
        }
        this.cacheIndex = (this.cacheIndex + 1) % this.cacheSize;
        return context;
    }

    public override void Initialize()
    {
        this.objects = new GameObject[this.cacheSize];
        for (int i = 0; i < this.cacheSize; i++)
        {
            this.objects[i] = UnityEngine.Object.Instantiate<GameObject>(this.prefab) as GameObject;
            this.objects[i].SetActive(false);
            this.objects[i].name += i;
        }
    }
}

