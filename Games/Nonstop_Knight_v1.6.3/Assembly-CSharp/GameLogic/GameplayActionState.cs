namespace GameLogic
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GameplayActionState : FiniteStateMachine.State
    {
        private Coroutine m_masterRoutine;

        public GameplayActionState() : base(Enum.GetName(typeof(GameplayState), GameplayState.ACTION), 4)
        {
            base.EnterMethod = new Action(this.onEnter);
            base.UpdateMethod = new Action<float>(this.onUpdate);
            base.ExitMethod = new Action(this.onExit);
        }

        [DebuggerHidden]
        private IEnumerator masterRoutine()
        {
            return new <masterRoutine>c__Iterator71();
        }

        public void onEnter()
        {
            UnityUtils.StopCoroutine(GameLogic.Binder.GameplayStateMachine.MonoBehaviour, ref this.m_masterRoutine);
            this.m_masterRoutine = UnityUtils.StartCoroutine(GameLogic.Binder.GameplayStateMachine.MonoBehaviour, this.masterRoutine());
        }

        public void onExit()
        {
            UnityUtils.StopCoroutine(GameLogic.Binder.GameplayStateMachine.MonoBehaviour, ref this.m_masterRoutine);
        }

        public void onUpdate(float dt)
        {
        }

        [CompilerGenerated]
        private sealed class <masterRoutine>c__Iterator71 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ActiveDungeon <ad>__0;
            internal RoomEndCondition <endCondition>__4;
            internal RoomEndCondition <endCondition>__5;
            internal bool <frenzyActive>__2;
            internal bool <inTournament>__3;
            internal Player <player>__1;

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
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        break;

                    case 8:
                        break;
                        this.$PC = -1;
                        goto Label_03E4;

                    default:
                        goto Label_03E4;
                }
                this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                if (((this.<ad>__0 == null) || (this.<ad>__0.ActiveRoom == null)) || (((this.<ad>__0.CurrentGameplayState != GameplayState.ACTION) || this.<ad>__0.ActiveRoom.CompletionTriggered) || this.<ad>__0.isTutorialDungeon()))
                {
                    this.$current = new WaitForFixedUpdate();
                    this.$PC = 1;
                }
                else
                {
                    this.<player>__1 = this.<ad>__0.PrimaryPlayerCharacter.OwningPlayer;
                    this.<frenzyActive>__2 = GameLogic.Binder.FrenzySystem.isFrenzyActive();
                    this.<inTournament>__3 = this.<ad>__0.ActiveTournament != null;
                    if ((this.<player>__1.BossTrain.Active && !this.<ad>__0.ActiveRoom.MainBossSummoned) && !this.<ad>__0.PrimaryPlayerCharacter.IsDead)
                    {
                        if (this.<player>__1.BossTrain.PendingJumpToFloorWithBoss == this.<ad>__0.Floor)
                        {
                            this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdStartBossFight(Room.BossSummonMethod.BossTrain), 0f);
                            this.$PC = 2;
                        }
                        else
                        {
                            this.<endCondition>__4 = !this.<frenzyActive>__2 ? RoomEndCondition.NORMAL_COMPLETION : RoomEndCondition.FRENZY_COMPLETION;
                            this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdCompleteRoom(this.<endCondition>__4), 0f);
                            this.$PC = 3;
                        }
                    }
                    else if (((this.<frenzyActive>__2 && !this.<ad>__0.ActiveRoom.MainBossSummoned) && (!this.<ad>__0.WildBossMode && this.<ad>__0.isBossFloor())) && this.<player>__1.floorCompletionGoalSatisfied(this.<ad>__0))
                    {
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdStartBossFight(Room.BossSummonMethod.Frenzy), 0f);
                        this.$PC = 4;
                    }
                    else if (((this.<ad>__0.isBossFloor() && !this.<ad>__0.ActiveRoom.MainBossSummoned) && (!this.<ad>__0.WildBossMode && this.<player>__1.autoSummonBossInFloor(this.<ad>__0.Floor))) && this.<player>__1.floorCompletionGoalSatisfied(this.<ad>__0))
                    {
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdStartBossFight(Room.BossSummonMethod.AutoSummonCompletedBossesToggle), 0f);
                        this.$PC = 5;
                    }
                    else if ((this.<inTournament>__3 && (this.<ad>__0.ActiveTournament.CurrentState != TournamentInstance.State.PENDING_JOIN_CONFIRMATION)) && (this.<ad>__0.ActiveTournament.CurrentState != TournamentInstance.State.ACTIVE))
                    {
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdCompleteRoom(RoomEndCondition.TOURNAMENT_END), 0f);
                        this.$PC = 6;
                    }
                    else if (this.<ad>__0.roomCompletionConditionSatisfied())
                    {
                        if (this.<ad>__0.PrimaryPlayerCharacter.IsDead)
                        {
                            this.<endCondition>__5 = RoomEndCondition.FAIL;
                        }
                        else if (this.<frenzyActive>__2)
                        {
                            this.<endCondition>__5 = RoomEndCondition.FRENZY_COMPLETION;
                        }
                        else
                        {
                            this.<endCondition>__5 = RoomEndCondition.NORMAL_COMPLETION;
                        }
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdCompleteRoom(this.<endCondition>__5), 0f);
                        this.$PC = 7;
                    }
                    else
                    {
                        this.$current = new WaitForFixedUpdate();
                        this.$PC = 8;
                    }
                }
                return true;
            Label_03E4:
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

