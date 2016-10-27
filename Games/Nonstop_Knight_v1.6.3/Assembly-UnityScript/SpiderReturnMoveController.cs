using System;
using UnityEngine;

[Serializable]
public class SpiderReturnMoveController : MonoBehaviour
{
    private AI ai;
    public MonoBehaviour animationBehaviour;
    private Transform character;
    public MovementMotor motor;
    private Vector3 spawnPos;

    public override void Awake()
    {
        this.character = this.motor.transform;
        this.ai = this.transform.parent.GetComponentInChildren<AI>();
        this.spawnPos = this.character.position;
    }

    public override void Main()
    {
    }

    public override void Update()
    {
        this.motor.movementDirection = this.spawnPos - this.character.position;
        this.motor.movementDirection.y = 0;
        if (this.motor.movementDirection.sqrMagnitude > 1)
        {
            this.motor.movementDirection = this.motor.movementDirection.normalized;
        }
        if (this.motor.movementDirection.sqrMagnitude < 0.01f)
        {
            this.character.position = new Vector3(this.spawnPos.x, this.character.position.y, this.spawnPos.z);
            this.motor.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.motor.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            this.motor.movementDirection = Vector3.zero;
            this.enabled = false;
            this.animationBehaviour.enabled = false;
        }
    }
}

