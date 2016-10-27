namespace PlayerView
{
    using System;
    using UnityEngine;

    public interface IFlashable
    {
        void initializeFlashMaterials();
        void setFlashMaterialColor(Color color);
        void setFlashMaterialsEnabled(bool enabled);
    }
}

