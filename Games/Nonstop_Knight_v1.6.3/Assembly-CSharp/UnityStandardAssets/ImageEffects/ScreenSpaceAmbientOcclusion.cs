namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Occlusion"), RequireComponent(typeof(Camera))]
    public class ScreenSpaceAmbientOcclusion : MonoBehaviour
    {
        public int m_Blur = 2;
        public int m_Downsampling = 2;
        public float m_MinZ = 0.01f;
        public float m_OcclusionAttenuation = 1f;
        public float m_OcclusionIntensity = 1.5f;
        public float m_Radius = 0.4f;
        public Texture2D m_RandomTexture;
        public SSAOSamples m_SampleCount = SSAOSamples.Medium;
        private Material m_SSAOMaterial;
        public Shader m_SSAOShader;
        private bool m_Supported;

        private static Material CreateMaterial(Shader shader)
        {
            if (shader == null)
            {
                return null;
            }
            Material material = new Material(shader);
            material.hideFlags = HideFlags.HideAndDontSave;
            return material;
        }

        private void CreateMaterials()
        {
            if ((this.m_SSAOMaterial == null) && this.m_SSAOShader.isSupported)
            {
                this.m_SSAOMaterial = CreateMaterial(this.m_SSAOShader);
                this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
            }
        }

        private static void DestroyMaterial(Material mat)
        {
            if (mat != null)
            {
                UnityEngine.Object.DestroyImmediate(mat);
                mat = null;
            }
        }

        private void OnDisable()
        {
            DestroyMaterial(this.m_SSAOMaterial);
        }

        private void OnEnable()
        {
            Camera component = base.GetComponent<Camera>();
            component.depthTextureMode |= DepthTextureMode.DepthNormals;
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.m_Supported || !this.m_SSAOShader.isSupported)
            {
                base.enabled = false;
            }
            else
            {
                int width;
                int height;
                this.CreateMaterials();
                this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
                this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
                this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
                this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
                this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
                this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
                RenderTexture dest = RenderTexture.GetTemporary(source.width / this.m_Downsampling, source.height / this.m_Downsampling, 0);
                float fieldOfView = base.GetComponent<Camera>().fieldOfView;
                float farClipPlane = base.GetComponent<Camera>().farClipPlane;
                float y = Mathf.Tan((fieldOfView * 0.01745329f) * 0.5f) * farClipPlane;
                float x = y * base.GetComponent<Camera>().aspect;
                this.m_SSAOMaterial.SetVector("_FarCorner", new Vector3(x, y, farClipPlane));
                if (this.m_RandomTexture != null)
                {
                    width = this.m_RandomTexture.width;
                    height = this.m_RandomTexture.height;
                }
                else
                {
                    width = 1;
                    height = 1;
                }
                this.m_SSAOMaterial.SetVector("_NoiseScale", new Vector3(((float) dest.width) / ((float) width), ((float) dest.height) / ((float) height), 0f));
                this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
                bool flag = this.m_Blur > 0;
                Graphics.Blit(!flag ? source : null, dest, this.m_SSAOMaterial, (int) this.m_SampleCount);
                if (flag)
                {
                    RenderTexture texture2 = RenderTexture.GetTemporary(source.width, source.height, 0);
                    this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(((float) this.m_Blur) / ((float) source.width), 0f, 0f, 0f));
                    this.m_SSAOMaterial.SetTexture("_SSAO", dest);
                    Graphics.Blit(null, texture2, this.m_SSAOMaterial, 3);
                    RenderTexture.ReleaseTemporary(dest);
                    RenderTexture texture3 = RenderTexture.GetTemporary(source.width, source.height, 0);
                    this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, ((float) this.m_Blur) / ((float) source.height), 0f, 0f));
                    this.m_SSAOMaterial.SetTexture("_SSAO", texture2);
                    Graphics.Blit(source, texture3, this.m_SSAOMaterial, 3);
                    RenderTexture.ReleaseTemporary(texture2);
                    dest = texture3;
                }
                this.m_SSAOMaterial.SetTexture("_SSAO", dest);
                Graphics.Blit(source, destination, this.m_SSAOMaterial, 4);
                RenderTexture.ReleaseTemporary(dest);
            }
        }

        private void Start()
        {
            if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            {
                this.m_Supported = false;
                base.enabled = false;
            }
            else
            {
                this.CreateMaterials();
                if ((this.m_SSAOMaterial == null) || (this.m_SSAOMaterial.passCount != 5))
                {
                    this.m_Supported = false;
                    base.enabled = false;
                }
                else
                {
                    this.m_Supported = true;
                }
            }
        }

        public enum SSAOSamples
        {
            Low,
            Medium,
            High
        }
    }
}

