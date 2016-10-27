namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [ConsoleCommand("vendor")]
    public class CmdRefreshVendorInventory : ICommand
    {
        private Player m_player;
        private bool m_refreshOnlyEmptySlots;

        public CmdRefreshVendorInventory(string[] serialized)
        {
            this.m_player = GameLogic.Binder.GameState.Player;
            this.m_refreshOnlyEmptySlots = false;
        }

        public CmdRefreshVendorInventory(Player player, [Optional, DefaultParameterValue(false)] bool refreshOnlyEmptySlots)
        {
            this.m_player = player;
            this.m_refreshOnlyEmptySlots = refreshOnlyEmptySlots;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorAE rae = new <executeRoutine>c__IteratorAE();
            rae.<>f__this = this;
            return rae;
        }

        public static void ExecuteStatic(Player player, [Optional, DefaultParameterValue(false)] bool refreshOnlyEmptySlots)
        {
            if (refreshOnlyEmptySlots)
            {
                bool flag = false;
                for (int i = 0; i < App.Binder.ConfigMeta.VENDOR_INVENTORY_SIZE; i++)
                {
                    if (player.Vendor.Inventory[i].Sold)
                    {
                        int vendorSlot = i + 1;
                        ShopEntryInstance instance = RollNewVendorEntry(player, vendorSlot, null);
                        if (instance != null)
                        {
                            player.Vendor.Inventory[i] = instance;
                        }
                        flag = true;
                    }
                }
                if (flag)
                {
                    player.Vendor.InventoryInspected = false;
                    GameLogic.Binder.EventBus.VendorInventoryRefreshed(player);
                }
            }
            else
            {
                player.Vendor.Inventory.Clear();
                for (int j = 0; j < App.Binder.ConfigMeta.VENDOR_INVENTORY_SIZE; j++)
                {
                    player.Vendor.Inventory.Add(null);
                }
                int num4 = -1;
                if (App.Binder.ConfigMeta.VENDOR_COIN_BUNDLES_ALWAYS_APPEAR)
                {
                    num4 = UnityEngine.Random.Range(2, 4);
                    ShopEntry fixedShopEntry = null;
                    LootTable vendorLootTable = App.Binder.ConfigLootTables.getLootTableForVendorSlot(num4);
                    if (vendorLootTable != null)
                    {
                        RigCoinBundleLootTableWeights(vendorLootTable, player);
                        LootTableItem item = vendorLootTable.roll();
                        if (item != null)
                        {
                            fixedShopEntry = ConfigShops.GetVendorShopEntry(item.Id);
                        }
                        vendorLootTable.resetWeights(true);
                    }
                    if (fixedShopEntry != null)
                    {
                        ShopEntryInstance instance2 = RollNewVendorEntry(player, num4, fixedShopEntry);
                        if (instance2 != null)
                        {
                            player.Vendor.Inventory[num4 - 1] = instance2;
                        }
                        else
                        {
                            UnityEngine.Debug.LogError("Vendor rigging failed for slot: " + num4);
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Vendor rigging failed for slot: " + num4);
                    }
                }
                for (int k = 0; k < App.Binder.ConfigMeta.VENDOR_INVENTORY_SIZE; k++)
                {
                    int num6 = k + 1;
                    if (num6 != num4)
                    {
                        ShopEntryInstance instance3 = RollNewVendorEntry(player, num6, null);
                        if (instance3 != null)
                        {
                            player.Vendor.Inventory[num6 - 1] = instance3;
                        }
                    }
                }
                player.Vendor.LastRefreshTimestamp = Service.Binder.ServerTime.GameTime;
                player.Vendor.InventoryInspected = false;
                GameLogic.Binder.EventBus.VendorInventoryRefreshed(player);
            }
        }

        private static ShopEntryInstance InstantiateShopEntry(Player player, ShopEntry shopEntry)
        {
            ChestType nONE = ChestType.NONE;
            if ((shopEntry.Type == ShopEntryType.SpecialChest) && (shopEntry.ChestType == ChestType.NONE))
            {
                List<ChestType> list = player.getAvailableSpecialChestTypes();
                if (list.Count == 0)
                {
                    nONE = ChestType.Special003;
                }
                else
                {
                    nONE = LangUtil.GetRandomValueFromList<ChestType>(list);
                }
            }
            return new ShopEntryInstance(shopEntry.Id, nONE);
        }

        private static void ModifyLootTableWeights(LootTable vendorLootTable, Player player)
        {
            if (!player.tokenBundlesCanDrop())
            {
                for (int i = 0; i < vendorLootTable.Items.Count; i++)
                {
                    LootTableItem lti = vendorLootTable.Items[i];
                    if (ConfigShops.GetVendorShopEntry(lti.Id).Type == ShopEntryType.TokenBundle)
                    {
                        vendorLootTable.setWeightForItem(lti, 0);
                    }
                }
            }
            if (!player.frenzyBundlesCanDrop())
            {
                for (int j = 0; j < vendorLootTable.Items.Count; j++)
                {
                    LootTableItem item2 = vendorLootTable.Items[j];
                    if (ConfigShops.GetVendorShopEntry(item2.Id).Type == ShopEntryType.FrenzyBundle)
                    {
                        vendorLootTable.setWeightForItem(item2, 0);
                    }
                }
            }
            if (!player.xpBundlesCanDrop())
            {
                for (int k = 0; k < vendorLootTable.Items.Count; k++)
                {
                    LootTableItem item3 = vendorLootTable.Items[k];
                    if (ConfigShops.GetVendorShopEntry(item3.Id).Type == ShopEntryType.XpBundle)
                    {
                        vendorLootTable.setWeightForItem(item3, 0);
                    }
                }
            }
            if (!player.hasEncounteredSpecialChest())
            {
                for (int m = 0; m < vendorLootTable.Items.Count; m++)
                {
                    LootTableItem item4 = vendorLootTable.Items[m];
                    if (ConfigShops.GetVendorShopEntry(item4.Id).Type == ShopEntryType.SpecialChest)
                    {
                        vendorLootTable.setWeightForItem(item4, 0);
                    }
                }
            }
            if (!player.bossBundlesCanDrop())
            {
                for (int n = 0; n < vendorLootTable.Items.Count; n++)
                {
                    LootTableItem item5 = vendorLootTable.Items[n];
                    if (ConfigShops.GetVendorShopEntry(item5.Id).Type == ShopEntryType.BossBundle)
                    {
                        vendorLootTable.setWeightForItem(item5, 0);
                    }
                }
            }
            if (!player.frenzyBundlesCanDrop())
            {
                for (int num6 = 0; num6 < vendorLootTable.Items.Count; num6++)
                {
                    LootTableItem item6 = vendorLootTable.Items[num6];
                    if (ConfigShops.GetVendorShopEntry(item6.Id).Type == ShopEntryType.FrenzyBundle)
                    {
                        vendorLootTable.setWeightForItem(item6, 0);
                    }
                }
            }
            if (!player.reviveBundlesCanDrop())
            {
                for (int num7 = 0; num7 < vendorLootTable.Items.Count; num7++)
                {
                    LootTableItem item7 = vendorLootTable.Items[num7];
                    if (ConfigShops.GetVendorShopEntry(item7.Id).Type == ShopEntryType.ReviveBundle)
                    {
                        vendorLootTable.setWeightForItem(item7, 0);
                    }
                }
            }
        }

        private static void RigCoinBundleLootTableWeights(LootTable vendorLootTable, Player player)
        {
            for (int i = 0; i < vendorLootTable.Items.Count; i++)
            {
                LootTableItem lti = vendorLootTable.Items[i];
                if (ConfigShops.GetVendorShopEntry(lti.Id).Type != ShopEntryType.CoinBundle)
                {
                    vendorLootTable.setWeightForItem(lti, 0);
                }
            }
        }

        private static ShopEntryInstance RollNewVendorEntry(Player player, int vendorSlot, [Optional, DefaultParameterValue(null)] ShopEntry fixedShopEntry)
        {
            ShopEntryInstance instance = null;
            if (fixedShopEntry != null)
            {
                return InstantiateShopEntry(player, fixedShopEntry);
            }
            int num = 0;
            LootTableItem item = null;
            ShopEntry shopEntry = null;
            LootTable vendorLootTable = App.Binder.ConfigLootTables.getLootTableForVendorSlot(vendorSlot);
            if (vendorLootTable == null)
            {
                UnityEngine.Debug.LogError("Unsupported vendor slot: " + vendorSlot);
                return null;
            }
            ModifyLootTableWeights(vendorLootTable, player);
        Label_004E:
            item = vendorLootTable.roll();
            if (item != null)
            {
                shopEntry = ConfigShops.GetVendorShopEntry(item.Id);
                if (shopEntry != null)
                {
                    if ((shopEntry.Type == ShopEntryType.CoinBundle) && ((((vendorSlot == 2) && (player.Vendor.Inventory.Count > 2)) && ((player.Vendor.Inventory[2] != null) && (player.Vendor.Inventory[2].ShopEntry.Type == ShopEntryType.CoinBundle))) || (((vendorSlot == 3) && (player.Vendor.Inventory.Count > 1)) && ((player.Vendor.Inventory[1] != null) && (player.Vendor.Inventory[1].ShopEntry.Type == ShopEntryType.CoinBundle)))))
                    {
                        goto Label_015E;
                    }
                    instance = InstantiateShopEntry(player, shopEntry);
                }
                else
                {
                    UnityEngine.Debug.LogError("No vendor shop entry found for id: " + item.Id);
                }
            }
            else
            {
                UnityEngine.Debug.LogError("VendorLootTable returned null");
            }
            num++;
        Label_015E:
            if ((instance == null) && (num < 0x3e8))
            {
                goto Label_004E;
            }
            vendorLootTable.resetWeights(true);
            return instance;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorAE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRefreshVendorInventory <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    CmdRefreshVendorInventory.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_refreshOnlyEmptySlots);
                }
                return false;
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
    }
}

