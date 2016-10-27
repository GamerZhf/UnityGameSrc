using System;
using UnityEngine;

[Serializable]
public class GlowPlane : MonoBehaviour
{
    public Color glowColor = Color.white;
    private Material mat;
    public float maxGlow = 0.5f;
    public float minGlow = 0.2f;
    public Transform playerTransform;
    private Vector3 pos;
    private Vector3 scale;

    public override void Main()
    {
    }

    public override void OnBecameInvisible()
    {
        this.enabled = false;
    }

    public override void OnBecameVisible()
    {
        this.enabled = true;
    }

    public override void OnDrawGizmos()
    {
        float num;
        Color color;
        Gizmos.color = this.glowColor;
        float single1 = num = this.maxGlow * 0.25f;
        Color color1 = color = Gizmos.color;
        float single2 = color.a = num;
        Gizmos.color = color;
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Vector3 size = (Vector3) (5f * Vector3.Scale(Vector3.one, new Vector3((float) 1, (float) 0, (float) 1)));
        Gizmos.DrawCube(Vector3.zero, size);
        Gizmos.matrix = Matrix4x4.identity;
    }

    public override void OnDrawGizmosSelected()
    {
        float num;
        Color color;
        Gizmos.color = this.glowColor;
        float single1 = num = this.maxGlow;
        Color color1 = color = Gizmos.color;
        float single2 = color.a = num;
        Gizmos.color = color;
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Vector3 size = (Vector3) (5f * Vector3.Scale(Vector3.one, new Vector3((float) 1, (float) 0, (float) 1)));
        Gizmos.DrawCube(Vector3.zero, size);
        Gizmos.matrix = Matrix4x4.identity;
    }

    public override void Start()
    {
        if (this.playerTransform == null)
        {
            this.playerTransform = GameObject.FindWithTag("Player").transform;
        }
        this.pos = this.transform.position;
        this.scale = this.transform.localScale;
        this.mat = this.GetComponent<Renderer>().material;
        this.enabled = false;
    }

    public override void Update()
    {
        Vector3 vector = this.pos - this.playerTransform.position;
        vector.y = 0;
        float magnitude = vector.magnitude;
        this.transform.localScale = Vector3.Lerp((Vector3) (Vector3.one * this.minGlow), this.scale, Mathf.Clamp01(magnitude * 0.35f));
        this.mat.SetColor("_TintColor", (Color) (this.glowColor * Mathf.Clamp(magnitude * 0.1f, this.minGlow, this.maxGlow)));
    }
}

