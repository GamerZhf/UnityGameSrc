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

    [RequireComponent(typeof(UnityEngine.CanvasGroup))]
    public class CanvasGroupAlphaFading : MonoBehaviour
    {
        public UnityEngine.CanvasGroup CanvasGroup;
        private Coroutine m_animationRoutine;
        private Image m_image;
        private ManualTimer m_timer = new ManualTimer();

        public Coroutine animateToAlpha(float alpha, float duration, [Optional, DefaultParameterValue(0f)] float delay)
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            if (!base.gameObject.activeInHierarchy)
            {
                return null;
            }
            this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(alpha, duration, delay));
            return this.m_animationRoutine;
        }

        public Coroutine animateToBlack(float duration, [Optional, DefaultParameterValue(0f)] float delay)
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            if (!base.gameObject.activeInHierarchy)
            {
                return null;
            }
            this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(1f, duration, delay));
            return this.m_animationRoutine;
        }

        public Coroutine animateToTransparent(float duration, [Optional, DefaultParameterValue(0f)] float delay)
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            if (!base.gameObject.activeInHierarchy)
            {
                return null;
            }
            this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(0f, duration, delay));
            return this.m_animationRoutine;
        }

        [DebuggerHidden]
        private IEnumerator animationRoutine(float targetAlpha, float duration, [Optional, DefaultParameterValue(0f)] float delay)
        {
            <animationRoutine>c__Iterator1FA iteratorfa = new <animationRoutine>c__Iterator1FA();
            iteratorfa.delay = delay;
            iteratorfa.duration = duration;
            iteratorfa.targetAlpha = targetAlpha;
            iteratorfa.<$>delay = delay;
            iteratorfa.<$>duration = duration;
            iteratorfa.<$>targetAlpha = targetAlpha;
            iteratorfa.<>f__this = this;
            return iteratorfa;
        }

        protected void Awake()
        {
            if (this.CanvasGroup == null)
            {
                this.CanvasGroup = base.GetComponent<UnityEngine.CanvasGroup>();
            }
            this.m_image = base.GetComponent<Image>();
        }

        public void setTransparent(bool transparent)
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            this.CanvasGroup.alpha = !transparent ? 1f : 0f;
            if (this.m_image != null)
            {
                this.m_image.enabled = !transparent;
            }
        }

        public bool IsAnimating
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_animationRoutine);
            }
        }

        public bool IsTransparent
        {
            get
            {
                return (this.CanvasGroup.alpha == 0f);
            }
        }

        public float TimeRemaining
        {
            get
            {
                return this.m_timer.timeRemaining();
            }
        }

        [CompilerGenerated]
        private sealed class <animationRoutine>c__Iterator1FA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>delay;
            internal float <$>duration;
            internal float <$>targetAlpha;
            internal CanvasGroupAlphaFading <>f__this;
            internal float <fromAlpha>__0;
            internal float delay;
            internal float duration;
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
                            goto Label_0094;
                        }
                        this.<>f__this.m_timer.set(this.delay);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0160;

                    default:
                        goto Label_01DA;
                }
                if (!this.<>f__this.m_timer.Idle)
                {
                    this.<>f__this.m_timer.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01DC;
                }
            Label_0094:
                if (this.<>f__this.m_image != null)
                {
                    this.<>f__this.m_image.enabled = true;
                }
                this.<fromAlpha>__0 = this.<>f__this.CanvasGroup.alpha;
                if (this.duration <= 0f)
                {
                    goto Label_0175;
                }
                this.<>f__this.m_timer.set(this.duration);
            Label_0160:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<>f__this.CanvasGroup.alpha = this.<fromAlpha>__0 + ((this.targetAlpha - this.<fromAlpha>__0) * this.<>f__this.m_timer.normalizedProgress());
                    this.<>f__this.m_timer.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01DC;
                }
            Label_0175:
                this.<>f__this.CanvasGroup.alpha = this.targetAlpha;
                if ((this.<>f__this.m_image != null) && (this.targetAlpha == 0f))
                {
                    this.<>f__this.m_image.enabled = false;
                }
                this.<>f__this.m_animationRoutine = null;
                goto Label_01DA;
                this.$PC = -1;
            Label_01DA:
                return false;
            Label_01DC:
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

