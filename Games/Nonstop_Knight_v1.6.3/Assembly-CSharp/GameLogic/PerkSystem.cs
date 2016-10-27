namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PerkSystem : MonoBehaviour, IPerkSystem
    {
        private Dictionary<SkillType, List<PerkType>> sm_postSkillUsePerks;

        public PerkSystem()
        {
            Dictionary<SkillType, List<PerkType>> dictionary = new Dictionary<SkillType, List<PerkType>>(new SkillTypeBoxAvoidanceComparer());
            List<PerkType> list = new List<PerkType>();
            list.Add(PerkType.PostSkillUseArmor);
            list.Add(PerkType.PostSkillUseAttackSpeed);
            list.Add(PerkType.PostSkillUseDamage);
            dictionary.Add(SkillType.NONE, list);
            list = new List<PerkType>();
            list.Add(PerkType.PostSkillUseWhirlwindDamage);
            dictionary.Add(SkillType.Whirlwind, list);
            list = new List<PerkType>();
            list.Add(PerkType.PostSkillUseLeapDamage);
            dictionary.Add(SkillType.Leap, list);
            list = new List<PerkType>();
            list.Add(PerkType.PostSkillUseCloneDamage);
            dictionary.Add(SkillType.Clone, list);
            list = new List<PerkType>();
            list.Add(PerkType.PostSkillUseSlamDamage);
            dictionary.Add(SkillType.Slam, list);
            list = new List<PerkType>();
            list.Add(PerkType.PostSkillUseOmnislashDamage);
            dictionary.Add(SkillType.Omnislash, list);
            list = new List<PerkType>();
            list.Add(PerkType.PostSkillUseImplosionDamage);
            dictionary.Add(SkillType.Implosion, list);
            this.sm_postSkillUsePerks = dictionary;
        }

        protected void Awake()
        {
            if (!ConfigApp.ProductionBuild)
            {
                for (int i = 0; i < ConfigPerks.ALL_PERKS.Count; i++)
                {
                    ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[ConfigPerks.ALL_PERKS[i]];
                }
            }
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((activeDungeon != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
                if (!primaryPlayerCharacter.IsDead)
                {
                    float num = Time.deltaTime * Time.timeScale;
                    if (primaryPlayerCharacter.getPerkInstanceCount(PerkType.PassiveDamage) > 0)
                    {
                        List<KeyValuePair<PerkInstance, BuffSource>> perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(primaryPlayerCharacter, PerkType.PassiveDamage);
                        for (int i = 0; i < perkInstancesOfType.Count; i++)
                        {
                            KeyValuePair<PerkInstance, BuffSource> pair = perkInstancesOfType[i];
                            PerkInstance key = pair.Key;
                            key.Timer -= num;
                            if (key.Timer <= 0f)
                            {
                                List<CharacterInstance> list2 = activeDungeon.ActiveRoom.getEnemyCharactersWithinRadius(primaryPlayerCharacter.PhysicsBody.Transform.position, ConfigPerks.MassDamageDeflection.Radius, primaryPlayerCharacter);
                                for (int j = 0; j < list2.Count; j++)
                                {
                                    CharacterInstance targetCharacter = list2[j];
                                    double baseAmount = targetCharacter.MaxLife(false) * primaryPlayerCharacter.getGenericModifierForPerkType(PerkType.PassiveDamage);
                                    CmdDealDamageToCharacter.ExecuteStatic(primaryPlayerCharacter, targetCharacter, baseAmount, false, DamageType.Magic, SkillType.NONE);
                                }
                                key.Timer = ConfigPerks.PassiveDamage.Interval;
                            }
                        }
                    }
                }
            }
        }

        private void onCharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            if (targetCharacter.IsPrimaryPlayerCharacter)
            {
                this.onPrimaryHeroDealtDamage(sourceCharacter, targetCharacter, worldPos, amount, critted, damageReduced, damageType, fromSkill);
            }
            else if ((sourceCharacter != null) && sourceCharacter.IsPlayerCharacter)
            {
                this.onPlayerCharacterDealedDamage(sourceCharacter, targetCharacter, worldPos, amount, critted, damageReduced, damageType, fromSkill);
            }
        }

        private void onCharacterKilled(CharacterInstance killedCharacter, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            if (killer != null)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
                if (killedCharacter.IsSupport && (primaryPlayerCharacter.getPerkInstanceCount(PerkType.SummonCoinExplosion) > 0))
                {
                    float num = primaryPlayerCharacter.OwningPlayer.ActiveCharacter.getGenericModifierForPerkType(PerkType.SummonCoinExplosion);
                    double amountPerDrop = Math.Max((double) 1.0, (double) ((App.Binder.ConfigMeta.MinionCoinDropCurve(activeDungeon.Floor) * num) * App.Binder.ConfigMeta.COIN_GAIN_CONTROLLER));
                    int dropCount = ConfigPerks.CoinExplosion.DropCountMinMax.getRandom();
                    Vector3 centerWorldPos = !killedCharacter.IsDead ? killedCharacter.PhysicsBody.Transform.position : killedCharacter.PositionAtTimeOfDeath;
                    GameLogic.Binder.LootSystem.triggerResourceExplosion(ResourceType.Coin, centerWorldPos, amountPerDrop, dropCount, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                }
                List<KeyValuePair<PerkInstance, BuffSource>> perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(primaryPlayerCharacter, PerkType.SummonCritterOnKill);
                for (int i = 0; i < perkInstancesOfType.Count; i++)
                {
                    KeyValuePair<PerkInstance, BuffSource> pair = perkInstancesOfType[i];
                    float modifier = pair.Key.Modifier;
                    if (UnityEngine.Random.Range((float) 0f, (float) 1f) <= modifier)
                    {
                        GameLogic.Binder.CharacterSpawningSystem.summonSupportCritters(primaryPlayerCharacter, 1, new Vector3?(killedCharacter.PositionAtTimeOfDeath));
                    }
                }
                if (killer.IsPrimaryPlayerCharacter)
                {
                    perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(primaryPlayerCharacter, PerkType.KillArmor);
                    for (int j = 0; j < perkInstancesOfType.Count; j++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair2 = perkInstancesOfType[j];
                        KeyValuePair<PerkInstance, BuffSource> pair3 = perkInstancesOfType[j];
                        GameLogic.Binder.BuffSystem.startBuffFromPerk(primaryPlayerCharacter, PerkType.KillArmor, ConfigPerks.SHARED_DATA[PerkType.KillArmor].DurationSeconds, (double) pair2.Key.Modifier, pair3.Value, null);
                    }
                    perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(primaryPlayerCharacter, PerkType.KillAttackSpeed);
                    for (int k = 0; k < perkInstancesOfType.Count; k++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair4 = perkInstancesOfType[k];
                        KeyValuePair<PerkInstance, BuffSource> pair5 = perkInstancesOfType[k];
                        GameLogic.Binder.BuffSystem.startBuffFromPerk(primaryPlayerCharacter, PerkType.KillAttackSpeed, ConfigPerks.SHARED_DATA[PerkType.KillAttackSpeed].DurationSeconds, (double) pair4.Key.Modifier, pair5.Value, null);
                    }
                    perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(primaryPlayerCharacter, PerkType.KillDamage);
                    for (int m = 0; m < perkInstancesOfType.Count; m++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair6 = perkInstancesOfType[m];
                        KeyValuePair<PerkInstance, BuffSource> pair7 = perkInstancesOfType[m];
                        GameLogic.Binder.BuffSystem.startBuffFromPerk(primaryPlayerCharacter, PerkType.KillDamage, ConfigPerks.SHARED_DATA[PerkType.KillDamage].DurationSeconds, (double) pair6.Key.Modifier, pair7.Value, null);
                    }
                    perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(primaryPlayerCharacter, PerkType.KillCritChance);
                    for (int n = 0; n < perkInstancesOfType.Count; n++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair8 = perkInstancesOfType[n];
                        KeyValuePair<PerkInstance, BuffSource> pair9 = perkInstancesOfType[n];
                        GameLogic.Binder.BuffSystem.startBuffFromPerk(primaryPlayerCharacter, PerkType.KillCritChance, ConfigPerks.SHARED_DATA[PerkType.KillCritChance].DurationSeconds, (double) pair8.Key.Modifier, pair9.Value, null);
                    }
                }
            }
        }

        private void onCharacterMeleeAttackContact(CharacterInstance sourceCharacter, Vector3 contactWorldPt, bool critted)
        {
            if (sourceCharacter.IsPrimaryPlayerCharacter)
            {
                float num = sourceCharacter.getGenericModifierForPerkType(PerkType.SummonCritter);
                if ((num > 0f) && (UnityEngine.Random.Range((float) 0f, (float) 1f) <= num))
                {
                    GameLogic.Binder.CharacterSpawningSystem.summonSupportCritters(sourceCharacter, 1, null);
                }
            }
        }

        private void onCharacterRevived(CharacterInstance character)
        {
            if (character.IsPrimaryPlayerCharacter)
            {
                List<KeyValuePair<PerkInstance, BuffSource>> perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(character, PerkType.ReviveArmor);
                for (int i = 0; i < perkInstancesOfType.Count; i++)
                {
                    KeyValuePair<PerkInstance, BuffSource> pair = perkInstancesOfType[i];
                    KeyValuePair<PerkInstance, BuffSource> pair2 = perkInstancesOfType[i];
                    GameLogic.Binder.BuffSystem.startBuffFromPerk(character, PerkType.ReviveArmor, ConfigPerks.SHARED_DATA[PerkType.ReviveArmor].DurationSeconds, (double) pair.Key.Modifier, pair2.Value, null);
                }
                perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(character, PerkType.ReviveDamage);
                for (int j = 0; j < perkInstancesOfType.Count; j++)
                {
                    KeyValuePair<PerkInstance, BuffSource> pair3 = perkInstancesOfType[j];
                    KeyValuePair<PerkInstance, BuffSource> pair4 = perkInstancesOfType[j];
                    GameLogic.Binder.BuffSystem.startBuffFromPerk(character, PerkType.ReviveDamage, ConfigPerks.SHARED_DATA[PerkType.ReviveDamage].DurationSeconds, (double) pair3.Key.Modifier, pair4.Value, null);
                }
            }
        }

        private void onCharacterSkillActivated(CharacterInstance character, SkillType skillType, float buildupTime, SkillExecutionStats executionStats)
        {
            if (character.IsPrimaryPlayerCharacter)
            {
                for (int i = 0; i < this.sm_postSkillUsePerks[SkillType.NONE].Count; i++)
                {
                    PerkType perkType = this.sm_postSkillUsePerks[0][i];
                    List<KeyValuePair<PerkInstance, BuffSource>> perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(character, perkType);
                    for (int k = 0; k < perkInstancesOfType.Count; k++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair = perkInstancesOfType[k];
                        PerkInstance key = pair.Key;
                        KeyValuePair<PerkInstance, BuffSource> pair2 = perkInstancesOfType[k];
                        BuffSource source = pair2.Value;
                        GameLogic.Binder.BuffSystem.startBuffFromPerk(character, perkType, ConfigPerks.SHARED_DATA[perkType].DurationSeconds, (double) key.Modifier, source, null);
                    }
                }
                for (int j = 0; j < ConfigSkills.ACTIVE_HERO_SKILLS.Count; j++)
                {
                    if (skillType == ((SkillType) ConfigSkills.ACTIVE_HERO_SKILLS[j]))
                    {
                        for (int m = 0; m < this.sm_postSkillUsePerks[skillType].Count; m++)
                        {
                            PerkType type2 = this.sm_postSkillUsePerks[skillType][m];
                            List<KeyValuePair<PerkInstance, BuffSource>> list2 = CharacterStatModifierUtil.GetPerkInstancesOfType(character, type2);
                            for (int n = 0; n < list2.Count; n++)
                            {
                                KeyValuePair<PerkInstance, BuffSource> pair3 = list2[n];
                                PerkInstance instance2 = pair3.Key;
                                KeyValuePair<PerkInstance, BuffSource> pair4 = list2[n];
                                BuffSource source2 = pair4.Value;
                                GameLogic.Binder.BuffSystem.startBuffFromPerk(character, type2, ConfigPerks.SHARED_DATA[type2].DurationSeconds, (double) instance2.Modifier, source2, null);
                            }
                        }
                    }
                }
                if ((skillType == SkillType.Leap) && (character.getPerkInstanceCount(PerkType.SkillUpgradeLeap3) > 0))
                {
                    character.NextAttackIsGuaranteedCritical = true;
                }
                if (GameLogic.Binder.GameState.ActiveDungeon.hasDungeonModifier(DungeonModifierType.HeroPostSkillIncreasedSkillDamage))
                {
                    Buff buff2 = new Buff();
                    buff2.BaseStat1 = BaseStatProperty.SkillDamage;
                    buff2.Modifier = ConfigDungeonModifiers.HeroPostSkillIncreasedSkillDamage.Modifier;
                    buff2.DurationSeconds = ConfigDungeonModifiers.HeroPostSkillIncreasedSkillDamage.BuffDurationSeconds;
                    BuffSource source3 = new BuffSource();
                    source3.Object = ConfigDungeonModifiers.HeroPostSkillIncreasedSkillDamage.BuffSource;
                    buff2.Source = source3;
                    buff2.HudSprite = "sprite_knight_player_256";
                    buff2.HudShowStacked = true;
                    buff2.HudHideTimer = true;
                    Buff buff = buff2;
                    GameLogic.Binder.BuffSystem.startBuff(character, buff);
                }
            }
        }

        private void onCharacterSkillExecutionMidpoint(CharacterInstance c, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (c.IsPrimaryPlayerCharacter)
            {
                Player owningPlayer = c.OwningPlayer;
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                if ((skillType == SkillType.Leap) && (c.getPerkInstanceCount(PerkType.SkillUpgradeLeap1) > 0))
                {
                    double baseCoinReward = App.Binder.ConfigMeta.MinionCoinDropCurve(activeDungeon.Floor) * ConfigSkills.Leap.UpgradedBonusCoinCountPerUsage;
                    double amountPerDrop = Math.Max(Math.Round((double) (owningPlayer.calculateStandardCoinRoll(baseCoinReward, GameLogic.CharacterType.UNSPECIFIED, 1) / 5.0)), 1.0);
                    GameLogic.Binder.LootSystem.triggerResourceExplosion(ResourceType.Coin, c.PhysicsBody.Transform.position, amountPerDrop, 5, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnCharacterSkillActivated -= new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            GameLogic.Binder.EventBus.OnCharacterSkillExecutionMidpoint -= new GameLogic.Events.CharacterSkillExecutionMidpoint(this.onCharacterSkillExecutionMidpoint);
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackContact -= new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
            GameLogic.Binder.EventBus.OnCharacterDealtDamage -= new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            GameLogic.Binder.EventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnCharacterRevived -= new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            GameLogic.Binder.EventBus.OnPetLevelUpped -= new GameLogic.Events.PetLevelUpped(this.onPetLevelUpped);
            GameLogic.Binder.EventBus.OnPetSelected -= new GameLogic.Events.PetSelected(this.onPetSelected);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnCharacterSkillActivated += new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            GameLogic.Binder.EventBus.OnCharacterSkillExecutionMidpoint += new GameLogic.Events.CharacterSkillExecutionMidpoint(this.onCharacterSkillExecutionMidpoint);
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackContact += new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
            GameLogic.Binder.EventBus.OnCharacterDealtDamage += new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            GameLogic.Binder.EventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnCharacterRevived += new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            GameLogic.Binder.EventBus.OnPetLevelUpped += new GameLogic.Events.PetLevelUpped(this.onPetLevelUpped);
            GameLogic.Binder.EventBus.OnPetSelected += new GameLogic.Events.PetSelected(this.onPetSelected);
        }

        private void onPetLevelUpped(Player player, string petId, bool cheated)
        {
            player.Runestones.refreshRunestoneSelections();
        }

        private void onPetSelected(Player player, PetInstance pet)
        {
            player.Runestones.refreshRunestoneSelections();
        }

        private void onPlayerCharacterDealedDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((damageType == DamageType.Melee) && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterWeaponDeflect))
            {
                double baseAmount = MathUtil.Clamp(Math.Round((double) (amount * ConfigDungeonModifiers.MonsterWeaponDeflect.DeflectionPct)), 0.0, double.MaxValue);
                if (baseAmount > 0.0)
                {
                    CmdDealDamageToCharacter.ExecuteStatic(targetCharacter, sourceCharacter, baseAmount, false, damageType, SkillType.NONE);
                }
            }
            if ((damageType == DamageType.Magic) && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterSkillDeflect))
            {
                double num2 = MathUtil.Clamp(Math.Round((double) (amount * ConfigDungeonModifiers.MonsterSkillDeflect.DeflectionPct)), 0.0, double.MaxValue);
                if (num2 > 0.0)
                {
                    CmdDealDamageToCharacter.ExecuteStatic(targetCharacter, sourceCharacter, num2, false, damageType, SkillType.NONE);
                }
            }
        }

        private void onPrimaryHeroDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            Buff buff3;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            List<KeyValuePair<PerkInstance, BuffSource>> perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(targetCharacter, PerkType.WoundedArmor);
            for (int i = 0; i < perkInstancesOfType.Count; i++)
            {
                KeyValuePair<PerkInstance, BuffSource> pair = perkInstancesOfType[i];
                KeyValuePair<PerkInstance, BuffSource> pair2 = perkInstancesOfType[i];
                GameLogic.Binder.BuffSystem.startBuffFromPerk(targetCharacter, PerkType.WoundedArmor, ConfigPerks.SHARED_DATA[PerkType.WoundedArmor].DurationSeconds, (double) pair.Key.Modifier, pair2.Value, null);
            }
            perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(targetCharacter, PerkType.WoundedAttackSpeed);
            for (int j = 0; j < perkInstancesOfType.Count; j++)
            {
                KeyValuePair<PerkInstance, BuffSource> pair3 = perkInstancesOfType[j];
                KeyValuePair<PerkInstance, BuffSource> pair4 = perkInstancesOfType[j];
                GameLogic.Binder.BuffSystem.startBuffFromPerk(targetCharacter, PerkType.WoundedAttackSpeed, ConfigPerks.SHARED_DATA[PerkType.WoundedAttackSpeed].DurationSeconds, (double) pair3.Key.Modifier, pair4.Value, null);
            }
            perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(targetCharacter, PerkType.WoundedDamage);
            for (int k = 0; k < perkInstancesOfType.Count; k++)
            {
                KeyValuePair<PerkInstance, BuffSource> pair5 = perkInstancesOfType[k];
                KeyValuePair<PerkInstance, BuffSource> pair6 = perkInstancesOfType[k];
                GameLogic.Binder.BuffSystem.startBuffFromPerk(targetCharacter, PerkType.WoundedDamage, ConfigPerks.SHARED_DATA[PerkType.WoundedDamage].DurationSeconds, (double) pair5.Key.Modifier, pair6.Value, null);
            }
            if ((sourceCharacter != null) && (damageType == DamageType.Ranged))
            {
                float num4 = targetCharacter.getGenericModifierForPerkType(PerkType.RangedDamageDeflection);
                if (num4 > 0f)
                {
                    double baseAmount = MathUtil.Clamp(Math.Round((double) (amount * num4)), 0.0, double.MaxValue);
                    if (baseAmount > 0.0)
                    {
                        CmdDealDamageToCharacter.ExecuteStatic(targetCharacter, sourceCharacter, baseAmount, false, damageType, SkillType.NONE);
                    }
                }
            }
            if (sourceCharacter != null)
            {
                float num6 = targetCharacter.getGenericModifierForPerkType(PerkType.DamageDeflection);
                if (num6 > 0f)
                {
                    double num7 = MathUtil.Clamp(Math.Round((double) (amount * num6)), 0.0, double.MaxValue);
                    if (num7 > 0.0)
                    {
                        CmdDealDamageToCharacter.ExecuteStatic(targetCharacter, sourceCharacter, num7, false, damageType, SkillType.NONE);
                    }
                }
            }
            float num8 = targetCharacter.getGenericModifierForPerkType(PerkType.MassDamageDeflection);
            if (num8 > 0f)
            {
                double num9 = MathUtil.Clamp(Math.Round((double) (amount * num8)), 0.0, double.MaxValue);
                if (num9 > 0.0)
                {
                    List<CharacterInstance> list2 = activeDungeon.ActiveRoom.getEnemyCharactersWithinRadius(targetCharacter.PhysicsBody.Transform.position, ConfigPerks.MassDamageDeflection.Radius, targetCharacter);
                    for (int m = 0; m < list2.Count; m++)
                    {
                        CmdDealDamageToCharacter.ExecuteStatic(targetCharacter, list2[m], num9, false, damageType, SkillType.NONE);
                    }
                }
            }
            if ((sourceCharacter != null) && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterStun))
            {
                float num11 = !sourceCharacter.IsBoss ? ConfigDungeonModifiers.MonsterStun.MinionProcChance : ConfigDungeonModifiers.MonsterStun.BossProcChance;
                if (UnityEngine.Random.Range((float) 0f, (float) 1f) <= num11)
                {
                    float num12 = !sourceCharacter.IsBoss ? ConfigDungeonModifiers.MonsterStun.MinionBuffDurationSeconds : ConfigDungeonModifiers.MonsterStun.BossBuffDurationSeconds;
                    buff3 = new Buff();
                    buff3.Stuns = true;
                    buff3.DurationSeconds = num12;
                    Buff buff = buff3;
                    GameLogic.Binder.BuffSystem.startBuff(targetCharacter, buff);
                }
            }
            if ((sourceCharacter != null) && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterFreeze))
            {
                float num13 = !sourceCharacter.IsBoss ? ConfigDungeonModifiers.MonsterFreeze.MinionProcChance : ConfigDungeonModifiers.MonsterFreeze.BossProcChance;
                if (UnityEngine.Random.Range((float) 0f, (float) 1f) <= num13)
                {
                    float num14 = !sourceCharacter.IsBoss ? ConfigDungeonModifiers.MonsterFreeze.MinionBuffDurationSeconds : ConfigDungeonModifiers.MonsterFreeze.BossBuffDurationSeconds;
                    buff3 = new Buff();
                    BuffSource source = new BuffSource();
                    source.Object = ConfigDungeonModifiers.MODIFIERS[DungeonModifierType.MonsterFreeze];
                    buff3.Source = source;
                    buff3.BaseStat1 = BaseStatProperty.AttacksPerSecond;
                    buff3.BaseStat2 = BaseStatProperty.MovementSpeed;
                    buff3.Modifier = ConfigPerks.GlobalFrostEffect.SpeedModifier;
                    buff3.DurationSeconds = num14;
                    Buff buff2 = buff3;
                    GameLogic.Binder.BuffSystem.startBuff(targetCharacter, buff2);
                }
            }
        }
    }
}

