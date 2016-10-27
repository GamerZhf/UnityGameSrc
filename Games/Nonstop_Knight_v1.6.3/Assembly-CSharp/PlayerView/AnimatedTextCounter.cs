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

    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class AnimatedTextCounter : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.UI.Text <Text>k__BackingField;
        [NonSerialized]
        public float AnimationDuration = 0.13f;
        [NonSerialized]
        public Easing.Function DefaultEasing = Easing.Function.OUT_QUAD;
        private string m_counterPrefix;
        private double m_currentValue;
        private Coroutine m_masterRoutine;
        private List<double> m_queue = new List<double>(20);
        private ManualTimer m_timer = new ManualTimer();

        protected void Awake()
        {
            this.Text = GameObjectExtensions.AddOrGetComponent<UnityEngine.UI.Text>(base.gameObject);
        }

        public void initialize(string counterPrefix, float animDuration)
        {
            this.m_counterPrefix = counterPrefix;
            this.AnimationDuration = animDuration;
        }

        public bool isAnimating()
        {
            return ((this.m_queue.Count > 0) || !this.m_timer.Idle);
        }

        [DebuggerHidden]
        private IEnumerator masterRoutine()
        {
            <masterRoutine>c__Iterator1F9 iteratorf = new <masterRoutine>c__Iterator1F9();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        protected void OnDisable()
        {
            UnityUtils.StopCoroutine(this, ref this.m_masterRoutine);
        }

        protected void OnEnable()
        {
            this.restartMasterRoutine();
        }

        public void queue(double amount)
        {
            this.m_queue.Add(amount);
        }

        private void restartMasterRoutine()
        {
            UnityUtils.StopCoroutine(this, ref this.m_masterRoutine);
            if (base.gameObject.activeInHierarchy)
            {
                this.m_masterRoutine = UnityUtils.StartCoroutine(this, this.masterRoutine());
            }
        }

        public void set(double value, [Optional, DefaultParameterValue(true)] bool clearQueue)
        {
            if (clearQueue)
            {
                if (this.m_queue.Count > 0)
                {
                }
                this.m_queue.Clear();
                this.restartMasterRoutine();
            }
            this.m_currentValue = MathUtil.Clamp(value, 0.0, double.MaxValue);
            if (this.m_counterPrefix != null)
            {
                this.Text.text = this.m_counterPrefix + MenuHelpers.BigValueToString(Math.Round(this.m_currentValue));
            }
            else
            {
                this.Text.text = MenuHelpers.BigValueToString(Math.Round(this.m_currentValue));
            }
        }

        public UnityEngine.UI.Text Text
        {
            [CompilerGenerated]
            get
            {
                return this.<Text>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Text>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <masterRoutine>c__Iterator1F9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AnimatedTextCounter <>f__this;
            internal double <delta>__4;
            internal float <easedV>__5;
            internal int <i>__1;
            internal double <source>__2;
            internal double <target>__3;
            internal double <total>__0;

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
                        break;

                    case 1:
                        goto Label_017C;

                    case 2:
                        break;
                        this.$PC = -1;
                        goto Label_01C2;

                    default:
                        goto Label_01C2;
                }
                this.<total>__0 = 0.0;
                this.<i>__1 = 0;
                while (this.<i>__1 < this.<>f__this.m_queue.Count)
                {
                    this.<total>__0 += this.<>f__this.m_queue[this.<i>__1];
                    this.<i>__1++;
                }
                this.<>f__this.m_queue.Clear();
                if (this.<total>__0 == 0.0)
                {
                    goto Label_01A3;
                }
                this.<>f__this.m_timer.set(this.<>f__this.AnimationDuration);
                this.<source>__2 = this.<>f__this.m_currentValue;
                this.<target>__3 = this.<>f__this.m_currentValue + this.<total>__0;
                this.<delta>__4 = this.<target>__3 - this.<source>__2;
            Label_017C:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<easedV>__5 = Easing.Apply(this.<>f__this.m_timer.normalizedProgress(), this.<>f__this.DefaultEasing);
                    this.<>f__this.set(this.<source>__2 + (this.<delta>__4 * this.<easedV>__5), false);
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01C4;
                }
                this.<>f__this.set(this.<target>__3, false);
            Label_01A3:
                this.$current = null;
                this.$PC = 2;
                goto Label_01C4;
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

