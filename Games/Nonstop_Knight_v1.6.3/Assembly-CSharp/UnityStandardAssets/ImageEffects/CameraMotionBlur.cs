namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Image Effects/Camera/Camera Motion Blur"), RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class CameraMotionBlur : PostEffectsBase
    {
        private Camera _camera;
        private Matrix4x4 currentViewProjMat;
        private Material dx11MotionBlurMaterial;
        public Shader dx11MotionBlurShader;
        public LayerMask excludeLayers = 0;
        public MotionBlurFilter filterType = MotionBlurFilter.Reconstruction;
        public float jitter = 0.05f;
        private static float MAX_RADIUS = 10f;
        public float maxVelocity = 8f;
        public float minVelocity = 0.1f;
        private Material motionBlurMaterial;
        public float movementScale;
        public Texture2D noiseTexture;
        private int prevFrameCount;
        private Vector3 prevFrameForward = Vector3.forward;
        private Vector3 prevFramePos = Vector3.zero;
        private Vector3 prevFrameUp = Vector3.up;
        public bool preview;
        public Vector3 previewScale = Vector3.one;
        private Matrix4x4 prevViewProjMat;
        public Shader replacementClear;
        public float rotationScale = 1f;
        public Shader shader;
        public bool showVelocity;
        public float showVelocityScale = 1f;
        public float softZDistance = 0.005f;
        private GameObject tmpCam;
        public int velocityDownsample = 1;
        public float velocityScale = 0.375f;
        private bool wasActive;

        private void CalculateViewProjection()
        {
            Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
            Matrix4x4 gPUProjectionMatrix = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true);
            this.currentViewProjMat = gPUProjectionMatrix * worldToCameraMatrix;
        }

        public override bool CheckResources()
        {
            base.CheckSupport(true, true);
            this.motionBlurMaterial = base.CheckShaderAndCreateMaterial(this.shader, this.motionBlurMaterial);
            if (base.supportDX11 && (this.filterType == MotionBlurFilter.ReconstructionDX11))
            {
                this.dx11MotionBlurMaterial = base.CheckShaderAndCreateMaterial(this.dx11MotionBlurShader, this.dx11MotionBlurMaterial);
            }
            if (!base.isSupported)
            {
                base.ReportAutoDisable();
            }
            return base.isSupported;
        }

        private static int divRoundUp(int x, int d)
        {
            return (((x + d) - 1) / d);
        }

        private Camera GetTmpCam()
        {
            if (this.tmpCam == null)
            {
                string name = "_" + this._camera.name + "_MotionBlurTmpCam";
                GameObject obj2 = GameObject.Find(name);
                if (null == obj2)
                {
                    System.Type[] components = new System.Type[] { typeof(Camera) };
                    this.tmpCam = new GameObject(name, components);
                }
                else
                {
                    this.tmpCam = obj2;
                }
            }
            this.tmpCam.hideFlags = HideFlags.DontSave;
            this.tmpCam.transform.position = this._camera.transform.position;
            this.tmpCam.transform.rotation = this._camera.transform.rotation;
            this.tmpCam.transform.localScale = this._camera.transform.localScale;
            this.tmpCam.GetComponent<Camera>().CopyFrom(this._camera);
            this.tmpCam.GetComponent<Camera>().enabled = false;
            this.tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
            this.tmpCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
            return this.tmpCam.GetComponent<Camera>();
        }

        private void OnDisable()
        {
            if (null != this.motionBlurMaterial)
            {
                UnityEngine.Object.DestroyImmediate(this.motionBlurMaterial);
                this.motionBlurMaterial = null;
            }
            if (null != this.dx11MotionBlurMaterial)
            {
                UnityEngine.Object.DestroyImmediate(this.dx11MotionBlurMaterial);
                this.dx11MotionBlurMaterial = null;
            }
            if (null != this.tmpCam)
            {
                UnityEngine.Object.DestroyImmediate(this.tmpCam);
                this.tmpCam = null;
            }
        }

        private void OnEnable()
        {
            if (this._camera == null)
            {
                this._camera = base.GetComponent<Camera>();
            }
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
                if (this.filterType == MotionBlurFilter.CameraMotion)
                {
                    this.StartFrame();
                }
                RenderTextureFormat format = !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.RGHalf;
                RenderTexture texture = RenderTexture.GetTemporary(divRoundUp(source.width, this.velocityDownsample), divRoundUp(source.height, this.velocityDownsample), 0, format);
                int width = 1;
                int height = 1;
                this.maxVelocity = Mathf.Max(2f, this.maxVelocity);
                float maxVelocity = this.maxVelocity;
                bool flag = (this.filterType == MotionBlurFilter.ReconstructionDX11) && (this.dx11MotionBlurMaterial == null);
                if (((this.filterType == MotionBlurFilter.Reconstruction) || flag) || (this.filterType == MotionBlurFilter.ReconstructionDisc))
                {
                    this.maxVelocity = Mathf.Min(this.maxVelocity, MAX_RADIUS);
                    width = divRoundUp(texture.width, (int) this.maxVelocity);
                    height = divRoundUp(texture.height, (int) this.maxVelocity);
                    maxVelocity = texture.width / width;
                }
                else
                {
                    width = divRoundUp(texture.width, (int) this.maxVelocity);
                    height = divRoundUp(texture.height, (int) this.maxVelocity);
                    maxVelocity = texture.width / width;
                }
                RenderTexture texture2 = RenderTexture.GetTemporary(width, height, 0, format);
                RenderTexture texture3 = RenderTexture.GetTemporary(width, height, 0, format);
                texture.filterMode = FilterMode.Point;
                texture2.filterMode = FilterMode.Point;
                texture3.filterMode = FilterMode.Point;
                if (this.noiseTexture != null)
                {
                    this.noiseTexture.filterMode = FilterMode.Point;
                }
                source.wrapMode = TextureWrapMode.Clamp;
                texture.wrapMode = TextureWrapMode.Clamp;
                texture3.wrapMode = TextureWrapMode.Clamp;
                texture2.wrapMode = TextureWrapMode.Clamp;
                this.CalculateViewProjection();
                if (base.gameObject.activeInHierarchy && !this.wasActive)
                {
                    this.Remember();
                }
                this.wasActive = base.gameObject.activeInHierarchy;
                Matrix4x4 matrix = Matrix4x4.Inverse(this.currentViewProjMat);
                this.motionBlurMaterial.SetMatrix("_InvViewProj", matrix);
                this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
                this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix);
                this.motionBlurMaterial.SetFloat("_MaxVelocity", maxVelocity);
                this.motionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", maxVelocity);
                this.motionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
                this.motionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
                this.motionBlurMaterial.SetFloat("_Jitter", this.jitter);
                this.motionBlurMaterial.SetTexture("_NoiseTex", this.noiseTexture);
                this.motionBlurMaterial.SetTexture("_VelTex", texture);
                this.motionBlurMaterial.SetTexture("_NeighbourMaxTex", texture3);
                this.motionBlurMaterial.SetTexture("_TileTexDebug", texture2);
                if (this.preview)
                {
                    Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
                    Matrix4x4 identity = Matrix4x4.identity;
                    identity.SetTRS((Vector3) (this.previewScale * 0.3333f), Quaternion.identity, Vector3.one);
                    Matrix4x4 gPUProjectionMatrix = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true);
                    this.prevViewProjMat = (gPUProjectionMatrix * identity) * worldToCameraMatrix;
                    this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
                    this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix);
                }
                if (this.filterType == MotionBlurFilter.CameraMotion)
                {
                    Vector4 zero = Vector4.zero;
                    float num4 = Vector3.Dot(base.transform.up, Vector3.up);
                    Vector3 rhs = this.prevFramePos - base.transform.position;
                    float magnitude = rhs.magnitude;
                    float num6 = 1f;
                    num6 = (Vector3.Angle(base.transform.up, this.prevFrameUp) / this._camera.fieldOfView) * (source.width * 0.75f);
                    zero.x = this.rotationScale * num6;
                    num6 = (Vector3.Angle(base.transform.forward, this.prevFrameForward) / this._camera.fieldOfView) * (source.width * 0.75f);
                    zero.y = (this.rotationScale * num4) * num6;
                    num6 = (Vector3.Angle(base.transform.forward, this.prevFrameForward) / this._camera.fieldOfView) * (source.width * 0.75f);
                    zero.z = (this.rotationScale * (1f - num4)) * num6;
                    if ((magnitude > Mathf.Epsilon) && (this.movementScale > Mathf.Epsilon))
                    {
                        zero.w = (this.movementScale * Vector3.Dot(base.transform.forward, rhs)) * (source.width * 0.5f);
                        zero.x += (this.movementScale * Vector3.Dot(base.transform.up, rhs)) * (source.width * 0.5f);
                        zero.y += (this.movementScale * Vector3.Dot(base.transform.right, rhs)) * (source.width * 0.5f);
                    }
                    if (this.preview)
                    {
                        this.motionBlurMaterial.SetVector("_BlurDirectionPacked", (Vector4) ((new Vector4(this.previewScale.y, this.previewScale.x, 0f, this.previewScale.z) * 0.5f) * this._camera.fieldOfView));
                    }
                    else
                    {
                        this.motionBlurMaterial.SetVector("_BlurDirectionPacked", zero);
                    }
                }
                else
                {
                    Graphics.Blit(source, texture, this.motionBlurMaterial, 0);
                    Camera tmpCam = null;
                    if (this.excludeLayers.value != 0)
                    {
                        tmpCam = this.GetTmpCam();
                    }
                    if (((tmpCam != null) && (this.excludeLayers.value != 0)) && ((this.replacementClear != null) && this.replacementClear.isSupported))
                    {
                        tmpCam.targetTexture = texture;
                        tmpCam.cullingMask = (int) this.excludeLayers;
                        tmpCam.RenderWithShader(this.replacementClear, string.Empty);
                    }
                }
                if (!this.preview && (Time.frameCount != this.prevFrameCount))
                {
                    this.prevFrameCount = Time.frameCount;
                    this.Remember();
                }
                source.filterMode = FilterMode.Bilinear;
                if (this.showVelocity)
                {
                    this.motionBlurMaterial.SetFloat("_DisplayVelocityScale", this.showVelocityScale);
                    Graphics.Blit(texture, destination, this.motionBlurMaterial, 1);
                }
                else if ((this.filterType == MotionBlurFilter.ReconstructionDX11) && !flag)
                {
                    this.dx11MotionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
                    this.dx11MotionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
                    this.dx11MotionBlurMaterial.SetFloat("_Jitter", this.jitter);
                    this.dx11MotionBlurMaterial.SetTexture("_NoiseTex", this.noiseTexture);
                    this.dx11MotionBlurMaterial.SetTexture("_VelTex", texture);
                    this.dx11MotionBlurMaterial.SetTexture("_NeighbourMaxTex", texture3);
                    this.dx11MotionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
                    this.dx11MotionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", maxVelocity);
                    Graphics.Blit(texture, texture2, this.dx11MotionBlurMaterial, 0);
                    Graphics.Blit(texture2, texture3, this.dx11MotionBlurMaterial, 1);
                    Graphics.Blit(source, destination, this.dx11MotionBlurMaterial, 2);
                }
                else if ((this.filterType == MotionBlurFilter.Reconstruction) || flag)
                {
                    this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
                    Graphics.Blit(texture, texture2, this.motionBlurMaterial, 2);
                    Graphics.Blit(texture2, texture3, this.motionBlurMaterial, 3);
                    Graphics.Blit(source, destination, this.motionBlurMaterial, 4);
                }
                else if (this.filterType == MotionBlurFilter.CameraMotion)
                {
                    Graphics.Blit(source, destination, this.motionBlurMaterial, 6);
                }
                else if (this.filterType == MotionBlurFilter.ReconstructionDisc)
                {
                    this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
                    Graphics.Blit(texture, texture2, this.motionBlurMaterial, 2);
                    Graphics.Blit(texture2, texture3, this.motionBlurMaterial, 3);
                    Graphics.Blit(source, destination, this.motionBlurMaterial, 7);
                }
                else
                {
                    Graphics.Blit(source, destination, this.motionBlurMaterial, 5);
                }
                RenderTexture.ReleaseTemporary(texture);
                RenderTexture.ReleaseTemporary(texture2);
                RenderTexture.ReleaseTemporary(texture3);
            }
        }

        private void Remember()
        {
            this.prevViewProjMat = this.currentViewProjMat;
            this.prevFrameForward = base.transform.forward;
            this.prevFrameUp = base.transform.up;
            this.prevFramePos = base.transform.position;
        }

        private void Start()
        {
            this.CheckResources();
            if (this._camera == null)
            {
                this._camera = base.GetComponent<Camera>();
            }
            this.wasActive = base.gameObject.activeInHierarchy;
            this.CalculateViewProjection();
            this.Remember();
            this.wasActive = false;
        }

        private void StartFrame()
        {
            this.prevFramePos = Vector3.Slerp(this.prevFramePos, base.transform.position, 0.75f);
        }

        public enum MotionBlurFilter
        {
            CameraMotion,
            LocalBlur,
            Reconstruction,
            ReconstructionDX11,
            ReconstructionDisc
        }
    }
}

