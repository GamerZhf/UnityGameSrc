using System;
using UnityEngine;

[Serializable]
public class conveyorBelt : MonoBehaviour
{
    public Material mat;
    public float scrollSpeed = 0.1f;

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

    public override void Start()
    {
        this.enabled = false;
    }

    public override void Update()
    {
        float num = (Time.time * this.scrollSpeed) % 1f;
        this.mat.SetTextureOffset("_MainTex", new Vector2((float) 0, -num));
        this.mat.SetTextureOffset("_BumpMap", new Vector2((float) 0, -num));
    }
}

