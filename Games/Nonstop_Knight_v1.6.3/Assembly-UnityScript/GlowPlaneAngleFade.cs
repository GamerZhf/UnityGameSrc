using System;
using UnityEngine;

[Serializable]
public class GlowPlaneAngleFade : MonoBehaviour
{
    public Transform cameraTransform;
    private float dot = 0.5f;
    public Color glowColor = Color.grey;

    public override void Main()
    {
    }

    public override void OnWillRenderObject()
    {
        this.GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", (Color) (this.glowColor * this.dot));
    }

    public override void Start()
    {
        if (this.cameraTransform == null)
        {
            this.cameraTransform = Camera.main.transform;
        }
    }

    public override void Update()
    {
        this.dot = 1.5f * Mathf.Clamp01(Vector3.Dot(this.cameraTransform.forward, -this.transform.up) - 0.25f);
    }
}

