namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CharacterMenuCamera : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Camera <Camera>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.RenderTexture <RenderTexture>k__BackingField;
        [CompilerGenerated]
        private MenuCharacterView <Target>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        private Quaternion m_defaultLocalRotation;

        protected void Awake()
        {
            this.Transform = base.transform;
            this.Camera = base.GetComponent<UnityEngine.Camera>();
            this.m_defaultLocalRotation = this.Transform.localRotation;
        }

        public void initializeRenderTexture()
        {
            this.releaseRenderTexture();
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                this.RenderTexture = new UnityEngine.RenderTexture(0x200, 0x200, 0x18, RenderTextureFormat.ARGB32);
            }
            else
            {
                this.RenderTexture = new UnityEngine.RenderTexture(0x500, 0x500, 0x18, RenderTextureFormat.ARGB32);
            }
            this.RenderTexture.filterMode = FilterMode.Bilinear;
            this.Camera.targetTexture = this.RenderTexture;
        }

        protected void LateUpdate()
        {
            if ((this.Target != null) && (this.Target.CharacterView != null))
            {
                Vector3 zero = Vector3.zero;
                bool flag = false;
                switch (this.Target.CharacterView.CharacterPrefab)
                {
                    case CharacterPrefab.PetCrab1:
                    case CharacterPrefab.PetCrab2:
                    case CharacterPrefab.PetBeaver1:
                        zero = new Vector3(0f, 0.35f, 9.5f);
                        break;

                    case CharacterPrefab.PetSquid1:
                    case CharacterPrefab.PetSquid2:
                        zero = new Vector3(0f, 1.75f, 10f);
                        break;

                    case CharacterPrefab.PetYeti1:
                        zero = new Vector3(0f, 0.4f, 7f);
                        break;

                    case CharacterPrefab.PetWalrus1:
                        zero = new Vector3(0f, 0.25f, 7.5f);
                        break;

                    case CharacterPrefab.PetShark1:
                    case CharacterPrefab.PetShark2:
                        zero = new Vector3(0f, 1.2f, 8f);
                        break;

                    case CharacterPrefab.PetStumpy1:
                        zero = new Vector3(0f, 0.3f, 8f);
                        break;

                    case CharacterPrefab.PetStumpy2:
                        zero = new Vector3(0f, 0.7f, 8f);
                        break;

                    case CharacterPrefab.PetDog1:
                        zero = new Vector3(0f, 0.25f, 9f);
                        break;

                    case CharacterPrefab.PetMoose1:
                    case CharacterPrefab.PetRam1:
                        zero = new Vector3(0f, 0.25f, 8.5f);
                        break;

                    case CharacterPrefab.PetChest1:
                        zero = new Vector3(0f, 0.2f, 9.5f);
                        break;

                    case CharacterPrefab.PetPanda1:
                        zero = new Vector3(0f, 0.45f, 8.5f);
                        break;

                    case CharacterPrefab.KnightMale:
                    case CharacterPrefab.KnightFemale:
                        zero = new Vector3(0f, 1f, 8f);
                        flag = true;
                        break;

                    case CharacterPrefab.PetDragon1:
                    case CharacterPrefab.PetDragon2:
                        zero = new Vector3(0f, 1.75f, 8f);
                        break;

                    default:
                        zero = new Vector3(0f, 0.7f, 8f);
                        break;
                }
                if (flag)
                {
                    this.Transform.position = this.Target.CharacterView.Transform.position + zero;
                    this.Transform.localRotation = this.m_defaultLocalRotation;
                }
                else
                {
                    Vector3 vector2 = this.Target.CharacterView.Transform.position + new Vector3(0f, zero.y, 0f);
                    Vector3 vector3 = vector2 + new Vector3(0f, 2f, zero.z);
                    Quaternion quaternion = Quaternion.LookRotation(vector2 - vector3, Vector3.up);
                    this.Transform.position = vector3;
                    this.Transform.rotation = quaternion;
                }
            }
        }

        protected void OnDestroy()
        {
            this.releaseRenderTexture();
        }

        protected void OnDisable()
        {
            this.releaseRenderTexture();
        }

        protected void OnEnable()
        {
            this.initializeRenderTexture();
        }

        private void releaseRenderTexture()
        {
            if (this.RenderTexture != null)
            {
                this.RenderTexture.Release();
                UnityEngine.Object.Destroy(this.RenderTexture);
                this.RenderTexture = null;
            }
            this.Camera.targetTexture = null;
        }

        public UnityEngine.Camera Camera
        {
            [CompilerGenerated]
            get
            {
                return this.<Camera>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Camera>k__BackingField = value;
            }
        }

        public UnityEngine.RenderTexture RenderTexture
        {
            [CompilerGenerated]
            get
            {
                return this.<RenderTexture>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RenderTexture>k__BackingField = value;
            }
        }

        public MenuCharacterView Target
        {
            [CompilerGenerated]
            get
            {
                return this.<Target>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Target>k__BackingField = value;
            }
        }

        public UnityEngine.Transform Transform
        {
            [CompilerGenerated]
            get
            {
                return this.<Transform>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Transform>k__BackingField = value;
            }
        }
    }
}

