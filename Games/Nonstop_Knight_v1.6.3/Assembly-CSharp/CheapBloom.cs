using System;
using UnityEngine;

public class CheapBloom : MonoBehaviour
{
    public UnityEngine.Material Material;

    protected void Awake()
    {
        if (SystemInfo.supportsImageEffects)
        {
            Shader shader = Shader.Find("CUSTOM/CheapBloom");
            if (shader.isSupported)
            {
                this.Material = new UnityEngine.Material(shader);
                return;
            }
        }
        base.enabled = false;
    }

    protected void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width / 4;
        int height = source.height / 4;
        RenderTexture dest = RenderTexture.GetTemporary(width, height, 0, source.format);
        dest.filterMode = FilterMode.Bilinear;
        Graphics.Blit(source, dest, this.Material, 0);
        RenderTexture texture2 = RenderTexture.GetTemporary(width, height, 0, source.format);
        texture2.filterMode = FilterMode.Bilinear;
        Graphics.Blit(dest, texture2, this.Material, 1);
        RenderTexture.ReleaseTemporary(dest);
        this.Material.SetTexture("_BloomTex", texture2);
        RenderTexture.ReleaseTemporary(texture2);
        Graphics.Blit(source, destination, this.Material, 2);
    }
}

