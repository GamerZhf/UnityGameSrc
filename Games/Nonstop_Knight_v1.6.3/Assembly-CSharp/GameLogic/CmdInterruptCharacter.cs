namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdInterruptCharacter : ICommand
    {
        private CharacterInstance m_character;
        private bool m_stopSkills;

        public CmdInterruptCharacter(CharacterInstance character, bool stopSkills)
        {
            this.m_character = character;
            this.m_stopSkills = stopSkills;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator5A iteratora = new <executeRoutine>c__Iterator5A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public static void ExecuteStatic(CharacterInstance c, bool stopSkills)
        {
            CmdStopAttack.ExecuteStatic(c);
            if (stopSkills)
            {
                List<SkillType> list = new List<SkillType>(c.SkillRoutines.Keys);
                for (int i = 0; i < list.Count; i++)
                {
                    CmdStopSkill.ExecuteStatic(c, list[i]);
                }
            }
            if (c.isBlinking())
            {
                Binder.CommandProcessor.stopCommand(c.PhysicsBody, ref c.BlinkRoutine);
                c.ExternallyControlled = false;
            }
            Binder.EventBus.CharacterInterrupted(c, stopSkills);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator5A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdInterruptCharacter <>f__this;

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
                    CmdInterruptCharacter.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_stopSkills);
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

