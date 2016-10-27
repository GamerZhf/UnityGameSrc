using System;
using UnityEngine;

[Serializable]
public class SpiderAttackMoveController : MonoBehaviour
{
    private AI ai;
    public MonoBehaviour animationBehaviour;
    public AudioSource audioSource;
    public SelfIlluminationBlink[] blinkComponents;
    public GlowPlane blinkPlane;
    private Transform character;
    public float damageAmount = 30f;
    public float damageRadius = 5f;
    private bool inRange;
    public GameObject intentionalExplosion;
    private float lastBlinkTime;
    private float lastRaycastSuccessfulTime;
    public MovementMotor motor;
    private float nextRaycastTime;
    private float noticeTime;
    private Transform player;
    public float proximityBuildupTime = 2f;
    public float proximityDistance = 4f;
    private float proximityLevel;
    public float proximityOfNoReturn = 0.6f;
    public Renderer proximityRenderer;
    public float targetDistanceMax = 3f;
    public float targetDistanceMin = 2f;

    public override void Awake()
    {
        this.character = this.motor.transform;
        this.player = GameObject.FindWithTag("Player").transform;
        this.ai = this.transform.parent.GetComponentInChildren<AI>();
        if (this.blinkComponents.Length == 0)
        {
            this.blinkComponents = this.transform.parent.GetComponentsInChildren<SelfIlluminationBlink>();
        }
    }

    public override void Explode()
    {
        float num = 1 - (Vector3.Distance(this.player.position, this.character.position) / this.damageRadius);
        Health component = this.player.GetComponent<Health>();
        if (component != null)
        {
            component.OnDamage(this.damageAmount * num, this.character.position - this.player.position);
        }
        this.player.GetComponent<Rigidbody>().AddExplosionForce((float) 10, this.character.position, this.damageRadius, (float) 0, ForceMode.Impulse);
        Spawner.Spawn(this.intentionalExplosion, this.transform.position, Quaternion.identity);
        Spawner.Destroy(this.character.gameObject);
    }

    public override void Main()
    {
    }

    public override void OnDisable()
    {
        if (this.proximityRenderer == null)
        {
            Debug.LogError("proximityRenderer is null", this);
        }
        else if (this.proximityRenderer.material == null)
        {
            Debug.LogError("proximityRenderer.material is null", this);
        }
        else
        {
            this.proximityRenderer.material.color = Color.white;
        }
        if (this.blinkPlane != null)
        {
            this.blinkPlane.GetComponent<Renderer>().enabled = false;
        }
    }

    public override void OnEnable()
    {
        this.inRange = false;
        this.nextRaycastTime = Time.time;
        this.lastRaycastSuccessfulTime = Time.time;
        this.noticeTime = Time.time;
        this.animationBehaviour.enabled = true;
        if (this.blinkPlane != null)
        {
            this.blinkPlane.GetComponent<Renderer>().enabled = false;
        }
    }

    public override void Update()
    {
        if (Time.time < (this.noticeTime + 0.7f))
        {
            this.motor.movementDirection = Vector3.zero;
        }
        else
        {
            Vector3 vector = this.player.position - this.character.position;
            vector.y = 0;
            float magnitude = vector.magnitude;
            vector = (Vector3) (vector / magnitude);
            if (this.inRange && (magnitude > this.targetDistanceMax))
            {
                this.inRange = false;
            }
            if (!this.inRange && (magnitude < this.targetDistanceMin))
            {
                this.inRange = true;
            }
            if (this.inRange)
            {
                this.motor.movementDirection = Vector3.zero;
            }
            else
            {
                this.motor.movementDirection = vector;
            }
            if (((magnitude < this.proximityDistance) && (Time.time < (this.lastRaycastSuccessfulTime + 1))) || (this.proximityLevel > this.proximityOfNoReturn))
            {
                this.proximityLevel += Time.deltaTime / this.proximityBuildupTime;
            }
            else
            {
                this.proximityLevel -= Time.deltaTime / this.proximityBuildupTime;
            }
            this.proximityLevel = Mathf.Clamp01(this.proximityLevel);
            if (this.proximityLevel == 1)
            {
                this.Explode();
            }
            if (Time.time > this.nextRaycastTime)
            {
                this.nextRaycastTime = Time.time + 1;
                if (this.ai.CanSeePlayer())
                {
                    this.lastRaycastSuccessfulTime = Time.time;
                }
                else if (Time.time > (this.lastRaycastSuccessfulTime + 2))
                {
                    this.ai.OnLostTrack();
                }
            }
            float num2 = ((float) 1) / Mathf.Lerp((float) 2, (float) 15, this.proximityLevel);
            if (Time.time > (this.lastBlinkTime + num2))
            {
                this.lastBlinkTime = Time.time;
                this.proximityRenderer.material.color = Color.red;
                if (this.audioSource.enabled)
                {
                    this.audioSource.Play();
                }
                int index = 0;
                SelfIlluminationBlink[] blinkComponents = this.blinkComponents;
                int length = blinkComponents.Length;
                while (index < length)
                {
                    blinkComponents[index].Blink();
                    index++;
                }
                if (this.blinkPlane != null)
                {
                    this.blinkPlane.GetComponent<Renderer>().enabled = !this.blinkPlane.GetComponent<Renderer>().enabled;
                }
            }
            if (Time.time > (this.lastBlinkTime + 0.04f))
            {
                this.proximityRenderer.material.color = Color.white;
            }
        }
    }
}

