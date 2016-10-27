namespace PlayerView
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CameraOccluder : MonoBehaviour
    {
        [CompilerGenerated]
        private float <Alpha>k__BackingField;
        [CompilerGenerated]
        private bool <Initialized>k__BackingField;
        [CompilerGenerated]
        private GameObject <OcclusionColliderChild>k__BackingField;
        [CompilerGenerated]
        private MeshRenderer[] <Renderers>k__BackingField;
        public const float COLLIDER_SCALE_FACTOR = 1.2f;
        private Coroutine m_animationRoutine;
        private Dictionary<MeshRenderer, Material> m_origMaterials = new Dictionary<MeshRenderer, Material>();
        private QueueEntry m_queued = new QueueEntry();
        private ManualTimer m_timer = new ManualTimer();
        private Dictionary<MeshRenderer, Material> m_transparentMaterials = new Dictionary<MeshRenderer, Material>();
        public const float TRANSPARENT_ALPHA = 0.3f;

        public Coroutine animateToOpaque(float duration, Easing.Function easing, [Optional, DefaultParameterValue(0f)] float delay)
        {
            if (!base.gameObject.activeSelf)
            {
                return null;
            }
            if ((!this.Initialized || this.m_queued.Active) || UnityUtils.CoroutineRunning(ref this.m_animationRoutine))
            {
                this.m_queued.TargetAlpha = 1f;
                this.m_queued.Duration = duration;
                this.m_queued.Easing = easing;
                this.m_queued.Delay = delay;
                this.m_queued.Active = true;
                return null;
            }
            this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(1f, duration, easing, delay));
            return this.m_animationRoutine;
        }

        public Coroutine animateToTransparent(float duration, Easing.Function easing, [Optional, DefaultParameterValue(0f)] float delay)
        {
            if (!base.gameObject.activeSelf)
            {
                return null;
            }
            if ((!this.Initialized || this.m_queued.Active) || UnityUtils.CoroutineRunning(ref this.m_animationRoutine))
            {
                this.m_queued.TargetAlpha = 0.3f;
                this.m_queued.Duration = duration;
                this.m_queued.Easing = easing;
                this.m_queued.Delay = delay;
                this.m_queued.Active = true;
                return null;
            }
            this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(0.3f, duration, easing, delay));
            return this.m_animationRoutine;
        }

        [DebuggerHidden]
        private IEnumerator animationRoutine(float targetAlpha, float duration, Easing.Function easing, [Optional, DefaultParameterValue(0f)] float delay)
        {
            <animationRoutine>c__IteratorE4 re = new <animationRoutine>c__IteratorE4();
            re.delay = delay;
            re.duration = duration;
            re.easing = easing;
            re.targetAlpha = targetAlpha;
            re.<$>delay = delay;
            re.<$>duration = duration;
            re.<$>easing = easing;
            re.<$>targetAlpha = targetAlpha;
            re.<>f__this = this;
            return re;
        }

        protected void Awake()
        {
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Med)
            {
                base.enabled = false;
            }
            else
            {
                this.Renderers = base.gameObject.GetComponentsInChildren<MeshRenderer>(true);
                Bounds bounds = new Bounds();
                for (int i = 0; i < this.Renderers.Length; i++)
                {
                    bounds.Encapsulate(this.Renderers[i].bounds);
                }
                this.OcclusionColliderChild = new GameObject(base.name + "-OcclusionCollider");
                this.OcclusionColliderChild.layer = Layers.CAMERA_OCCLUDERS;
                this.OcclusionColliderChild.tag = Tags.CAMERA_OCCLUDER;
                Transform transform = this.OcclusionColliderChild.transform;
                transform.SetParent(base.transform, false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
                BoxCollider collider = this.OcclusionColliderChild.AddComponent<BoxCollider>();
                collider.center = bounds.center;
                collider.size = (Vector3) (bounds.size * 1.2f);
                this.Alpha = 1f;
            }
        }

        protected void OnDisable()
        {
            this.setTransparent(false);
        }

        protected void OnEnable()
        {
            this.setTransparent(false);
        }

        private void setAlpha(float a)
        {
            this.Alpha = a;
            if (this.Renderers != null)
            {
                for (int i = 0; i < this.Renderers.Length; i++)
                {
                    MeshRenderer key = this.Renderers[i];
                    if (key != null)
                    {
                        if ((this.m_transparentMaterials != null) && this.m_transparentMaterials.ContainsKey(key))
                        {
                            this.m_transparentMaterials[key].SetFloat("_Alpha", this.Alpha);
                        }
                        key.enabled = this.Alpha > 0f;
                    }
                }
            }
        }

        public void setTransparent(bool transparent)
        {
            if (this.Initialized)
            {
                if (base.gameObject.activeSelf)
                {
                    UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
                }
                this.toggleTransparentMaterials(transparent);
                this.setAlpha(!transparent ? 1f : 0.3f);
                this.m_queued.Active = false;
            }
        }

        protected void Start()
        {
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Med)
            {
                base.enabled = false;
            }
            else
            {
                for (int i = 0; i < this.Renderers.Length; i++)
                {
                    MeshRenderer key = this.Renderers[i];
                    this.m_origMaterials.Add(key, key.sharedMaterial);
                    Material material = PlayerView.Binder.MaterialStorage.instantiateTransparentMaterial(key.sharedMaterial);
                    this.m_transparentMaterials.Add(key, material);
                }
                this.Initialized = true;
            }
        }

        private void toggleTransparentMaterials(bool enabled)
        {
            if (this.Renderers != null)
            {
                for (int i = 0; i < this.Renderers.Length; i++)
                {
                    MeshRenderer renderer = this.Renderers[i];
                    if (renderer != null)
                    {
                        if (enabled)
                        {
                            renderer.sharedMaterial = this.m_transparentMaterials[renderer];
                        }
                        else
                        {
                            renderer.sharedMaterial = this.m_origMaterials[renderer];
                        }
                    }
                }
            }
        }

        protected void Update()
        {
            if (this.Initialized && (this.m_queued.Active && !this.IsAnimating))
            {
                this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(this.m_queued.TargetAlpha, this.m_queued.Duration, this.m_queued.Easing, this.m_queued.Delay));
                this.m_queued.Active = false;
            }
        }

        public float Alpha
        {
            [CompilerGenerated]
            get
            {
                return this.<Alpha>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Alpha>k__BackingField = value;
            }
        }

        public bool Initialized
        {
            [CompilerGenerated]
            get
            {
                return this.<Initialized>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Initialized>k__BackingField = value;
            }
        }

        public bool IsAnimating
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_animationRoutine);
            }
        }

        public bool IsOpaque
        {
            get
            {
                return (this.Alpha == 1f);
            }
        }

        public GameObject OcclusionColliderChild
        {
            [CompilerGenerated]
            get
            {
                return this.<OcclusionColliderChild>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<OcclusionColliderChild>k__BackingField = value;
            }
        }

        public MeshRenderer[] Renderers
        {
            [CompilerGenerated]
            get
            {
                return this.<Renderers>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Renderers>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <animationRoutine>c__IteratorE4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>delay;
            internal float <$>duration;
            internal Easing.Function <$>easing;
            internal float <$>targetAlpha;
            internal CameraOccluder <>f__this;
            internal float <easedV>__1;
            internal float <fromAlpha>__0;
            internal float delay;
            internal float duration;
            internal Easing.Function easing;
            internal float targetAlpha;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (this.delay <= 0f)
                        {
                            goto Label_008E;
                        }
                        this.<>f__this.m_timer.set(this.delay);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0146;

                    default:
                        goto Label_01A0;
                }
                if (!this.<>f__this.m_timer.Idle)
                {
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01A2;
                }
            Label_008E:
                this.<>f__this.toggleTransparentMaterials(true);
                this.<fromAlpha>__0 = this.<>f__this.Alpha;
                if (this.duration <= 0f)
                {
                    goto Label_015B;
                }
                this.<>f__this.m_timer.set(this.duration);
            Label_0146:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<easedV>__1 = Easing.Apply(this.<>f__this.m_timer.normalizedProgress(), this.easing);
                    this.<>f__this.setAlpha(this.<fromAlpha>__0 + ((this.targetAlpha - this.<fromAlpha>__0) * this.<easedV>__1));
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01A2;
                }
            Label_015B:
                this.<>f__this.setAlpha(this.targetAlpha);
                if (this.<>f__this.IsOpaque)
                {
                    this.<>f__this.toggleTransparentMaterials(false);
                }
                this.<>f__this.m_animationRoutine = null;
                goto Label_01A0;
                this.$PC = -1;
            Label_01A0:
                return false;
            Label_01A2:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        private class QueueEntry
        {
            public bool Active;
            public float Delay;
            public float Duration;
            public Easing.Function Easing;
            public float TargetAlpha;
        }
    }
}

