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

    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class ImageFlashEffect : MonoBehaviour
    {
        [CompilerGenerated]
        private Action<ImageFlashEffect> <EndCallback>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.UI.Image <Image>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        private Coroutine m_mainRoutine;
        private Color m_originalColor;
        private ManualTimer m_timer = new ManualTimer();

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.Image = base.GetComponent<UnityEngine.UI.Image>();
            this.m_originalColor = this.Image.color;
        }

        public void hide()
        {
            UnityUtils.StopCoroutine(this, ref this.m_mainRoutine);
            this.Image.enabled = false;
        }

        [DebuggerHidden]
        private IEnumerator mainRoutine(float duration, Easing.Function entryEasing, Easing.Function exitEasing, float delay)
        {
            <mainRoutine>c__IteratorF4 rf = new <mainRoutine>c__IteratorF4();
            rf.delay = delay;
            rf.duration = duration;
            rf.entryEasing = entryEasing;
            rf.exitEasing = exitEasing;
            rf.<$>delay = delay;
            rf.<$>duration = duration;
            rf.<$>entryEasing = entryEasing;
            rf.<$>exitEasing = exitEasing;
            rf.<>f__this = this;
            return rf;
        }

        protected void OnDisable()
        {
            this.hide();
        }

        [ContextMenu("show()")]
        public void show(float duration, Easing.Function entryEasing, Easing.Function exitEasing, [Optional, DefaultParameterValue(0f)] float delay)
        {
            base.gameObject.SetActive(true);
            UnityUtils.StopCoroutine(this, ref this.m_mainRoutine);
            this.m_mainRoutine = UnityUtils.StartCoroutine(this, this.mainRoutine(duration, entryEasing, exitEasing, delay));
        }

        public Action<ImageFlashEffect> EndCallback
        {
            [CompilerGenerated]
            get
            {
                return this.<EndCallback>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<EndCallback>k__BackingField = value;
            }
        }

        public UnityEngine.UI.Image Image
        {
            [CompilerGenerated]
            get
            {
                return this.<Image>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Image>k__BackingField = value;
            }
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }

        public bool Visible
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_mainRoutine);
            }
        }

        [CompilerGenerated]
        private sealed class <mainRoutine>c__IteratorF4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>delay;
            internal float <$>duration;
            internal Easing.Function <$>entryEasing;
            internal Easing.Function <$>exitEasing;
            internal ImageFlashEffect <>f__this;
            internal float <deltaAlpha>__3;
            internal float <easedV>__6;
            internal float <easedV>__9;
            internal float <entryDuration>__4;
            internal float <exitDuration>__7;
            internal IEnumerator <ie>__0;
            internal float <sourceAlpha>__1;
            internal float <targetAlpha>__2;
            internal float <v>__5;
            internal float <v>__8;
            internal float delay;
            internal float duration;
            internal Easing.Function entryEasing;
            internal Easing.Function exitEasing;

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
                            goto Label_007C;
                        }
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(this.delay);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_019D;

                    case 3:
                        goto Label_02C2;

                    default:
                        goto Label_0326;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_0328;
                }
            Label_007C:
                this.<>f__this.Image.enabled = true;
                this.<sourceAlpha>__1 = 0f;
                this.<targetAlpha>__2 = this.<>f__this.m_originalColor.a;
                this.<deltaAlpha>__3 = this.<targetAlpha>__2 - this.<sourceAlpha>__1;
                this.<entryDuration>__4 = this.duration * 0.5f;
                this.<>f__this.m_timer.set(this.<entryDuration>__4);
            Label_019D:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<v>__5 = this.<>f__this.m_timer.normalizedProgress();
                    this.<easedV>__6 = Easing.Apply(this.<v>__5, this.entryEasing);
                    this.<>f__this.Image.color = new Color(this.<>f__this.m_originalColor.r, this.<>f__this.m_originalColor.g, this.<>f__this.m_originalColor.b, this.<sourceAlpha>__1 + (this.<deltaAlpha>__3 * this.<easedV>__6));
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0328;
                }
                this.<sourceAlpha>__1 = this.<>f__this.m_originalColor.a;
                this.<targetAlpha>__2 = 0f;
                this.<deltaAlpha>__3 = this.<targetAlpha>__2 - this.<sourceAlpha>__1;
                this.<exitDuration>__7 = this.duration * 0.5f;
                this.<>f__this.m_timer.set(this.<exitDuration>__7);
            Label_02C2:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<v>__8 = this.<>f__this.m_timer.normalizedProgress();
                    this.<easedV>__9 = Easing.Apply(this.<v>__8, this.exitEasing);
                    this.<>f__this.Image.color = new Color(this.<>f__this.m_originalColor.r, this.<>f__this.m_originalColor.g, this.<>f__this.m_originalColor.b, this.<sourceAlpha>__1 + (this.<deltaAlpha>__3 * this.<easedV>__9));
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0328;
                }
                if (this.<>f__this.EndCallback != null)
                {
                    this.<>f__this.EndCallback(this.<>f__this);
                }
                this.<>f__this.Image.enabled = false;
                this.<>f__this.m_mainRoutine = null;
                goto Label_0326;
                this.$PC = -1;
            Label_0326:
                return false;
            Label_0328:
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

