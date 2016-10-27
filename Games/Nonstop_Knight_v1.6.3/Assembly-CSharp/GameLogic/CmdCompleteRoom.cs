namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [ConsoleCommand("next")]
    public class CmdCompleteRoom : ICommand
    {
        private RoomEndCondition m_endCondition;

        public CmdCompleteRoom(RoomEndCondition endCondition)
        {
            this.m_endCondition = endCondition;
        }

        public CmdCompleteRoom(string[] serialized)
        {
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator51 iterator = new <executeRoutine>c__Iterator51();
            iterator.<>f__this = this;
            return iterator;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator51 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCompleteRoom <>f__this;
            internal Room <activeRoom>__1;
            internal ActiveDungeon <ad>__0;
            internal CharacterType <bossType>__8;
            internal CharacterInstance <c>__5;
            internal int <i>__4;
            internal int <i>__6;
            internal int <i>__7;
            internal CharacterInstance <pc>__3;
            internal Player <player>__2;

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
                        this.<ad>__0 = Binder.GameState.ActiveDungeon;
                        this.<activeRoom>__1 = this.<ad>__0.ActiveRoom;
                        this.<player>__2 = Binder.GameState.Player;
                        this.<pc>__3 = this.<player>__2.ActiveCharacter;
                        this.<activeRoom>__1.CompletionTriggered = true;
                        this.<activeRoom>__1.EndCondition = this.<>f__this.m_endCondition;
                        if (this.<activeRoom>__1.EndCondition != RoomEndCondition.FRENZY_COMPLETION)
                        {
                            this.$current = Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.ROOM_COMPLETION, 0f), 0f);
                            this.$PC = 1;
                            goto Label_03B3;
                        }
                        this.<player>__2.setLastBossEncounterFailed(false, false);
                        goto Label_027A;

                    case 1:
                        this.<i>__4 = 0;
                        while (this.<i>__4 < Binder.GameState.PersistentCharacters.Count)
                        {
                            this.<c>__5 = Binder.GameState.PersistentCharacters[this.<i>__4];
                            CmdInterruptCharacter.ExecuteStatic(this.<c>__5, true);
                            this.<i>__4++;
                        }
                        this.<i>__6 = this.<activeRoom>__1.ActiveProjectiles.Count - 1;
                        while (this.<i>__6 >= 0)
                        {
                            this.<activeRoom>__1.ActiveProjectiles[this.<i>__6].destroy();
                            this.<i>__6--;
                        }
                        this.<activeRoom>__1.flagAllDungeonBoostsForOffscreenDestroy();
                        this.<i>__7 = this.<activeRoom>__1.ActiveAreaEffects.Count - 1;
                        while (this.<i>__7 >= 0)
                        {
                            this.<activeRoom>__1.ActiveAreaEffects[this.<i>__7].destroy();
                            this.<i>__7--;
                        }
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_03B1;
                }
                while (Binder.EventBus.ProcessingQueuedEvents)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_03B3;
                }
                if (!this.<pc>__3.IsDead)
                {
                    Binder.DeathSystem.killAllNonPersistentCharacters(false, true);
                }
                if (this.<activeRoom>__1.MainBossSummoned && !this.<player>__2.BossTrain.Active)
                {
                    if (this.<activeRoom>__1.EndCondition == RoomEndCondition.FAIL)
                    {
                        this.<player>__2.setLastBossEncounterFailed(true, false);
                    }
                    else
                    {
                        this.<player>__2.setLastBossEncounterFailed(false, false);
                    }
                }
            Label_027A:
                if ((!this.<ad>__0.isTutorialDungeon() && this.<activeRoom>__1.MainBossSummoned) && ((this.<activeRoom>__1.EndCondition == RoomEndCondition.NORMAL_COMPLETION) || (this.<activeRoom>__1.EndCondition == RoomEndCondition.FRENZY_COMPLETION)))
                {
                    this.<bossType>__8 = Binder.CharacterResources.getResource(this.<ad>__0.BossId).Type;
                    Binder.LootSystem.awardBossRewards(this.<ad>__0, this.<bossType>__8, false);
                }
                if ((this.<activeRoom>__1.EndCondition == RoomEndCondition.NORMAL_COMPLETION) || (this.<activeRoom>__1.EndCondition == RoomEndCondition.FRENZY_COMPLETION))
                {
                    this.<player>__2.setLastCompletedFloor(this.<ad>__0.Floor, false);
                    this.<player>__2.setMinionsKilledSinceLastRoomCompletion(0, false);
                    if (this.<ad>__0.ActiveTournament != null)
                    {
                        this.<ad>__0.ActiveTournament.WildBossesKilledSinceLastRoomCompletion = 0;
                    }
                    if (this.<player>__2.BossTrain.Active && this.<ad>__0.isBossFloor())
                    {
                        this.<player>__2.BossTrain.NumBossFloorsCompleted++;
                    }
                }
                Binder.EventBus.RoomCompleted(this.<activeRoom>__1);
                goto Label_03B1;
                this.$PC = -1;
            Label_03B1:
                return false;
            Label_03B3:
                return true;
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

