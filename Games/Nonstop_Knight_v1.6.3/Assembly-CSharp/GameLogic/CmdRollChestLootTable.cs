namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CmdRollChestLootTable : ICommand
    {
        private ChestType m_chestType;
        private List<ShopEntryType> m_disallowedShopEntryTypes;
        private bool m_isBossDrop;
        private Player m_player;
        private Reward m_reward;
        private static List<Item> m_tempCandidateList = new List<Item>(0x20);

        public CmdRollChestLootTable(ChestType chestType, Player player, bool isBossDrop, ref Reward reward, [Optional, DefaultParameterValue(null)] List<ShopEntryType> disallowedShopEntryTypes)
        {
            this.m_chestType = chestType;
            this.m_player = player;
            this.m_isBossDrop = isBossDrop;
            this.m_reward = reward;
            this.m_disallowedShopEntryTypes = disallowedShopEntryTypes;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator60 iterator = new <executeRoutine>c__Iterator60();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(ChestType chestType, Player player, bool isBossDrop, ref Reward reward, [Optional, DefaultParameterValue(null)] List<ShopEntryType> disallowedShopEntryTypes)
        {
            if (chestType == ChestType.NONE)
            {
                UnityEngine.Debug.LogError("Trying to ChestTableRoll with ChestType.NONE");
            }
            else
            {
                int chestTypeNumRolls = ConfigLootTables.GetChestTypeNumRolls(chestType);
                for (int i = 0; i < chestTypeNumRolls; i++)
                {
                    LootTable chestLootTable = App.Binder.ConfigLootTables.getLootTableForChestType(chestType, i);
                    ModifyChestTableWeights(chestLootTable, player, chestType, isBossDrop, disallowedShopEntryTypes);
                    Roll(chestType, player, chestLootTable, isBossDrop, ref reward);
                    chestLootTable.resetWeights(true);
                }
            }
        }

        private static void ModifyChestTableWeights(LootTable chestLootTable, Player player, ChestType chestType, bool isBossDrop, [Optional, DefaultParameterValue(null)] List<ShopEntryType> disallowedShopEntryTypes)
        {
            CharacterInstance activeCharacter = player.ActiveCharacter;
            if ((chestType == ChestType.Basic001) && !player.hasCompletedTutorial("TUT052A"))
            {
                bool flag = false;
                for (int k = 0; k < chestLootTable.Items.Count; k++)
                {
                    LootTableItem lti = chestLootTable.Items[k];
                    if (lti.Id == ConfigTutorials.FIXED_TUTORIAL_CLOAK_ITEM_ID)
                    {
                        flag = true;
                        chestLootTable.setWeightForItem(lti, 100);
                    }
                    else
                    {
                        chestLootTable.setWeightForItem(lti, 0);
                    }
                }
                if (flag)
                {
                    return;
                }
                UnityEngine.Debug.LogError("Failed to rig Basic001 chest loot table, continuing normally..");
                chestLootTable.resetWeights(true);
            }
            float num2 = activeCharacter.getGenericModifierForPerkType(PerkType.LuckItem);
            for (int i = 0; i < chestLootTable.Items.Count; i++)
            {
                LootTableItem item2 = chestLootTable.Items[i];
                Item item3 = GameLogic.Binder.ItemResources.getItemForLootTableRollId(item2.Id, ItemType.UNSPECIFIED);
                if (item3 != null)
                {
                    if ((num2 > 0f) && (item3.Rarity >= 2))
                    {
                        int weight = item2.Weight + ((int) (item2.Weight * num2));
                        chestLootTable.setWeightForItem(item2, weight);
                    }
                    if (GameLogic.Binder.ItemResources.itemExists(item2.Id) && !player.itemTypeCanDrop(item3.Type, false))
                    {
                        chestLootTable.setWeightForItem(item2, 0);
                    }
                    if (!player.isItemRarityAllowed(item3.Rarity, player.getCurrentFloor(false), isBossDrop))
                    {
                        chestLootTable.setWeightForItem(item2, 0);
                    }
                }
            }
            ItemType type = player.nextItemDropTypeIsForced();
            if (type != ItemType.UNSPECIFIED)
            {
                for (int m = 0; m < chestLootTable.Items.Count; m++)
                {
                    LootTableItem item4 = chestLootTable.Items[m];
                    if (GameLogic.Binder.ItemResources.itemExists(item4.Id) && (GameLogic.Binder.ItemResources.getItemForLootTableRollId(item4.Id, ItemType.UNSPECIFIED).Type != type))
                    {
                        chestLootTable.setWeightForItem(item4, 0);
                    }
                }
            }
            for (int j = 0; j < chestLootTable.Items.Count; j++)
            {
                LootTableItem item6 = chestLootTable.Items[j];
                ConfigRunestones.SharedData runestoneForLootTableRollId = ConfigRunestones.GetRunestoneForLootTableRollId(item6.Id);
                if ((runestoneForLootTableRollId != null) && player.Runestones.ownsAllRunestonesWithRarity(runestoneForLootTableRollId.Rarity))
                {
                    chestLootTable.setWeightForItem(item6, 0);
                }
            }
            if (!player.diamondsCanDrop())
            {
                for (int n = 0; n < chestLootTable.Items.Count; n++)
                {
                    LootTableItem item7 = chestLootTable.Items[n];
                    if (ConfigShops.IsVendorShopEntry(item7.Id) && (ConfigShops.GetShopEntry(item7.Id).Type == ShopEntryType.DiamondBundle))
                    {
                        chestLootTable.setWeightForItem(item7, 0);
                    }
                }
            }
            if (!player.tokenBundlesCanDrop())
            {
                for (int num8 = 0; num8 < chestLootTable.Items.Count; num8++)
                {
                    LootTableItem item8 = chestLootTable.Items[num8];
                    if (ConfigShops.IsVendorShopEntry(item8.Id) && (ConfigShops.GetShopEntry(item8.Id).Type == ShopEntryType.TokenBundle))
                    {
                        chestLootTable.setWeightForItem(item8, 0);
                    }
                }
            }
            if (!player.frenzyBundlesCanDrop())
            {
                for (int num9 = 0; num9 < chestLootTable.Items.Count; num9++)
                {
                    LootTableItem item9 = chestLootTable.Items[num9];
                    if (ConfigShops.IsVendorShopEntry(item9.Id) && (ConfigShops.GetShopEntry(item9.Id).Type == ShopEntryType.FrenzyBundle))
                    {
                        chestLootTable.setWeightForItem(item9, 0);
                    }
                }
            }
            if (!player.bossBundlesCanDrop())
            {
                for (int num10 = 0; num10 < chestLootTable.Items.Count; num10++)
                {
                    LootTableItem item10 = chestLootTable.Items[num10];
                    if (ConfigShops.IsVendorShopEntry(item10.Id) && (ConfigShops.GetShopEntry(item10.Id).Type == ShopEntryType.BossBundle))
                    {
                        chestLootTable.setWeightForItem(item10, 0);
                    }
                }
            }
            if (!player.reviveBundlesCanDrop())
            {
                for (int num11 = 0; num11 < chestLootTable.Items.Count; num11++)
                {
                    LootTableItem item11 = chestLootTable.Items[num11];
                    if (ConfigShops.IsVendorShopEntry(item11.Id) && (ConfigShops.GetShopEntry(item11.Id).Type == ShopEntryType.ReviveBundle))
                    {
                        chestLootTable.setWeightForItem(item11, 0);
                    }
                }
            }
            if (!player.xpBundlesCanDrop())
            {
                for (int num12 = 0; num12 < chestLootTable.Items.Count; num12++)
                {
                    LootTableItem item12 = chestLootTable.Items[num12];
                    if (ConfigShops.IsVendorShopEntry(item12.Id) && (ConfigShops.GetShopEntry(item12.Id).Type == ShopEntryType.XpBundle))
                    {
                        chestLootTable.setWeightForItem(item12, 0);
                    }
                }
            }
            if (disallowedShopEntryTypes != null)
            {
                for (int num13 = 0; num13 < chestLootTable.Items.Count; num13++)
                {
                    LootTableItem item13 = chestLootTable.Items[num13];
                    if (ConfigShops.IsVendorShopEntry(item13.Id))
                    {
                        ShopEntry shopEntry = ConfigShops.GetShopEntry(item13.Id);
                        for (int num14 = 0; num14 < disallowedShopEntryTypes.Count; num14++)
                        {
                            if (((ShopEntryType) disallowedShopEntryTypes[num14]) == shopEntry.Type)
                            {
                                chestLootTable.setWeightForItem(item13, 0);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static void Roll(ChestType chestType, Player player, LootTable lootTable, bool isBossDrop, ref Reward reward)
        {
            int floor = player.getLastCompletedFloor(false) + 1;
            LootTableItem item = lootTable.roll();
            if (item == null)
            {
                Item randomValueFromList = null;
                m_tempCandidateList.Clear();
                for (int i = 0; i < lootTable.Items.Count; i++)
                {
                    LootTableItem item3 = lootTable.Items[i];
                    randomValueFromList = GameLogic.Binder.ItemResources.getItemForLootTableRollId(item3.Id, ItemType.UNSPECIFIED);
                    if (((randomValueFromList != null) && player.isItemRarityAllowed(randomValueFromList.Rarity, floor, isBossDrop)) && player.itemTypeCanDrop(randomValueFromList.Type, true))
                    {
                        m_tempCandidateList.Add(randomValueFromList);
                    }
                }
                if (m_tempCandidateList.Count > 0)
                {
                    randomValueFromList = LangUtil.GetRandomValueFromList<Item>(m_tempCandidateList);
                }
                if (randomValueFromList == null)
                {
                    m_tempCandidateList.Clear();
                    for (int j = 0; j < lootTable.Items.Count; j++)
                    {
                        LootTableItem item4 = lootTable.Items[j];
                        randomValueFromList = GameLogic.Binder.ItemResources.getItemForLootTableRollId(item4.Id, ItemType.UNSPECIFIED);
                        if (randomValueFromList != null)
                        {
                            m_tempCandidateList.Add(randomValueFromList);
                        }
                    }
                    if (m_tempCandidateList.Count > 0)
                    {
                        randomValueFromList = LangUtil.GetRandomValueFromList<Item>(m_tempCandidateList);
                    }
                }
                if (randomValueFromList == null)
                {
                    int rarity = player.clampItemRarityToMaxAllowed(UnityEngine.Random.Range(1, ConfigMeta.ITEM_HIGHEST_RARITY + 1), floor, isBossDrop);
                    randomValueFromList = GameLogic.Binder.ItemResources.getRandomItemOfRarity(rarity, ItemType.UNSPECIFIED);
                    UnityEngine.Debug.LogWarning("All chest table weights were 0 and we had to fallback to the last resort: " + chestType);
                }
                int startRank = -1;
                if (ConfigMeta.IsRetirementChest(chestType))
                {
                    startRank = 0;
                }
                ItemInstance instance = ItemInstance.Create(randomValueFromList, player, startRank);
                reward.ItemDrops.Add(instance);
                player.updateItemRollHistory(instance);
            }
            else if (item.Id != "NO_DROP")
            {
                if (ConfigShops.IsVendorShopEntry(item.Id))
                {
                    reward.addShopEntryDrop(player, item.Id, App.Binder.ConfigMeta.MYSTERYCHEST_DIMINISHINGCOINS);
                }
                else
                {
                    ConfigRunestones.SharedData runestoneForLootTableRollId = ConfigRunestones.GetRunestoneForLootTableRollId(item.Id);
                    if (runestoneForLootTableRollId != null)
                    {
                        int num6 = runestoneForLootTableRollId.Rarity;
                        while (player.Runestones.ownsRunestone(runestoneForLootTableRollId.Id) && !player.Runestones.ownsAllRunestonesWithRarity(num6))
                        {
                            runestoneForLootTableRollId = ConfigRunestones.GetRandomRunestoneOfRarity(num6);
                        }
                        reward.RunestoneDrops.Add(runestoneForLootTableRollId.Id);
                    }
                    else if (GameLogic.Binder.ItemResources.itemExists(item.Id))
                    {
                        Item item5 = GameLogic.Binder.ItemResources.getItemForLootTableRollId(item.Id, ItemType.UNSPECIFIED);
                        int num7 = -1;
                        if (!player.hasCompletedTutorial("TUT052A") && (item5.Id == ConfigTutorials.FIXED_TUTORIAL_CLOAK_ITEM_ID))
                        {
                            num7 = 5;
                        }
                        ItemInstance instance2 = ItemInstance.Create(item5, player, num7);
                        reward.ItemDrops.Add(instance2);
                        player.updateItemRollHistory(instance2);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("No item found for chest table roll: " + item.Id);
                    }
                }
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator60 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRollChestLootTable <>f__this;

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
                    CmdRollChestLootTable.ExecuteStatic(this.<>f__this.m_chestType, this.<>f__this.m_player, this.<>f__this.m_isBossDrop, ref this.<>f__this.m_reward, this.<>f__this.m_disallowedShopEntryTypes);
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

