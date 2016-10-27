using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class FiniteStateMachine
{
    private Coroutine m_changeRoutine;
    private State m_currentState;
    private State m_globalState;
    private Action m_onPreStateChange;
    private Action m_onStateChanged;
    private MonoBehaviour m_owner;
    private State m_previousState;
    private State m_queuedState;

    public FiniteStateMachine(MonoBehaviour owner)
    {
        this.m_owner = owner;
    }

    [DebuggerHidden]
    private IEnumerator changeRoutine(State newState, float delaySeconds)
    {
        <changeRoutine>c__Iterator36 iterator = new <changeRoutine>c__Iterator36();
        iterator.newState = newState;
        iterator.delaySeconds = delaySeconds;
        iterator.<$>newState = newState;
        iterator.<$>delaySeconds = delaySeconds;
        iterator.<>f__this = this;
        return iterator;
    }

    public void changeState(State newState, [Optional, DefaultParameterValue(0f)] float delaySeconds)
    {
        if (UnityUtils.CoroutineRunning(ref this.m_changeRoutine))
        {
            UnityEngine.Debug.LogError("FSM state change already ongoing, aborting previous..");
            UnityUtils.StopCoroutine(this.m_owner, ref this.m_changeRoutine);
        }
        this.m_changeRoutine = this.m_owner.StartCoroutine(this.changeRoutine(newState, delaySeconds));
    }

    public void clear()
    {
        this.m_currentState = null;
        this.m_previousState = null;
        this.m_globalState = null;
        this.m_queuedState = null;
    }

    public bool isInState(State state)
    {
        return (this.m_currentState == state);
    }

    public void update(float dt)
    {
        if ((this.m_globalState != null) && (this.m_globalState.UpdateMethod != null))
        {
            this.m_globalState.UpdateMethod(dt);
        }
        if ((this.m_currentState != null) && (this.m_currentState.UpdateMethod != null))
        {
            this.m_currentState.UpdateMethod(dt);
        }
    }

    public State CurrentState
    {
        get
        {
            return this.m_currentState;
        }
    }

    public State GlobalState
    {
        get
        {
            return this.m_globalState;
        }
        set
        {
            this.m_globalState = value;
        }
    }

    public Action OnPreStateChange
    {
        set
        {
            this.m_onPreStateChange = value;
        }
    }

    public Action OnStateChanged
    {
        set
        {
            this.m_onStateChanged = value;
        }
    }

    public State PreviousState
    {
        get
        {
            return this.m_previousState;
        }
    }

    public State QueuedState
    {
        get
        {
            return this.m_queuedState;
        }
    }

    [CompilerGenerated]
    private sealed class <changeRoutine>c__Iterator36 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>delaySeconds;
        internal FiniteStateMachine.State <$>newState;
        internal FiniteStateMachine <>f__this;
        internal ManualTimer <timer>__0;
        internal float delaySeconds;
        internal FiniteStateMachine.State newState;

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
                    this.<>f__this.m_queuedState = this.newState;
                    if (this.delaySeconds <= 0f)
                    {
                        goto Label_0094;
                    }
                    this.<timer>__0 = new ManualTimer(this.delaySeconds);
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_0155;

                case 3:
                    goto Label_01E1;

                default:
                    goto Label_0214;
            }
            if (!this.<timer>__0.Idle)
            {
                this.<timer>__0.tick(Time.fixedDeltaTime);
                this.$current = null;
                this.$PC = 1;
                goto Label_0216;
            }
        Label_0094:
            if (this.<>f__this.m_onPreStateChange != null)
            {
                this.<>f__this.m_onPreStateChange();
            }
            this.<>f__this.m_previousState = this.<>f__this.m_currentState;
            if (this.<>f__this.m_currentState != null)
            {
                if (this.<>f__this.m_currentState.ExitMethod != null)
                {
                    this.<>f__this.m_currentState.ExitMethod();
                }
                else if (this.<>f__this.m_currentState.ExitRoutine != null)
                {
                    this.$current = this.<>f__this.m_owner.StartCoroutine(this.<>f__this.m_currentState.ExitRoutine());
                    this.$PC = 2;
                    goto Label_0216;
                }
            }
        Label_0155:
            this.<>f__this.m_currentState = this.newState;
            if (this.<>f__this.m_currentState.EnterMethod != null)
            {
                this.<>f__this.m_currentState.EnterMethod();
            }
            else if (this.<>f__this.m_currentState.EnterRoutine != null)
            {
                this.$current = this.<>f__this.m_owner.StartCoroutine(this.<>f__this.m_currentState.EnterRoutine());
                this.$PC = 3;
                goto Label_0216;
            }
        Label_01E1:
            if (this.<>f__this.m_onStateChanged != null)
            {
                this.<>f__this.m_onStateChanged();
            }
            this.<>f__this.m_changeRoutine = null;
            this.$PC = -1;
        Label_0214:
            return false;
        Label_0216:
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

    public class State
    {
        private Action m_enterMethod;
        private Func<IEnumerator> m_enterRoutine;
        private Action m_exitMethod;
        private Func<IEnumerator> m_exitRoutine;
        private int m_ID;
        private string m_name;
        private Action<float> m_updateMethod;

        public State([Optional, DefaultParameterValue("UNSPECIFIED")] string name, [Optional, DefaultParameterValue(-1)] int ID)
        {
            this.m_name = name;
            this.m_ID = ID;
        }

        public State(Action<float> updateMethod, [Optional, DefaultParameterValue("UNSPECIFIED")] string name, [Optional, DefaultParameterValue(-1)] int ID)
        {
            this.m_enterMethod = null;
            this.m_updateMethod = updateMethod;
            this.m_exitMethod = null;
            this.m_name = name;
            this.m_ID = ID;
        }

        public State(Action enterMethod, Action<float> updateMethod, Action exitMethod, [Optional, DefaultParameterValue("UNSPECIFIED")] string name, [Optional, DefaultParameterValue(-1)] int ID)
        {
            this.m_enterMethod = enterMethod;
            this.m_updateMethod = updateMethod;
            this.m_exitMethod = exitMethod;
            this.m_name = name;
            this.m_ID = ID;
        }

        public Action EnterMethod
        {
            get
            {
                return this.m_enterMethod;
            }
            set
            {
                this.m_enterMethod = value;
            }
        }

        public Func<IEnumerator> EnterRoutine
        {
            get
            {
                return this.m_enterRoutine;
            }
            set
            {
                this.m_enterRoutine = value;
            }
        }

        public Action ExitMethod
        {
            get
            {
                return this.m_exitMethod;
            }
            set
            {
                this.m_exitMethod = value;
            }
        }

        public Func<IEnumerator> ExitRoutine
        {
            get
            {
                return this.m_exitRoutine;
            }
            set
            {
                this.m_exitRoutine = value;
            }
        }

        public int ID
        {
            get
            {
                return this.m_ID;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
        }

        public Action<float> UpdateMethod
        {
            get
            {
                return this.m_updateMethod;
            }
            set
            {
                this.m_updateMethod = value;
            }
        }
    }
}

