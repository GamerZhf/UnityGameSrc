namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdStartBossTrain : ICommand
    {
        private ActiveDungeon m_activeDungeon;
        private int m_numCharges;
        private Player m_player;

        public CmdStartBossTrain(ActiveDungeon activeDungeon, Player player, int numCharges)
        {
            this.m_activeDungeon = activeDungeon;
            this.m_player = player;
            this.m_numCharges = numCharges;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator6D iteratord = new <executeRoutine>c__Iterator6D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        public static void ExecuteStatic(ActiveDungeon ad, Player player, int numCharges)
        {
            if (!player.BossTrain.Active)
            {
                int num2;
                int num = player.getCurrentFloor(false);
                if ((ad.ActiveRoom != null) && ad.ActiveRoom.MainBossSummoned)
                {
                    num2 = num + 1;
                }
                else
                {
                    num2 = num;
                }
                int nextFloorWithBoss = ConfigDungeons.GetNextFloorWithBoss(num2);
                player.BossTrain.Active = true;
                player.BossTrain.ActivationFloor = num;
                player.BossTrain.ChargesRemaining = numCharges;
                player.BossTrain.ChargesTotal = numCharges;
                player.BossTrain.NumBossFloorsCompleted = 0;
                player.BossTrain.PendingJumpToFloorWithBoss = nextFloorWithBoss;
                GameLogic.Binder.EventBus.BossTrainStarted(player, numCharges);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator6D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdStartBossTrain <>f__this;

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
                    CmdStartBossTrain.ExecuteStatic(this.<>f__this.m_activeDungeon, this.<>f__this.m_player, this.<>f__this.m_numCharges);
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

