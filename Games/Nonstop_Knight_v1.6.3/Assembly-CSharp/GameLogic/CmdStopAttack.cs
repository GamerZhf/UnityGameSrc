namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdStopAttack : ICommand
    {
        private CharacterInstance m_character;

        public CmdStopAttack(CharacterInstance character)
        {
            this.m_character = character;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator6F iteratorf = new <executeRoutine>c__Iterator6F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public static void ExecuteStatic(CharacterInstance c)
        {
            Binder.CommandProcessor.stopCommand(c.PhysicsBody, ref c.AttackRoutine);
            Binder.EventBus.CharacterAttackStopped(c);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator6F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdStopAttack <>f__this;

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
                    CmdStopAttack.ExecuteStatic(this.<>f__this.m_character);
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

