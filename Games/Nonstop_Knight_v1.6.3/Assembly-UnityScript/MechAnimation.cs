using System;
using UnityEngine;

[Serializable]
public class MechAnimation : MonoBehaviour
{
    public SignalSender footstepSignals;
    public AnimationClip idle;
    private float lastAnimTime;
    private float lastFootstepTime;
    public Rigidbody rigid;
    private Transform tr;
    public AnimationClip turnLeft;
    public AnimationClip turnRight;
    public AnimationClip walk;

    public override void FixedUpdate()
    {
        float t = (Mathf.Abs(this.rigid.angularVelocity.y) * 57.29578f) / 100f;
        float num2 = this.rigid.velocity.magnitude / 2.5f;
        float num3 = Mathf.Sign(this.rigid.angularVelocity.y);
        this.GetComponent<Animation>()[this.walk.name].speed = Mathf.Lerp(1f, (this.GetComponent<Animation>()[this.walk.name].length / this.GetComponent<Animation>()[this.turnLeft.name].length) * 1.33f, t);
        this.GetComponent<Animation>()[this.turnLeft.name].time = this.GetComponent<Animation>()[this.walk.name].time;
        this.GetComponent<Animation>()[this.turnRight.name].time = this.GetComponent<Animation>()[this.walk.name].time;
        this.GetComponent<Animation>()[this.turnLeft.name].weight = Mathf.Clamp01(-t * num3);
        this.GetComponent<Animation>()[this.turnRight.name].weight = Mathf.Clamp01(t * num3);
        this.GetComponent<Animation>()[this.walk.name].weight = Mathf.Clamp01(num2);
        if ((num2 + t) > 0.1f)
        {
            float num4 = Mathf.Repeat((this.GetComponent<Animation>()[this.walk.name].normalizedTime * 2) + 0.1f, (float) 1);
            if ((num4 < this.lastAnimTime) && (Time.time > (this.lastFootstepTime + 0.1f)))
            {
                this.footstepSignals.SendSignals(this);
                this.lastFootstepTime = Time.time;
            }
            this.lastAnimTime = num4;
        }
    }

    public override void Main()
    {
    }

    public override void OnEnable()
    {
        this.tr = this.rigid.transform;
        this.GetComponent<Animation>()[this.idle.name].layer = 0;
        this.GetComponent<Animation>()[this.idle.name].weight = 1;
        this.GetComponent<Animation>()[this.idle.name].enabled = true;
        this.GetComponent<Animation>()[this.walk.name].layer = 1;
        this.GetComponent<Animation>()[this.turnLeft.name].layer = 1;
        this.GetComponent<Animation>()[this.turnRight.name].layer = 1;
        this.GetComponent<Animation>()[this.walk.name].weight = 1;
        this.GetComponent<Animation>()[this.turnLeft.name].weight = 0;
        this.GetComponent<Animation>()[this.turnRight.name].weight = 0;
        this.GetComponent<Animation>()[this.walk.name].enabled = true;
        this.GetComponent<Animation>()[this.turnLeft.name].enabled = true;
        this.GetComponent<Animation>()[this.turnRight.name].enabled = true;
    }
}

