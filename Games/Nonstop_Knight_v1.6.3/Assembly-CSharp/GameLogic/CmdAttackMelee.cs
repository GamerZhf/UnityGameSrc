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

    public class CmdAttackMelee : ICommand
    {
        private CharacterInstance m_sourceCharacter;
        private CharacterInstance m_targetCharacter;

        public CmdAttackMelee(CharacterInstance sourceCharacter, CharacterInstance targetCharacter)
        {
            this.m_sourceCharacter = sourceCharacter;
            this.m_targetCharacter = targetCharacter;
        }

        private double calculateDamage(CharacterInstance c, bool canCrit, bool forceCrit, out bool critted)
        {
            double num = c.DamagePerHit(true);
            float num2 = UnityEngine.Random.Range(-ConfigGameplay.GLOBAL_MELEE_DAMAGE_VARIATION_PCT, ConfigGameplay.GLOBAL_MELEE_DAMAGE_VARIATION_PCT);
            double num3 = MathUtil.Clamp(num + (num * num2), 1.0, double.MaxValue);
            if (canCrit || forceCrit)
            {
                critted = (UnityEngine.Random.Range((float) 0f, (float) 1f) <= c.CriticalHitChancePct(true)) || forceCrit;
                if (critted)
                {
                    num3 *= c.CriticalHitMultiplier(true);
                }
                return num3;
            }
            critted = false;
            return num3;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator4A iteratora = new <executeRoutine>c__Iterator4A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator4A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdAttackMelee <>f__this;
            internal ActiveDungeon <ad>__0;
            internal double <amount>__11;
            internal int <coinDropCount>__32;
            internal float <coinGainPct>__30;
            internal double <coinReward>__31;
            internal float <cooldownTime>__5;
            internal bool <critKnockbackSlow>__20;
            internal List<KeyValuePair<PerkInstance, BuffSource>> <critStunPerks>__12;
            internal bool <critted>__10;
            internal bool <crittedToCleavableTarget>__41;
            internal DamageType <damageType>__18;
            internal bool <doResist>__27;
            internal List<CharacterInstance> <enemiesAroundUs>__38;
            internal CharacterInstance <enemy>__40;
            internal float <force>__43;
            internal bool <forceCrit>__8;
            internal SkillType <fromSkillType>__17;
            internal int <i>__13;
            internal int <i>__16;
            internal int <i>__25;
            internal int <i>__39;
            internal IEnumerator <ie>__6;
            internal bool <isHighestFloor>__34;
            internal int <j>__45;
            internal int <killCounter>__1;
            internal bool <knockback>__19;
            internal float <knockBackForce>__21;
            internal float <knockbackProcChance>__22;
            internal float <lifestealModifier>__26;
            internal float <modifierSum>__35;
            internal List<KeyValuePair<PerkInstance, BuffSource>> <perks>__15;
            internal List<KeyValuePair<PerkInstance, BuffSource>> <perks>__24;
            internal List<KeyValuePair<PerkInstance, BuffSource>> <perks>__44;
            internal Vector3 <primaryContactPtWorld>__3;
            internal float <procChance>__9;
            internal Vector3 <pushDirectionXz>__23;
            internal Vector3 <pushDirectionXz>__42;
            internal double <stolenAmount>__28;
            internal float <stunChance>__14;
            internal string <targetCharacterIdAtAttackStart>__2;
            internal Vector3 <targetWorldPos>__29;
            internal float <timeUntilContact>__4;
            internal double <totalDamageDealt>__7;
            internal double <xpAmount>__33;
            internal int <xpDropCount>__36;
            internal double <xpPerDrop>__37;

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
                        this.<killCounter>__1 = 0;
                        this.<>f__this.m_sourceCharacter.resetAttackTimer();
                        this.<>f__this.m_sourceCharacter.AttackSourceCounter++;
                        if (!this.<>f__this.m_targetCharacter.IsDead && this.<ad>__0.ActiveRoom.characterWithinAttackDistance(this.<>f__this.m_sourceCharacter, this.<>f__this.m_targetCharacter))
                        {
                            this.<targetCharacterIdAtAttackStart>__2 = this.<>f__this.m_targetCharacter.Id;
                            GameLogic.Binder.EventBus.CharacterMeleeAttackStarted(this.<>f__this.m_sourceCharacter, this.<>f__this.m_targetCharacter);
                            this.<primaryContactPtWorld>__3 = this.<>f__this.m_targetCharacter.PhysicsBody.transform.position;
                            this.<timeUntilContact>__4 = this.<>f__this.m_sourceCharacter.getAttackDuration() * this.<>f__this.m_sourceCharacter.Character.AttackContactTimeNormalized;
                            this.<cooldownTime>__5 = this.<>f__this.m_sourceCharacter.getAttackDuration() - this.<timeUntilContact>__4;
                            this.<ie>__6 = TimeUtil.WaitForFixedSeconds(this.<timeUntilContact>__4);
                            break;
                        }
                        goto Label_10BD;

                    case 1:
                        break;

                    case 2:
                        if (this.<>f__this.m_sourceCharacter.IsDead)
                        {
                            UnityEngine.Debug.LogError("Mid-CmdAttackMelee source character is dead (this coroutine should've been stopped already): " + this.<>f__this.m_sourceCharacter.Id);
                            goto Label_10BD;
                        }
                        if (((this.<>f__this.m_sourceCharacter.TargetCharacter == this.<>f__this.m_targetCharacter) && !this.<>f__this.m_targetCharacter.IsDead) && this.<ad>__0.ActiveRoom.characterWithinAttackDistance(this.<>f__this.m_sourceCharacter, this.<>f__this.m_targetCharacter))
                        {
                            this.<totalDamageDealt>__7 = 0.0;
                            this.<forceCrit>__8 = this.<>f__this.m_sourceCharacter.NextAttackIsGuaranteedCritical;
                            if ((this.<>f__this.m_sourceCharacter.IsPrimaryPlayerCharacter && (this.<>f__this.m_sourceCharacter.getPerkInstanceCount(PerkType.CritExplosion) > 0)) && ((this.<>f__this.m_sourceCharacter.AttackSourceCounter % 5) == 0))
                            {
                                this.<forceCrit>__8 = true;
                            }
                            if ((!this.<forceCrit>__8 && this.<>f__this.m_sourceCharacter.IsBoss) && ((this.<>f__this.m_sourceCharacter.getPerkInstanceCount(PerkType.BossCriticalHit) > 0) && ((this.<>f__this.m_sourceCharacter.AttackSourceCounter % Mathf.RoundToInt(this.<>f__this.m_sourceCharacter.getGenericModifierForPerkType(PerkType.BossCriticalHit))) == 0)))
                            {
                                this.<forceCrit>__8 = true;
                            }
                            if (!this.<>f__this.m_sourceCharacter.IsPlayerCharacter && this.<ad>__0.hasDungeonModifier(DungeonModifierType.MonsterCanCrit))
                            {
                                this.<procChance>__9 = !this.<>f__this.m_sourceCharacter.IsBoss ? ConfigDungeonModifiers.MonsterCanCrit.MinionCritChance : ConfigDungeonModifiers.MonsterCanCrit.BossCritChance;
                                if (UnityEngine.Random.Range((float) 0f, (float) 1f) <= this.<procChance>__9)
                                {
                                    this.<forceCrit>__8 = true;
                                }
                            }
                            this.<amount>__11 = this.<>f__this.calculateDamage(this.<>f__this.m_sourceCharacter, true, this.<forceCrit>__8, out this.<critted>__10);
                            GameLogic.Binder.EventBus.CharacterMeleeAttackContact(this.<>f__this.m_sourceCharacter, this.<primaryContactPtWorld>__3, this.<critted>__10);
                            if (this.<>f__this.m_sourceCharacter.IsPrimaryPlayerCharacter)
                            {
                                this.<critStunPerks>__12 = CharacterStatModifierUtil.GetPerkInstancesOfType(this.<>f__this.m_sourceCharacter, PerkType.CritStun);
                                if (!this.<critted>__10 || (this.<critStunPerks>__12.Count <= 0))
                                {
                                    this.<stunChance>__14 = this.<>f__this.m_sourceCharacter.getGenericModifierForPerkType(PerkType.AtkProcStun);
                                    if ((this.<stunChance>__14 > 0f) && (UnityEngine.Random.Range((float) 0f, (float) 1f) <= this.<stunChance>__14))
                                    {
                                        this.<perks>__15 = CharacterStatModifierUtil.GetPerkInstancesOfType(this.<>f__this.m_sourceCharacter, PerkType.AtkProcStun);
                                        this.<i>__16 = 0;
                                        while (this.<i>__16 < this.<perks>__15.Count)
                                        {
                                            KeyValuePair<PerkInstance, BuffSource> pair3 = this.<perks>__15[this.<i>__16];
                                            KeyValuePair<PerkInstance, BuffSource> pair4 = this.<perks>__15[this.<i>__16];
                                            GameLogic.Binder.BuffSystem.startBuffFromPerk(this.<>f__this.m_targetCharacter, PerkType.AtkProcStun, ConfigPerks.SHARED_DATA[PerkType.AtkProcStun].DurationSeconds, (double) pair3.Key.Modifier, pair4.Value, null);
                                            this.<i>__16++;
                                        }
                                    }
                                }
                                else
                                {
                                    this.<i>__13 = 0;
                                    while (this.<i>__13 < this.<critStunPerks>__12.Count)
                                    {
                                        KeyValuePair<PerkInstance, BuffSource> pair = this.<critStunPerks>__12[this.<i>__13];
                                        KeyValuePair<PerkInstance, BuffSource> pair2 = this.<critStunPerks>__12[this.<i>__13];
                                        GameLogic.Binder.BuffSystem.startBuffFromPerk(this.<>f__this.m_targetCharacter, PerkType.CritStun, ConfigPerks.SHARED_DATA[PerkType.CritStun].DurationSeconds, (double) pair.Key.Modifier, pair2.Value, null);
                                        this.<i>__13++;
                                    }
                                }
                            }
                            if ((this.<>f__this.m_sourceCharacter.IsBoss && (this.<>f__this.m_sourceCharacter.getPerkInstanceCount(PerkType.BossStun) > 0)) && ((this.<>f__this.m_sourceCharacter.AttackSourceCounter % Mathf.RoundToInt(this.<>f__this.m_sourceCharacter.getGenericModifierForPerkType(PerkType.BossStun))) == 0))
                            {
                                Buff buff = new Buff();
                                buff.Stuns = true;
                                buff.DurationSeconds = ConfigPerks.SHARED_DATA[PerkType.BossStun].DurationSeconds;
                                buff.SourceCharacter = this.<>f__this.m_sourceCharacter;
                                GameLogic.Binder.BuffSystem.startBuff(this.<>f__this.m_targetCharacter, buff);
                            }
                            this.<fromSkillType>__17 = SkillType.NONE;
                            if (this.<>f__this.m_sourceCharacter.Prefab == CharacterPrefab.KnightClone)
                            {
                                this.<damageType>__18 = DamageType.Magic;
                                this.<fromSkillType>__17 = SkillType.Clone;
                            }
                            else
                            {
                                this.<damageType>__18 = DamageType.Melee;
                            }
                            CmdDealDamageToCharacter.ExecuteStatic(this.<>f__this.m_sourceCharacter, this.<>f__this.m_targetCharacter, this.<amount>__11, this.<critted>__10, this.<damageType>__18, this.<fromSkillType>__17);
                            this.<totalDamageDealt>__7 += this.<amount>__11;
                            if (this.<>f__this.m_targetCharacter.IsDead)
                            {
                                this.<killCounter>__1++;
                            }
                            this.<knockback>__19 = false;
                            this.<critKnockbackSlow>__20 = false;
                            if (this.<>f__this.m_sourceCharacter.IsPrimaryPlayerCharacter)
                            {
                                this.<knockBackForce>__21 = 0f;
                                if (this.<critted>__10 && (this.<>f__this.m_sourceCharacter.getPerkInstanceCount(PerkType.CritKnockbackSlow) > 0))
                                {
                                    this.<knockBackForce>__21 = UnityEngine.Random.Range(ConfigPerks.CritKnockbackSlow.KnockbackForceMin, ConfigPerks.CritKnockbackSlow.KnockbackForceMax);
                                    this.<knockback>__19 = true;
                                    this.<critKnockbackSlow>__20 = true;
                                }
                                else
                                {
                                    this.<knockbackProcChance>__22 = this.<>f__this.m_sourceCharacter.getGenericModifierForPerkType(PerkType.AtkProcKnockback);
                                    if ((this.<knockbackProcChance>__22 > 0f) && (UnityEngine.Random.Range((float) 0f, (float) 1f) <= this.<knockbackProcChance>__22))
                                    {
                                        this.<knockBackForce>__21 = UnityEngine.Random.Range(ConfigPerks.AtkProcKnockback.KnockbackForceMin, ConfigPerks.AtkProcKnockback.KnockbackForceMax);
                                        this.<knockback>__19 = true;
                                    }
                                }
                                if (!this.<>f__this.m_targetCharacter.IsDead)
                                {
                                    if (this.<knockback>__19)
                                    {
                                        this.<pushDirectionXz>__23 = Vector3Extensions.ToXzVector3(this.<>f__this.m_targetCharacter.PhysicsBody.Transform.position - this.<>f__this.m_sourceCharacter.PhysicsBody.Transform.position).normalized;
                                        GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<>f__this.m_targetCharacter, new CmdPushCharacter(this.<>f__this.m_targetCharacter, (Vector3) (this.<pushDirectionXz>__23 * this.<knockBackForce>__21)), 0f);
                                    }
                                    if (this.<critKnockbackSlow>__20)
                                    {
                                        this.<perks>__24 = CharacterStatModifierUtil.GetPerkInstancesOfType(this.<>f__this.m_sourceCharacter, PerkType.CritKnockbackSlow);
                                        this.<i>__25 = 0;
                                        while (this.<i>__25 < this.<perks>__24.Count)
                                        {
                                            KeyValuePair<PerkInstance, BuffSource> pair5 = this.<perks>__24[this.<i>__25];
                                            KeyValuePair<PerkInstance, BuffSource> pair6 = this.<perks>__24[this.<i>__25];
                                            GameLogic.Binder.BuffSystem.startOrRefreshBuffFromPerk(this.<>f__this.m_targetCharacter, PerkType.CritKnockbackSlow, ConfigPerks.SHARED_DATA[PerkType.CritKnockbackSlow].DurationSeconds, (double) pair5.Key.Modifier, pair6.Value, null);
                                            this.<i>__25++;
                                        }
                                    }
                                }
                            }
                            this.<lifestealModifier>__26 = 0f;
                            this.<doResist>__27 = false;
                            if (this.<>f__this.m_sourceCharacter.IsPrimaryPlayerCharacter)
                            {
                                this.<lifestealModifier>__26 = this.<>f__this.m_sourceCharacter.getGenericModifierForPerkType(PerkType.Lifesteal);
                                this.<doResist>__27 = this.<>f__this.m_targetCharacter.IsBoss && (this.<>f__this.m_targetCharacter.getPerkInstanceCount(PerkType.BossResistLifesteal) > 0);
                            }
                            else if (this.<>f__this.m_sourceCharacter.IsBoss)
                            {
                                this.<lifestealModifier>__26 = this.<>f__this.m_sourceCharacter.getGenericModifierForPerkType(PerkType.BossLifesteal);
                            }
                            if ((this.<lifestealModifier>__26 > 0f) && !this.<doResist>__27)
                            {
                                this.<stolenAmount>__28 = MathUtil.Clamp(Math.Round((double) (this.<totalDamageDealt>__7 * this.<lifestealModifier>__26)), 0.0, double.MaxValue);
                                if (this.<stolenAmount>__28 > 0.0)
                                {
                                    CmdGainHp.ExecuteStatic(this.<>f__this.m_sourceCharacter, this.<stolenAmount>__28, false);
                                }
                            }
                            if (this.<>f__this.m_sourceCharacter.IsPrimaryPlayerCharacter)
                            {
                                this.<targetWorldPos>__29 = !this.<>f__this.m_targetCharacter.IsDead ? this.<>f__this.m_targetCharacter.PhysicsBody.Transform.position : this.<>f__this.m_targetCharacter.PositionAtTimeOfDeath;
                                if ((this.<>f__this.m_sourceCharacter.getPerkInstanceCount(PerkType.CoinExplosion) > 0) && ((this.<>f__this.m_sourceCharacter.AttackSourceCounter % 4) == 0))
                                {
                                    this.<coinGainPct>__30 = this.<>f__this.m_sourceCharacter.getGenericModifierForPerkType(PerkType.CoinExplosion);
                                    this.<coinReward>__31 = Math.Max((double) 1.0, (double) ((App.Binder.ConfigMeta.MinionCoinDropCurve(this.<ad>__0.Floor) * this.<coinGainPct>__30) * App.Binder.ConfigMeta.COIN_GAIN_CONTROLLER));
                                    this.<coinDropCount>__32 = ConfigPerks.CoinExplosion.DropCountMinMax.getRandom();
                                    GameLogic.Binder.LootSystem.triggerResourceExplosion(ResourceType.Coin, this.<targetWorldPos>__29, this.<coinReward>__31, this.<coinDropCount>__32, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                                }
                                if ((this.<>f__this.m_sourceCharacter.getPerkInstanceCount(PerkType.ExpExplosion) > 0) && ((this.<>f__this.m_sourceCharacter.AttackSourceCounter % 3) == 0))
                                {
                                    this.<xpAmount>__33 = this.<>f__this.m_sourceCharacter.getGenericModifierForPerkType(PerkType.ExpExplosion);
                                    if (this.<>f__this.m_targetCharacter.IsBoss)
                                    {
                                        this.<isHighestFloor>__34 = this.<ad>__0.Floor == this.<>f__this.m_sourceCharacter.OwningPlayer.getHighestFloorReached();
                                        this.<xpAmount>__33 *= App.Binder.ConfigMeta.XpFromBossKill(this.<ad>__0.Floor, this.<isHighestFloor>__34);
                                    }
                                    else
                                    {
                                        this.<xpAmount>__33 *= App.Binder.ConfigMeta.XpFromMinionKill(this.<ad>__0.Floor);
                                    }
                                    this.<modifierSum>__35 = this.<>f__this.m_sourceCharacter.getCharacterTypeXpModifier(this.<>f__this.m_targetCharacter.Type) + this.<>f__this.m_sourceCharacter.UniversalXpBonus(true);
                                    this.<xpAmount>__33 += this.<xpAmount>__33 * this.<modifierSum>__35;
                                    this.<xpDropCount>__36 = ConfigPerks.ExpExplosion.DropCountMinMax.getRandom();
                                    this.<xpPerDrop>__37 = this.<xpAmount>__33 / ((double) this.<xpDropCount>__36);
                                    GameLogic.Binder.LootSystem.triggerResourceExplosion(ResourceType.Xp, this.<targetWorldPos>__29, this.<xpPerDrop>__37, this.<xpDropCount>__36, string.Empty);
                                }
                            }
                            if (this.<>f__this.m_sourceCharacter.CleaveDamagePct(true) > 0f)
                            {
                                this.<enemiesAroundUs>__38 = this.<ad>__0.ActiveRoom.getEnemyCharactersWithinRadius(this.<>f__this.m_sourceCharacter.PhysicsBody.Transform.position, this.<>f__this.m_sourceCharacter.CleaveRange(true), this.<>f__this.m_sourceCharacter);
                                this.<i>__39 = 0;
                                while (this.<i>__39 < this.<enemiesAroundUs>__38.Count)
                                {
                                    this.<enemy>__40 = this.<enemiesAroundUs>__38[this.<i>__39];
                                    if (this.<enemy>__40 != this.<>f__this.m_targetCharacter)
                                    {
                                        this.<amount>__11 = this.<>f__this.calculateDamage(this.<>f__this.m_sourceCharacter, false, false, out this.<crittedToCleavableTarget>__41);
                                        if (this.<critted>__10)
                                        {
                                            this.<amount>__11 *= this.<>f__this.m_sourceCharacter.CriticalHitMultiplier(true);
                                        }
                                        this.<amount>__11 = MathUtil.Clamp(this.<amount>__11 * this.<>f__this.m_sourceCharacter.CleaveDamagePct(true), 1.0, double.MaxValue);
                                        CmdDealDamageToCharacter.ExecuteStatic(this.<>f__this.m_sourceCharacter, this.<enemy>__40, this.<amount>__11, this.<crittedToCleavableTarget>__41, DamageType.Melee, SkillType.NONE);
                                        this.<totalDamageDealt>__7 += this.<amount>__11;
                                        if (this.<enemy>__40.IsDead)
                                        {
                                            this.<killCounter>__1++;
                                        }
                                        if (!this.<enemy>__40.IsDead)
                                        {
                                            if (this.<knockback>__19)
                                            {
                                                this.<pushDirectionXz>__42 = Vector3Extensions.ToXzVector3(this.<enemy>__40.PhysicsBody.Transform.position - this.<>f__this.m_sourceCharacter.PhysicsBody.Transform.position).normalized;
                                                this.<force>__43 = UnityEngine.Random.Range(ConfigPerks.AtkProcKnockback.KnockbackForceMin, ConfigPerks.AtkProcKnockback.KnockbackForceMax);
                                                GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<enemy>__40, new CmdPushCharacter(this.<enemy>__40, (Vector3) (this.<pushDirectionXz>__42 * this.<force>__43)), 0f);
                                            }
                                            if (this.<critKnockbackSlow>__20)
                                            {
                                                this.<perks>__44 = CharacterStatModifierUtil.GetPerkInstancesOfType(this.<>f__this.m_sourceCharacter, PerkType.CritKnockbackSlow);
                                                this.<j>__45 = 0;
                                                while (this.<j>__45 < this.<perks>__44.Count)
                                                {
                                                    KeyValuePair<PerkInstance, BuffSource> pair7 = this.<perks>__44[this.<j>__45];
                                                    KeyValuePair<PerkInstance, BuffSource> pair8 = this.<perks>__44[this.<j>__45];
                                                    GameLogic.Binder.BuffSystem.startOrRefreshBuffFromPerk(this.<enemy>__40, PerkType.CritKnockbackSlow, ConfigPerks.SHARED_DATA[PerkType.CritKnockbackSlow].DurationSeconds, (double) pair7.Key.Modifier, pair8.Value, null);
                                                    this.<j>__45++;
                                                }
                                            }
                                        }
                                    }
                                    this.<i>__39++;
                                }
                            }
                        }
                        this.<>f__this.m_sourceCharacter.NextAttackIsGuaranteedCritical = false;
                        this.<ie>__6 = TimeUtil.WaitForFixedSeconds(this.<cooldownTime>__5);
                        goto Label_104B;

                    case 3:
                        goto Label_104B;

                    default:
                        goto Label_10BD;
                }
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 1;
                    goto Label_10BF;
                }
                CmdHitDestructibles.ExecuteStatic(this.<>f__this.m_sourceCharacter, this.<>f__this.m_sourceCharacter.PhysicsBody.transform.position, ConfigGameplay.CHARACTER_MELEE_ATTACK_DESTRUCTIBLE_HIT_RADIUS, SkillType.NONE);
                this.$current = null;
                this.$PC = 2;
                goto Label_10BF;
            Label_104B:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 3;
                    goto Label_10BF;
                }
                GameLogic.Binder.CommandProcessor.stopCommand(this.<>f__this.m_sourceCharacter.PhysicsBody, ref this.<>f__this.m_sourceCharacter.AttackRoutine);
                GameLogic.Binder.EventBus.CharacterMeleeAttackEnded(this.<>f__this.m_sourceCharacter, this.<>f__this.m_targetCharacter, this.<primaryContactPtWorld>__3, this.<killCounter>__1);
                goto Label_10BD;
                this.$PC = -1;
            Label_10BD:
                return false;
            Label_10BF:
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
    }
}

