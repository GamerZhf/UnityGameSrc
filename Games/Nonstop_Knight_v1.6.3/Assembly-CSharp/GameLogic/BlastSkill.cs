namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BlastSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorBF rbf = new <ExecuteRoutine>c__IteratorBF();
            rbf.c = c;
            rbf.executionStats = executionStats;
            rbf.<$>c = c;
            rbf.<$>executionStats = executionStats;
            return rbf;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorBF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal double <damage>__0;
            internal List<CharacterInstance> <enemiesAroundUs>__1;
            internal CharacterInstance <enemy>__3;
            internal int <i>__2;
            internal Vector3 <pushDirectionXz>__4;
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
                if ((this.$PC == 0) && !this.c.IsDead)
                {
                    this.<damage>__0 = MathUtil.ClampMin(this.c.SkillDamage(true), 1.0) * ConfigSkills.Blast.DamagePct;
                    this.<enemiesAroundUs>__1 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getEnemyCharactersWithinRadius(this.c.PhysicsBody.Transform.position, ConfigSkills.Blast.Radius, this.c);
                    this.<i>__2 = 0;
                    while (this.<i>__2 < this.<enemiesAroundUs>__1.Count)
                    {
                        this.<enemy>__3 = this.<enemiesAroundUs>__1[this.<i>__2];
                        this.<pushDirectionXz>__4 = Vector3Extensions.ToXzVector3(this.<enemy>__3.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                        GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<enemy>__3, new CmdPushCharacter(this.<enemy>__3, (Vector3) (this.<pushDirectionXz>__4 * ConfigSkills.Blast.PushForce)), 0f);
                        CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<enemy>__3, this.<damage>__0, false, DamageType.Magic, SkillType.Blast);
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

