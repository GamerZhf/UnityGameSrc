using System;
using UnityEngine;

[Serializable]
public class SpawnAtCheckpoint : MonoBehaviour
{
    public Transform checkpoint;

    public override void Main()
    {
    }

    public override void OnSignal()
    {
        this.transform.position = this.checkpoint.position;
        this.transform.rotation = this.checkpoint.rotation;
        ResetHealthOnAll();
    }

    public static void ResetHealthOnAll()
    {
        Health[] healthArray = (Health[]) UnityEngine.Object.FindObjectsOfType(typeof(Health));
        int index = 0;
        Health[] healthArray2 = healthArray;
        int length = healthArray2.Length;
        while (index < length)
        {
            healthArray2[index].dead = false;
            healthArray2[index].health = healthArray2[index].maxHealth;
            index++;
        }
    }
}

