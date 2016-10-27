namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GameplayWaitingState : FiniteStateMachine.State
    {
        private Coroutine m_masterRoutine;

        public GameplayWaitingState() : base(Enum.GetName(typeof(GameplayState), GameplayState.WAITING), 3)
        {
            base.EnterMethod = new Action(this.onEnter);
            base.UpdateMethod = new Action<float>(this.onUpdate);
            base.ExitMethod = new Action(this.onExit);
        }

        [DebuggerHidden]
        private IEnumerator masterRoutine()
        {
            return new <masterRoutine>c__Iterator74();
        }

        public void onEnter()
        {
            this.m_masterRoutine = UnityUtils.StartCoroutine(Binder.GameplayStateMachine.MonoBehaviour, this.masterRoutine());
        }

        public void onExit()
        {
            UnityUtils.StopCoroutine(Binder.GameplayStateMachine.MonoBehaviour, ref this.m_masterRoutine);
        }

        public void onUpdate(float dt)
        {
        }

        [CompilerGenerated]
        private sealed class <masterRoutine>c__Iterator74 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
                        this.$current = null;
                        this.$PC = 1;
                        return true;

                    default:
                        break;
                        this.$PC = -1;
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
}

