namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdInspectSkill : ICommand
    {
        private SkillInstance m_skillInstance;

        public CmdInspectSkill(SkillInstance skillInstance)
        {
            this.m_skillInstance = skillInstance;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorA6 ra = new <executeRoutine>c__IteratorA6();
            ra.<>f__this = this;
            return ra;
        }

        public static void ExecuteStatic(SkillInstance skillInstance)
        {
            if (skillInstance != null)
            {
                skillInstance.InspectedByPlayer = true;
                Binder.EventBus.SkillInspected(skillInstance);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorA6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdInspectSkill <>f__this;

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
                    CmdInspectSkill.ExecuteStatic(this.<>f__this.m_skillInstance);
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

