namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Obscurance"), ExecuteInEditMode]
    internal class ScreenSpaceAmbientObscurance : PostEffectsBase
    {
        private Material aoMaterial;
        public Shader aoShader;
        [Range(0f, 5f)]
        public float blurFilterDistance = 1.25f;
        [Range(0f, 3f)]
        public int blurIterations = 1;
        [Range(0f, 1f)]
        public int downsample;
        [Range(0f, 3f)]
        public float intensity = 0.5f;
        [Range(0.1f, 3f)]
        public float radius = 0.2f;
        public Texture2D rand;

        public override bool CheckResources()
        {
            base.CheckSupport(true);
            this.aoMaterial = base.CheckShaderAndCreateMaterial(this.aoShader, this.aoMaterial);
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private void OnDisable()
        {
            if (this.aoMaterial != null)
            {
                UnityEngine.Object.DestroyImmediate(this.aoMaterial);
            }
            this.aoMaterial = null;
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources())
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                RenderTexture texture2;
                Matrix4x4 projectionMatrix = base.GetComponent<Camera>().projectionMatrix;
                Matrix4x4 inverse = projectionMatrix.inverse;
                Vector4 vector = new Vector4(-2f / (Screen.width * projectionMatrix[0]), -2f / (Screen.height * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]);
                this.aoMaterial.SetVector("_ProjInfo", vector);
                this.aoMaterial.SetMatrix("_ProjectionInv", inverse);
                this.aoMaterial.SetTexture("_Rand", this.rand);
                this.aoMaterial.SetFloat("_Radius", this.radius);
                this.aoMaterial.SetFloat("_Radius2", this.radius * this.radius);
                this.aoMaterial.SetFloat("_Intensity", this.intensity);
                this.aoMaterial.SetFloat("_BlurFilterDistance", this.blurFilterDistance);
                int width = source.width;
                int height = source.height;
                RenderTexture temporary = RenderTexture.GetTemporary(width >> this.downsample, height >> this.downsample);
                Graphics.Blit(source, temporary, this.aoMaterial, 0);
                if (this.downsample > 0)
                {
                    texture2 = RenderTexture.GetTemporary(width, height);
                    Graphics.Blit(temporary, texture2, this.aoMaterial, 4);
                    RenderTexture.ReleaseTemporary(temporary);
                    temporary = texture2;
                }
                for (int i = 0; i < this.blurIterations; i++)
                {
                    this.aoMaterial.SetVector("_Axis", new Vector2(1f, 0f));
                    texture2 = RenderTexture.GetTemporary(width, height);
                    Graphics.Blit(temporary, texture2, this.aoMaterial, 1);
                    RenderTexture.ReleaseTemporary(temporary);
                    this.aoMaterial.SetVector("_Axis", new Vector2(0f, 1f));
                    temporary = RenderTexture.GetTemporary(width, height);
                    Graphics.Blit(texture2, temporary, this.aoMaterial, 1);
                    RenderTexture.ReleaseTemporary(texture2);
                }
                this.aoMaterial.SetTexture("_AOTex", temporary);
                Graphics.Blit(source, destination, this.aoMaterial, 2);
                RenderTexture.ReleaseTemporary(temporary);
            }
        }
    }
}

