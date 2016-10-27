namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Camera)), ExecuteInEditMode, AddComponentMenu("Image Effects/Camera/Depth of Field (deprecated)")]
    public class DepthOfFieldDeprecated : PostEffectsBase
    {
        private Camera _camera;
        public DofBlurriness bluriness = DofBlurriness.High;
        public bool bokeh;
        private static float BOKEH_EXTRA_BLUR = 2f;
        public BokehDestination bokehDestination = BokehDestination.Background;
        public int bokehDownsample = 1;
        public float bokehIntensity = 0.15f;
        private Material bokehMaterial;
        public float bokehScale = 2.4f;
        public Shader bokehShader;
        private RenderTexture bokehSource;
        private RenderTexture bokehSource2;
        public bool bokehSupport = true;
        public Texture2D bokehTexture;
        public float bokehThresholdContrast = 0.1f;
        public float bokehThresholdLuminance = 0.55f;
        private Material dofBlurMaterial;
        public Shader dofBlurShader;
        private Material dofMaterial;
        public Shader dofShader;
        private RenderTexture finalDefocus;
        private float focalDistance01 = 0.1f;
        private float focalEndCurve = 2f;
        public float focalPoint = 1f;
        public float focalSize;
        private float focalStartCurve = 2f;
        public float focalZDistance;
        public float focalZEndCurve = 1f;
        public float focalZStartCurve = 1f;
        public float foregroundBlurExtrude = 1.15f;
        private RenderTexture foregroundTexture;
        private RenderTexture lowRezWorkTexture;
        public float maxBlurSpread = 1.75f;
        private RenderTexture mediumRezWorkTexture;
        public Transform objectFocus;
        private float oneOverBaseSize = 0.001953125f;
        public Dof34QualitySetting quality = Dof34QualitySetting.OnlyBackground;
        public DofResolution resolution = DofResolution.Low;
        public bool simpleTweakMode = true;
        private static int SMOOTH_DOWNSAMPLE_PASS = 6;
        public float smoothness = 0.5f;
        public bool visualize;
        private float widthOverHeight = 1.25f;

        private void AddBokeh(RenderTexture bokehInfo, RenderTexture tempTex, RenderTexture finalTarget)
        {
            if (this.bokehMaterial != null)
            {
                Mesh[] meshes = Quads.GetMeshes(tempTex.width, tempTex.height);
                RenderTexture.active = tempTex;
                GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
                GL.PushMatrix();
                GL.LoadIdentity();
                bokehInfo.filterMode = FilterMode.Point;
                float num = (bokehInfo.width * 1f) / (bokehInfo.height * 1f);
                float x = 2f / (1f * bokehInfo.width);
                x += ((this.bokehScale * this.maxBlurSpread) * BOKEH_EXTRA_BLUR) * this.oneOverBaseSize;
                this.bokehMaterial.SetTexture("_Source", bokehInfo);
                this.bokehMaterial.SetTexture("_MainTex", this.bokehTexture);
                this.bokehMaterial.SetVector("_ArScale", new Vector4(x, x * num, 0.5f, 0.5f * num));
                this.bokehMaterial.SetFloat("_Intensity", this.bokehIntensity);
                this.bokehMaterial.SetPass(0);
                foreach (Mesh mesh in meshes)
                {
                    if (mesh != null)
                    {
                        Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
                    }
                }
                GL.PopMatrix();
                Graphics.Blit(tempTex, finalTarget, this.dofMaterial, 8);
                bokehInfo.filterMode = FilterMode.Bilinear;
            }
        }

        private void AllocateTextures(bool blurForeground, RenderTexture source, int divider, int lowTexDivider)
        {
            this.foregroundTexture = null;
            if (blurForeground)
            {
                this.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
            }
            this.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
            this.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
            this.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0);
            this.bokehSource = null;
            this.bokehSource2 = null;
            if (this.bokeh)
            {
                this.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
                this.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
                this.bokehSource.filterMode = FilterMode.Bilinear;
                this.bokehSource2.filterMode = FilterMode.Bilinear;
                RenderTexture.active = this.bokehSource2;
                GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
            }
            source.filterMode = FilterMode.Bilinear;
            this.finalDefocus.filterMode = FilterMode.Bilinear;
            this.mediumRezWorkTexture.filterMode = FilterMode.Bilinear;
            this.lowRezWorkTexture.filterMode = FilterMode.Bilinear;
            if (this.foregroundTexture != null)
            {
                this.foregroundTexture.filterMode = FilterMode.Bilinear;
            }
        }

        private void Blur(RenderTexture from, RenderTexture to, DofBlurriness iterations, int blurPass, float spread)
        {
            RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
            if (iterations > DofBlurriness.Low)
            {
                this.BlurHex(from, to, blurPass, spread, temporary);
                if (iterations > DofBlurriness.High)
                {
                    this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
                    Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
                    this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, 0f, 0f, 0f));
                    Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
                }
            }
            else
            {
                this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
                Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
                this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, 0f, 0f, 0f));
                Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
            }
            RenderTexture.ReleaseTemporary(temporary);
        }

        private void BlurFg(RenderTexture from, RenderTexture to, DofBlurriness iterations, int blurPass, float spread)
        {
            this.dofBlurMaterial.SetTexture("_TapHigh", from);
            RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
            if (iterations > DofBlurriness.Low)
            {
                this.BlurHex(from, to, blurPass, spread, temporary);
                if (iterations > DofBlurriness.High)
                {
                    this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
                    Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
                    this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, 0f, 0f, 0f));
                    Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
                }
            }
            else
            {
                this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
                Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
                this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, 0f, 0f, 0f));
                Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
            }
            RenderTexture.ReleaseTemporary(temporary);
        }

        private void BlurHex(RenderTexture from, RenderTexture to, int blurPass, float spread, RenderTexture tmp)
        {
            this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
            Graphics.Blit(from, tmp, this.dofBlurMaterial, blurPass);
            this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, 0f, 0f, 0f));
            Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
            this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, spread * this.oneOverBaseSize, 0f, 0f));
            Graphics.Blit(to, tmp, this.dofBlurMaterial, blurPass);
            this.dofBlurMaterial.SetVector("offsets", new Vector4((spread / this.widthOverHeight) * this.oneOverBaseSize, -spread * this.oneOverBaseSize, 0f, 0f));
            Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
        }

        public override bool CheckResources()
        {
            base.CheckSupport(true);
            this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
            this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
            this.bokehSupport = this.bokehShader.isSupported;
            if ((this.bokeh && this.bokehSupport) && (this.bokehShader != null))
            {
                this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
            }
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private void CreateMaterials()
        {
            this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
            this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
            this.bokehSupport = this.bokehShader.isSupported;
            if ((this.bokeh && this.bokehSupport) && (this.bokehShader != null))
            {
                this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
            }
        }

        private void Downsample(RenderTexture from, RenderTexture to)
        {
            this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * to.width), 1f / (1f * to.height), 0f, 0f));
            Graphics.Blit(from, to, this.dofMaterial, SMOOTH_DOWNSAMPLE_PASS);
        }

        private float FocalDistance01(float worldDist)
        {
            return (this._camera.WorldToViewportPoint(((Vector3) ((worldDist - this._camera.nearClipPlane) * this._camera.transform.forward)) + this._camera.transform.position).z / (this._camera.farClipPlane - this._camera.nearClipPlane));
        }

        private int GetDividerBasedOnQuality()
        {
            int num = 1;
            if (this.resolution == DofResolution.Medium)
            {
                return 2;
            }
            if (this.resolution == DofResolution.Low)
            {
                num = 2;
            }
            return num;
        }

        private int GetLowResolutionDividerBasedOnQuality(int baseDivider)
        {
            int num = baseDivider;
            if (this.resolution == DofResolution.High)
            {
                num *= 2;
            }
            if (this.resolution == DofResolution.Low)
            {
                num *= 2;
            }
            return num;
        }

        private void OnDisable()
        {
            Quads.Cleanup();
        }

        private void OnEnable()
        {
            this._camera = base.GetComponent<Camera>();
            this._camera.depthTextureMode |= DepthTextureMode.Depth;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                if (this.smoothness < 0.1f)
                {
                    this.smoothness = 0.1f;
                }
                this.bokeh = this.bokeh && this.bokehSupport;
                float num = !this.bokeh ? 1f : BOKEH_EXTRA_BLUR;
                bool blurForeground = this.quality > Dof34QualitySetting.OnlyBackground;
                float num2 = this.focalSize / (this._camera.farClipPlane - this._camera.nearClipPlane);
                if (this.simpleTweakMode)
                {
                    this.focalDistance01 = (this.objectFocus == null) ? this.FocalDistance01(this.focalPoint) : (this._camera.WorldToViewportPoint(this.objectFocus.position).z / this._camera.farClipPlane);
                    this.focalStartCurve = this.focalDistance01 * this.smoothness;
                    this.focalEndCurve = this.focalStartCurve;
                    blurForeground = blurForeground && (this.focalPoint > (this._camera.nearClipPlane + Mathf.Epsilon));
                }
                else
                {
                    if (this.objectFocus != null)
                    {
                        Vector3 vector = this._camera.WorldToViewportPoint(this.objectFocus.position);
                        vector.z /= this._camera.farClipPlane;
                        this.focalDistance01 = vector.z;
                    }
                    else
                    {
                        this.focalDistance01 = this.FocalDistance01(this.focalZDistance);
                    }
                    this.focalStartCurve = this.focalZStartCurve;
                    this.focalEndCurve = this.focalZEndCurve;
                    blurForeground = blurForeground && (this.focalPoint > (this._camera.nearClipPlane + Mathf.Epsilon));
                }
                this.widthOverHeight = (1f * source.width) / (1f * source.height);
                this.oneOverBaseSize = 0.001953125f;
                this.dofMaterial.SetFloat("_ForegroundBlurExtrude", this.foregroundBlurExtrude);
                this.dofMaterial.SetVector("_CurveParams", new Vector4(!this.simpleTweakMode ? this.focalStartCurve : (1f / this.focalStartCurve), !this.simpleTweakMode ? this.focalEndCurve : (1f / this.focalEndCurve), num2 * 0.5f, this.focalDistance01));
                this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * source.width), 1f / (1f * source.height), 0f, 0f));
                int dividerBasedOnQuality = this.GetDividerBasedOnQuality();
                int lowResolutionDividerBasedOnQuality = this.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality);
                this.AllocateTextures(blurForeground, source, dividerBasedOnQuality, lowResolutionDividerBasedOnQuality);
                Graphics.Blit(source, source, this.dofMaterial, 3);
                this.Downsample(source, this.mediumRezWorkTexture);
                this.Blur(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DofBlurriness.Low, 4, this.maxBlurSpread);
                if (this.bokeh && ((BokehDestination.Foreground & this.bokehDestination) != ((BokehDestination) 0)))
                {
                    this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast, this.bokehThresholdLuminance, 0.95f, 0f));
                    Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
                    Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
                    this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread * num);
                }
                else
                {
                    this.Downsample(this.mediumRezWorkTexture, this.lowRezWorkTexture);
                    this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread);
                }
                this.dofBlurMaterial.SetTexture("_TapLow", this.lowRezWorkTexture);
                this.dofBlurMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
                Graphics.Blit(null, this.finalDefocus, this.dofBlurMaterial, 3);
                if (this.bokeh && ((BokehDestination.Foreground & this.bokehDestination) != ((BokehDestination) 0)))
                {
                    this.AddBokeh(this.bokehSource2, this.bokehSource, this.finalDefocus);
                }
                this.dofMaterial.SetTexture("_TapLowBackground", this.finalDefocus);
                this.dofMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
                Graphics.Blit(source, !blurForeground ? destination : this.foregroundTexture, this.dofMaterial, !this.visualize ? 0 : 2);
                if (blurForeground)
                {
                    Graphics.Blit(this.foregroundTexture, source, this.dofMaterial, 5);
                    this.Downsample(source, this.mediumRezWorkTexture);
                    this.BlurFg(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DofBlurriness.Low, 2, this.maxBlurSpread);
                    if (this.bokeh && ((BokehDestination.Foreground & this.bokehDestination) != ((BokehDestination) 0)))
                    {
                        this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast * 0.5f, this.bokehThresholdLuminance, 0f, 0f));
                        Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
                        Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
                        this.BlurFg(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread * num);
                    }
                    else
                    {
                        this.BlurFg(this.mediumRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread);
                    }
                    Graphics.Blit(this.lowRezWorkTexture, this.finalDefocus);
                    this.dofMaterial.SetTexture("_TapLowForeground", this.finalDefocus);
                    Graphics.Blit(source, destination, this.dofMaterial, !this.visualize ? 4 : 1);
                    if (this.bokeh && ((BokehDestination.Foreground & this.bokehDestination) != ((BokehDestination) 0)))
                    {
                        this.AddBokeh(this.bokehSource2, this.bokehSource, destination);
                    }
                }
                this.ReleaseTextures();
            }
        }

        private void ReleaseTextures()
        {
            if (this.foregroundTexture != null)
            {
                RenderTexture.ReleaseTemporary(this.foregroundTexture);
            }
            if (this.finalDefocus != null)
            {
                RenderTexture.ReleaseTemporary(this.finalDefocus);
            }
            if (this.mediumRezWorkTexture != null)
            {
                RenderTexture.ReleaseTemporary(this.mediumRezWorkTexture);
            }
            if (this.lowRezWorkTexture != null)
            {
                RenderTexture.ReleaseTemporary(this.lowRezWorkTexture);
            }
            if (this.bokehSource != null)
            {
                RenderTexture.ReleaseTemporary(this.bokehSource);
            }
            if (this.bokehSource2 != null)
            {
                RenderTexture.ReleaseTemporary(this.bokehSource2);
            }
        }

        public enum BokehDestination
        {
            Background = 1,
            BackgroundAndForeground = 3,
            Foreground = 2
        }

        public enum Dof34QualitySetting
        {
            BackgroundAndForeground = 2,
            OnlyBackground = 1
        }

        public enum DofBlurriness
        {
            High = 2,
            Low = 1,
            VeryHigh = 4
        }

        public enum DofResolution
        {
            High = 2,
            Low = 4,
            Medium = 3
        }
    }
}

