using System;
using UnityEngine;

[Serializable, RequireComponent(typeof(SphereCollider))]
public class DisableOutsideRadius : MonoBehaviour
{
    private float activeRadius;
    private SphereCollider sphereCollider;
    private GameObject target;

    public override void Awake()
    {
        this.target = this.transform.parent.gameObject;
        this.sphereCollider = this.GetComponent<SphereCollider>();
        this.activeRadius = this.sphereCollider.radius;
        this.Disable();
    }

    public override void Disable()
    {
        this.transform.parent = this.target.transform.parent;
        this.target.transform.parent = this.transform;
        this.target.SetActive(false);
        this.sphereCollider.radius = this.activeRadius;
    }

    public override void Enable()
    {
        this.target.transform.parent = this.transform.parent;
        this.target.SetActive(true);
        this.transform.parent = this.target.transform;
        this.sphereCollider.radius = this.activeRadius * 1.1f;
    }

    public override void Main()
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && (this.target.transform.parent == this.transform))
        {
            this.Enable();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.Disable();
        }
    }
}

