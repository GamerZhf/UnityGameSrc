namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdEvolveRunestone : ICommand
    {
        private Player m_player;
        private RunestoneInstance m_runestone;

        public CmdEvolveRunestone(Player player, RunestoneInstance runestone)
        {
            this.m_player = player;
            this.m_runestone = runestone;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator9B iteratorb = new <executeRoutine>c__Iterator9B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public static void ExecuteStatic(Player player, RunestoneInstance runestone)
        {
            if (runestone.isAtMaxLevel())
            {
                UnityEngine.Debug.LogWarning("Runestone already at max level, cannot level-up: " + runestone.Id);
            }
            else
            {
                int num = runestone.getCompletedRankUpsForNextEvolve();
                int num2 = runestone.getRequiredRankUpsForNextEvolve();
                if (num >= num2)
                {
                    int num4 = runestone.Level + 1;
                    runestone.Level = num4;
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Level-upping runestone without needed ranks: " + runestone.Id);
                }
                Binder.EventBus.RunestoneLevelUpped(player, runestone);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator9B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdEvolveRunestone <>f__this;

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
                    CmdEvolveRunestone.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_runestone);
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

