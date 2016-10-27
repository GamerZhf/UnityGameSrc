namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Camera)), ExecuteInEditMode, AddComponentMenu("Image Effects/Rendering/Global Fog")]
    internal class GlobalFog : PostEffectsBase
    {
        [Tooltip("Apply distance-based fog?")]
        public bool distanceFog = true;
        private Material fogMaterial;
        public Shader fogShader;
        [Tooltip("Fog top Y coordinate")]
        public float height = 1f;
        [Range(0.001f, 10f)]
        public float heightDensity = 2f;
        [Tooltip("Apply height-based fog?")]
        public bool heightFog = true;
        [Tooltip("Push fog away from the camera by this amount")]
        public float startDistance;
        [Tooltip("Distance fog is based on radial distance from camera when checked")]
        public bool useRadialDistance;

        public override bool CheckResources()
        {
            base.CheckSupport(true);
            this.fogMaterial = base.CheckShaderAndCreateMaterial(this.fogShader, this.fogMaterial);
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
        {
            RenderTexture.active = dest;
            fxMaterial.SetTexture("_MainTex", source);
            GL.PushMatrix();
            GL.LoadOrtho();
            fxMaterial.SetPass(passNr);
            GL.Begin(7);
            GL.MultiTexCoord2(0, 0f, 0f);
            GL.Vertex3(0f, 0f, 3f);
            GL.MultiTexCoord2(0, 1f, 0f);
            GL.Vertex3(1f, 0f, 2f);
            GL.MultiTexCoord2(0, 1f, 1f);
            GL.Vertex3(1f, 1f, 1f);
            GL.MultiTexCoord2(0, 0f, 1f);
            GL.Vertex3(0f, 1f, 0f);
            GL.End();
            GL.PopMatrix();
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!this.CheckResources() || (!this.distanceFog && !this.heightFog))
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                Vector4 vector8;
                Camera component = base.GetComponent<Camera>();
                Transform transform = component.transform;
                float nearClipPlane = component.nearClipPlane;
                float farClipPlane = component.farClipPlane;
                float fieldOfView = component.fieldOfView;
                float aspect = component.aspect;
                Matrix4x4 identity = Matrix4x4.identity;
                float num5 = fieldOfView * 0.5f;
                Vector3 vector = (Vector3) (((transform.right * nearClipPlane) * Mathf.Tan(num5 * 0.01745329f)) * aspect);
                Vector3 vector2 = (Vector3) ((transform.up * nearClipPlane) * Mathf.Tan(num5 * 0.01745329f));
                Vector3 v = (((Vector3) (transform.forward * nearClipPlane)) - vector) + vector2;
                float num6 = (v.magnitude * farClipPlane) / nearClipPlane;
                v.Normalize();
                v = (Vector3) (v * num6);
                Vector3 vector4 = (((Vector3) (transform.forward * nearClipPlane)) + vector) + vector2;
                vector4.Normalize();
                vector4 = (Vector3) (vector4 * num6);
                Vector3 vector5 = (((Vector3) (transform.forward * nearClipPlane)) + vector) - vector2;
                vector5.Normalize();
                vector5 = (Vector3) (vector5 * num6);
                Vector3 vector6 = (((Vector3) (transform.forward * nearClipPlane)) - vector) - vector2;
                vector6.Normalize();
                vector6 = (Vector3) (vector6 * num6);
                identity.SetRow(0, v);
                identity.SetRow(1, vector4);
                identity.SetRow(2, vector5);
                identity.SetRow(3, vector6);
                Vector3 position = transform.position;
                float y = position.y - this.height;
                float z = (y > 0f) ? 0f : 1f;
                this.fogMaterial.SetMatrix("_FrustumCornersWS", identity);
                this.fogMaterial.SetVector("_CameraWS", position);
                this.fogMaterial.SetVector("_HeightParams", new Vector4(this.height, y, z, this.heightDensity * 0.5f));
                this.fogMaterial.SetVector("_DistanceParams", new Vector4(-Mathf.Max(this.startDistance, 0f), 0f, 0f, 0f));
                FogMode fogMode = RenderSettings.fogMode;
                float fogDensity = RenderSettings.fogDensity;
                float fogStartDistance = RenderSettings.fogStartDistance;
                float fogEndDistance = RenderSettings.fogEndDistance;
                bool flag = fogMode == FogMode.Linear;
                float f = !flag ? 0f : (fogEndDistance - fogStartDistance);
                float num13 = (Mathf.Abs(f) <= 0.0001f) ? 0f : (1f / f);
                vector8.x = fogDensity * 1.201122f;
                vector8.y = fogDensity * 1.442695f;
                vector8.z = !flag ? 0f : -num13;
                vector8.w = !flag ? 0f : (fogEndDistance * num13);
                this.fogMaterial.SetVector("_SceneFogParams", vector8);
                this.fogMaterial.SetVector("_SceneFogMode", new Vector4((float) fogMode, !this.useRadialDistance ? ((float) 0) : ((float) 1), 0f, 0f));
                int passNr = 0;
                if (this.distanceFog && this.heightFog)
                {
                    passNr = 0;
                }
                else if (this.distanceFog)
                {
                    passNr = 1;
                }
                else
                {
                    passNr = 2;
                }
                CustomGraphicsBlit(source, destination, this.fogMaterial, passNr);
            }
        }
    }
}

