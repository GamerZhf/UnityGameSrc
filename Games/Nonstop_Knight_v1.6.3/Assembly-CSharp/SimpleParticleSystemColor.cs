using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParticleSystemColor : ParticleSystemColor
{
    private Color m_origStartColor;
    public List<ParticleSystem> ParticleSystems;

    protected void Awake()
    {
        this.m_origStartColor = this.ParticleSystems[0].startColor;
    }

    public override void instantiateMaterials()
    {
    }

    public override void resetColor()
    {
        for (int i = 0; i < this.ParticleSystems.Count; i++)
        {
            this.ParticleSystems[i].startColor = this.m_origStartColor;
        }
    }

    public override void setColor(object param)
    {
        Color color = (Color) param;
        for (int i = 0; i < this.ParticleSystems.Count; i++)
        {
            this.ParticleSystems[i].startColor = color;
        }
    }
}

