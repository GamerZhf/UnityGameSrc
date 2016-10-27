namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Vendor : IJsonData
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        public List<ShopEntryInstance> Inventory;
        public bool InventoryInspected;
        public long LastRefreshTimestamp;

        public Vendor()
        {
            this.Inventory = new List<ShopEntryInstance>();
        }

        public Vendor(Vendor another)
        {
            this.Inventory = new List<ShopEntryInstance>();
            this.copyFrom(another);
        }

        public void copyFrom(Vendor another)
        {
            this.Inventory = new List<ShopEntryInstance>();
            this.LastRefreshTimestamp = another.LastRefreshTimestamp;
            this.InventoryInspected = another.InventoryInspected;
        }

        public void enforceInventoryLegality()
        {
            for (int i = this.Inventory.Count - 1; i >= 0; i--)
            {
                ShopEntryInstance item = this.Inventory[i];
                if (item.ShopEntry == null)
                {
                    this.Inventory.Remove(item);
                }
                else if ((item.ShopEntry.Type == ShopEntryType.SpecialChest) && (item.PrerolledChestType == ChestType.NONE))
                {
                    this.Inventory.Remove(item);
                }
                else if ((item.ShopEntry.Type == ShopEntryType.BossBundle) && !App.Binder.ConfigMeta.BOSS_POTIONS_ENABLED)
                {
                    this.Inventory.Remove(item);
                }
            }
            if (this.Inventory.Count != App.Binder.ConfigMeta.VENDOR_INVENTORY_SIZE)
            {
                CmdRefreshVendorInventory.ExecuteStatic(this.Player, false);
            }
        }

        public ShopEntryInstance getFirstVendorEntry()
        {
            if (this.Inventory.Count > 0)
            {
                return this.Inventory[0];
            }
            return null;
        }

        public int getNumberUnsoldVendorItems()
        {
            int num = 0;
            for (int i = 0; i < this.Inventory.Count; i++)
            {
                if (this.Inventory[i].Sold)
                {
                    num++;
                }
            }
            return num;
        }

        public long getSecondsToNextVendorInventoryRefresh()
        {
            long num = MathUtil.Clamp((long) (Service.Binder.ServerTime.GameTime - this.LastRefreshTimestamp), (long) 0L, (long) 0x7fffffffffffffffL);
            long num2 = App.Binder.ConfigMeta.VENDOR_REFRESH_INTERVAL_SECONDS;
            return MathUtil.Clamp((long) (num2 - num), (long) 0L, (long) 0x7fffffffffffffffL);
        }

        public int getSlotNumber(ShopEntryInstance sei)
        {
            for (int i = 0; i < this.Inventory.Count; i++)
            {
                if (this.Inventory[i] == sei)
                {
                    return (i + 1);
                }
            }
            return -1;
        }

        public void postDeserializeInitialization()
        {
            for (int i = 0; i < this.Inventory.Count; i++)
            {
                this.Inventory[i].postDeserializeInitialization();
            }
        }

        [JsonIgnore]
        public GameLogic.Player Player
        {
            [CompilerGenerated]
            get
            {
                return this.<Player>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Player>k__BackingField = value;
            }
        }
    }
}

