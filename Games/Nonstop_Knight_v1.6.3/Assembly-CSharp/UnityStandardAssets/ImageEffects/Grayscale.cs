namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Image Effects/Color Adjustments/Grayscale"), ExecuteInEditMode]
    public class Grayscale : ImageEffectBase
    {
        public float rampOffset;
        public Texture textureRamp;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            base.material.SetTexture("_RampTex", this.textureRamp);
            base.material.SetFloat("_RampOffset", this.rampOffset);
            Graphics.Blit(source, destination, base.material);
        }
    }
}

