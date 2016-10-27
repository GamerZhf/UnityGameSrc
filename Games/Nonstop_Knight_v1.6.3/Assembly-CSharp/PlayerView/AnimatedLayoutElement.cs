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

    public class AnimatedLayoutElement : MonoBehaviour
    {
        [NonSerialized]
        public float CollapseAnimationDuration = 0.08f;
        [NonSerialized]
        public float ExpandAnimationDuration = 0.15f;
        public UnityEngine.UI.LayoutElement LayoutElement;
        private Coroutine m_animationRoutine;
        private ManualTimer m_timer = new ManualTimer();

        [DebuggerHidden]
        private IEnumerator animationRoutine(float duration, float targetHeight, Easing.Function easing)
        {
            <animationRoutine>c__Iterator1F7 iteratorf = new <animationRoutine>c__Iterator1F7();
            iteratorf.duration = duration;
            iteratorf.targetHeight = targetHeight;
            iteratorf.easing = easing;
            iteratorf.<$>duration = duration;
            iteratorf.<$>targetHeight = targetHeight;
            iteratorf.<$>easing = easing;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        protected void Awake()
        {
        }

        public Coroutine collapse(System.Action completionCallback, [Optional, DefaultParameterValue(false)] bool instant)
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            if (instant)
            {
                this.LayoutElement.minHeight = 0f;
                if (completionCallback != null)
                {
                    completionCallback();
                }
            }
            else if (base.gameObject.activeInHierarchy)
            {
                this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.collapseRoutine(completionCallback));
            }
            else
            {
                UnityEngine.Debug.LogWarning("Trying to collapse while GameObject is inactive: " + base.name);
            }
            return this.m_animationRoutine;
        }

        public Coroutine collapseAndExpand(float targetHeight, System.Action collapseCompletionCallback, System.Action expandStartCallback, [Optional, DefaultParameterValue(false)] bool instant)
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            if (base.gameObject.activeInHierarchy)
            {
                this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.collapseAndExpandRoutine(targetHeight, collapseCompletionCallback, expandStartCallback, instant));
            }
            else
            {
                UnityEngine.Debug.LogWarning("Trying to collapseAndExpand while GameObject is inactive: " + base.name);
            }
            return this.m_animationRoutine;
        }

        [DebuggerHidden]
        private IEnumerator collapseAndExpandRoutine(float targetHeight, System.Action collapseCompletionCallback, System.Action expandStartCallback, [Optional, DefaultParameterValue(false)] bool instant)
        {
            <collapseAndExpandRoutine>c__Iterator1F4 iteratorf = new <collapseAndExpandRoutine>c__Iterator1F4();
            iteratorf.instant = instant;
            iteratorf.collapseCompletionCallback = collapseCompletionCallback;
            iteratorf.expandStartCallback = expandStartCallback;
            iteratorf.targetHeight = targetHeight;
            iteratorf.<$>instant = instant;
            iteratorf.<$>collapseCompletionCallback = collapseCompletionCallback;
            iteratorf.<$>expandStartCallback = expandStartCallback;
            iteratorf.<$>targetHeight = targetHeight;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        [DebuggerHidden]
        private IEnumerator collapseRoutine(System.Action completionCallback)
        {
            <collapseRoutine>c__Iterator1F6 iteratorf = new <collapseRoutine>c__Iterator1F6();
            iteratorf.completionCallback = completionCallback;
            iteratorf.<$>completionCallback = completionCallback;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public Coroutine expand(float targetHeight, System.Action startCallback, [Optional, DefaultParameterValue(false)] bool instant)
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            if (instant)
            {
                if (startCallback != null)
                {
                    startCallback();
                }
                this.LayoutElement.minHeight = targetHeight;
            }
            else if (base.gameObject.activeInHierarchy)
            {
                this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.expandRoutine(targetHeight, startCallback));
            }
            else
            {
                UnityEngine.Debug.LogWarning("Trying to expand while GameObject is inactive: " + base.name);
            }
            return this.m_animationRoutine;
        }

        [DebuggerHidden]
        private IEnumerator expandRoutine(float targetHeight, System.Action startCallback)
        {
            <expandRoutine>c__Iterator1F5 iteratorf = new <expandRoutine>c__Iterator1F5();
            iteratorf.startCallback = startCallback;
            iteratorf.targetHeight = targetHeight;
            iteratorf.<$>startCallback = startCallback;
            iteratorf.<$>targetHeight = targetHeight;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public bool isCollapsed()
        {
            return (this.LayoutElement.minHeight == 0f);
        }

        [CompilerGenerated]
        private sealed class <animationRoutine>c__Iterator1F7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>duration;
            internal Easing.Function <$>easing;
            internal float <$>targetHeight;
            internal AnimatedLayoutElement <>f__this;
            internal float <delta>__2;
            internal float <easedV>__3;
            internal float <source>__0;
            internal float <target>__1;
            internal float duration;
            internal Easing.Function easing;
            internal float targetHeight;

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
                        this.<>f__this.m_timer.set(this.duration);
                        this.<source>__0 = this.<>f__this.LayoutElement.minHeight;
                        this.<target>__1 = this.targetHeight;
                        this.<delta>__2 = this.<target>__1 - this.<source>__0;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0116;
                }
                if (!this.<>f__this.m_timer.Idle)
                {
                    this.<easedV>__3 = Easing.Apply(this.<>f__this.m_timer.normalizedProgress(), this.easing);
                    this.<>f__this.LayoutElement.minHeight = this.<source>__0 + (this.<delta>__2 * this.<easedV>__3);
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.LayoutElement.minHeight = this.<target>__1;
                goto Label_0116;
                this.$PC = -1;
            Label_0116:
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

        [CompilerGenerated]
        private sealed class <collapseAndExpandRoutine>c__Iterator1F4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal System.Action <$>collapseCompletionCallback;
            internal System.Action <$>expandStartCallback;
            internal bool <$>instant;
            internal float <$>targetHeight;
            internal AnimatedLayoutElement <>f__this;
            internal IEnumerator <ie>__0;
            internal IEnumerator <ie>__1;
            internal System.Action collapseCompletionCallback;
            internal System.Action expandStartCallback;
            internal bool instant;
            internal float targetHeight;

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
                        if (!this.instant)
                        {
                            this.<ie>__0 = this.<>f__this.animationRoutine(this.<>f__this.CollapseAnimationDuration, 0f, Easing.Function.IN_CUBIC);
                            break;
                        }
                        this.<>f__this.LayoutElement.minHeight = 0f;
                        goto Label_009E;

                    case 1:
                        break;

                    case 2:
                        goto Label_0135;

                    default:
                        goto Label_0158;
                }
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_015A;
                }
            Label_009E:
                if (this.collapseCompletionCallback != null)
                {
                    this.collapseCompletionCallback();
                }
                if (this.expandStartCallback != null)
                {
                    this.expandStartCallback();
                }
                if (this.instant)
                {
                    this.<>f__this.LayoutElement.minHeight = this.targetHeight;
                    goto Label_0145;
                }
                this.<ie>__1 = this.<>f__this.animationRoutine(this.<>f__this.ExpandAnimationDuration, this.targetHeight, Easing.Function.OUT_CUBIC);
            Label_0135:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_015A;
                }
            Label_0145:
                this.<>f__this.m_animationRoutine = null;
                this.$PC = -1;
            Label_0158:
                return false;
            Label_015A:
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
        private sealed class <collapseRoutine>c__Iterator1F6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal System.Action <$>completionCallback;
            internal AnimatedLayoutElement <>f__this;
            internal IEnumerator <ie>__0;
            internal System.Action completionCallback;

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
                        this.<ie>__0 = this.<>f__this.animationRoutine(this.<>f__this.CollapseAnimationDuration, 0f, Easing.Function.IN_CUBIC);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00A3;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                if (this.completionCallback != null)
                {
                    this.completionCallback();
                }
                this.<>f__this.m_animationRoutine = null;
                goto Label_00A3;
                this.$PC = -1;
            Label_00A3:
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

        [CompilerGenerated]
        private sealed class <expandRoutine>c__Iterator1F5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal System.Action <$>startCallback;
            internal float <$>targetHeight;
            internal AnimatedLayoutElement <>f__this;
            internal IEnumerator <ie>__0;
            internal System.Action startCallback;
            internal float targetHeight;

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
                        if (this.startCallback != null)
                        {
                            this.startCallback();
                        }
                        this.<ie>__0 = this.<>f__this.animationRoutine(this.<>f__this.ExpandAnimationDuration, this.targetHeight, Easing.Function.OUT_CUBIC);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00A4;
                }
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.m_animationRoutine = null;
                goto Label_00A4;
                this.$PC = -1;
            Label_00A4:
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

