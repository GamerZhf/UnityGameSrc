namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AnnouncementSystem : MonoBehaviour
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache3;
        [CompilerGenerated]
        private bool <PendingBlockingAnnouncements>k__BackingField;
        private int m_pendingAdventureMilestoneAnnouncement;
        private bool m_pendingFloorEntryAnnouncement;

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameplayLoadingStarted -= new GameLogic.Events.GameplayLoadingStarted(this.onGameplayLoadingStarted);
            GameLogic.Binder.EventBus.OnGameplayStateChanged -= new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameplayLoadingStarted += new GameLogic.Events.GameplayLoadingStarted(this.onGameplayLoadingStarted);
            GameLogic.Binder.EventBus.OnGameplayStateChanged += new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
        }

        private void onGameplayLoadingStarted(ActiveDungeon ad)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (!ad.isTutorialDungeon())
            {
                this.m_pendingFloorEntryAnnouncement = true;
                if (((ad.ActiveTournament == null) && player.hasCompletedTutorial("TUT351C")) && ((ad.Floor > player.getHighestFloorReached()) && PlayerView.Binder.AdventureMilestones.floorHasMilestone(player, ad.Floor)))
                {
                    this.m_pendingAdventureMilestoneAnnouncement = ad.Floor;
                }
            }
        }

        private void onGameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            if (!GameLogic.Binder.GameState.ActiveDungeon.isTutorialDungeon() && (currentState == GameplayState.START_CEREMONY_STEP1))
            {
                this.PendingBlockingAnnouncements = true;
            }
        }

        private bool triggerBlockingAnnouncement()
        {
            string str4;
            string str6;
            string str7;
            string str8;
            List<BannerPopupContent.RewardContent> list;
            string str10;
            TournamentInstance instance;
            int num11;
            int totalContribution;
            int contribution;
            string str11;
            string str12;
            Reward reward13;
            RewardCeremonyMenu.InputParameters parameters13;
            BannerPopupContent.InputParameters parameters14;
            Player player = GameLogic.Binder.GameState.Player;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (player.PendingPostRetirementTokenRewardCeremony)
            {
                Reward reward = player.getFirstUnclaimedRetirementTriggerChest();
                if ((reward != null) && !reward.isEmpty())
                {
                    if (reward.TokenDrops.Count > 0)
                    {
                        reward13 = new Reward();
                        reward13.TokenDrops = new List<double>(reward.TokenDrops);
                        Reward reward2 = reward13;
                        CmdConsumeReward.ExecuteStatic(player, reward2, false, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                        reward.TokenDrops.Clear();
                        parameters13 = new RewardCeremonyMenu.InputParameters();
                        parameters13.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.POST_RETIREMENT_TOKENS.Title, null, false));
                        parameters13.Description = _.L(ConfigUi.CeremonyEntries.POST_RETIREMENT_TOKENS.Description, null, false);
                        parameters13.SingleReward = reward2;
                        parameters13.SingleRewardOpenAtStart = true;
                        RewardCeremonyMenu.InputParameters parameters = parameters13;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters, 0f, false, true);
                    }
                    else if (reward.FrenzyPotions > 0)
                    {
                        reward13 = new Reward();
                        reward13.FrenzyPotions = reward.FrenzyPotions;
                        Reward reward3 = reward13;
                        CmdConsumeReward.ExecuteStatic(player, reward3, false, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                        reward.FrenzyPotions = 0;
                        parameters13 = new RewardCeremonyMenu.InputParameters();
                        parameters13.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.POST_RETIREMENT_FRENZY_POTIONS.Title, null, false));
                        parameters13.Description = _.L(ConfigUi.CeremonyEntries.POST_RETIREMENT_FRENZY_POTIONS.Description, null, false);
                        parameters13.SingleReward = reward3;
                        parameters13.SingleRewardOpenAtStart = true;
                        RewardCeremonyMenu.InputParameters parameters2 = parameters13;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters2, 0f, false, true);
                    }
                }
                else
                {
                    player.PendingPostRetirementTokenRewardCeremony = false;
                }
                return true;
            }
            if (player.PendingPostRetirementGiftRewardCeremony)
            {
                Dictionary<Reward, bool> dictionary = new Dictionary<Reward, bool>();
                for (int m = player.UnclaimedRewards.Count - 1; m >= 0; m--)
                {
                    Reward key = player.UnclaimedRewards[m];
                    if (ConfigMeta.IsRetirementChest(key.ChestType) && (key.ChestType != ChestType.RetirementTrigger))
                    {
                        dictionary.Add(key, false);
                    }
                }
                if (dictionary.Count > 0)
                {
                    foreach (KeyValuePair<Reward, bool> pair in dictionary)
                    {
                        CmdConsumeReward.ExecuteStatic(player, pair.Key, true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                    }
                    PlayerView.Binder.DungeonHud.resetResourceBar();
                    parameters13 = new RewardCeremonyMenu.InputParameters();
                    parameters13.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.POST_RETIREMENT_GIFTS.Title, null, false));
                    parameters13.Description = _.L(ConfigUi.CeremonyEntries.POST_RETIREMENT_GIFTS.Description, null, false);
                    parameters13.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.POST_RETIREMENT_GIFTS.ChestOpenAtStart;
                    parameters13.MultiRewards = dictionary;
                    parameters13.DisableFlyToHud = true;
                    RewardCeremonyMenu.InputParameters parameters3 = parameters13;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters3, 0f, false, true);
                }
                player.PendingPostRetirementGiftRewardCeremony = false;
                return true;
            }
            if (player.UnclaimedPassiveRewardableSeconds > 0L)
            {
                Player.PassiveProgress progress = player.getPassiveProgress(player.getLastCompletedFloor(false) + 1, player.UnclaimedPassiveRewardableSeconds, false);
                CmdClaimPassiveRewards.ExecuteStatic(player);
                if ((progress.NumBossKills > 0) || (progress.NumMinionKills > 0))
                {
                    RewardCeremonyEntry entry;
                    Reward reward5 = new Reward();
                    reward5.CoinDrops.Add(progress.PassiveCoinReward);
                    CmdGainResources.ExecuteStatic(player, ResourceType.Coin, progress.PassiveCoinReward, true, "TRACKING_ID_AFK_COIN_GAIN", null);
                    if (progress.NumBossAdditionalDrops > 0)
                    {
                        entry = ConfigUi.CeremonyEntries.PASSIVE_PROGRESS_4;
                    }
                    else if (progress.NumBossKills > 0)
                    {
                        entry = ConfigUi.CeremonyEntries.PASSIVE_PROGRESS_3;
                    }
                    else if (progress.NumFloorCompletions > 0)
                    {
                        entry = ConfigUi.CeremonyEntries.PASSIVE_PROGRESS_2;
                    }
                    else
                    {
                        entry = ConfigUi.CeremonyEntries.PASSIVE_PROGRESS_1;
                    }
                    string str = _.L(entry.Description, null, false);
                    string description = _.L(entry.Description2, null, false);
                    if (progress.NumBossAdditionalDrops > 0)
                    {
                        description = MenuHelpers.GetFormattedDescriptionColored(description, "$BossAdditionalDropAmount$", progress.NumBossAdditionalDrops);
                    }
                    if (progress.NumBossKills > 0)
                    {
                        description = MenuHelpers.GetFormattedDescriptionColored(MenuHelpers.GetFormattedDescriptionColored(description, "$BossKillAmount$", progress.NumBossKills), "$TokenAmount$", (int) Math.Floor(progress.PassiveTokenReward));
                    }
                    if (progress.NumFloorCompletions > 0)
                    {
                        description = MenuHelpers.GetFormattedDescriptionColored(description, "$FloorAmount$", progress.NumFloorCompletions);
                    }
                    if (progress.NumMinionKills > 0)
                    {
                        description = MenuHelpers.GetFormattedDescriptionColored(description, "$MinionKillAmount$", progress.NumMinionKills);
                    }
                    bool flag = App.Binder.ConfigMeta.NOTIFY_NEW_MISSIONS_DURING_WELCOME_BACK && player.HasUnlockedMissions;
                    parameters13 = new RewardCeremonyMenu.InputParameters();
                    parameters13.Title = StringExtensions.ToUpperLoca(_.L(entry.Title, null, false));
                    parameters13.Description = str;
                    parameters13.Description2 = description;
                    parameters13.SingleRewardOpenAtStart = entry.ChestOpenAtStart;
                    parameters13.SingleReward = reward5;
                    parameters13.NumNewBountiesAvailable = !flag ? 0 : player.Missions.getNumUninspectedMissions();
                    RewardCeremonyMenu.InputParameters parameters4 = parameters13;
                    if (flag)
                    {
                        for (int n = 0; n < player.Missions.Instances.Count; n++)
                        {
                            CmdInspectMission.ExecuteStatic(player, player.Missions.Instances[n]);
                        }
                    }
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters4, 0f, false, true);
                    return true;
                }
            }
            Reward reward6 = player.getFirstUnclaimedReward(ChestType.ServerGift);
            if (reward6 != null)
            {
                CmdConsumeReward.ExecuteStatic(player, reward6, true, "TRACKING_ID_COMMUNITY_RESOURCE_GAIN");
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
                parameters13 = new RewardCeremonyMenu.InputParameters();
                parameters13.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.SERVER_GIFT.Title, null, false));
                parameters13.Description = _.L(ConfigUi.CeremonyEntries.SERVER_GIFT.Description, null, false);
                parameters13.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.SERVER_GIFT.ChestOpenAtStart;
                parameters13.SingleReward = reward6;
                RewardCeremonyMenu.InputParameters parameters5 = parameters13;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters5, 0f, false, true);
                return true;
            }
            if (player.getFirstUnclaimedReward(ChestType.RewardBoxMulti) != null)
            {
                Dictionary<Reward, bool> dictionary2 = new Dictionary<Reward, bool>();
                for (int num3 = player.UnclaimedRewards.Count - 1; num3 >= 0; num3--)
                {
                    Reward reward8 = player.UnclaimedRewards[num3];
                    if (reward8.ChestType == ChestType.RewardBoxMulti)
                    {
                        dictionary2.Add(reward8, false);
                    }
                }
                if (dictionary2.Count > 0)
                {
                    parameters13 = new RewardCeremonyMenu.InputParameters();
                    parameters13.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.SHOP_PURCHASE.Title, null, false));
                    parameters13.Description = _.L(ConfigUi.CeremonyEntries.SHOP_PURCHASE.Description, null, false);
                    parameters13.MultiRewards = dictionary2;
                    parameters13.ConsumeRewardsAfterChestOpen = true;
                    RewardCeremonyMenu.InputParameters parameters6 = parameters13;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters6, 0f, false, true);
                }
                return true;
            }
            Reward reward9 = player.getFirstUnclaimedReward(ChestType.NONE);
            if (reward9 != null)
            {
                CmdConsumeReward.ExecuteStatic(player, reward9, true, string.Empty);
                RewardCeremonyEntry ceremonyEntryForReward = ConfigUi.GetCeremonyEntryForReward(reward9);
                parameters13 = new RewardCeremonyMenu.InputParameters();
                parameters13.Title = StringExtensions.ToUpperLoca(_.L(ceremonyEntryForReward.Title, null, false));
                parameters13.Description = _.L(ceremonyEntryForReward.Description, null, false);
                parameters13.SingleRewardOpenAtStart = ceremonyEntryForReward.ChestOpenAtStart;
                parameters13.SingleReward = reward9;
                RewardCeremonyMenu.InputParameters parameters7 = parameters13;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters7, 0f, false, true);
                return true;
            }
            if ((activeDungeon.ActiveTournament != null) && !activeDungeon.ActiveTournament.EntryAnnounced)
            {
                TournamentView view = Service.Binder.TournamentSystem.GetTournamentView(activeDungeon.ActiveTournament.TournamentId);
                if (view != null)
                {
                    string str3 = _.L(ConfigLoca.BH_ANNOUNCEMENT_WELCOME_DESCRIPTION, null, false) + "\n\n";
                    DungeonRuleset rulesetForId = ConfigDungeonModifiers.GetRulesetForId(view.TournamentInfo.TournamentRulesetId);
                    if ((rulesetForId != null) && (rulesetForId.DungeonModifiers.Count > 0))
                    {
                        for (int num4 = 0; num4 < rulesetForId.DungeonModifiers.Count; num4++)
                        {
                            DungeonModifierType type = rulesetForId.DungeonModifiers[num4];
                            DungeonModifier modifier = ConfigDungeonModifiers.MODIFIERS[type];
                            if (!string.IsNullOrEmpty(modifier.Description))
                            {
                                str3 = str3 + ("- " + _.L(modifier.Description, null, false) + "\n");
                            }
                        }
                    }
                    else
                    {
                        str3 = "- " + _.L(ConfigLoca.BH_ANNOUNCEMENT_NO_ADDED_DIFFICULTY, null, false);
                    }
                    parameters14 = new BannerPopupContent.InputParameters();
                    parameters14.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_ANNOUNCEMENT_WELCOME_TITLE, null, false));
                    parameters14.Shield = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_logo_bosshunt");
                    parameters14.Description = str3;
                    BannerPopupContent.InputParameters parameters8 = parameters14;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.BannerPopupContent, parameters8, 0f, false, true);
                }
                activeDungeon.ActiveTournament.EntryAnnounced = true;
                return true;
            }
            RewardMilestone unclaimedRewardMilestoneWithLowestCompletion = Service.Binder.TournamentSystem.GetUnclaimedRewardMilestoneWithLowestCompletion(out str4);
            if (((unclaimedRewardMilestoneWithLowestCompletion == null) || activeDungeon.WildBossMode) || activeDungeon.ActiveRoom.MainBossSummoned)
            {
                if (this.m_pendingAdventureMilestoneAnnouncement > 0)
                {
                    parameters14 = new BannerPopupContent.InputParameters();
                    parameters14.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_ANNOUNCEMENT_MILESTONE_TITLE, null, false));
                    parameters14.Description = _.L(ConfigLoca.BH_ANNOUNCEMENT_MILESTONE_DESCRIPTION, new <>__AnonType15<int>(this.m_pendingAdventureMilestoneAnnouncement), false);
                    parameters14.Shield = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_logo_adventure");
                    parameters14.FillRewardsFromAdventureMilestoneFloor = this.m_pendingAdventureMilestoneAnnouncement;
                    BannerPopupContent.InputParameters parameters10 = parameters14;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.BannerPopupContent, parameters10, 0f, false, true);
                    this.m_pendingAdventureMilestoneAnnouncement = 0;
                    return true;
                }
                if (string.IsNullOrEmpty(player.Tournaments.LastUnselectedTournamentId) || !player.Tournaments.ActiveInstances.ContainsKey(player.Tournaments.LastUnselectedTournamentId))
                {
                    goto Label_130A;
                }
                instance = player.Tournaments.getInstanceOrNull(player.Tournaments.LastUnselectedTournamentId);
                if ((instance == null) || (((instance.CurrentState != TournamentInstance.State.PENDING_END_ANNOUNCEMENT) && (instance.CurrentState != TournamentInstance.State.ERROR_EXPIRED)) && ((instance.CurrentState != TournamentInstance.State.ERROR_JOIN_TOO_EARLY) && (instance.CurrentState != TournamentInstance.State.ERROR_JOIN_TOO_LATE))))
                {
                    goto Label_130A;
                }
                TournamentView view3 = Service.Binder.TournamentSystem.GetTournamentView(instance.TournamentId);
                if (view3 == null)
                {
                    goto Label_1274;
                }
                if (instance.CurrentState != TournamentInstance.State.PENDING_END_ANNOUNCEMENT)
                {
                    parameters14 = new BannerPopupContent.InputParameters();
                    parameters14.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_ANNOUNCEMENT_JOIN_FAIL_TITLE, null, false));
                    parameters14.Description = _.L(ConfigLoca.BH_ANNOUNCEMENT_JOIN_FAIL_DESCRIPTION, null, false);
                    parameters14.Shield = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_logo_bosshunt");
                    BannerPopupContent.InputParameters parameters12 = parameters14;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.BannerPopupContent, parameters12, 0f, false, true);
                    goto Label_1274;
                }
                num11 = view3.CountNumCompletedMilestones();
                totalContribution = view3.GetTotalContribution();
                contribution = view3.Instance.GetContribution();
                switch (view3.getLeaderboardRanking(view3.PlayerEntry))
                {
                    case 0:
                        str12 = _.L(ConfigLoca.BH_RANKING_GOLD, null, false);
                        goto Label_115C;

                    case 1:
                        str12 = _.L(ConfigLoca.BH_RANKING_SILVER, null, false);
                        goto Label_115C;

                    case 2:
                        str12 = _.L(ConfigLoca.BH_RANKING_BRONZE, null, false);
                        goto Label_115C;

                    case -1:
                        str11 = _.L(ConfigLoca.BH_ANNOUNCEMENT_ENDED_DESCRIPTION2, new <>__AnonType17<int, int, int>(num11, totalContribution, contribution), false);
                        goto Label_1193;
                }
                str12 = _.L(ConfigLoca.BH_RANKING_DEFAULT, null, false);
                goto Label_115C;
            }
            TournamentView tournamentView = Service.Binder.TournamentSystem.GetTournamentView(str4);
            int numCompletedMilestones = tournamentView.Instance.NumMilestonesClaimed + 1;
            for (int i = 0; i < unclaimedRewardMilestoneWithLowestCompletion.CardPackAmount; i++)
            {
                reward13 = new Reward();
                reward13.ChestType = ChestType.TournamentCards;
                reward13.RewardSourceId = tournamentView.TournamentInfo.Id;
                reward13.RewardSource = RewardSourceType.TournamentMilestone;
                reward13.TournamentUpgradeReward = TournamentUpgradeReward.GenerateReward(numCompletedMilestones);
                Reward reward10 = reward13;
                player.UnclaimedRewards.Add(reward10);
            }
            for (int j = 0; j < unclaimedRewardMilestoneWithLowestCompletion.MainReward.Amount; j++)
            {
                reward13 = new Reward();
                reward13.RewardSourceId = tournamentView.TournamentInfo.Id;
                reward13.RewardSource = RewardSourceType.TournamentMilestone;
                Reward reward11 = reward13;
                string shopEntryId = (ConfigShops.GetShopEntry(unclaimedRewardMilestoneWithLowestCompletion.MainReward.Id) == null) ? "PetBoxSmall" : unclaimedRewardMilestoneWithLowestCompletion.MainReward.Id;
                reward11.addShopEntryDrop(GameLogic.Binder.GameState.Player, shopEntryId, false);
                reward11.ChestTypeVisualOverride = reward11.ChestType;
                reward11.ChestType = ChestType.TournamentMainReward;
                player.UnclaimedRewards.Add(reward11);
            }
            int leaderboardRanking = tournamentView.getLeaderboardRanking(tournamentView.PlayerEntry);
            if ((leaderboardRanking != -1) && (leaderboardRanking < unclaimedRewardMilestoneWithLowestCompletion.ContributorRewards.Count))
            {
                RewardMilestone.Entry entry3 = unclaimedRewardMilestoneWithLowestCompletion.ContributorRewards[leaderboardRanking];
                reward13 = new Reward();
                reward13.ChestType = ChestType.TournamentContributorReward;
                reward13.RewardSourceId = tournamentView.TournamentInfo.Id;
                reward13.RewardSource = RewardSourceType.TournamentMilestone;
                Reward reward12 = reward13;
                PetReward reward14 = new PetReward();
                reward14.PetId = entry3.Id;
                reward14.Amount = entry3.Amount;
                reward12.Pets.Add(reward14);
                player.UnclaimedRewards.Add(reward12);
            }
            tournamentView.updateHighestClaimedMilestoneThreshold(unclaimedRewardMilestoneWithLowestCompletion.Threshold);
            TournamentInstance instance1 = tournamentView.Instance;
            instance1.NumMilestonesClaimed++;
            int number = tournamentView.getMilestoneNumber(unclaimedRewardMilestoneWithLowestCompletion);
            if (tournamentView.Instance.NumMilestonesClaimed < tournamentView.getMilestoneCount())
            {
                str6 = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_ANNOUNCEMENT_MILESTONE_TITLE2, new <>__AnonType12<int>(number), false));
            }
            else
            {
                str6 = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_ANNOUNCEMENT_LAST_MILESTONE_TITLE, null, false));
            }
            switch (leaderboardRanking)
            {
                case 0:
                    str8 = _.L(ConfigLoca.BH_RANKING_GOLD, null, false);
                    break;

                case 1:
                    str8 = _.L(ConfigLoca.BH_RANKING_SILVER, null, false);
                    break;

                case 2:
                    str8 = _.L(ConfigLoca.BH_RANKING_BRONZE, null, false);
                    break;

                case -1:
                    str7 = _.L(ConfigLoca.BH_ANNOUNCEMENT_MILESTONE_DESCRIPTION3, new <>__AnonType14<int>(unclaimedRewardMilestoneWithLowestCompletion.Threshold), false);
                    goto Label_0C51;

                default:
                    str8 = _.L(ConfigLoca.BH_RANKING_DEFAULT, null, false);
                    break;
            }
            str7 = _.L(ConfigLoca.BH_ANNOUNCEMENT_MILESTONE_DESCRIPTION2, new <>__AnonType13<int, string>(unclaimedRewardMilestoneWithLowestCompletion.Threshold, str8), false);
        Label_0C51:
            list = new List<BannerPopupContent.RewardContent>();
            string str9 = string.Empty;
            BannerPopupContent.RewardContent item = new BannerPopupContent.RewardContent();
            item.Icon = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_cardpack_floater");
            item.Text = "x" + unclaimedRewardMilestoneWithLowestCompletion.CardPackAmount;
            list.Add(item);
            str9 = str9 + "CardPacks=" + unclaimedRewardMilestoneWithLowestCompletion.CardPackAmount;
            Sprite sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.GetFloaterSpriteForShopEntry(unclaimedRewardMilestoneWithLowestCompletion.MainReward.Id));
            ShopEntry shopEntry = ConfigShops.GetShopEntry(unclaimedRewardMilestoneWithLowestCompletion.MainReward.Id);
            if (shopEntry.Type == ShopEntryType.TokenBundle)
            {
                double v = ConfigShops.CalculateTokenBundleSize(player, shopEntry.Id);
                str10 = "x" + MenuHelpers.BigValueToString(v);
            }
            else
            {
                str10 = "x" + unclaimedRewardMilestoneWithLowestCompletion.MainReward.Amount;
            }
            BannerPopupContent.RewardContent content2 = new BannerPopupContent.RewardContent();
            content2.Icon = sprite;
            content2.Text = str10;
            list.Add(content2);
            object[] objArray1 = new object[] { str9, ";", unclaimedRewardMilestoneWithLowestCompletion.MainReward.Id, "=", unclaimedRewardMilestoneWithLowestCompletion.MainReward.Amount };
            str9 = string.Concat(objArray1);
            if (leaderboardRanking != -1)
            {
                RewardMilestone.Entry entry5 = unclaimedRewardMilestoneWithLowestCompletion.ContributorRewards[leaderboardRanking];
                if (GameLogic.Binder.CharacterResources.hasCharacter(entry5.Id))
                {
                    Character character = GameLogic.Binder.CharacterResources.getResource(entry5.Id);
                    sprite = PlayerView.Binder.SpriteResources.getSprite(character.AvatarSprite);
                    str10 = "x" + entry5.Amount;
                    BannerPopupContent.RewardContent content3 = new BannerPopupContent.RewardContent();
                    content3.Icon = sprite;
                    content3.Text = str10;
                    list.Add(content3);
                    object[] objArray2 = new object[] { str9, ";", entry5.Id, "=", entry5.Amount };
                    str9 = string.Concat(objArray2);
                }
            }
            parameters14 = new BannerPopupContent.InputParameters();
            parameters14.Title = str6;
            parameters14.Shield = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_logo_bosshunt");
            parameters14.Description = str7;
            parameters14.Rewards = list;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate {
                    PlayerView.Binder.DungeonHud.TaskPanel.refreshTaskPanelTournamentMainReward();
                    PlayerView.Binder.DungeonHud.TaskPanel.refreshTaskPanelTournamentContributorReward();
                    PlayerView.Binder.DungeonHud.TaskPanel.refreshTaskPanelTournamentCardPack();
                };
            }
            parameters14.HideCallback = <>f__am$cache3;
            BannerPopupContent.InputParameters parameter = parameters14;
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.BannerPopupContent, parameter, 0f, false, true);
            TournamentLogEvent event3 = new TournamentLogEvent();
            event3.Type = TournamentLogEvent.LogEventType.MileStoneReached;
            event3.Id = "milestone" + unclaimedRewardMilestoneWithLowestCompletion.Threshold;
            event3.AdditionalData = unclaimedRewardMilestoneWithLowestCompletion.Threshold.ToString();
            TournamentLogEvent logEvent = event3;
            Service.Binder.TournamentSystem.RegisterPriorityLogEvent(logEvent, str4);
            Service.Binder.TrackingSystem.sendBossHuntEvent(player, str4, tournamentView.BucketId, leaderboardRanking, "reach_milestone", number, str9);
            return true;
        Label_115C:
            str11 = _.L(ConfigLoca.BH_ANNOUNCEMENT_ENDED_DESCRIPTION1, new <>__AnonType16<int, int, int, string>(num11, totalContribution, contribution, str12), false);
        Label_1193:
            parameters14 = new BannerPopupContent.InputParameters();
            parameters14.Title = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_ANNOUNCEMENT_ENDED_TITLE, null, false));
            parameters14.Shield = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_logo_bosshunt");
            parameters14.Description = str11;
            BannerPopupContent.InputParameters parameters11 = parameters14;
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.BannerPopupContent, parameters11, 0f, false, true);
        Label_1274:
            instance.CurrentState = TournamentInstance.State.CLEARED_FOR_REMOVAL;
            player.Tournaments.CompletedTournamentHistory.Add(instance.TournamentId);
            player.Tournaments.LatestCompletedTournamentStartTime = instance.TournamentView.TournamentInfo.StartTimeEpoch;
            for (int k = player.UnclaimedRewards.Count - 1; k >= 0; k--)
            {
                if (player.UnclaimedRewards[k].TournamentUpgradeReward != null)
                {
                    player.UnclaimedRewards.RemoveAt(k);
                }
            }
            CmdSelectTournament.ExecuteStatic(player, null);
            player.Tournaments.LastUnselectedTournamentId = null;
            return true;
        Label_130A:
            if (App.Binder.AppboyIOSBridge.Bridge.HasForcedMessages())
            {
                App.Binder.AppboyIOSBridge.Bridge.ShowNextMessage();
                return true;
            }
            return false;
        }

        private bool triggerNonBlockingAnnouncement()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            if ((PlayerView.Binder.DungeonHud.MenuButton.Button.interactable && player.hasUnlockedSkill(SkillType.Clone)) && (!player.hasCompletedTutorial("CTUT002") && (PlayerView.Binder.TutorialSystem.ActiveContextTutorialId != "CTUT002")))
            {
                PlayerView.Binder.TutorialSystem.startContextTutorial("CTUT002");
                return true;
            }
            if (((App.Binder.ConfigMeta.CTUT003_ENABLED && !player.hasRetired()) && ((player.getNumMonstersKilled() >= App.Binder.ConfigMeta.CTUT003_REQUIRED_MONSTER_KILLS) && (player.getNumItemUpgrades() <= App.Binder.ConfigMeta.CTUT003_REQUIRED_UPGRADE_COUNT))) && (!player.hasCompletedTutorial("CTUT003") && (PlayerView.Binder.TutorialSystem.ActiveContextTutorialId != "CTUT003")))
            {
                ItemInstance ii = activeCharacter.getEquippedItemOfType(ItemType.Weapon);
                int rank = ii.Rank + 1;
                double num2 = activeCharacter.getAdjustedItemUpgradeCost(ii.Item.Type, player.getRiggedItemLevel(ii), rank);
                if (player.getResourceAmount(ResourceType.Coin) >= num2)
                {
                    PlayerView.Binder.TutorialSystem.startContextTutorial("CTUT003");
                    return true;
                }
            }
            if (((App.Binder.ConfigMeta.CTUT004_ENABLED && !player.hasRetired()) && ((player.getNumMonstersKilled() >= App.Binder.ConfigMeta.CTUT004_REQUIRED_MONSTER_KILLS) && (player.getNumFirstBossSummonCount() <= 0))) && ((!player.hasCompletedTutorial("CTUT004") && (PlayerView.Binder.TutorialSystem.ActiveContextTutorialId != "CTUT004")) && ((activeDungeon.CurrentGameplayState == GameplayState.ACTION) && PlayerView.Binder.DungeonHud.FloorProgressionRibbon.ChallengeButtonRoot.activeInHierarchy)))
            {
                PlayerView.Binder.TutorialSystem.startContextTutorial("CTUT004");
                return true;
            }
            if (!this.m_pendingFloorEntryAnnouncement)
            {
                return false;
            }
            if (GameLogic.Binder.FrenzySystem.isFrenzyActive() || player.BossTrain.Active)
            {
                this.m_pendingFloorEntryAnnouncement = false;
            }
            else if (!PlayerView.Binder.DungeonHud.FloaterText.IsVisible)
            {
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_BannerAnnounceFloor, (float) 0.2f);
                PlayerView.Binder.DungeonHud.AnnouncementBanner.show(StringExtensions.ToUpperLoca(_.L(ConfigLoca.ANNOUNCEMENT_FLOOR, new <>__AnonType15<int>(activeDungeon.Floor), false)), activeDungeon.Dungeon.Name, false, 0f, 1f, null);
                this.m_pendingFloorEntryAnnouncement = false;
            }
            return true;
        }

        private bool triggerTechAnnouncement()
        {
            if ((Service.Binder.PlayerService != null) && !string.IsNullOrEmpty(Service.Binder.PlayerService.PendingClientUpdateFromUrl))
            {
                RequiredClientUpdateContent.InputParameters parameters3 = new RequiredClientUpdateContent.InputParameters();
                parameters3.DownloadUrl = Service.Binder.PlayerService.PendingClientUpdateFromUrl;
                RequiredClientUpdateContent.InputParameters parameter = parameters3;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TechPopupMenu, MenuContentType.RequiredClientUpdateContent, parameter, 0f, false, true);
                Service.Binder.PlayerService.PendingClientUpdateFromUrl = null;
                return true;
            }
            if ((Service.Binder.PlayerService != null) && Service.Binder.PlayerService.IsWaitingForConflictPopup)
            {
                PlayerProgressRestorePopupContent.InputParameters parameters4 = new PlayerProgressRestorePopupContent.InputParameters();
                parameters4.Status = Service.Binder.PlayerService.ConflictStatus;
                parameters4.CurrentLevel = GameLogic.Binder.GameState.Player.Rank;
                parameters4.OtherLevel = Service.Binder.PlayerService.ConflictingPlayerLevel;
                PlayerProgressRestorePopupContent.InputParameters parameters2 = parameters4;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TechPopupMenu, MenuContentType.PlayerProgressRestorePopupContent, parameters2, 0f, false, true);
                return true;
            }
            if (((Service.Binder.ShopManager != null) && (Service.Binder.ShopManager.PendingCount > 0)) && ((Service.Binder.ShopManager.State == ShopManagerState.Available) && !Service.Binder.ShopManager.PendingPopupShowed))
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TechPopupMenu, MenuContentType.PendingPurchasesPopupContent, null, 0f, false, true);
                return true;
            }
            return false;
        }

        protected void Update()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && !App.Binder.AppContext.Splash.IsVisible) && (((activeDungeon.CurrentGameplayState == GameplayState.START_CEREMONY_STEP1) || (activeDungeon.CurrentGameplayState == GameplayState.START_CEREMONY_STEP2)) || (activeDungeon.CurrentGameplayState == GameplayState.ACTION))) && ((!PlayerView.Binder.MenuSystem.InTransition && PlayerView.Binder.InputSystem.InputEnabled) && !GameLogic.Binder.TimeSystem.tutorialSlowdownEnabled()))
            {
                this.PendingBlockingAnnouncements = true;
                MenuType type = PlayerView.Binder.MenuSystem.topmostActiveMenuType();
                if ((((type == MenuType.TechPopupMenu) || !this.triggerTechAnnouncement()) && ((!activeDungeon.isTutorialDungeon() && ((activeDungeon.CurrentGameplayState == GameplayState.START_CEREMONY_STEP2) || (activeDungeon.CurrentGameplayState == GameplayState.ACTION))) && (((type == MenuType.NONE) || (type == MenuType.SlidingInventoryMenu)) || ((type == MenuType.SlidingTaskPanel) || (type == MenuType.SlidingAdventurePanel))))) && !this.triggerBlockingAnnouncement())
                {
                    this.PendingBlockingAnnouncements = false;
                    if (!this.triggerNonBlockingAnnouncement())
                    {
                    }
                }
            }
        }

        public bool PendingBlockingAnnouncements
        {
            [CompilerGenerated]
            get
            {
                return this.<PendingBlockingAnnouncements>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PendingBlockingAnnouncements>k__BackingField = value;
            }
        }
    }
}

