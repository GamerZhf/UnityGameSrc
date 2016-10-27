namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CombatTextIngame : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private Action<CombatTextIngame> <EndCallback>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public PlayerView.Billboard Billboard;
        public SpriteRenderer Icon;
        private Coroutine m_mainRoutine;
        private Color m_origShadowColor;
        private TransformAnimation m_ta;
        private TransformAnimationTask m_taTask1;
        private TransformAnimationTask m_taTask2;
        private ManualTimer m_timer = new ManualTimer();
        public TextMesh MainText;
        public TextMesh ShadowText;

        protected void Awake()
        {
            this.Tm = base.transform;
            this.m_ta = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(base.gameObject);
            this.m_taTask1 = new TransformAnimationTask(this.Tm, 0.1f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_taTask1.scale((Vector3) (Vector3.one * 0.23f), true, Easing.Function.OUT_BACK);
            this.m_taTask2 = new TransformAnimationTask(this.Tm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_taTask2.scale((Vector3) (Vector3.one * 0.13f), true, Easing.Function.SMOOTHSTEP);
            if (this.ShadowText != null)
            {
                this.m_origShadowColor = this.ShadowText.color;
            }
        }

        public void cleanUpForReuse()
        {
            UnityUtils.StopCoroutine(this, ref this.m_mainRoutine);
        }

        public void initialize(Camera worldCamera)
        {
            this.Billboard.Camera = worldCamera;
            this.Billboard.Reversed = true;
        }

        [DebuggerHidden]
        public IEnumerator mainRoutine(float stayDuration, float fadeOutDuration, float moveSpeed)
        {
            <mainRoutine>c__IteratorF8 rf = new <mainRoutine>c__IteratorF8();
            rf.stayDuration = stayDuration;
            rf.fadeOutDuration = fadeOutDuration;
            rf.moveSpeed = moveSpeed;
            rf.<$>stayDuration = stayDuration;
            rf.<$>fadeOutDuration = fadeOutDuration;
            rf.<$>moveSpeed = moveSpeed;
            rf.<>f__this = this;
            return rf;
        }

        public void setFontSize(int size)
        {
            this.MainText.fontSize = size;
            this.ShadowText.fontSize = size;
        }

        public void setText(string text, Color color)
        {
            this.MainText.text = text;
            this.ShadowText.text = text;
            this.MainText.color = color;
            this.ShadowText.color = this.m_origShadowColor;
        }

        public void show(float stayDuration, float fadeOutDuration, [Optional, DefaultParameterValue(4f)] float moveSpeed)
        {
            UnityUtils.StopCoroutine(this, ref this.m_mainRoutine);
            this.m_mainRoutine = UnityUtils.StartCoroutine(this, this.mainRoutine(stayDuration, fadeOutDuration, moveSpeed));
        }

        public Action<CombatTextIngame> EndCallback
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

        public bool Running
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_mainRoutine);
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

        [CompilerGenerated]
        private sealed class <mainRoutine>c__IteratorF8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>fadeOutDuration;
            internal float <$>moveSpeed;
            internal float <$>stayDuration;
            internal CombatTextIngame <>f__this;
            internal IEnumerator <ie>__0;
            internal Vector3 <worldPos>__1;
            internal float fadeOutDuration;
            internal float moveSpeed;
            internal float stayDuration;

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
                        this.<>f__this.MainText.color = new Color(this.<>f__this.MainText.color.r, this.<>f__this.MainText.color.g, this.<>f__this.MainText.color.b, 1f);
                        this.<>f__this.ShadowText.color = new Color(this.<>f__this.ShadowText.color.r, this.<>f__this.ShadowText.color.g, this.<>f__this.ShadowText.color.b, 1f);
                        if (this.<>f__this.Icon != null)
                        {
                            this.<>f__this.Icon.color = new Color(this.<>f__this.Icon.color.r, this.<>f__this.Icon.color.g, this.<>f__this.Icon.color.b, 1f);
                        }
                        this.<>f__this.Tm.localScale = Vector3.zero;
                        this.<>f__this.m_taTask1.reset();
                        this.<>f__this.m_ta.addTask(this.<>f__this.m_taTask1);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0225;

                    case 3:
                        goto Label_026D;

                    case 4:
                        goto Label_0483;

                    case 5:
                        goto Label_04B0;

                    default:
                        goto Label_0503;
                }
                while (this.<>f__this.m_ta.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0505;
                }
                this.<>f__this.m_taTask2.reset();
                this.<>f__this.m_ta.addTask(this.<>f__this.m_taTask2);
            Label_0225:
                while (this.<>f__this.m_ta.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0505;
                }
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(this.stayDuration);
            Label_026D:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 3;
                    goto Label_0505;
                }
                this.<>f__this.m_timer.set(this.fadeOutDuration);
            Label_0483:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<worldPos>__1 = this.<>f__this.Tm.position;
                    this.<worldPos>__1.y += this.moveSpeed * Time.unscaledDeltaTime;
                    this.<>f__this.Tm.position = this.<worldPos>__1;
                    this.<>f__this.MainText.color = new Color(this.<>f__this.MainText.color.r, this.<>f__this.MainText.color.g, this.<>f__this.MainText.color.b, 1f - this.<>f__this.m_timer.normalizedProgress());
                    this.<>f__this.ShadowText.color = new Color(this.<>f__this.ShadowText.color.r, this.<>f__this.ShadowText.color.g, this.<>f__this.ShadowText.color.b, 1f - this.<>f__this.m_timer.normalizedProgress());
                    if (this.<>f__this.Icon != null)
                    {
                        this.<>f__this.Icon.color = new Color(this.<>f__this.Icon.color.r, this.<>f__this.Icon.color.g, this.<>f__this.Icon.color.b, 1f - this.<>f__this.m_timer.normalizedProgress());
                    }
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_0505;
                }
            Label_04B0:
                while (this.<>f__this.m_ta.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_0505;
                }
                if (this.<>f__this.EndCallback != null)
                {
                    this.<>f__this.EndCallback(this.<>f__this);
                }
                this.<>f__this.m_mainRoutine = null;
                goto Label_0503;
                this.$PC = -1;
            Label_0503:
                return false;
            Label_0505:
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

