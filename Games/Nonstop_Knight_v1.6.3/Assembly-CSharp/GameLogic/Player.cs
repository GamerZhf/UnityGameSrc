namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Player : Versionable, IDatabaseObject, IJsonData, ICharacterStatModifier
    {
        [CompilerGenerated]
        private string <_id>k__BackingField;
        public GameLogic.Achievements Achievements = new GameLogic.Achievements();
        public int ActiveCharacterIndex;
        public PlayerAugmentations Augmentations = new PlayerAugmentations();
        public Dictionary<string, int> BestedLeaderboardUserIds = new Dictionary<string, int>();
        public Dictionary<string, long> BoostStartTimestamps = new Dictionary<string, long>();
        [JsonIgnore]
        public BossTrainData BossTrain = new BossTrainData();
        public List<CharacterInstance> CharacterInstances = new List<CharacterInstance>();
        public List<string> ClaimedIapTransactionIds = new List<string>();
        [JsonIgnore]
        public List<string> ClaimedRewardIds = new List<string>();
        public List<string> CompletedSideQuests = new List<string>();
        public List<string> CompletedTutorials = new List<string>();
        public HeroStats CumulativeRetiredHeroStats = new HeroStats();
        public int DailyAdCount;
        public int DailyAdCountMystery;
        public int DailyAdCountVendor;
        public double DailyDiamondDropCount;
        [JsonIgnore]
        public Dictionary<string, bool> DungeonExplored = new Dictionary<string, bool>();
        public string FgUserHandle;
        public bool HasOpenedStarterBundleOfferFromTaskPanel;
        public bool HasPurchasedStarterBundle;
        public bool HasSeenRateGamePopup;
        public bool HasUnlockedMissions;
        public bool HighestFloorRewardClaimedDuringThisRun;
        public bool IsDisconnectedFromPlatform;
        public bool IsReadyForRateGamePopup;
        public List<ItemType> ItemTypeRollHistory = new List<ItemType>();
        public bool LastBossEncounterFailed;
        public int LastCompletedFloor = -1;
        public long LastDailyAdCountResetTimestamp;
        public long LastDailyDiamondDropCountResetTimestamp;
        [JsonIgnore]
        public long LastDungeonChallengeTimestamp;
        public long LastMysteryChestDropTimestamp;
        public long LastPassiveRewardClaimTimestamp;
        public string LastRolledItemId = string.Empty;
        public long LastSerializationTimestamp;
        [JsonIgnore]
        public long LastSerializationTimestampDuringDeserialization;
        [JsonIgnore]
        public int LastVisualizedMapNodeUnlockingIndex = -1;
        [JsonIgnore]
        public string LeaderboardUserId = string.Empty;
        private bool m_postDeserializeInitComplete;
        private List<ItemType> m_tempCandidateList = new List<ItemType>(ConfigMeta.ACTIVE_ITEM_TYPES.Count);
        [JsonIgnore]
        public Dictionary<string, int> Materials = new Dictionary<string, int>();
        public int MinionsKilledSinceLastRoomCompletion;
        public GameLogic.Missions Missions = new GameLogic.Missions(MissionType.Adventure);
        public bool MusicEnabled = true;
        public int MysteryChestsWithCoinsConsumed;
        public bool NextLeaderboardTargetVisible;
        [JsonIgnore]
        public PlayerNotifiers Notifiers;
        public int NumPendingRankUpCeremonies;
        public List<string> OneShotAnalyticsEvents = new List<string>();
        public string Password;
        [JsonIgnore]
        public List<string> PendingDungeonCompletionRewards = new List<string>();
        public bool PendingPostRetirementGiftRewardCeremony;
        public bool PendingPostRetirementTokenRewardCeremony;
        public List<string> PendingRankUpRunestoneUnlocks = new List<string>();
        public List<SkillType> PendingSkillUnlocks = new List<SkillType>();
        public GameLogic.Pets Pets = new GameLogic.Pets();
        public PlayerPreferences Preferences = new PlayerPreferences();
        public GameLogic.PromotionEvents PromotionEvents = new GameLogic.PromotionEvents();
        public Dictionary<string, ulong> PromotionStates = new Dictionary<string, ulong>();
        public int Rank = 1;
        public Service.RemoteNotificationPlayerData RemoteNotificationPlayerData = new Service.RemoteNotificationPlayerData();
        [JsonIgnore]
        public Dictionary<string, long> RenewableResourceTimestamps = new Dictionary<string, long>();
        public Dictionary<string, double> Resources = new Dictionary<string, double>();
        [JsonIgnore]
        public XXHash RetirementHash = new XXHash(1);
        public int RetirementRandomSeed;
        public GameLogic.Runestones Runestones = new GameLogic.Runestones();
        public GameLogic.ServerStats ServerStats = new GameLogic.ServerStats();
        public string SessionId;
        public Dictionary<string, int> ShopEntryStackExhaustedCount = new Dictionary<string, int>();
        public GameLogic.SocialData SocialData = new GameLogic.SocialData();
        public bool SoundEnabled = true;
        public int TipIndex;
        public GameLogic.Tournaments Tournaments = new GameLogic.Tournaments();
        public PlayerTrackingData TrackingData = new PlayerTrackingData();
        public List<LeaderboardEntry> UnclaimedLeaderboardRewards = new List<LeaderboardEntry>();
        public long UnclaimedPassiveRewardableSeconds;
        public List<Reward> UnclaimedRewards = new List<Reward>();
        public List<Reward> UnclaimedUpdateRewards = new List<Reward>();
        public List<SkillType> UnlockedSkills = new List<SkillType>();
        public string UnshownVersionUpgradeNotification;
        public int UnusedFreeRevives;
        public int UnusedFrenzyPotions;
        public GameLogic.Vendor Vendor = new GameLogic.Vendor();

        public void addPassiveProgress(long numSeconds)
        {
            this.UnclaimedPassiveRewardableSeconds += numSeconds;
        }

        public void addResources(ResourceType rt, double amount)
        {
            string key = ConfigMeta.RESOURCE_TYPE_TO_STRING_MAPPING[rt];
            Dictionary<string, double> dictionary = this.getResources(rt != ResourceType.Coin);
            if (dictionary.ContainsKey(key))
            {
                Dictionary<string, double> dictionary2;
                string str2;
                double num = dictionary2[str2];
                (dictionary2 = dictionary)[str2 = key] = num + amount;
            }
            else
            {
                dictionary.Add(key, amount);
            }
        }

        public bool autoSummonBossInFloor(int floor)
        {
            if (!this.bossAutoSummonPossibleInFloor(floor))
            {
                return false;
            }
            if (!this.Preferences.AutoSummonBosses)
            {
                return false;
            }
            return true;
        }

        public bool boostEntryExists(BoostType boost)
        {
            if (this.BoostStartTimestamps.Count == 0)
            {
                return false;
            }
            return this.BoostStartTimestamps.ContainsKey(boost.ToString());
        }

        public bool bossAutoSummonPossibleInFloor(int floor)
        {
            if (!App.Binder.ConfigMeta.BOSS_AUTO_SUMMON_ENABLED)
            {
                return false;
            }
            if (!this.Tournaments.hasTournamentSelected())
            {
                if (this.CumulativeRetiredHeroStats.HeroesRetired < 2)
                {
                    return false;
                }
                if (!App.Binder.ConfigMeta.ALLOW_BOSS_AUTO_SUMMON_IN_NEW_FLOORS && (floor >= this.getHighestFloorReached()))
                {
                    return false;
                }
            }
            return true;
        }

        public bool bossBundlesCanDrop()
        {
            bool flag = this.ActiveCharacter.Inventory.BossPotions < App.Binder.ConfigMeta.THRESHHOLD_DONTOFFER_BOSSTICKET;
            return ((App.Binder.ConfigMeta.BOSS_POTIONS_ENABLED && this.hasRetired()) && flag);
        }

        public double calculateStandardCoinRoll(double baseCoinReward, GameLogic.CharacterType killedCharacterType, [Optional, DefaultParameterValue(1)] int multiplier)
        {
            double baseAmount = baseCoinReward;
            baseAmount *= multiplier;
            return Math.Floor(CharacterStatModifierUtil.ApplyCoinBonuses(this.ActiveCharacter, killedCharacterType, baseAmount, false));
        }

        public bool canEvolveItem(ItemInstance ii)
        {
            return false;
        }

        public bool canManuallySummonFloorBoss(ActiveDungeon ad)
        {
            return (ad.isBossFloor() && (ConfigApp.CHEAT_BOSS_ALWAYS_SUMMONABLE || this.floorCompletionGoalSatisfied(ad)));
        }

        public bool canRankUp()
        {
            if (this.getResourceAmount(ResourceType.Xp) < App.Binder.ConfigMeta.XpRequiredForRankUp(this.Rank))
            {
                return false;
            }
            return true;
        }

        public bool canRankUpActiveCharacter()
        {
            return this.canRankUpCharacterInstance(this.ActiveCharacter, this.getResourceAmount(ResourceType.Coin));
        }

        public bool canRankUpCharacterInstance(CharacterInstance characterInstance, double coins)
        {
            if (characterInstance == null)
            {
                return false;
            }
            if (characterInstance.isAtMaxRank())
            {
                return false;
            }
            return true;
        }

        public bool canRetire()
        {
            return ((this.getFirstUnclaimedRetirementTriggerChest() != null) && !this.Tournaments.hasTournamentSelected());
        }

        public bool canUnlockItemInstance(ItemInstance ii)
        {
            if (ii == null)
            {
                return false;
            }
            if (ii.Unlocked)
            {
                return false;
            }
            if (!this.ActiveCharacter.hasReachedUnlockFloorForItem(ii))
            {
                return false;
            }
            return true;
        }

        public bool canUnlockSkill(SkillType skillType)
        {
            if (this.Rank < ConfigSkills.SHARED_DATA[skillType].UnlockRank)
            {
                return false;
            }
            SkillInstance instance = this.ActiveCharacter.getSkillInstance(skillType);
            if ((instance != null) && (instance.Rank > 0))
            {
                return false;
            }
            return true;
        }

        public bool canUpgradeItemInstance(ItemInstance ii, double coins)
        {
            if (ii == null)
            {
                return false;
            }
            if (ii.isAtMaxRank())
            {
                return false;
            }
            if (!ii.Unlocked)
            {
                return false;
            }
            if (!this.ActiveCharacter.isItemInstanceEquipped(ii))
            {
                return false;
            }
            int rank = ii.Rank + 1;
            if (this.ActiveCharacter.getAdjustedItemUpgradeCost(ii.Item.Type, this.getRiggedItemLevel(ii), rank) > coins)
            {
                return false;
            }
            if ((ii.Item.Type == ItemType.Armor) && !this.hasCompletedTutorial("TUT051A"))
            {
                return false;
            }
            if ((ii.Item.Type == ItemType.Cloak) && !this.hasCompletedTutorial("TUT052A"))
            {
                return false;
            }
            return true;
        }

        public bool chestTypeCanDrop(ChestType chestType)
        {
            int num = this.getCurrentFloor(false);
            if (ConfigMeta.IsSpecialBossChest(chestType) && (num < App.Binder.ConfigMeta.SPECIAL_CHEST_DROP_MIN_FLOOR))
            {
                return false;
            }
            if (!this.isChestEnabled(chestType))
            {
                return false;
            }
            if (this.getHighestFloorReached() < App.Binder.ConfigMeta.CHEST_UNLOCK_FLOOR[chestType])
            {
                return false;
            }
            if (App.Binder.ConfigMeta.CHEST_DROP_MAX_FLOOR.ContainsKey(chestType) && (num > App.Binder.ConfigMeta.CHEST_DROP_MAX_FLOOR[chestType]))
            {
                return false;
            }
            return true;
        }

        public int clampItemRarityToMaxAllowed(int rarity, int floor, bool isBossDrop)
        {
            while (!this.isItemRarityAllowed(rarity, floor, isBossDrop))
            {
                rarity--;
                if (rarity <= 0)
                {
                    return rarity;
                }
            }
            return rarity;
        }

        public bool combatStatsEnabled()
        {
            return (App.Binder.ConfigMeta.COMBAT_STATS_ENABLED && this.Preferences.CombatStatsEnabled);
        }

        public bool diamondsCanDrop()
        {
            if (this.DailyDiamondDropCount >= App.Binder.ConfigMeta.DAILY_DIAMOND_LIMIT)
            {
                return false;
            }
            return true;
        }

        public bool doOfferStarterBundle()
        {
            if (!App.Binder.ConfigMeta.STARTER_BUNDLE_SELLING_ENABLED)
            {
                return false;
            }
            if (this.HasPurchasedStarterBundle || this.HasOpenedStarterBundleOfferFromTaskPanel)
            {
                return false;
            }
            if (!this.hasRetired())
            {
                return false;
            }
            if (Service.Binder.ShopManager.GetShopEntryBySlot(ShopManager.ValidBundleSlots[0]) == null)
            {
                return false;
            }
            return ((this.getLastCompletedFloor(true) + 1) >= App.Binder.ConfigMeta.STARTER_BUNDLE_TASKPANEL_OFFER_FLOOR);
        }

        public bool doShowRateGamePopup()
        {
            return (!this.HasSeenRateGamePopup && this.IsReadyForRateGamePopup);
        }

        public bool doUnlockMissions()
        {
            return ((this.CumulativeRetiredHeroStats.HeroesRetired >= 2) || this.hasCompletedAllTutorialsInCategory(ConfigTutorials.MISSION_AND_PET_TUTORIALS));
        }

        public bool floorCompleted(int floor)
        {
            return (floor <= this.getLastCompletedFloor(false));
        }

        public bool floorCompletionGoalSatisfied(ActiveDungeon ad)
        {
            return (this.getRemainingMinionKillsUntilFloorCompletion(ad.Floor, ad.isTutorialDungeon(), this.getLastBossEncounterFailed(false)) <= 0);
        }

        public bool frenzyBundlesCanDrop()
        {
            bool flag = this.ActiveCharacter.Inventory.FrenzyPotions < App.Binder.ConfigMeta.THRESHHOLD_DONTOFFER_FRENZY;
            return (this.hasRetired() && flag);
        }

        public double getActiveTokenRewardFloorMultiplier()
        {
            int num = this.getLastCompletedFloor(false) + 1;
            double num2 = 1.0;
            for (int i = 0; i < App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS.Count; i++)
            {
                int key = App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS[i].Key;
                if (num < key)
                {
                    return num2;
                }
                double num5 = App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS[i].Value;
                num2 = Math.Max(num2, num5);
            }
            return num2;
        }

        public List<ChestType> getAvailableSpecialChestTypes()
        {
            List<ChestType> list = new List<ChestType>();
            for (int i = 0; i < ConfigMeta.BOSS_CHESTS_SPECIAL.Length; i++)
            {
                ChestType chestType = ConfigMeta.BOSS_CHESTS_SPECIAL[i];
                if (this.hasEncounteredChest(chestType))
                {
                    list.Add(chestType);
                }
            }
            return list;
        }

        public float getBaseStatModifier(BaseStatProperty baseStat)
        {
            float num = 0f;
            if (this.Tournaments.hasTournamentSelected())
            {
                num += this.Tournaments.SelectedTournament.Upgrades.getBaseStatModifier(baseStat);
            }
            else
            {
                num += this.Augmentations.getBaseStatModifier(baseStat);
            }
            num += this.Runestones.getBaseStatModifier(baseStat);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                num += instance.Character.FixedPerks.getBaseStatModifier(instance.Level, baseStat);
            }
            return num;
        }

        public CharacterInstance getCharacterInstance(string characterId)
        {
            for (int i = 0; i < this.CharacterInstances.Count; i++)
            {
                if (this.CharacterInstances[i].CharacterId == characterId)
                {
                    return this.CharacterInstances[i];
                }
            }
            return null;
        }

        public float getCharacterTypeArmorModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            if (this.Tournaments.hasTournamentSelected())
            {
                num += this.Tournaments.SelectedTournament.Upgrades.getCharacterTypeArmorModifier(characterType);
            }
            else
            {
                num += this.Augmentations.getCharacterTypeArmorModifier(characterType);
            }
            num += this.Runestones.getCharacterTypeArmorModifier(characterType);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                num += instance.Character.FixedPerks.getCharacterTypeArmorModifier(instance.Level, characterType);
            }
            return num;
        }

        public float getCharacterTypeCoinModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            if (this.Tournaments.hasTournamentSelected())
            {
                num += this.Tournaments.SelectedTournament.Upgrades.getCharacterTypeCoinModifier(characterType);
            }
            else
            {
                num += this.Augmentations.getCharacterTypeCoinModifier(characterType);
            }
            num += this.Runestones.getCharacterTypeCoinModifier(characterType);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                num += instance.Character.FixedPerks.getCharacterTypeCoinModifier(instance.Level, characterType);
            }
            return num;
        }

        public float getCharacterTypeDamageModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            if (this.Tournaments.hasTournamentSelected())
            {
                num += this.Tournaments.SelectedTournament.Upgrades.getCharacterTypeDamageModifier(characterType);
            }
            else
            {
                num += this.Augmentations.getCharacterTypeDamageModifier(characterType);
            }
            num += this.Runestones.getCharacterTypeDamageModifier(characterType);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                num += instance.Character.FixedPerks.getCharacterTypeDamageModifier(instance.Level, characterType);
            }
            return num;
        }

        public float getCharacterTypeXpModifier(GameLogic.CharacterType characterType)
        {
            float num = 0f;
            if (this.Tournaments.hasTournamentSelected())
            {
                num += this.Tournaments.SelectedTournament.Upgrades.getCharacterTypeXpModifier(characterType);
            }
            else
            {
                num += this.Augmentations.getCharacterTypeXpModifier(characterType);
            }
            num += this.Runestones.getCharacterTypeXpModifier(characterType);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                num += instance.Character.FixedPerks.getCharacterTypeXpModifier(instance.Level, characterType);
            }
            return num;
        }

        public int getCurrentFloor([Optional, DefaultParameterValue(false)] bool ignoreTournaments)
        {
            return (this.getLastCompletedFloor(ignoreTournaments) + 1);
        }

        public Reward getFirstUnclaimedBossReward()
        {
            for (int i = this.UnclaimedRewards.Count - 1; i >= 0; i--)
            {
                Reward reward = this.UnclaimedRewards[i];
                if (this.isClaimableReward(reward) && ConfigMeta.IsBossChest(reward.ChestType))
                {
                    return reward;
                }
            }
            return null;
        }

        public Reward getFirstUnclaimedRetirementTriggerChest()
        {
            for (int i = this.UnclaimedRewards.Count - 1; i >= 0; i--)
            {
                Reward reward = this.UnclaimedRewards[i];
                if (reward.ChestType == ChestType.RetirementTrigger)
                {
                    return reward;
                }
            }
            return null;
        }

        public Reward getFirstUnclaimedReward(ChestType chestType)
        {
            for (int i = this.UnclaimedRewards.Count - 1; i >= 0; i--)
            {
                Reward reward = this.UnclaimedRewards[i];
                if (reward.ChestType == chestType)
                {
                    return reward;
                }
            }
            return null;
        }

        public Reward getFirstUnclaimedReward(ChestType chestType, RewardSourceType rewardSource)
        {
            for (int i = this.UnclaimedRewards.Count - 1; i >= 0; i--)
            {
                Reward reward = this.UnclaimedRewards[i];
                if ((reward.ChestType == chestType) && (reward.RewardSource == rewardSource))
                {
                    return reward;
                }
            }
            return null;
        }

        public Reward getFirstUnclaimedReward(ChestType chestType, RewardSourceType rewardSource, string rewardSourceId)
        {
            for (int i = this.UnclaimedRewards.Count - 1; i >= 0; i--)
            {
                Reward reward = this.UnclaimedRewards[i];
                if (((reward.ChestType == chestType) && (reward.RewardSource == rewardSource)) && (reward.RewardSourceId == rewardSourceId))
                {
                    return reward;
                }
            }
            return null;
        }

        public Reward getFirstUnclaimedSocialReward()
        {
            for (int i = this.UnclaimedRewards.Count - 1; i >= 0; i--)
            {
                Reward reward = this.UnclaimedRewards[i];
                if (reward.isSocialGift())
                {
                    return reward;
                }
            }
            return null;
        }

        public int getGatedRetirementMinFloor()
        {
            return App.Binder.ConfigMeta.RETIREMENT_MIN_FLOOR;
        }

        public float getGenericModifierForPerkType(PerkType perkType)
        {
            float num = 0f;
            if (this.Tournaments.hasTournamentSelected())
            {
                num += this.Tournaments.SelectedTournament.Upgrades.getGenericModifierForPerkType(perkType);
            }
            else
            {
                num += this.Augmentations.getGenericModifierForPerkType(perkType);
            }
            num += this.Runestones.getGenericModifierForPerkType(perkType);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                num += instance.Character.FixedPerks.getGenericModifierForPerkType(instance.Level, perkType);
            }
            return num;
        }

        public int getHighestFloorReached()
        {
            return Mathf.Max(this.CumulativeRetiredHeroStats.HighestFloor, this.ActiveCharacter.HeroStats.HighestFloor);
        }

        public double getHighestTokenGainWithRetirement()
        {
            return Math.Max(this.CumulativeRetiredHeroStats.HighestTokenGainWithRetirement, this.ActiveCharacter.HeroStats.HighestTokenGainWithRetirement);
        }

        public double getHighestTokenMultiplierReached()
        {
            return Math.Max(this.CumulativeRetiredHeroStats.HighestTokenMultiplier, this.ActiveCharacter.HeroStats.HighestTokenMultiplier);
        }

        public bool getLastBossEncounterFailed([Optional, DefaultParameterValue(false)] bool ignoreTournaments)
        {
            if (!ignoreTournaments && this.Tournaments.hasTournamentSelected())
            {
                return this.Tournaments.SelectedTournament.LastBossEncounterFailed;
            }
            return this.LastBossEncounterFailed;
        }

        public int getLastCompletedFloor([Optional, DefaultParameterValue(false)] bool ignoreTournaments)
        {
            if (!ignoreTournaments && this.Tournaments.hasTournamentSelected())
            {
                return this.Tournaments.SelectedTournament.LastCompletedFloor;
            }
            return this.LastCompletedFloor;
        }

        public int getMaterialAmount(string itemId)
        {
            if (this.Materials.ContainsKey(itemId))
            {
                return this.Materials[itemId];
            }
            return 0;
        }

        public int getMinionsKilledSinceLastRoomCompletion([Optional, DefaultParameterValue(false)] bool ignoreTournaments)
        {
            if (!ignoreTournaments && this.Tournaments.hasTournamentSelected())
            {
                return this.Tournaments.SelectedTournament.MinionsKilledSinceLastRoomCompletion;
            }
            return this.MinionsKilledSinceLastRoomCompletion;
        }

        public double getMysteryCoinOfferMultiplier(string shopEntryId)
        {
            ConfigMeta configMeta = App.Binder.ConfigMeta;
            double num = this.MysteryChestsWithCoinsConsumed * configMeta.DIMINISHINGCOINS_MULTIPLIER;
            double num2 = configMeta.VENDOR_COIN_BUNDLES[shopEntryId];
            double num3 = num2 - num;
            return Math.Max(num3, App.Binder.ConfigMeta.MYSTERYCHEST_COINBUNDLE_MINIMUM);
        }

        public void getNextTokenRewardFloorMultiplierInfo(int fromFloor, out int nextFloor, out double nextMultiplier)
        {
            int num = fromFloor;
            nextFloor = App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS[App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS.Count - 1].Key;
            nextMultiplier = App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS[App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS.Count - 1].Value;
            if (num > nextFloor)
            {
                nextFloor = -1;
                nextMultiplier = 0.0;
            }
            else
            {
                for (int i = 0; i < App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS.Count; i++)
                {
                    int key = App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS[i].Key;
                    if (key >= num)
                    {
                        nextFloor = key;
                        nextMultiplier = App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS[i].Value;
                        break;
                    }
                }
            }
        }

        public float getNormalizedProgressTowardsNextRankUp()
        {
            return Mathf.Clamp01((float) (this.getResourceAmount(ResourceType.Xp) / App.Binder.ConfigMeta.XpRequiredForRankUp(this.Rank)));
        }

        public int getNumBasicChestItemsEncountered()
        {
            int num = 0;
            for (int i = 0; i < ConfigMeta.BOSS_CHESTS_BASIC.Length; i++)
            {
                LootTable table = GameLogic.Binder.ItemResources.getDynamicChestLootTable(ConfigMeta.BOSS_CHESTS_BASIC[i]);
                for (int j = 0; j < table.Items.Count; j++)
                {
                    if (this.hasEncounteredItem(table.Items[j].Id))
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int getNumberOfAllowedPurchases(string shopEntryId)
        {
            VendorPriceData vendorPriceData = App.Binder.ConfigMeta.GetVendorPriceData(shopEntryId);
            if (vendorPriceData == null)
            {
                return -1;
            }
            if (App.Binder.ConfigMeta.VENDORSTACKSIZE_ALWAYS_USE_MAX)
            {
                return vendorPriceData.NumAllowedPurchases;
            }
            int num = 0;
            if (this.ShopEntryStackExhaustedCount.ContainsKey(shopEntryId))
            {
                num = this.ShopEntryStackExhaustedCount[shopEntryId];
            }
            int num2 = vendorPriceData.MinimumStack + num;
            return Math.Min(num2, vendorPriceData.NumAllowedPurchases);
        }

        public int getNumberOfUnclaimedBossChests()
        {
            int num = 0;
            for (int i = 0; i < this.UnclaimedRewards.Count; i++)
            {
                Reward reward = this.UnclaimedRewards[i];
                if (this.isClaimableReward(reward) && ConfigMeta.IsBossChest(reward.ChestType))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfUnclaimedChestsOfType(ChestType chestType)
        {
            int num = 0;
            for (int i = 0; i < this.UnclaimedRewards.Count; i++)
            {
                if (this.UnclaimedRewards[i].ChestType == chestType)
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfUnclaimedChestsOfType(ChestType chestType, RewardSourceType rewardSource)
        {
            int num = 0;
            for (int i = 0; i < this.UnclaimedRewards.Count; i++)
            {
                if ((this.UnclaimedRewards[i].ChestType == chestType) && (this.UnclaimedRewards[i].RewardSource == rewardSource))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfUnclaimedLevelUpRewards()
        {
            return ((this.PendingSkillUnlocks.Count + this.PendingRankUpRunestoneUnlocks.Count) + this.NumPendingRankUpCeremonies);
        }

        public int getNumberOfUnclaimedMysteryChests()
        {
            int num = 0;
            for (int i = 0; i < this.UnclaimedRewards.Count; i++)
            {
                Reward reward = this.UnclaimedRewards[i];
                if (this.isClaimableReward(reward) && ConfigMeta.IsMysteryChest(reward.ChestType))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfUnclaimedRetirementChests()
        {
            int num = 0;
            for (int i = 0; i < this.UnclaimedRewards.Count; i++)
            {
                if (ConfigMeta.IsRetirementChest(this.UnclaimedRewards[i].ChestType))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfUnclaimedSocialGifts()
        {
            int num = 0;
            for (int i = 0; i < this.UnclaimedRewards.Count; i++)
            {
                if (this.UnclaimedRewards[i].isSocialGift())
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumBossChestItemsEncountered()
        {
            return ((this.getNumBasicChestItemsEncountered() + this.getNumSpecialChestItemsEncountered()) + this.getNumEventChestItemsEncountered());
        }

        public int getNumEventChestItemsEncountered()
        {
            int num = 0;
            for (int i = 0; i < ConfigMeta.BOSS_CHESTS_EVENT.Length; i++)
            {
                LootTable table = GameLogic.Binder.ItemResources.getDynamicChestLootTable(ConfigMeta.BOSS_CHESTS_EVENT[i]);
                for (int j = 0; j < table.Items.Count; j++)
                {
                    if (this.hasEncounteredItem(table.Items[j].Id))
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int getNumEventChestTypesEnabled()
        {
            int num = 0;
            for (int i = 0; i < ConfigMeta.BOSS_CHESTS_EVENT.Length; i++)
            {
                if (this.isChestEnabled(ConfigMeta.BOSS_CHESTS_EVENT[i]))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumFirstBossSummonCount()
        {
            return Mathf.Max(this.CumulativeRetiredHeroStats.FirstBossSummonCount, this.ActiveCharacter.HeroStats.FirstBossSummonCount);
        }

        public double getNumItemUpgrades()
        {
            return Math.Max(this.CumulativeRetiredHeroStats.ItemUpgrades, this.ActiveCharacter.HeroStats.ItemUpgrades);
        }

        public double getNumMonstersKilled()
        {
            return Math.Max(this.CumulativeRetiredHeroStats.MonstersKilled, this.ActiveCharacter.HeroStats.MonstersKilled);
        }

        public int getNumSpecialChestItemsEncountered()
        {
            int num = 0;
            for (int i = 0; i < ConfigMeta.BOSS_CHESTS_SPECIAL.Length; i++)
            {
                LootTable table = GameLogic.Binder.ItemResources.getDynamicChestLootTable(ConfigMeta.BOSS_CHESTS_SPECIAL[i]);
                for (int j = 0; j < table.Items.Count; j++)
                {
                    if (this.hasEncounteredItem(table.Items[j].Id))
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public PassiveProgress getPassiveProgress(int startFloor, long numSeconds, [Optional, DefaultParameterValue(false)] bool stopOnceCoinDropsRunOut)
        {
            int floor = startFloor;
            bool lastBossEncounterFailed = this.getLastBossEncounterFailed(false);
            int num2 = App.Binder.ConfigMeta.PassiveMaxMinionCoinDropCount();
            double num3 = numSeconds;
            int num4 = App.Binder.ConfigMeta.PASSIVE_MAX_BOSS_ENCOUNTER_COUNT;
            double num5 = App.Binder.ConfigMeta.PassiveMinionKillFrequencySeconds();
            PassiveProgress progress = new PassiveProgress();
            progress.NumOverflowedMinionKills = this.getMinionsKilledSinceLastRoomCompletion(false);
            while (num3 > 0.0)
            {
                Dungeon dungeon = GameLogic.Binder.DungeonResources.getResource(ConfigDungeons.GetDungeonIdForFloor(floor));
                double num6 = App.Binder.ConfigMeta.MinionCoinDropCurve(floor);
                int num7 = this.getRequiredMinionKillsForFloorCompletion(floor, false, lastBossEncounterFailed);
                if (floor == (this.getLastCompletedFloor(false) + 1))
                {
                    num7 -= this.getMinionsKilledSinceLastRoomCompletion(false);
                }
                while ((num7 > 0) && (num3 >= num5))
                {
                    num7--;
                    progress.NumMinionKills++;
                    progress.NumMinionKillsOnBossProgressStop++;
                    progress.NumOverflowedMinionKills++;
                    if (--num2 >= 0)
                    {
                        progress.PassiveCoinReward += num6;
                    }
                    num3 -= num5;
                }
                if (num7 > 0)
                {
                    break;
                }
                if (dungeon.hasBoss())
                {
                    if (((floor >= this.getHighestFloorReached()) || (num3 < App.Binder.ConfigMeta.PASSIVE_BOSS_KILL_FREQUENCY_SECONDS)) || (num4 == 0))
                    {
                        break;
                    }
                    if (this.canRetire())
                    {
                        progress.PassiveTokenReward += App.Binder.ConfigMeta.RetirementTokenReward(this.ActiveCharacter, floor);
                    }
                    if ((ConfigMeta.BOSS_ADDITIONAL_DROPS_ENABLED && this.canRetire()) && App.Binder.ConfigMeta.BossShouldDropRewardBoxAtFloor(floor))
                    {
                        progress.NumBossAdditionalDrops++;
                    }
                    progress.NumBossKills++;
                    num4--;
                    num3 -= App.Binder.ConfigMeta.PASSIVE_BOSS_KILL_FREQUENCY_SECONDS;
                    for (double i = App.Binder.ConfigMeta.PASSIVE_BOSS_KILL_FREQUENCY_SECONDS; i >= num5; i -= num5)
                    {
                        progress.NumMinionKills++;
                        if (num2 > 0)
                        {
                            progress.PassiveCoinReward += num6;
                            num2--;
                        }
                    }
                }
                floor++;
                progress.NumOverflowedMinionKills = 0;
                lastBossEncounterFailed = false;
            }
            double num9 = App.Binder.ConfigMeta.MinionCoinDropCurve(floor);
            while (num3 > num5)
            {
                if (num2 > 0)
                {
                    progress.PassiveCoinReward += num9;
                    num2--;
                }
                else if (stopOnceCoinDropsRunOut)
                {
                    break;
                }
                progress.NumMinionKills++;
                progress.NumOverflowedMinionKills++;
                num3 -= num5;
            }
            progress.PassiveCoinReward *= App.Binder.ConfigMeta.PASSIVE_COIN_EARNING_RATE_MULTIPLIER;
            if (progress.PassiveCoinReward < 1.0)
            {
                progress.PassiveCoinReward = 1.0;
            }
            progress.PassiveCoinReward = CharacterStatModifierUtil.ApplyCoinBonuses(this.ActiveCharacter, GameLogic.CharacterType.UNSPECIFIED, progress.PassiveCoinReward, true);
            progress.PassiveCoinReward = Math.Floor(progress.PassiveCoinReward);
            progress.NumFloorCompletions = floor - startFloor;
            return progress;
        }

        public int getPerkInstanceCount(PerkType perkType)
        {
            int num = 0;
            if (this.Tournaments.hasTournamentSelected())
            {
                num += this.Tournaments.SelectedTournament.Upgrades.getPerkInstanceCount(perkType);
            }
            else
            {
                num += this.Augmentations.getPerkInstanceCount(perkType);
            }
            num += this.Runestones.getPerkInstanceCount(perkType);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                num += instance.Character.FixedPerks.getPerkInstanceCount(instance.Level, perkType);
            }
            return num;
        }

        public void getPerkInstancesOfType(PerkType perkType, IBuffIconProvider iconProvider, ref List<KeyValuePair<PerkInstance, BuffSource>> outPerkInstances)
        {
            if (this.Tournaments.hasTournamentSelected())
            {
                this.Tournaments.SelectedTournament.Upgrades.getPerkInstancesOfType(perkType, iconProvider, ref outPerkInstances);
            }
            else
            {
                this.Augmentations.getPerkInstancesOfType(perkType, iconProvider, ref outPerkInstances);
            }
            this.Runestones.getPerkInstancesOfType(perkType, iconProvider, ref outPerkInstances);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                instance.Character.FixedPerks.getPerkInstancesOfType(instance.Level, perkType, iconProvider, ref outPerkInstances);
            }
        }

        public int getPossibleUpgradeAmountForCost(ItemType itemType, int level, int startingRank, double availableCash)
        {
            int num = startingRank;
            int num2 = 0;
            while (availableCash > 0.0)
            {
                double num3 = this.ActiveCharacter.getAdjustedItemUpgradeCost(itemType, level, num++);
                availableCash -= num3;
                if (availableCash < 0.0)
                {
                    return num2;
                }
                num2++;
            }
            return num2;
        }

        public ItemType getRandomRiggedItemType()
        {
            ItemType type = this.nextItemDropTypeIsForced();
            if (type != ItemType.UNSPECIFIED)
            {
                return type;
            }
            this.m_tempCandidateList.Clear();
            for (int i = 0; i < ConfigMeta.ACTIVE_ITEM_TYPES.Count; i++)
            {
                ItemType itemType = ConfigMeta.ACTIVE_ITEM_TYPES[i];
                if (this.itemTypeCanDrop(itemType, false))
                {
                    this.m_tempCandidateList.Add(itemType);
                }
            }
            if (this.m_tempCandidateList.Count == 0)
            {
                Debug.LogError("No random rigged item type candidates found, returning any random type..");
                return LangUtil.GetRandomValueFromList<ItemType>(ConfigMeta.ACTIVE_ITEM_TYPES);
            }
            return LangUtil.GetRandomValueFromList<ItemType>(this.m_tempCandidateList);
        }

        public float getRemainingBoostSeconds(BoostType boost)
        {
            if (!this.boostEntryExists(boost))
            {
                return 0f;
            }
            string str = boost.ToString();
            long num2 = Service.Binder.ServerTime.GameTime - this.BoostStartTimestamps[str];
            return Mathf.Clamp(ConfigBoosts.SHARED_DATA[boost].DurationSeconds - num2, 0f, float.MaxValue);
        }

        public int getRemainingMinionKillsUntilFloorCompletion(int floor, bool isTutorialDungeon, bool lastBossEncounterFailed)
        {
            int num = this.getRequiredMinionKillsForFloorCompletion(floor, isTutorialDungeon, lastBossEncounterFailed);
            int num2 = this.getMinionsKilledSinceLastRoomCompletion(false);
            return Mathf.Clamp(num - num2, 0, 0x7fffffff);
        }

        public int getRequiredMinionKillsForFloorCompletion(int floor, bool isTutorialDungeon, bool lastBossEncounterFailed)
        {
            if (isTutorialDungeon)
            {
                return ConfigTutorials.TUTORIAL_REQUIRED_MINION_KILLS_UNTIL_BOSS_SUMMON;
            }
            int num = App.Binder.ConfigMeta.BossSummonRequiredMinionKills(floor, lastBossEncounterFailed) + ((int) this.ActiveCharacter.getGenericModifierForPerkType(PerkType.CooldownBonusBosses));
            return Mathf.Clamp(num, 0, 0x7fffffff);
        }

        public double getResourceAmount(ResourceType rt)
        {
            return this.getResources((rt == 1) == 0)[ConfigMeta.RESOURCE_TYPE_TO_STRING_MAPPING[rt]];
        }

        public Dictionary<string, double> getResources([Optional, DefaultParameterValue(false)] bool ignoreTournaments)
        {
            if (!ignoreTournaments && this.Tournaments.hasTournamentSelected())
            {
                return this.Tournaments.SelectedTournament.Resources;
            }
            return this.Resources;
        }

        public int getRiggedItemLevel(ItemInstance ii)
        {
            if ((!this.hasRetired() && !ConfigApp.CHEAT_SKIP_TUTORIALS) && !this.Tournaments.hasTournamentSelected())
            {
                int num = this.getLastCompletedFloor(false) + 1;
                switch (ii.Item.Type)
                {
                    case ItemType.Armor:
                        switch (num)
                        {
                            case 0:
                                return 1;

                            case 1:
                                return 2;
                        }
                        break;

                    case ItemType.Cloak:
                        switch (num)
                        {
                            case 0:
                                return 1;

                            case 1:
                                return 2;

                            case 2:
                                return 4;
                        }
                        break;
                }
            }
            return ii.Level;
        }

        public float getSkillCooldownModifier(SkillType skillType)
        {
            float num = 0f;
            if (this.Tournaments.hasTournamentSelected())
            {
                num += this.Tournaments.SelectedTournament.Upgrades.getSkillCooldownModifier(skillType);
            }
            else
            {
                num += this.Augmentations.getSkillCooldownModifier(skillType);
            }
            num += this.Runestones.getSkillCooldownModifier(skillType);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                num += instance.Character.FixedPerks.getSkillCooldownModifier(instance.Level, skillType);
            }
            return num;
        }

        public float getSkillDamageModifier(SkillType skillType)
        {
            float num = 0f;
            if (this.Tournaments.hasTournamentSelected())
            {
                num += this.Tournaments.SelectedTournament.Upgrades.getSkillDamageModifier(skillType);
            }
            else
            {
                num += this.Augmentations.getSkillDamageModifier(skillType);
            }
            num += this.Runestones.getSkillDamageModifier(skillType);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                num += instance.Character.FixedPerks.getSkillDamageModifier(instance.Level, skillType);
            }
            return num;
        }

        public int getSkillExtraCharges(SkillType skillType)
        {
            int num = 0;
            if (this.Tournaments.hasTournamentSelected())
            {
                num += this.Tournaments.SelectedTournament.Upgrades.getSkillExtraCharges(skillType);
            }
            else
            {
                num += this.Augmentations.getSkillExtraCharges(skillType);
            }
            num += this.Runestones.getSkillExtraCharges(skillType);
            PetInstance instance = this.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.Character.FixedPerks != null))
            {
                num += instance.Character.FixedPerks.getSkillExtraCharges(instance.Level, skillType);
            }
            return num;
        }

        public double getStartingCoins()
        {
            return (double) Mathf.Floor(this.getGenericModifierForPerkType(PerkType.CoinBonusStart));
        }

        public int getStartingFloor()
        {
            int num = 1;
            return (num + ((int) this.getGenericModifierForPerkType(PerkType.StartingFloorBonus)));
        }

        public double getTotalTokensFromRetirement(out double tokensInRetirementTriggerChests, out double tokensFromEquipment, out double tokenRewardFloorMultiplier)
        {
            tokensInRetirementTriggerChests = 0.0;
            for (int i = 0; i < this.UnclaimedRewards.Count; i++)
            {
                Reward reward = this.UnclaimedRewards[i];
                if (ConfigMeta.IsRetirementChest(reward.ChestType) && (reward.ChestType == ChestType.RetirementTrigger))
                {
                    tokensInRetirementTriggerChests += reward.getTotalTokenAmount();
                }
            }
            tokensFromEquipment = this.ActiveCharacter.getTotalEquipmentTokenValue();
            tokenRewardFloorMultiplier = this.getActiveTokenRewardFloorMultiplier();
            return Math.Floor((tokensInRetirementTriggerChests + tokensFromEquipment) * tokenRewardFloorMultiplier);
        }

        public bool hasClaimedReward(string rewardId)
        {
            for (int i = 0; i < this.ClaimedRewardIds.Count; i++)
            {
                if (this.ClaimedRewardIds[i] == rewardId)
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasCompletedAllTutorialsInCategory(List<string> category)
        {
            for (int i = 0; i < category.Count; i++)
            {
                if (!this.hasCompletedTutorial(category[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public bool hasCompletedSideQuest(string id)
        {
            return this.CompletedSideQuests.Contains(id);
        }

        public bool hasCompletedTutorial(string id)
        {
            return (ConfigApp.CHEAT_SKIP_TUTORIALS || this.CompletedTutorials.Contains(id));
        }

        public bool hasEncounteredCharacterType(GameLogic.CharacterType characterType)
        {
            return ((characterType == GameLogic.CharacterType.UNSPECIFIED) || (this.ActiveCharacter.HeroStats.EncounteredCharacterTypes.Contains(characterType) || this.CumulativeRetiredHeroStats.EncounteredCharacterTypes.Contains(characterType)));
        }

        public bool hasEncounteredChest(ChestType chestType)
        {
            if (chestType == ChestType.NONE)
            {
                return true;
            }
            string item = chestType.ToString();
            return (this.ActiveCharacter.HeroStats.EncounteredChestTypes.Contains(item) || this.CumulativeRetiredHeroStats.EncounteredChestTypes.Contains(item));
        }

        public bool hasEncounteredItem(string itemId)
        {
            return (this.ActiveCharacter.HeroStats.EncounteredItemsIds.Contains(itemId) || this.CumulativeRetiredHeroStats.EncounteredItemsIds.Contains(itemId));
        }

        public bool hasEncounteredSpecialChest()
        {
            for (int i = 0; i < ConfigMeta.BOSS_CHESTS_SPECIAL.Length; i++)
            {
                if (this.hasEncounteredChest(ConfigMeta.BOSS_CHESTS_SPECIAL[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasExploredDungeon(string dungeonId)
        {
            if (this.DungeonExplored.ContainsKey(dungeonId))
            {
                return this.DungeonExplored[dungeonId];
            }
            return false;
        }

        public bool hasRetired()
        {
            return (this.CumulativeRetiredHeroStats.HeroesRetired > 0);
        }

        public bool hasSkillInvulnerability(SkillType skillType)
        {
            if (this.Tournaments.hasTournamentSelected())
            {
                if (this.Tournaments.SelectedTournament.Upgrades.hasSkillInvulnerability(skillType))
                {
                    return true;
                }
            }
            else if (this.Augmentations.hasSkillInvulnerability(skillType))
            {
                return true;
            }
            if (this.Runestones.hasSkillInvulnerability(skillType))
            {
                return true;
            }
            PetInstance instance = this.Pets.getSelectedPetInstance();
            return (((instance != null) && (instance.Character.FixedPerks != null)) && instance.Character.FixedPerks.hasSkillInvulnerability(instance.Level, skillType));
        }

        public bool hasUnlockedCharacter(string characterId)
        {
            return (this.getCharacterInstance(characterId) != null);
        }

        public bool hasUnlockedSkill(SkillType skillType)
        {
            if (skillType == SkillType.NONE)
            {
                return true;
            }
            for (int i = 0; i < this.UnlockedSkills.Count; i++)
            {
                if (((SkillType) this.UnlockedSkills[i]) == skillType)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isAdventurePanelUnlocked()
        {
            return (this.getFirstUnclaimedRetirementTriggerChest() != null);
        }

        public bool isAtMaxRank()
        {
            return (this.Rank >= App.Binder.ConfigMeta.XP_LEVEL_CAP);
        }

        public bool isChestEnabled(ChestType chestType)
        {
            if (ConfigMeta.IsEventBossChest(chestType))
            {
                return this.hasEncounteredChest(chestType);
            }
            return true;
        }

        public bool isClaimableReward(Reward reward)
        {
            return ((!ConfigMeta.IsBossChest(reward.ChestType) && !ConfigMeta.IsMysteryChest(reward.ChestType)) || ((reward.RewardSource == this.DungeonDropRewardSource) && (reward.RewardSourceId == this.DungeonDropRewardSourceId)));
        }

        public bool isHeroOrSkillPopupUnlocked()
        {
            return (ConfigApp.CHEAT_SKIP_TUTORIALS || ((this.Rank >= 2) || (this.getNumberOfUnclaimedLevelUpRewards() > 0)));
        }

        public bool isItemRarityAllowed(int rarity, int floor, bool isBossDrop)
        {
            if (rarity > ConfigMeta.ITEM_HIGHEST_RARITY)
            {
                return false;
            }
            if (((rarity >= 2) && !this.canRetire()) && !this.Tournaments.hasTournamentSelected())
            {
                return false;
            }
            if (isBossDrop)
            {
                int num = App.Binder.ConfigMeta.ITEM_RARITY_BOSS_DROP_MIN_FLOOR[rarity];
                if (floor < num)
                {
                    return false;
                }
                int num2 = App.Binder.ConfigMeta.ITEM_RARITY_BOSS_DROP_MAX_FLOOR[rarity];
                if (floor > num2)
                {
                    return false;
                }
            }
            return true;
        }

        public bool isPetPopupUnlocked()
        {
            return (this.Pets.getNumPetsOwned() > 0);
        }

        public double itemCumulativeUpgradeCost(ItemType itemType, int level, int rank, double numRankUps)
        {
            double d = 0.0;
            int num2 = (int) Math.Floor(numRankUps);
            int num3 = rank + num2;
            for (int i = rank + 1; i <= num3; i++)
            {
                d += this.ActiveCharacter.getAdjustedItemUpgradeCost(itemType, level, i);
            }
            double num5 = numRankUps - num2;
            if (num5 > 0.0)
            {
                d += this.ActiveCharacter.getAdjustedItemUpgradeCost(itemType, level, num3 + 1) * num5;
            }
            return Math.Floor(d);
        }

        public bool itemTypeCanDrop(ItemType itemType, [Optional, DefaultParameterValue(false)] bool bypassRollHistory)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            DungeonModifierType nONE = DungeonModifierType.NONE;
            switch (itemType)
            {
                case ItemType.Weapon:
                    nONE = DungeonModifierType.NoItemTypeDropWeapon;
                    break;

                case ItemType.Armor:
                    nONE = DungeonModifierType.NoItemTypeDropArmor;
                    break;

                case ItemType.Cloak:
                    nONE = DungeonModifierType.NoItemTypeDropCloak;
                    break;
            }
            if ((activeDungeon != null) && activeDungeon.hasDungeonModifier(nONE))
            {
                return false;
            }
            if ((!bypassRollHistory && (this.ItemTypeRollHistory.Count > 0)) && (((ItemType) this.ItemTypeRollHistory[this.ItemTypeRollHistory.Count - 1]) == itemType))
            {
                return false;
            }
            return true;
        }

        public bool itemTypeInRollHistory(ItemType itemType)
        {
            foreach (ItemType type in this.ItemTypeRollHistory)
            {
                if (type == itemType)
                {
                    return true;
                }
            }
            return false;
        }

        public bool mysteryChestCanContainSpecialOffer()
        {
            if (!Service.Binder.AdsSystem.adReady())
            {
                return false;
            }
            if (this.Tournaments.hasTournamentSelected())
            {
                if (this.Tournaments.SelectedTournament.DailyAdCountMystery >= App.Binder.ConfigMeta.DAILY_ADS_LIMIT_TOURNAMENT_MYSTERY)
                {
                    return false;
                }
            }
            else if (this.DailyAdCountMystery >= App.Binder.ConfigMeta.DAILY_ADS_LIMIT_ADVENTURE_MYSTERY)
            {
                return false;
            }
            return true;
        }

        public bool mysteryChestCanDrop()
        {
            if (this.getNumberOfUnclaimedMysteryChests() > 0)
            {
                return false;
            }
            if (GameLogic.Binder.FrenzySystem.isFrenzyActive())
            {
                return false;
            }
            if (this.getCurrentFloor(false) < App.Binder.ConfigMeta.MYSTERY_CHEST_DROP_UNLOCK_FLOOR)
            {
                return false;
            }
            long num = Service.Binder.ServerTime.GameTime - this.LastMysteryChestDropTimestamp;
            return (num >= App.Binder.ConfigMeta.MYSTERY_CHEST_COOLDOWN_SECONDS);
        }

        public ItemType nextItemDropTypeIsForced()
        {
            if (this.ItemTypeRollHistory.Count >= ConfigMeta.ITEM_TYPE_ROLL_HISTORY)
            {
                for (int i = 0; i < ConfigMeta.ACTIVE_ITEM_TYPES.Count; i++)
                {
                    ItemType itemType = ConfigMeta.ACTIVE_ITEM_TYPES[i];
                    if (!this.itemTypeInRollHistory(itemType))
                    {
                        return itemType;
                    }
                }
            }
            return ItemType.UNSPECIFIED;
        }

        public void onCharacterOwnedPreKill(CharacterInstance character)
        {
            if (character == this.ActiveCharacter)
            {
                this.Preferences.AutoSummonBosses = false;
            }
        }

        public bool passiveProgressUnlocked()
        {
            return (this.getHighestFloorReached() >= 3);
        }

        public void postDeserializeInitialization()
        {
            if (this.hasRetired() && (this.getFirstUnclaimedRetirementTriggerChest() == null))
            {
                Reward item = new Reward();
                item.ChestType = ChestType.RetirementTrigger;
                this.UnclaimedRewards.Add(item);
            }
            for (int i = this.CharacterInstances.Count - 1; i >= 0; i--)
            {
                if (!GameLogic.Binder.CharacterResources.containsResource(this.CharacterInstances[i].CharacterId))
                {
                    Debug.LogWarning("Cleaning up non-existing character from player: " + this.CharacterInstances[i].CharacterId);
                    this.CharacterInstances.RemoveAt(i);
                }
            }
            IEnumerator enumerator = Enum.GetValues(typeof(ResourceType)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ResourceType current = (ResourceType) ((int) enumerator.Current);
                    if (!this.Resources.ContainsKey(current.ToString()))
                    {
                        this.Resources.Add(current.ToString(), 0.0);
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
            for (int j = 0; j < this.UnclaimedRewards.Count; j++)
            {
                this.UnclaimedRewards[j].postDeserializeInitialization();
            }
            for (int k = 0; k < this.CharacterInstances.Count; k++)
            {
                this.CharacterInstances[k].postDeserializeInitialization();
            }
            this.Achievements.Player = this;
            this.Achievements.postDeserializeInitialization();
            this.Vendor.Player = this;
            this.Vendor.postDeserializeInitialization();
            this.Augmentations.Player = this;
            this.Augmentations.postDeserializeInitialization();
            this.Pets.Player = this;
            this.Pets.postDeserializeInitialization();
            this.Missions.Player = this;
            this.Missions.postDeserializeInitialization();
            this.PromotionEvents.Player = this;
            this.PromotionEvents.postDeserializeInitialization();
            this.Tournaments.Player = this;
            this.Tournaments.postDeserializeInitialization();
            this.Runestones.Player = this;
            this.Runestones.postDeserializeInitialization();
            if (string.IsNullOrEmpty(this.SocialData.Name))
            {
                this.SocialData.Name = _.L(ConfigLoca.HERO_KNIGHT, null, false);
            }
            this.CumulativeRetiredHeroStats.HighestTokenMultiplier = Math.Max(this.CumulativeRetiredHeroStats.HighestTokenMultiplier, 1.0);
            for (int m = 0; m < this.CharacterInstances.Count; m++)
            {
                CharacterInstance instance = this.CharacterInstances[m];
                for (int num5 = 0; num5 < instance.ItemSlots.Count; num5++)
                {
                    if (instance.ItemSlots[num5].ItemInstance != null)
                    {
                        instance.ItemSlots[num5].ItemInstance.enforcePerkLegality(this);
                    }
                }
                for (int num6 = 0; num6 < instance.Inventory.ItemInstances.Count; num6++)
                {
                    instance.Inventory.ItemInstances[num6].enforcePerkLegality(this);
                }
            }
            for (int n = 0; n < this.UnclaimedRewards.Count; n++)
            {
                for (int num8 = 0; num8 < this.UnclaimedRewards[n].ItemDrops.Count; num8++)
                {
                    this.UnclaimedRewards[n].ItemDrops[num8].enforcePerkLegality(this);
                }
            }
            if (this.RetirementRandomSeed == 0)
            {
                this.rerollRetirementRandomSeed();
            }
            else
            {
                this.setRetirementRandomSeed(this.RetirementRandomSeed);
            }
            for (int num9 = this.UnclaimedRewards.Count - 1; num9 >= 0; num9--)
            {
                if ((this.UnclaimedRewards[num9].TournamentUpgradeReward != null) && (this.UnclaimedRewards[num9].TournamentUpgradeReward.countNumSelected() > 0))
                {
                    CmdConsumeReward.ExecuteStatic(this, this.UnclaimedRewards[num9], true, string.Empty);
                }
            }
            for (int num10 = this.UnclaimedRewards.Count - 1; num10 >= 0; num10--)
            {
                Reward reward = this.UnclaimedRewards[num10];
                if ((reward.RewardSource == RewardSourceType.TournamentDungeonDrop) && !this.Tournaments.ActiveInstances.ContainsKey(reward.RewardSourceId))
                {
                    this.UnclaimedRewards.RemoveAt(num10);
                }
            }
            this.Notifiers = new PlayerNotifiers(this);
            this.m_postDeserializeInitComplete = true;
        }

        public bool potionsUnlocked()
        {
            return ((this.hasRetired() || (this.getCurrentFloor(false) >= App.Binder.ConfigMeta.REVIVE_UNLOCK_FLOOR)) || this.Tournaments.hasTournamentSelected());
        }

        public void rerollRetirementRandomSeed()
        {
            this.setRetirementRandomSeed(UnityEngine.Random.Range(-2147483648, 0x7fffffff));
        }

        public bool reviveBundlesCanDrop()
        {
            return (this.ActiveCharacter.Inventory.RevivePotions < App.Binder.ConfigMeta.THRESHHOLD_DONTOFFER_REVIVEPOTION);
        }

        public void setLastBossEncounterFailed(bool failed, [Optional, DefaultParameterValue(false)] bool ignoreTournaments)
        {
            if (!ignoreTournaments && this.Tournaments.hasTournamentSelected())
            {
                this.Tournaments.SelectedTournament.LastBossEncounterFailed = failed;
            }
            else
            {
                this.LastBossEncounterFailed = failed;
            }
        }

        public void setLastCompletedFloor(int floor, [Optional, DefaultParameterValue(false)] bool ignoreTournaments)
        {
            if (!ignoreTournaments && this.Tournaments.hasTournamentSelected())
            {
                this.Tournaments.SelectedTournament.LastCompletedFloor = floor;
            }
            else
            {
                this.LastCompletedFloor = floor;
            }
        }

        public void setMinionsKilledSinceLastRoomCompletion(int value, [Optional, DefaultParameterValue(false)] bool ignoreTournaments)
        {
            if (!ignoreTournaments && this.Tournaments.hasTournamentSelected())
            {
                this.Tournaments.SelectedTournament.MinionsKilledSinceLastRoomCompletion = value;
            }
            else
            {
                this.MinionsKilledSinceLastRoomCompletion = value;
            }
        }

        public void setResourceAmount(ResourceType rt, double amount)
        {
            string key = ConfigMeta.RESOURCE_TYPE_TO_STRING_MAPPING[rt];
            Dictionary<string, double> dictionary = this.getResources(rt != ResourceType.Coin);
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = amount;
            }
            else
            {
                dictionary.Add(key, amount);
            }
        }

        private void setRetirementRandomSeed(int seed)
        {
            this.RetirementRandomSeed = seed;
            this.RetirementHash.seed = (uint) seed;
        }

        public void setStackExhausted(string shopEntryId)
        {
            if (this.ShopEntryStackExhaustedCount.ContainsKey(shopEntryId))
            {
                Dictionary<string, int> dictionary;
                string str;
                int num = dictionary[str];
                (dictionary = this.ShopEntryStackExhaustedCount)[str = shopEntryId] = num + 1;
            }
            else
            {
                this.ShopEntryStackExhaustedCount.Add(shopEntryId, 1);
            }
        }

        public bool shopUnlocked()
        {
            if (ConfigApp.CHEAT_SKIP_TUTORIALS)
            {
                return true;
            }
            if (!this.hasCompletedTutorial("TUT004A"))
            {
                return false;
            }
            return (this.hasRetired() || ((this.getLastCompletedFloor(true) + 1) >= App.Binder.ConfigMeta.VENDOR_AND_SHOP_UNLOCK_FLOOR));
        }

        public bool tokenBundlesCanDrop()
        {
            return this.hasRetired();
        }

        public void updateItemRollHistory(ItemInstance itemInstance)
        {
            this.LastRolledItemId = itemInstance.Item.Id;
            this.ItemTypeRollHistory.Add(itemInstance.Item.Type);
            while (this.ItemTypeRollHistory.Count > ConfigMeta.ITEM_TYPE_ROLL_HISTORY)
            {
                this.ItemTypeRollHistory.RemoveAt(0);
            }
        }

        public bool xpBundlesCanDrop()
        {
            return !this.isAtMaxRank();
        }

        public string _id
        {
            [CompilerGenerated]
            get
            {
                return this.<_id>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<_id>k__BackingField = value;
            }
        }

        [JsonIgnore]
        public CharacterInstance ActiveCharacter
        {
            get
            {
                return this.CharacterInstances[this.ActiveCharacterIndex];
            }
        }

        [JsonIgnore]
        public RewardSourceType DungeonDropRewardSource
        {
            get
            {
                return (!this.Tournaments.hasTournamentSelected() ? RewardSourceType.NONE : RewardSourceType.TournamentDungeonDrop);
            }
        }

        [JsonIgnore]
        public string DungeonDropRewardSourceId
        {
            get
            {
                return (!this.Tournaments.hasTournamentSelected() ? null : this.Tournaments.SelectedTournament.TournamentId);
            }
        }

        public class BossTrainData
        {
            public int ActivationFloor;
            public bool Active;
            public int ChargesRemaining;
            public int ChargesTotal;
            public int NumBossFloorsCompleted;
            public int PendingJumpToFloorWithBoss;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PassiveProgress
        {
            public int NumBossAdditionalDrops;
            public int NumBossKills;
            public int NumFloorCompletions;
            public int NumMinionKills;
            public int NumMinionKillsOnBossProgressStop;
            public int NumOverflowedMinionKills;
            public double PassiveCoinReward;
            public double PassiveTokenReward;
        }
    }
}

