using System;
using UnityEngine;

[Serializable]
public class PerFrameRaycast : MonoBehaviour
{
    private RaycastHit hitInfo;
    private Transform tr;

    public override void Awake()
    {
        this.tr = this.transform;
    }

    public override RaycastHit GetHitInfo()
    {
        return this.hitInfo;
    }

    public override void Main()
    {
    }

    public override void Update()
    {
        this.hitInfo = new RaycastHit();
        Physics.Raycast(this.tr.position, this.tr.forward, out this.hitInfo);
    }
}

