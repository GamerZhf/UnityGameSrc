namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ScalePulse : MonoBehaviour
    {
        public float ANIMATION_DURATION = 1.5f;
        public bool LOOPING = true;
        private bool m_enabled;
        private Coroutine m_mainRoutine;
        private TransformAnimation m_ta;
        private ManualTimer m_timer = new ManualTimer();
        public float MAX_SCALE = 1.3f;
        public bool PLAY_ON_AWAKE = true;
        public RectTransform TargetRectTm;
        public float WAIT_BETWEEN_CYCLES = 0.2f;

        protected void Awake()
        {
            this.m_ta = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(base.gameObject);
            this.m_enabled = this.PLAY_ON_AWAKE;
        }

        [DebuggerHidden]
        private IEnumerator mainRoutine()
        {
            <mainRoutine>c__Iterator1FC iteratorfc = new <mainRoutine>c__Iterator1FC();
            iteratorfc.<>f__this = this;
            return iteratorfc;
        }

        protected void OnDisable()
        {
            UnityUtils.StopCoroutine(this, ref this.m_mainRoutine);
        }

        protected void OnEnable()
        {
            this.m_mainRoutine = UnityUtils.StartCoroutine(this, this.mainRoutine());
        }

        [ContextMenu("play()")]
        public void play()
        {
            if (base.gameObject.activeInHierarchy)
            {
                UnityUtils.StopCoroutine(this, ref this.m_mainRoutine);
                this.m_enabled = true;
                this.m_mainRoutine = UnityUtils.StartCoroutine(this, this.mainRoutine());
            }
        }

        public void stop()
        {
            this.m_enabled = false;
        }

        [CompilerGenerated]
        private sealed class <mainRoutine>c__Iterator1FC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ScalePulse <>f__this;
            internal IEnumerator <ie>__2;
            internal TransformAnimationTask <tt>__0;
            internal TransformAnimationTask <tt2>__1;

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
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_022D;

                    case 1:
                    case 2:
                        break;

                    case 3:
                        goto Label_0178;

                    case 4:
                        goto Label_01E0;

                    case 5:
                        break;
                        this.$PC = -1;
                        goto Label_022B;

                    default:
                        goto Label_022B;
                }
                while (!this.<>f__this.m_enabled)
                {
                    this.$current = new WaitForSeconds(UnityEngine.Random.Range((float) 0.05f, (float) 0.1f));
                    this.$PC = 2;
                    goto Label_022D;
                }
                this.<>f__this.m_ta.stopAll();
                this.<>f__this.TargetRectTm.localScale = Vector3.one;
                this.<tt>__0 = new TransformAnimationTask(this.<>f__this.TargetRectTm, this.<>f__this.ANIMATION_DURATION * 0.5f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__0.scale((Vector3) (Vector3.one * this.<>f__this.MAX_SCALE), true, Easing.Function.SMOOTHSTEP);
                this.<>f__this.m_ta.addTask(this.<tt>__0);
                this.<tt2>__1 = new TransformAnimationTask(this.<>f__this.TargetRectTm, this.<>f__this.ANIMATION_DURATION * 0.5f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt2>__1.scale(Vector3.one, true, Easing.Function.SMOOTHSTEP);
                this.<>f__this.m_ta.addTask(this.<tt2>__1);
            Label_0178:
                while (this.<>f__this.m_ta.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_022D;
                }
                this.<>f__this.m_timer.set(this.<>f__this.WAIT_BETWEEN_CYCLES);
                this.<ie>__2 = this.<>f__this.m_timer.tickUntilEndUnscaled();
            Label_01E0:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 4;
                    goto Label_022D;
                }
                if (!this.<>f__this.LOOPING)
                {
                    this.<>f__this.m_enabled = false;
                }
                this.$current = null;
                this.$PC = 5;
                goto Label_022D;
            Label_022B:
                return false;
            Label_022D:
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

