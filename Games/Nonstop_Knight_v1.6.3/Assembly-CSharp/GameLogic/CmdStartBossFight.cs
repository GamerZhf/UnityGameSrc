namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdStartBossFight : ICommand
    {
        private Room.BossSummonMethod m_bossSummonMethod;

        public CmdStartBossFight(Room.BossSummonMethod bossSummonMethod)
        {
            this.m_bossSummonMethod = bossSummonMethod;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator6C iteratorc = new <executeRoutine>c__Iterator6C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator6C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdStartBossFight <>f__this;
            internal ActiveDungeon <ad>__0;
            internal Character <boss>__3;
            internal int <nextFloorWithBoss>__4;
            internal CharacterInstance <pc>__2;
            internal Player <player>__1;

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
                    this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                    this.<player>__1 = GameLogic.Binder.GameState.Player;
                    this.<pc>__2 = this.<ad>__0.PrimaryPlayerCharacter;
                    this.<boss>__3 = GameLogic.Binder.CharacterResources.getResource(this.<ad>__0.BossId);
                    if (this.<boss>__3 == null)
                    {
                        UnityEngine.Debug.LogError("Trying to start a boss fight without a boss id");
                    }
                    else
                    {
                        this.<ad>__0.ActiveRoom.MainBossSummoned = true;
                        this.<ad>__0.ActiveRoom.BossDifficultyDuringSummon = ConfigGameplay.CalculateBossDifficulty(this.<player>__1, this.<ad>__0.Floor, this.<ad>__0.isEliteBossFloor(), this.<boss>__3, this.<ad>__0.getProgressDifficultyExponent());
                        this.<ad>__0.ActiveRoom.BossSummonedWith = this.<>f__this.m_bossSummonMethod;
                        this.<ad>__0.SecondChanceUsed = false;
                        this.<ad>__0.NumberOfPaidRevivesUsed = 0;
                        if (this.<player>__1.BossTrain.Active)
                        {
                            this.<player>__1.BossTrain.ChargesRemaining--;
                            this.<nextFloorWithBoss>__4 = ConfigDungeons.GetNextFloorWithBoss(this.<ad>__0.Floor + 1);
                            this.<player>__1.BossTrain.PendingJumpToFloorWithBoss = this.<nextFloorWithBoss>__4;
                        }
                        if (this.<>f__this.m_bossSummonMethod == Room.BossSummonMethod.Frenzy)
                        {
                            GameLogic.Binder.CharacterSpawningSystem.summonActiveRoomBoss();
                        }
                        else
                        {
                            this.<pc>__2.resetDynamicRuntimeData();
                            GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.BOSS_START, 0f), 0f);
                        }
                    }
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

