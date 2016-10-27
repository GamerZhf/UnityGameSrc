namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class BossEscaperSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorC3 rc = new <ExecuteRoutine>c__IteratorC3();
            rc.c = c;
            rc.rank = rank;
            rc.executionStats = executionStats;
            rc.<$>c = c;
            rc.<$>rank = rank;
            rc.<$>executionStats = executionStats;
            return rc;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorC3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal int <$>rank;
            internal IEnumerator <ie>__0;
            internal CharacterInstance c;
            internal SkillExecutionStats executionStats;
            internal int rank;

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
                        this.<ie>__0 = BossSummonerSharedSkill.ExecuteRoutine(SkillType.BossEscaper, this.c, this.rank, this.executionStats);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_007E;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_007E;
                this.$PC = -1;
            Label_007E:
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

