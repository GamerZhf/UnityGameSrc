namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ItemInstance : IJsonData, ICharacterStatModifier, IComparable<ItemInstance>
    {
        [CompilerGenerated]
        private GameLogic.Item <Item>k__BackingField;
        public const string DEBUG_ITEM_INSTANCE_ID = "DEBUG_ITEM_INSTANCE_ID";
        public string DropVersion;
        [JsonIgnore]
        public int EvolveRank;
        public bool InspectedByPlayer;
        public string ItemId;
        public int Level;
        public PerkContainer Perks;
        public int Rank;
        public int RerollCount;
        public bool Unlocked;
        public int UnlockFloor;

        public ItemInstance()
        {
            this.ItemId = string.Empty;
            this.Perks = new PerkContainer();
            this.DropVersion = string.Empty;
        }

        public ItemInstance(ItemInstance prototype)
        {
            this.ItemId = string.Empty;
            this.Perks = new PerkContainer();
            this.DropVersion = string.Empty;
            this.copyFrom(prototype);
        }

        public ItemInstance(string itemId, int level, int rank, int unlockFloor, Player player)
        {
            this.ItemId = string.Empty;
            this.Perks = new PerkContainer();
            this.DropVersion = string.Empty;
            this.ItemId = itemId;
            this.Level = level;
            this.Rank = rank;
            this.UnlockFloor = unlockFloor;
            this.DropVersion = ConfigApp.BundleVersion;
            GameLogic.Item item = GameLogic.Binder.ItemResources.getResource(this.ItemId);
            ItemType itemType = item.Type;
            int rarity = item.Rarity;
            int num2 = ConfigPerks.NUM_RANDOM_PERKS_PER_ITEM_RARITY[rarity];
            for (int i = 0; i < num2; i++)
            {
                this.Perks.PerkInstances.Add(ConfigPerks.RollNewRandomItemPerkInstance(itemType, this.Perks, player));
            }
            this.postDeserializeInitialization();
        }

        public int CompareTo(ItemInstance other)
        {
            if (this.Level > other.Level)
            {
                return 1;
            }
            if (this.Level < other.Level)
            {
                return -1;
            }
            return this.Item.CompareTo(other.Item);
        }

        public void copyFrom(ItemInstance another)
        {
            this.ItemId = another.ItemId;
            this.Level = another.Level;
            this.Rank = another.Rank;
            this.UnlockFloor = another.UnlockFloor;
            this.Unlocked = another.Unlocked;
            this.InspectedByPlayer = another.InspectedByPlayer;
            this.DropVersion = another.DropVersion;
            this.Perks = new PerkContainer(another.Perks);
            this.Item = another.Item;
            this.RerollCount = another.RerollCount;
            this.EvolveRank = another.EvolveRank;
        }

        public static ItemInstance Create(GameLogic.Item item, Player player, [Optional, DefaultParameterValue(-1)] int startRank)
        {
            int num;
            if (startRank > -1)
            {
                num = startRank;
            }
            else
            {
                bool flag;
                num = Mathf.Clamp((player.ActiveCharacter.getHighestItemLevelPlusRankEquippedOrInInventory(item.Type, out flag) + UnityEngine.Random.Range(ConfigMeta.ITEM_START_RANK_OFFSET_MIN, ConfigMeta.ITEM_START_RANK_OFFSET_MAX + 1)) - 1, 0, 0x7fffffff);
            }
            return new ItemInstance(item.Id, 1, num, App.Binder.ConfigMeta.ItemUnlockFloor(player.getLastCompletedFloor(false) + 1), player);
        }

        public void enforcePerkLegality(Player player)
        {
            int num = ConfigPerks.NUM_RANDOM_PERKS_PER_ITEM_RARITY[this.Rarity];
            while (this.Perks.PerkInstances.Count > num)
            {
                this.Perks.PerkInstances.RemoveAt(this.Perks.PerkInstances.Count - 1);
            }
            while (this.Perks.PerkInstances.Count < num)
            {
                this.Perks.PerkInstances.Add(ConfigPerks.RollNewRandomItemPerkInstance(this.Item.Type, this.Perks, player));
            }
            for (int i = 0; i < this.Perks.PerkInstances.Count; i++)
            {
                ItemType itemType = this.Item.Type;
                PerkType type = this.Perks.PerkInstances[i].Type;
                ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[type];
                if (!ConfigPerks.ItemTypeSupportsPerk(itemType, type))
                {
                    this.Perks.PerkInstances[i] = ConfigPerks.RollNewRandomItemPerkInstance(itemType, this.Perks, player);
                }
                else if (!player.hasUnlockedSkill(data.LinkedToSkill))
                {
                    this.Perks.PerkInstances[i] = ConfigPerks.RollNewRandomItemPerkInstance(itemType, this.Perks, player);
                }
                else if (this.Perks.PerkInstances[i].Modifier < data.ModifierMin)
                {
                    this.Perks.PerkInstances[i].Modifier = data.ModifierMin;
                }
                else if (this.Perks.PerkInstances[i].Modifier > data.ModifierMax)
                {
                    this.Perks.PerkInstances[i].Modifier = data.ModifierMax;
                }
            }
        }

        public float getBaseStatModifier(BaseStatProperty prop)
        {
            float num = 0f;
            for (int i = 0; i < this.Perks.PerkInstances.Count; i++)
            {
                PerkInstance instance = this.Perks.PerkInstances[i];
                float num3 = instance.getBaseStatModifier(prop);
                num += num3;
                if (num3 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance.Type);
                }
            }
            for (int j = 0; j < this.Item.FixedPerks.PerkInstances.Count; j++)
            {
                PerkInstance instance2 = this.Item.FixedPerks.PerkInstances[j];
                float num5 = instance2.getBaseStatModifier(prop);
                num += num5;
                if (num5 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance2.Type);
                }
            }
            return num;
        }

        public float getCharacterTypeArmorModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.Perks.PerkInstances.Count; i++)
            {
                PerkInstance instance = this.Perks.PerkInstances[i];
                float num3 = instance.getCharacterTypeArmorModifier(characterType);
                num += num3;
                if (num3 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance.Type);
                }
            }
            for (int j = 0; j < this.Item.FixedPerks.PerkInstances.Count; j++)
            {
                PerkInstance instance2 = this.Item.FixedPerks.PerkInstances[j];
                float num5 = instance2.getCharacterTypeArmorModifier(characterType);
                num += num5;
                if (num5 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance2.Type);
                }
            }
            return num;
        }

        public float getCharacterTypeCoinModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.Perks.PerkInstances.Count; i++)
            {
                PerkInstance instance = this.Perks.PerkInstances[i];
                float num3 = instance.getCharacterTypeCoinModifier(characterType);
                num += num3;
                if (num3 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance.Type);
                }
            }
            for (int j = 0; j < this.Item.FixedPerks.PerkInstances.Count; j++)
            {
                PerkInstance instance2 = this.Item.FixedPerks.PerkInstances[j];
                float num5 = instance2.getCharacterTypeCoinModifier(characterType);
                num += num5;
                if (num5 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance2.Type);
                }
            }
            return num;
        }

        public float getCharacterTypeDamageModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.Perks.PerkInstances.Count; i++)
            {
                PerkInstance instance = this.Perks.PerkInstances[i];
                float num3 = instance.getCharacterTypeDamageModifier(characterType);
                num += num3;
                if (num3 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance.Type);
                }
            }
            for (int j = 0; j < this.Item.FixedPerks.PerkInstances.Count; j++)
            {
                PerkInstance instance2 = this.Item.FixedPerks.PerkInstances[j];
                float num5 = instance2.getCharacterTypeDamageModifier(characterType);
                num += num5;
                if (num5 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance2.Type);
                }
            }
            return num;
        }

        public float getCharacterTypeXpModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.Perks.PerkInstances.Count; i++)
            {
                PerkInstance instance = this.Perks.PerkInstances[i];
                float num3 = instance.getCharacterTypeXpModifier(characterType);
                num += num3;
                if (num3 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance.Type);
                }
            }
            for (int j = 0; j < this.Item.FixedPerks.PerkInstances.Count; j++)
            {
                PerkInstance instance2 = this.Item.FixedPerks.PerkInstances[j];
                float num5 = instance2.getCharacterTypeXpModifier(characterType);
                num += num5;
                if (num5 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance2.Type);
                }
            }
            return num;
        }

        public int getCompletedRankUpsForNextEvolve()
        {
            int evolveRank = this.EvolveRank;
            int num2 = 0;
            while (evolveRank > 0)
            {
                num2 += App.Binder.ConfigMeta.ItemEvolveCurve(evolveRank--);
            }
            return Mathf.Clamp(this.Rank - num2, 0, 0x7fffffff);
        }

        public float getEvolveBonusForPerk(PerkType perkType)
        {
            return this.getEvolveBonusForPerk(perkType, this.EvolveRank);
        }

        public float getEvolveBonusForPerk(PerkType perkType, int evolveRank)
        {
            return (ConfigPerks.SHARED_DATA[perkType].EvolveBonusPerRank * evolveRank);
        }

        public float getGenericModifierForPerkType(PerkType perkType)
        {
            float num = 0f;
            for (int i = 0; i < this.Perks.PerkInstances.Count; i++)
            {
                PerkInstance instance = this.Perks.PerkInstances[i];
                float num3 = instance.getGenericModifierForPerkType(perkType);
                num += num3;
                if (num3 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance.Type);
                }
            }
            for (int j = 0; j < this.Item.FixedPerks.PerkInstances.Count; j++)
            {
                PerkInstance instance2 = this.Item.FixedPerks.PerkInstances[j];
                float num5 = instance2.getGenericModifierForPerkType(perkType);
                num += num5;
                if (num5 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance2.Type);
                }
            }
            return num;
        }

        public float getNormalizedProgressToNextEvolve()
        {
            int num = this.getCompletedRankUpsForNextEvolve();
            int num2 = this.getRequiredRankUpsForNextEvolve();
            if (num2 == 0)
            {
                return 1f;
            }
            return Mathf.Clamp01(((float) num) / ((float) num2));
        }

        public int getPerkInstanceCount(PerkType perkType)
        {
            int num = 0;
            num += this.Perks.getPerkInstanceCount(perkType);
            return (num + this.Item.FixedPerks.getPerkInstanceCount(perkType));
        }

        public void getPerkInstancesOfType(PerkType perkType, IBuffIconProvider iconProvider, ref List<KeyValuePair<PerkInstance, BuffSource>> outPerkInstances)
        {
            this.Perks.getPerkInstancesOfType(perkType, this.Item, ref outPerkInstances);
            this.Item.FixedPerks.getPerkInstancesOfType(perkType, this.Item, ref outPerkInstances);
        }

        public int getRequiredRankUpsForNextEvolve()
        {
            int evolveRank = this.EvolveRank + 1;
            return App.Binder.ConfigMeta.ItemEvolveCurve(evolveRank);
        }

        public float getSkillCooldownModifier(SkillType skillType)
        {
            float num = 0f;
            for (int i = 0; i < this.Perks.PerkInstances.Count; i++)
            {
                PerkInstance instance = this.Perks.PerkInstances[i];
                float num3 = instance.getSkillCooldownModifier(skillType);
                num += num3;
                if (num3 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance.Type);
                }
            }
            for (int j = 0; j < this.Item.FixedPerks.PerkInstances.Count; j++)
            {
                PerkInstance instance2 = this.Item.FixedPerks.PerkInstances[j];
                float num5 = instance2.getSkillCooldownModifier(skillType);
                num += num5;
                if (num5 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance2.Type);
                }
            }
            return num;
        }

        public float getSkillDamageModifier(SkillType skillType)
        {
            float num = 0f;
            for (int i = 0; i < this.Perks.PerkInstances.Count; i++)
            {
                PerkInstance instance = this.Perks.PerkInstances[i];
                float num3 = instance.getSkillDamageModifier(skillType);
                num += num3;
                if (num3 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance.Type);
                }
            }
            for (int j = 0; j < this.Item.FixedPerks.PerkInstances.Count; j++)
            {
                PerkInstance instance2 = this.Item.FixedPerks.PerkInstances[j];
                float num5 = instance2.getSkillDamageModifier(skillType);
                num += num5;
                if (num5 != 0f)
                {
                    num += this.getEvolveBonusForPerk(instance2.Type);
                }
            }
            return num;
        }

        public int getSkillExtraCharges(SkillType skillType)
        {
            return (this.Perks.getSkillExtraCharges(skillType) + this.Item.FixedPerks.getSkillExtraCharges(skillType));
        }

        public bool hasSkillInvulnerability(SkillType skillType)
        {
            return (this.Perks.hasSkillInvulnerability(skillType) || this.Item.FixedPerks.hasSkillInvulnerability(skillType));
        }

        public bool isAtMaxRank()
        {
            return false;
        }

        public bool isDebugItemInstance()
        {
            return (this.ItemId == "DEBUG_ITEM_INSTANCE_ID");
        }

        public void postDeserializeInitialization()
        {
            if (this.isDebugItemInstance())
            {
                GameLogic.Item item = new GameLogic.Item();
                item.Id = "DEBUG_ITEM_INSTANCE_ID";
                item.Rarity = 0;
                item.Type = ItemType.Weapon;
                item.SpriteId = "sprite_weapon001";
                item.Name = "Longsword";
                item.postDeserializeInitialization();
                this.Item = item;
            }
            else
            {
                this.Item = GameLogic.Binder.ItemResources.getResource(this.ItemId);
                if (this.Item == null)
                {
                    Debug.LogError("Item not found: " + this.ItemId);
                }
            }
            if (string.IsNullOrEmpty(this.DropVersion))
            {
                this.DropVersion = ConfigApp.BundleVersion;
            }
        }

        [JsonIgnore]
        public GameLogic.Item Item
        {
            [CompilerGenerated]
            get
            {
                return this.<Item>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Item>k__BackingField = value;
            }
        }

        [JsonIgnore]
        public int LevelPlusRank
        {
            get
            {
                return (this.Level + this.Rank);
            }
        }

        [JsonIgnore]
        public int Rarity
        {
            get
            {
                return this.Item.Rarity;
            }
        }
    }
}

