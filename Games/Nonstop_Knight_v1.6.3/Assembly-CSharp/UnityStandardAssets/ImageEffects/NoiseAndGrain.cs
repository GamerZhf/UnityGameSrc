namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Noise/Noise And Grain (Filmic)"), RequireComponent(typeof(Camera))]
    public class NoiseAndGrain : PostEffectsBase
    {
        public float blackIntensity = 1f;
        public bool dx11Grain;
        private Material dx11NoiseMaterial;
        public Shader dx11NoiseShader;
        public FilterMode filterMode = FilterMode.Bilinear;
        public float generalIntensity = 0.5f;
        public Vector3 intensities = new Vector3(1f, 1f, 1f);
        public float intensityMultiplier = 0.25f;
        public float midGrey = 0.2f;
        public bool monochrome;
        public float monochromeTiling = 64f;
        private Material noiseMaterial;
        public Shader noiseShader;
        public Texture2D noiseTexture;
        public float softness;
        private static float TILE_AMOUNT = 64f;
        public Vector3 tiling = new Vector3(64f, 64f, 64f);
        public float whiteIntensity = 1f;

        public override bool CheckResources()
        {
            base.CheckSupport(false);
            this.noiseMaterial = base.CheckShaderAndCreateMaterial(this.noiseShader, this.noiseMaterial);
            if (this.dx11Grain && base.supportDX11)
            {
                this.dx11NoiseMaterial = base.CheckShaderAndCreateMaterial(this.dx11NoiseShader, this.dx11NoiseMaterial);
            }
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private static void DrawNoiseQuadGrid(RenderTexture source, RenderTexture dest, Material fxMaterial, Texture2D noise, int passNr)
        {
            RenderTexture.active = dest;
            float num = noise.width * 1f;
            float num2 = (1f * source.width) / TILE_AMOUNT;
            fxMaterial.SetTexture("_MainTex", source);
            GL.PushMatrix();
            GL.LoadOrtho();
            float num3 = (1f * source.width) / (1f * source.height);
            float num4 = 1f / num2;
            float num5 = num4 * num3;
            float num6 = num / (noise.width * 1f);
            fxMaterial.SetPass(passNr);
            GL.Begin(7);
            for (float i = 0f; i < 1f; i += num4)
            {
                for (float j = 0f; j < 1f; j += num5)
                {
                    float x = UnityEngine.Random.Range((float) 0f, (float) 1f);
                    float y = UnityEngine.Random.Range((float) 0f, (float) 1f);
                    x = Mathf.Floor(x * num) / num;
                    y = Mathf.Floor(y * num) / num;
                    float num11 = 1f / num;
                    GL.MultiTexCoord2(0, x, y);
                    GL.MultiTexCoord2(1, 0f, 0f);
                    GL.Vertex3(i, j, 0.1f);
                    GL.MultiTexCoord2(0, x + (num6 * num11), y);
                    GL.MultiTexCoord2(1, 1f, 0f);
                    GL.Vertex3(i + num4, j, 0.1f);
                    GL.MultiTexCoord2(0, x + (num6 * num11), y + (num6 * num11));
                    GL.MultiTexCoord2(1, 1f, 1f);
                    GL.Vertex3(i + num4, j + num5, 0.1f);
                    GL.MultiTexCoord2(0, x, y + (num6 * num11));
                    GL.MultiTexCoord2(1, 0f, 1f);
                    GL.Vertex3(i, j + num5, 0.1f);
                }
            }
            GL.End();
            GL.PopMatrix();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources() || (null == this.noiseTexture))
            {
                Graphics.Blit(source, destination);
                if (null == this.noiseTexture)
                {
                    Debug.LogWarning("Noise & Grain effect failing as noise texture is not assigned. please assign.", base.transform);
                }
            }
            else
            {
                this.softness = Mathf.Clamp(this.softness, 0f, 0.99f);
                if (this.dx11Grain && base.supportDX11)
                {
                    this.dx11NoiseMaterial.SetFloat("_DX11NoiseTime", (float) Time.frameCount);
                    this.dx11NoiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
                    this.dx11NoiseMaterial.SetVector("_NoisePerChannel", !this.monochrome ? this.intensities : Vector3.one);
                    this.dx11NoiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
                    this.dx11NoiseMaterial.SetVector("_NoiseAmount", (Vector4) (new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier));
                    if (this.softness > Mathf.Epsilon)
                    {
                        RenderTexture temporary = RenderTexture.GetTemporary((int) (source.width * (1f - this.softness)), (int) (source.height * (1f - this.softness)));
                        DrawNoiseQuadGrid(source, temporary, this.dx11NoiseMaterial, this.noiseTexture, !this.monochrome ? 2 : 3);
                        this.dx11NoiseMaterial.SetTexture("_NoiseTex", temporary);
                        Graphics.Blit(source, destination, this.dx11NoiseMaterial, 4);
                        RenderTexture.ReleaseTemporary(temporary);
                    }
                    else
                    {
                        DrawNoiseQuadGrid(source, destination, this.dx11NoiseMaterial, this.noiseTexture, !this.monochrome ? 0 : 1);
                    }
                }
                else
                {
                    if (this.noiseTexture != null)
                    {
                        this.noiseTexture.wrapMode = TextureWrapMode.Repeat;
                        this.noiseTexture.filterMode = this.filterMode;
                    }
                    this.noiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
                    this.noiseMaterial.SetVector("_NoisePerChannel", !this.monochrome ? this.intensities : Vector3.one);
                    this.noiseMaterial.SetVector("_NoiseTilingPerChannel", !this.monochrome ? ((Vector4) this.tiling) : ((Vector4) (Vector3.one * this.monochromeTiling)));
                    this.noiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
                    this.noiseMaterial.SetVector("_NoiseAmount", (Vector4) (new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier));
                    if (this.softness > Mathf.Epsilon)
                    {
                        RenderTexture dest = RenderTexture.GetTemporary((int) (source.width * (1f - this.softness)), (int) (source.height * (1f - this.softness)));
                        DrawNoiseQuadGrid(source, dest, this.noiseMaterial, this.noiseTexture, 2);
                        this.noiseMaterial.SetTexture("_NoiseTex", dest);
                        Graphics.Blit(source, destination, this.noiseMaterial, 1);
                        RenderTexture.ReleaseTemporary(dest);
                    }
                    else
                    {
                        DrawNoiseQuadGrid(source, destination, this.noiseMaterial, this.noiseTexture, 0);
                    }
                }
            }
        }
    }
}

