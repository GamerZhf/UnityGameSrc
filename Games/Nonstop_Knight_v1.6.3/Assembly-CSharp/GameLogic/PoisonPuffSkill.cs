namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class PoisonPuffSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorD9 rd = new <ExecuteRoutine>c__IteratorD9();
            rd.c = c;
            rd.executionStats = executionStats;
            rd.<$>c = c;
            rd.<$>executionStats = executionStats;
            return rd;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorD9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal double <damage>__0;
            internal List<CharacterInstance> <enemiesAroundUs>__1;
            internal CharacterInstance <enemy>__3;
            internal int <i>__2;
            internal CharacterInstance c;
            internal SkillExecutionStats executionStats;

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
                    this.<damage>__0 = MathUtil.ClampMin(this.c.DamagePerHit(true), 1.0);
                    this.<enemiesAroundUs>__1 = Binder.GameState.ActiveDungeon.ActiveRoom.getEnemyCharactersWithinRadius(this.c.PhysicsBody.Transform.position, this.c.Character.getBaseStatFloat(BaseStatProperty.AttackRange), this.c);
                    this.<i>__2 = 0;
                    while (this.<i>__2 < this.<enemiesAroundUs>__1.Count)
                    {
                        this.<enemy>__3 = this.<enemiesAroundUs>__1[this.<i>__2];
                        CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<enemy>__3, this.<damage>__0, false, DamageType.Magic, SkillType.PoisonPuff);
                        if (this.<enemy>__3.IsDead)
                        {
                            this.executionStats.KillCount++;
                        }
                        this.<i>__2++;
                    }
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

