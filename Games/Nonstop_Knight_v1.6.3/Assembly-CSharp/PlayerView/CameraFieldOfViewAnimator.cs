namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class CameraFieldOfViewAnimator : MonoBehaviour
    {
        [CompilerGenerated]
        private float <DefaultFov>k__BackingField;
        private Coroutine m_animationRoutine;
        private Camera m_camera;
        private ManualTimer m_timer = new ManualTimer();

        public Coroutine animate(float targetFov, float duration, [Optional, DefaultParameterValue(0f)] float delay)
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(targetFov, duration, delay));
            return this.m_animationRoutine;
        }

        [DebuggerHidden]
        private IEnumerator animationRoutine(float targetFov, float duration, float delay)
        {
            <animationRoutine>c__IteratorE3 re = new <animationRoutine>c__IteratorE3();
            re.delay = delay;
            re.duration = duration;
            re.targetFov = targetFov;
            re.<$>delay = delay;
            re.<$>duration = duration;
            re.<$>targetFov = targetFov;
            re.<>f__this = this;
            return re;
        }

        protected void Awake()
        {
            this.m_camera = base.GetComponent<Camera>();
            this.DefaultFov = this.m_camera.fieldOfView;
        }

        public void setFov(float targetFov)
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            this.m_camera.fieldOfView = targetFov;
        }

        public float DefaultFov
        {
            [CompilerGenerated]
            get
            {
                return this.<DefaultFov>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DefaultFov>k__BackingField = value;
            }
        }

        public bool IsAnimating
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_animationRoutine);
            }
        }

        [CompilerGenerated]
        private sealed class <animationRoutine>c__IteratorE3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>delay;
            internal float <$>duration;
            internal float <$>targetFov;
            internal CameraFieldOfViewAnimator <>f__this;
            internal float <easedV>__1;
            internal float <fromFov>__0;
            internal float delay;
            internal float duration;
            internal float targetFov;

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
                            goto Label_009A;
                        }
                        this.<>f__this.m_timer.set(this.delay);
                        break;

                    case 1:
                        break;

                    case 2:
                        this.$current = null;
                        this.$PC = 3;
                        goto Label_01C4;

                    case 3:
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_01C4;

                    case 4:
                        this.<fromFov>__0 = this.<>f__this.m_camera.fieldOfView;
                        if (this.duration <= 0f)
                        {
                            goto Label_0199;
                        }
                        this.<>f__this.m_timer.set(this.duration);
                        goto Label_0184;

                    case 5:
                        goto Label_0184;

                    default:
                        goto Label_01C2;
                }
                if (!this.<>f__this.m_timer.Idle)
                {
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01C4;
                }
            Label_009A:
                this.$current = null;
                this.$PC = 2;
                goto Label_01C4;
            Label_0184:
                if (!this.<>f__this.m_timer.Idle)
                {
                    this.<easedV>__1 = Easing.Apply(this.<>f__this.m_timer.normalizedProgress(), Easing.Function.SMOOTHSTEP);
                    this.<>f__this.m_camera.fieldOfView = this.<fromFov>__0 + ((this.targetFov - this.<fromFov>__0) * this.<easedV>__1);
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_01C4;
                }
            Label_0199:
                this.<>f__this.m_camera.fieldOfView = this.targetFov;
                this.<>f__this.m_animationRoutine = null;
                this.$PC = -1;
            Label_01C2:
                return false;
            Label_01C4:
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
    }
}

