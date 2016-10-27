namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ImplosionSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorD3 rd = new <ExecuteRoutine>c__IteratorD3();
            rd.c = c;
            rd.executionStats = executionStats;
            rd.<$>c = c;
            rd.<$>executionStats = executionStats;
            return rd;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorD3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal double <damage>__11;
            internal Buff <debuff>__14;
            internal float <distToEnemy>__3;
            internal float <distV>__4;
            internal float <elapsed>__6;
            internal List<CharacterInstance> <enemiesAroundUs>__0;
            internal CharacterInstance <enemy>__13;
            internal CharacterInstance <enemy>__2;
            internal bool <hasStunRune>__9;
            internal float <hpGainPctOfDamageDealt>__15;
            internal int <i>__1;
            internal int <i>__12;
            internal int <i>__16;
            internal IEnumerator <ie>__7;
            internal bool <midpointSignaled>__8;
            internal int <numCritters>__17;
            internal Vector3 <pushDirectionXz>__5;
            internal float <stunDuration>__10;
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
                        CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                        this.<enemiesAroundUs>__0 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getEnemyCharactersWithinRadius(this.c.PhysicsBody.Transform.position, ConfigSkills.Implosion.Radius, this.c);
                        this.<i>__1 = 0;
                        while (this.<i>__1 < this.<enemiesAroundUs>__0.Count)
                        {
                            this.<enemy>__2 = this.<enemiesAroundUs>__0[this.<i>__1];
                            this.<distToEnemy>__3 = (PhysicsUtil.DistBetween(this.c, this.<enemy>__2) - this.c.Radius) - this.<enemy>__2.Radius;
                            this.<distV>__4 = Mathf.Clamp01(this.<distToEnemy>__3 / ConfigSkills.Implosion.Radius);
                            this.<pushDirectionXz>__5 = -Vector3Extensions.ToXzVector3(this.<enemy>__2.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                            GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<enemy>__2, new CmdPushCharacter(this.<enemy>__2, (Vector3) ((this.<pushDirectionXz>__5 * ConfigSkills.Implosion.DrawForceMax) * this.<distV>__4)), 0f);
                            this.<i>__1++;
                        }
                        this.<elapsed>__6 = 0f;
                        this.<ie>__7 = TimeUtil.WaitForFixedSeconds(ConfigSkills.Implosion.DrawDuration);
                        this.<midpointSignaled>__8 = false;
                        while (this.<ie>__7.MoveNext())
                        {
                            this.$current = this.<ie>__7.Current;
                            this.$PC = 1;
                            return true;
                        Label_01CA:
                            this.<elapsed>__6 += Time.unscaledDeltaTime;
                            if (!this.<midpointSignaled>__8 && (this.<elapsed>__6 >= (ConfigSkills.Implosion.DrawDuration * 0.5f)))
                            {
                                GameLogic.Binder.EventBus.CharacterSkillExecutionMidpoint(this.c, SkillType.Implosion, this.executionStats);
                                this.<midpointSignaled>__8 = true;
                            }
                        }
                        this.<hasStunRune>__9 = this.c.getPerkInstanceCount(PerkType.SkillUpgradeImplosion3) > 0;
                        this.<stunDuration>__10 = !this.<hasStunRune>__9 ? 0f : this.c.getGenericModifierForPerkType(PerkType.SkillUpgradeImplosion3);
                        CmdHitDestructibles.ExecuteStatic(this.c, this.c.PhysicsBody.Transform.position, ConfigSkills.Implosion.DestructibleBlastRadius, SkillType.Implosion);
                        this.<damage>__11 = MathUtil.ClampMin(this.c.SkillDamage(true), 1.0) * ConfigSkills.Implosion.DamagePct;
                        this.<damage>__11 = CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(this.c, SkillType.Implosion, this.<damage>__11);
                        this.<i>__12 = 0;
                        while (this.<i>__12 < this.<enemiesAroundUs>__0.Count)
                        {
                            this.<enemy>__13 = this.<enemiesAroundUs>__0[this.<i>__12];
                            if (this.<hasStunRune>__9)
                            {
                                Buff buff = new Buff();
                                buff.Stuns = true;
                                buff.DurationSeconds = this.<stunDuration>__10;
                                this.<debuff>__14 = buff;
                                GameLogic.Binder.BuffSystem.startBuff(this.<enemy>__13, this.<debuff>__14);
                            }
                            CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<enemy>__13, this.<damage>__11, false, DamageType.Magic, SkillType.Implosion);
                            if (this.<enemy>__13.IsDead)
                            {
                                this.executionStats.KillCount++;
                            }
                            this.<i>__12++;
                        }
                        if (this.c.getPerkInstanceCount(PerkType.SkillUpgradeImplosion4) > 0)
                        {
                            this.<hpGainPctOfDamageDealt>__15 = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeImplosion4);
                            this.<i>__16 = 0;
                            while (this.<i>__16 < this.<enemiesAroundUs>__0.Count)
                            {
                                GameLogic.Binder.CommandProcessor.execute(new CmdGainHp(this.c, this.<damage>__11 * this.<hpGainPctOfDamageDealt>__15, false, this.<i>__16 * 0.1f), 0f);
                                this.<i>__16++;
                            }
                        }
                        if (this.c.getPerkInstanceCount(PerkType.SkillUpgradeImplosion1) > 0)
                        {
                            this.<numCritters>__17 = (int) this.c.getGenericModifierForPerkType(PerkType.SkillUpgradeImplosion1);
                            GameLogic.Binder.CharacterSpawningSystem.summonSupportCritters(this.c, this.<numCritters>__17, null);
                            break;
                            this.$PC = -1;
                        }
                        break;

                    case 1:
                        goto Label_01CA;
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

