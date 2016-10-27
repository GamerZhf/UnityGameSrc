namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GameplayBossFightState : FiniteStateMachine.State
    {
        private Coroutine m_masterRoutine;

        public GameplayBossFightState() : base(Enum.GetName(typeof(GameplayState), GameplayState.BOSS_FIGHT), 6)
        {
            base.EnterMethod = new Action(this.onEnter);
            base.UpdateMethod = new Action<float>(this.onUpdate);
            base.ExitMethod = new Action(this.onExit);
        }

        [DebuggerHidden]
        private IEnumerator masterRoutine()
        {
            return new <masterRoutine>c__Iterator72();
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
        private sealed class <masterRoutine>c__Iterator72 : IEnumerator, IDisposable, IEnumerator<object>
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
                this.$PC = -1;
                if (this.$PC == 0)
                {
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

