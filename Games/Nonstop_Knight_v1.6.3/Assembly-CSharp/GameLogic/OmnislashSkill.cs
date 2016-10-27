namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class OmnislashSkill
    {
        private static void ApplyRunestoneBuffs(CharacterInstance source, CharacterInstance target)
        {
            List<KeyValuePair<PerkInstance, BuffSource>> perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(source, PerkType.SkillUpgradeOmnislash4);
            for (int i = 0; i < perkInstancesOfType.Count; i++)
            {
                KeyValuePair<PerkInstance, BuffSource> pair = perkInstancesOfType[i];
                KeyValuePair<PerkInstance, BuffSource> pair2 = perkInstancesOfType[i];
                GameLogic.Binder.BuffSystem.startOrRefreshBuffFromPerk(target, PerkType.SkillUpgradeOmnislash4, ConfigPerks.SHARED_DATA[PerkType.SkillUpgradeOmnislash4].DurationSeconds, pair.Key.Modifier * source.SkillDamage(true), pair2.Value, source);
            }
            perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(source, PerkType.SkillUpgradeOmnislash1);
            for (int j = 0; j < perkInstancesOfType.Count; j++)
            {
                KeyValuePair<PerkInstance, BuffSource> pair3 = perkInstancesOfType[j];
                KeyValuePair<PerkInstance, BuffSource> pair4 = perkInstancesOfType[j];
                GameLogic.Binder.BuffSystem.startOrRefreshBuffFromPerk(target, PerkType.SkillUpgradeOmnislash1, ConfigPerks.SHARED_DATA[PerkType.SkillUpgradeOmnislash1].DurationSeconds, (double) pair3.Key.Modifier, pair4.Value, source);
            }
        }

        public static double CalculateTotalDamage(CharacterInstance c, out float baseTotalDamageMultiplier, out float baseTotalDamageMultiplierIncreasePerExtraTarget, out int numExtraTargets, out int slashCount, out double baseDamage, out double bonusDamage)
        {
            numExtraTargets = 0;
            baseTotalDamageMultiplier = ConfigSkills.Omnislash.TotalDamageMultiplier;
            baseTotalDamageMultiplierIncreasePerExtraTarget = ConfigSkills.Omnislash.TotalDamageMultiplierIncreasePerExtraTarget;
            slashCount = ConfigSkills.Omnislash.SlashCount + numExtraTargets;
            if (c.IsPet)
            {
                baseDamage = c.OwningPlayer.ActiveCharacter.DamagePerHit(false);
                bonusDamage = 0.0;
                return baseDamage;
            }
            float num = baseTotalDamageMultiplier + (baseTotalDamageMultiplierIncreasePerExtraTarget * ((float) numExtraTargets));
            baseDamage = MathUtil.ClampMin(c.SkillDamage(true), 1.0) * num;
            double num2 = CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(c, SkillType.Omnislash, baseDamage);
            bonusDamage = num2 - baseDamage;
            return num2;
        }

        public static bool DashModificationEnabled(CharacterInstance c)
        {
            return ((c.getPerkInstanceCount(PerkType.SkillUpgradeOmnislash2) > 0) || (c.Character.CoreAiBehaviour == AiBehaviourType.SupportDasher));
        }

        [DebuggerHidden]
        private static IEnumerator ExecuteDash(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteDash>c__IteratorD8 rd = new <ExecuteDash>c__IteratorD8();
            rd.c = c;
            rd.executionStats = executionStats;
            rd.<$>c = c;
            rd.<$>executionStats = executionStats;
            return rd;
        }

        [DebuggerHidden]
        private static IEnumerator ExecuteDefault(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteDefault>c__IteratorD7 rd = new <ExecuteDefault>c__IteratorD7();
            rd.c = c;
            rd.executionStats = executionStats;
            rd.<$>c = c;
            rd.<$>executionStats = executionStats;
            return rd;
        }

        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorD6 rd = new <ExecuteRoutine>c__IteratorD6();
            rd.c = c;
            rd.rank = rank;
            rd.executionStats = executionStats;
            rd.<$>c = c;
            rd.<$>rank = rank;
            rd.<$>executionStats = executionStats;
            return rd;
        }

        public static void PreExecute(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (DashModificationEnabled(c))
            {
                Vector3 forward = c.PhysicsBody.Transform.forward;
                float upgradedDashMovementForce = ConfigSkills.Omnislash.UpgradedDashMovementForce;
                CharacterInstance b = activeDungeon.ActiveRoom.getEnemyCharacterWithHighestThreat(c, true);
                if ((b != null) && (PhysicsUtil.DistBetween(c, b) <= ConfigSkills.Omnislash.UpgradedDashClosestEnemyDistanceThreshold))
                {
                    Vector3 vector2 = b.PhysicsBody.Transform.position - c.PhysicsBody.Transform.position;
                    forward = Vector3Extensions.ToXzVector3(vector2.normalized);
                }
                executionStats.MovementDir = forward;
                executionStats.MovementForce = upgradedDashMovementForce;
            }
            else
            {
                List<CharacterInstance> list = activeDungeon.ActiveRoom.getEnemyCharactersWithinRadius(c.PhysicsBody.Transform.position, ConfigSkills.Omnislash.Radius, c);
                executionStats.EnemiesAround = list.Count;
                if (list.Count == 0)
                {
                    executionStats.PreExecuteFailed = true;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ExecuteDash>c__IteratorD8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal ActiveDungeon <ad>__0;
            internal double <baseDamage>__5;
            internal float <baseTotalDamageMultiplier>__1;
            internal float <baseTotalDamageMultiplierIncreasePerExtraTarget>__2;
            internal double <bonusDamage>__6;
            internal float <critRuneModifier>__9;
            internal bool <critted>__19;
            internal double <damage>__20;
            internal Vector3 <dashDir>__12;
            internal double <dmgPerSlash>__11;
            internal List<CharacterInstance> <enemiesAroundUs>__16;
            internal HashSet<CharacterInstance> <enemiesHit>__13;
            internal CharacterInstance <enemy>__18;
            internal bool <hasCritRune>__8;
            internal int <i>__17;
            internal int <numExtraTargets>__3;
            internal Vector3 <pushDirectionXz>__21;
            internal int <slashCount>__4;
            internal double <totalDamage>__7;
            internal float <upgradedDashDamagePct>__10;
            internal Vector3 <vel>__14;
            internal float <velSqrMag>__15;
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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.c.PhysicsBody.gameObject.layer = Layers.IGNORE_CHARACTERS;
                        this.<totalDamage>__7 = OmnislashSkill.CalculateTotalDamage(this.c, out this.<baseTotalDamageMultiplier>__1, out this.<baseTotalDamageMultiplierIncreasePerExtraTarget>__2, out this.<numExtraTargets>__3, out this.<slashCount>__4, out this.<baseDamage>__5, out this.<bonusDamage>__6);
                        this.<hasCritRune>__8 = this.c.getPerkInstanceCount(PerkType.SkillUpgradeOmnislash3) > 0;
                        this.<critRuneModifier>__9 = this.c.getGenericModifierForPerkType(PerkType.SkillUpgradeOmnislash3);
                        this.<upgradedDashDamagePct>__10 = !this.c.IsBoss ? ConfigSkills.Omnislash.UpgradedDashDamagePct : ConfigSkills.BossOmnislash.UpgradedDashDamagePct;
                        this.<dmgPerSlash>__11 = MathUtil.ClampMin(this.<totalDamage>__7 / ((double) this.<slashCount>__4), 1.0) * this.<upgradedDashDamagePct>__10;
                        this.<dashDir>__12 = this.executionStats.MovementDir;
                        this.<enemiesHit>__13 = new HashSet<CharacterInstance>();
                        this.<vel>__14 = (Vector3) (this.<dashDir>__12 * this.executionStats.MovementForce);
                        this.<velSqrMag>__15 = this.<vel>__14.sqrMagnitude;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_03FC;
                }
                if (this.<velSqrMag>__15 > 25f)
                {
                    this.<velSqrMag>__15 = this.<vel>__14.sqrMagnitude;
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, this.<vel>__14, this.<vel>__14.normalized);
                    PhysicsUtil.ApplyDrag(ref this.<vel>__14, ConfigSkills.Omnislash.UpgradedDashDragPerSecond, Time.fixedDeltaTime);
                    CmdHitDestructibles.ExecuteStatic(this.c, this.c.PhysicsBody.Transform.position, ConfigSkills.Omnislash.EffectiveHitRadius, SkillType.Omnislash);
                    this.<enemiesAroundUs>__16 = this.<ad>__0.ActiveRoom.getEnemyCharactersWithinRadius(this.c.PhysicsBody.Transform.position, ConfigSkills.Omnislash.EffectiveHitRadius, this.c);
                    this.executionStats.EnemiesAround = this.<enemiesAroundUs>__16.Count;
                    this.<i>__17 = 0;
                    while (this.<i>__17 < this.<enemiesAroundUs>__16.Count)
                    {
                        this.<enemy>__18 = this.<enemiesAroundUs>__16[this.<i>__17];
                        if (!this.<enemiesHit>__13.Contains(this.<enemy>__18))
                        {
                            this.<critted>__19 = this.<hasCritRune>__8 && (this.<critRuneModifier>__9 >= UnityEngine.Random.value);
                            this.<damage>__20 = this.<dmgPerSlash>__11;
                            this.executionStats.SpecialCaseThisFrame = this.<critted>__19;
                            if (this.<critted>__19)
                            {
                                this.<damage>__20 *= this.c.CriticalHitMultiplier(true);
                            }
                            OmnislashSkill.ApplyRunestoneBuffs(this.c, this.<enemy>__18);
                            CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<enemy>__18, this.<damage>__20, this.<critted>__19, DamageType.Magic, SkillType.Omnislash);
                            if (this.<enemy>__18.IsDead)
                            {
                                this.executionStats.KillCount++;
                            }
                            else
                            {
                                this.<pushDirectionXz>__21 = Vector3Extensions.ToXzVector3(this.<enemy>__18.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                                GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<enemy>__18, new CmdPushCharacter(this.<enemy>__18, (Vector3) ((this.<pushDirectionXz>__21 * ConfigSkills.Omnislash.UpgradedDashPushForcePerVelocity) * this.<vel>__14.magnitude)), 0f);
                            }
                            this.<enemiesHit>__13.Add(this.<enemy>__18);
                            GameLogic.Binder.EventBus.CharacterSkillExecutionMidpoint(this.c, SkillType.Omnislash, this.executionStats);
                        }
                        this.<i>__17++;
                    }
                    this.$current = new WaitForFixedUpdate();
                    this.$PC = 1;
                    return true;
                }
                goto Label_03FC;
                this.$PC = -1;
            Label_03FC:
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

        [CompilerGenerated]
        private sealed class <ExecuteDefault>c__IteratorD7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal ActiveDungeon <ad>__1;
            internal double <baseDamage>__6;
            internal float <baseTotalDamageMultiplier>__2;
            internal float <baseTotalDamageMultiplierIncreasePerExtraTarget>__3;
            internal double <bonusDamage>__7;
            internal float <critRuneModifier>__10;
            internal bool <critted>__22;
            internal double <damage>__23;
            internal double <dmgPerSlash>__11;
            internal bool <hasCritRune>__9;
            internal int <i>__16;
            internal int <i>__17;
            internal int <j>__24;
            internal Vector3 <jumpPosXz>__21;
            internal int <numExtraTargets>__4;
            internal Dictionary<CharacterInstance, int> <numTimesEnemyDealtDamage>__15;
            internal Vector3 <randDirXz>__19;
            internal Vector3 <randPtXz>__18;
            internal int <slashCount>__5;
            internal List<CharacterInstance> <sortedEnemiesAliveInRadius>__12;
            internal CharacterInstance <target>__13;
            internal int <targetCharacterIdx>__14;
            internal Vector3 <targetPosXz>__20;
            internal double <totalDamage>__8;
            internal IEnumerator <waitIe>__0;
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
                        this.<waitIe>__0 = TimeUtil.WaitForFixedSeconds(0.01f);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0570;

                    default:
                        goto Label_05AB;
                }
                if (this.<waitIe>__0.MoveNext())
                {
                    this.$current = this.<waitIe>__0.Current;
                    this.$PC = 1;
                    goto Label_05AD;
                }
                this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                this.<totalDamage>__8 = OmnislashSkill.CalculateTotalDamage(this.c, out this.<baseTotalDamageMultiplier>__2, out this.<baseTotalDamageMultiplierIncreasePerExtraTarget>__3, out this.<numExtraTargets>__4, out this.<slashCount>__5, out this.<baseDamage>__6, out this.<bonusDamage>__7);
                this.<hasCritRune>__9 = this.c.getPerkInstanceCount(PerkType.SkillUpgradeOmnislash3) > 0;
                this.<critRuneModifier>__10 = this.c.getGenericModifierForPerkType(PerkType.SkillUpgradeOmnislash3);
                this.<dmgPerSlash>__11 = MathUtil.ClampMin(this.<totalDamage>__8 / ((double) this.<slashCount>__5), 1.0);
                this.<sortedEnemiesAliveInRadius>__12 = this.<ad>__1.ActiveRoom.getEnemyCharactersWithinRadiusSortedByDistance(this.c.PhysicsBody.Transform.position, ConfigSkills.Omnislash.Radius, this.c);
                this.executionStats.EnemiesAround = this.<sortedEnemiesAliveInRadius>__12.Count;
                this.<target>__13 = null;
                this.<targetCharacterIdx>__14 = 0;
                this.<numTimesEnemyDealtDamage>__15 = new Dictionary<CharacterInstance, int>();
                this.<i>__16 = 0;
                while (this.<i>__16 < this.<sortedEnemiesAliveInRadius>__12.Count)
                {
                    this.<numTimesEnemyDealtDamage>__15.Add(this.<sortedEnemiesAliveInRadius>__12[this.<i>__16], 0);
                    this.<i>__16++;
                }
                this.<i>__17 = 0;
                while (this.<i>__17 < this.<slashCount>__5)
                {
                    Dictionary<CharacterInstance, int> dictionary;
                    CharacterInstance instance;
                    if (this.<sortedEnemiesAliveInRadius>__12.Count == 0)
                    {
                        this.<target>__13 = null;
                    }
                    else if ((this.<targetCharacterIdx>__14 + 1) >= this.<sortedEnemiesAliveInRadius>__12.Count)
                    {
                        this.<targetCharacterIdx>__14 = 0;
                        this.<target>__13 = this.<sortedEnemiesAliveInRadius>__12[this.<targetCharacterIdx>__14];
                    }
                    else
                    {
                        this.<targetCharacterIdx>__14++;
                        this.<target>__13 = this.<sortedEnemiesAliveInRadius>__12[this.<targetCharacterIdx>__14];
                    }
                    CmdSetCharacterTarget.ExecuteStatic(this.c, this.<target>__13, Vector3.zero);
                    if ((this.<target>__13 != null) && this.<target>__13.IsDead)
                    {
                        goto Label_0580;
                    }
                    if (this.<target>__13 == null)
                    {
                        break;
                    }
                    this.<randPtXz>__18 = new Vector3(UnityEngine.Random.Range((float) -1f, (float) 1f), 0f, UnityEngine.Random.Range((float) -1f, (float) 1f));
                    this.<randDirXz>__19 = this.<randPtXz>__18.normalized;
                    this.<targetPosXz>__20 = Vector3Extensions.ToXzVector3(this.<target>__13.PhysicsBody.Transform.position);
                    this.<jumpPosXz>__21 = this.<targetPosXz>__20 + ((Vector3) (this.<randDirXz>__19 * this.c.AttackRange(true)));
                    int? mask = null;
                    this.<jumpPosXz>__21 = this.<ad>__1.ActiveRoom.calculateNearestEmptySpot(this.<jumpPosXz>__21, Vector3.zero, 1f, 1f, 6f, mask);
                    this.c.PhysicsBody.Transform.position = this.<jumpPosXz>__21;
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.<targetPosXz>__20 - this.<jumpPosXz>__21);
                    this.<critted>__22 = this.<hasCritRune>__9 && (this.<critRuneModifier>__10 >= UnityEngine.Random.value);
                    this.<damage>__23 = this.<dmgPerSlash>__11;
                    this.executionStats.SpecialCaseThisFrame = this.<critted>__22;
                    if (this.<critted>__22)
                    {
                        this.<damage>__23 *= this.c.CriticalHitMultiplier(true);
                    }
                    this.<j>__24 = 0;
                    while (this.<j>__24 < this.<numTimesEnemyDealtDamage>__15[this.<target>__13])
                    {
                        this.<damage>__23 *= ConfigSkills.Omnislash.SameTargetDiminishingDamageMultiplier;
                        this.<j>__24++;
                    }
                    if (this.<numTimesEnemyDealtDamage>__15[this.<target>__13] == 0)
                    {
                        OmnislashSkill.ApplyRunestoneBuffs(this.c, this.<target>__13);
                    }
                    CmdHitDestructibles.ExecuteStatic(this.c, this.c.PhysicsBody.Transform.position, ConfigSkills.Omnislash.EffectiveHitRadius, SkillType.Omnislash);
                    CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<target>__13, this.<damage>__23, this.<critted>__22, DamageType.Magic, SkillType.Omnislash);
                    int num2 = dictionary[instance];
                    (dictionary = this.<numTimesEnemyDealtDamage>__15)[instance = this.<target>__13] = num2 + 1;
                    this.executionStats.KillDuringThisFrame = false;
                    if (this.<target>__13.IsDead)
                    {
                        this.executionStats.KillCount++;
                        this.executionStats.LastKillWorldPos = this.<target>__13.PositionAtTimeOfDeath;
                        this.executionStats.KillDuringThisFrame = true;
                        this.<sortedEnemiesAliveInRadius>__12.Remove(this.<target>__13);
                        CmdSetCharacterTarget.ExecuteStatic(this.c, null, Vector3.zero);
                    }
                    GameLogic.Binder.EventBus.CharacterSkillExecutionMidpoint(this.c, SkillType.Omnislash, this.executionStats);
                    this.<waitIe>__0 = TimeUtil.WaitForFixedSeconds(ConfigSkills.Omnislash.DelayBetweenSlashes);
                Label_0570:
                    while (this.<waitIe>__0.MoveNext())
                    {
                        this.$current = this.<waitIe>__0.Current;
                        this.$PC = 2;
                        goto Label_05AD;
                    }
                Label_0580:
                    this.<i>__17++;
                }
                goto Label_05AB;
                this.$PC = -1;
            Label_05AB:
                return false;
            Label_05AD:
                return true;
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

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorD6 : IEnumerator, IDisposable, IEnumerator<object>
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
                        this.<ie>__0 = null;
                        if (!OmnislashSkill.DashModificationEnabled(this.c))
                        {
                            this.<ie>__0 = OmnislashSkill.ExecuteDefault(this.c, this.rank, this.executionStats);
                            break;
                        }
                        this.<ie>__0 = OmnislashSkill.ExecuteDash(this.c, this.rank, this.executionStats);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00B5;
                }
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_00B5;
                this.$PC = -1;
            Label_00B5:
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

