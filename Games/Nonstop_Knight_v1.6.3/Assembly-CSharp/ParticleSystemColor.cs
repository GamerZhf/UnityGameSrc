using System;
using UnityEngine;

public abstract class ParticleSystemColor : MonoBehaviour
{
    protected ParticleSystemColor()
    {
    }

    public abstract void instantiateMaterials();
    public abstract void resetColor();
    public abstract void setColor(object param);
}

