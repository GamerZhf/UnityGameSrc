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
    using UnityEngine.UI;

    public class ScreenTransitionEffect : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Canvas <Canvas>k__BackingField;
        public Image FadeToBlack;
        private Material m_material;
        private ManualTimer m_timer = new ManualTimer();
        public const string MATERIAL_PARAM = "_FadeToBlack";

        protected void Awake()
        {
            this.Canvas = base.GetComponent<UnityEngine.Canvas>();
            this.m_material = new Material(this.FadeToBlack.material);
            this.FadeToBlack.material = this.m_material;
            this.FadeToBlack.enabled = true;
            this.Canvas.enabled = true;
            this.m_material.SetFloat("_FadeToBlack", 1f);
        }

        public Coroutine fadeFromBlack([Optional, DefaultParameterValue(0f)] float delay)
        {
            if (!this.Opaque)
            {
                return null;
            }
            base.StopAllCoroutines();
            return base.StartCoroutine(this.fadeFromRoutine(Color.black, delay, ConfigUi.FADE_TO_BLACK_DURATION));
        }

        [DebuggerHidden]
        private IEnumerator fadeFromRoutine(Color color, float delay, float duration)
        {
            <fadeFromRoutine>c__Iterator179 iterator = new <fadeFromRoutine>c__Iterator179();
            iterator.delay = delay;
            iterator.duration = duration;
            iterator.color = color;
            iterator.<$>delay = delay;
            iterator.<$>duration = duration;
            iterator.<$>color = color;
            iterator.<>f__this = this;
            return iterator;
        }

        public Coroutine fadeFromWhite(float duration, [Optional, DefaultParameterValue(0f)] float delay)
        {
            if (!this.Opaque)
            {
                return null;
            }
            base.StopAllCoroutines();
            return base.StartCoroutine(this.fadeFromRoutine(Color.white, delay, duration));
        }

        public Coroutine fadeToBlack([Optional, DefaultParameterValue(0f)] float delay)
        {
            if (this.Opaque)
            {
                return null;
            }
            base.StopAllCoroutines();
            return base.StartCoroutine(this.fadeToRoutine(Color.black, delay, ConfigUi.FADE_TO_BLACK_DURATION));
        }

        [DebuggerHidden]
        private IEnumerator fadeToRoutine(Color color, float delay, float duration)
        {
            <fadeToRoutine>c__Iterator178 iterator = new <fadeToRoutine>c__Iterator178();
            iterator.delay = delay;
            iterator.duration = duration;
            iterator.color = color;
            iterator.<$>delay = delay;
            iterator.<$>duration = duration;
            iterator.<$>color = color;
            iterator.<>f__this = this;
            return iterator;
        }

        public Coroutine fadeToWhite(float duration, [Optional, DefaultParameterValue(0f)] float delay)
        {
            if (this.Opaque)
            {
                return null;
            }
            base.StopAllCoroutines();
            return base.StartCoroutine(this.fadeToRoutine(Color.white, delay, duration));
        }

        public void setOpaque(bool opaque)
        {
            base.StopAllCoroutines();
            if (opaque)
            {
                this.m_material.SetFloat("_FadeToBlack", 1f);
            }
            else
            {
                this.m_material.SetFloat("_FadeToBlack", 0f);
            }
            this.FadeToBlack.enabled = opaque;
            this.Canvas.enabled = opaque;
        }

        public bool Animating
        {
            get
            {
                return !this.m_timer.Idle;
            }
        }

        public UnityEngine.Canvas Canvas
        {
            [CompilerGenerated]
            get
            {
                return this.<Canvas>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Canvas>k__BackingField = value;
            }
        }

        public bool Opaque
        {
            get
            {
                return this.FadeToBlack.enabled;
            }
        }

        [CompilerGenerated]
        private sealed class <fadeFromRoutine>c__Iterator179 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Color <$>color;
            internal float <$>delay;
            internal float <$>duration;
            internal ScreenTransitionEffect <>f__this;
            internal float <eased>__1;
            internal IEnumerator <ie>__0;
            internal Color color;
            internal float delay;
            internal float duration;

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
                            goto Label_007A;
                        }
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(this.delay);
                        break;

                    case 1:
                        break;

                    case 2:
                        this.$current = null;
                        this.$PC = 3;
                        goto Label_01B4;

                    case 3:
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_01B4;

                    case 4:
                        this.<>f__this.m_material.SetColor("_BlackColor", this.color);
                        goto Label_0155;

                    case 5:
                        goto Label_0155;

                    default:
                        goto Label_01B2;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01B4;
                }
            Label_007A:
                this.<>f__this.m_timer.set(this.duration);
                this.$current = null;
                this.$PC = 2;
                goto Label_01B4;
            Label_0155:
                if (!this.<>f__this.m_timer.Idle)
                {
                    this.<eased>__1 = Easing.Apply(this.<>f__this.m_timer.normalizedProgress(), Easing.Function.IN_CUBIC);
                    this.<>f__this.m_material.SetFloat("_FadeToBlack", 1f - this.<eased>__1);
                    this.<>f__this.m_timer.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_01B4;
                }
                this.<>f__this.m_material.SetFloat("_FadeToBlack", 0f);
                this.<>f__this.FadeToBlack.enabled = false;
                this.<>f__this.Canvas.enabled = false;
                goto Label_01B2;
                this.$PC = -1;
            Label_01B2:
                return false;
            Label_01B4:
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

        [CompilerGenerated]
        private sealed class <fadeToRoutine>c__Iterator178 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Color <$>color;
            internal float <$>delay;
            internal float <$>duration;
            internal ScreenTransitionEffect <>f__this;
            internal float <eased>__1;
            internal IEnumerator <ie>__0;
            internal Color color;
            internal float delay;
            internal float duration;

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
                            goto Label_006E;
                        }
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(this.delay);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0126;

                    default:
                        goto Label_0161;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0163;
                }
            Label_006E:
                this.<>f__this.m_timer.set(this.duration);
                this.<>f__this.FadeToBlack.enabled = true;
                this.<>f__this.Canvas.enabled = true;
                this.<>f__this.m_material.SetColor("_BlackColor", this.color);
            Label_0126:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<eased>__1 = Easing.Apply(this.<>f__this.m_timer.normalizedProgress(), Easing.Function.OUT_CUBIC);
                    this.<>f__this.m_material.SetFloat("_FadeToBlack", this.<eased>__1);
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0163;
                }
                this.<>f__this.m_material.SetFloat("_FadeToBlack", 1f);
                goto Label_0161;
                this.$PC = -1;
            Label_0161:
                return false;
            Label_0163:
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

