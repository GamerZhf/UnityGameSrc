namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Bloom and Glow/BloomAndFlares (3.5, Deprecated)")]
    public class BloomAndFlares : PostEffectsBase
    {
        private Material addBrightStuffBlendOneOneMaterial;
        public Shader addBrightStuffOneOneShader;
        public int bloomBlurIterations = 2;
        public float bloomIntensity = 1f;
        public float bloomThreshold = 0.5f;
        private Material brightPassFilterMaterial;
        public Shader brightPassFilterShader;
        private bool doHdr;
        public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);
        public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);
        public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);
        public Color flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);
        public UnityStandardAssets.ImageEffects.HDRBloomMode hdr;
        public float hollyStretchWidth = 3.5f;
        public int hollywoodFlareBlurIterations = 2;
        private Material hollywoodFlaresMaterial;
        public Shader hollywoodFlaresShader;
        public float lensflareIntensity = 1f;
        private Material lensFlareMaterial;
        public LensflareStyle34 lensflareMode = LensflareStyle34.Anamorphic;
        public bool lensflares;
        public Shader lensFlareShader;
        public float lensflareThreshold = 0.3f;
        public Texture2D lensFlareVignetteMask;
        private Material screenBlend;
        public UnityStandardAssets.ImageEffects.BloomScreenBlendMode screenBlendMode = UnityStandardAssets.ImageEffects.BloomScreenBlendMode.Add;
        public Shader screenBlendShader;
        private Material separableBlurMaterial;
        public Shader separableBlurShader;
        public float sepBlurSpread = 1.5f;
        public TweakMode34 tweakMode;
        public float useSrcAlphaAsMask = 0.5f;
        private Material vignetteMaterial;
        public Shader vignetteShader;

        private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
        {
            this.addBrightStuffBlendOneOneMaterial.SetFloat("_Intensity", intensity_);
            Graphics.Blit(from, to, this.addBrightStuffBlendOneOneMaterial);
        }

        private void BlendFlares(RenderTexture from, RenderTexture to)
        {
            this.lensFlareMaterial.SetVector("colorA", (Vector4) (new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity));
            this.lensFlareMaterial.SetVector("colorB", (Vector4) (new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity));
            this.lensFlareMaterial.SetVector("colorC", (Vector4) (new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity));
            this.lensFlareMaterial.SetVector("colorD", (Vector4) (new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity));
            Graphics.Blit(from, to, this.lensFlareMaterial);
        }

        private void BrightFilter(float thresh, float useAlphaAsMask, RenderTexture from, RenderTexture to)
        {
            if (this.doHdr)
            {
                this.brightPassFilterMaterial.SetVector("threshold", new Vector4(thresh, 1f, 0f, 0f));
            }
            else
            {
                this.brightPassFilterMaterial.SetVector("threshold", new Vector4(thresh, 1f / (1f - thresh), 0f, 0f));
            }
            this.brightPassFilterMaterial.SetFloat("useSrcAlphaAsMask", useAlphaAsMask);
            Graphics.Blit(from, to, this.brightPassFilterMaterial);
        }

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.screenBlend = base.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
            this.lensFlareMaterial = base.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
            this.vignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.vignetteMaterial);
            this.separableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
            this.addBrightStuffBlendOneOneMaterial = base.CheckShaderAndCreateMaterial(this.addBrightStuffOneOneShader, this.addBrightStuffBlendOneOneMaterial);
            this.hollywoodFlaresMaterial = base.CheckShaderAndCreateMaterial(this.hollywoodFlaresShader, this.hollywoodFlaresMaterial);
            this.brightPassFilterMaterial = base.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
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
                this.doHdr = false;
                if (this.hdr == UnityStandardAssets.ImageEffects.HDRBloomMode.Auto)
                {
                    this.doHdr = (source.format == RenderTextureFormat.ARGBHalf) && base.GetComponent<Camera>().hdr;
                }
                else
                {
                    this.doHdr = this.hdr == UnityStandardAssets.ImageEffects.HDRBloomMode.On;
                }
                this.doHdr = this.doHdr && base.supportHDRTextures;
                UnityStandardAssets.ImageEffects.BloomScreenBlendMode screenBlendMode = this.screenBlendMode;
                if (this.doHdr)
                {
                    screenBlendMode = UnityStandardAssets.ImageEffects.BloomScreenBlendMode.Add;
                }
                RenderTextureFormat format = !this.doHdr ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf;
                RenderTexture dest = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, format);
                RenderTexture texture2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
                RenderTexture to = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
                RenderTexture texture4 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
                float num = (1f * source.width) / (1f * source.height);
                float num2 = 0.001953125f;
                Graphics.Blit(source, dest, this.screenBlend, 2);
                Graphics.Blit(dest, texture2, this.screenBlend, 2);
                RenderTexture.ReleaseTemporary(dest);
                this.BrightFilter(this.bloomThreshold, this.useSrcAlphaAsMask, texture2, to);
                texture2.DiscardContents();
                if (this.bloomBlurIterations < 1)
                {
                    this.bloomBlurIterations = 1;
                }
                for (int i = 0; i < this.bloomBlurIterations; i++)
                {
                    float num4 = (1f + (i * 0.5f)) * this.sepBlurSpread;
                    this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, num4 * num2, 0f, 0f));
                    RenderTexture texture5 = (i != 0) ? texture2 : to;
                    Graphics.Blit(texture5, texture4, this.separableBlurMaterial);
                    texture5.DiscardContents();
                    this.separableBlurMaterial.SetVector("offsets", new Vector4((num4 / num) * num2, 0f, 0f, 0f));
                    Graphics.Blit(texture4, texture2, this.separableBlurMaterial);
                    texture4.DiscardContents();
                }
                if (this.lensflares)
                {
                    if (this.lensflareMode == LensflareStyle34.Ghosting)
                    {
                        this.BrightFilter(this.lensflareThreshold, 0f, texture2, texture4);
                        texture2.DiscardContents();
                        this.Vignette(0.975f, texture4, to);
                        texture4.DiscardContents();
                        this.BlendFlares(to, texture2);
                        to.DiscardContents();
                    }
                    else
                    {
                        this.hollywoodFlaresMaterial.SetVector("_threshold", new Vector4(this.lensflareThreshold, 1f / (1f - this.lensflareThreshold), 0f, 0f));
                        this.hollywoodFlaresMaterial.SetVector("tintColor", (Vector4) ((new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a) * this.lensflareIntensity));
                        Graphics.Blit(texture4, to, this.hollywoodFlaresMaterial, 2);
                        texture4.DiscardContents();
                        Graphics.Blit(to, texture4, this.hollywoodFlaresMaterial, 3);
                        to.DiscardContents();
                        this.hollywoodFlaresMaterial.SetVector("offsets", new Vector4(((this.sepBlurSpread * 1f) / num) * num2, 0f, 0f, 0f));
                        this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth);
                        Graphics.Blit(texture4, to, this.hollywoodFlaresMaterial, 1);
                        texture4.DiscardContents();
                        this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 2f);
                        Graphics.Blit(to, texture4, this.hollywoodFlaresMaterial, 1);
                        to.DiscardContents();
                        this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 4f);
                        Graphics.Blit(texture4, to, this.hollywoodFlaresMaterial, 1);
                        texture4.DiscardContents();
                        if (this.lensflareMode == LensflareStyle34.Anamorphic)
                        {
                            for (int j = 0; j < this.hollywoodFlareBlurIterations; j++)
                            {
                                this.separableBlurMaterial.SetVector("offsets", new Vector4(((this.hollyStretchWidth * 2f) / num) * num2, 0f, 0f, 0f));
                                Graphics.Blit(to, texture4, this.separableBlurMaterial);
                                to.DiscardContents();
                                this.separableBlurMaterial.SetVector("offsets", new Vector4(((this.hollyStretchWidth * 2f) / num) * num2, 0f, 0f, 0f));
                                Graphics.Blit(texture4, to, this.separableBlurMaterial);
                                texture4.DiscardContents();
                            }
                            this.AddTo(1f, to, texture2);
                            to.DiscardContents();
                        }
                        else
                        {
                            for (int k = 0; k < this.hollywoodFlareBlurIterations; k++)
                            {
                                this.separableBlurMaterial.SetVector("offsets", new Vector4(((this.hollyStretchWidth * 2f) / num) * num2, 0f, 0f, 0f));
                                Graphics.Blit(to, texture4, this.separableBlurMaterial);
                                to.DiscardContents();
                                this.separableBlurMaterial.SetVector("offsets", new Vector4(((this.hollyStretchWidth * 2f) / num) * num2, 0f, 0f, 0f));
                                Graphics.Blit(texture4, to, this.separableBlurMaterial);
                                texture4.DiscardContents();
                            }
                            this.Vignette(1f, to, texture4);
                            to.DiscardContents();
                            this.BlendFlares(texture4, to);
                            texture4.DiscardContents();
                            this.AddTo(1f, to, texture2);
                            to.DiscardContents();
                        }
                    }
                }
                this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
                this.screenBlend.SetTexture("_ColorBuffer", source);
                Graphics.Blit(texture2, destination, this.screenBlend, (int) screenBlendMode);
                RenderTexture.ReleaseTemporary(texture2);
                RenderTexture.ReleaseTemporary(to);
                RenderTexture.ReleaseTemporary(texture4);
            }
        }

        private void Vignette(float amount, RenderTexture from, RenderTexture to)
        {
            if (this.lensFlareVignetteMask != null)
            {
                this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
                Graphics.Blit(from, to, this.screenBlend, 3);
            }
            else
            {
                this.vignetteMaterial.SetFloat("vignetteIntensity", amount);
                Graphics.Blit(from, to, this.vignetteMaterial);
            }
        }
    }
}

