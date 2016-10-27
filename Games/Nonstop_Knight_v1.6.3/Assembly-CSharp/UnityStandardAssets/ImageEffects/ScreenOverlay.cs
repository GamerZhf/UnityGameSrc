namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Image Effects/Other/Screen Overlay"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
    public class ScreenOverlay : PostEffectsBase
    {
        public OverlayBlendMode blendMode = OverlayBlendMode.Overlay;
        public float intensity = 1f;
        private Material overlayMaterial;
        public Shader overlayShader;
        public Texture2D texture;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.overlayMaterial = base.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                Vector4 vector = new Vector4(1f, 0f, 0f, 1f);
                this.overlayMaterial.SetVector("_UV_Transform", vector);
                this.overlayMaterial.SetFloat("_Intensity", this.intensity);
                this.overlayMaterial.SetTexture("_Overlay", this.texture);
                Graphics.Blit(source, destination, this.overlayMaterial, (int) this.blendMode);
            }
        }

        public enum OverlayBlendMode
        {
            Additive,
            ScreenBlend,
            Multiply,
            Overlay,
            AlphaBlend
        }
    }
}

