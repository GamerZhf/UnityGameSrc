namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(UnityEngine.UI.Slider))]
    public class AnimatedProgressBar : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.UI.Slider <Slider>k__BackingField;
        public Easing.Function DefaultEasing;
        private Coroutine m_animationRoutine;
        private ManualTimer m_timer = new ManualTimer();

        [DebuggerHidden]
        private IEnumerator animatedProgressRoutine(float sourceV, float targetV, float duration, Easing.Function easingFunction, AnimationComplete completeCallback, float delay)
        {
            <animatedProgressRoutine>c__Iterator1F8 iteratorf = new <animatedProgressRoutine>c__Iterator1F8();
            iteratorf.delay = delay;
            iteratorf.sourceV = sourceV;
            iteratorf.targetV = targetV;
            iteratorf.duration = duration;
            iteratorf.easingFunction = easingFunction;
            iteratorf.completeCallback = completeCallback;
            iteratorf.<$>delay = delay;
            iteratorf.<$>sourceV = sourceV;
            iteratorf.<$>targetV = targetV;
            iteratorf.<$>duration = duration;
            iteratorf.<$>easingFunction = easingFunction;
            iteratorf.<$>completeCallback = completeCallback;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public void animateToNormalizedValue(float targetV, float duration, [Optional, DefaultParameterValue(null)] Easing.Function? easingFunction, [Optional, DefaultParameterValue(null)] AnimationComplete completeCallback, [Optional, DefaultParameterValue(0f)] float delay)
        {
            this.animateToNormalizedValue(this.Slider.normalizedValue, targetV, duration, easingFunction, completeCallback, delay);
        }

        public void animateToNormalizedValue(float sourceV, float targetV, float duration, [Optional, DefaultParameterValue(null)] Easing.Function? easingFunction, [Optional, DefaultParameterValue(null)] AnimationComplete completeCallback, [Optional, DefaultParameterValue(0f)] float delay)
        {
            if (!easingFunction.HasValue)
            {
                easingFunction = new Easing.Function?(this.DefaultEasing);
            }
            if (!base.gameObject.activeInHierarchy)
            {
                this.setNormalizedValue(targetV);
            }
            else
            {
                UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
                this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animatedProgressRoutine(sourceV, targetV, duration, easingFunction.Value, completeCallback, delay));
            }
        }

        protected void Awake()
        {
            this.Slider = base.GetComponent<UnityEngine.UI.Slider>();
        }

        public float getNormalizedValue()
        {
            return this.Slider.normalizedValue;
        }

        public void setNormalizedValue(float v)
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            this.Slider.normalizedValue = v;
        }

        [ContextMenu("testAnimation()")]
        public void testAnimation()
        {
            this.Awake();
            this.animateToNormalizedValue(0f, 1f, 2f, new Easing.Function?(this.DefaultEasing), null, 0f);
        }

        public bool Animating
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_animationRoutine);
            }
        }

        public UnityEngine.UI.Slider Slider
        {
            [CompilerGenerated]
            get
            {
                return this.<Slider>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Slider>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <animatedProgressRoutine>c__Iterator1F8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AnimatedProgressBar.AnimationComplete <$>completeCallback;
            internal float <$>delay;
            internal float <$>duration;
            internal Easing.Function <$>easingFunction;
            internal float <$>sourceV;
            internal float <$>targetV;
            internal AnimatedProgressBar <>f__this;
            internal float <currentV>__3;
            internal float <deltaV>__1;
            internal float <eased>__2;
            internal IEnumerator <ie>__0;
            internal AnimatedProgressBar.AnimationComplete completeCallback;
            internal float delay;
            internal float duration;
            internal Easing.Function easingFunction;
            internal float sourceV;
            internal float targetV;

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
                            goto Label_0078;
                        }
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(this.delay);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0136;

                    default:
                        goto Label_0195;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_0197;
                }
            Label_0078:
                this.<>f__this.Slider.normalizedValue = this.sourceV;
                this.<deltaV>__1 = this.targetV - this.sourceV;
                this.<>f__this.m_timer.set(this.duration);
            Label_0136:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<eased>__2 = Easing.Apply(this.<>f__this.m_timer.normalizedProgress(), this.easingFunction);
                    this.<currentV>__3 = this.sourceV + (this.<deltaV>__1 * this.<eased>__2);
                    this.<>f__this.Slider.normalizedValue = this.<currentV>__3;
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0197;
                }
                this.<>f__this.Slider.normalizedValue = this.targetV;
                if (this.completeCallback != null)
                {
                    this.completeCallback(this.<>f__this);
                }
                this.<>f__this.m_animationRoutine = null;
                goto Label_0195;
                this.$PC = -1;
            Label_0195:
                return false;
            Label_0197:
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

        public delegate void AnimationComplete(AnimatedProgressBar abp);
    }
}

