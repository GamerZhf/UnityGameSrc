namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class HealSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorD2 rd = new <ExecuteRoutine>c__IteratorD2();
            rd.executionStats = executionStats;
            rd.<$>executionStats = executionStats;
            return rd;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorD2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SkillExecutionStats <$>executionStats;
            internal double <amount>__0;
            internal SkillExecutionStats executionStats;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if ((this.$PC == 0) && !this.executionStats.TargetCharacter.IsDead)
                {
                    this.<amount>__0 = this.executionStats.TargetCharacter.MaxLife(true) * ConfigSkills.Heal.LifeGainPct;
                    CmdGainHp.ExecuteStatic(this.executionStats.TargetCharacter, this.<amount>__0, false);
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

