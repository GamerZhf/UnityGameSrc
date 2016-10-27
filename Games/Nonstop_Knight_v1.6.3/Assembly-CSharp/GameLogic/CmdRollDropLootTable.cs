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

    public class CmdRollDropLootTable : ICommand
    {
        private LootTable m_dropLootTable;
        private GameLogic.CharacterType m_killedCharacterType;
        private Player m_player;
        private string m_predefinedDropRollId;
        private Vector3 m_worldPos;

        public CmdRollDropLootTable(LootTable dropLootTable, Player player, Vector3 worldPos, GameLogic.CharacterType killedCharacterType, [Optional, DefaultParameterValue(null)] string predefinedDropRollId)
        {
            this.m_dropLootTable = dropLootTable;
            this.m_player = player;
            this.m_worldPos = worldPos;
            this.m_killedCharacterType = killedCharacterType;
            this.m_predefinedDropRollId = predefinedDropRollId;
        }

        private static ChestType DropTableRoll(LootTable dropLootTable, Player player, string predefinedDropRollId)
        {
            LootTableItem item;
            ChestType nONE = ChestType.NONE;
            ModifyDropTableWeights(dropLootTable, player);
            if (!string.IsNullOrEmpty(predefinedDropRollId))
            {
                LootTableItem item2 = new LootTableItem();
                item2.Id = predefinedDropRollId;
                item = item2;
            }
            else
            {
                item = dropLootTable.roll();
            }
            if ((item != null) && (item.Id != "NO_DROP"))
            {
                try
                {
                    nONE = (ChestType) ((int) Enum.Parse(typeof(ChestType), item.Id));
                }
                catch (Exception)
                {
                    UnityEngine.Debug.LogError("Cannot parse ChestType enum from boss chest loot table roll: " + item.Id);
                }
            }
            dropLootTable.resetWeights(true);
            return nONE;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator61 iterator = new <executeRoutine>c__Iterator61();
            iterator.<>f__this = this;
            return iterator;
        }

        public static Reward ExecuteStatic(LootTable dropLootTable, Player player, Vector3 worldPos, GameLogic.CharacterType killedCharacterType, [Optional, DefaultParameterValue(null)] string predefinedDropRollId, [Optional, DefaultParameterValue(0)] ChestType chestTypeVisualOverride)
        {
            Reward reward = new Reward();
            reward.RewardSource = player.DungeonDropRewardSource;
            reward.RewardSourceId = player.DungeonDropRewardSourceId;
            ChestType chestType = DropTableRoll(dropLootTable, player, predefinedDropRollId);
            if (chestType != ChestType.NONE)
            {
                List<ShopEntryType> disallowedShopEntryTypes = null;
                if (dropLootTable == App.Binder.ConfigLootTables.BossAdditionalDropLootTable)
                {
                    List<ShopEntryType> list2 = new List<ShopEntryType>();
                    list2.Add(ShopEntryType.MysteryItem);
                    disallowedShopEntryTypes = list2;
                }
                bool isBossDrop = (dropLootTable == App.Binder.ConfigLootTables.BossDropLootTable) || (dropLootTable == App.Binder.ConfigLootTables.BossAdditionalDropLootTable);
                CmdRollChestLootTable.ExecuteStatic(chestType, player, isBossDrop, ref reward, disallowedShopEntryTypes);
                reward.ChestType = chestType;
                reward.ChestTypeVisualOverride = chestTypeVisualOverride;
            }
            if (reward.DiamondDrops.Count > 0)
            {
                player.DailyDiamondDropCount += reward.getTotalDiamondAmount();
            }
            if (ConfigMeta.IsMysteryChest(chestType))
            {
                player.LastMysteryChestDropTimestamp = Service.Binder.ServerTime.GameTime;
            }
            if (reward.ChestType != ChestType.NONE)
            {
                player.UnclaimedRewards.Add(reward);
            }
            else
            {
                CmdConsumeReward.ExecuteStatic(player, reward, true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
            }
            GameLogic.Binder.EventBus.DropLootTableRolled(dropLootTable, worldPos, reward);
            return reward;
        }

        private static void ModifyDropTableWeights(LootTable dropLootTable, Player player)
        {
            if (dropLootTable == App.Binder.ConfigLootTables.MinionDropLootTable)
            {
                if (!player.mysteryChestCanDrop())
                {
                    dropLootTable.setWeightForIds("CHEST", 0, false);
                }
                else if (!player.mysteryChestCanContainSpecialOffer())
                {
                    dropLootTable.getFirstMatchingItem("CHEST").SubTable.setWeightForIds(ChestType.MysterySpecialOffer.ToString(), 0, false);
                }
            }
            else if (dropLootTable == App.Binder.ConfigLootTables.BossDropLootTable)
            {
                ChestType nONE = ChestType.NONE;
                for (int i = 0; i < dropLootTable.Items.Count; i++)
                {
                    LootTableItem lti = dropLootTable.Items[i];
                    ChestType chestType = (ChestType) ((int) Enum.Parse(typeof(ChestType), lti.Id));
                    if (!player.chestTypeCanDrop(chestType))
                    {
                        dropLootTable.setWeightForItem(lti, 0);
                    }
                    else if ((lti.Weight > 0) && !player.hasEncounteredChest(chestType))
                    {
                        nONE = chestType;
                    }
                }
                if (nONE != ChestType.NONE)
                {
                    for (int j = 0; j < dropLootTable.Items.Count; j++)
                    {
                        LootTableItem item2 = dropLootTable.Items[j];
                        ChestType type3 = (ChestType) ((int) Enum.Parse(typeof(ChestType), item2.Id));
                        if (type3 != nONE)
                        {
                            dropLootTable.setWeightForItem(item2, 0);
                        }
                    }
                }
                else
                {
                    CharacterInstance activeCharacter = player.ActiveCharacter;
                    if (activeCharacter.getPerkInstanceCount(PerkType.LuckChest) > 0)
                    {
                        string str = ChestType.Basic001.ToString();
                        float num3 = activeCharacter.getGenericModifierForPerkType(PerkType.LuckChest);
                        for (int k = 0; k < dropLootTable.Items.Count; k++)
                        {
                            LootTableItem item3 = dropLootTable.Items[k];
                            if (item3.Id != str)
                            {
                                int weight = item3.Weight + ((int) (item3.Weight * num3));
                                dropLootTable.setWeightForItem(item3, weight);
                            }
                        }
                    }
                }
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator61 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRollDropLootTable <>f__this;

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
                    CmdRollDropLootTable.ExecuteStatic(this.<>f__this.m_dropLootTable, this.<>f__this.m_player, this.<>f__this.m_worldPos, this.<>f__this.m_killedCharacterType, this.<>f__this.m_predefinedDropRollId, ChestType.NONE);
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

