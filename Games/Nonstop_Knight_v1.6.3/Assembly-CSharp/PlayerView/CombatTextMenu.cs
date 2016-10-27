namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class CombatTextMenu : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private Action<CombatTextMenu> <EndCallback>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public UnityEngine.CanvasGroup CanvasGroup;
        private Coroutine m_mainRoutine;
        private TransformAnimation m_ta;
        private TransformAnimationTask m_taTask;
        private ManualTimer m_timer = new ManualTimer();
        public UnityEngine.UI.Text Text;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.m_ta = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(base.gameObject);
            this.m_taTask = new TransformAnimationTask(this.RectTm, 0.15f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_taTask.scale(Vector3.one, true, Easing.Function.OUT_BACK);
        }

        public void cleanUpForReuse()
        {
            UnityUtils.StopCoroutine(this, ref this.m_mainRoutine);
            this.Text.color = Color.white;
        }

        public void initialize()
        {
        }

        [DebuggerHidden]
        private IEnumerator mainRoutine()
        {
            <mainRoutine>c__IteratorF9 rf = new <mainRoutine>c__IteratorF9();
            rf.<>f__this = this;
            return rf;
        }

        public void setColor(Color color)
        {
            this.Text.color = color;
        }

        public void setText(string text)
        {
            this.Text.text = text;
        }

        public void show()
        {
            UnityUtils.StopCoroutine(this, ref this.m_mainRoutine);
            this.m_mainRoutine = UnityUtils.StartCoroutine(this, this.mainRoutine());
        }

        public Action<CombatTextMenu> EndCallback
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

        public bool Running
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_mainRoutine);
            }
        }

        [CompilerGenerated]
        private sealed class <mainRoutine>c__IteratorF9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CombatTextMenu <>f__this;
            internal Vector2 <anchoredPos>__1;
            internal float <easedV>__2;
            internal IEnumerator <ie>__0;

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
                        this.<>f__this.CanvasGroup.alpha = 1f;
                        this.<>f__this.RectTm.localScale = Vector3.zero;
                        this.<>f__this.m_taTask.reset();
                        this.<>f__this.m_ta.addTask(this.<>f__this.m_taTask);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00DE;

                    case 3:
                        goto Label_01B2;

                    default:
                        goto Label_021A;
                }
                if (this.<>f__this.m_ta.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_021C;
                }
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.05f);
            Label_00DE:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_021C;
                }
                this.<>f__this.m_timer.set(1f);
            Label_01B2:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<anchoredPos>__1 = this.<>f__this.RectTm.anchoredPosition;
                    this.<anchoredPos>__1.y += 200f * Time.unscaledDeltaTime;
                    this.<>f__this.RectTm.anchoredPosition = this.<anchoredPos>__1;
                    this.<easedV>__2 = Easing.Apply(this.<>f__this.m_timer.normalizedProgress(), Easing.Function.IN_CUBIC);
                    this.<>f__this.CanvasGroup.alpha = 1f - this.<easedV>__2;
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_021C;
                }
                this.<>f__this.CanvasGroup.alpha = 0f;
                if (this.<>f__this.EndCallback != null)
                {
                    this.<>f__this.EndCallback(this.<>f__this);
                }
                this.<>f__this.m_mainRoutine = null;
                goto Label_021A;
                this.$PC = -1;
            Label_021A:
                return false;
            Label_021C:
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

