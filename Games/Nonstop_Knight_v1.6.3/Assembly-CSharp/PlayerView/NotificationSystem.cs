namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using UnityEngine;

    public class NotificationSystem : MonoBehaviour
    {
        private bool m_notifyAdventurePanelTournamentTab;

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnDropLootTableRolled -= new GameLogic.Events.DropLootTableRolled(this.onDropLootTableRolled);
            GameLogic.Binder.EventBus.OnVendorInventoryRefreshed -= new GameLogic.Events.VendorInventoryRefreshed(this.onVendorInventoryRefreshed);
            GameLogic.Binder.EventBus.OnResourcesGained -= new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            GameLogic.Binder.EventBus.OnTournamentSelected -= new GameLogic.Events.TournamentSelected(this.onTournamentSelected);
        }

        private void onDropLootTableRolled(LootTable lootTable, Vector3 worldPos, Reward drop)
        {
            if (ConfigMeta.IsRetirementChest(drop.ChestType))
            {
                GameLogic.Binder.GameState.Player.Notifiers.HeroRetirementsInspected = false;
            }
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnDropLootTableRolled += new GameLogic.Events.DropLootTableRolled(this.onDropLootTableRolled);
            GameLogic.Binder.EventBus.OnVendorInventoryRefreshed += new GameLogic.Events.VendorInventoryRefreshed(this.onVendorInventoryRefreshed);
            GameLogic.Binder.EventBus.OnResourcesGained += new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            GameLogic.Binder.EventBus.OnTournamentSelected += new GameLogic.Events.TournamentSelected(this.onTournamentSelected);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = 0; i < App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS.Count; i++)
            {
                if (App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS[i].Key == activeDungeon.Floor)
                {
                    player.Notifiers.HeroRetirementsInspected = false;
                    break;
                }
            }
        }

        private void onResourcesGained(Player player, ResourceType type, double amount, bool instant, string analyticsSourceOrSinkId, Vector3? worldPt)
        {
            if (type == ResourceType.Token)
            {
                if (amount < 0.0)
                {
                    player.Notifiers.markAllAugNotificationsThatWeCannotPurchaseAsUninspected();
                }
                else
                {
                    player.Notifiers.refreshAugShopInspectedFlag();
                }
                PlayerView.Binder.DungeonHud.refreshAdventureButton();
            }
        }

        private void onTournamentSelected(Player player, TournamentInstance selectedTournament, TournamentInstance unselectedTournament)
        {
            if (selectedTournament != null)
            {
                player.Tournaments.markTournamentAsNotified(selectedTournament.TournamentId);
            }
        }

        private void onVendorInventoryRefreshed(Player player)
        {
            player.Notifiers.ShopInspected = false;
        }

        protected void Update()
        {
        }

        public bool NotifyAdventurePanelTournamentTab
        {
            get
            {
                return this.m_notifyAdventurePanelTournamentTab;
            }
            set
            {
                this.m_notifyAdventurePanelTournamentTab = value;
                if (PlayerView.Binder.DungeonHud != null)
                {
                    PlayerView.Binder.DungeonHud.refreshAdventureButton();
                }
            }
        }
    }
}

