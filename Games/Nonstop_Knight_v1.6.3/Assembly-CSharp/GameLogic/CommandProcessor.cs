namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CommandProcessor : ICommandProcessor
    {
        private ICoroutineExecutor m_ce;

        public CommandProcessor(ICoroutineExecutor coroutineExecutor)
        {
            this.m_ce = coroutineExecutor;
        }

        public virtual Coroutine execute(ICommand command, [Optional, DefaultParameterValue(0f)] float delaySeconds)
        {
            return this.m_ce.StartCoroutine(this.executeRoutine(command, delaySeconds));
        }

        public Coroutine executeCharacterSpecific(CharacterInstance character, ICommand command, [Optional, DefaultParameterValue(0)] float delaySeconds)
        {
            return character.PhysicsBody.StartCoroutine(this.executeRoutine(command, delaySeconds));
        }

        [DebuggerHidden]
        private IEnumerator executeRoutine(ICommand cmd, float delaySeconds)
        {
            <executeRoutine>c__Iterator43 iterator = new <executeRoutine>c__Iterator43();
            iterator.delaySeconds = delaySeconds;
            iterator.cmd = cmd;
            iterator.<$>delaySeconds = delaySeconds;
            iterator.<$>cmd = cmd;
            return iterator;
        }

        public void stopCommand(MonoBehaviour owner, ref Coroutine commandCoroutine)
        {
            if (commandCoroutine != null)
            {
                owner.StopCoroutine(commandCoroutine);
                commandCoroutine = null;
            }
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator43 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ICommand <$>cmd;
            internal float <$>delaySeconds;
            internal IEnumerator <ie>__1;
            internal ManualTimer <timer>__0;
            internal ICommand cmd;
            internal float delaySeconds;

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
                        if (this.delaySeconds <= 0f)
                        {
                            goto Label_007F;
                        }
                        this.<timer>__0 = new ManualTimer(this.delaySeconds);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00B2;

                    default:
                        goto Label_00C9;
                }
                if (!this.<timer>__0.Idle)
                {
                    this.<timer>__0.tick(Time.fixedDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_00CB;
                }
            Label_007F:
                this.<ie>__1 = this.cmd.executeRoutine();
            Label_00B2:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_00CB;
                }
                this.$PC = -1;
            Label_00C9:
                return false;
            Label_00CB:
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

