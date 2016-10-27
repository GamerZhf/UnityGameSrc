namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class WhirlwindSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorDD rdd = new <ExecuteRoutine>c__IteratorDD();
            rdd.c = c;
            rdd.executionStats = executionStats;
            rdd.<$>c = c;
            rdd.<$>executionStats = executionStats;
            return rdd;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorDD : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal Room <activeRoom>__0;
            internal float <angle>__28;
            internal double <damage>__6;
            internal HashSet<CharacterInstance> <damagedEnemies>__12;
            internal float <damagePct>__7;
            internal double <damagePerSpin>__10;
            internal Vector3 <direction>__18;
            internal float <duration>__3;
            internal float <easedV>__27;
            internal List<CharacterInstance> <enemiesHit>__20;
            internal CharacterInstance <enemy>__22;
            internal bool <hasHugeRune>__2;
            internal bool <hasShieldRune>__1;
            internal int <i>__21;
            internal int <j>__24;
            internal int <j>__25;
            internal float <knockBackForce>__8;
            internal int <loopCount>__17;
            internal ConfigPerks.SharedData <perkData>__9;
            internal List<KeyValuePair<PerkInstance, BuffSource>> <perks>__23;
            internal Quaternion <probeDirRotation>__15;
            internal Vector3 <pushDirectionXz>__26;
            internal float <radius>__4;
            internal Ray <ray>__19;
            internal ManualTimer <spinTimer>__14;
            internal Vector3 <startForward>__16;
            internal float <targetTotalRotation>__11;
            internal int <totalSpinCount>__5;
            internal ManualTimer <totalTimer>__13;
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
                        this.<activeRoom>__0 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom;
                        this.<hasShieldRune>__1 = this.c.getPerkInstanceCount(PerkType.SkillUpgradeWhirlwind1) > 0;
                        this.<hasHugeRune>__2 = this.c.getPerkInstanceCount(PerkType.SkillUpgradeWhirlwind4) > 0;
                        this.<duration>__3 = ConfigSkills.Whirlwind.Duration;
                        this.<radius>__4 = ConfigSkills.Whirlwind.Radius;
                        this.<totalSpinCount>__5 = ConfigSkills.Whirlwind.TotalSpinCount;
                        if (this.<hasShieldRune>__1)
                        {
                            this.<duration>__3 = Mathf.Max(this.<duration>__3, ConfigSkills.Whirlwind.ShieldRuneDuration);
                            this.<totalSpinCount>__5 = Mathf.Max(this.<totalSpinCount>__5, ConfigSkills.Whirlwind.ShieldRuneTotalSpinCount);
                        }
                        if (this.<hasHugeRune>__2)
                        {
                            this.<duration>__3 = Mathf.Max(this.<duration>__3, ConfigSkills.Whirlwind.HugeRuneDuration);
                            this.<radius>__4 = Mathf.Max(this.<radius>__4, ConfigSkills.Whirlwind.HugeRuneRadius);
                            this.<totalSpinCount>__5 = Mathf.Max(this.<totalSpinCount>__5, ConfigSkills.Whirlwind.HugeRuneTotalSpinCount);
                        }
                        this.<damage>__6 = MathUtil.ClampMin(this.c.SkillDamage(true), 1.0);
                        this.<damagePct>__7 = !this.c.IsBoss ? ConfigSkills.Whirlwind.TotalDamagePct : ConfigSkills.BossWhirlwind.TotalDamagePct;
                        if (this.<hasHugeRune>__2)
                        {
                            this.<damagePct>__7 = Mathf.Max(this.<damagePct>__7, !this.c.IsBoss ? ConfigSkills.Whirlwind.HugeRuneTotalDamagePct : ConfigSkills.BossWhirlwind.HugeRuneTotalDamagePct);
                        }
                        this.<damage>__6 *= this.<damagePct>__7;
                        this.<damage>__6 = CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(this.c, SkillType.Whirlwind, this.<damage>__6);
                        if (this.c.IsPet)
                        {
                            this.<damage>__6 = this.c.OwningPlayer.ActiveCharacter.DamagePerHit(false);
                            this.<radius>__4 *= 0.75f;
                        }
                        this.<knockBackForce>__8 = 0f;
                        if (this.<hasShieldRune>__1)
                        {
                            this.<perkData>__9 = ConfigPerks.SHARED_DATA[PerkType.SkillUpgradeWhirlwind1];
                            this.<knockBackForce>__8 = UnityEngine.Random.Range(this.<perkData>__9.ModifierMin, this.<perkData>__9.ModifierMax);
                        }
                        this.<damagePerSpin>__10 = this.<damage>__6 / ((double) this.<totalSpinCount>__5);
                        this.<targetTotalRotation>__11 = 360f * this.<totalSpinCount>__5;
                        this.<damagedEnemies>__12 = new HashSet<CharacterInstance>();
                        this.<totalTimer>__13 = new ManualTimer(this.<duration>__3);
                        this.<spinTimer>__14 = new ManualTimer(this.<duration>__3 / ((float) this.<totalSpinCount>__5));
                        this.<probeDirRotation>__15 = Quaternion.Euler(0f, -40f, 0f);
                        this.<startForward>__16 = this.c.PhysicsBody.Transform.forward;
                        this.<loopCount>__17 = 0;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0769;
                }
                while (!this.<totalTimer>__13.Idle)
                {
                    CmdHitDestructibles.ExecuteStatic(this.c, this.c.PhysicsBody.Transform.position, this.<radius>__4, SkillType.Whirlwind);
                    this.<direction>__18 = (Vector3) (this.<probeDirRotation>__15 * this.c.PhysicsBody.Transform.forward);
                    this.<ray>__19 = new Ray(this.c.PhysicsBody.Transform.position, this.<direction>__18);
                    this.<enemiesHit>__20 = this.<activeRoom>__0.getEnemyCharactersAlongLineSegment(this.<ray>__19, this.<radius>__4, this.c);
                    if ((this.<enemiesHit>__20 != null) && (this.<enemiesHit>__20.Count > 0))
                    {
                        this.<i>__21 = 0;
                        while (this.<i>__21 < this.<enemiesHit>__20.Count)
                        {
                            this.<enemy>__22 = this.<enemiesHit>__20[this.<i>__21];
                            if (!this.<damagedEnemies>__12.Contains(this.<enemy>__22))
                            {
                                this.<perks>__23 = CharacterStatModifierUtil.GetPerkInstancesOfType(this.c, PerkType.SkillUpgradeWhirlwind2);
                                this.<j>__24 = 0;
                                while (this.<j>__24 < this.<perks>__23.Count)
                                {
                                    KeyValuePair<PerkInstance, BuffSource> pair = this.<perks>__23[this.<j>__24];
                                    KeyValuePair<PerkInstance, BuffSource> pair2 = this.<perks>__23[this.<j>__24];
                                    GameLogic.Binder.BuffSystem.startOrRefreshBuffFromPerk(this.<enemy>__22, PerkType.SkillUpgradeWhirlwind2, ConfigPerks.SHARED_DATA[PerkType.SkillUpgradeWhirlwind2].DurationSeconds, (double) pair.Key.Modifier, pair2.Value, this.c);
                                    this.<j>__24++;
                                }
                                this.<perks>__23 = CharacterStatModifierUtil.GetPerkInstancesOfType(this.c, PerkType.SkillUpgradeWhirlwind3);
                                this.<j>__25 = 0;
                                while (this.<j>__25 < this.<perks>__23.Count)
                                {
                                    KeyValuePair<PerkInstance, BuffSource> pair3 = this.<perks>__23[this.<j>__25];
                                    KeyValuePair<PerkInstance, BuffSource> pair4 = this.<perks>__23[this.<j>__25];
                                    GameLogic.Binder.BuffSystem.startOrRefreshBuffFromPerk(this.<enemy>__22, PerkType.SkillUpgradeWhirlwind3, ConfigPerks.SHARED_DATA[PerkType.SkillUpgradeWhirlwind3].DurationSeconds, pair3.Key.Modifier * this.c.SkillDamage(true), pair4.Value, this.c);
                                    this.<j>__25++;
                                }
                                CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<enemy>__22, this.<damagePerSpin>__10, false, DamageType.Magic, SkillType.Whirlwind);
                                this.<damagedEnemies>__12.Add(this.<enemy>__22);
                                if (this.<enemy>__22.IsDead)
                                {
                                    this.executionStats.KillCount++;
                                }
                                else if (this.<knockBackForce>__8 > 0f)
                                {
                                    this.<pushDirectionXz>__26 = Vector3Extensions.ToXzVector3(this.<enemy>__22.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                                    GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<enemy>__22, new CmdPushCharacter(this.<enemy>__22, (Vector3) (this.<pushDirectionXz>__26 * this.<knockBackForce>__8)), 0f);
                                }
                            }
                            this.<i>__21++;
                        }
                    }
                    this.<easedV>__27 = Easing.Apply(this.<totalTimer>__13.normalizedProgress(), Easing.Function.OUT_QUAD);
                    this.<angle>__28 = this.<targetTotalRotation>__11 * this.<easedV>__27;
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, this.c.Velocity, (Vector3) (Quaternion.Euler(0f, this.<angle>__28, 0f) * this.<startForward>__16));
                    this.<totalTimer>__13.tick(Time.deltaTime);
                    this.<spinTimer>__14.tick(Time.deltaTime);
                    if (this.<spinTimer>__14.Idle && (this.<loopCount>__17 < this.<totalSpinCount>__5))
                    {
                        this.<loopCount>__17++;
                        this.<spinTimer>__14.reset();
                        this.<damagedEnemies>__12.Clear();
                        GameLogic.Binder.EventBus.CharacterSkillExecutionMidpoint(this.c, SkillType.Whirlwind, this.executionStats);
                    }
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                if ((this.<knockBackForce>__8 > 0f) && this.<activeRoom>__0.MainBossSummoned)
                {
                    CmdSetCharacterTarget.ExecuteStatic(this.c, this.<activeRoom>__0.getClosestEnemyBossCharacter(this.c, false), Vector3.zero);
                    goto Label_0769;
                    this.$PC = -1;
                }
            Label_0769:
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

