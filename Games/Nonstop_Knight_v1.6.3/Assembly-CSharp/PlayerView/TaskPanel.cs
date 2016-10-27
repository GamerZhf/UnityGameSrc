namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TaskPanel : MonoBehaviour
    {
        private Coroutine m_dungeonDropGainRoutine;
        private Dictionary<TaskPanelItemType, TaskPanelAction> m_taskPanelActions;
        private List<TaskPanelItem> m_taskPanelItems = new List<TaskPanelItem>();
        public RectTransform TaskPanelVerticalGroup;

        protected void Awake()
        {
            Dictionary<TaskPanelItemType, TaskPanelAction> dictionary = new Dictionary<TaskPanelItemType, TaskPanelAction>(new TaskPanelItemTypeBoxAvoidanceComparer());
            TaskPanelAction action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onAchievementTaskButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelAchievements);
            dictionary.Add(TaskPanelItemType.AchievementTask, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onVendorNewButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelVendorNewItems);
            dictionary.Add(TaskPanelItemType.VendorNewItems, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onLeaderboardRewardButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelLeaderboardRewards);
            dictionary.Add(TaskPanelItemType.LeaderboardReward, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onSilverChestButtonClicked);
            dictionary.Add(TaskPanelItemType.NormalChestDrop, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onBossChestButtonClicked);
            dictionary.Add(TaskPanelItemType.BossChestDrop, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onLevelUpRewardButtonClicked);
            dictionary.Add(TaskPanelItemType.LevelUpReward, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onStarterBundleButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelStarterBundle);
            dictionary.Add(TaskPanelItemType.StarterBundle, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onAppboyButtonClicked);
            dictionary.Add(TaskPanelItemType.AppboyTask, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onRateGameButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelRateGame);
            dictionary.Add(TaskPanelItemType.RateGame, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onHeroNamingTutorialButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelHeroNamingTutorial);
            dictionary.Add(TaskPanelItemType.HeroNamingTutorial, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onUpdateRewardButtonClicked);
            dictionary.Add(TaskPanelItemType.UpdateReward, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onMissionCompletedButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelMissionCompleted);
            dictionary.Add(TaskPanelItemType.MissionCompleted, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onMissionsBigPrizeButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelMissionsBigPrize);
            dictionary.Add(TaskPanelItemType.MissionsBigPrize, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onNewMissionsButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelNewMissions);
            dictionary.Add(TaskPanelItemType.NewMissions, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onPetLevelUpButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelPetLevelUp);
            dictionary.Add(TaskPanelItemType.PetLevelUp, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onPromotionButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelPromotions);
            dictionary.Add(TaskPanelItemType.Promotion, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onHighestFloorReachedTaskButtonClicked);
            dictionary.Add(TaskPanelItemType.HighestFloorReached, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onFrenzyPotionTutorialButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelFrenzyPotionTutorial);
            dictionary.Add(TaskPanelItemType.FrenzyPotionTutorial, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onSocialGiftButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelSocialGifts);
            dictionary.Add(TaskPanelItemType.SocialGift, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onTournamentMilestoneMainRewardButtonClicked);
            dictionary.Add(TaskPanelItemType.TournamentMilestoneMainReward, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onTournamentMilestoneContributorRewardButtonClicked);
            dictionary.Add(TaskPanelItemType.TournamentMilestoneContributorReward, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onTournamentCardPackButtonClicked);
            dictionary.Add(TaskPanelItemType.TournamentCardPack, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onPromotionEventMissionCompletedButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelPromotionEventMissionCompleted);
            dictionary.Add(TaskPanelItemType.PromotionEventMissionCompleted, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onPromotionEventMissionsBigPrizeButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelPromotionEventMissionsBigPrize);
            dictionary.Add(TaskPanelItemType.PromotionEventMissionsBigPrize, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onBossHuntFirstTimeTutorialButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelBossHuntFirstTimeTutorial);
            dictionary.Add(TaskPanelItemType.BossHuntFirstTimeTutorial, action);
            action = new TaskPanelAction();
            action.ClickHandler = new System.Action(this.onBossHuntVeteranTutorialButtonClicked);
            action.AutoRefresh = new System.Action(this.refreshTaskPanelBossHuntVeteranTutorial);
            dictionary.Add(TaskPanelItemType.BossHuntVeteranTutorial, action);
            this.m_taskPanelActions = dictionary;
        }

        public bool doesTaskPanelItemExist(TaskPanelItemType type)
        {
            <doesTaskPanelItemExist>c__AnonStorey2E9 storeye = new <doesTaskPanelItemExist>c__AnonStorey2E9();
            storeye.type = type;
            return (this.m_taskPanelItems.Find(new Predicate<TaskPanelItem>(storeye.<>m__189)) != null);
        }

        [DebuggerHidden]
        private IEnumerator dungeonDropGainRoutine(Reward reward)
        {
            <dungeonDropGainRoutine>c__Iterator101 iterator = new <dungeonDropGainRoutine>c__Iterator101();
            iterator.reward = reward;
            iterator.<$>reward = reward;
            iterator.<>f__this = this;
            return iterator;
        }

        public Vector3 getTaskPanelItemWorldPosition(TaskPanelItemType type)
        {
            <getTaskPanelItemWorldPosition>c__AnonStorey2EA storeyea = new <getTaskPanelItemWorldPosition>c__AnonStorey2EA();
            storeyea.type = type;
            TaskPanelItem item = this.m_taskPanelItems.Find(new Predicate<TaskPanelItem>(storeyea.<>m__18A));
            return ((item != null) ? item.transform.position : Vector3.zero);
        }

        public void onAchievementTaskButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                int num;
                string str = GameLogic.Binder.GameState.Player.Achievements.getFirstCompletableAchievement(out num);
                if (!string.IsNullOrEmpty(str))
                {
                    ConfigAchievements.SharedData data = ConfigAchievements.SHARED_DATA[str];
                    double totalAmount = CmdClaimAchievement.ExecuteStatic(GameLogic.Binder.GameState.Player, str, num, false);
                    Reward reward = new Reward();
                    MathUtil.DistributeValuesIntoChunksDouble(totalAmount, 3, ref reward.DiamondDrops);
                    string str2 = _.L(ConfigLoca.CEREMONY_DESCRIPTION_ACHIEVEMENT, null, false) + "\n" + MenuHelpers.ColoredText(_.L(data.Description, new <>__AnonType9<string>(MenuHelpers.BigValueToString(data.TierRequirements[num])), false));
                    RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                    parameters2.Title = StringExtensions.ToUpperLoca(_.L(data.Title, null, false));
                    parameters2.Description = str2;
                    parameters2.SingleRewardOpenAtStart = !reward.isWrappedInsideChest();
                    parameters2.SingleReward = reward;
                    RewardCeremonyMenu.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, true, true);
                }
            }
        }

        public void onAppboyButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && (App.Binder.AppboyIOSBridge.Bridge.GetPendingMessageCount() > 0))
            {
                App.Binder.AppboyIOSBridge.Bridge.ShowNextMessage();
                this.refreshTaskPanelAppboy();
            }
        }

        public void onBossChestButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && !UnityUtils.CoroutineRunning(ref this.m_dungeonDropGainRoutine))
            {
                RewardCeremonyMenu.InputParameters parameters3;
                Player player = GameLogic.Binder.GameState.Player;
                Dictionary<Reward, bool> dict = new Dictionary<Reward, bool>();
                for (int i = player.UnclaimedRewards.Count - 1; i >= 0; i--)
                {
                    Reward reward = player.UnclaimedRewards[i];
                    if (player.isClaimableReward(reward) && ConfigMeta.IsBossChest(reward.ChestType))
                    {
                        CmdConsumeReward.ExecuteStatic(player, reward, true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                        dict.Add(reward, false);
                    }
                }
                if (dict.Count > 1)
                {
                    parameters3 = new RewardCeremonyMenu.InputParameters();
                    parameters3.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.BOSS_VICTORY.Title, null, false));
                    parameters3.Description = _.L(ConfigLoca.CEREMONY_DESCRIPTION_BOSS_VICTORY_MULTIPLE, null, false);
                    parameters3.MultiRewards = dict;
                    RewardCeremonyMenu.InputParameters parameter = parameters3;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, false, true);
                }
                else if (dict.Count == 1)
                {
                    Reward firstKeyFromDict = LangUtil.GetFirstKeyFromDict<Reward, bool>(dict);
                    string str = MenuHelpers.GetFormattedDescriptionColored(_.L(ConfigUi.CeremonyEntries.BOSS_VICTORY.Description, null, false), "$ChestName$", _.L(ConfigUi.CHEST_BLUEPRINTS[firstKeyFromDict.getVisualChestType()].Name, null, false));
                    parameters3 = new RewardCeremonyMenu.InputParameters();
                    parameters3.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.BOSS_VICTORY.Title, null, false));
                    parameters3.Description = str;
                    parameters3.SingleReward = firstKeyFromDict;
                    RewardCeremonyMenu.InputParameters parameters2 = parameters3;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters2, 0f, false, true);
                }
                else
                {
                    UnityEngine.Debug.LogWarning("No unclaimed boss chests found");
                }
            }
        }

        public void onBossHuntFirstTimeTutorialButtonClicked()
        {
            PlayerView.Binder.TutorialSystem.startContextTutorial("CTUT006");
        }

        public void onBossHuntVeteranTutorialButtonClicked()
        {
            PlayerView.Binder.TutorialSystem.startContextTutorial("CTUT007");
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnVendorInventoryRefreshed -= new GameLogic.Events.VendorInventoryRefreshed(this.onVendorInventoryRefreshed);
            GameLogic.Binder.EventBus.OnVendorInventoryInspected -= new GameLogic.Events.VendorInventoryInspected(this.onVendorInventoryInspected);
            GameLogic.Binder.EventBus.OnTournamentDonationMade -= new GameLogic.Events.TournamentDonationMade(this.onTournamentDonationMade);
            Service.Binder.EventBus.OnAppboyIngameMessageReady -= new Service.Events.AppboyIngameMessageReady(this.refreshTaskPanelAppboy);
            Service.Binder.EventBus.OnTournamentCardsReceived -= new Service.Events.TournamentCardsReceived(this.onTournamentCardsReceived);
            Service.Binder.EventBus.OnPromotionsAvailable -= new Service.Events.PromotionsAvailable(this.onPromotionsAvailable);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnVendorInventoryRefreshed += new GameLogic.Events.VendorInventoryRefreshed(this.onVendorInventoryRefreshed);
            GameLogic.Binder.EventBus.OnVendorInventoryInspected += new GameLogic.Events.VendorInventoryInspected(this.onVendorInventoryInspected);
            GameLogic.Binder.EventBus.OnTournamentDonationMade += new GameLogic.Events.TournamentDonationMade(this.onTournamentDonationMade);
            Service.Binder.EventBus.OnAppboyIngameMessageReady += new Service.Events.AppboyIngameMessageReady(this.refreshTaskPanelAppboy);
            Service.Binder.EventBus.OnTournamentCardsReceived += new Service.Events.TournamentCardsReceived(this.onTournamentCardsReceived);
            Service.Binder.EventBus.OnPromotionsAvailable += new Service.Events.PromotionsAvailable(this.onPromotionsAvailable);
        }

        public void onFrenzyPotionTutorialButtonClicked()
        {
            PlayerView.Binder.TutorialSystem.startContextTutorial("CTUT005");
        }

        public void onHeroNamingTutorialButtonClicked()
        {
            PlayerView.Binder.TutorialSystem.startContextTutorial("CTUT001");
        }

        public void onHighestFloorReachedTaskButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                Reward reward = new Reward();
                double item = App.Binder.ConfigMeta.HighestFloorCoins(player);
                reward.CoinDrops.Add(item);
                CmdConsumeReward.ExecuteStatic(player, reward, false, string.Empty);
                player.HighestFloorRewardClaimedDuringThisRun = true;
                RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.CEREMONY_TITLE_HIGHEST_FLOOR, null, false));
                parameters2.Description = _.L(ConfigLoca.CEREMONY_DESCRIPTION_HIGHEST_FLOOR, null, false);
                parameters2.SingleRewardOpenAtStart = !reward.isWrappedInsideChest();
                parameters2.SingleReward = reward;
                RewardCeremonyMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, true, true);
                this.refreshTaskPanelHighestFloorReached();
            }
        }

        public void onLeaderboardRewardButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (player.UnclaimedLeaderboardRewards.Count > 0)
                {
                    Reward reward = new Reward();
                    RewardCeremonyMenu.InputParameters parameter = new RewardCeremonyMenu.InputParameters();
                    int text = 0;
                    int num2 = 0;
                    for (int i = 0; i < player.UnclaimedLeaderboardRewards.Count; i++)
                    {
                        if (player.UnclaimedLeaderboardRewards[i].Dummy)
                        {
                            num2++;
                        }
                        else
                        {
                            text++;
                        }
                    }
                    string input = string.Empty;
                    LeaderboardEntry entry = null;
                    for (int j = player.UnclaimedLeaderboardRewards.Count - 1; j >= 0; j--)
                    {
                        LeaderboardEntry entry2 = player.UnclaimedLeaderboardRewards[j];
                        if (entry == null)
                        {
                            entry = entry2;
                        }
                        else if (entry2.isDummy())
                        {
                            if (entry.isDummy() && (entry2.HighestFloor > entry.HighestFloor))
                            {
                                entry = entry2;
                            }
                        }
                        else if (entry.isDummy() || (entry2.HighestFloor > entry.HighestFloor))
                        {
                            entry = entry2;
                        }
                    }
                    if (text > 0)
                    {
                        LeaderboardEntry entry3 = null;
                        for (int k = 0; k < player.UnclaimedLeaderboardRewards.Count; k++)
                        {
                            if (!player.UnclaimedLeaderboardRewards[k].Dummy)
                            {
                                LeaderboardEntry entry4 = player.UnclaimedLeaderboardRewards[k];
                                if (num2 == 0)
                                {
                                    double item = App.Binder.ConfigMeta.LeaderboardCoins(player);
                                    reward.CoinDrops.Add(item);
                                }
                                entry3 = entry4;
                            }
                        }
                        if (text == 1)
                        {
                            input = entry3.getPrettyName();
                        }
                        for (int m = player.UnclaimedLeaderboardRewards.Count - 1; m >= 0; m--)
                        {
                            if (!player.UnclaimedLeaderboardRewards[m].Dummy)
                            {
                                player.UnclaimedLeaderboardRewards.RemoveAt(m);
                            }
                        }
                    }
                    if (num2 > 0)
                    {
                        LeaderboardEntry entry5 = null;
                        for (int n = 0; n < player.UnclaimedLeaderboardRewards.Count; n++)
                        {
                            if (player.UnclaimedLeaderboardRewards[n].Dummy)
                            {
                                LeaderboardEntry entry6 = player.UnclaimedLeaderboardRewards[n];
                                reward.DiamondDrops.Add(App.Binder.ConfigMeta.LEADERBOARD_RANK_REWARD_DIAMONDS);
                                entry5 = entry6;
                            }
                        }
                        if (num2 == 1)
                        {
                            input = _.L(entry5.Name, null, false);
                        }
                        for (int num9 = player.UnclaimedLeaderboardRewards.Count - 1; num9 >= 0; num9--)
                        {
                            if (player.UnclaimedLeaderboardRewards[num9].Dummy)
                            {
                                player.UnclaimedLeaderboardRewards.RemoveAt(num9);
                            }
                        }
                    }
                    string str2 = string.Empty;
                    if (((text == 1) && (num2 == 0)) || ((text == 0) && (num2 == 1)))
                    {
                        if (App.Binder.LocaSystem.IsArabic(input))
                        {
                            input = RTLConverter.Reverse(input);
                        }
                        str2 = _.L(ConfigLoca.CEREMONY_DESCRIPTION_LEADERBOARD_RANK_UP1, new <>__AnonType10<string>(MenuHelpers.ColoredText(input)), false);
                    }
                    else if ((text > 1) && (num2 == 0))
                    {
                        str2 = _.L(ConfigLoca.CEREMONY_DESCRIPTION_LEADERBOARD_RANK_UP2, new <>__AnonType4<string>(MenuHelpers.ColoredText(text)), false);
                    }
                    else if ((text == 0) && (num2 > 1))
                    {
                        str2 = _.L(ConfigLoca.CEREMONY_DESCRIPTION_LEADERBOARD_RANK_UP3, new <>__AnonType4<string>(MenuHelpers.ColoredText(num2)), false);
                    }
                    else if ((text == 1) && (num2 == 1))
                    {
                        str2 = _.L(ConfigLoca.CEREMONY_DESCRIPTION_LEADERBOARD_RANK_UP4, new <>__AnonType11<string, string>(MenuHelpers.ColoredText(text), MenuHelpers.ColoredText(num2)), false);
                    }
                    else if ((text == 1) && (num2 > 1))
                    {
                        str2 = _.L(ConfigLoca.CEREMONY_DESCRIPTION_LEADERBOARD_RANK_UP5, new <>__AnonType11<string, string>(MenuHelpers.ColoredText(text), MenuHelpers.ColoredText(num2)), false);
                    }
                    else if ((text > 1) && (num2 == 1))
                    {
                        str2 = _.L(ConfigLoca.CEREMONY_DESCRIPTION_LEADERBOARD_RANK_UP6, new <>__AnonType11<string, string>(MenuHelpers.ColoredText(text), MenuHelpers.ColoredText(num2)), false);
                    }
                    else if ((text > 1) && (num2 > 1))
                    {
                        str2 = _.L(ConfigLoca.CEREMONY_DESCRIPTION_LEADERBOARD_RANK_UP7, new <>__AnonType11<string, string>(MenuHelpers.ColoredText(text), MenuHelpers.ColoredText(num2)), false);
                    }
                    RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                    parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.LEADERBOARD_RANK_UP.Title, null, false));
                    parameters2.Description = str2;
                    parameters2.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.LEADERBOARD_RANK_UP.ChestOpenAtStart;
                    parameters2.SingleReward = reward;
                    parameters2.BeatenLeaderboardEntry = entry;
                    parameter = parameters2;
                    CmdConsumeReward.ExecuteStatic(player, reward, true, string.Empty);
                    player.NextLeaderboardTargetVisible = true;
                    LeaderboardEntry entry7 = GameLogic.Binder.LeaderboardSystem.getNextLeaderboardTargetForPlayer(LeaderboardType.Royal, GameLogic.Binder.GameState.Player);
                    if (entry7 != null)
                    {
                        parameter.NextLeaderboardEntryTarget = entry7;
                        GameLogic.Binder.GameState.Player.NextLeaderboardTargetVisible = false;
                    }
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, false, true);
                }
            }
        }

        public void onLevelUpRewardButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                RewardCeremonyMenu.InputParameters parameters5;
                Player player = GameLogic.Binder.GameState.Player;
                CharacterInstance activeCharacter = player.ActiveCharacter;
                if (player.PendingSkillUnlocks.Count > 0)
                {
                    Reward reward = new Reward();
                    SkillType skillType = player.PendingSkillUnlocks[0];
                    reward.Skill = skillType;
                    CmdUnlockSkill.ExecuteStatic(player, skillType, false);
                    this.refreshTaskPanelUnclaimedLevelUpRewards();
                    if (ConfigSkills.SHARED_DATA[reward.Skill].Passive)
                    {
                        parameters5 = new RewardCeremonyMenu.InputParameters();
                        parameters5.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.TALENT_UNLOCK.Title, null, false));
                        parameters5.Description = _.L(ConfigUi.CeremonyEntries.TALENT_UNLOCK.Description, null, false);
                        parameters5.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.TALENT_UNLOCK.ChestOpenAtStart;
                        parameters5.SingleReward = reward;
                        RewardCeremonyMenu.InputParameters parameter = parameters5;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, false, true);
                    }
                    else
                    {
                        ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[skillType];
                        string str = MenuHelpers.ColoredText(StringExtensions.ToUpperLoca(_.L(data.Name, null, false)));
                        string str2 = MenuHelpers.GetFormattedSkillDescription(_.L(data.Description, null, false), activeCharacter, skillType, PerkType.NONE, false, true);
                        parameters5 = new RewardCeremonyMenu.InputParameters();
                        parameters5.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.SKILL_UNLOCK.Title, null, false));
                        parameters5.Description = _.L(ConfigUi.CeremonyEntries.SKILL_UNLOCK.Description, null, false);
                        parameters5.Description3 = str;
                        parameters5.Description4 = str2;
                        parameters5.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.SKILL_UNLOCK.ChestOpenAtStart;
                        parameters5.SingleReward = reward;
                        RewardCeremonyMenu.InputParameters parameters2 = parameters5;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters2, 0f, false, true);
                    }
                }
                else if (player.PendingRankUpRunestoneUnlocks.Count > 0)
                {
                    Reward reward2 = new Reward();
                    string runestoneId = player.PendingRankUpRunestoneUnlocks[0];
                    PerkType type = ConfigRunestones.GetRunestoneData(runestoneId).PerkInstance.Type;
                    reward2.RunestoneDrops.Add(runestoneId);
                    CmdGainRunestone.ExecuteStatic(player, runestoneId, false);
                    this.refreshTaskPanelUnclaimedLevelUpRewards();
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(runestoneId);
                    string str4 = _.L(ConfigSkills.SHARED_DATA[skillTypeForRunestone].Name, null, false);
                    string str5 = MenuHelpers.GetFormattedDescription(_.L(ConfigUi.CeremonyEntries.RUNESTONE_UNLOCK.Description, null, false), "$SkillName$", str4);
                    string str6 = MenuHelpers.ColoredText(StringExtensions.ToUpperLoca(_.L(ConfigRunestones.GetName(runestoneId), null, false)));
                    string str7 = MenuHelpers.GetFormattedSkillDescription(_.L(ConfigRunestones.GetDescription(runestoneId), null, false), activeCharacter, skillTypeForRunestone, type, false, true);
                    parameters5 = new RewardCeremonyMenu.InputParameters();
                    parameters5.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.RUNESTONE_UNLOCK.Title, null, false));
                    parameters5.Description = str5;
                    parameters5.Description3 = str6;
                    parameters5.Description4 = str7;
                    parameters5.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.RUNESTONE_UNLOCK.ChestOpenAtStart;
                    parameters5.SingleReward = reward2;
                    RewardCeremonyMenu.InputParameters parameters3 = parameters5;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters3, 0f, false, true);
                }
                else if (player.NumPendingRankUpCeremonies > 0)
                {
                    Reward reward3 = new Reward();
                    double amount = App.Binder.ConfigMeta.RankUpRewardGems(player.Rank);
                    CmdGainResources.ExecuteStatic(player, ResourceType.Diamond, amount, true, string.Empty, null);
                    reward3.DiamondDrops.Add(amount);
                    player.NumPendingRankUpCeremonies--;
                    this.refreshTaskPanelUnclaimedLevelUpRewards();
                    if (player.canRankUp())
                    {
                        CmdRankUpPlayer.ExecuteStatic(player, false);
                    }
                    RewardCeremonyEntry entry = !player.isAtMaxRank() ? ConfigUi.CeremonyEntries.RANK_UP : ConfigUi.CeremonyEntries.RANK_UP_MAX_LEVEL;
                    parameters5 = new RewardCeremonyMenu.InputParameters();
                    parameters5.Title = StringExtensions.ToUpperLoca(_.L(entry.Title, null, false));
                    parameters5.Description = _.L(entry.Description, null, false);
                    parameters5.SingleRewardOpenAtStart = entry.ChestOpenAtStart;
                    parameters5.SingleReward = reward3;
                    RewardCeremonyMenu.InputParameters parameters4 = parameters5;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters4, 0f, false, true);
                }
            }
        }

        public void onMissionCompletedButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                MissionInstance mission = player.Missions.getFirstRewardableMissionInstance();
                if (mission != null)
                {
                    Reward reward = new Reward();
                    mission.fillReward(player, ref reward);
                    CmdConsumeReward.ExecuteStatic(player, reward, true, string.Empty);
                    mission.RewardClaimed = true;
                    CmdEndMission.ExecuteStatic(player, mission, true);
                    RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                    parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.CEREMONY_TITLE_ONE_MISSION_COMPLETED, null, false));
                    parameters2.Description = _.L(ConfigLoca.CEREMONY_DESCRIPTION_ONE_MISSION_COMPLETED, null, false) + "\n" + MenuHelpers.ColoredText(ConfigMissions.GetMissionData(mission.MissionId).getFormattedMissionDescription(mission.Requirement, false, null));
                    parameters2.SingleRewardOpenAtStart = !reward.isWrappedInsideChest();
                    parameters2.SingleReward = reward;
                    RewardCeremonyMenu.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, true, true);
                }
                else
                {
                    UnityEngine.Debug.LogError("No rewardable mission instance found");
                }
            }
        }

        public void onMissionsBigPrizeButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                int text = player.Missions.getNumCompletedMissionsRequiredForBigPrize();
                Reward reward2 = new Reward();
                reward2.ChestType = App.Binder.ConfigMeta.MISSION_BIG_PRIZE_CHEST_TYPE;
                Reward reward = reward2;
                CmdRollChestLootTable.ExecuteStatic(App.Binder.ConfigMeta.MISSION_BIG_PRIZE_CHEST_TYPE, player, false, ref reward, null);
                CmdConsumeReward.ExecuteStatic(player, reward, true, string.Empty);
                player.Missions.NumUnclaimedMissionCompletions = Mathf.Max(player.Missions.NumUnclaimedMissionCompletions - text, 0);
                RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.CEREMONY_TITLE_MEGABOX, null, false));
                parameters2.Description = _.L(ConfigLoca.CEREMONY_DESCRIPTION_ALL_MISSIONS_COMPLETED, new <>__AnonType4<string>(MenuHelpers.ColoredText(text)), false);
                parameters2.SingleReward = reward;
                RewardCeremonyMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, true, true);
            }
        }

        public void onNewMissionsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.MissionsPopupContent, null, 0f, false, true);
            }
        }

        public void onPetLevelUpButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                PetInstance instance = player.Pets.getFirstLevelUppablePet();
                if (instance != null)
                {
                    Reward reward2 = new Reward();
                    List<string> list = new List<string>();
                    list.Add("PetBundleSmall");
                    reward2.ShopEntryDrops = list;
                    List<PetReward> list2 = new List<PetReward>();
                    PetReward item = new PetReward();
                    item.PetId = instance.CharacterId;
                    list2.Add(item);
                    reward2.Pets = list2;
                    Reward reward = reward2;
                    CmdLevelUpPet.ExecuteStatic(player, instance.CharacterId, false);
                    if (instance.Level != 0)
                    {
                        RewardCeremonyMenu.InputParameters parameters3;
                        if (instance.Level == 1)
                        {
                            string petAttackTypeDescription = MenuHelpers.GetPetAttackTypeDescription(instance.Character);
                            parameters3 = new RewardCeremonyMenu.InputParameters();
                            parameters3.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.CEREMONY_TITLE_PET_UNLOCK, null, false));
                            parameters3.Description = _.L(ConfigLoca.CEREMONY_DESCRIPTION_PET_UNLOCK, null, false);
                            parameters3.Description3 = StringExtensions.ToUpperLoca(instance.Character.Name);
                            parameters3.Description4 = petAttackTypeDescription;
                            parameters3.SingleReward = reward;
                            parameters3.SingleRewardOpenAtStart = true;
                            parameters3.DisableFlyToHud = true;
                            parameters3.CeremonyEntry = ConfigUi.CeremonyEntries.PET_UNLOCK;
                            RewardCeremonyMenu.InputParameters parameter = parameters3;
                            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, true, true);
                        }
                        else
                        {
                            string str3;
                            string str2 = StringExtensions.ToUpperLoca(instance.Character.Name);
                            PerkInstance instance2 = instance.Character.FixedPerks.getPerkInstanceAtIndex(instance.Level - 2);
                            ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[instance2.Type];
                            if (data.LinkedToRunestone != null)
                            {
                                ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(data.LinkedToRunestone);
                                str3 = _.L(ConfigLoca.PET_PERK_DESCRIPTION, new <>__AnonType0<string, string>(_.L(ConfigPerks.SHARED_DATA[runestoneData.PerkInstance.Type].ShortDescription, null, false), _.L(ConfigSkills.SHARED_DATA[runestoneData.LinkedToSkill].Name, null, false)), false);
                            }
                            else
                            {
                                str3 = MenuHelpers.GetFormattedPerkDescription(instance2.Type, instance2.Modifier, data.DurationSeconds, data.Threshold, 0f, false);
                            }
                            parameters3 = new RewardCeremonyMenu.InputParameters();
                            parameters3.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.CEREMONY_TITLE_PET_LEVELUP, null, false));
                            parameters3.Description = _.L(ConfigLoca.CEREMONY_DESCRIPTION_PET_LEVELUP, null, false);
                            parameters3.Description3 = str2;
                            parameters3.Description4 = str3;
                            parameters3.SingleReward = reward;
                            parameters3.SingleRewardOpenAtStart = true;
                            parameters3.DisableFlyToHud = true;
                            parameters3.CeremonyEntry = ConfigUi.CeremonyEntries.PET_LEVEL_UP;
                            RewardCeremonyMenu.InputParameters parameters2 = parameters3;
                            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters2, 0f, true, true);
                        }
                    }
                }
            }
        }

        public void onPromotionButtonClicked()
        {
            RemotePromotion nextPromotionVisibleInTaskPanel = Service.Binder.PromotionManager.GetNextPromotionVisibleInTaskPanel();
            if (nextPromotionVisibleInTaskPanel != null)
            {
                Service.Binder.PromotionManager.ShowPromotionPopup(nextPromotionVisibleInTaskPanel);
            }
        }

        public void onPromotionEventMissionCompletedButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                PromotionEventInstance instance = null;
                MissionInstance instance2 = null;
                if (player.PromotionEvents.hasEvents())
                {
                    instance = player.PromotionEvents.getNewestEventInstance();
                    instance2 = instance.Missions.getFirstRewardableMissionInstance();
                }
                if (instance2 != null)
                {
                    ConfigMissions.Mission missionData = ConfigMissions.GetMissionData(instance2.MissionId);
                    Reward reward = new Reward();
                    instance2.fillReward(player, ref reward);
                    CmdConsumeReward.ExecuteStatic(player, reward, true, string.Empty);
                    instance2.RewardClaimed = true;
                    CmdEndMission.ExecuteStatic(player, instance2, true);
                    string text = missionData.getFormattedMissionDescription(instance2.Requirement, false, instance.getData().Missions.getDescriptionOverride(instance2.MissionId));
                    RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                    parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.CEREMONY_TITLE_ONE_PROMOTION_EVENT_MISSION_COMPLETED, null, false));
                    parameters2.Description = _.L(ConfigLoca.CEREMONY_DESCRIPTION_ONE_PROMOTION_EVENT_MISSION_COMPLETED, null, false) + "\n" + MenuHelpers.ColoredText(text);
                    parameters2.SingleRewardOpenAtStart = !reward.isWrappedInsideChest();
                    parameters2.SingleReward = reward;
                    RewardCeremonyMenu.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, true, true);
                }
                else
                {
                    UnityEngine.Debug.LogError("No rewardable promotion event mission instance found");
                }
            }
        }

        public void onPromotionEventMissionsBigPrizeButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (!player.PromotionEvents.hasEvents())
                {
                    UnityEngine.Debug.LogError("No rewardable promotion events found");
                }
                else
                {
                    PromotionEventInstance instance = player.PromotionEvents.getNewestEventInstance();
                    ConfigPromotionEvents.Event event2 = instance.getData();
                    int text = instance.Missions.getNumCompletedMissionsRequiredForBigPrize();
                    instance.Missions.NumUnclaimedMissionCompletions = Mathf.Max(instance.Missions.NumUnclaimedMissionCompletions - text, 0);
                    instance.Missions.NumClaimedBigPrizes++;
                    ChestType bigPrizeRewardChestType = event2.Missions.BigPrizeRewardChestType;
                    if (bigPrizeRewardChestType != ChestType.NONE)
                    {
                        LootTable table = GameLogic.Binder.ItemResources.getDynamicChestLootTable(bigPrizeRewardChestType);
                        Dictionary<Reward, bool> dictionary = new Dictionary<Reward, bool>();
                        for (int i = 0; i < table.Items.Count; i++)
                        {
                            Item item = GameLogic.Binder.ItemResources.getItemForLootTableRollId(table.Items[i].Id, ItemType.UNSPECIFIED);
                            Reward reward = new Reward();
                            reward.ChestType = bigPrizeRewardChestType;
                            reward.ItemDrops.Add(ItemInstance.Create(item, player, -1));
                            CmdConsumeReward.ExecuteStatic(player, reward, true, string.Empty);
                            dictionary.Add(reward, false);
                        }
                        RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                        parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.CEREMONY_TITLE_PROMOTION_EVENT_BIG_PRIZE, null, false));
                        parameters2.Description = _.L(ConfigLoca.CEREMONY_DESCRIPTION_ALL_PROMOTION_EVENT_MISSIONS_COMPLETED, new <>__AnonType4<string>(MenuHelpers.ColoredText(text)), false);
                        parameters2.MultiRewards = dictionary;
                        RewardCeremonyMenu.InputParameters parameter = parameters2;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, true, true);
                    }
                }
            }
        }

        public void onPromotionsAvailable()
        {
            this.refreshTaskPanelPromotions();
        }

        public void onRateGameButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                GameLogic.Binder.GameState.Player.HasSeenRateGamePopup = true;
                this.refreshTaskPanelRateGame();
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.RateGameMiniPopupContent, null, 0f, false, true);
            }
        }

        public void onSilverChestButtonClicked()
        {
            <onSilverChestButtonClicked>c__AnonStorey2EB storeyeb = new <onSilverChestButtonClicked>c__AnonStorey2EB();
            storeyeb.<>f__this = this;
            if (!PlayerView.Binder.MenuSystem.InTransition && !UnityUtils.CoroutineRunning(ref this.m_dungeonDropGainRoutine))
            {
                storeyeb.player = GameLogic.Binder.GameState.Player;
                storeyeb.reward = storeyeb.player.getFirstUnclaimedReward(ChestType.MysterySpecialOffer, storeyeb.player.DungeonDropRewardSource, storeyeb.player.DungeonDropRewardSourceId);
                if (storeyeb.reward != null)
                {
                    if (!Service.Binder.AdsSystem.adReady())
                    {
                        CmdConvertReward.ExecuteStatic(storeyeb.player, storeyeb.reward, ChestType.MysteryStandard);
                        this.m_dungeonDropGainRoutine = UnityUtils.StartCoroutine(this, this.dungeonDropGainRoutine(storeyeb.reward));
                    }
                    else
                    {
                        MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                        SpecialOfferAdPromptMiniPopup.InputParams @params = new SpecialOfferAdPromptMiniPopup.InputParams();
                        @params.Reward = storeyeb.reward;
                        @params.CancelCallback = new System.Action(storeyeb.<>m__18B);
                        parameters2.MenuContentParams = @params;
                        MiniPopupMenu.InputParameters parameter = parameters2;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.SpecialOfferAdPromptMiniPopup, parameter, 0f, false, true);
                    }
                }
                else
                {
                    storeyeb.reward = storeyeb.player.getFirstUnclaimedReward(ChestType.MysteryStandard, storeyeb.player.DungeonDropRewardSource, storeyeb.player.DungeonDropRewardSourceId);
                    if (storeyeb.reward != null)
                    {
                        this.m_dungeonDropGainRoutine = UnityUtils.StartCoroutine(this, this.dungeonDropGainRoutine(storeyeb.reward));
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning("No unclaimed normal or ad chests found");
                    }
                }
            }
        }

        public void onSocialGiftButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (player.getNumberOfUnclaimedSocialGifts() > 0)
                {
                    Reward reward = player.getFirstUnclaimedSocialReward();
                    if (reward != null)
                    {
                        FbPlatformUser user;
                        CmdConsumeReward.ExecuteStatic(player, reward, true, string.Empty);
                        RewardCeremonyEntry ceremonyEntryForReward = ConfigUi.GetCeremonyEntryForReward(reward);
                        string str = !Service.Binder.FacebookAdapter.Friends.TryGetValue(reward.RewardSourceId, out user) ? "a friend!" : user.userName;
                        RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                        parameters2.Title = StringExtensions.ToUpperLoca(_.L(ceremonyEntryForReward.Title, null, false));
                        parameters2.Description = string.Format("Social gift from {0}", str);
                        parameters2.SingleReward = reward;
                        parameters2.SingleRewardOpenAtStart = true;
                        RewardCeremonyMenu.InputParameters parameter = parameters2;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, false, true);
                    }
                }
                this.refreshTaskPanelSocialGifts();
            }
        }

        public void onStarterBundleButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                GameLogic.Binder.GameState.Player.HasOpenedStarterBundleOfferFromTaskPanel = true;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.StarterBundlePopupContent, null, 0f, false, true);
            }
        }

        public void onTournamentCardPackButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Reward reward = GameLogic.Binder.GameState.Player.getFirstUnclaimedReward(ChestType.TournamentCards);
                if (reward != null)
                {
                    string localizedTitle = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_MILESTONE_TITLE, null, false));
                    string localizedDescription = _.L(ConfigLoca.BH_INCREASE_GROUP_POWER, null, false) + "\n" + _.L(ConfigLoca.UI_PROMPT_CHOOSE_ONE, null, false);
                    if (reward.RewardSource == RewardSourceType.TournamentPartyMember)
                    {
                        localizedTitle = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_DONATION_TITLE, null, false));
                        localizedDescription = _.L(ConfigLoca.BH_GIFT_FROM_PLAYER, new <>__AnonType5<string>(reward.RewardSourceId), false) + "\n" + _.L(ConfigLoca.UI_PROMPT_CHOOSE_ONE, null, false);
                    }
                    else if (reward.RewardSource == RewardSourceType.SelfReward)
                    {
                        localizedTitle = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_DONATION_TITLE, null, false));
                        localizedDescription = _.L(ConfigLoca.BH_GIFT_FROM_YOU, null, false) + "\n" + _.L(ConfigLoca.UI_PROMPT_CHOOSE_ONE, null, false);
                    }
                    RewardCeremonyMenu.StartCardPackCeremony(localizedTitle, localizedDescription, reward, false);
                }
            }
        }

        private void onTournamentCardsReceived(string tournamentId)
        {
            this.refreshTaskPanelTournamentCardPack();
        }

        private void onTournamentDonationMade(Player player, TournamentInstance ti, int count, double totalPrice)
        {
            this.refreshTaskPanelTournamentCardPack();
        }

        public void onTournamentMilestoneContributorRewardButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Reward reward = GameLogic.Binder.GameState.Player.getFirstUnclaimedReward(ChestType.TournamentContributorReward);
                if (reward != null)
                {
                    RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                    parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_MILESTONE_TITLE, null, false));
                    parameters2.Description = _.L(ConfigLoca.BH_MILESTONE_TOP_HUNTER, null, false);
                    parameters2.SingleReward = reward;
                    parameters2.SingleRewardOpenAtStart = true;
                    parameters2.ConsumeRewardsAfterChestOpen = true;
                    RewardCeremonyMenu.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, false, true);
                }
            }
        }

        public void onTournamentMilestoneMainRewardButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                Reward reward = player.getFirstUnclaimedReward(ChestType.TournamentMainReward);
                if (reward != null)
                {
                    CmdConsumeReward.ExecuteStatic(player, reward, true, null);
                    Dictionary<Reward, bool> dict = new Dictionary<Reward, bool>();
                    if (reward.MegaBoxes > 0)
                    {
                        for (int i = player.UnclaimedRewards.Count - 1; i >= 0; i--)
                        {
                            Reward key = player.UnclaimedRewards[i];
                            if (key.ChestType == ChestType.RewardBoxMulti)
                            {
                                dict.Add(key, false);
                                CmdConsumeReward.ExecuteStatic(player, key, true, null);
                            }
                        }
                    }
                    else
                    {
                        bool flag = (reward.getVisualChestType() == ChestType.NONE) || (reward.getVisualChestType() == ChestType.TournamentMainReward);
                        dict.Add(reward, flag);
                    }
                    Reward firstKeyFromDict = null;
                    bool flag2 = false;
                    if (dict.Count == 1)
                    {
                        firstKeyFromDict = LangUtil.GetFirstKeyFromDict<Reward, bool>(dict);
                        flag2 = dict[firstKeyFromDict];
                        dict = null;
                    }
                    RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                    parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_MILESTONE_TITLE, null, false));
                    parameters2.Description = _.L(ConfigLoca.BH_MILESTONE_DESCRIPTION, null, false);
                    parameters2.SingleReward = firstKeyFromDict;
                    parameters2.SingleRewardOpenAtStart = flag2;
                    parameters2.MultiRewards = dict;
                    RewardCeremonyMenu.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, false, true);
                }
                this.refreshTaskPanelTournamentMainReward();
            }
        }

        public void onUpdateRewardButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (player.UnclaimedUpdateRewards.Count != 0)
                {
                    int index = player.UnclaimedUpdateRewards.Count - 1;
                    Reward reward = player.UnclaimedUpdateRewards[index];
                    player.UnclaimedUpdateRewards.RemoveAt(index);
                    this.refreshTaskPanelUpdateReward();
                    CmdConsumeReward.ExecuteStatic(player, reward, false, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                    RewardCeremonyEntry ceremonyEntryForReward = ConfigUi.GetCeremonyEntryForReward(reward);
                    RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                    parameters2.Title = StringExtensions.ToUpperLoca(_.L(ceremonyEntryForReward.Title, null, false));
                    parameters2.Description = _.L(ceremonyEntryForReward.Description, null, false);
                    parameters2.Description2 = _.L(ceremonyEntryForReward.Description2, null, false);
                    parameters2.SingleRewardOpenAtStart = ceremonyEntryForReward.ChestOpenAtStart;
                    parameters2.SingleReward = reward;
                    RewardCeremonyMenu.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, false, true);
                }
            }
        }

        private void onVendorInventoryInspected(Player player)
        {
            this.refreshTaskPanelVendorNewItems();
        }

        private void onVendorInventoryRefreshed(Player player)
        {
            this.refreshTaskPanelVendorNewItems();
        }

        public void onVendorNewButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.VendorPopupContent, null, 0f, false, true);
            }
        }

        public void refreshTaskPanelAchievements()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.refreshTaskPanelItem(TaskPanelItemType.AchievementTask, player.Achievements.getNumberOfCompletableAchievements(), null);
        }

        public void refreshTaskPanelAppboy()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((activeDungeon != null) && activeDungeon.isTutorialDungeon())
            {
                this.refreshTaskPanelItem(TaskPanelItemType.AppboyTask, 0, null);
            }
            else
            {
                int pendingMessageCount = App.Binder.AppboyIOSBridge.Bridge.GetPendingMessageCount();
                this.refreshTaskPanelItem(TaskPanelItemType.AppboyTask, pendingMessageCount, null);
            }
        }

        public void refreshTaskPanelBossHuntFirstTimeTutorial()
        {
            Player player = GameLogic.Binder.GameState.Player;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            TournamentView view = Service.Binder.TournamentSystem.NextAvailableViewOrNull();
            bool flag = ((((!player.Tournaments.hasTournamentSelected() && player.Tournaments.tournamentsUnlocked()) && (!activeDungeon.ActiveRoom.MainBossSummoned && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))) && ((!player.hasCompletedTutorial("CTUT006") && !PlayerView.Binder.TutorialSystem.isContextTutorialActive()) && (!PlayerView.Binder.NotificationSystem.NotifyAdventurePanelTournamentTab && Service.Binder.TournamentSystem.Initialized))) && ((view != null) && (view.ServerJoinStatus == TournamentViewRemote.Status.OK))) && !player.Tournaments.hasTournamentBeenNotified(view.TournamentInfo.Id);
            this.refreshTaskPanelItem(TaskPanelItemType.BossHuntFirstTimeTutorial, !flag ? 0 : 1, null);
        }

        public void refreshTaskPanelBossHuntVeteranTutorial()
        {
            Player player = GameLogic.Binder.GameState.Player;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            TournamentView view = Service.Binder.TournamentSystem.NextAvailableViewOrNull();
            bool flag = ((((!player.Tournaments.hasTournamentSelected() && player.Tournaments.tournamentsUnlocked()) && (!activeDungeon.ActiveRoom.MainBossSummoned && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))) && ((player.hasCompletedTutorial("CTUT006") && !PlayerView.Binder.TutorialSystem.isContextTutorialActive()) && (!PlayerView.Binder.NotificationSystem.NotifyAdventurePanelTournamentTab && Service.Binder.TournamentSystem.Initialized))) && ((view != null) && (view.ServerJoinStatus == TournamentViewRemote.Status.OK))) && !player.Tournaments.hasTournamentBeenNotified(view.TournamentInfo.Id);
            this.refreshTaskPanelItem(TaskPanelItemType.BossHuntVeteranTutorial, !flag ? 0 : 1, null);
        }

        public void refreshTaskPanelFrenzyPotionTutorial()
        {
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            bool flag = (((App.Binder.ConfigMeta.CTUT005_ENABLED && (player.CumulativeRetiredHeroStats.HeroesRetired == 1)) && ((player.CumulativeRetiredHeroStats.MinionsKilledDuringFrenzy == 0) && (activeCharacter.HeroStats.FrenzyActivations == 0))) && (((activeCharacter.Inventory.FrenzyPotions >= 1) && (player.getCurrentFloor(false) >= 4)) && (!player.hasCompletedTutorial("CTUT005") && !PlayerView.Binder.TutorialSystem.isContextTutorialActive()))) && !((SlidingInventoryMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingInventoryMenu)).isPotionUsageForceDisabled();
            this.refreshTaskPanelItem(TaskPanelItemType.FrenzyPotionTutorial, !flag ? 0 : 1, null);
        }

        public void refreshTaskPanelHeroNamingTutorial()
        {
            Player player = GameLogic.Binder.GameState.Player;
            bool flag = ((!player.SocialData.HasGivenCustomName && !player.hasCompletedTutorial("CTUT001")) && player.isHeroOrSkillPopupUnlocked()) && !PlayerView.Binder.TutorialSystem.isContextTutorialActive();
            this.refreshTaskPanelItem(TaskPanelItemType.HeroNamingTutorial, !flag ? 0 : 1, null);
        }

        public void refreshTaskPanelHighestFloorReached()
        {
            Player player = GameLogic.Binder.GameState.Player;
            bool flag = ((!player.Tournaments.hasTournamentSelected() && player.hasRetired()) && !player.HighestFloorRewardClaimedDuringThisRun) && (player.ActiveCharacter.HeroStats.HighestFloor > player.CumulativeRetiredHeroStats.HighestFloor);
            this.refreshTaskPanelItem(TaskPanelItemType.HighestFloorReached, !flag ? 0 : 1, null);
        }

        public TaskPanelItem refreshTaskPanelItem(TaskPanelItemType type, int counter, [Optional, DefaultParameterValue(null)] SpriteAtlasEntry iconSpriteOverride)
        {
            TaskPanelItem item = null;
            for (int i = 0; i < this.m_taskPanelItems.Count; i++)
            {
                if (this.m_taskPanelItems[i].Type == type)
                {
                    item = this.m_taskPanelItems[i];
                    break;
                }
            }
            bool flag = item != null;
            if (counter <= 0)
            {
                if (flag)
                {
                    this.m_taskPanelItems.Remove(item);
                    PlayerView.Binder.TaskPanelItemPool.returnObject(item);
                    this.refreshTaskPanelRoot();
                }
                return null;
            }
            if (!flag)
            {
                item = PlayerView.Binder.TaskPanelItemPool.getObject();
                item.initialize(type, ConfigUi.TASK_PANEL_ITEM_BLUEPRINTS[type]);
                item.RectTm.SetParent(this.TaskPanelVerticalGroup, false);
                item.gameObject.SetActive(true);
                if (ConfigUi.TASK_PANEL_DO_SHOW_NEWEST_FIRST)
                {
                    item.RectTm.SetAsFirstSibling();
                }
                else
                {
                    item.RectTm.SetAsLastSibling();
                }
                this.m_taskPanelItems.Add(item);
                this.refreshTaskPanelRoot();
            }
            item.refresh(this.m_taskPanelActions[type].ClickHandler, this.m_taskPanelActions[type].AutoRefresh, counter, iconSpriteOverride);
            return item;
        }

        public void refreshTaskPanelLeaderboardRewards()
        {
            bool flag = GameLogic.Binder.GameState.Player.UnclaimedLeaderboardRewards.Count > 0;
            this.refreshTaskPanelItem(TaskPanelItemType.LeaderboardReward, !flag ? 0 : 1, null);
        }

        public void refreshTaskPanelMissionCompleted()
        {
            Player player = GameLogic.Binder.GameState.Player;
            int counter = !player.HasUnlockedMissions ? 0 : player.Missions.getNumRewardableMissions();
            this.refreshTaskPanelItem(TaskPanelItemType.MissionCompleted, counter, null);
        }

        public void refreshTaskPanelMissionsBigPrize()
        {
            Player player = GameLogic.Binder.GameState.Player;
            bool flag = (player.Missions.hasEnoughCompletedMissionsForBigPrize() && (player.Missions.getNumRewardableMissions() == 0)) && player.HasUnlockedMissions;
            this.refreshTaskPanelItem(TaskPanelItemType.MissionsBigPrize, !flag ? 0 : 1, null);
        }

        public void refreshTaskPanelMysteryAndBossChests()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.refreshTaskPanelItem(TaskPanelItemType.NormalChestDrop, player.getNumberOfUnclaimedMysteryChests(), null);
            int counter = player.getNumberOfUnclaimedBossChests();
            SpriteAtlasEntry iconSpriteOverride = (counter != 0) ? ConfigUi.CHEST_BLUEPRINTS[player.getFirstUnclaimedBossReward().getVisualChestType()].DropSprite : null;
            this.refreshTaskPanelItem(TaskPanelItemType.BossChestDrop, counter, iconSpriteOverride);
            this.refreshTaskPanelRoot();
        }

        public void refreshTaskPanelNewMissions()
        {
        }

        public void refreshTaskPanelPetLevelUp()
        {
            int counter = GameLogic.Binder.GameState.Player.Pets.getTotalNumOfLevelUps();
            this.refreshTaskPanelItem(TaskPanelItemType.PetLevelUp, counter, null);
        }

        public void refreshTaskPanelPromotionEventMissionCompleted()
        {
            Player player = GameLogic.Binder.GameState.Player;
            int counter = !player.PromotionEvents.hasEvents() ? 0 : player.PromotionEvents.getNewestEventInstance().Missions.getNumRewardableMissions();
            this.refreshTaskPanelItem(TaskPanelItemType.PromotionEventMissionCompleted, counter, null);
        }

        public void refreshTaskPanelPromotionEventMissionsBigPrize()
        {
            Player player = GameLogic.Binder.GameState.Player;
            bool flag = player.PromotionEvents.hasEvents();
            SpriteAtlasEntry iconSpriteOverride = null;
            if (flag)
            {
                PromotionEventInstance instance = player.PromotionEvents.getNewestEventInstance();
                flag = instance.Missions.hasEnoughCompletedMissionsForBigPrize();
                iconSpriteOverride = instance.getData().Missions.BigPrizeSprite;
            }
            this.refreshTaskPanelItem(TaskPanelItemType.PromotionEventMissionsBigPrize, !flag ? 0 : 1, iconSpriteOverride);
        }

        public void refreshTaskPanelPromotions()
        {
            int counter = (Service.Binder.PromotionManager.GetNextPromotionVisibleInTaskPanel() == null) ? 0 : 1;
            this.refreshTaskPanelItem(TaskPanelItemType.Promotion, counter, null);
        }

        public void refreshTaskPanelRateGame()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.refreshTaskPanelItem(TaskPanelItemType.RateGame, !player.doShowRateGamePopup() ? 0 : 1, null);
        }

        private void refreshTaskPanelRoot()
        {
            for (int i = 0; i < this.m_taskPanelItems.Count; i++)
            {
                TaskPanelItem item = !ConfigUi.TASK_PANEL_DO_SHOW_NEWEST_FIRST ? this.m_taskPanelItems[i] : this.m_taskPanelItems[(this.m_taskPanelItems.Count - 1) - i];
                item.ContentRoot.SetActive(i < ConfigUi.TASK_PANEL_NUM_VISIBLE_ITEMS_MAX);
            }
        }

        public void refreshTaskPanelSocialGifts()
        {
            Player player = GameLogic.Binder.GameState.Player;
            bool flag = !player.Tournaments.hasTournamentSelected() && (player.getNumberOfUnclaimedSocialGifts() > 0);
            this.refreshTaskPanelItem(TaskPanelItemType.SocialGift, !flag ? 0 : 1, null);
        }

        public void refreshTaskPanelStarterBundle()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.refreshTaskPanelItem(TaskPanelItemType.StarterBundle, !player.doOfferStarterBundle() ? 0 : 1, null);
        }

        public void refreshTaskPanelTournamentCardPack()
        {
            Player player = GameLogic.Binder.GameState.Player;
            int counter = 0;
            if (player.Tournaments.hasTournamentSelected())
            {
                counter = player.getNumberOfUnclaimedChestsOfType(ChestType.TournamentCards);
            }
            this.refreshTaskPanelItem(TaskPanelItemType.TournamentCardPack, counter, null);
        }

        public void refreshTaskPanelTournamentContributorReward()
        {
            Player player = GameLogic.Binder.GameState.Player;
            int counter = player.getNumberOfUnclaimedChestsOfType(ChestType.TournamentContributorReward);
            SpriteAtlasEntry iconSpriteOverride = null;
            if (counter > 0)
            {
                Reward reward = player.getFirstUnclaimedReward(ChestType.TournamentContributorReward);
                if (reward.Pets.Count > 0)
                {
                    iconSpriteOverride = GameLogic.Binder.CharacterResources.getResource(reward.Pets[0].PetId).AvatarSprite;
                }
            }
            this.refreshTaskPanelItem(TaskPanelItemType.TournamentMilestoneContributorReward, counter, iconSpriteOverride);
        }

        public void refreshTaskPanelTournamentMainReward()
        {
            Player player = GameLogic.Binder.GameState.Player;
            int counter = player.getNumberOfUnclaimedChestsOfType(ChestType.TournamentMainReward);
            SpriteAtlasEntry iconSpriteOverride = null;
            if (counter > 0)
            {
                Reward reward = player.getFirstUnclaimedReward(ChestType.TournamentMainReward);
                if (reward.ChestTypeVisualOverride != ChestType.NONE)
                {
                    iconSpriteOverride = ConfigUi.CHEST_BLUEPRINTS[reward.ChestTypeVisualOverride].SoloSprite;
                }
                else if (reward.TokenDrops.Count > 0)
                {
                    iconSpriteOverride = ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Token];
                }
                else if (reward.MegaBoxes > 0)
                {
                    iconSpriteOverride = ConfigUi.CHEST_BLUEPRINTS[ChestType.RewardBoxMulti].SoloSprite;
                }
                else if (reward.isWrappedInsideChest())
                {
                    iconSpriteOverride = ConfigUi.CHEST_BLUEPRINTS[reward.getVisualChestType()].SoloSprite;
                }
            }
            this.refreshTaskPanelItem(TaskPanelItemType.TournamentMilestoneMainReward, counter, iconSpriteOverride);
        }

        public void refreshTaskPanelUnclaimedLevelUpRewards()
        {
            this.refreshTaskPanelItem(TaskPanelItemType.LevelUpReward, GameLogic.Binder.GameState.Player.getNumberOfUnclaimedLevelUpRewards(), null);
        }

        public void refreshTaskPanelUpdateReward()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.refreshTaskPanelItem(TaskPanelItemType.UpdateReward, player.UnclaimedUpdateRewards.Count, null);
        }

        public void refreshTaskPanelVendorNewItems()
        {
            bool flag = ((App.Binder.ConfigMeta.ALLOW_VENDOR_TASKPANEL_NOTIFIER && Service.Binder.AdsSystem.adReady()) && (!GameLogic.Binder.GameState.Player.Vendor.InventoryInspected && PlayerView.Binder.DungeonHud.VendorButton.Tm.gameObject.activeSelf)) && PlayerView.Binder.DungeonHud.VendorButton.Button.interactable;
            this.refreshTaskPanelItem(TaskPanelItemType.VendorNewItems, !flag ? 0 : 1, null);
        }

        protected void Update()
        {
            if ((GameLogic.Binder.GameState.ActiveDungeon != null) && (GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom != null))
            {
                foreach (KeyValuePair<TaskPanelItemType, TaskPanelAction> pair in this.m_taskPanelActions)
                {
                    TaskPanelAction action = pair.Value;
                    if (action.AutoRefresh != null)
                    {
                        action.Timer -= Time.unscaledDeltaTime;
                        if (action.Timer <= 0f)
                        {
                            action.AutoRefresh();
                            action.Timer = action.AutoRefreshFreqSeconds + TaskPanelAction.AUTO_UPDATE_RANDOM_OFFSET_SECONDS.getRandom();
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <doesTaskPanelItemExist>c__AnonStorey2E9
        {
            internal TaskPanelItemType type;

            internal bool <>m__189(TaskPanelItem x)
            {
                return (x.Type == this.type);
            }
        }

        [CompilerGenerated]
        private sealed class <dungeonDropGainRoutine>c__Iterator101 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Reward <$>reward;
            internal TaskPanel <>f__this;
            internal int <i>__7;
            internal IEnumerator <ie>__4;
            internal IEnumerator <ie>__6;
            internal int <j>__3;
            internal int <j>__5;
            internal Player <player>__0;
            internal Vector2 <sourceScreenPos>__2;
            internal Vector3 <sourceWorldPt>__1;
            internal Reward reward;

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
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        this.<sourceWorldPt>__1 = Vector3.zero;
                        if (!ConfigMeta.IsMysteryChest(this.reward.ChestType))
                        {
                            this.<sourceWorldPt>__1 = this.<>f__this.getTaskPanelItemWorldPosition(TaskPanelItemType.BossChestDrop);
                            break;
                        }
                        this.<sourceWorldPt>__1 = this.<>f__this.getTaskPanelItemWorldPosition(TaskPanelItemType.NormalChestDrop);
                        break;

                    case 1:
                        this.<sourceScreenPos>__2 = RectTransformUtility.WorldToScreenPoint(PlayerView.Binder.DungeonHud.Canvas.worldCamera, this.<sourceWorldPt>__1);
                        PlayerView.Binder.AudioSystem.playSfxIfNotAlreadyPlaying(AudioSourceType.SfxUi_RewardClaim);
                        this.<j>__3 = 0;
                        goto Label_0169;

                    case 2:
                        goto Label_014B;

                    case 3:
                        goto Label_01E9;

                    default:
                        goto Label_0330;
                }
                this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdConsumeReward(this.<player>__0, this.reward, true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN"), 0f);
                this.$PC = 1;
                goto Label_0332;
            Label_014B:
                while (this.<ie>__4.MoveNext())
                {
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 2;
                    goto Label_0332;
                }
                this.<j>__3++;
            Label_0169:
                if (this.<j>__3 < this.reward.CoinDrops.Count)
                {
                    PlayerView.Binder.DungeonHud.flyToHudCoinGain(this.reward.CoinDrops[this.<j>__3], this.<sourceScreenPos>__2, false);
                    this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.HUD_RESOURCE_GAIN_VISUALIZATION_INTERVAL);
                    goto Label_014B;
                }
                this.<j>__5 = 0;
                while (this.<j>__5 < this.reward.DiamondDrops.Count)
                {
                    PlayerView.Binder.DungeonHud.flyToHudDiamondGain(this.reward.DiamondDrops[this.<j>__5], this.<sourceScreenPos>__2, false);
                    this.<ie>__6 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.HUD_RESOURCE_GAIN_VISUALIZATION_INTERVAL);
                Label_01E9:
                    while (this.<ie>__6.MoveNext())
                    {
                        this.$current = this.<ie>__6.Current;
                        this.$PC = 3;
                        goto Label_0332;
                    }
                    this.<j>__5++;
                }
                this.<i>__7 = 0;
                while (this.<i>__7 < this.reward.ItemDrops.Count)
                {
                    PlayerView.Binder.DungeonHud.flyToHudItemGain(this.reward.ItemDrops[this.<i>__7], this.<sourceScreenPos>__2, false, false);
                    this.<i>__7++;
                }
                if (this.reward.Revives > 0)
                {
                    PlayerView.Binder.DungeonHud.flyToHudPotionGain(PotionType.Revive, this.<sourceScreenPos>__2, false, false);
                }
                if (this.reward.FrenzyPotions > 0)
                {
                    PlayerView.Binder.DungeonHud.flyToHudPotionGain(PotionType.Frenzy, this.<sourceScreenPos>__2, false, false);
                }
                if (this.reward.BossPotions > 0)
                {
                    PlayerView.Binder.DungeonHud.flyToHudPotionGain(PotionType.Boss, this.<sourceScreenPos>__2, false, false);
                }
                if (this.reward.Skill != SkillType.NONE)
                {
                    PlayerView.Binder.DungeonHud.flyToHudSkillGain(this.reward.Skill, this.<sourceScreenPos>__2, false, false);
                }
                this.<>f__this.m_dungeonDropGainRoutine = null;
                goto Label_0330;
                this.$PC = -1;
            Label_0330:
                return false;
            Label_0332:
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
        private sealed class <getTaskPanelItemWorldPosition>c__AnonStorey2EA
        {
            internal TaskPanelItemType type;

            internal bool <>m__18A(TaskPanelItem x)
            {
                return (x.Type == this.type);
            }
        }

        [CompilerGenerated]
        private sealed class <onSilverChestButtonClicked>c__AnonStorey2EB
        {
            internal TaskPanel <>f__this;
            internal Player player;
            internal Reward reward;

            internal void <>m__18B()
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                CmdConvertReward.ExecuteStatic(this.player, this.reward, ChestType.MysteryStandard);
                this.<>f__this.m_dungeonDropGainRoutine = UnityUtils.StartCoroutine(this.<>f__this, this.<>f__this.dungeonDropGainRoutine(this.reward));
            }
        }

        private class TaskPanelAction
        {
            [CompilerGenerated]
            private float <Timer>k__BackingField;
            public static MinMax AUTO_UPDATE_RANDOM_OFFSET_SECONDS = new MinMax(0f, 2f);
            public System.Action AutoRefresh;
            public float AutoRefreshFreqSeconds = 2f;
            public System.Action ClickHandler;

            public TaskPanelAction()
            {
                this.Timer = AUTO_UPDATE_RANDOM_OFFSET_SECONDS.getRandom();
            }

            public float Timer
            {
                [CompilerGenerated]
                get
                {
                    return this.<Timer>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<Timer>k__BackingField = value;
                }
            }
        }
    }
}

