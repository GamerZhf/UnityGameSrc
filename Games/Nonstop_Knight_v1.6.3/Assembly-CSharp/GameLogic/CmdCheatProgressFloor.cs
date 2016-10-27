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

    [ConsoleCommand("floor")]
    public class CmdCheatProgressFloor : ICommand
    {
        private bool m_doSimulateProgress;
        private int m_targetFloor;

        public CmdCheatProgressFloor(string[] serialized)
        {
            this.m_doSimulateProgress = serialized[0] == "simulate";
            this.m_targetFloor = !this.m_doSimulateProgress ? int.Parse(serialized[0]) : int.Parse(serialized[1]);
        }

        public CmdCheatProgressFloor(int targetFloor, [Optional, DefaultParameterValue(false)] bool doSimulateProgress)
        {
            this.m_doSimulateProgress = doSimulateProgress;
            this.m_targetFloor = targetFloor;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator83 iterator = new <executeRoutine>c__Iterator83();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(int targetFloor, [Optional, DefaultParameterValue(false)] bool doSimulateProgress)
        {
            bool flag = false;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            if (((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && ((player != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION)))
            {
                CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
                List<ItemSlot> list = primaryPlayerCharacter.getItemSlots(false);
                for (int i = 0; i < list.Count; i++)
                {
                    ItemSlot slot = list[i];
                    ItemInstance itemInstance = slot.ItemInstance;
                    float num2 = (slot.CompatibleItemType != ItemType.Armor) ? 1.47f : 1.8f;
                    if (itemInstance != null)
                    {
                        int[] values = new int[3];
                        values[0] = itemInstance.Rank;
                        values[1] = Mathf.RoundToInt((targetFloor * App.Binder.ConfigMeta.LEVEL_TO_FLOOR_MULTIPLIER) * num2);
                        itemInstance.Rank = Mathf.Max(values);
                    }
                }
                if (doSimulateProgress)
                {
                    for (int j = player.getLastCompletedFloor(false) + 1; j < targetFloor; j++)
                    {
                        Dungeon dungeon2 = GameLogic.Binder.DungeonResources.getResource(ConfigDungeons.GetDungeonIdForFloor(j));
                        if (flag)
                        {
                            int num4 = player.getRequiredMinionKillsForFloorCompletion(j, false, false);
                            for (int num5 = 0; num5 < num4; num5++)
                            {
                                string id = dungeon2.getRandomMinionId();
                                GameLogic.CharacterType killedCharacterType = GameLogic.Binder.CharacterResources.getResource(id).Type;
                                double baseCoinReward = App.Binder.ConfigMeta.MinionCoinDropCurve(j);
                                double amount = player.calculateStandardCoinRoll(baseCoinReward, killedCharacterType, 1);
                                Vector3? worldPt = null;
                                CmdGainResources.ExecuteStatic(player, ResourceType.Coin, amount, false, string.Empty, worldPt);
                                double num8 = App.Binder.ConfigMeta.XpFromMinionKill(activeDungeon.Floor);
                                float num9 = primaryPlayerCharacter.getCharacterTypeXpModifier(killedCharacterType) + primaryPlayerCharacter.UniversalXpBonus(true);
                                num8 += num8 * num9;
                                CmdGainResources.ExecuteStatic(player, ResourceType.Xp, num8, true, string.Empty, new Vector3?(Vector3.zero));
                                CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.MinionDropLootTable, player, Vector3.zero, killedCharacterType, null, ChestType.NONE);
                            }
                        }
                        if (dungeon2.hasBoss())
                        {
                            string str2 = dungeon2.getBossId(player, j);
                            GameLogic.CharacterType type = GameLogic.Binder.CharacterResources.getResource(str2).Type;
                            double num10 = App.Binder.ConfigMeta.BossCoinDropCurve(j, false);
                            double num11 = player.calculateStandardCoinRoll(num10, type, 1);
                            Vector3? nullable2 = null;
                            CmdGainResources.ExecuteStatic(player, ResourceType.Coin, num11, false, string.Empty, nullable2);
                            bool isHighestFloor = activeDungeon.Floor == player.getHighestFloorReached();
                            double num12 = App.Binder.ConfigMeta.XpFromBossKill(activeDungeon.Floor, isHighestFloor);
                            float num13 = primaryPlayerCharacter.getCharacterTypeXpModifier(type) + primaryPlayerCharacter.UniversalXpBonus(true);
                            num12 += num12 * num13;
                            Vector3? nullable3 = null;
                            CmdGainResources.ExecuteStatic(player, ResourceType.Xp, num12, true, string.Empty, nullable3);
                            GameLogic.Binder.LootSystem.grantRetirementTriggerChestIfAllowed();
                            if (player.canRetire())
                            {
                                Reward reward = player.getFirstUnclaimedRetirementTriggerChest();
                                double item = App.Binder.ConfigMeta.RetirementTokenReward(primaryPlayerCharacter, activeDungeon.Floor);
                                reward.TokenDrops.Add(item);
                            }
                            CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.BossDropLootTable, player, Vector3.zero, type, null, ChestType.NONE);
                            if ((ConfigMeta.BOSS_ADDITIONAL_DROPS_ENABLED && player.canRetire()) && App.Binder.ConfigMeta.BossShouldDropRewardBoxAtFloor(j))
                            {
                                CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.BossAdditionalDropLootTable, player, Vector3.zero, type, null, ChestType.NONE);
                            }
                        }
                        for (int n = player.UnclaimedRewards.Count - 1; n >= 0; n--)
                        {
                            Reward reward2 = player.UnclaimedRewards[n];
                            if (!ConfigMeta.IsRetirementChest(reward2.ChestType))
                            {
                                CmdConsumeReward.ExecuteStatic(player, reward2, true, string.Empty);
                            }
                        }
                        player.setLastCompletedFloor(j, false);
                    }
                    while (player.NumPendingRankUpCeremonies > 0)
                    {
                        double num16 = App.Binder.ConfigMeta.RankUpRewardGems(player.Rank);
                        Vector3? nullable4 = null;
                        CmdGainResources.ExecuteStatic(player, ResourceType.Diamond, num16, true, string.Empty, nullable4);
                        player.NumPendingRankUpCeremonies--;
                        if (player.canRankUp())
                        {
                            CmdRankUpPlayer.ExecuteStatic(player, true);
                        }
                    }
                    for (int k = player.PendingSkillUnlocks.Count - 1; k >= 0; k--)
                    {
                        CmdUnlockSkill.ExecuteStatic(player, player.PendingSkillUnlocks[0], true);
                    }
                    for (int m = player.PendingRankUpRunestoneUnlocks.Count - 1; m >= 0; m--)
                    {
                        CmdGainRunestone.ExecuteStatic(player, player.PendingRankUpRunestoneUnlocks[m], false);
                    }
                }
                player.CompletedTutorials.Clear();
                ConfigTutorials.CheatCompleteAllFtueTutorials(player);
                ConfigTutorials.CheatCompleteAllContextTutorials(player);
                player.setLastCompletedFloor(Mathf.Clamp(targetFloor - 1, 0, 0x7fffffff), false);
                player.setLastBossEncounterFailed(false, false);
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
                GameLogic.Binder.DecoSpawningSystem.unloadAllDecos(activeDungeon.ActiveRoom);
                App.Binder.AppContext.hardReset(null);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator83 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCheatProgressFloor <>f__this;

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
                    CmdCheatProgressFloor.ExecuteStatic(this.<>f__this.m_targetFloor, this.<>f__this.m_doSimulateProgress);
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

