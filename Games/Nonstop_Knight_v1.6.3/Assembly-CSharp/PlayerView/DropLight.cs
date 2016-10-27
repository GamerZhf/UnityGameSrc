namespace PlayerView
{
    using App;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(UnityEngine.SpriteRenderer))]
    public class DropLight : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.SpriteRenderer <SpriteRenderer>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public UnityEngine.Color Color = new UnityEngine.Color(1f, 0.5647059f, 0f, 1f);
        public float LightIntensity = 1f;
        public bool LockColor;
        public float LockedWorldPosY = float.MinValue;
        public bool LockIntensity;
        private bool m_initialized;
        public bool OverrideLowEndDeviceRestriction;

        protected void Awake()
        {
            this.Tm = base.transform;
            this.SpriteRenderer = GameObjectExtensions.AddOrGetComponent<UnityEngine.SpriteRenderer>(base.gameObject);
            this.Tm.localRotation = Quaternion.Euler(90f, 0f, 0f);
            this.initialize();
        }

        private void initialize()
        {
            if (PlayerView.Binder.SpriteResources != null)
            {
                this.SpriteRenderer.sprite = PlayerView.Binder.SpriteResources.getSprite("Gameplay", "sprite_light_source_additive");
                this.SpriteRenderer.material = Resources.Load<Material>("Materials/fx_droplight");
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
                this.SpriteRenderer.color = (UnityEngine.Color) (this.Color * this.LightIntensity);
                if (this.LockedWorldPosY > float.MinValue)
                {
                    this.Tm.position = new Vector3(this.Tm.position.x, this.LockedWorldPosY, this.Tm.position.z);
                }
            }
        }

        public void setVisible(bool visible)
        {
            if (!this.OverrideLowEndDeviceRestriction && (ConfigDevice.DeviceQuality() <= DeviceQualityType.Med))
            {
                this.SpriteRenderer.enabled = false;
            }
            else
            {
                this.SpriteRenderer.enabled = visible;
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

