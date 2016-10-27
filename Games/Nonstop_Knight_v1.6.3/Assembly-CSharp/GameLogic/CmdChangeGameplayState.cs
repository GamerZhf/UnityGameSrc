namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class CmdChangeGameplayState : ICommand
    {
        private float m_delay;
        private GameplayState m_newState;

        public CmdChangeGameplayState(string[] serialized)
        {
            this.m_newState = (GameplayState) ((int) Enum.Parse(typeof(GameplayState), serialized[0]));
        }

        public CmdChangeGameplayState(GameplayState newState, [Optional, DefaultParameterValue(0f)] float delay)
        {
            this.m_newState = newState;
            this.m_delay = delay;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator4D iteratord = new <executeRoutine>c__Iterator4D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator4D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdChangeGameplayState <>f__this;
            internal ActiveDungeon <ad>__0;
            internal int <i>__2;
            internal IEnumerator <ie>__1;

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
                        this.<ad>__0 = Binder.GameState.ActiveDungeon;
                        Binder.EventBus.GameplayStateChangeStarted(this.<ad>__0.CurrentGameplayState, this.<>f__this.m_newState, this.<>f__this.m_delay);
                        if (this.<>f__this.m_delay <= 0f)
                        {
                            goto Label_00B9;
                        }
                        this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(this.<>f__this.m_delay);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_016C;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
            Label_00B9:
                this.<ad>__0.PreviousGameplayState = this.<ad>__0.CurrentGameplayState;
                this.<ad>__0.CurrentGameplayState = this.<>f__this.m_newState;
                this.<i>__2 = 0;
                while (this.<i>__2 < this.<ad>__0.ActiveRoom.ActiveCharacters.Count)
                {
                    CmdInterruptCharacter.ExecuteStatic(this.<ad>__0.ActiveRoom.ActiveCharacters[this.<i>__2], false);
                    this.<i>__2++;
                }
                Binder.EventBus.GameplayStateChanged(this.<ad>__0.PreviousGameplayState, this.<ad>__0.CurrentGameplayState);
                goto Label_016C;
                this.$PC = -1;
            Label_016C:
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

