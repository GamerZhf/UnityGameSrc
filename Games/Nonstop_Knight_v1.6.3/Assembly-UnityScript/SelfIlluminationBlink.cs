using System;
using UnityEngine;

[Serializable]
public class SelfIlluminationBlink : MonoBehaviour
{
    public float blink;

    public override void Blink()
    {
        this.blink = 1f - this.blink;
    }

    public override void Main()
    {
    }

    public override void OnWillRenderObject()
    {
        this.GetComponent<Renderer>().sharedMaterial.SetFloat("_SelfIllumStrength", this.blink);
    }
}

