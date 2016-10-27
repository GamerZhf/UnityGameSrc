using System;
using UnityEngine;

[Serializable]
public class SpiderAnimation : MonoBehaviour
{
    public AnimationClip activateAnim;
    public AudioSource audioSource;
    public AnimationClip backAnim;
    public SignalSender footstepSignals;
    public bool footstepSounds;
    public AnimationClip forwardAnim;
    private float lastAnimTime;
    private float lastFootstepTime;
    public AnimationClip leftAnim;
    public MovementMotor motor;
    public AnimationClip rightAnim;
    public bool skiddingSounds;
    private Transform tr;

    public static float HorizontalAngle(Vector3 direction)
    {
        return (Mathf.Atan2(direction.x, direction.z) * 57.29578f);
    }

    public override void Main()
    {
    }

    public override void OnDisable()
    {
        this.GetComponent<Animation>()[this.activateAnim.name].enabled = true;
        this.GetComponent<Animation>()[this.activateAnim.name].weight = 1;
        this.GetComponent<Animation>()[this.activateAnim.name].normalizedTime = 1;
        this.GetComponent<Animation>()[this.activateAnim.name].speed = -1;
        this.GetComponent<Animation>().CrossFade(this.activateAnim.name, 0.3f, PlayMode.StopAll);
    }

    public override void OnEnable()
    {
        this.tr = this.motor.transform;
        this.GetComponent<Animation>()[this.activateAnim.name].enabled = true;
        this.GetComponent<Animation>()[this.activateAnim.name].weight = 1;
        this.GetComponent<Animation>()[this.activateAnim.name].time = 0;
        this.GetComponent<Animation>()[this.activateAnim.name].speed = 1;
        this.GetComponent<Animation>()[this.forwardAnim.name].layer = 1;
        this.GetComponent<Animation>()[this.forwardAnim.name].enabled = true;
        this.GetComponent<Animation>()[this.forwardAnim.name].weight = 0;
        this.GetComponent<Animation>()[this.backAnim.name].layer = 1;
        this.GetComponent<Animation>()[this.backAnim.name].enabled = true;
        this.GetComponent<Animation>()[this.backAnim.name].weight = 0;
        this.GetComponent<Animation>()[this.leftAnim.name].layer = 1;
        this.GetComponent<Animation>()[this.leftAnim.name].enabled = true;
        this.GetComponent<Animation>()[this.leftAnim.name].weight = 0;
        this.GetComponent<Animation>()[this.rightAnim.name].layer = 1;
        this.GetComponent<Animation>()[this.rightAnim.name].enabled = true;
        this.GetComponent<Animation>()[this.rightAnim.name].weight = 0;
    }

    public override void Update()
    {
        Vector3 movementDirection = this.motor.movementDirection;
        movementDirection.y = 0;
        float magnitude = movementDirection.magnitude;
        this.GetComponent<Animation>()[this.forwardAnim.name].speed = magnitude;
        this.GetComponent<Animation>()[this.rightAnim.name].speed = magnitude;
        this.GetComponent<Animation>()[this.backAnim.name].speed = magnitude;
        this.GetComponent<Animation>()[this.leftAnim.name].speed = magnitude;
        float num2 = Mathf.DeltaAngle(HorizontalAngle(this.tr.forward), HorizontalAngle(movementDirection));
        if (magnitude > 0.01f)
        {
            float num3 = new float();
            if (num2 < -90)
            {
                num3 = Mathf.InverseLerp((float) (-180), (float) (-90), num2);
                this.GetComponent<Animation>()[this.forwardAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.rightAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.backAnim.name].weight = 1 - num3;
                this.GetComponent<Animation>()[this.leftAnim.name].weight = 1;
            }
            else if (num2 < 0)
            {
                num3 = Mathf.InverseLerp((float) (-90), (float) 0, num2);
                this.GetComponent<Animation>()[this.forwardAnim.name].weight = num3;
                this.GetComponent<Animation>()[this.rightAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.backAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.leftAnim.name].weight = 1 - num3;
            }
            else if (num2 < 90)
            {
                num3 = Mathf.InverseLerp((float) 0, (float) 90, num2);
                this.GetComponent<Animation>()[this.forwardAnim.name].weight = 1 - num3;
                this.GetComponent<Animation>()[this.rightAnim.name].weight = num3;
                this.GetComponent<Animation>()[this.backAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.leftAnim.name].weight = 0;
            }
            else
            {
                num3 = Mathf.InverseLerp((float) 90, (float) 180, num2);
                this.GetComponent<Animation>()[this.forwardAnim.name].weight = 0;
                this.GetComponent<Animation>()[this.rightAnim.name].weight = 1 - num3;
                this.GetComponent<Animation>()[this.backAnim.name].weight = num3;
                this.GetComponent<Animation>()[this.leftAnim.name].weight = 0;
            }
        }
        if (this.skiddingSounds)
        {
            if ((magnitude > 0.2f) && !this.audioSource.isPlaying)
            {
                this.audioSource.Play();
            }
            else if ((magnitude < 0.2f) && this.audioSource.isPlaying)
            {
                this.audioSource.Pause();
            }
        }
        if (this.footstepSounds && (magnitude > 0.2f))
        {
            float num4 = Mathf.Repeat((this.GetComponent<Animation>()[this.forwardAnim.name].normalizedTime * 4) + 0.1f, (float) 1);
            if ((num4 < this.lastAnimTime) && (Time.time > (this.lastFootstepTime + 0.1f)))
            {
                this.footstepSignals.SendSignals(this);
                this.lastFootstepTime = Time.time;
            }
            this.lastAnimTime = num4;
        }
    }
}

