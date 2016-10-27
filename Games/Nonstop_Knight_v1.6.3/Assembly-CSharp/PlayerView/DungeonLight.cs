namespace PlayerView
{
    using App;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(PlayerView.Billboard)), RequireComponent(typeof(UnityEngine.SpriteRenderer))]
    public class DungeonLight : MonoBehaviour
    {
        [CompilerGenerated]
        private PlayerView.Billboard <Billboard>k__BackingField;
        [CompilerGenerated]
        private Transform <CamTm>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.SpriteRenderer <SpriteRenderer>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public UnityEngine.Color Color = new UnityEngine.Color(1f, 0.5647059f, 0f, 1f);
        [NonSerialized]
        public float FlickerAmplitude = 0.3f;
        [NonSerialized]
        public float FlickerFrequency = 2f;
        public float LightIntensity = 1f;
        public float LightRange = 2f;
        public bool LockColor;
        public bool LockIntensity;
        private bool m_initialized;
        private float m_randFlickerOffset;

        protected void Awake()
        {
            this.Tm = base.transform;
            this.SpriteRenderer = GameObjectExtensions.AddOrGetComponent<UnityEngine.SpriteRenderer>(base.gameObject);
            this.Billboard = GameObjectExtensions.AddOrGetComponent<PlayerView.Billboard>(base.gameObject);
            this.m_randFlickerOffset = UnityEngine.Random.Range((float) 0f, (float) 100f);
            this.initialize();
        }

        private void initialize()
        {
            if (PlayerView.Binder.SpriteResources != null)
            {
                this.SpriteRenderer.sprite = PlayerView.Binder.SpriteResources.getSprite("Gameplay", "sprite_light_source_additive");
                this.SpriteRenderer.material = Resources.Load<Material>("Materials/fx_dungeonlight");
                if (PlayerView.Binder.RoomView != null)
                {
                    this.Billboard.Camera = PlayerView.Binder.RoomView.RoomCamera.Camera;
                    this.CamTm = this.Billboard.Camera.transform;
                }
                this.Billboard.Reversed = true;
                this.Billboard.ManualUpdate = true;
                this.setVisible(false);
                this.m_initialized = true;
            }
        }

        protected void LateUpdate()
        {
            if (!this.m_initialized)
            {
                this.initialize();
            }
            if (this.m_initialized && this.SpriteRenderer.enabled)
            {
                float x = (Time.time * this.FlickerFrequency) + this.m_randFlickerOffset;
                this.Tm.localScale = (Vector3) ((Vector3.one * this.LightRange) + ((Vector3.one * (Mathf.PerlinNoise(x, x) - 0.5f)) * this.FlickerAmplitude));
                this.SpriteRenderer.color = (UnityEngine.Color) (this.Color * this.LightIntensity);
                this.Billboard.update(Time.deltaTime);
            }
        }

        public void setVisible(bool visible)
        {
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                this.SpriteRenderer.enabled = false;
                this.Billboard.enabled = false;
            }
            else
            {
                this.SpriteRenderer.enabled = visible;
                this.Billboard.enabled = visible;
            }
        }

        public PlayerView.Billboard Billboard
        {
            [CompilerGenerated]
            get
            {
                return this.<Billboard>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Billboard>k__BackingField = value;
            }
        }

        public Transform CamTm
        {
            [CompilerGenerated]
            get
            {
                return this.<CamTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CamTm>k__BackingField = value;
            }
        }

        public UnityEngine.SpriteRenderer SpriteRenderer
        {
            [CompilerGenerated]
            get
            {
                return this.<SpriteRenderer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SpriteRenderer>k__BackingField = value;
            }
        }

        public Transform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }
    }
}

