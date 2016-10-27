namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraBackdrop : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Camera <Camera>k__BackingField;
        [CompilerGenerated]
        private Billboard <ChildBillboard>k__BackingField;
        [CompilerGenerated]
        private SpriteRenderer <ChildSpriteRenderer>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <ChildTm>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        public float BackdropScale = 13f;
        public Sprite BackdropSprite;
        public float DistanceFromCamera = 100f;
        private Vector2 m_parallaxOffset;
        private Vector3 m_prevFrameLocalPosition;
        private Quaternion m_rotation = Quaternion.identity;
        public float ParallaxFactor = 0.2f;

        protected void Awake()
        {
            this.Transform = base.transform;
            this.Camera = base.GetComponent<UnityEngine.Camera>();
        }

        protected void LateUpdate()
        {
            this.update();
        }

        protected void OnDestroy()
        {
            UnityEngine.Object.Destroy(this.ChildTm);
        }

        protected void OnDisable()
        {
            if (this.ChildTm != null)
            {
                this.ChildTm.gameObject.SetActive(false);
            }
        }

        protected void OnEnable()
        {
            if (this.ChildTm != null)
            {
                this.ChildTm.gameObject.SetActive(true);
            }
        }

        [ContextMenu("reset()")]
        public void reset()
        {
            this.m_prevFrameLocalPosition = this.Transform.position;
            this.m_parallaxOffset = Vector2.zero;
            this.m_rotation = this.Transform.rotation;
            this.update();
        }

        protected void Start()
        {
            this.ChildTm = new GameObject("BackdropSprite").transform;
            UnityEngine.Object.DontDestroyOnLoad(this.ChildTm);
            this.ChildTm.SetParent(this.Transform.parent, true);
            this.ChildSpriteRenderer = this.ChildTm.gameObject.AddComponent<SpriteRenderer>();
            this.ChildSpriteRenderer.sprite = this.BackdropSprite;
            this.ChildBillboard = this.ChildTm.gameObject.AddComponent<Billboard>();
            this.ChildBillboard.Camera = this.Camera;
            this.ChildBillboard.ManualUpdate = true;
            this.reset();
        }

        private void update()
        {
            if (this.ChildTm != null)
            {
                this.ChildTm.localScale = (Vector3) (Vector3.one * this.BackdropScale);
                this.ChildTm.rotation = this.m_rotation;
                Vector3 vector = this.Transform.localPosition - this.m_prevFrameLocalPosition;
                this.m_parallaxOffset.x -= vector.x * this.ParallaxFactor;
                this.m_parallaxOffset.y -= vector.z * this.ParallaxFactor;
                this.m_prevFrameLocalPosition = this.Transform.localPosition;
                this.ChildTm.position = this.Transform.position + this.Transform.TransformVector(new Vector3(this.m_parallaxOffset.x, this.m_parallaxOffset.y, this.DistanceFromCamera));
            }
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

        public Billboard ChildBillboard
        {
            [CompilerGenerated]
            get
            {
                return this.<ChildBillboard>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ChildBillboard>k__BackingField = value;
            }
        }

        public SpriteRenderer ChildSpriteRenderer
        {
            [CompilerGenerated]
            get
            {
                return this.<ChildSpriteRenderer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ChildSpriteRenderer>k__BackingField = value;
            }
        }

        public UnityEngine.Transform ChildTm
        {
            [CompilerGenerated]
            get
            {
                return this.<ChildTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ChildTm>k__BackingField = value;
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

