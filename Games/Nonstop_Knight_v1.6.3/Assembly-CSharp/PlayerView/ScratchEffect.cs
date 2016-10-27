namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class ScratchEffect : MonoBehaviour
    {
        [CompilerGenerated]
        private Action<ScratchEffect> <EndCallback>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.UI.Image <Image>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private TransformAnimation <transformAnimation>k__BackingField;
        public float EntryDuration;
        public Easing.Function EntryEasing;
        public float ExitDuration;
        private Coroutine m_mainRoutine;
        public bool RandomizeInitialRotation = true;
        public float StayDuration;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.Image = base.GetComponent<UnityEngine.UI.Image>();
            this.transformAnimation = base.gameObject.AddComponent<TransformAnimation>();
        }

        [DebuggerHidden]
        private IEnumerator mainRoutine()
        {
            <mainRoutine>c__IteratorF5 rf = new <mainRoutine>c__IteratorF5();
            rf.<>f__this = this;
            return rf;
        }

        public void show()
        {
            UnityUtils.StopCoroutine(this, ref this.m_mainRoutine);
            this.m_mainRoutine = UnityUtils.StartCoroutine(this, this.mainRoutine());
        }

        public Action<ScratchEffect> EndCallback
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

        public TransformAnimation transformAnimation
        {
            [CompilerGenerated]
            get
            {
                return this.<transformAnimation>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<transformAnimation>k__BackingField = value;
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
        private sealed class <mainRoutine>c__IteratorF5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ScratchEffect <>f__this;
            internal ManualTimer <exitTimer>__1;
            internal TransformAnimationTask <tt>__0;

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
                        if (this.<>f__this.RandomizeInitialRotation)
                        {
                            this.<>f__this.transform.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range((float) 0f, (float) 359f));
                        }
                        this.<>f__this.Image.color = Color.white;
                        this.<>f__this.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_020C;

                    case 1:
                        this.<tt>__0 = new TransformAnimationTask(this.<>f__this.transform, this.<>f__this.EntryDuration, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__0.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<>f__this.transformAnimation.addTask(this.<tt>__0);
                        break;

                    case 2:
                        break;

                    case 3:
                        this.<exitTimer>__1 = new ManualTimer(this.<>f__this.ExitDuration);
                        goto Label_01BC;

                    case 4:
                        goto Label_01BC;

                    default:
                        goto Label_020A;
                }
                if (this.<>f__this.transformAnimation.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 2;
                }
                else
                {
                    this.$current = new WaitForSeconds(this.<>f__this.StayDuration);
                    this.$PC = 3;
                }
                goto Label_020C;
            Label_01BC:
                while (!this.<exitTimer>__1.Idle)
                {
                    this.<>f__this.Image.color = new Color(1f, 1f, 1f, 1f - this.<exitTimer>__1.normalizedProgress());
                    this.<exitTimer>__1.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_020C;
                }
                if (this.<>f__this.EndCallback != null)
                {
                    this.<>f__this.EndCallback(this.<>f__this);
                }
                this.<>f__this.m_mainRoutine = null;
                goto Label_020A;
                this.$PC = -1;
            Label_020A:
                return false;
            Label_020C:
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

