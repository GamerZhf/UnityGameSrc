namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class CheatPopupContent : MenuContent
    {
        public Text BuildInfoServer;
        public Text BuildInfoVersion;
        public GameObject DemoButtonRoot;
        public Text FacebookId;
        public Text FacebookName;
        public Text FpsCounterButtonText;
        public const int LOCA_TAB_INDEX = 3;
        public GameObject LocaTab;
        private Dictionary<int, GameObject[]> m_tabGameObjects;
        public Text MasterRemoteContentText;
        public Text NetworkUsage;
        public Text NotificationsCheatButtonText;
        public const int PROGRESS_TAB_INDEX = 1;
        public GameObject ProgressTab;
        public const int QA_TAB_INDEX = 2;
        public GameObject QaTab;
        public const int REWARDS_TAB_INDEX = 0;
        public GameObject RewarsTab_Content1;
        public GameObject RewarsTab_Content2;

        private void gainItem(Player player, string itemId)
        {
            CharacterInstance activeCharacter = player.ActiveCharacter;
            if (!activeCharacter.ownsItem(itemId))
            {
                int num = player.getLastCompletedFloor(false) + 1;
                ItemInstance itemInstance = new ItemInstance(itemId, 1, num * App.Binder.ConfigMeta.LEVEL_TO_FLOOR_MULTIPLIER, player.getLastCompletedFloor(false), player);
                CmdGainItemInstance.ExecuteStatic(activeCharacter, itemInstance, string.Empty);
                CmdInspectItem.ExecuteStatic(itemInstance);
                CmdUnlockItem.ExecuteStatic(itemInstance);
            }
        }

        public override string getTitleForTab(int idx)
        {
            switch (idx)
            {
                case 0:
                    return "REWARDS";

                case 1:
                    return "PROGRESS";

                case 2:
                    return "QA";

                case 3:
                    return "LOCA";
            }
            return null;
        }

        public void onAllItemsButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            List<string> list = GameLogic.Binder.ItemResources.getOrderedItemIdList();
            for (int i = 0; i < list.Count; i++)
            {
                this.gainItem(player, list[i]);
            }
        }

        public void onAllSkillsButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            player.UnlockedSkills.Clear();
            for (int i = 0; i < ConfigSkills.ALL_HERO_SKILLS.Count; i++)
            {
                CmdUnlockSkill.ExecuteStatic(player, ConfigSkills.ALL_HERO_SKILLS[i], true);
                CmdInspectSkill.ExecuteStatic(activeCharacter.getSkillInstance(ConfigSkills.ALL_HERO_SKILLS[i]));
            }
            player.Runestones.RunestoneInstances.Clear();
            for (int j = 0; j < ConfigRunestones.RUNESTONES.Length; j++)
            {
                string id = ConfigRunestones.RUNESTONES[j].Id;
                CmdGainRunestone.ExecuteStatic(player, id, true);
            }
            for (int k = 0; k < player.Runestones.RunestoneInstances.Count; k++)
            {
                player.Runestones.RunestoneInstances[k].InspectedByPlayer = true;
            }
        }

        public void onAllUpgradesButtonClicked()
        {
            GameLogic.Binder.GameState.Player.Augmentations.cheatAddAugmentations(true, 0x7fffffff);
        }

        public void onArabicButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.Arabic);
        }

        public void onAscendFloorButtonClicked()
        {
            this.progressToFloor(App.Binder.ConfigMeta.RETIREMENT_MIN_FLOOR + 1, true);
        }

        protected override void onAwake()
        {
            Dictionary<int, GameObject[]> dictionary = new Dictionary<int, GameObject[]>();
            GameObject[] objArray1 = new GameObject[] { this.RewarsTab_Content1, this.RewarsTab_Content2 };
            dictionary.Add(0, objArray1);
            GameObject[] objArray2 = new GameObject[] { this.ProgressTab };
            dictionary.Add(1, objArray2);
            GameObject[] objArray3 = new GameObject[] { this.QaTab };
            dictionary.Add(2, objArray3);
            GameObject[] objArray4 = new GameObject[] { this.LocaTab };
            dictionary.Add(3, objArray4);
            this.m_tabGameObjects = dictionary;
        }

        public void onBossPotionButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                string str = "BossBundleSmall";
                Reward reward2 = new Reward();
                reward2.ShopEntryId = str;
                reward2.BossPotions = (int) App.Binder.ConfigMeta.VENDOR_BOSS_BUNDLES[str];
                Reward reward = reward2;
                this.transitionToRewardCeremony(reward, ConfigUi.CeremonyEntries.SHOP_PURCHASE);
            }
        }

        public void onChineseSimplifiedButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.ChineseSimplified);
        }

        public void onChineseTraditionalButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.ChineseTraditional);
        }

        protected override void onCleanup()
        {
        }

        public void onCoinAdChestButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                Reward reward = new Reward();
                reward.ChestType = ChestType.MysterySpecialOffer;
                double item = App.Binder.ConfigMeta.CoinBundleSize(player, "CoinBundleSmall", 0.0);
                reward.CoinDrops.Add(item);
                CmdConsumeReward.ExecuteStatic(GameLogic.Binder.GameState.Player, reward, true, string.Empty);
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(GameLogic.Binder.GameState.Player);
                RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.AD_REWARD.Title, null, false));
                parameters2.Description = _.L(ConfigUi.CeremonyEntries.AD_REWARD.Description, null, false);
                parameters2.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.AD_REWARD.ChestOpenAtStart;
                parameters2.SingleReward = reward;
                RewardCeremonyMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter);
            }
        }

        public void onCoinsButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            ItemInstance ii = player.ActiveCharacter.getHighestLevelItemOwned();
            if (ii != null)
            {
                double num = player.ActiveCharacter.getAdjustedItemUpgradeCost(ii.Item.Type, player.getRiggedItemLevel(ii), ii.Rank + 1);
                CmdGainResources.ExecuteStatic(player, ResourceType.Coin, num * 1000.0, false, string.Empty, null);
            }
            else
            {
                CmdGainResources.ExecuteStatic(player, ResourceType.Coin, 9E+100, false, string.Empty, null);
            }
        }

        public void onCompleteEverythingButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            player.Rank = App.Binder.ConfigMeta.XP_LEVEL_CAP;
            player.setLastCompletedFloor(ConfigDungeons.MAX_DUNGEON_COUNT - 1, false);
            player.setResourceAmount(ResourceType.Coin, 1110000000000);
            player.setResourceAmount(ResourceType.Diamond, 999999999.0);
            player.setResourceAmount(ResourceType.Token, 19300000000);
            player.setResourceAmount(ResourceType.Crown, 1600000000000);
            player.UnclaimedPassiveRewardableSeconds = 0L;
            player.LastPassiveRewardClaimTimestamp = Service.Binder.ServerTime.GameTime;
            player.setMinionsKilledSinceLastRoomCompletion(0, true);
            player.setLastBossEncounterFailed(false, true);
            player.LastMysteryChestDropTimestamp = 0L;
            player.NumPendingRankUpCeremonies = 0;
            player.UnclaimedRewards.Clear();
            player.ItemTypeRollHistory.Clear();
            player.LastRolledItemId = string.Empty;
            player.Vendor.Inventory.Clear();
            player.UnclaimedLeaderboardRewards.Clear();
            player.CompletedTutorials.Clear();
            ConfigTutorials.CheatCompleteAllFtueTutorials(player);
            ConfigTutorials.CheatCompleteAllContextTutorials(player);
            Player player2 = JsonUtils.Deserialize<Player>(ResourceUtil.LoadSafe<TextAsset>("Players/humanPlayer1", false).text, true);
            CharacterInstance activeCharacter = player.ActiveCharacter;
            activeCharacter.copyFrom(player2.ActiveCharacter);
            activeCharacter.postDeserializeInitialization();
            player.UnlockedSkills.Clear();
            for (int i = 0; i < ConfigSkills.ALL_HERO_SKILLS.Count; i++)
            {
                CmdUnlockSkill.ExecuteStatic(player, ConfigSkills.ALL_HERO_SKILLS[i], true);
                CmdInspectSkill.ExecuteStatic(activeCharacter.getSkillInstance(ConfigSkills.ALL_HERO_SKILLS[i]));
            }
            CmdAssignActiveSkill.ExecuteStatic(activeCharacter, SkillType.Implosion);
            CmdAssignActiveSkill.ExecuteStatic(activeCharacter, SkillType.Omnislash);
            CmdAssignActiveSkill.ExecuteStatic(activeCharacter, SkillType.Slam);
            player.Achievements.Claimed.Clear();
            player.Achievements.Notified.Clear();
            for (int j = 0; j < ConfigAchievements.ACHIEVEMENT_IDS.Count; j++)
            {
                CmdClaimAchievement.ExecuteStatic(player, ConfigAchievements.ACHIEVEMENT_IDS[j], ConfigAchievements.MAX_TIER, true);
            }
            player.CumulativeRetiredHeroStats = new HeroStats();
            player.CumulativeRetiredHeroStats.FloorsCompleted = player.getLastCompletedFloor(false);
            player.CumulativeRetiredHeroStats.MonstersKilled = 1202020.0;
            player.CumulativeRetiredHeroStats.CoinsEarned = 9.99E+101;
            player.CumulativeRetiredHeroStats.ItemsUnlocked = 32343025.0;
            player.CumulativeRetiredHeroStats.ItemUpgrades = 2343240.0;
            player.CumulativeRetiredHeroStats.MultikilledMonsters = 4001200.0;
            player.CumulativeRetiredHeroStats.SilverChestsOpened = 0x41a8ac;
            player.CumulativeRetiredHeroStats.GoldChestsOpened = 0x53f52;
            player.CumulativeRetiredHeroStats.RankUps = player.Rank;
            player.CumulativeRetiredHeroStats.HeroesRetired = 0x3e7;
            player.CumulativeRetiredHeroStats.HighestFloor = player.getLastCompletedFloor(false) + 1;
            player.CumulativeRetiredHeroStats.EncounteredChestTypes.Clear();
            foreach (KeyValuePair<ChestType, int> pair in App.Binder.ConfigMeta.CHEST_UNLOCK_FLOOR)
            {
                player.CumulativeRetiredHeroStats.EncounteredChestTypes.Add(pair.Key.ToString());
            }
            player.CumulativeRetiredHeroStats.EncounteredCharacterTypes.Clear();
            for (int k = 0; k < ConfigGameplay.ALL_CHARACTER_TYPES.Count; k++)
            {
                GameLogic.CharacterType type2 = ConfigGameplay.ALL_CHARACTER_TYPES[k];
                player.CumulativeRetiredHeroStats.EncounteredCharacterTypes.Add(type2);
            }
            player.BestedLeaderboardUserIds.Clear();
            for (int m = 0; m < ConfigLeaderboard.DUMMY_PLAYERS.Length; m++)
            {
                player.BestedLeaderboardUserIds.Add(ConfigLeaderboard.DUMMY_PLAYERS[m].UserId, 0);
            }
            for (int n = activeCharacter.ItemSlots.Count - 1; n >= 0; n--)
            {
                activeCharacter.ItemSlots[n].ItemInstance = null;
            }
            for (int num6 = activeCharacter.Inventory.ItemInstances.Count - 1; num6 >= 0; num6--)
            {
                activeCharacter.Inventory.ItemInstances[num6] = null;
            }
            List<string> list = GameLogic.Binder.ItemResources.getOrderedItemIdList();
            for (int num7 = 0; num7 < list.Count; num7++)
            {
                ItemInstance itemInstance = new ItemInstance(list[num7], 1, 160, player.getLastCompletedFloor(false), player);
                CmdGainItemInstance.ExecuteStatic(activeCharacter, itemInstance, string.Empty);
                CmdInspectItem.ExecuteStatic(itemInstance);
                CmdUnlockItem.ExecuteStatic(itemInstance);
            }
            CmdEquipItem.ExecuteStatic(activeCharacter, activeCharacter.getFirstItemInstanceFromInventory(ItemType.Weapon));
            CmdEquipItem.ExecuteStatic(activeCharacter, activeCharacter.getFirstItemInstanceFromInventory(ItemType.Armor));
            CmdEquipItem.ExecuteStatic(activeCharacter, activeCharacter.getFirstItemInstanceFromInventory(ItemType.Cloak));
            for (int num8 = 0; num8 < player.Runestones.RunestoneInstances.Count; num8++)
            {
                player.Runestones.RunestoneInstances[num8].InspectedByPlayer = true;
            }
            player.Augmentations.cheatAddAugmentations(true, 0x7fffffff);
            Reward reward2 = new Reward();
            reward2.ChestType = ChestType.RetirementTrigger;
            Reward item = reward2;
            item.addResourceDrop(ResourceType.Token, 500.0);
            player.UnclaimedRewards.Add(item);
            if (ConfigMeta.BOSS_ADDITIONAL_DROPS_ENABLED)
            {
                ChestType[] array = new ChestType[] { ChestType.RewardBoxCommon };
                for (int num9 = 0; num9 < 3; num9++)
                {
                    item = new Reward();
                    ChestType randomValueFromArray = LangUtil.GetRandomValueFromArray<ChestType>(array);
                    item.ChestType = randomValueFromArray;
                    CmdRollChestLootTable.ExecuteStatic(randomValueFromArray, player, true, ref item, null);
                    player.UnclaimedRewards.Add(item);
                }
            }
            CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
            App.Binder.AppContext.hardReset(null);
        }

        public void onCompleteMissionsClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            player.HasUnlockedMissions = true;
            for (int i = 0; i < player.Missions.Instances.Count; i++)
            {
                CmdCheatCompleteMission.ExecuteStatic(player, player.Missions.Instances[i], false);
            }
            foreach (KeyValuePair<string, PromotionEventInstance> pair in player.PromotionEvents.Instances)
            {
                PromotionEventInstance instance = pair.Value;
                for (int j = 0; j < instance.Missions.Instances.Count; j++)
                {
                    MissionInstance mission = instance.Missions.Instances[j];
                    CmdCheatCompleteMission.ExecuteStatic(player, mission, true);
                }
            }
        }

        public void onDemoButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            player.UnclaimedPassiveRewardableSeconds = 0L;
            player.LastPassiveRewardClaimTimestamp = Service.Binder.ServerTime.GameTime;
            player.setMinionsKilledSinceLastRoomCompletion(0, true);
            player.setLastBossEncounterFailed(false, true);
            player.LastMysteryChestDropTimestamp = 0L;
            player.NumPendingRankUpCeremonies = 0;
            player.UnclaimedRewards.Clear();
            player.ItemTypeRollHistory.Clear();
            player.LastRolledItemId = string.Empty;
            player.Vendor.Inventory.Clear();
            player.UnclaimedLeaderboardRewards.Clear();
            player.HasPurchasedStarterBundle = false;
            player.HasOpenedStarterBundleOfferFromTaskPanel = false;
            Player player2 = JsonUtils.Deserialize<Player>(ResourceUtil.LoadSafe<TextAsset>("Players/humanPlayer1", false).text, true);
            CharacterInstance activeCharacter = player.ActiveCharacter;
            activeCharacter.copyFrom(player2.ActiveCharacter);
            activeCharacter.postDeserializeInitialization();
            int floor = 0x29;
            player.Rank = 9;
            player.setLastCompletedFloor(floor, false);
            player.setResourceAmount(ResourceType.Coin, 1250000.0);
            player.setResourceAmount(ResourceType.Diamond, 5000.0);
            player.setResourceAmount(ResourceType.Token, 5000.0);
            player.setResourceAmount(ResourceType.Dust, 5000.0);
            player.CompletedTutorials.Clear();
            ConfigTutorials.CheatCompleteAllFtueTutorials(player);
            ConfigTutorials.CheatCompleteAllContextTutorials(player);
            player.UnlockedSkills.Clear();
            for (int i = 0; i < ConfigSkills.ALL_HERO_SKILLS.Count; i++)
            {
                CmdUnlockSkill.ExecuteStatic(player, ConfigSkills.ALL_HERO_SKILLS[i], true);
                CmdInspectSkill.ExecuteStatic(activeCharacter.getSkillInstance(ConfigSkills.ALL_HERO_SKILLS[i]));
            }
            CmdAssignActiveSkill.ExecuteStatic(activeCharacter, SkillType.Whirlwind);
            CmdAssignActiveSkill.ExecuteStatic(activeCharacter, SkillType.Omnislash);
            CmdAssignActiveSkill.ExecuteStatic(activeCharacter, SkillType.Clone);
            player.Achievements.Claimed.Clear();
            player.Achievements.Notified.Clear();
            for (int j = 0; j < ConfigAchievements.ACHIEVEMENT_IDS.Count; j++)
            {
                CmdClaimAchievement.ExecuteStatic(player, ConfigAchievements.ACHIEVEMENT_IDS[j], UnityEngine.Random.Range(1, ConfigAchievements.MAX_TIER + 1), true);
            }
            player.CumulativeRetiredHeroStats = new HeroStats();
            player.CumulativeRetiredHeroStats.FloorsCompleted = player.getLastCompletedFloor(false);
            player.CumulativeRetiredHeroStats.MonstersKilled = 1020.0;
            player.CumulativeRetiredHeroStats.CoinsEarned = 12500000.0;
            player.CumulativeRetiredHeroStats.TokensEarned = 64.0;
            player.CumulativeRetiredHeroStats.ItemsUnlocked = 34.0;
            player.CumulativeRetiredHeroStats.ItemUpgrades = 40.0;
            player.CumulativeRetiredHeroStats.KnightUpgrades = 32.0;
            player.CumulativeRetiredHeroStats.MultikilledMonsters = 40.0;
            player.CumulativeRetiredHeroStats.SilverChestsOpened = 320;
            player.CumulativeRetiredHeroStats.GoldChestsOpened = 40;
            player.CumulativeRetiredHeroStats.RankUps = player.Rank;
            player.CumulativeRetiredHeroStats.HeroesRetired = 4;
            player.CumulativeRetiredHeroStats.HighestFloor = player.getLastCompletedFloor(false) + 1;
            player.CumulativeRetiredHeroStats.EncounteredChestTypes.Clear();
            foreach (KeyValuePair<ChestType, int> pair in App.Binder.ConfigMeta.CHEST_UNLOCK_FLOOR)
            {
                player.CumulativeRetiredHeroStats.EncounteredChestTypes.Add(pair.Key.ToString());
            }
            player.CumulativeRetiredHeroStats.EncounteredCharacterTypes.Clear();
            for (int k = 0; k < ConfigGameplay.ALL_CHARACTER_TYPES.Count; k++)
            {
                GameLogic.CharacterType type2 = ConfigGameplay.ALL_CHARACTER_TYPES[k];
                player.CumulativeRetiredHeroStats.EncounteredCharacterTypes.Add(type2);
            }
            player.BestedLeaderboardUserIds.Clear();
            for (int m = 0; m < ConfigLeaderboard.DUMMY_PLAYERS.Length; m++)
            {
                player.BestedLeaderboardUserIds.Add(ConfigLeaderboard.DUMMY_PLAYERS[m].UserId, 0);
            }
            for (int n = activeCharacter.ItemSlots.Count - 1; n >= 0; n--)
            {
                activeCharacter.ItemSlots[n].ItemInstance = null;
            }
            for (int num7 = activeCharacter.Inventory.ItemInstances.Count - 1; num7 >= 0; num7--)
            {
                activeCharacter.Inventory.ItemInstances[num7] = null;
            }
            List<string> list2 = new List<string>();
            list2.Add("Weapon006");
            list2.Add("Armor015");
            list2.Add("Cloak015");
            list2.Add("Weapon019");
            list2.Add("Weapon018");
            list2.Add("Armor012");
            list2.Add("Cloak002");
            List<string> list = list2;
            for (int num8 = 0; num8 < list.Count; num8++)
            {
                ItemInstance itemInstance = new ItemInstance(list[num8], 1, 110, player.getLastCompletedFloor(false), player);
                CmdGainItemInstance.ExecuteStatic(activeCharacter, itemInstance, string.Empty);
                CmdInspectItem.ExecuteStatic(itemInstance);
                CmdUnlockItem.ExecuteStatic(itemInstance);
            }
            CmdEquipItem.ExecuteStatic(activeCharacter, activeCharacter.getFirstItemInstanceFromInventory(ItemType.Weapon));
            CmdEquipItem.ExecuteStatic(activeCharacter, activeCharacter.getFirstItemInstanceFromInventory(ItemType.Armor));
            CmdEquipItem.ExecuteStatic(activeCharacter, activeCharacter.getFirstItemInstanceFromInventory(ItemType.Cloak));
            player.Augmentations.cheatAddAugmentations(true, 10);
            Reward reward2 = new Reward();
            reward2.ChestType = ChestType.RetirementTrigger;
            Reward item = reward2;
            item.addResourceDrop(ResourceType.Token, 500.0);
            player.UnclaimedRewards.Add(item);
            ChestType[] array = new ChestType[] { ChestType.RewardBoxCommon };
            for (int num9 = 0; num9 < 3; num9++)
            {
                item = new Reward();
                ChestType randomValueFromArray = LangUtil.GetRandomValueFromArray<ChestType>(array);
                item.ChestType = randomValueFromArray;
                CmdRollChestLootTable.ExecuteStatic(randomValueFromArray, player, false, ref item, null);
                player.UnclaimedRewards.Add(item);
            }
            CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
            App.Binder.AppContext.hardReset(null);
        }

        public void onDetachFromSocial()
        {
            Service.Binder.TaskManager.StartTask(Service.Binder.PlayerService.DetachGameState(), null);
            TechPopupContent.InputParameters parameters2 = new TechPopupContent.InputParameters();
            parameters2.Title = "Detached";
            parameters2.Message = "Client is now detached. KILL and then restart the app.";
            parameters2.ButtonText = "Got it";
            TechPopupContent.InputParameters parameter = parameters2;
            PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.MessagePopupMenu, MenuContentType.TechPopupContent, parameter);
        }

        public void onDeviceHighQualityButtonClicked()
        {
            CmdCheatDeviceQuality.ExecuteStatic(DeviceQualityType.High);
        }

        public void onDeviceLowQualityButtonClicked()
        {
            CmdCheatDeviceQuality.ExecuteStatic(DeviceQualityType.Low);
        }

        public void onDeviceMediumQualityButtonClicked()
        {
            CmdCheatDeviceQuality.ExecuteStatic(DeviceQualityType.Med);
        }

        protected void OnDisable()
        {
            Service.Binder.EventBus.OnNewContentAvailable -= new Service.Events.NewContentAvailable(this.onNewContentAvailable);
        }

        protected void OnEnable()
        {
            Service.Binder.EventBus.OnNewContentAvailable += new Service.Events.NewContentAvailable(this.onNewContentAvailable);
        }

        public void onEnableDungeonEventClicked(string eventType)
        {
            try
            {
                ConfigApp.CHEAT_DUNGEON_EVENT_TYPE = (DungeonEventType) ((int) Enum.Parse(typeof(DungeonEventType), eventType));
            }
            catch
            {
                return;
            }
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            if (ConfigApp.CHEAT_DUNGEON_EVENT_TYPE == DungeonEventType.Halloween)
            {
                LootTable table = GameLogic.Binder.ItemResources.getDynamicChestLootTable(ChestType.EventHalloween);
                for (int i = 0; i < table.Items.Count; i++)
                {
                    LootTableItem item = table.Items[i];
                    Item item2 = GameLogic.Binder.ItemResources.getItemForLootTableRollId(item.Id, ItemType.UNSPECIFIED);
                    this.gainItem(player, item2.Id);
                    ItemInstance ii = activeCharacter.getFirstOwnedItemInstanceWithItemId(item2.Id, null);
                    if (player.ActiveCharacter.canEquipItem(ii))
                    {
                        CmdEquipItem.ExecuteStatic(player.ActiveCharacter, ii);
                    }
                }
            }
            CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
            App.Binder.AppContext.hardReset(null);
        }

        public void onEnglishButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.English);
        }

        public void onFloorButtonClicked(int floor)
        {
            this.progressToFloor(floor, true);
        }

        public void onForceLoadPlayerProgressButtonClicked()
        {
        }

        public void onFpsCounterButtonClicked()
        {
            ConfigApp.CHEAT_SHOW_FPS_COUNTER = !ConfigApp.CHEAT_SHOW_FPS_COUNTER;
            PlayerView.Binder.DungeonHud.refreshFpsCounter();
            base.refresh();
        }

        public void onFrenchButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.French);
        }

        public void onFrenzyPotionButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Reward reward2 = new Reward();
                reward2.ShopEntryId = "FrenzyBundleSmall";
                reward2.FrenzyPotions = 1;
                Reward reward = reward2;
                this.transitionToRewardCeremony(reward, ConfigUi.CeremonyEntries.SHOP_PURCHASE);
            }
        }

        public void onGemsButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            double amount = Math.Max((double) (player.getResourceAmount(ResourceType.Diamond) * 20.0), (double) 99999.0);
            CmdGainResources.ExecuteStatic(player, ResourceType.Diamond, amount, false, string.Empty, null);
        }

        public void onGermanButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.German);
        }

        public void onIndonesianButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.Indonesian);
        }

        public void onItalianButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.Italian);
        }

        public void onJapaneseButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.Japanese);
        }

        public void onKoreanButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.Korean);
        }

        public void onMaxPetsButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            CmdSelectPet.ExecuteStatic(player, null);
            player.Pets.Instances.Clear();
            for (int i = 0; i < App.Binder.ConfigMeta.PETS.Count; i++)
            {
                CmdGainPet.ExecuteStatic(player, App.Binder.ConfigMeta.PETS[i].Id, 1, true);
            }
            for (int j = 0; j < player.Pets.Instances.Count; j++)
            {
                player.Pets.Instances[j].Level = ConfigGameplay.PET_MAX_LEVEL;
                player.Pets.Instances[j].Duplicates = 0;
                player.Pets.Instances[j].InspectedByPlayer = true;
            }
        }

        public void onMaxXpLevelButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            player.Rank = App.Binder.ConfigMeta.XP_LEVEL_CAP;
            GameLogic.Binder.EventBus.PlayerRankUpped(player, true);
            this.onAllSkillsButtonClicked();
        }

        public void onMultiPartChestCeremonyButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                Reward reward2 = new Reward();
                reward2.ChestType = ChestType.MysteryStandard;
                List<ItemInstance> list = new List<ItemInstance>();
                list.Add(new ItemInstance("Weapon009", 1, 0, 0, player));
                reward2.ItemDrops = list;
                Reward reward = reward2;
                string str = MenuHelpers.GetFormattedDescriptionColored(_.L(ConfigUi.CeremonyEntries.BOSS_VICTORY.Description, null, false), "$ChestName$", _.L(ConfigUi.CHEST_BLUEPRINTS[reward.getVisualChestType()].Name, null, false));
                RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.BOSS_VICTORY.Title, null, false));
                parameters2.Description = str;
                parameters2.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.BOSS_VICTORY.ChestOpenAtStart;
                parameters2.SingleReward = reward;
                RewardCeremonyMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter);
            }
        }

        public void onMysteryChestButtonClicked()
        {
            ChestType[] array = new ChestType[] { ChestType.MysteryStandard, ChestType.MysterySpecialOffer };
            ChestType randomValueFromArray = LangUtil.GetRandomValueFromArray<ChestType>(array);
            Player player = GameLogic.Binder.GameState.Player;
            Reward reward = new Reward();
            reward.ChestType = randomValueFromArray;
            reward.RewardSource = player.DungeonDropRewardSource;
            reward.RewardSourceId = player.DungeonDropRewardSourceId;
            CmdRollChestLootTable.ExecuteStatic(randomValueFromArray, player, false, ref reward, null);
            player.UnclaimedRewards.Add(reward);
            PlayerView.Binder.DungeonHud.TaskPanel.refreshTaskPanelMysteryAndBossChests();
        }

        private void onNewContentAvailable()
        {
            this.onRefresh();
        }

        public void onNext20UpgradesButtonClicked()
        {
            GameLogic.Binder.GameState.Player.Augmentations.cheatAddAugmentations(false, 20);
        }

        public void onNextBossButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                GameLogic.Binder.CommandProcessor.execute(new CmdCheatProgressBoss(), 0f);
            }
        }

        public void onNextFloorButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                GameLogic.Binder.LootSystem.grantRetirementTriggerChestIfAllowed();
                RoomEndCondition endCondition = !GameLogic.Binder.FrenzySystem.isFrenzyActive() ? RoomEndCondition.NORMAL_COMPLETION : RoomEndCondition.FRENZY_COMPLETION;
                GameLogic.Binder.CommandProcessor.execute(new CmdCompleteRoom(endCondition), 0f);
            }
        }

        public void onNextLastThemeFloorClicked(int dungeonCycleFloor)
        {
            int num = GameLogic.Binder.GameState.Player.getLastCompletedFloor(false) + 1;
            int num2 = num / ConfigDungeons.MAX_DUNGEON_COUNT;
            int targetFloor = (num2 * ConfigDungeons.MAX_DUNGEON_COUNT) + dungeonCycleFloor;
            if (num >= targetFloor)
            {
                num2++;
                targetFloor += ConfigDungeons.MAX_DUNGEON_COUNT;
            }
            this.progressToFloor(targetFloor, true);
        }

        public void onNextXpLevelButtonClicked()
        {
            CmdRankUpPlayer.ExecuteStatic(GameLogic.Binder.GameState.Player, true);
        }

        public void onNotificationsCheatButtonClicked()
        {
            ConfigApp.CHEAT_FAST_NOTIFICATIONS = !ConfigApp.CHEAT_FAST_NOTIFICATIONS;
            base.refresh();
        }

        public void onPetBoxButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                Reward reward = new Reward();
                reward.ChestType = ChestType.PetBoxSmall;
                reward.addShopEntryDrop(player, "PetBundleSmall", false);
                CmdConsumeReward.ExecuteStatic(player, reward, false, string.Empty);
                RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                parameters2.Title = "Success!";
                parameters2.Description = "placeholder description";
                parameters2.SingleReward = reward;
                RewardCeremonyMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, true, true);
            }
        }

        public void onPortugueseButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.Portuguese);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.reconstructContent();
        }

        public void onProtoResetButtonClicked()
        {
            foreach (string str in IOUtil.GetFilesInDirectoryFromPersistentStorage(string.Empty))
            {
                IOUtil.DeleteFileFromPersistentStorage(str);
            }
            CmdSavePlayerDataToPersistentStorage.ExecuteStatic(PlayerLoader.CreateProtoPlayer());
            App.Binder.AppContext.hardReset(null);
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("CHEATS", string.Empty, string.Empty);
            this.BuildInfoVersion.text = App.Binder.BuildResources.getBuildInfoDescription();
            this.BuildInfoServer.text = "Server: " + GameLogic.Binder.GameState.Player.Preferences.DevServerId;
            SocialData socialData = GameLogic.Binder.GameState.Player.SocialData;
            string text1 = "FB: " + socialData.FacebookName;
            if (text1 == null)
            {
            }
            this.FacebookName.text = "N/A";
            string text2 = "FBID: " + socialData.FacebookId;
            if (text2 == null)
            {
            }
            this.FacebookId.text = "N/A";
            object[] objArray1 = new object[] { "RemoteContent: version ", Service.Binder.ContentService.MasterRemoteContent.ContentVersion, " (", TimeUtil.UnixTimestampToDateTime(Service.Binder.ContentService.MasterRemoteContent.CacheTimestamp).ToString(), ")" };
            this.MasterRemoteContentText.text = string.Concat(objArray1);
            this.FpsCounterButtonText.text = !ConfigApp.CHEAT_SHOW_FPS_COUNTER ? "Show\nFPS counter" : "Hide\nFPS counter";
            this.NotificationsCheatButtonText.text = !ConfigApp.CHEAT_FAST_NOTIFICATIONS ? "Enable fast\nnotifications" : "Disable fast\nnotifications";
        }

        public void onRefreshMissionsClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            player.HasUnlockedMissions = true;
            for (int i = 0; i < player.Missions.Instances.Count; i++)
            {
                CmdStartMission.ExecuteStatic(player, player.Missions.Instances[i]);
            }
            foreach (KeyValuePair<string, PromotionEventInstance> pair in player.PromotionEvents.Instances)
            {
                PromotionEventInstance instance = pair.Value;
                for (int j = 0; j < instance.Missions.Instances.Count; j++)
                {
                    MissionInstance mission = instance.Missions.Instances[j];
                    CmdStartMission.ExecuteStatic(player, mission);
                }
            }
        }

        public void onRefreshVendorInventoryButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                CmdRefreshVendorInventory.ExecuteStatic(GameLogic.Binder.GameState.Player, false);
            }
        }

        public void onResetButtonClicked()
        {
            foreach (string str in IOUtil.GetFilesInDirectoryFromPersistentStorage(string.Empty))
            {
                IOUtil.DeleteFileFromPersistentStorage(str);
            }
            App.Binder.AppContext.hardReset(null);
        }

        public void onRevivePotionButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                string str = "ReviveBundleSmall";
                Reward reward2 = new Reward();
                reward2.ShopEntryId = str;
                reward2.Revives = (int) App.Binder.ConfigMeta.VENDOR_REVIVE_BUNDLES[str];
                Reward reward = reward2;
                this.transitionToRewardCeremony(reward, ConfigUi.CeremonyEntries.SHOP_PURCHASE);
            }
        }

        public void onRewardBoxCeremonyButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                Dictionary<Reward, bool> dictionary = new Dictionary<Reward, bool>();
                ChestType[] array = new ChestType[] { ChestType.RewardBoxCommon };
                for (int i = 0; i < 3; i++)
                {
                    Reward reward = new Reward();
                    ChestType randomValueFromArray = LangUtil.GetRandomValueFromArray<ChestType>(array);
                    reward.ChestType = randomValueFromArray;
                    CmdRollChestLootTable.ExecuteStatic(randomValueFromArray, player, false, ref reward, null);
                    dictionary.Add(reward, false);
                }
                RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.POST_RETIREMENT_GIFTS.Title, null, false));
                parameters2.Description = _.L(ConfigUi.CeremonyEntries.POST_RETIREMENT_GIFTS.Description, null, false);
                parameters2.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.POST_RETIREMENT_GIFTS.ChestOpenAtStart;
                parameters2.MultiRewards = dictionary;
                RewardCeremonyMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter);
            }
        }

        public void onRussianButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.Russian);
        }

        public void onSendPushNotificationClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if ((player.RemoteNotificationPlayerData != null) && !string.IsNullOrEmpty(player.RemoteNotificationPlayerData.RegistrationId))
            {
                Service.Binder.TaskManager.StartTask(Request<string>.Get(string.Format("/testpush/{0}/{1}/{2}", (player.RemoteNotificationPlayerData.Provider != RemoteNotificationProvider.GooglePlay) ? "ios" : "google", App.Binder.LocaSystem.DisplayLanguage, player.RemoteNotificationPlayerData.RegistrationId)).Task, null);
            }
        }

        public void onSinglePartChestCeremonyButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                Reward reward2 = new Reward();
                reward2.ChestType = ChestType.MysterySpecialOffer;
                List<ItemInstance> list = new List<ItemInstance>();
                list.Add(new ItemInstance("Weapon009", 1, 0, 0, player));
                reward2.ItemDrops = list;
                Reward reward = reward2;
                RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.AD_REWARD.Title, null, false));
                parameters2.Description = _.L(ConfigUi.CeremonyEntries.AD_REWARD.Description, null, false);
                parameters2.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.AD_REWARD.ChestOpenAtStart;
                parameters2.SingleReward = reward;
                RewardCeremonyMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter);
            }
        }

        public void onSkipTutorialsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                ConfigTutorials.CheatCompleteAllFtueTutorials(player);
                ConfigTutorials.CheatCompleteAllContextTutorials(player);
                player.setLastCompletedFloor(App.Binder.ConfigMeta.RETIREMENT_MIN_FLOOR, false);
                player.setResourceAmount(ResourceType.Coin, 100000.0);
                Reward item = new Reward();
                item.ChestType = ChestType.RewardBoxCommon;
                item.FrenzyPotions = 1;
                player.UnclaimedRewards.Add(item);
                GameLogic.Binder.LootSystem.grantRetirementTriggerChestIfAllowed();
                for (int i = 0; i < player.ActiveCharacter.ItemSlots.Count; i++)
                {
                    ItemInstance itemInstance = player.ActiveCharacter.ItemSlots[i].ItemInstance;
                    if (itemInstance != null)
                    {
                        itemInstance.Rank = App.Binder.ConfigMeta.RETIREMENT_MIN_FLOOR * App.Binder.ConfigMeta.LEVEL_TO_FLOOR_MULTIPLIER;
                    }
                }
                CmdRankUpPlayer.ExecuteStatic(player, true);
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
                App.Binder.AppContext.hardReset(null);
            }
        }

        public void onSpanishButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.Spanish);
        }

        public void onStarterDragonButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (!player.HasPurchasedStarterBundle)
                {
                    player.HasPurchasedStarterBundle = true;
                    CmdGainPet.ExecuteStatic(player, "Pet001", App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(1), true);
                }
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onSummonPetClicked(string petId)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && (((activeDungeon.CurrentGameplayState != GameplayState.ROOM_COMPLETION) && (activeDungeon.CurrentGameplayState != GameplayState.ENDED)) && (activeDungeon.CurrentGameplayState != GameplayState.END_CEREMONY)))
            {
                CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
                Vector3 worldPt = Vector3Extensions.ToXzVector3(activeCharacter.PhysicsBody.Transform.position) + ((Vector3) (activeCharacter.PhysicsBody.Transform.forward * 3f));
                worldPt.x += UnityEngine.Random.Range((float) -3f, (float) 3f);
                worldPt.z += UnityEngine.Random.Range((float) -3f, (float) 3f);
                worldPt = activeDungeon.ActiveRoom.calculateNearestEmptySpot(worldPt, activeCharacter.PhysicsBody.Transform.position - worldPt, 1f, 1f, 6f, null);
                CmdSpawnCharacter.SpawningData data2 = new CmdSpawnCharacter.SpawningData();
                data2.CharacterPrototype = GameLogic.Binder.CharacterResources.getResource(petId);
                data2.Rank = 1;
                data2.SpawnWorldPos = worldPt;
                data2.SpawnWorlRot = activeCharacter.PhysicsBody.Transform.rotation;
                data2.IsPlayerCharacter = true;
                data2.IsPlayerSupportCharacter = true;
                data2.IsPet = true;
                CmdSpawnCharacter.SpawningData data = data2;
                CmdSpawnCharacter.ExecuteStatic(data);
            }
        }

        public override void onTabButtonClicked(int idx)
        {
            this.reconstructContent();
        }

        public void onTestAppboyMessage()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && (GameLogic.Binder.GameState.ActiveDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                App.Binder.AppboyIOSBridge.Bridge.TestMessage();
            }
        }

        public void onTestSceneButtonClicked(string dungeonId)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && (GameLogic.Binder.GameState.ActiveDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                PlayerView.Binder.ScreenTransitionEffect.setOpaque(true);
                GameLogic.Binder.CommandProcessor.execute(new CmdCheatLoadDungeon(dungeonId), 0f);
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onTimeSkip1hButtonClicked()
        {
            this.timeSkip(60);
        }

        public void onTimeSkip24hButtonClicked()
        {
            this.timeSkip(0x5a0);
        }

        public void onTimeSkip6hButtonClicked()
        {
            this.timeSkip(360);
        }

        public void onTokensButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            double amount = Math.Max((double) (player.getResourceAmount(ResourceType.Token) * 20.0), (double) 99999.0);
            CmdGainResources.ExecuteStatic(player, ResourceType.Token, amount, false, string.Empty, null);
        }

        public void onTurkishButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.Turkish);
        }

        public void onUnlockPetsButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            CmdSelectPet.ExecuteStatic(player, null);
            player.Pets.Instances.Clear();
            for (int i = 0; i < App.Binder.ConfigMeta.PETS.Count; i++)
            {
                CmdGainPet.ExecuteStatic(player, App.Binder.ConfigMeta.PETS[i].Id, 1, true);
            }
            for (int j = 0; j < player.Pets.Instances.Count; j++)
            {
                player.Pets.Instances[j].Level = 1;
                player.Pets.Instances[j].Duplicates = 0;
                player.Pets.Instances[j].InspectedByPlayer = true;
            }
        }

        public void onVietnameseButtonClicked()
        {
            CmdCheatLanguage.ExecuteStatic(SystemLanguage.Vietnamese);
        }

        public void onXpPotionButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                string str = "XpBundleSmall";
                Reward reward2 = new Reward();
                reward2.ShopEntryId = str;
                reward2.XpPotions = (int) App.Binder.ConfigMeta.VENDOR_XP_BUNDLES[str];
                Reward reward = reward2;
                this.transitionToRewardCeremony(reward, ConfigUi.CeremonyEntries.SHOP_PURCHASE);
            }
        }

        private void progressToFloor(int targetFloor, bool doSimulateProgress)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
            {
                CmdCheatProgressFloor.ExecuteStatic(targetFloor, doSimulateProgress);
            }
        }

        private void reconstructContent()
        {
            int num = ((StackedPopupMenu) base.m_contentMenu).Smcc.getActiveTabIndex();
            foreach (KeyValuePair<int, GameObject[]> pair in this.m_tabGameObjects)
            {
                bool flag = num == pair.Key;
                for (int i = 0; i < pair.Value.Length; i++)
                {
                    pair.Value[i].SetActive(flag);
                }
            }
            if (num == 0)
            {
                this.DemoButtonRoot.SetActive(true);
            }
            this.onRefresh();
        }

        public void resetPromotionState()
        {
            foreach (RemotePromotion promotion in Service.Binder.PromotionService.Promotions)
            {
                if (promotion.State != null)
                {
                    promotion.State = new PromotionState(0L);
                }
            }
            CmdSavePlayerDataToPersistentStorage.ExecuteStatic(GameLogic.Binder.GameState.Player);
            App.Binder.AppContext.hardReset(null);
        }

        private void timeSkip(int minutes)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                CmdCheatAfk.ExecuteStatic(minutes);
            }
        }

        private void transitionToRewardCeremony(Reward reward, RewardCeremonyEntry rce)
        {
            if (reward != null)
            {
                Player player = GameLogic.Binder.GameState.Player;
                CmdConsumeReward.ExecuteStatic(player, reward, true, string.Empty);
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
                RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                parameters2.Title = StringExtensions.ToUpperLoca(_.L(rce.Title, null, false));
                parameters2.Description = _.L(rce.Description, null, false);
                parameters2.SingleRewardOpenAtStart = rce.ChestOpenAtStart;
                parameters2.SingleReward = reward;
                RewardCeremonyMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter);
            }
            else
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected void Update()
        {
            string[] textArray1 = new string[] { "Sent: ", (Service.Binder.ServiceContext.TotalKbsSent / 1024f).ToString("0.00"), " MB / ", "Received: ", (Service.Binder.ServiceContext.TotalKbsReceived / 1024f).ToString("0.00"), " MB" };
            this.NetworkUsage.text = string.Concat(textArray1);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.CheatPopupContent;
            }
        }

        public override bool UsesTabs
        {
            get
            {
                return true;
            }
        }
    }
}

