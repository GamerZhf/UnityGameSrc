namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Image Effects/Displacement/Fisheye"), RequireComponent(typeof(Camera)), ExecuteInEditMode]
    internal class Fisheye : PostEffectsBase
    {
        private Material fisheyeMaterial;
        public Shader fishEyeShader;
        public float strengthX = 0.05f;
        public float strengthY = 0.05f;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.fisheyeMaterial = base.CheckShaderAndCreateMaterial(this.fishEyeShader, this.fisheyeMaterial);
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
                float num = 0.15625f;
                float num2 = (source.width * 1f) / (source.height * 1f);
                this.fisheyeMaterial.SetVector("intensity", new Vector4((this.strengthX * num2) * num, this.strengthY * num, (this.strengthX * num2) * num, this.strengthY * num));
                Graphics.Blit(source, destination, this.fisheyeMaterial);
            }
        }
    }
}

