using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ManualTimer
{
    private bool m_idle;
    private float m_originalTimeSet;
    private float m_timeRemaining;

    public ManualTimer()
    {
        this.m_timeRemaining = 0f;
        this.m_idle = true;
    }

    public ManualTimer(float duration)
    {
        this.set(duration);
    }

    public void addAndClamp(float addedSeconds)
    {
        this.m_timeRemaining = Mathf.Clamp(this.m_timeRemaining + addedSeconds, 0f, this.m_originalTimeSet);
        if (this.m_timeRemaining > 0f)
        {
            this.m_idle = false;
        }
    }

    public void end()
    {
        this.m_timeRemaining = 0f;
        this.m_idle = true;
    }

    public float normalizedProgress()
    {
        if (this.m_originalTimeSet <= 0f)
        {
            return 1f;
        }
        return Mathf.Clamp01(this.timeElapsed() / this.m_originalTimeSet);
    }

    public float originalTimeSet()
    {
        return this.m_originalTimeSet;
    }

    public void reset()
    {
        this.m_timeRemaining = this.m_originalTimeSet;
        this.m_idle = false;
    }

    public void set(float duration)
    {
        this.m_originalTimeSet = duration;
        this.m_timeRemaining = duration;
        this.m_idle = false;
    }

    public bool tick(float dt)
    {
        if (!this.Idle)
        {
            this.m_timeRemaining -= dt;
            if (this.m_timeRemaining <= 0f)
            {
                this.end();
                return true;
            }
        }
        return false;
    }

    [DebuggerHidden]
    public IEnumerator tickUntilEndUnscaled()
    {
        <tickUntilEndUnscaled>c__Iterator37 iterator = new <tickUntilEndUnscaled>c__Iterator37();
        iterator.<>f__this = this;
        return iterator;
    }

    public float timeElapsed()
    {
        return (this.m_originalTimeSet - this.m_timeRemaining);
    }

    public float timeRemaining()
    {
        return this.m_timeRemaining;
    }

    public bool Idle
    {
        get
        {
            return this.m_idle;
        }
    }

    [CompilerGenerated]
    private sealed class <tickUntilEndUnscaled>c__Iterator37 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ManualTimer <>f__this;

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
                case 1:
                    if (!this.<>f__this.Idle)
                    {
                        if (!this.<>f__this.tick(Time.unscaledDeltaTime))
                        {
                            this.$current = null;
                            this.$PC = 1;
                            return true;
                            this.$PC = -1;
                        }
                        break;
                    }
                    break;
            }
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

