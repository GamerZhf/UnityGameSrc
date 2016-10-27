namespace Service
{
    using App;
    using GameLogic;
    using PlayerView;
    using Service.SupersonicAds;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TrackingSystem : MonoBehaviour
    {
        private int m_pendingFrenzyActivationFloor = -1;
        public const string TRACKING_ID_AFK_COIN_GAIN = "TRACKING_ID_AFK_COIN_GAIN";
        public const string TRACKING_ID_COMMUNITY_RESOURCE_GAIN = "TRACKING_ID_COMMUNITY_RESOURCE_GAIN";
        public const string TRACKING_ID_GAMEPLAY_LOOT_GAIN = "TRACKING_ID_GAMEPLAY_LOOT_GAIN";
        public const string TRACKING_ID_REVIVE = "TRACKING_ID_REVIVE";

        private void injectStandardPayload(Player player, TrackingEvent e)
        {
            e.Payload.Add("ascension_count", player.CumulativeRetiredHeroStats.HeroesRetired);
            e.Payload.Add("player_level", player.Rank);
            e.Payload.Add("floor", player.getLastCompletedFloor(false) + 1);
            e.Payload.Add("gender", player.ActiveCharacter.isFemale());
            e.Payload.Add("balance_diamonds", player.getResourceAmount(ResourceType.Diamond));
            e.Payload.Add("balance_tokens", player.getResourceAmount(ResourceType.Token));
            e.Payload.Add("highest_floor", player.getHighestFloorReached());
            e.Payload.Add("player_name", player.SocialData.Name);
            e.Payload.Add("online_connection", Application.internetReachability != NetworkReachability.NotReachable);
            e.Payload.Add("fb_connected", player.SocialData.FacebookId != null);
            e.Payload.Add("active_boss_hunt", !player.Tournaments.hasTournamentSelected() ? "none" : player.Tournaments.SelectedTournamentId);
        }

        private void onAdWatched(VideoResult result)
        {
            if (result.Success)
            {
                AdsData customData = (AdsData) result.CustomData;
                Player player = GameLogic.Binder.GameState.Player;
                this.sendAdViewEvent(player, customData.AdReward, customData.AdZone, "finish");
                this.sendCrmEvent(player, "crm_ad_view_watch");
            }
        }

        private void onAdWatchStarted(string adZoneId, Reward reward)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.sendAdViewEvent(player, reward, adZoneId, "watch");
        }

        private void onAppboyAction(string action, string campainId)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.sendAppboyActionEvent(player, action, campainId);
        }

        private void onCharacterRevived(CharacterInstance character)
        {
            if (character.IsPrimaryPlayerCharacter)
            {
                Player player = GameLogic.Binder.GameState.Player;
                this.sendCrmEvent(player, "crm_revives_used");
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnPlayerRankUpped -= new GameLogic.Events.PlayerRankUpped(this.onPlayerRankUpped);
            GameLogic.Binder.EventBus.OnTutorialCompleted -= new GameLogic.Events.TutorialCompleted(this.onTutorialCompleted);
            GameLogic.Binder.EventBus.OnRunestoneGained -= new GameLogic.Events.RunestoneGained(this.onRunestoneGained);
            GameLogic.Binder.EventBus.OnPlayerRetired -= new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            GameLogic.Binder.EventBus.OnItemPerksRerolled -= new GameLogic.Events.ItemPerksRerolled(this.onItemPerksRerolled);
            GameLogic.Binder.EventBus.OnItemInstantUpgraded -= new GameLogic.Events.ItemInstantUpgraded(this.onItemInstantUpgraded);
            GameLogic.Binder.EventBus.OnResourcesGained -= new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            GameLogic.Binder.EventBus.OnPlayerAugmentationPurchased -= new GameLogic.Events.PlayerAugmentationPurchased(this.onPlayerAugmentationPurchased);
            GameLogic.Binder.EventBus.OnItemGained -= new GameLogic.Events.ItemGained(this.onItemGained);
            GameLogic.Binder.EventBus.OnItemRankUpped -= new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnPassiveProgress -= new GameLogic.Events.PassiveProgress(this.onPassiveProgress);
            GameLogic.Binder.EventBus.OnRoomCompleted -= new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            GameLogic.Binder.EventBus.OnFrenzyActivated -= new GameLogic.Events.FrenzyActivated(this.onFrenzyActivated);
            GameLogic.Binder.EventBus.OnFrenzyDeactivated -= new GameLogic.Events.FrenzyDeactivated(this.onFrenzyDeactivated);
            GameLogic.Binder.EventBus.OnCharacterRevived -= new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            GameLogic.Binder.EventBus.OnLeaderboardOutperformed -= new GameLogic.Events.LeaderboardOutperformed(this.onLeaderboardOutperformed);
            GameLogic.Binder.EventBus.OnRewardConsumed -= new GameLogic.Events.RewardConsumed(this.onRewardConsumed);
            GameLogic.Binder.EventBus.OnMissionEnded -= new GameLogic.Events.MissionEnded(this.onMissionEnded);
            GameLogic.Binder.EventBus.OnMissionStarted -= new GameLogic.Events.MissionStarted(this.onMissionStarted);
            GameLogic.Binder.EventBus.OnTournamentDonationMade -= new GameLogic.Events.TournamentDonationMade(this.onTournamentDonationMade);
            GameLogic.Binder.EventBus.OnTournamentUpgradeGained -= new GameLogic.Events.TournamentUpgradeGained(this.onTournamentUpgradeGained);
            PlayerView.Binder.EventBus.OnShopEntryPurchased -= new PlayerView.Events.ShopEntryPurchased(this.onShopEntryPurchased);
            PlayerView.Binder.EventBus.OnAdWatchStarted -= new PlayerView.Events.AdWatchStarted(this.onAdWatchStarted);
            PlayerView.Binder.EventBus.OnAdWatched -= new PlayerView.Events.AdWatched(this.onAdWatched);
            PlayerView.Binder.EventBus.OnSpecialOfferAdOffered -= new PlayerView.Events.SpecialOfferAdOffered(this.onSpecialOfferAdOffered);
            PlayerView.Binder.EventBus.OnSpecialOfferAdRejected -= new PlayerView.Events.SpecialOfferAdRejected(this.onSpecialOfferAdRejected);
            PlayerView.Binder.EventBus.OnAppboyAction -= new PlayerView.Events.AppboyAction(this.onAppboyAction);
            Service.Binder.EventBus.OnPromotionAction -= new Service.Events.PromotionAction(this.onPromotionAction);
            Service.Binder.EventBus.OnTournamentCardsReceived -= new Service.Events.TournamentCardsReceived(this.onTournamentCardsReceived);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnPlayerRankUpped += new GameLogic.Events.PlayerRankUpped(this.onPlayerRankUpped);
            GameLogic.Binder.EventBus.OnTutorialCompleted += new GameLogic.Events.TutorialCompleted(this.onTutorialCompleted);
            GameLogic.Binder.EventBus.OnRunestoneGained += new GameLogic.Events.RunestoneGained(this.onRunestoneGained);
            GameLogic.Binder.EventBus.OnPlayerRetired += new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            GameLogic.Binder.EventBus.OnItemPerksRerolled += new GameLogic.Events.ItemPerksRerolled(this.onItemPerksRerolled);
            GameLogic.Binder.EventBus.OnItemInstantUpgraded += new GameLogic.Events.ItemInstantUpgraded(this.onItemInstantUpgraded);
            GameLogic.Binder.EventBus.OnResourcesGained += new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            GameLogic.Binder.EventBus.OnPlayerAugmentationPurchased += new GameLogic.Events.PlayerAugmentationPurchased(this.onPlayerAugmentationPurchased);
            GameLogic.Binder.EventBus.OnItemGained += new GameLogic.Events.ItemGained(this.onItemGained);
            GameLogic.Binder.EventBus.OnItemRankUpped += new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnPassiveProgress += new GameLogic.Events.PassiveProgress(this.onPassiveProgress);
            GameLogic.Binder.EventBus.OnRoomCompleted += new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            GameLogic.Binder.EventBus.OnFrenzyActivated += new GameLogic.Events.FrenzyActivated(this.onFrenzyActivated);
            GameLogic.Binder.EventBus.OnFrenzyDeactivated += new GameLogic.Events.FrenzyDeactivated(this.onFrenzyDeactivated);
            GameLogic.Binder.EventBus.OnCharacterRevived += new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            GameLogic.Binder.EventBus.OnLeaderboardOutperformed += new GameLogic.Events.LeaderboardOutperformed(this.onLeaderboardOutperformed);
            GameLogic.Binder.EventBus.OnRewardConsumed += new GameLogic.Events.RewardConsumed(this.onRewardConsumed);
            GameLogic.Binder.EventBus.OnMissionEnded += new GameLogic.Events.MissionEnded(this.onMissionEnded);
            GameLogic.Binder.EventBus.OnMissionStarted += new GameLogic.Events.MissionStarted(this.onMissionStarted);
            GameLogic.Binder.EventBus.OnTournamentDonationMade += new GameLogic.Events.TournamentDonationMade(this.onTournamentDonationMade);
            GameLogic.Binder.EventBus.OnTournamentUpgradeGained += new GameLogic.Events.TournamentUpgradeGained(this.onTournamentUpgradeGained);
            PlayerView.Binder.EventBus.OnShopEntryPurchased += new PlayerView.Events.ShopEntryPurchased(this.onShopEntryPurchased);
            PlayerView.Binder.EventBus.OnAdWatchStarted += new PlayerView.Events.AdWatchStarted(this.onAdWatchStarted);
            PlayerView.Binder.EventBus.OnAdWatched += new PlayerView.Events.AdWatched(this.onAdWatched);
            PlayerView.Binder.EventBus.OnSpecialOfferAdOffered += new PlayerView.Events.SpecialOfferAdOffered(this.onSpecialOfferAdOffered);
            PlayerView.Binder.EventBus.OnSpecialOfferAdRejected += new PlayerView.Events.SpecialOfferAdRejected(this.onSpecialOfferAdRejected);
            PlayerView.Binder.EventBus.OnAppboyAction += new PlayerView.Events.AppboyAction(this.onAppboyAction);
            Service.Binder.EventBus.OnPromotionAction += new Service.Events.PromotionAction(this.onPromotionAction);
            Service.Binder.EventBus.OnTournamentCardsReceived += new Service.Events.TournamentCardsReceived(this.onTournamentCardsReceived);
        }

        private void onFrenzyActivated()
        {
            this.m_pendingFrenzyActivationFloor = GameLogic.Binder.GameState.ActiveDungeon.Floor;
        }

        private void onFrenzyDeactivated()
        {
            if (this.m_pendingFrenzyActivationFloor != -1)
            {
                Player player = GameLogic.Binder.GameState.Player;
                this.sendFrenzyEndEvent(player, this.m_pendingFrenzyActivationFloor, player.getLastCompletedFloor(false) + 1);
            }
        }

        private void onItemGained(CharacterInstance character, ItemInstance itemInstance, string trackingId)
        {
            if (!string.IsNullOrEmpty(trackingId))
            {
                Player owningPlayer = character.OwningPlayer;
                if (trackingId == "TRACKING_ID_GAMEPLAY_LOOT_GAIN")
                {
                    owningPlayer.TrackingData.ItemsGained++;
                }
            }
        }

        private void onItemInstantUpgraded(ItemInstance itemInstance, double diamondCost)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.sendPurchaseItemEvent(player, itemInstance.ItemId, "instant_upgrade", ResourceType.Diamond, diamondCost, 1, 0);
            this.sendCrmEvent(player, "crm_purchase_item_instupg");
        }

        private void onItemPerksRerolled(Player player, ItemInstance itemInstance, double diamondCost)
        {
            this.sendPurchaseItemEvent(player, itemInstance.ItemId, "reroll", ResourceType.Diamond, diamondCost, 1, 0);
            this.sendCrmEvent(player, "crm_purchase_item_reroll");
        }

        private void onItemRankUpped(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free)
        {
            if (!free)
            {
                Player owningPlayer = character.OwningPlayer;
                switch (itemInstance.Item.Type)
                {
                    case ItemType.Weapon:
                        owningPlayer.TrackingData.WeaponLevelups += rankUpCount;
                        if (owningPlayer.TrackingData.PerSessionWeaponLevelups == 0)
                        {
                            this.sendCrmEvent(owningPlayer, "crm_levelup_weapon");
                        }
                        owningPlayer.TrackingData.PerSessionWeaponLevelups += rankUpCount;
                        break;

                    case ItemType.Armor:
                        owningPlayer.TrackingData.ArmorLevelups += rankUpCount;
                        if (owningPlayer.TrackingData.PerSessionArmorLevelups == 0)
                        {
                            this.sendCrmEvent(owningPlayer, "crm_levelup_armor");
                        }
                        owningPlayer.TrackingData.PerSessionArmorLevelups += rankUpCount;
                        break;

                    case ItemType.Cloak:
                        owningPlayer.TrackingData.CloakLevelups += rankUpCount;
                        if (owningPlayer.TrackingData.PerSessionCloakLevelups == 0)
                        {
                            this.sendCrmEvent(owningPlayer, "crm_levelup_cloak");
                        }
                        owningPlayer.TrackingData.PerSessionCloakLevelups += rankUpCount;
                        break;
                }
            }
        }

        private void onLeaderboardOutperformed(LeaderboardType leaderboardType)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if ((leaderboardType == LeaderboardType.Royal) && !player.TrackingData.PerSessionWasOutperformed)
            {
                player.TrackingData.PerSessionWasOutperformed = true;
                this.sendCrmEvent(player, "crm_leaderboard_beaten");
            }
        }

        private void onMissionEnded(Player player, MissionInstance mission, bool success)
        {
            this.sendMissionEvent("completed", player, mission);
        }

        private void onMissionStarted(Player player, MissionInstance mission)
        {
            this.sendMissionEvent("started", player, mission);
        }

        private void onPassiveProgress(Player player, int numPassiveMinionKills, int numPassiveFloorCompletions, int numPassiveBossKills)
        {
            player.TrackingData.PassiveFloorCompletions += numPassiveFloorCompletions;
        }

        private void onPlayerAugmentationPurchased(Player player, string id, ResourceType spentResource, double price)
        {
            this.sendBuyAugmentEvent(player, id, spentResource, price);
        }

        private void onPlayerRankUpped(Player player, bool cheated)
        {
            Service.Binder.SdkController.Event(ESdkEvent.LevelUp, player.Rank);
        }

        private void onPlayerRetired(Player player, int retirementFloor)
        {
            this.sendPlayerProgressEvent(player);
            this.sendPlayerAscendEvent(player, retirementFloor);
        }

        private void onPromotionAction(string name, string action)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.sendPromotionActionEvent(player, name, action);
        }

        private void onResourcesGained(Player player, ResourceType resourceType, double amount, bool visualizationManuallyControlled, string trackingId, Vector3? worldPt)
        {
            if (!string.IsNullOrEmpty(trackingId))
            {
                if ((trackingId == "TRACKING_ID_AFK_COIN_GAIN") && (amount > 0.0))
                {
                    player.TrackingData.CoinsEarnedPassive += amount;
                }
                else if ((trackingId == "TRACKING_ID_COMMUNITY_RESOURCE_GAIN") && (amount > 0.0))
                {
                    switch (resourceType)
                    {
                        case ResourceType.Coin:
                            this.sendCommunityGrantEvent(player, amount, 0.0, 0.0);
                            break;

                        case ResourceType.Diamond:
                            this.sendCommunityGrantEvent(player, 0.0, amount, 0.0);
                            break;

                        case ResourceType.Token:
                            this.sendCommunityGrantEvent(player, 0.0, 0.0, amount);
                            break;
                    }
                }
                else if ((trackingId == "TRACKING_ID_GAMEPLAY_LOOT_GAIN") && (amount > 0.0))
                {
                    switch (resourceType)
                    {
                        case ResourceType.Coin:
                            player.TrackingData.CoinsEarnedActive += amount;
                            break;

                        case ResourceType.Diamond:
                            player.TrackingData.DiamondsEarned += amount;
                            break;

                        case ResourceType.Token:
                            player.TrackingData.TokensEarned += amount;
                            break;
                    }
                }
                else if ((trackingId == "TRACKING_ID_REVIVE") && (amount < 0.0))
                {
                    double price = -amount;
                    this.sendPurchaseItemEvent(player, string.Empty, "revive", resourceType, price, 1, 0);
                    this.sendCrmEvent(player, "crm_purchase_item_revive");
                }
            }
        }

        private void onRewardConsumed(Player player, Reward reward)
        {
            if (reward.Pets.Count > 0)
            {
                TrackingEvent e = new TrackingEvent("petbox_opened");
                if (reward.Pets.Count > 1)
                {
                    e.Payload.Add("box_type", "mega_pet");
                }
                else
                {
                    e.Payload.Add("box_type", "pet");
                }
                for (int i = 0; i < reward.Pets.Count; i++)
                {
                    e.Payload.Add("reward_" + (i + 1), reward.Pets[i].PetId);
                    e.Payload.Add("reward_" + (i + 1) + "_amount", reward.Pets[i].Amount);
                }
                for (int j = 0; j < player.Pets.Instances.Count; j++)
                {
                    PetInstance instance = player.Pets.Instances[j];
                    e.Payload.Add("pet_" + (j + 1), instance.CharacterId);
                    e.Payload.Add("pet_" + (j + 1) + "_state", instance.Level);
                }
                this.injectStandardPayload(player, e);
                Service.Binder.TrackingService.AddEvent(e);
            }
        }

        private void onRoomCompleted(Room room)
        {
            Player player = GameLogic.Binder.GameState.Player;
            ActiveDungeon activeDungeon = room.ActiveDungeon;
            switch (room.EndCondition)
            {
                case RoomEndCondition.NORMAL_COMPLETION:
                case RoomEndCondition.FRENZY_COMPLETION:
                    if (room.MainBossSummoned)
                    {
                        bool isEliteBoss = activeDungeon.isEliteBossFloor();
                        this.sendBossFightEvent(player, activeDungeon.BossId, isEliteBoss, false, true, room.BossSummonedWith);
                        if (isEliteBoss)
                        {
                            this.sendCrmEvent(player, "crm_elite_boss_beaten");
                        }
                    }
                    player.TrackingData.ActiveFloorCompletions++;
                    this.sendPlayerProgressEvent(player);
                    return;

                case RoomEndCondition.FAIL:
                    if (!room.MainBossSummoned)
                    {
                        player.TrackingData.KilledByMinions++;
                        this.sendKnockedDownEvent(player, "no_boss", Room.BossSummonMethod.UNSPECIFIED);
                        return;
                    }
                    this.sendBossFightEvent(player, activeDungeon.BossId, activeDungeon.isEliteBossFloor(), false, false, room.BossSummonedWith);
                    if (player.TrackingData.LastFailedBossFightFloor != activeDungeon.Floor)
                    {
                        player.TrackingData.LastFailedBossFightFloor = activeDungeon.Floor;
                        player.TrackingData.LostToSameBossInARowCount = 1;
                        break;
                    }
                    player.TrackingData.LostToSameBossInARowCount++;
                    if (player.TrackingData.LostToSameBossInARowCount == 3)
                    {
                        this.sendCrmEvent(player, "crm_bossfight_3lost");
                    }
                    break;

                default:
                    return;
            }
            player.TrackingData.BossFightsLost++;
            this.sendKnockedDownEvent(player, room.BossDifficultyDuringSummon, room.BossSummonedWith);
        }

        private void onRunestoneGained(Player player, RunestoneInstance runestone, bool cheated)
        {
            if (!cheated)
            {
                this.sendRunestoneUnlockEvent(player, runestone.Id);
            }
        }

        private void onShopEntryPurchased(Player player, ShopEntryInstance shopEntryInstance, ShopEntry shopEntry, ResourceType spentResource, double totalPrice, int purchaseCount)
        {
            string shopEntryId;
            string itemCategory = "vendor";
            int repeatPurchase = 0;
            if (shopEntryInstance != null)
            {
                shopEntryId = shopEntryInstance.ShopEntryId;
                repeatPurchase = shopEntryInstance.NumTimesPurchased;
                if (shopEntryInstance.ShopEntry.Type == ShopEntryType.ReviveBundle)
                {
                    this.sendCrmEvent(player, "crm_purchase_item_revive");
                }
            }
            else
            {
                shopEntryId = shopEntry.Id;
                if (shopEntry.Type == ShopEntryType.ReviveBundle)
                {
                    this.sendCrmEvent(player, "crm_purchase_item_revive");
                }
                else if (shopEntry.Type == ShopEntryType.MegaBoxBundle)
                {
                    itemCategory = "megabox";
                    purchaseCount = (int) ConfigShops.CalculateMegaBoxBundleSize(shopEntryId);
                }
            }
            this.sendPurchaseItemEvent(player, shopEntryId, itemCategory, spentResource, totalPrice, purchaseCount, repeatPurchase);
            this.sendCrmEvent(player, "crm_purchase_item_vendor");
        }

        private void onSpecialOfferAdOffered(Reward reward)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.sendAdViewEvent(player, reward, AdsSystem.ADS_DEFAULT_ZONE, "offer");
        }

        private void onSpecialOfferAdRejected(Reward reward)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.sendAdViewEvent(player, reward, AdsSystem.ADS_DEFAULT_ZONE, "decline");
        }

        private void onTournamentCardsReceived(string tournamentId)
        {
            int num;
            string bucketId;
            Player player = GameLogic.Binder.GameState.Player;
            TournamentView tournamentView = Service.Binder.TournamentSystem.GetTournamentView(tournamentId);
            if (tournamentView != null)
            {
                num = tournamentView.getLeaderboardRanking(tournamentView.PlayerEntry);
                bucketId = tournamentView.BucketId;
            }
            else
            {
                num = -1;
                bucketId = string.Empty;
            }
            this.sendBossHuntEvent(player, tournamentId, bucketId, num, "receive_donation", -1, string.Empty);
        }

        private void onTournamentDonationMade(Player player, TournamentInstance tournament, int count, double totalPrice)
        {
            int num;
            string bucketId;
            if (tournament.TournamentView != null)
            {
                num = tournament.TournamentView.getLeaderboardRanking(tournament.TournamentView.PlayerEntry);
                bucketId = tournament.TournamentView.BucketId;
            }
            else
            {
                num = -1;
                bucketId = string.Empty;
            }
            this.sendBossHuntEvent(player, tournament.TournamentId, bucketId, num, "donate", -1, string.Empty);
        }

        private void onTournamentUpgradeGained(Player player, string id, bool epicVersion, int numMilestonesCompleted)
        {
            if (player.Tournaments.hasTournamentSelected())
            {
                int num;
                string bucketId;
                string str3;
                string selectedTournamentId = player.Tournaments.SelectedTournamentId;
                TournamentView tournamentView = Service.Binder.TournamentSystem.GetTournamentView(selectedTournamentId);
                if (tournamentView != null)
                {
                    num = tournamentView.getLeaderboardRanking(tournamentView.PlayerEntry);
                    bucketId = tournamentView.BucketId;
                }
                else
                {
                    num = -1;
                    bucketId = string.Empty;
                }
                if (epicVersion)
                {
                    str3 = id + "_Epic";
                }
                else
                {
                    str3 = id + "_Normal";
                }
                this.sendBossHuntEvent(player, selectedTournamentId, bucketId, num, "get_card", -1, str3);
            }
        }

        private void onTutorialCompleted(Player player, string tutorialId)
        {
            if (tutorialId == ConfigSdk.COMPLETED_TUTORIAL_ID)
            {
                Service.Binder.SdkController.Event(ESdkEvent.Tuturial, null);
            }
            object[] objArray1 = new object[] { "tutorialid", tutorialId };
            TrackingEvent event2 = new TrackingEvent("tutorial_step", objArray1);
            Service.Binder.TrackingService.AddEvent(event2);
        }

        private void sendAdViewEvent(Player player, Reward reward, string adZoneId, string action)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            string itemId = string.Empty;
            if (adZoneId == AdsSystem.ADS_DEFAULT_ZONE)
            {
                str = "mystery_chest";
                if (reward.ItemDrops.Count <= 0)
                {
                    if (reward.CoinDrops.Count <= 0)
                    {
                        if (reward.DiamondDrops.Count <= 0)
                        {
                            if (reward.TokenDrops.Count <= 0)
                            {
                                if (reward.Revives <= 0)
                                {
                                    if (reward.FrenzyPotions <= 0)
                                    {
                                        if (reward.XpPotions <= 0)
                                        {
                                            if (reward.BossPotions <= 0)
                                            {
                                                Debug.LogError("Unsupported mystery_chest content: " + adZoneId);
                                                return;
                                            }
                                            str2 = "mystery_bosspotions";
                                        }
                                        else
                                        {
                                            str2 = "mystery_xppotions";
                                        }
                                    }
                                    else
                                    {
                                        str2 = "mystery_frenzypotions";
                                    }
                                }
                                else
                                {
                                    str2 = "mystery_revives";
                                }
                            }
                            else
                            {
                                str2 = "mystery_tokens";
                            }
                        }
                        else
                        {
                            str2 = "mystery_diamonds";
                        }
                    }
                    else
                    {
                        str2 = "mystery_coins";
                    }
                }
                else
                {
                    str2 = "mystery_item";
                    itemId = reward.ItemDrops[0].ItemId;
                }
            }
            else if (adZoneId == AdsSystem.ADS_VENDOR_ZONE)
            {
                str = "vendor_ad";
                str2 = "vendor";
                if (reward.ShopEntryId != null)
                {
                    itemId = reward.ShopEntryId;
                }
                else
                {
                    itemId = string.Empty;
                }
            }
            else
            {
                Debug.LogError("Unsupported ad zone: " + adZoneId);
                return;
            }
            TrackingEvent e = new TrackingEvent("ad_view");
            e.Payload.Add("action", action);
            e.Payload.Add("ad_type", str);
            e.Payload.Add("reward_id", itemId);
            e.Payload.Add("reward_type", str2);
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
        }

        private void sendAppboyActionEvent(Player player, string action, string campaign)
        {
            TrackingEvent e = new TrackingEvent("appboy");
            this.injectStandardPayload(player, e);
            e.Payload.Add("action", action);
            if (!string.IsNullOrEmpty(campaign))
            {
                e.Payload.Add("campaign", campaign);
            }
            Service.Binder.TrackingService.AddEvent(e);
        }

        public void sendAreaChangeEvent(Player player, string fromAreaId, string toAreaId, string loadState, long loadTime)
        {
            TrackingEvent e = new TrackingEvent("area_change");
            e.Payload.Add("area_id", toAreaId);
            if (!string.IsNullOrEmpty(fromAreaId) && !string.IsNullOrEmpty(toAreaId))
            {
                e.Payload.Add("area_transition", fromAreaId + "-" + toAreaId);
            }
            else
            {
                e.Payload.Add("area_transition", string.Empty);
            }
            e.Payload.Add("load_state", loadState);
            e.Payload.Add("load_time", loadTime);
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
        }

        public void sendBossFightEvent(Player player, string bossCharacterId, bool isEliteBoss, bool isWildBoss, bool won, Room.BossSummonMethod bossSummonMethod)
        {
            if (string.IsNullOrEmpty(bossCharacterId))
            {
                Debug.LogWarning("Trying to send boss_fight event with empty bossCharacterId");
            }
            else
            {
                Character character = GameLogic.Binder.CharacterResources.getResource(bossCharacterId);
                if (character != null)
                {
                    TrackingEvent e = new TrackingEvent("boss_fight");
                    if (isWildBoss)
                    {
                        e.Payload.Add("boss_type", "wild");
                    }
                    else if (isEliteBoss)
                    {
                        e.Payload.Add("boss_type", "elite");
                    }
                    else
                    {
                        e.Payload.Add("boss_type", "normal");
                    }
                    e.Payload.Add("boss_character_id", bossCharacterId);
                    e.Payload.Add("boss_ai", character.BossAiBehaviour.ToString());
                    if ((character.BossAiParameters != null) && (character.BossAiParameters.Length > 0))
                    {
                        e.Payload.Add("boss_ai_param1", character.BossAiParameters[0]);
                    }
                    else
                    {
                        e.Payload.Add("boss_ai_param1", string.Empty);
                    }
                    if ((character.BossAiParameters != null) && (character.BossAiParameters.Length > 1))
                    {
                        e.Payload.Add("boss_ai_param2", character.BossAiParameters[1]);
                    }
                    else
                    {
                        e.Payload.Add("boss_ai_param2", string.Empty);
                    }
                    if ((character.BossAiParameters != null) && (character.BossAiParameters.Length > 2))
                    {
                        e.Payload.Add("boss_ai_param3", character.BossAiParameters[2]);
                    }
                    else
                    {
                        e.Payload.Add("boss_ai_param3", string.Empty);
                    }
                    PerkType bossPerkType = ConfigPerks.GetBossPerkType(player, bossCharacterId);
                    if (isEliteBoss && (bossPerkType != PerkType.NONE))
                    {
                        e.Payload.Add("boss_perk", bossPerkType.ToString());
                    }
                    else
                    {
                        e.Payload.Add("boss_perk", string.Empty);
                    }
                    e.Payload.Add("boss_fight_won", won);
                    e.Payload.Add("summon_method", bossSummonMethod.ToString());
                    e.Payload.Add("frenzy_summon", bossSummonMethod == Room.BossSummonMethod.Frenzy);
                    this.injectStandardPayload(player, e);
                    Service.Binder.TrackingService.AddEvent(e);
                }
            }
        }

        public void sendBossHuntEvent(Player player, string bossHuntId, string bucketId, int leaderboardRanking, string action, int milestone, string reward)
        {
            TrackingEvent e = new TrackingEvent("boss_hunt");
            e.Payload.Add("boss_hunt_id", bossHuntId);
            e.Payload.Add("bucket_id", bucketId);
            e.Payload.Add("leaderboard_ranking", leaderboardRanking);
            e.Payload.Add("action", action);
            e.Payload.Add("milestone_number", milestone);
            e.Payload.Add("reward", reward);
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
        }

        private void sendBuyAugmentEvent(Player player, string itemId, ResourceType spentResource, double price)
        {
            TrackingEvent e = new TrackingEvent("buy_augment");
            e.Payload.Add("item_id", itemId);
            double num = (spentResource != ResourceType.Token) ? 0.0 : price;
            if (num == 0.0)
            {
                Debug.LogError("No tokens spent for buy_augment event: " + itemId);
            }
            else
            {
                e.Payload.Add("tokens_spent", num);
                this.injectStandardPayload(player, e);
                Service.Binder.TrackingService.AddEvent(e);
            }
        }

        private void sendCommunityGrantEvent(Player player, double coinsEarned, double diamondsEarned, double tokensEarned)
        {
            TrackingEvent e = new TrackingEvent("community_grant");
            e.Payload.Add("coins_earned", coinsEarned);
            e.Payload.Add("diamonds_earned", diamondsEarned);
            e.Payload.Add("tokens_earned", tokensEarned);
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
        }

        public void sendCrmEvent(Player player, string eventId)
        {
            TrackingEvent e = new TrackingEvent(eventId);
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
        }

        public void sendFacebookEvent(Player player, string action, string flow, string context)
        {
            TrackingEvent e = new TrackingEvent("facebook");
            e.Payload.Add("flow", flow);
            e.Payload.Add("action", action);
            e.Payload.Add("context", context);
            if (player != null)
            {
                this.injectStandardPayload(player, e);
            }
            Service.Binder.TrackingService.AddEvent(e);
        }

        private void sendFrenzyEndEvent(Player player, int startFloor, int endFloor)
        {
            TrackingEvent e = new TrackingEvent("frenzy_end");
            e.Payload.Add("start_floor", startFloor);
            e.Payload.Add("end_floor", endFloor);
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
            this.m_pendingFrenzyActivationFloor = -1;
        }

        public void sendGameLoadEvent(Player player, string loadState, long loadTime)
        {
            TrackingEvent e = new TrackingEvent("game_load");
            e.Payload.Add("load_state", loadState);
            e.Payload.Add("load_time", loadTime);
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
        }

        private void sendKnockedDownEvent(Player player, string difficultyIndicator, Room.BossSummonMethod bossSummonMethod)
        {
            TrackingEvent e = new TrackingEvent("knocked_down");
            e.Payload.Add("difficulty_indicator", difficultyIndicator);
            e.Payload.Add("frenzy_summon", bossSummonMethod == Room.BossSummonMethod.Frenzy);
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
        }

        private void sendMissionEvent(string state, Player player, MissionInstance mission)
        {
            TrackingEvent e = new TrackingEvent("bounty_quest");
            e.Payload.Add("quest_state", state);
            e.Payload.Add("quest_code", mission.MissionId);
            e.Payload.Add("quest_goal", mission.Requirement);
            if (mission.MissionType == MissionType.PromotionEvent)
            {
                foreach (KeyValuePair<string, PromotionEventInstance> pair in player.PromotionEvents.Instances)
                {
                    string key = pair.Key;
                    if (pair.Value.Missions.Instances.Contains(mission))
                    {
                        e.Payload.Add("quest_event_id", key);
                        break;
                    }
                }
            }
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
        }

        private void sendPlayerAscendEvent(Player player, int retirementFloor)
        {
            TrackingEvent e = new TrackingEvent("ascend");
            Reward reward = player.getFirstUnclaimedRetirementTriggerChest();
            double num = 0.0;
            if (reward != null)
            {
                num = reward.getTotalTokenAmount();
            }
            e.Payload.Add("tokens_claimed", num);
            e.Payload.Add("num_box_common_claimed", player.getNumberOfUnclaimedChestsOfType(ChestType.RewardBoxCommon));
            e.Payload.Add("num_box_rare_claimed", player.getNumberOfUnclaimedChestsOfType(ChestType.RewardBoxRare));
            e.Payload.Add("num_box_epic_claimed", player.getNumberOfUnclaimedChestsOfType(ChestType.RewardBoxEpic));
            this.injectStandardPayload(player, e);
            e.Payload["floor"] = retirementFloor;
            Service.Binder.TrackingService.AddEvent(e);
        }

        private void sendPlayerProgressEvent(Player player)
        {
            TrackingEvent e = new TrackingEvent("player_progress");
            e.Payload.Add("tokens_earned", player.TrackingData.TokensEarned);
            e.Payload.Add("diamonds_earned", player.TrackingData.DiamondsEarned);
            e.Payload.Add("coins_earned_active", player.TrackingData.CoinsEarnedActive);
            e.Payload.Add("coins_earned_passive", player.TrackingData.CoinsEarnedPassive);
            e.Payload.Add("items_gained", player.TrackingData.ItemsGained);
            e.Payload.Add("weapon_levelups", player.TrackingData.WeaponLevelups);
            e.Payload.Add("armor_levelups", player.TrackingData.ArmorLevelups);
            e.Payload.Add("cloak_levelups", player.TrackingData.CloakLevelups);
            e.Payload.Add("killed_by_minions", player.TrackingData.KilledByMinions);
            e.Payload.Add("boss_fights_lost", player.TrackingData.BossFightsLost);
            e.Payload.Add("active_floor_completions", player.TrackingData.ActiveFloorCompletions);
            e.Payload.Add("passive_floor_completions", player.TrackingData.PassiveFloorCompletions);
            e.Payload.Add("free_revives_used", player.TrackingData.FreeRevivesUsed);
            PetInstance instance = player.Pets.getSelectedPetInstance();
            if (instance != null)
            {
                e.Payload.Add("active_pet", instance.CharacterId);
                e.Payload.Add("active_pet_level", instance.Level);
            }
            e.Payload.Add("boss_auto_summon_enabled", player.Preferences.AutoSummonBosses);
            e.Payload.Add("combat_stats_enabled", player.Preferences.CombatStatsEnabled);
            e.Payload.Add("main_menu_opens_top", player.TrackingData.NumMainMenuOpensTopButton);
            e.Payload.Add("main_menu_opens_arrow", player.TrackingData.NumMainMenuOpensArrowButton);
            e.Payload.Add("main_menu_opens_swipe", player.TrackingData.NumMainMenuOpensSwipe);
            for (int i = 0; i < ConfigSkills.SkillGroupCount; i++)
            {
                string str3;
                string key = "skill_" + (i + 1);
                SkillInstance instance2 = player.ActiveCharacter.getActiveSkillInstanceForGroup(i);
                if (instance2 == null)
                {
                    goto Label_03DE;
                }
                string str2 = string.Empty;
                SkillType skillType = instance2.SkillType;
                switch (skillType)
                {
                    case SkillType.Omnislash:
                        str2 = "slash";
                        goto Label_036F;

                    case SkillType.Slam:
                        str2 = "slam";
                        goto Label_036F;

                    case SkillType.Implosion:
                        str2 = "void";
                        goto Label_036F;

                    case SkillType.Clone:
                        str2 = "clone";
                        goto Label_036F;

                    default:
                        if (skillType != SkillType.Leap)
                        {
                            if (skillType != SkillType.Whirlwind)
                            {
                                break;
                            }
                            str2 = "whirl";
                        }
                        else
                        {
                            str2 = "leap";
                        }
                        goto Label_036F;
                }
                str2 = "changeme";
                Debug.LogError("Unsupported skill type: " + instance2.SkillType);
            Label_036F:
                str3 = player.Runestones.getSelectedRunestoneId(instance2.SkillType, RunestoneSelectionSource.Player);
                if (str3 != null)
                {
                    int num2 = ConfigRunestones.GetRunestoneOrderNumberForSkillType(str3, instance2.SkillType) + 2;
                    e.Payload.Add(key, str2 + "_" + num2);
                }
                else
                {
                    e.Payload.Add(key, str2 + "_1");
                }
                continue;
            Label_03DE:
                e.Payload.Add(key, string.Empty);
            }
            if ((player.TrackingData.FpsTotalFrames >= 5L) && (player.TrackingData.FpsTotalSeconds >= 5f))
            {
                e.Payload.Add("fps", (int) (player.TrackingData.FpsTotalFrames / ((long) player.TrackingData.FpsTotalSeconds)));
            }
            else
            {
                e.Payload.Add("fps", 0);
            }
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
            player.TrackingData.clearPlayerProgressFields();
        }

        private void sendPromotionActionEvent(Player player, string name, string action)
        {
            TrackingEvent e = new TrackingEvent("promotion");
            this.injectStandardPayload(player, e);
            e.Payload.Add("name", name);
            e.Payload.Add("action", action);
            Service.Binder.TrackingService.AddEvent(e);
        }

        public void sendPurchaseItemEvent(Player player, string itemId, string itemCategory, ResourceType spentResource, double price, int purchaseCount, [Optional, DefaultParameterValue(0)] int repeatPurchase)
        {
            TrackingEvent e = new TrackingEvent("purchase_item");
            e.Payload.Add("item_id", itemId);
            e.Payload.Add("item_category", itemCategory);
            e.Payload.Add("action", "instant");
            double num = (spentResource != ResourceType.Diamond) ? 0.0 : price;
            if (num == 0.0)
            {
                Debug.LogError("No diamonds spent for purchase_item event: " + itemId + " - " + itemCategory);
            }
            else
            {
                e.Payload.Add("diamonds_spent", num);
                e.Payload.Add("purchase_count", purchaseCount);
                e.Payload.Add("repeat_purchase", repeatPurchase);
                this.injectStandardPayload(player, e);
                Service.Binder.TrackingService.AddEvent(e);
            }
        }

        public void sendRateGameEvent(Player player, string action)
        {
            TrackingEvent e = new TrackingEvent("rate_game");
            e.Payload.Add("action", action);
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
        }

        private void sendRunestoneUnlockEvent(Player player, string runestoneId)
        {
            TrackingEvent e = new TrackingEvent("runestone_unlock");
            e.Payload.Add("runestone_id", runestoneId);
            e.Payload.Add("unique_runestones", player.Runestones.numRunestonesOwned());
            this.injectStandardPayload(player, e);
            Service.Binder.TrackingService.AddEvent(e);
        }

        protected void Update()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            if (((activeDungeon != null) && (player != null)) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                player.TrackingData.FpsTotalFrames += 1L;
                player.TrackingData.FpsTotalSeconds += Time.unscaledDeltaTime;
            }
        }
    }
}

