using PlayerView;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class CloneSmokeParticleSystem : ParticleSystemColor
{
    public ParticleSystemRenderer FogRenderer;
    private bool m_isColorOverridden;
    private ColorParameters m_originalColors;

    protected void Awake()
    {
        this.m_originalColors.Fog = this.FogRenderer.sharedMaterial.color;
    }

    public override void instantiateMaterials()
    {
        this.FogRenderer.sharedMaterial = Binder.MaterialStorage.getSharedGenericMaterial(this.FogRenderer.sharedMaterial);
    }

    protected void OnDestroy()
    {
        Binder.MaterialStorage.releaseSharedGenericMaterialReference(this.FogRenderer.sharedMaterial);
    }

    public override void resetColor()
    {
        if (this.m_isColorOverridden)
        {
            this.FogRenderer.material.color = this.m_originalColors.Fog;
        }
        this.m_isColorOverridden = false;
    }

    public override void setColor(object param)
    {
        if (!(param is ColorParameters))
        {
            Debug.LogWarning("Incompatible ColorParameters! Canceling particle system color override.");
        }
        else
        {
            ColorParameters parameters = (ColorParameters) param;
            parameters.Fog.a = this.m_originalColors.Fog.a;
            this.FogRenderer.sharedMaterial.color = parameters.Fog;
            this.m_isColorOverridden = true;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ColorParameters
    {
        public Color Fog;
    }
}

