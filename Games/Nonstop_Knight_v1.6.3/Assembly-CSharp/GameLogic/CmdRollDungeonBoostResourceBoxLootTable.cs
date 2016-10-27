namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdRollDungeonBoostResourceBoxLootTable : ICommand
    {
        private LootTable m_lootTable;
        private Player m_player;

        public CmdRollDungeonBoostResourceBoxLootTable(LootTable lootTable, Player player)
        {
            this.m_lootTable = lootTable;
            this.m_player = player;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator63 iterator = new <executeRoutine>c__Iterator63();
            iterator.<>f__this = this;
            return iterator;
        }

        public static string ExecuteStatic(LootTable lootTable, Player player)
        {
            ModifyChestTableWeights(lootTable, player);
            string str = Roll(lootTable, player);
            lootTable.resetWeights(true);
            return str;
        }

        private static void ModifyChestTableWeights(LootTable lootTable, Player player)
        {
            if (!player.tokenBundlesCanDrop())
            {
                for (int i = 0; i < lootTable.Items.Count; i++)
                {
                    LootTableItem lti = lootTable.Items[i];
                    if (ConfigShops.GetVendorShopEntry(lti.Id).Type == ShopEntryType.TokenBundle)
                    {
                        lootTable.setWeightForItem(lti, 0);
                    }
                }
            }
            if (!player.frenzyBundlesCanDrop())
            {
                for (int j = 0; j < lootTable.Items.Count; j++)
                {
                    LootTableItem item2 = lootTable.Items[j];
                    if (ConfigShops.GetVendorShopEntry(item2.Id).Type == ShopEntryType.FrenzyBundle)
                    {
                        lootTable.setWeightForItem(item2, 0);
                    }
                }
            }
            if (!player.xpBundlesCanDrop())
            {
                for (int k = 0; k < lootTable.Items.Count; k++)
                {
                    LootTableItem item3 = lootTable.Items[k];
                    if (ConfigShops.GetVendorShopEntry(item3.Id).Type == ShopEntryType.XpBundle)
                    {
                        lootTable.setWeightForItem(item3, 0);
                    }
                }
            }
            if (!player.bossBundlesCanDrop())
            {
                for (int m = 0; m < lootTable.Items.Count; m++)
                {
                    LootTableItem item4 = lootTable.Items[m];
                    if (ConfigShops.GetVendorShopEntry(item4.Id).Type == ShopEntryType.BossBundle)
                    {
                        lootTable.setWeightForItem(item4, 0);
                    }
                }
            }
        }

        private static string Roll(LootTable lootTable, Player player)
        {
            LootTableItem item = lootTable.roll();
            if (item == null)
            {
                return null;
            }
            if (!ConfigShops.IsVendorShopEntry(item.Id))
            {
                UnityEngine.Debug.LogError("DungeonBoost ResourceBox contents is not a shop entry");
                return null;
            }
            return item.Id;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator63 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRollDungeonBoostResourceBoxLootTable <>f__this;

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
                    CmdRollDungeonBoostResourceBoxLootTable.ExecuteStatic(this.<>f__this.m_lootTable, this.<>f__this.m_player);
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

