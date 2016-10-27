using System;
using UnityEngine;

[Serializable]
public class SpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn;
    public SignalSender onDestroyedSignals;
    private GameObject spawned;

    public override void Main()
    {
        this.enabled = false;
    }

    public override void OnSignal()
    {
        this.spawned = Spawner.Spawn(this.objectToSpawn, this.transform.position, this.transform.rotation);
        if (this.onDestroyedSignals.receivers.Length > 0)
        {
            this.enabled = true;
        }
    }

    public override void Update()
    {
        if ((this.spawned == null) || !this.spawned.activeInHierarchy)
        {
            this.onDestroyedSignals.SendSignals(this);
            this.enabled = false;
        }
    }
}

