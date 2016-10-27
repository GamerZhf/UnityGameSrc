using System;
using UnityEngine;

[Serializable]
public class SimpleBullet : MonoBehaviour
{
    public float dist = 0x2710;
    public float lifeTime = 0.5f;
    private float spawnTime;
    public float speed = 10;
    private Transform tr;

    public override void Main()
    {
    }

    public override void OnEnable()
    {
        this.tr = this.transform;
        this.spawnTime = Time.time;
    }

    public override void Update()
    {
        this.tr.position += (Vector3) ((this.tr.forward * this.speed) * Time.deltaTime);
        this.dist -= this.speed * Time.deltaTime;
        if ((Time.time > (this.spawnTime + this.lifeTime)) || (this.dist < 0))
        {
            Spawner.Destroy(this.gameObject);
        }
    }
}

