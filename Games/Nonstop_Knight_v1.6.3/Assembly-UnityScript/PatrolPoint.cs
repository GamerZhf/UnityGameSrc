using System;
using UnityEngine;

[Serializable]
public class PatrolPoint : MonoBehaviour
{
    public Vector3 position;

    public override void Awake()
    {
        this.position = this.transform.position;
    }

    public override void Main()
    {
    }
}

