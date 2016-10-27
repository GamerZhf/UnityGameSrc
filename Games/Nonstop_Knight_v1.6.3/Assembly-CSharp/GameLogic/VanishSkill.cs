namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class VanishSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorDC rdc = new <ExecuteRoutine>c__IteratorDC();
            rdc.c = c;
            rdc.executionStats = executionStats;
            rdc.<$>c = c;
            rdc.<$>executionStats = executionStats;
            return rdc;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorDC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal double <damage>__0;
            internal float <duration>__5;
            internal List<CharacterInstance> <enemiesAroundUs>__1;
            internal CharacterInstance <enemy>__3;
            internal int <i>__2;
            internal IEnumerator <ie>__6;
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
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<damage>__0 = this.c.SkillDamage(true) * ConfigSkills.Vanish.BlastDamagePct;
                        this.<enemiesAroundUs>__1 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getEnemyCharactersWithinRadius(this.c.PhysicsBody.Transform.position, ConfigSkills.Vanish.BlastRadius, this.c);
                        this.<i>__2 = 0;
                        while (this.<i>__2 < this.<enemiesAroundUs>__1.Count)
                        {
                            this.<enemy>__3 = this.<enemiesAroundUs>__1[this.<i>__2];
                            this.<pushDirectionXz>__4 = Vector3Extensions.ToXzVector3(this.<enemy>__3.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                            GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<enemy>__3, new CmdPushCharacter(this.<enemy>__3, (Vector3) (this.<pushDirectionXz>__4 * ConfigSkills.Vanish.BlastPushForce)), 0f);
                            CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<enemy>__3, this.<damage>__0, false, DamageType.Magic, SkillType.Vanish);
                            if (this.<enemy>__3.IsDead)
                            {
                                this.executionStats.KillCount++;
                            }
                            this.<i>__2++;
                        }
                        this.<duration>__5 = ConfigSkills.Vanish.Duration;
                        this.<ie>__6 = TimeUtil.WaitForFixedSeconds(this.<duration>__5);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_01C6;
                }
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_01C6;
                this.$PC = -1;
            Label_01C6:
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

