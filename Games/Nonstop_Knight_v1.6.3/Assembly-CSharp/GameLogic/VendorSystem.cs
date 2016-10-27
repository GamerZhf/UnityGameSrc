namespace GameLogic
{
    using App;
    using Service;
    using System;
    using UnityEngine;

    public class VendorSystem : MonoBehaviour, IVendorSystem
    {
        protected void Awake()
        {
        }

        protected void FixedUpdate()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if ((GameLogic.Binder.GameState.Player != null) && ((player.Vendor.Inventory.Count == 0) || (player.Vendor.getSecondsToNextVendorInventoryRefresh() <= 0L)))
            {
                CmdRefreshVendorInventory.ExecuteStatic(player, false);
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnRoomCompleted -= new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            GameLogic.Binder.EventBus.OnSuspectedSystemClockCheat -= new GameLogic.Events.SuspectedSystemClockCheat(this.onSuspectedSystemClockCheat);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnRoomCompleted += new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            GameLogic.Binder.EventBus.OnSuspectedSystemClockCheat += new GameLogic.Events.SuspectedSystemClockCheat(this.onSuspectedSystemClockCheat);
        }

        private void onGameStateInitialized()
        {
            GameLogic.Binder.GameState.Player.Vendor.enforceInventoryLegality();
        }

        private void onRoomCompleted(Room room)
        {
            if ((room.ActiveDungeon.Floor + 1) == App.Binder.ConfigMeta.VENDOR_AND_SHOP_UNLOCK_FLOOR)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (!player.hasRetired())
                {
                    CmdRefreshVendorInventory.ExecuteStatic(player, false);
                }
            }
        }

        private void onSuspectedSystemClockCheat(long timeOffsetSeconds)
        {
            GameLogic.Binder.GameState.Player.Vendor.LastRefreshTimestamp = Service.Binder.ServerTime.GameTime;
        }
    }
}

