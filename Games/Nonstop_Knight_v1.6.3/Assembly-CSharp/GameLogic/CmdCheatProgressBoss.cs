namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ConsoleCommand("boss")]
    public class CmdCheatProgressBoss : ICommand
    {
        public CmdCheatProgressBoss()
        {
        }

        public CmdCheatProgressBoss(string[] serialized)
        {
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            return new <executeRoutine>c__Iterator82();
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator82 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ActiveDungeon <ad>__0;
            internal double <baseRewardCoins>__9;
            internal double <coins>__10;
            internal Dungeon <dungeon>__4;
            internal RoomEndCondition <endCondition>__13;
            internal int <floor>__3;
            internal int <i>__6;
            internal int <minionKillsThisFloor>__5;
            internal GameLogic.CharacterType <minionType>__8;
            internal float <modifierSum>__12;
            internal CharacterInstance <pc>__2;
            internal Player <player>__1;
            internal string <randomFloorMinionId>__7;
            internal double <xpAmount>__11;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        if ((((this.<ad>__0 != null) && (this.<ad>__0.ActiveRoom != null)) && (this.<player>__1 != null)) && (this.<ad>__0.CurrentGameplayState == GameplayState.ACTION))
                        {
                            if (((this.<ad>__0.ActiveTournament == null) || this.<ad>__0.ActiveRoom.MainBossSummoned) || this.<ad>__0.WildBossMode)
                            {
                                this.<pc>__2 = this.<ad>__0.PrimaryPlayerCharacter;
                                this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.WAITING, 0f), 0f);
                                this.$PC = 1;
                                return true;
                            }
                            GameLogic.Binder.CharacterSpawningSystem.trySummonWildBoss();
                        }
                        goto Label_034A;

                    case 1:
                    {
                        this.<floor>__3 = this.<player>__1.getLastCompletedFloor(false) + 1;
                        while (true)
                        {
                            this.<dungeon>__4 = GameLogic.Binder.DungeonResources.getResource(ConfigDungeons.GetDungeonIdForFloor(this.<floor>__3));
                            this.<minionKillsThisFloor>__5 = this.<player>__1.getRequiredMinionKillsForFloorCompletion(this.<floor>__3, false, false);
                            this.<i>__6 = 0;
                            while (this.<i>__6 < this.<minionKillsThisFloor>__5)
                            {
                                this.<randomFloorMinionId>__7 = this.<dungeon>__4.getRandomMinionId();
                                this.<minionType>__8 = GameLogic.Binder.CharacterResources.getResource(this.<randomFloorMinionId>__7).Type;
                                this.<baseRewardCoins>__9 = App.Binder.ConfigMeta.MinionCoinDropCurve(this.<floor>__3);
                                this.<coins>__10 = this.<player>__1.calculateStandardCoinRoll(this.<baseRewardCoins>__9, this.<minionType>__8, 1);
                                Vector3? worldPt = null;
                                CmdGainResources.ExecuteStatic(this.<player>__1, ResourceType.Coin, this.<coins>__10, false, string.Empty, worldPt);
                                this.<xpAmount>__11 = App.Binder.ConfigMeta.XpFromMinionKill(this.<ad>__0.Floor);
                                this.<modifierSum>__12 = this.<pc>__2.getCharacterTypeXpModifier(this.<minionType>__8) + this.<pc>__2.UniversalXpBonus(true);
                                this.<xpAmount>__11 += this.<xpAmount>__11 * this.<modifierSum>__12;
                                Vector3? nullable2 = null;
                                CmdGainResources.ExecuteStatic(this.<player>__1, ResourceType.Xp, this.<xpAmount>__11, true, string.Empty, nullable2);
                                CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.MinionDropLootTable, this.<player>__1, Vector3.zero, this.<minionType>__8, null, ChestType.NONE);
                                this.<i>__6++;
                            }
                            if (this.<dungeon>__4.hasBoss())
                            {
                                break;
                            }
                            this.<player>__1.setLastCompletedFloor(this.<floor>__3, false);
                            this.<floor>__3++;
                        }
                    }
                    default:
                        goto Label_034A;
                }
                if (this.<ad>__0.isBossFloor())
                {
                    GameLogic.Binder.CommandProcessor.execute(new CmdStartBossFight(Room.BossSummonMethod.Player), 0f);
                }
                else
                {
                    CmdStartBossTrain.ExecuteStatic(this.<ad>__0, this.<player>__1, 1);
                    this.<endCondition>__13 = !GameLogic.Binder.FrenzySystem.isFrenzyActive() ? RoomEndCondition.NORMAL_COMPLETION : RoomEndCondition.FRENZY_COMPLETION;
                    GameLogic.Binder.CommandProcessor.execute(new CmdCompleteRoom(this.<endCondition>__13), 0f);
                    goto Label_034A;
                    this.$PC = -1;
                }
            Label_034A:
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

