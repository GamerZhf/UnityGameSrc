namespace UnityTest
{
    using System;

    [Flags]
    public enum CheckMethod
    {
        AfterPeriodOfTime = 1,
        FixedUpdate = 8,
        LateUpdate = 0x10,
        OnBecameInvisible = 0x800,
        OnBecameVisible = 0x1000,
        OnCollisionEnter = 0x10000,
        OnCollisionEnter2D = 0x400000,
        OnCollisionExit = 0x20000,
        OnCollisionExit2D = 0x800000,
        OnCollisionStay = 0x40000,
        OnCollisionStay2D = 0x1000000,
        OnControllerColliderHit = 0x100,
        OnDestroy = 0x20,
        OnDisable = 0x80,
        OnEnable = 0x40,
        OnJointBreak = 0x400,
        OnParticleCollision = 0x200,
        OnTriggerEnter = 0x2000,
        OnTriggerEnter2D = 0x80000,
        OnTriggerExit = 0x4000,
        OnTriggerExit2D = 0x100000,
        OnTriggerStay = 0x8000,
        OnTriggerStay2D = 0x200000,
        Start = 2,
        Update = 4
    }
}

