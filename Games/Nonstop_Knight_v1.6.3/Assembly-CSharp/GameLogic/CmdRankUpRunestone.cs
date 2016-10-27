namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdRankUpRunestone : ICommand
    {
        private Player m_player;
        private RunestoneInstance m_runestone;

        public CmdRankUpRunestone(Player player, RunestoneInstance runestone)
        {
            this.m_player = player;
            this.m_runestone = runestone;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorAC rac = new <executeRoutine>c__IteratorAC();
            rac.<>f__this = this;
            return rac;
        }

        public static void ExecuteStatic(Player player, RunestoneInstance runestone)
        {
            if (runestone.isAtMaxLevel())
            {
                UnityEngine.Debug.LogWarning("Runestone already at max level, cannot rank-up: " + runestone.Id);
            }
            else
            {
                int num2 = runestone.Rank + 1;
                runestone.Rank = num2;
                Binder.EventBus.RunestoneRankUpped(player, runestone);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorAC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRankUpRunestone <>f__this;

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
                    CmdRankUpRunestone.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_runestone);
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

