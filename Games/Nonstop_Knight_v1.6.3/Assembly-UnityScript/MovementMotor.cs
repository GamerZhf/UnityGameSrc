using System;
using UnityEngine;

[Serializable]
public class MovementMotor : MonoBehaviour
{
    [HideInInspector]
    public Vector3 facingDirection;
    [HideInInspector]
    public Vector3 movementDirection;
    [HideInInspector]
    public Vector3 movementTarget;

    public override void Main()
    {
    }
}

