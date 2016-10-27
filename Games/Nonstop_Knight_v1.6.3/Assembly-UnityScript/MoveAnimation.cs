using System;
using UnityEngine;

[Serializable]
public class MoveAnimation
{
    [HideInInspector]
    public float angle;
    public AnimationClip clip;
    [HideInInspector]
    public bool currentBest;
    [HideInInspector]
    public float speed;
    public Vector3 velocity;
    [HideInInspector]
    public float weight;

    public override void Init()
    {
        this.velocity.y = 0;
        this.speed = this.velocity.magnitude;
        this.angle = PlayerAnimation.HorizontalAngle(this.velocity);
    }
}

