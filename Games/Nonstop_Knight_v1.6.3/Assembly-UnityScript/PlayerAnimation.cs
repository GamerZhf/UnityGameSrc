using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class PlayerAnimation : MonoBehaviour
{
    private float angle;
    public Animation animationComponent;
    private MoveAnimation bestAnimation;
    public SignalSender footstepSignals;
    public AnimationClip idle;
    private float idleWeight;
    private float lastAnimTime;
    private float lastFootstepTime;
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 localVelocity = Vector3.zero;
    private float lowerBodyDeltaAngle;
    private Vector3 lowerBodyForward = Vector3.forward;
    private Vector3 lowerBodyForwardTarget = Vector3.forward;
    public float maxIdleSpeed = 0.5f;
    public float minWalkSpeed = 2f;
    public MoveAnimation[] moveAnimations;
    public Rigidbody rigid;
    public Transform rootBone;
    public AnimationClip shootAdditive;
    private float speed;
    private Transform tr;
    public AnimationClip turn;
    public Transform upperBodyBone;
    private Vector3 velocity = Vector3.zero;

    public override void Awake()
    {
        this.tr = this.rigid.transform;
        this.lastPosition = this.tr.position;
        int index = 0;
        MoveAnimation[] moveAnimations = this.moveAnimations;
        int length = moveAnimations.Length;
        while (index < length)
        {
            moveAnimations[index].Init();
            this.animationComponent[moveAnimations[index].clip.name].layer = 1;
            this.animationComponent[moveAnimations[index].clip.name].enabled = true;
            index++;
        }
        this.animationComponent.SyncLayer(1);
        this.animationComponent[this.idle.name].layer = 2;
        this.animationComponent[this.turn.name].layer = 3;
        this.animationComponent[this.idle.name].enabled = true;
        this.animationComponent[this.shootAdditive.name].layer = 4;
        this.animationComponent[this.shootAdditive.name].weight = 1;
        this.animationComponent[this.shootAdditive.name].speed = 0.6f;
        this.animationComponent[this.shootAdditive.name].blendMode = AnimationBlendMode.Additive;
    }

    public override void FixedUpdate()
    {
        this.velocity = (Vector3) ((this.tr.position - this.lastPosition) / Time.deltaTime);
        this.localVelocity = this.tr.InverseTransformDirection(this.velocity);
        this.localVelocity.y = 0;
        this.speed = this.localVelocity.magnitude;
        this.angle = HorizontalAngle(this.localVelocity);
        this.lastPosition = this.tr.position;
    }

    public static float HorizontalAngle(Vector3 direction)
    {
        return (Mathf.Atan2(direction.x, direction.z) * 57.29578f);
    }

    public override void LateUpdate()
    {
        if (Mathf.InverseLerp(this.minWalkSpeed, this.maxIdleSpeed, this.speed) < 1)
        {
            Vector3 zero = Vector3.zero;
            int index = 0;
            MoveAnimation[] moveAnimations = this.moveAnimations;
            int length = moveAnimations.Length;
            while (index < length)
            {
                if ((this.animationComponent[moveAnimations[index].clip.name].weight != 0) && (Vector3.Dot(moveAnimations[index].velocity, this.localVelocity) > 0))
                {
                    zero += (Vector3) (moveAnimations[index].velocity * this.animationComponent[moveAnimations[index].clip.name].weight);
                }
                index++;
            }
            float b = Mathf.DeltaAngle(HorizontalAngle((Vector3) (this.tr.rotation * zero)), HorizontalAngle(this.velocity));
            this.lowerBodyDeltaAngle = Mathf.LerpAngle(this.lowerBodyDeltaAngle, b, Time.deltaTime * 10);
            this.lowerBodyForwardTarget = this.tr.forward;
            this.lowerBodyForward = (Vector3) (Quaternion.Euler((float) 0, this.lowerBodyDeltaAngle, (float) 0) * this.lowerBodyForwardTarget);
        }
        else
        {
            this.lowerBodyForward = Vector3.RotateTowards(this.lowerBodyForward, this.lowerBodyForwardTarget, (Time.deltaTime * 520) * 0.01745329f, (float) 1);
            this.lowerBodyDeltaAngle = Mathf.DeltaAngle(HorizontalAngle(this.tr.forward), HorizontalAngle(this.lowerBodyForward));
            if (Mathf.Abs(this.lowerBodyDeltaAngle) > 80)
            {
                this.lowerBodyForwardTarget = this.tr.forward;
            }
        }
        Quaternion rotation = Quaternion.Euler((float) 0, this.lowerBodyDeltaAngle, (float) 0);
        this.rootBone.rotation = rotation * this.rootBone.rotation;
        this.upperBodyBone.rotation = Quaternion.Inverse(rotation) * this.upperBodyBone.rotation;
    }

    public override void Main()
    {
    }

    public override void OnStartFire()
    {
        if (Time.timeScale != 0)
        {
            this.animationComponent[this.shootAdditive.name].enabled = true;
        }
    }

    public override void OnStopFire()
    {
        this.animationComponent[this.shootAdditive.name].enabled = false;
    }

    public override void Update()
    {
        this.idleWeight = Mathf.Lerp(this.idleWeight, Mathf.InverseLerp(this.minWalkSpeed, this.maxIdleSpeed, this.speed), Time.deltaTime * 10);
        this.animationComponent[this.idle.name].weight = this.idleWeight;
        if (this.speed > 0)
        {
            float positiveInfinity = float.PositiveInfinity;
            int index = 0;
            MoveAnimation[] moveAnimations = this.moveAnimations;
            int length = moveAnimations.Length;
            while (index < length)
            {
                float num2 = Mathf.Abs(Mathf.DeltaAngle(this.angle, moveAnimations[index].angle));
                float num3 = Mathf.Abs((float) (this.speed - moveAnimations[index].speed));
                float num4 = num2 + num3;
                if (RuntimeServices.EqualityOperator(moveAnimations[index], this.bestAnimation))
                {
                    num4 *= 0.9f;
                }
                if (num4 < positiveInfinity)
                {
                    this.bestAnimation = moveAnimations[index];
                    positiveInfinity = num4;
                }
                index++;
            }
            this.animationComponent.CrossFade(this.bestAnimation.clip.name);
        }
        else
        {
            this.bestAnimation = null;
        }
        if ((this.lowerBodyForward != this.lowerBodyForwardTarget) && (this.idleWeight >= 0.9f))
        {
            this.animationComponent.CrossFade(this.turn.name, 0.05f);
        }
        if ((this.bestAnimation > null) && (this.idleWeight < 0.9f))
        {
            float num5 = Mathf.Repeat((this.animationComponent[this.bestAnimation.clip.name].normalizedTime * 2) + 0.1f, (float) 1);
            if ((num5 < this.lastAnimTime) && (Time.time > (this.lastFootstepTime + 0.1f)))
            {
                this.footstepSignals.SendSignals(this);
                this.lastFootstepTime = Time.time;
            }
            this.lastAnimTime = num5;
        }
    }
}

