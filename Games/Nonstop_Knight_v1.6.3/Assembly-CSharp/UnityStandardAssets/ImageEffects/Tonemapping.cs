namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Image Effects/Color Adjustments/Tonemapping"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
    public class Tonemapping : PostEffectsBase
    {
        public float adaptionSpeed = 1.5f;
        public AdaptiveTexSize adaptiveTextureSize = AdaptiveTexSize.Square256;
        private Texture2D curveTex;
        public float exposureAdjustment = 1.5f;
        public float middleGrey = 0.4f;
        public AnimationCurve remapCurve;
        private RenderTexture rt;
        private RenderTextureFormat rtFormat = RenderTextureFormat.ARGBHalf;
        private Material tonemapMaterial;
        public Shader tonemapper;
        public TonemapperType type = TonemapperType.Photographic;
        public bool validRenderTextureFormat = true;
        public float white = 2f;

        public override bool CheckResources()
        {
            base.CheckSupport(false, true);
            this.tonemapMaterial = base.CheckShaderAndCreateMaterial(this.tonemapper, this.tonemapMaterial);
            if ((this.curveTex == null) && (this.type == TonemapperType.UserCurve))
            {
                this.curveTex = new Texture2D(0x100, 1, TextureFormat.ARGB32, false, true);
                this.curveTex.filterMode = FilterMode.Bilinear;
                this.curveTex.wrapMode = TextureWrapMode.Clamp;
                this.curveTex.hideFlags = HideFlags.DontSave;
            }
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private bool CreateInternalRenderTexture()
        {
            if (this.rt != null)
            {
                return false;
            }
            this.rtFormat = !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.RGHalf;
            this.rt = new RenderTexture(1, 1, 0, this.rtFormat);
            this.rt.hideFlags = HideFlags.DontSave;
            return true;
        }

        private void OnDisable()
        {
            if (this.rt != null)
            {
                UnityEngine.Object.DestroyImmediate(this.rt);
                this.rt = null;
            }
            if (this.tonemapMaterial != null)
            {
                UnityEngine.Object.DestroyImmediate(this.tonemapMaterial);
                this.tonemapMaterial = null;
            }
            if (this.curveTex != null)
            {
                UnityEngine.Object.DestroyImmediate(this.curveTex);
                this.curveTex = null;
            }
        }

        [ImageEffectTransformsToLDR]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                this.exposureAdjustment = (this.exposureAdjustment >= 0.001f) ? this.exposureAdjustment : 0.001f;
                if (this.type == TonemapperType.UserCurve)
                {
                    float num = this.UpdateCurve();
                    this.tonemapMaterial.SetFloat("_RangeScale", num);
                    this.tonemapMaterial.SetTexture("_Curve", this.curveTex);
                    Graphics.Blit(source, destination, this.tonemapMaterial, 4);
                }
                else if (this.type == TonemapperType.SimpleReinhard)
                {
                    this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
                    Graphics.Blit(source, destination, this.tonemapMaterial, 6);
                }
                else if (this.type == TonemapperType.Hable)
                {
                    this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
                    Graphics.Blit(source, destination, this.tonemapMaterial, 5);
                }
                else if (this.type == TonemapperType.Photographic)
                {
                    this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
                    Graphics.Blit(source, destination, this.tonemapMaterial, 8);
                }
                else if (this.type == TonemapperType.OptimizedHejiDawson)
                {
                    this.tonemapMaterial.SetFloat("_ExposureAdjustment", 0.5f * this.exposureAdjustment);
                    Graphics.Blit(source, destination, this.tonemapMaterial, 7);
                }
                else
                {
                    bool flag = this.CreateInternalRenderTexture();
                    RenderTexture dest = RenderTexture.GetTemporary((int) this.adaptiveTextureSize, (int) this.adaptiveTextureSize, 0, this.rtFormat);
                    Graphics.Blit(source, dest);
                    int num2 = (int) Mathf.Log(dest.width * 1f, 2f);
                    int num3 = 2;
                    RenderTexture[] textureArray = new RenderTexture[num2];
                    for (int i = 0; i < num2; i++)
                    {
                        textureArray[i] = RenderTexture.GetTemporary(dest.width / num3, dest.width / num3, 0, this.rtFormat);
                        num3 *= 2;
                    }
                    RenderTexture texture2 = textureArray[num2 - 1];
                    Graphics.Blit(dest, textureArray[0], this.tonemapMaterial, 1);
                    if (this.type == TonemapperType.AdaptiveReinhardAutoWhite)
                    {
                        for (int k = 0; k < (num2 - 1); k++)
                        {
                            Graphics.Blit(textureArray[k], textureArray[k + 1], this.tonemapMaterial, 9);
                            texture2 = textureArray[k + 1];
                        }
                    }
                    else if (this.type == TonemapperType.AdaptiveReinhard)
                    {
                        for (int m = 0; m < (num2 - 1); m++)
                        {
                            Graphics.Blit(textureArray[m], textureArray[m + 1]);
                            texture2 = textureArray[m + 1];
                        }
                    }
                    this.adaptionSpeed = (this.adaptionSpeed >= 0.001f) ? this.adaptionSpeed : 0.001f;
                    this.tonemapMaterial.SetFloat("_AdaptionSpeed", this.adaptionSpeed);
                    this.rt.MarkRestoreExpected();
                    Graphics.Blit(texture2, this.rt, this.tonemapMaterial, !flag ? 2 : 3);
                    this.middleGrey = (this.middleGrey >= 0.001f) ? this.middleGrey : 0.001f;
                    this.tonemapMaterial.SetVector("_HdrParams", new Vector4(this.middleGrey, this.middleGrey, this.middleGrey, this.white * this.white));
                    this.tonemapMaterial.SetTexture("_SmallTex", this.rt);
                    if (this.type == TonemapperType.AdaptiveReinhard)
                    {
                        Graphics.Blit(source, destination, this.tonemapMaterial, 0);
                    }
                    else if (this.type == TonemapperType.AdaptiveReinhardAutoWhite)
                    {
                        Graphics.Blit(source, destination, this.tonemapMaterial, 10);
                    }
                    else
                    {
                        Debug.LogError("No valid adaptive tonemapper type found!");
                        Graphics.Blit(source, destination);
                    }
                    for (int j = 0; j < num2; j++)
                    {
                        RenderTexture.ReleaseTemporary(textureArray[j]);
                    }
                    RenderTexture.ReleaseTemporary(dest);
                }
            }
        }

        public float UpdateCurve()
        {
            float time = 1f;
            if (this.remapCurve.keys.Length < 1)
            {
                Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(2f, 1f) };
                this.remapCurve = new AnimationCurve(keys);
            }
            if (this.remapCurve != null)
            {
                if (this.remapCurve.length > 0)
                {
                    Keyframe keyframe = this.remapCurve[this.remapCurve.length - 1];
                    time = keyframe.time;
                }
                for (float i = 0f; i <= 1f; i += 0.003921569f)
                {
                    float r = this.remapCurve.Evaluate((i * 1f) * time);
                    this.curveTex.SetPixel((int) Mathf.Floor(i * 255f), 0, new Color(r, r, r));
                }
                this.curveTex.Apply();
            }
            return (1f / time);
        }

        public enum AdaptiveTexSize
        {
            Square1024 = 0x400,
            Square128 = 0x80,
            Square16 = 0x10,
            Square256 = 0x100,
            Square32 = 0x20,
            Square512 = 0x200,
            Square64 = 0x40
        }

        public enum TonemapperType
        {
            SimpleReinhard,
            UserCurve,
            Hable,
            Photographic,
            OptimizedHejiDawson,
            AdaptiveReinhard,
            AdaptiveReinhardAutoWhite
        }
    }
}

