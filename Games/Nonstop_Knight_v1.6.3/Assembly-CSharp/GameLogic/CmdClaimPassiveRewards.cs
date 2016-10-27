namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdClaimPassiveRewards : ICommand
    {
        private Player m_player;

        public CmdClaimPassiveRewards(Player player)
        {
            this.m_player = player;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator87 iterator = new <executeRoutine>c__Iterator87();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(Player player)
        {
            CharacterInstance activeCharacter = player.ActiveCharacter;
            Player.PassiveProgress progress = player.getPassiveProgress(player.getLastCompletedFloor(false) + 1, player.UnclaimedPassiveRewardableSeconds, false);
            player.setMinionsKilledSinceLastRoomCompletion(progress.NumOverflowedMinionKills, false);
            if (progress.NumFloorCompletions > 0)
            {
                int rankUpCount = progress.NumBossKills * App.Binder.ConfigMeta.PASSIVE_ITEM_UPGRADE_COUNT_AFTER_BOSS_FIGHT;
                CharacterInstance character = player.ActiveCharacter;
                List<ItemSlot> list = character.getItemSlots(false);
                for (int j = 0; j < list.Count; j++)
                {
                    ItemInstance itemInstance = list[j].ItemInstance;
                    CmdRankUpItem.ExecuteStatic(character, itemInstance, rankUpCount, true);
                }
            }
            Vector3 position = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter.PhysicsBody.Transform.position;
            int num3 = 0;
            for (int i = 0; i < progress.NumFloorCompletions; i++)
            {
                int floor = player.getLastCompletedFloor(false) + 1;
                Dungeon dungeon = GameLogic.Binder.DungeonResources.getResource(ConfigDungeons.GetDungeonIdForFloor(floor));
                if ((num3 < App.Binder.ConfigMeta.PASSIVE_MAX_BOSS_ITEM_DROP_COUNT) && dungeon.hasBoss())
                {
                    string id = dungeon.getBossId(player, floor);
                    GameLogic.CharacterType killedCharacterType = GameLogic.Binder.CharacterResources.getResource(id).Type;
                    Reward reward = CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.BossDropLootTable, player, position, killedCharacterType, null, ChestType.NONE);
                    if (!reward.isEmpty())
                    {
                        CmdConsumeReward.ExecuteStatic(player, reward, true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                        num3++;
                    }
                }
                if (dungeon.hasBoss())
                {
                    string str2 = dungeon.getBossId(player, floor);
                    GameLogic.CharacterType type = GameLogic.Binder.CharacterResources.getResource(str2).Type;
                    GameLogic.Binder.LootSystem.grantRetirementTriggerChestIfAllowed();
                    if (player.canRetire())
                    {
                        Reward reward2 = player.getFirstUnclaimedRetirementTriggerChest();
                        double item = App.Binder.ConfigMeta.RetirementTokenReward(activeCharacter, floor);
                        reward2.TokenDrops.Add(item);
                    }
                    if ((ConfigMeta.BOSS_ADDITIONAL_DROPS_ENABLED && player.canRetire()) && App.Binder.ConfigMeta.BossShouldDropRewardBoxAtFloor(floor))
                    {
                        CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.BossAdditionalDropLootTable, player, position, type, null, ChestType.NONE);
                    }
                }
                player.setLastCompletedFloor(floor, false);
            }
            player.LastPassiveRewardClaimTimestamp = Service.Binder.ServerTime.GameTime;
            player.UnclaimedPassiveRewardableSeconds = 0L;
            GameLogic.Binder.EventBus.PassiveProgress(player, progress.NumMinionKills, progress.NumFloorCompletions, progress.NumBossKills);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator87 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdClaimPassiveRewards <>f__this;

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
                    CmdClaimPassiveRewards.ExecuteStatic(this.<>f__this.m_player);
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

