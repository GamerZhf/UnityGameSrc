using System;
using UnityEngine;

[Serializable, RequireComponent(typeof(PerFrameRaycast))]
public class AutoFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float coneAngle = 1.5f;
    public float damagePerSecond = 20f;
    public bool firing;
    public float forcePerSecond = 20f;
    public float frequency = 10;
    public float hitSoundVolume = 0.5f;
    private float lastFireTime = -1;
    public GameObject muzzleFlashFront;
    private PerFrameRaycast raycast;
    public Transform spawnPoint;

    public override void Awake()
    {
        this.muzzleFlashFront.SetActive(false);
        this.raycast = this.GetComponent<PerFrameRaycast>();
        if (this.spawnPoint == null)
        {
            this.spawnPoint = this.transform;
        }
    }

    public override void Main()
    {
    }

    public override void OnStartFire()
    {
        if (Time.timeScale != 0)
        {
            this.firing = true;
            this.muzzleFlashFront.SetActive(true);
            if (this.GetComponent<AudioSource>() != null)
            {
                this.GetComponent<AudioSource>().Play();
            }
        }
    }

    public override void OnStopFire()
    {
        this.firing = false;
        this.muzzleFlashFront.SetActive(false);
        if (this.GetComponent<AudioSource>() != null)
        {
            this.GetComponent<AudioSource>().Stop();
        }
    }

    public override void Update()
    {
        if (this.firing && (Time.time > (this.lastFireTime + (((float) 1) / this.frequency))))
        {
            Quaternion quaternion = Quaternion.Euler(UnityEngine.Random.Range(-this.coneAngle, this.coneAngle), UnityEngine.Random.Range(-this.coneAngle, this.coneAngle), (float) 0);
            SimpleBullet component = (Spawner.Spawn(this.bulletPrefab, this.spawnPoint.position, this.spawnPoint.rotation * quaternion) as GameObject).GetComponent<SimpleBullet>();
            this.lastFireTime = Time.time;
            RaycastHit hitInfo = this.raycast.GetHitInfo();
            if (hitInfo.transform != null)
            {
                Health health = hitInfo.transform.GetComponent<Health>();
                if (health != null)
                {
                    health.OnDamage(this.damagePerSecond / this.frequency, -this.spawnPoint.forward);
                }
                if (hitInfo.rigidbody != null)
                {
                    Vector3 force = (Vector3) (this.transform.forward * (this.forcePerSecond / this.frequency));
                    hitInfo.rigidbody.AddForceAtPosition(force, hitInfo.point, ForceMode.Impulse);
                }
                component.dist = hitInfo.distance;
            }
            else
            {
                component.dist = 0x3e8;
            }
        }
    }
}

