namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Image Effects/Color Adjustments/Color Correction (3D Lookup Texture)"), ExecuteInEditMode]
    public class ColorCorrectionLookup : PostEffectsBase
    {
        public string basedOnTempTex = string.Empty;
        public Texture3D converted3DLut;
        private Material material;
        public Shader shader;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.material = base.CheckShaderAndCreateMaterial(this.shader, this.material);
            if (!base.isSupported || !SystemInfo.supports3DTextures)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        public void Convert(Texture2D temp2DTex, string path)
        {
            if (temp2DTex != null)
            {
                int width = temp2DTex.width * temp2DTex.height;
                width = temp2DTex.height;
                if (!this.ValidDimensions(temp2DTex))
                {
                    Debug.LogWarning("The given 2D texture " + temp2DTex.name + " cannot be used as a 3D LUT.");
                    this.basedOnTempTex = string.Empty;
                }
                else
                {
                    Color[] pixels = temp2DTex.GetPixels();
                    Color[] colors = new Color[pixels.Length];
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            for (int k = 0; k < width; k++)
                            {
                                int num5 = (width - j) - 1;
                                colors[(i + (j * width)) + ((k * width) * width)] = pixels[((k * width) + i) + ((num5 * width) * width)];
                            }
                        }
                    }
                    if (this.converted3DLut != null)
                    {
                        UnityEngine.Object.DestroyImmediate(this.converted3DLut);
                    }
                    this.converted3DLut = new Texture3D(width, width, width, TextureFormat.ARGB32, false);
                    this.converted3DLut.SetPixels(colors);
                    this.converted3DLut.Apply();
                    this.basedOnTempTex = path;
                }
            }
            else
            {
                Debug.LogError("Couldn't color correct with 3D LUT texture. Image Effect will be disabled.");
            }
        }

        private void OnDestroy()
        {
            if (this.converted3DLut != null)
            {
                UnityEngine.Object.DestroyImmediate(this.converted3DLut);
            }
            this.converted3DLut = null;
        }

        private void OnDisable()
        {
            if (this.material != null)
            {
                UnityEngine.Object.DestroyImmediate(this.material);
                this.material = null;
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources() || !SystemInfo.supports3DTextures)
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                if (this.converted3DLut == null)
                {
                    this.SetIdentityLut();
                }
                int width = this.converted3DLut.width;
                this.converted3DLut.wrapMode = TextureWrapMode.Clamp;
                this.material.SetFloat("_Scale", ((float) (width - 1)) / (1f * width));
                this.material.SetFloat("_Offset", 1f / (2f * width));
                this.material.SetTexture("_ClutTex", this.converted3DLut);
                Graphics.Blit(source, destination, this.material, (QualitySettings.activeColorSpace != ColorSpace.Linear) ? 0 : 1);
            }
        }

        public void SetIdentityLut()
        {
            int width = 0x10;
            Color[] colors = new Color[(width * width) * width];
            float num2 = 1f / ((1f * width) - 1f);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < width; k++)
                    {
                        colors[(i + (j * width)) + ((k * width) * width)] = new Color((i * 1f) * num2, (j * 1f) * num2, (k * 1f) * num2, 1f);
                    }
                }
            }
            if (this.converted3DLut != null)
            {
                UnityEngine.Object.DestroyImmediate(this.converted3DLut);
            }
            this.converted3DLut = new Texture3D(width, width, width, TextureFormat.ARGB32, false);
            this.converted3DLut.SetPixels(colors);
            this.converted3DLut.Apply();
            this.basedOnTempTex = string.Empty;
        }

        public bool ValidDimensions(Texture2D tex2d)
        {
            if (tex2d == null)
            {
                return false;
            }
            if (tex2d.height != Mathf.FloorToInt(Mathf.Sqrt((float) tex2d.width)))
            {
                return false;
            }
            return true;
        }
    }
}

