using System;
using UnityEngine;

[Serializable]
public class DestroyObject : MonoBehaviour
{
    public GameObject objectToDestroy;

    public override void Main()
    {
    }

    public override void OnSignal()
    {
        Spawner.Destroy(this.objectToDestroy);
    }
}

