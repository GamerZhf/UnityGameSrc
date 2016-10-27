namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [ConsoleCommand("runestone")]
    public class CmdGainRunestone : ICommand
    {
        private bool m_cheated;
        private Player m_player;
        private string m_runestoneId;

        public CmdGainRunestone(string[] serialized)
        {
            this.m_player = Binder.GameState.Player;
            this.m_runestoneId = LangUtil.FirstLetterToUpper(serialized[0]);
            this.m_cheated = true;
        }

        public CmdGainRunestone(Player player, string runestoneId, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            this.m_player = player;
            this.m_runestoneId = runestoneId;
            this.m_cheated = cheated;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorA0 ra = new <executeRoutine>c__IteratorA0();
            ra.<>f__this = this;
            return ra;
        }

        public static void ExecuteStatic(Player player, string runestoneId, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            if (player.Runestones.ownsRunestone(runestoneId))
            {
                if (!player.PendingRankUpRunestoneUnlocks.Contains(runestoneId))
                {
                    if (Application.isEditor)
                    {
                        UnityEngine.Debug.LogWarning("Gaining runestone without rank up pending flag: " + runestoneId);
                    }
                }
                else
                {
                    player.PendingRankUpRunestoneUnlocks.Remove(runestoneId);
                }
                RunestoneInstance runestone = player.Runestones.getRunestoneInstance(runestoneId);
                if (runestone != null)
                {
                    CmdRankUpRunestone.ExecuteStatic(player, runestone);
                    while (runestone.canEvolve())
                    {
                        CmdEvolveRunestone.ExecuteStatic(player, runestone);
                    }
                    return;
                }
            }
            RunestoneInstance instance3 = new RunestoneInstance();
            instance3.Id = runestoneId;
            instance3.Level = 1;
            instance3.Unlocked = true;
            RunestoneInstance item = instance3;
            player.Runestones.RunestoneInstances.Add(item);
            Binder.EventBus.RunestoneGained(player, item, cheated);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorA0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdGainRunestone <>f__this;

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
                    CmdGainRunestone.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_runestoneId, this.<>f__this.m_cheated);
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

