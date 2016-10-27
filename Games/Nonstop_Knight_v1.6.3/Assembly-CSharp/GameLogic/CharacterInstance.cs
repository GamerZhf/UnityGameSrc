namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using PlayerView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CharacterInstance : IJsonData, IPoolable, ICharacterStatModifier
    {
        [CompilerGenerated]
        private string <Id>k__BackingField;
        public List<SkillType> ActiveSkillTypes = new List<SkillType>();
        [JsonIgnore]
        public float AttackCooldownTimer;
        [JsonIgnore]
        public Coroutine AttackRoutine;
        [JsonIgnore]
        public int AttackSourceCounter;
        [JsonIgnore]
        public int AttackTargetCounter;
        [JsonIgnore]
        public Coroutine BlinkRoutine;
        [JsonIgnore]
        public PerkContainer BossPerks = new PerkContainer();
        [JsonIgnore]
        public GameLogic.Character Character;
        public string CharacterId = string.Empty;
        [JsonIgnore]
        public bool Charmed;
        [JsonIgnore]
        public bool Confused;
        [JsonIgnore]
        public double CurrentHp;
        public static List<string> DEFENSE_STAT_HEADERS;
        [JsonIgnore]
        public bool ExternallyControlled;
        [JsonIgnore]
        public Vector3 Facing;
        [JsonIgnore]
        public float FutureTimeOfDeath;
        public GameLogic.HeroStats HeroStats = new GameLogic.HeroStats();
        [JsonIgnore]
        public HighestLevelItemInfo HighestLevelItemOwnedAtFloorStart;
        public const string ID_CLEANED_UP = "CLEANED_UP";
        public GameLogic.Inventory Inventory = new GameLogic.Inventory();
        [JsonIgnore]
        public bool IsBoss;
        [JsonIgnore]
        public bool IsBossClone;
        [JsonIgnore]
        public bool IsDead;
        [JsonIgnore]
        public bool IsEliteBoss;
        [JsonIgnore]
        public bool IsPersistent;
        [JsonIgnore]
        public bool IsPet;
        [JsonIgnore]
        public bool IsPlayerCharacter;
        [JsonIgnore]
        public bool IsSupport;
        [JsonIgnore]
        public bool IsWildBoss;
        public List<ItemSlot> ItemSlots = new List<ItemSlot>();
        [JsonIgnore]
        public Vector3 ManualTargetPos = Vector3.zero;
        [JsonIgnore]
        public bool NextAttackIsGuaranteedCritical;
        public static List<string> OFFENSE_STAT_HEADERS;
        [JsonIgnore]
        public Player OwningPlayer;
        [JsonIgnore]
        public GameLogic.PhysicsBody PhysicsBody;
        [JsonIgnore]
        public Vector3 PositionAtTimeOfDeath;
        public int Rank;
        [JsonIgnore]
        public bool RemainInPlaceOnGuard;
        [JsonIgnore]
        public ManualTimer RunAccelerationTimer = new ManualTimer(ConfigGameplay.CHARACTER_FULLSPEED_ACCELERATION_TIMER);
        public List<SkillInstance> SkillInstances = new List<SkillInstance>();
        [JsonIgnore]
        public Dictionary<SkillType, Coroutine> SkillRoutines = new Dictionary<SkillType, Coroutine>(new SkillTypeBoxAvoidanceComparer());
        [JsonIgnore]
        public object Source;
        [JsonIgnore]
        public bool SpinningAround;
        [JsonIgnore]
        public bool Stunned;
        [JsonIgnore]
        public int StunnedCount;
        [JsonIgnore]
        public PerkContainer SupportPerks = new PerkContainer();
        [JsonIgnore]
        public CharacterInstance TargetCharacter;
        public static List<string> UTILITY_STAT_HEADERS;
        [JsonIgnore]
        public Vector3 Velocity;

        static CharacterInstance()
        {
            List<string> list = new List<string>();
            list.Add(ConfigLoca.ATTRIBUTES_WEAPON_DAMAGE);
            list.Add(ConfigLoca.ATTRIBUTES_WEAPON_DAMAGE_COREBONUS);
            list.Add(ConfigLoca.ATTRIBUTES_WEAPON_DAMAGE_BONUS);
            list.Add(ConfigLoca.ATTRIBUTES_ATTACKS_PER_SECONDS);
            list.Add(ConfigLoca.ATTRIBUTES_SPLASH_DAMAGE);
            list.Add(ConfigLoca.ATTRIBUTES_CRITICAL_HIT_CHANCE);
            list.Add(ConfigLoca.ATTRIBUTES_CRITICAL_HIT_DAMAGE);
            OFFENSE_STAT_HEADERS = list;
            list = new List<string>();
            list.Add(ConfigLoca.ATTRIBUTES_ARMOR);
            list.Add(ConfigLoca.ATTRIBUTES_ARMOR_CORE_BONUS);
            list.Add(ConfigLoca.ATTRIBUTES_ARMOR_BONUS);
            list.Add(ConfigLoca.ATTRIBUTES_ARMOR_BLOCK);
            DEFENSE_STAT_HEADERS = list;
            list = new List<string>();
            list.Add(ConfigLoca.ATTRIBUTES_SKILL_DAMAGE);
            list.Add(ConfigLoca.ATTRIBUTES_SKILL_DAMAGE_COREBONUS);
            list.Add(ConfigLoca.ATTRIBUTES_FRENZY_DURATION);
            list.Add(ConfigLoca.ATTRIBUTES_FRENZY_DURATION_BONUS);
            list.Add(ConfigLoca.ATTRIBUTES_FRENZY_DURATION_BONUS_PER_KILL);
            list.Add(ConfigLoca.ATTRIBUTES_TOKEN_BONUS);
            list.Add(ConfigLoca.ATTRIBUTES_COIN_BONUS_FROM_AUGS);
            list.Add(ConfigLoca.ATTRIBUTES_AFK_COIN_BONUS_FROM_AUGS);
            UTILITY_STAT_HEADERS = list;
        }

        public void assignToDefaultLayer()
        {
            if (this.IsSupport)
            {
                this.PhysicsBody.gameObject.layer = Layers.SUPPORT_CHARACTERS;
            }
            else if (this.IsPlayerCharacter)
            {
                this.PhysicsBody.gameObject.layer = Layers.IGNORE_CHARACTERS;
            }
            else
            {
                this.PhysicsBody.gameObject.layer = Layers.CHARACTERS;
            }
        }

        public float AttackRange([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                return this.OwningPlayer.ActiveCharacter.AttackRange(false);
            }
            float baseAmount = this.Character.getBaseStatFloat(BaseStatProperty.AttackRange);
            return CharacterStatModifierUtil.ApplyBaseStatBonuses_Float(this, BaseStatProperty.AttackRange, baseAmount, includeBuffs);
        }

        public float AttacksPerSecond([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            float num;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                num = this.OwningPlayer.ActiveCharacter.AttacksPerSecond(false) * ConfigSkills.Clone.AttackSpeedMultiplier;
            }
            else
            {
                num = this.Character.getBaseStatFloat(BaseStatProperty.AttacksPerSecond);
            }
            if ((!this.IsPlayerCharacter && (activeDungeon != null)) && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterIncreasedAttackSpeed))
            {
                num += ConfigDungeonModifiers.MonsterIncreasedAttackSpeed.RawAttacksPerSecondIncrease;
            }
            return Mathf.Clamp(CharacterStatModifierUtil.ApplyBaseStatBonuses_Float(this, BaseStatProperty.AttacksPerSecond, num, includeBuffs), 0f, ConfigGameplay.ATTACKS_PER_SECOND_CAP);
        }

        public bool canBlink(Vector3 targetWorldPt)
        {
            if (this.ExternallyControlled)
            {
                return false;
            }
            if (this.isAttacking())
            {
                return false;
            }
            if (!this.isAtBlinkDistance(targetWorldPt))
            {
                return false;
            }
            return true;
        }

        public bool canEquipItem(ItemInstance ii)
        {
            return ((ii.Unlocked && !this.isItemInstanceEquipped(ii)) && this.isItemInstanceInInventory(ii));
        }

        public void cleanUpForReuse()
        {
            CmdInterruptCharacter.ExecuteStatic(this, true);
            this.Id = "CLEANED_UP";
            this.resetDynamicRuntimeData();
            this.Character = null;
            this.OwningPlayer = null;
            this.IsPersistent = false;
            this.IsSupport = false;
            this.IsPlayerCharacter = false;
            this.IsPet = false;
            this.CurrentHp = 0.0;
            this.RemainInPlaceOnGuard = false;
            this.TargetCharacter = null;
            this.Velocity = Vector3.zero;
            this.Facing = Vector3.zero;
            this.ManualTargetPos = Vector3.zero;
            this.ExternallyControlled = false;
            this.RunAccelerationTimer.reset();
            this.AttackRoutine = null;
            this.AttackCooldownTimer = 0f;
            this.SkillRoutines.Clear();
            this.IsDead = false;
            this.PositionAtTimeOfDeath = Vector3.zero;
            this.IsBoss = false;
            this.IsEliteBoss = false;
            this.IsWildBoss = false;
            this.IsBossClone = false;
            this.FutureTimeOfDeath = 0f;
            this.Stunned = false;
            this.Charmed = false;
            this.Confused = false;
            this.AttackSourceCounter = 0;
            this.AttackTargetCounter = 0;
            this.SpinningAround = false;
            this.BlinkRoutine = null;
            this.NextAttackIsGuaranteedCritical = false;
            this.Source = null;
            this.BossPerks.PerkInstances.Clear();
            this.SupportPerks.PerkInstances.Clear();
        }

        public float CleaveDamagePct([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                return this.OwningPlayer.ActiveCharacter.CleaveDamagePct(false);
            }
            float baseAmount = this.Character.getBaseStatFloat(BaseStatProperty.CleaveDamagePct);
            if ((this.getPerkInstanceCount(PerkType.FullSplashDamage) > 0) || (this.IsPrimaryPlayerCharacter && activeDungeon.hasDungeonModifier(DungeonModifierType.HeroIncreasedCleaveDamagePct)))
            {
                baseAmount = ConfigPerks.FullSplashDamage.BaseCleaveDamagePct;
            }
            return CharacterStatModifierUtil.ApplyBaseStatBonuses_Float(this, BaseStatProperty.CleaveDamagePct, baseAmount, includeBuffs);
        }

        public float CleaveRange([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                return this.OwningPlayer.ActiveCharacter.CleaveRange(false);
            }
            float baseAmount = this.Character.getBaseStatFloat(BaseStatProperty.CleaveRange);
            if (this.getPerkInstanceCount(PerkType.FullSplashDamage) > 0)
            {
                baseAmount = ConfigPerks.FullSplashDamage.BaseCleaveRange;
            }
            return CharacterStatModifierUtil.ApplyBaseStatBonuses_Float(this, BaseStatProperty.CleaveRange, baseAmount, includeBuffs);
        }

        public void completeAttackTimer()
        {
            this.AttackCooldownTimer = 0f;
        }

        public void copyFrom(CharacterInstance another)
        {
            this.Id = another.Id;
            this.CharacterId = another.CharacterId;
            this.Rank = another.Rank;
            this.HeroStats = new GameLogic.HeroStats(another.HeroStats);
            this.SkillInstances = new List<SkillInstance>(another.SkillInstances);
            this.ActiveSkillTypes = new List<SkillType>(another.ActiveSkillTypes);
            this.Inventory = new GameLogic.Inventory(another.Inventory);
            this.ItemSlots = new List<ItemSlot>(another.ItemSlots);
            this.IsBoss = another.IsBoss;
            this.IsEliteBoss = another.IsEliteBoss;
            this.IsWildBoss = another.IsWildBoss;
            this.IsBossClone = another.IsBossClone;
            this.HighestLevelItemOwnedAtFloorStart = another.HighestLevelItemOwnedAtFloorStart;
            this.postDeserializeInitialization();
        }

        public float CriticalHitChancePct([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                return 0f;
            }
            float baseAmount = this.Character.getBaseStatFloat(BaseStatProperty.CriticalHitChancePct);
            return Mathf.Clamp(CharacterStatModifierUtil.ApplyBaseStatBonuses_Float(this, BaseStatProperty.CriticalHitChancePct, baseAmount, includeBuffs), 0f, ConfigGameplay.GLOBAL_CAP_CRITICAL_HIT_CHANCE_PCT);
        }

        public float CriticalHitMultiplier([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                return this.OwningPlayer.ActiveCharacter.CriticalHitMultiplier(false);
            }
            float baseAmount = this.Character.getBaseStatFloat(BaseStatProperty.CriticalHitMultiplier);
            if ((activeDungeon != null) && activeDungeon.hasDungeonModifier(DungeonModifierType.GlobalCriticalHitDamageBonus))
            {
                if (this.IsPlayerCharacter)
                {
                    baseAmount += ConfigDungeonModifiers.GlobalCriticalHitDamageBonus.PlayerCharacterMultiplier;
                }
                else
                {
                    baseAmount += ConfigDungeonModifiers.GlobalCriticalHitDamageBonus.MonsterMultiplier;
                }
            }
            return CharacterStatModifierUtil.ApplyBaseStatBonuses_Float(this, BaseStatProperty.CriticalHitMultiplier, baseAmount, includeBuffs);
        }

        public double DamagePerHit([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (this.IsSupport)
            {
                CharacterInstance activeCharacter = this.OwningPlayer.ActiveCharacter;
                if (this.Prefab == CharacterPrefab.KnightClone)
                {
                    double num = activeCharacter.SkillDamage(false) * ConfigSkills.Clone.DphMultiplier;
                    return CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(this, SkillType.Clone, num);
                }
                if (this.IsPet)
                {
                    return (activeCharacter.DamagePerHit(includeBuffs) * this.Character.MainHeroDamagePerHitPct);
                }
                return (activeCharacter.SkillDamage(includeBuffs) * this.Character.MainHeroDamagePerHitPct);
            }
            double baseAmount = this.Character.getBaseStatDouble(BaseStatProperty.DamagePerHit);
            if (this.IsPlayerCharacter)
            {
                List<ItemSlot> list = this.getItemSlots(false);
                for (int i = 0; i < list.Count; i++)
                {
                    ItemInstance itemInstance = list[i].ItemInstance;
                    if ((itemInstance != null) && (itemInstance.Item.Type == ItemType.Weapon))
                    {
                        baseAmount += App.Binder.ConfigMeta.ItemPowerCurve(itemInstance.Item.Type, this.OwningPlayer.getRiggedItemLevel(itemInstance), itemInstance.Rank);
                    }
                }
            }
            else if (this.IsBoss)
            {
                baseAmount += App.Binder.ConfigMeta.BossDamagePerHitCurve(this.Rank, activeDungeon.getProgressDifficultyExponent(), this.IsWildBoss);
                if ((this.Character.BossAiBehaviour == AiBehaviourType.BossCaster) && (this.Character.BossAiParameters[0] == "BossSplitter"))
                {
                    for (int j = 0; j < (activeDungeon.ActiveRoom.numberOfBossesAlive() - 1); j++)
                    {
                        baseAmount *= ConfigSkills.BossSplitter.DamageMultiplierPerAliveClone;
                    }
                }
                if (activeDungeon.isTutorialDungeon())
                {
                    baseAmount *= ConfigTutorials.TUTORIAL_BOSS_DPH_MULTIPLIER;
                }
            }
            else
            {
                baseAmount += App.Binder.ConfigMeta.MinionDamagePerHitCurve(this.Rank, activeDungeon.getProgressDifficultyExponent());
                if (activeDungeon.isTutorialDungeon())
                {
                    baseAmount *= ConfigTutorials.TUTORIAL_MINION_DPH_MULTIPLIER;
                }
            }
            return CharacterStatModifierUtil.ApplyBaseStatBonuses_Double(this, BaseStatProperty.DamagePerHit, baseAmount, includeBuffs);
        }

        public double DamagePerSecond([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                return this.OwningPlayer.ActiveCharacter.DamagePerSecond(false);
            }
            return (this.DamagePerHit(includeBuffs) * this.AttacksPerSecond(includeBuffs));
        }

        public List<string> defenseAttributesToRichTextFormattedStringList(string title)
        {
            List<string> list2 = new List<string>();
            list2.Add(MenuHelpers.BigValueToString(this.MaxLife(false)));
            list2.Add(MenuHelpers.BigModifierToString(this.getGenericModifierForPerkType(PerkType.CoreBonusLife), true));
            list2.Add(MenuHelpers.BigModifierToString(this.getGenericModifierForPerkType(PerkType.LifeBonus), true));
            list2.Add(MenuHelpers.BigModifierToString(this.getGenericModifierForPerkType(PerkType.ArmorBlockAll), false));
            return list2;
        }

        public SkillInstance getActiveSkillInstanceForGroup(int group)
        {
            for (int i = 0; i < this.ActiveSkillTypes.Count; i++)
            {
                SkillType skillType = this.ActiveSkillTypes[i];
                if ((skillType != SkillType.NONE) && (ConfigSkills.SHARED_DATA[skillType].Group == group))
                {
                    return this.getSkillInstance(skillType);
                }
            }
            return null;
        }

        public int getActiveSkillSlotIndex(SkillType skillType)
        {
            for (int i = 0; i < this.ActiveSkillTypes.Count; i++)
            {
                if (((SkillType) this.ActiveSkillTypes[i]) == skillType)
                {
                    return i;
                }
            }
            return -1;
        }

        public double getAdjustedItemUpgradeCost(ItemType itemType, int level, int rank)
        {
            double num = App.Binder.ConfigMeta.ItemUpgradeCostCurve(itemType, level, rank);
            float num2 = this.getGenericModifierForPerkType(PerkType.ItemUpgradeCostBonus);
            return Math.Max((double) (num + (num * num2)), (double) 1.0);
        }

        public float getAttackDuration()
        {
            return (1f / this.AttacksPerSecond(true));
        }

        public float getBaseStatModifier(BaseStatProperty baseStat)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.getBaseStatModifier(baseStat);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.getBaseStatModifier(baseStat);
            }
            float num = 0f;
            if (this.OwningPlayer != null)
            {
                num += this.OwningPlayer.getBaseStatModifier(baseStat);
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.getBaseStatModifier(baseStat);
                }
            }
            if (baseStat == BaseStatProperty.SkillDamage)
            {
                for (int j = 0; j < this.ActiveSkillTypes.Count; j++)
                {
                    SkillInstance instance2 = this.getSkillInstance(this.ActiveSkillTypes[j]);
                    if (instance2 != null)
                    {
                        num += App.Binder.ConfigMeta.SkillUpgradeDamageBonus(instance2.Rank);
                    }
                }
            }
            return num;
        }

        public float getCharacterTypeArmorModifier(GameLogic.CharacterType characterType)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.getCharacterTypeArmorModifier(characterType);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.getCharacterTypeArmorModifier(characterType);
            }
            if (characterType == GameLogic.CharacterType.UNSPECIFIED)
            {
                return 0f;
            }
            float num = 0f;
            if (this.OwningPlayer != null)
            {
                num += this.OwningPlayer.getCharacterTypeArmorModifier(characterType);
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.getCharacterTypeArmorModifier(characterType);
                }
            }
            return num;
        }

        public float getCharacterTypeCoinModifier(GameLogic.CharacterType characterType)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.getCharacterTypeCoinModifier(characterType);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.getCharacterTypeCoinModifier(characterType);
            }
            float num = 0f;
            if (this.OwningPlayer != null)
            {
                num += this.OwningPlayer.getCharacterTypeCoinModifier(characterType);
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.getCharacterTypeCoinModifier(characterType);
                }
            }
            return num;
        }

        public float getCharacterTypeDamageModifier(GameLogic.CharacterType characterType)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.getCharacterTypeDamageModifier(characterType);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.getCharacterTypeDamageModifier(characterType);
            }
            if (characterType == GameLogic.CharacterType.UNSPECIFIED)
            {
                return 0f;
            }
            float num = 0f;
            if (this.OwningPlayer != null)
            {
                num += this.OwningPlayer.getCharacterTypeDamageModifier(characterType);
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.getCharacterTypeDamageModifier(characterType);
                }
            }
            return num;
        }

        public float getCharacterTypeXpModifier(GameLogic.CharacterType characterType)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.getCharacterTypeXpModifier(characterType);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.getCharacterTypeXpModifier(characterType);
            }
            if (characterType == GameLogic.CharacterType.UNSPECIFIED)
            {
                return 0f;
            }
            float num = 0f;
            if (this.OwningPlayer != null)
            {
                num += this.OwningPlayer.getCharacterTypeXpModifier(characterType);
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.getCharacterTypeXpModifier(characterType);
                }
            }
            return num;
        }

        public ItemInstance getEquippedItemOfType(ItemType itemType)
        {
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CompatibleItemType == itemType)
                {
                    return list[i].ItemInstance;
                }
            }
            return null;
        }

        public int getEquippedItemsLevelAndRankSum()
        {
            int num = 0;
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.Level + itemInstance.Rank;
                }
            }
            return num;
        }

        public ItemInstance getFirstItemInstanceFromInventory(ItemType itemType)
        {
            List<ItemInstance> list = this.getItemInstances(false);
            for (int i = 0; i < list.Count; i++)
            {
                if ((list[i] != null) && (list[i].Item.Type == itemType))
                {
                    return list[i];
                }
            }
            return null;
        }

        public ItemInstance getFirstOwnedItemInstanceWithItemId(string itemId, [Optional, DefaultParameterValue(null)] ItemInstance excludeInstance)
        {
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (((itemInstance != null) && (itemInstance.ItemId == itemId)) && (itemInstance != excludeInstance))
                {
                    return itemInstance;
                }
            }
            List<ItemInstance> list2 = this.getItemInstances(false);
            for (int j = 0; j < list2.Count; j++)
            {
                ItemInstance instance2 = list2[j];
                if (((instance2 != null) && (instance2.ItemId == itemId)) && (instance2 != excludeInstance))
                {
                    return instance2;
                }
            }
            return null;
        }

        public float getGenericModifierForPerkType(PerkType perkType)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.getGenericModifierForPerkType(perkType);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.getGenericModifierForPerkType(perkType);
            }
            float num = 0f;
            if (this.OwningPlayer != null)
            {
                num += this.OwningPlayer.getGenericModifierForPerkType(perkType);
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.getGenericModifierForPerkType(perkType);
                }
            }
            return num;
        }

        public Vector3 getGroundLevelWorldPos()
        {
            return (this.PhysicsBody.Transform.position + ((Vector3) (Vector3.up * 0.25f)));
        }

        public Vector3 getHeadLevelWorldPos()
        {
            float num;
            if (this.IsBoss)
            {
                num = 3f;
            }
            else if (this.Prefab == CharacterPrefab.Critter)
            {
                num = 0.25f;
            }
            else
            {
                num = 1.5f;
            }
            return (this.PhysicsBody.Transform.position + ((Vector3) (Vector3.up * num)));
        }

        public int getHighestItemLevelPlusRankEquippedOrInInventory(ItemType itemType, out bool multipleHighestTied)
        {
            ItemInstance instance = null;
            List<ItemSlot> list = this.getItemSlots(false);
            List<ItemInstance> list2 = this.getItemInstances(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (((itemInstance != null) && (itemInstance.Item.Type == itemType)) && ((instance == null) || (itemInstance.LevelPlusRank > instance.LevelPlusRank)))
                {
                    instance = itemInstance;
                }
            }
            for (int j = 0; j < list2.Count; j++)
            {
                ItemInstance instance3 = list2[j];
                if (((instance3 != null) && (instance3.Item.Type == itemType)) && ((instance == null) || (instance3.LevelPlusRank > instance.LevelPlusRank)))
                {
                    instance = instance3;
                }
            }
            multipleHighestTied = false;
            if (instance != null)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    ItemInstance instance4 = list[k].ItemInstance;
                    if (((instance4 != null) && (instance4 != instance)) && ((instance4.Item.Type == itemType) && (instance4.LevelPlusRank == instance.LevelPlusRank)))
                    {
                        multipleHighestTied = true;
                        break;
                    }
                }
                for (int m = 0; m < list2.Count; m++)
                {
                    ItemInstance instance5 = list2[m];
                    if (((instance5 != null) && (instance5 != instance)) && ((instance5.Item.Type == itemType) && (instance5.LevelPlusRank == instance.LevelPlusRank)))
                    {
                        multipleHighestTied = true;
                        break;
                    }
                }
            }
            if (instance != null)
            {
                return instance.LevelPlusRank;
            }
            return 0;
        }

        public ItemInstance getHighestLevelItemOwned()
        {
            ItemInstance instance = null;
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if ((itemInstance != null) && ((instance == null) || (itemInstance.LevelPlusRank > instance.LevelPlusRank)))
                {
                    instance = itemInstance;
                }
            }
            List<ItemInstance> list2 = this.getItemInstances(false);
            for (int j = 0; j < list2.Count; j++)
            {
                ItemInstance instance3 = list2[j];
                if ((instance3 != null) && ((instance == null) || (instance3.LevelPlusRank > instance.LevelPlusRank)))
                {
                    instance = instance3;
                }
            }
            return instance;
        }

        public List<ItemInstance> getItemInstances([Optional, DefaultParameterValue(false)] bool ignoreTournaments)
        {
            if ((!ignoreTournaments && (this.OwningPlayer != null)) && this.OwningPlayer.Tournaments.hasTournamentSelected())
            {
                return this.OwningPlayer.Tournaments.SelectedTournament.Inventory.ItemInstances;
            }
            return this.Inventory.ItemInstances;
        }

        public int getItemInstantUpgradeCount(ItemInstance itemInstance, out int currentMaxLevelPlusRank)
        {
            bool flag;
            currentMaxLevelPlusRank = this.getHighestItemLevelPlusRankEquippedOrInInventory(itemInstance.Item.Type, out flag);
            return Mathf.Clamp(currentMaxLevelPlusRank - itemInstance.LevelPlusRank, 0, 0x7fffffff);
        }

        public List<ItemSlot> getItemSlots([Optional, DefaultParameterValue(false)] bool ignoreTournaments)
        {
            if ((!ignoreTournaments && (this.OwningPlayer != null)) && this.OwningPlayer.Tournaments.hasTournamentSelected())
            {
                return this.OwningPlayer.Tournaments.SelectedTournament.ItemSlots;
            }
            return this.ItemSlots;
        }

        public float getLimitedLifetimeForSummon(GameLogic.Character summonedCharacterPrototype)
        {
            float limitedLifetimeSeconds = summonedCharacterPrototype.LimitedLifetimeSeconds;
            if (limitedLifetimeSeconds <= 0f)
            {
                return limitedLifetimeSeconds;
            }
            return (limitedLifetimeSeconds + (limitedLifetimeSeconds * this.getGenericModifierForPerkType(PerkType.ExtendedSummon)));
        }

        public int getNumberOfItemInstancesOwnedWithItemId(string itemId)
        {
            int num = 0;
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if ((itemInstance != null) && (itemInstance.ItemId == itemId))
                {
                    num++;
                }
            }
            List<ItemInstance> list2 = this.getItemInstances(false);
            for (int j = 0; j < list2.Count; j++)
            {
                if ((list2[j] != null) && (list2[j].ItemId == itemId))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfItemsOwnedWithRarity(int rarity)
        {
            int num = 0;
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemSlot slot = list[i];
                if ((slot.ItemInstance != null) && (slot.ItemInstance.Item.Rarity == rarity))
                {
                    num++;
                }
            }
            List<ItemInstance> list2 = this.getItemInstances(false);
            for (int j = 0; j < list2.Count; j++)
            {
                if (list2[j].Item.Rarity == rarity)
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfUninspectedItems(bool includeUnequippableItems)
        {
            int num = 0;
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (((itemInstance != null) && !itemInstance.InspectedByPlayer) && (includeUnequippableItems || this.hasReachedUnlockFloorForItem(itemInstance)))
                {
                    num++;
                }
            }
            List<ItemInstance> list2 = this.getItemInstances(false);
            for (int j = 0; j < list2.Count; j++)
            {
                ItemInstance instance2 = list2[j];
                if (((instance2 != null) && !instance2.InspectedByPlayer) && (includeUnequippableItems || this.hasReachedUnlockFloorForItem(instance2)))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfUninspectedItems(ItemType itemType, bool includeUnequippableItems)
        {
            int num = 0;
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if ((((itemInstance != null) && !itemInstance.InspectedByPlayer) && (itemInstance.Item.Type == itemType)) && (includeUnequippableItems || this.hasReachedUnlockFloorForItem(itemInstance)))
                {
                    num++;
                }
            }
            List<ItemInstance> list2 = this.getItemInstances(false);
            for (int j = 0; j < list2.Count; j++)
            {
                ItemInstance instance2 = list2[j];
                if ((((instance2 != null) && !instance2.InspectedByPlayer) && (instance2.Item.Type == itemType)) && (includeUnequippableItems || this.hasReachedUnlockFloorForItem(instance2)))
                {
                    num++;
                }
            }
            return num;
        }

        public int getPerkInstanceCount(PerkType perkType)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.getPerkInstanceCount(perkType);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.getPerkInstanceCount(perkType);
            }
            int num = 0;
            if (this.OwningPlayer != null)
            {
                num += this.OwningPlayer.getPerkInstanceCount(perkType);
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.getPerkInstanceCount(perkType);
                }
            }
            return num;
        }

        public void getPerkInstancesOfType(PerkType perkType, IBuffIconProvider iconProvider, ref List<KeyValuePair<PerkInstance, BuffSource>> outPerkInstances)
        {
            if (this.IsBoss)
            {
                this.BossPerks.getPerkInstancesOfType(perkType, iconProvider, ref outPerkInstances);
            }
            else if (this.IsSupport)
            {
                this.SupportPerks.getPerkInstancesOfType(perkType, iconProvider, ref outPerkInstances);
            }
            else
            {
                if (this.OwningPlayer != null)
                {
                    this.OwningPlayer.getPerkInstancesOfType(perkType, iconProvider, ref outPerkInstances);
                }
                List<ItemSlot> list = this.getItemSlots(false);
                for (int i = 0; i < list.Count; i++)
                {
                    ItemInstance itemInstance = list[i].ItemInstance;
                    if (itemInstance != null)
                    {
                        itemInstance.getPerkInstancesOfType(perkType, iconProvider, ref outPerkInstances);
                    }
                }
            }
        }

        public int getShardCountForItem(Item item)
        {
            if (!this.Inventory.ItemShards.ContainsKey(item.Id))
            {
                return 0;
            }
            return this.Inventory.ItemShards[item.Id];
        }

        public float getSkillCooldown(SkillType skillType)
        {
            if (ConfigApp.CHEAT_NO_SKILL_COOLDOWNS)
            {
                return 0.01f;
            }
            float num = !this.IsBoss ? ConfigSkills.SHARED_DATA[skillType].Cooldown : ConfigSkills.SHARED_DATA[skillType].BossCooldown;
            float num2 = 0f;
            num2 += GameLogic.Binder.BuffSystem.getSkillCooldownModifierSumFromActiveBuffs(this, skillType);
            num2 += this.getSkillCooldownModifier(skillType);
            if (this.IsPrimaryPlayerCharacter && GameLogic.Binder.GameState.ActiveDungeon.hasDungeonModifier(DungeonModifierType.HeroDecreasedSkillCooldowns))
            {
                num2 += ConfigDungeonModifiers.HeroDecreasedSkillCooldowns.CooldownModifier;
            }
            if (this.IsPrimaryPlayerCharacter && GameLogic.Binder.GameState.ActiveDungeon.hasDungeonModifier(DungeonModifierType.HeroIncreasedSkillCooldowns))
            {
                num2 += ConfigDungeonModifiers.HeroIncreasedSkillCooldowns.CooldownModifier;
            }
            float a = num + (num * num2);
            if (this.IsPrimaryPlayerCharacter)
            {
                return Mathf.Max(a, ConfigGameplay.HERO_SKILL_COOLDOWN_MIN);
            }
            return a;
        }

        public float getSkillCooldownModifier(SkillType skillType)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.getSkillCooldownModifier(skillType);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.getSkillCooldownModifier(skillType);
            }
            float num = 0f;
            if (this.OwningPlayer != null)
            {
                num += this.OwningPlayer.getSkillCooldownModifier(skillType);
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.getSkillCooldownModifier(skillType);
                }
            }
            return num;
        }

        public float getSkillDamageModifier(SkillType skillType)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.getSkillDamageModifier(skillType);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.getSkillDamageModifier(skillType);
            }
            float num = 0f;
            if (this.OwningPlayer != null)
            {
                num += this.OwningPlayer.getSkillDamageModifier(skillType);
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.getSkillDamageModifier(skillType);
                }
            }
            return num;
        }

        public int getSkillExtraCharges(SkillType skillType)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.getSkillExtraCharges(skillType);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.getSkillExtraCharges(skillType);
            }
            int num = 0;
            if (this.OwningPlayer != null)
            {
                num += this.OwningPlayer.getSkillExtraCharges(skillType);
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (itemInstance != null)
                {
                    num += itemInstance.getSkillExtraCharges(skillType);
                }
            }
            return num;
        }

        public SkillInstance getSkillInstance(SkillType skillType)
        {
            for (int i = 0; i < this.SkillInstances.Count; i++)
            {
                if (this.SkillInstances[i].SkillType == skillType)
                {
                    return this.SkillInstances[i];
                }
            }
            return null;
        }

        public float getSpurtBuffStrength()
        {
            return (((float) GameLogic.Binder.BuffSystem.getNumberOfBuffsWithId(this, ConfigGameplay.SPURT_BUFF_ID)) / ((float) ConfigGameplay.SPURTING_MAX_NUM_BUFFS));
        }

        public double getTotalEquipmentTokenValue()
        {
            int num = LangUtil.TryGetIntValueFromDictionary<int>(this.HeroStats.ItemsGainedByRarity, ConfigMeta.ITEM_HIGHEST_RARITY);
            return (App.Binder.ConfigMeta.ItemTokenValue(this) * num);
        }

        public int getTotalNumberOfFixedPerksInEquippedItems()
        {
            int num = 0;
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemSlot slot = list[i];
                if (slot.ItemInstance != null)
                {
                    num += slot.ItemInstance.Item.FixedPerks.count();
                }
            }
            return num;
        }

        public int getTotalNumberOfRandomPerksInEquippedItems()
        {
            int num = 0;
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemSlot slot = list[i];
                if (slot.ItemInstance != null)
                {
                    num += ConfigPerks.NUM_RANDOM_PERKS_PER_ITEM_RARITY[slot.ItemInstance.Item.Rarity];
                }
            }
            return num;
        }

        public PerkType getVisualRunestonePerkType(SkillType skillType)
        {
            List<ConfigRunestones.SharedData> runestonesForSkillType = ConfigRunestones.GetRunestonesForSkillType(skillType);
            for (int i = runestonesForSkillType.Count - 1; i >= 0; i--)
            {
                PerkType perkType = runestonesForSkillType[i].PerkInstance.Type;
                if (this.getPerkInstanceCount(perkType) > 0)
                {
                    return perkType;
                }
            }
            return PerkType.NONE;
        }

        public bool hasReachedUnlockFloorForItem(ItemInstance itemInstance)
        {
            return true;
        }

        public bool hasSkillInvulnerability(SkillType skillType)
        {
            if (this.IsBoss)
            {
                return this.BossPerks.hasSkillInvulnerability(skillType);
            }
            if (this.IsSupport)
            {
                return this.SupportPerks.hasSkillInvulnerability(skillType);
            }
            if ((this.OwningPlayer != null) && this.OwningPlayer.hasSkillInvulnerability(skillType))
            {
                return true;
            }
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if ((itemInstance != null) && itemInstance.hasSkillInvulnerability(skillType))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isAtBlinkDistance(Vector3 targetWorldPt)
        {
            return (PhysicsUtil.DistBetween(this, targetWorldPt) >= ConfigGameplay.BLINK_DISTANCE_THRESHOLD);
        }

        public bool isAtMaxRank()
        {
            return (this.Rank >= App.Binder.ConfigMeta.GLOBAL_LEVEL_CAP);
        }

        public bool isAttacking()
        {
            return (this.AttackRoutine != null);
        }

        public bool isBlinking()
        {
            return (this.BlinkRoutine != null);
        }

        public bool isDecoyClone()
        {
            return ((this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone)) && (this.getPerkInstanceCount(PerkType.SkillUpgradeClone3) > 0));
        }

        public bool isExecutingSkill()
        {
            return (this.SkillRoutines.Count > 0);
        }

        public bool isExecutingSkill(SkillType skillType)
        {
            return this.SkillRoutines.ContainsKey(skillType);
        }

        public bool isExecutingSkillWithExternalControl()
        {
            if (this.isExecutingSkill())
            {
                foreach (KeyValuePair<SkillType, Coroutine> pair in this.SkillRoutines)
                {
                    if (ConfigSkills.SHARED_DATA[pair.Key].ExternalControl)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isFemale()
        {
            return (this.CharacterId == "Hero002");
        }

        public bool isFriendlyTowards(CharacterInstance another)
        {
            if (this.Confused)
            {
                return false;
            }
            if (this.Charmed)
            {
                return (this.IsPlayerCharacter != another.IsPlayerCharacter);
            }
            if (another.Charmed)
            {
                return (this.IsPlayerCharacter != another.IsPlayerCharacter);
            }
            return (this.IsPlayerCharacter == another.IsPlayerCharacter);
        }

        public bool isInvisible()
        {
            if (!this.isExecutingSkill())
            {
                return false;
            }
            return this.isExecutingSkill(SkillType.Vanish);
        }

        public bool isInvulnerable()
        {
            if (this.IsSupport && (this.Prefab != CharacterPrefab.KnightClone))
            {
                return true;
            }
            if (!this.isExecutingSkill())
            {
                return GameLogic.Binder.BuffSystem.hasBuffFromPerk(this, PerkType.MultikillShield);
            }
            foreach (KeyValuePair<SkillType, Coroutine> pair in this.SkillRoutines)
            {
                SkillType key = pair.Key;
                if ((this.IsPrimaryPlayerCharacter || this.IsBoss) && this.hasSkillInvulnerability(key))
                {
                    return true;
                }
                if (ConfigSkills.SHARED_DATA[key].Invulnerability)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isItemEquipped(Item item)
        {
            return this.isItemEquipped(item.Id);
        }

        public bool isItemEquipped(string itemId)
        {
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                if ((list[i].ItemInstance != null) && (list[i].ItemInstance.ItemId == itemId))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isItemInInventory(Item item)
        {
            return this.isItemInInventory(item.Id);
        }

        public bool isItemInInventory(string itemId)
        {
            List<ItemInstance> list = this.getItemInstances(false);
            for (int i = 0; i < list.Count; i++)
            {
                if ((list[i] != null) && (list[i].ItemId == itemId))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isItemInstanceEquipped(ItemInstance itemInstance)
        {
            List<ItemSlot> list = this.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ItemInstance == itemInstance)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isItemInstanceInInventory(ItemInstance itemInstance)
        {
            List<ItemInstance> list = this.getItemInstances(false);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == itemInstance)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isSkillActive(SkillType skillType)
        {
            for (int i = 0; i < this.ActiveSkillTypes.Count; i++)
            {
                if (((SkillType) this.ActiveSkillTypes[i]) == skillType)
                {
                    return true;
                }
            }
            return false;
        }

        public double MaxLife([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                return this.OwningPlayer.ActiveCharacter.MaxLife(false);
            }
            double baseAmount = this.Character.getBaseStatDouble(BaseStatProperty.Life);
            if (this.IsPlayerCharacter)
            {
                List<ItemSlot> list = this.getItemSlots(false);
                for (int i = 0; i < list.Count; i++)
                {
                    ItemInstance itemInstance = list[i].ItemInstance;
                    if ((itemInstance != null) && (itemInstance.Item.Type == ItemType.Armor))
                    {
                        baseAmount += App.Binder.ConfigMeta.ItemPowerCurve(itemInstance.Item.Type, this.OwningPlayer.getRiggedItemLevel(itemInstance), itemInstance.Rank);
                    }
                }
            }
            else if (this.IsBoss)
            {
                baseAmount += App.Binder.ConfigMeta.BossLifeCurve(this.Rank, activeDungeon.getProgressDifficultyExponent(), this.IsWildBoss);
                if (activeDungeon.isTutorialDungeon())
                {
                    baseAmount *= ConfigTutorials.TUTORIAL_BOSS_LIFE_MULTIPLIER;
                }
            }
            else
            {
                baseAmount += App.Binder.ConfigMeta.MinionLifeCurve(this.Rank, activeDungeon.getProgressDifficultyExponent());
                if (activeDungeon.isTutorialDungeon())
                {
                    baseAmount *= ConfigTutorials.TUTORIAL_MINION_LIFE_MULTIPLIER;
                }
            }
            if ((!this.IsPlayerCharacter && (activeDungeon != null)) && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterIncreasedLife))
            {
                baseAmount *= ConfigDungeonModifiers.MonsterIncreasedLife.RawLifeMultiplier;
            }
            return CharacterStatModifierUtil.ApplyBaseStatBonuses_Double(this, BaseStatProperty.Life, baseAmount, includeBuffs);
        }

        public float MovementSpeed([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                return (this.OwningPlayer.ActiveCharacter.MovementSpeed(includeBuffs) * ConfigSkills.Clone.MovementSpeedMultiplier);
            }
            float baseAmount = this.Character.getBaseStatFloat(BaseStatProperty.MovementSpeed);
            return CharacterStatModifierUtil.ApplyBaseStatBonuses_Float(this, BaseStatProperty.MovementSpeed, baseAmount, includeBuffs);
        }

        public List<string> offenseAttributesToRichTextFormattedStringList(string title)
        {
            string str;
            PetInstance instance = GameLogic.Binder.GameState.Player.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.SpawnedCharacterInstance != null))
            {
                str = MenuHelpers.BigValueToString(instance.SpawnedCharacterInstance.DamagePerHit(false));
            }
            else
            {
                str = "-";
            }
            List<string> list2 = new List<string>();
            list2.Add(MenuHelpers.BigValueToString(this.DamagePerHit(false)));
            list2.Add(MenuHelpers.BigModifierToString(this.getGenericModifierForPerkType(PerkType.CoreBonusDamagePerHit), true));
            list2.Add(MenuHelpers.BigModifierToString(this.getBaseStatModifier(BaseStatProperty.DamagePerHit), true));
            list2.Add(MathUtil.RoundToNumDecimals(this.AttacksPerSecond(false), 2).ToString());
            list2.Add(MenuHelpers.BigModifierToString(this.CleaveDamagePct(false), false));
            list2.Add(MenuHelpers.BigModifierToString(this.CriticalHitChancePct(false), false));
            list2.Add(MenuHelpers.BigModifierToString(this.CriticalHitMultiplier(false), false));
            list2.Add(str);
            return list2;
        }

        public bool ownsItem(Item item)
        {
            return (this.isItemEquipped(item) || this.isItemInInventory(item));
        }

        public bool ownsItem(string itemId)
        {
            return (this.isItemEquipped(itemId) || this.isItemInInventory(itemId));
        }

        public bool ownsItemInstance(ItemInstance itemInstance)
        {
            return (this.isItemInstanceEquipped(itemInstance) || this.isItemInstanceInInventory(itemInstance));
        }

        public void postDeserializeInitialization()
        {
            this.Character = GameLogic.Binder.CharacterResources.getResource(this.CharacterId);
            Assert.IsTrue_Release(this.Character != null, "Cannot link Character to CharacterInstance with id: " + this.CharacterId);
            for (int i = this.ActiveSkillTypes.Count - 1; i >= 0; i--)
            {
                if (((SkillType) this.ActiveSkillTypes[i]) == SkillType.NONE)
                {
                    this.ActiveSkillTypes.RemoveAt(i);
                }
            }
            while (this.ActiveSkillTypes.Count > ConfigSkills.SkillGroupCount)
            {
                this.ActiveSkillTypes.RemoveAt(this.ActiveSkillTypes.Count - 1);
            }
            IEnumerator enumerator = Enum.GetValues(typeof(ItemType)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ItemType current = (ItemType) ((int) enumerator.Current);
                    if (current != ItemType.UNSPECIFIED)
                    {
                        bool flag = false;
                        for (int m = 0; m < this.ItemSlots.Count; m++)
                        {
                            if (this.ItemSlots[m].CompatibleItemType == current)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            ItemSlot item = new ItemSlot();
                            item.CompatibleItemType = current;
                            this.ItemSlots.Add(item);
                        }
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            for (int j = 0; j < this.ItemSlots.Count; j++)
            {
                ItemInstance itemInstance = this.ItemSlots[j].ItemInstance;
                if (itemInstance != null)
                {
                    itemInstance.InspectedByPlayer = true;
                }
            }
            this.Inventory.postDeserializeInitialization();
            for (int k = 0; k < this.ItemSlots.Count; k++)
            {
                this.ItemSlots[k].postDeserializeInitialization();
            }
        }

        public void resetAttackTimer()
        {
            if (this.AttacksPerSecond(true) > 0f)
            {
                this.AttackCooldownTimer = this.getAttackDuration();
            }
            else
            {
                this.AttackCooldownTimer = float.MaxValue;
            }
        }

        public void resetDynamicRuntimeData()
        {
            CmdSetCharacterTarget.ExecuteStatic(this, null, Vector3.zero);
            CmdSetStunCondition.ExecuteStatic(this, false);
            CmdSetCharmCondition.ExecuteStatic(this, false);
            CmdSetConfusedCondition.ExecuteStatic(this, false);
            this.ExternallyControlled = false;
            this.Velocity = Vector3.zero;
            this.Facing = Vector3.forward;
            this.ManualTargetPos = Vector3.zero;
            this.RunAccelerationTimer.reset();
            this.IsDead = false;
            this.StunnedCount = 0;
            this.assignToDefaultLayer();
            this.PhysicsBody.CharacterController.enabled = true;
        }

        public double SkillDamage([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            double num;
            if (this.IsPlayerCharacter)
            {
                num = this.Character.getBaseStatDouble(BaseStatProperty.SkillDamage);
                List<ItemSlot> list = this.getItemSlots(false);
                for (int i = 0; i < list.Count; i++)
                {
                    ItemInstance itemInstance = list[i].ItemInstance;
                    if ((itemInstance != null) && (itemInstance.Item.Type == ItemType.Cloak))
                    {
                        num += App.Binder.ConfigMeta.ItemPowerCurve(itemInstance.Item.Type, this.OwningPlayer.getRiggedItemLevel(itemInstance), itemInstance.Rank);
                    }
                }
            }
            else
            {
                num = this.DamagePerHit(includeBuffs);
            }
            return CharacterStatModifierUtil.ApplyBaseStatBonuses_Double(this, BaseStatProperty.SkillDamage, num, includeBuffs);
        }

        public float Threat([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                float threatMultiplier = ConfigSkills.Clone.ThreatMultiplier;
                if (this.getPerkInstanceCount(PerkType.SkillUpgradeClone3) > 0)
                {
                    threatMultiplier = 2f;
                }
                return (this.OwningPlayer.ActiveCharacter.Threat(includeBuffs) * threatMultiplier);
            }
            float baseAmount = this.Character.getBaseStatFloat(BaseStatProperty.Threat);
            return CharacterStatModifierUtil.ApplyBaseStatBonuses_Float(this, BaseStatProperty.Threat, baseAmount, includeBuffs);
        }

        public float UniversalArmorBonus([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                return this.OwningPlayer.ActiveCharacter.UniversalArmorBonus(false);
            }
            float baseAmount = this.Character.getBaseStatFloat(BaseStatProperty.UniversalArmorBonus);
            return CharacterStatModifierUtil.ApplyBaseStatBonuses_Float(this, BaseStatProperty.UniversalArmorBonus, baseAmount, includeBuffs);
        }

        public float UniversalXpBonus([Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            if (this.IsSupport && (this.Prefab == CharacterPrefab.KnightClone))
            {
                return this.OwningPlayer.ActiveCharacter.UniversalXpBonus(false);
            }
            return (this.Character.getBaseStatFloat(BaseStatProperty.UniversalXpBonus) + this.getBaseStatModifier(BaseStatProperty.UniversalXpBonus));
        }

        public void updateHighestLevelItemOwned()
        {
            ItemInstance instance = this.getHighestLevelItemOwned();
            this.HighestLevelItemOwnedAtFloorStart.ItemType = instance.Item.Type;
            this.HighestLevelItemOwnedAtFloorStart.Level = instance.Level;
            this.HighestLevelItemOwnedAtFloorStart.Rank = instance.Rank;
        }

        public List<string> utilityAttributesToRichTextFormattedStringList(string title)
        {
            List<string> list2 = new List<string>();
            list2.Add(MenuHelpers.BigValueToString(this.SkillDamage(false)));
            list2.Add(MenuHelpers.BigModifierToString(this.getGenericModifierForPerkType(PerkType.CoreBonusSkillDamage), true));
            list2.Add(MathUtil.RoundToNumDecimals(GameLogic.Binder.FrenzySystem.getDuration(this), 2).ToString() + _.L(ConfigLoca.UNIT_SECONDS_SHORT, null, false));
            list2.Add(MenuHelpers.BigModifierToString(this.getGenericModifierForPerkType(PerkType.CoreBonusFrenzyDuration), true));
            list2.Add(MathUtil.RoundToNumDecimals(GameLogic.Binder.FrenzySystem.getDurationBonusPerKill(), 2).ToString() + _.L(ConfigLoca.UNIT_SECONDS_SHORT, null, false));
            list2.Add(MenuHelpers.BigModifierToString(this.getGenericModifierForPerkType(PerkType.CoreBonusTokens), true));
            list2.Add(MenuHelpers.BigModifierToString(this.getGenericModifierForPerkType(PerkType.CoreBonusCoins), true));
            list2.Add(MenuHelpers.BigModifierToString(this.getGenericModifierForPerkType(PerkType.CoinBonusPassive), true));
            return list2;
        }

        [JsonIgnore]
        public float CurrentHpNormalized
        {
            get
            {
                return Mathf.Clamp01((float) (this.CurrentHp / this.MaxLife(true)));
            }
        }

        [JsonIgnore]
        public string Id
        {
            [CompilerGenerated]
            get
            {
                return this.<Id>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Id>k__BackingField = value;
            }
        }

        [JsonIgnore]
        public bool IsPrimaryPlayerCharacter
        {
            get
            {
                return (this.IsPlayerCharacter && (this == this.OwningPlayer.ActiveCharacter));
            }
        }

        [JsonIgnore]
        public string Name
        {
            get
            {
                return this.Character.Name;
            }
        }

        [JsonIgnore]
        public CharacterPrefab Prefab
        {
            get
            {
                return this.Character.Prefab;
            }
        }

        [JsonIgnore]
        public float Radius
        {
            get
            {
                return this.Character.Radius;
            }
        }

        [JsonIgnore]
        public int Rarity
        {
            get
            {
                return this.Character.Rarity;
            }
        }

        [JsonIgnore]
        public GameLogic.CharacterType Type
        {
            get
            {
                return this.Character.Type;
            }
        }

        [JsonIgnore]
        public bool UsesRangedAttack
        {
            get
            {
                return (this.Character.RangedProjectileType != ProjectileType.NONE);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HighestLevelItemInfo
        {
            public GameLogic.ItemType ItemType;
            public int Level;
            public int Rank;
        }
    }
}

