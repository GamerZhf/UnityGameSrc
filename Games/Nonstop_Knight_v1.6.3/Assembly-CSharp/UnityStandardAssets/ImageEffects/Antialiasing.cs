namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Other/Antialiasing"), RequireComponent(typeof(Camera))]
    public class Antialiasing : PostEffectsBase
    {
        public float blurRadius = 18f;
        private Material dlaa;
        public Shader dlaaShader;
        public bool dlaaSharp;
        public float edgeSharpness = 4f;
        public float edgeThreshold = 0.2f;
        public float edgeThresholdMin = 0.05f;
        private Material materialFXAAII;
        private Material materialFXAAIII;
        private Material materialFXAAPreset2;
        private Material materialFXAAPreset3;
        public AAMode mode = AAMode.FXAA3Console;
        private Material nfaa;
        public Shader nfaaShader;
        public float offsetScale = 0.2f;
        public Shader shaderFXAAII;
        public Shader shaderFXAAIII;
        public Shader shaderFXAAPreset2;
        public Shader shaderFXAAPreset3;
        public bool showGeneratedNormals;
        private Material ssaa;
        public Shader ssaaShader;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.materialFXAAPreset2 = base.CreateMaterial(this.shaderFXAAPreset2, this.materialFXAAPreset2);
            this.materialFXAAPreset3 = base.CreateMaterial(this.shaderFXAAPreset3, this.materialFXAAPreset3);
            this.materialFXAAII = base.CreateMaterial(this.shaderFXAAII, this.materialFXAAII);
            this.materialFXAAIII = base.CreateMaterial(this.shaderFXAAIII, this.materialFXAAIII);
            this.nfaa = base.CreateMaterial(this.nfaaShader, this.nfaa);
            this.ssaa = base.CreateMaterial(this.ssaaShader, this.ssaa);
            this.dlaa = base.CreateMaterial(this.dlaaShader, this.dlaa);
            if (!this.ssaaShader.isSupported)
            {
                base.NotSupported();
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        public Material CurrentAAMaterial()
        {
            switch (this.mode)
            {
                case AAMode.FXAA2:
                    return this.materialFXAAII;

                case AAMode.FXAA3Console:
                    return this.materialFXAAIII;

                case AAMode.FXAA1PresetA:
                    return this.materialFXAAPreset2;

                case AAMode.FXAA1PresetB:
                    return this.materialFXAAPreset3;

                case AAMode.NFAA:
                    return this.nfaa;

                case AAMode.SSAA:
                    return this.ssaa;

                case AAMode.DLAA:
                    return this.dlaa;
            }
            return null;
        }

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else if ((this.mode == AAMode.FXAA3Console) && (this.materialFXAAIII != null))
            {
                this.materialFXAAIII.SetFloat("_EdgeThresholdMin", this.edgeThresholdMin);
                this.materialFXAAIII.SetFloat("_EdgeThreshold", this.edgeThreshold);
                this.materialFXAAIII.SetFloat("_EdgeSharpness", this.edgeSharpness);
                Graphics.Blit(source, destination, this.materialFXAAIII);
            }
            else if ((this.mode == AAMode.FXAA1PresetB) && (this.materialFXAAPreset3 != null))
            {
                Graphics.Blit(source, destination, this.materialFXAAPreset3);
            }
            else if ((this.mode == AAMode.FXAA1PresetA) && (this.materialFXAAPreset2 != null))
            {
                source.anisoLevel = 4;
                Graphics.Blit(source, destination, this.materialFXAAPreset2);
                source.anisoLevel = 0;
            }
            else if ((this.mode == AAMode.FXAA2) && (this.materialFXAAII != null))
            {
                Graphics.Blit(source, destination, this.materialFXAAII);
            }
            else if ((this.mode == AAMode.SSAA) && (this.ssaa != null))
            {
                Graphics.Blit(source, destination, this.ssaa);
            }
            else if ((this.mode == AAMode.DLAA) && (this.dlaa != null))
            {
                source.anisoLevel = 0;
                RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
                Graphics.Blit(source, temporary, this.dlaa, 0);
                Graphics.Blit(temporary, destination, this.dlaa, !this.dlaaSharp ? 1 : 2);
                RenderTexture.ReleaseTemporary(temporary);
            }
            else if ((this.mode == AAMode.NFAA) && (this.nfaa != null))
            {
                source.anisoLevel = 0;
                this.nfaa.SetFloat("_OffsetScale", this.offsetScale);
                this.nfaa.SetFloat("_BlurRadius", this.blurRadius);
                Graphics.Blit(source, destination, this.nfaa, !this.showGeneratedNormals ? 0 : 1);
            }
            else
            {
                Graphics.Blit(source, destination);
            }
        }
    }
}

