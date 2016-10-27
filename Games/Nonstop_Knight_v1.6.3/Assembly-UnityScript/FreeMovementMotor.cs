using System;
using UnityEngine;

[Serializable, RequireComponent(typeof(Rigidbody))]
public class FreeMovementMotor : MovementMotor
{
    public float turningSmoothing = 0.3f;
    public float walkingSnappyness = 50;
    public float walkingSpeed = 5f;

    public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
        dirA -= Vector3.Project(dirA, axis);
        dirB -= Vector3.Project(dirB, axis);
        return (Vector3.Angle(dirA, dirB) * ((Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) >= 0) ? ((float) 1) : ((float) (-1))));
    }

    public override void FixedUpdate()
    {
        Vector3 vector = (Vector3) (base.movementDirection * this.walkingSpeed);
        Vector3 vector2 = vector - this.GetComponent<Rigidbody>().velocity;
        if (this.GetComponent<Rigidbody>().useGravity)
        {
            vector2.y = 0;
        }
        this.GetComponent<Rigidbody>().AddForce((Vector3) (vector2 * this.walkingSnappyness), ForceMode.Acceleration);
        Vector3 facingDirection = base.facingDirection;
        if (facingDirection == Vector3.zero)
        {
            facingDirection = base.movementDirection;
        }
        if (facingDirection == Vector3.zero)
        {
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else
        {
            float num = AngleAroundAxis(this.transform.forward, facingDirection, Vector3.up);
            this.GetComponent<Rigidbody>().angularVelocity = (Vector3) ((Vector3.up * num) * this.turningSmoothing);
        }
    }

    public override void Main()
    {
    }
}

