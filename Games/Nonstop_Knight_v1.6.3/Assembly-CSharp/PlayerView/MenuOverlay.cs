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

    public class MenuOverlay : MonoBehaviour
    {
        public UnityEngine.UI.Image Image;
        private Coroutine m_animationRoutine;
        private ManualTimer m_timer = new ManualTimer();

        [DebuggerHidden]
        private IEnumerator animationRoutine(float targetAlpha, float duration, Easing.Function easing)
        {
            <animationRoutine>c__Iterator137 iterator = new <animationRoutine>c__Iterator137();
            iterator.duration = duration;
            iterator.easing = easing;
            iterator.targetAlpha = targetAlpha;
            iterator.<$>duration = duration;
            iterator.<$>easing = easing;
            iterator.<$>targetAlpha = targetAlpha;
            iterator.<>f__this = this;
            return iterator;
        }

        public Coroutine fadeToBlack(float duration, [Optional, DefaultParameterValue(1f)] float targetAlpha, [Optional, DefaultParameterValue(0)] Easing.Function easing)
        {
            this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(targetAlpha, duration, easing));
            return this.m_animationRoutine;
        }

        public void fadeToTransparent(float duration, [Optional, DefaultParameterValue(0)] Easing.Function easing)
        {
            this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(0f, duration, easing));
        }

        public void setTransparent(bool transparent)
        {
            Color color = this.Image.color;
            color.a = !transparent ? 1f : 0f;
            this.Image.color = color;
            this.Image.enabled = !transparent;
        }

        public bool IsAnimating
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_animationRoutine);
            }
        }

        [CompilerGenerated]
        private sealed class <animationRoutine>c__Iterator137 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>duration;
            internal Easing.Function <$>easing;
            internal float <$>targetAlpha;
            internal MenuOverlay <>f__this;
            internal Color <c>__1;
            internal float <easedV>__2;
            internal float <fromAlpha>__0;
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
                        this.<>f__this.Image.enabled = true;
                        this.<fromAlpha>__0 = this.<>f__this.Image.color.a;
                        this.<c>__1 = this.<>f__this.Image.color;
                        if (this.duration <= 0f)
                        {
                            goto Label_012C;
                        }
                        this.<>f__this.m_timer.set(this.duration);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_01B4;
                }
                if (!this.<>f__this.m_timer.Idle)
                {
                    this.<easedV>__2 = Easing.Apply(this.<>f__this.m_timer.normalizedProgress(), this.easing);
                    this.<c>__1.a = this.<fromAlpha>__0 + ((this.targetAlpha - this.<fromAlpha>__0) * this.<easedV>__2);
                    this.<>f__this.Image.color = this.<c>__1;
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_012C:
                this.<c>__1.a = this.targetAlpha;
                this.<>f__this.Image.color = this.<c>__1;
                if (this.<>f__this.Image.color.a > 0f)
                {
                    this.<>f__this.Image.enabled = true;
                }
                else
                {
                    this.<>f__this.Image.enabled = false;
                }
                this.<>f__this.m_animationRoutine = null;
                goto Label_01B4;
                this.$PC = -1;
            Label_01B4:
                return false;
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

