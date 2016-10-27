namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdUnlockRunestone : ICommand
    {
        private Player m_player;
        private RunestoneInstance m_runestone;

        public CmdUnlockRunestone(Player player, RunestoneInstance runestone)
        {
            this.m_player = player;
            this.m_runestone = runestone;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorBA rba = new <executeRoutine>c__IteratorBA();
            rba.<>f__this = this;
            return rba;
        }

        public static void ExecuteStatic(Player player, RunestoneInstance runestone)
        {
            if (runestone.Unlocked)
            {
                UnityEngine.Debug.LogWarning("Player has already unlocked runestone: " + runestone.Id);
            }
            else
            {
                runestone.Unlocked = true;
                Binder.EventBus.RunestoneUnlocked(player, runestone);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorBA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdUnlockRunestone <>f__this;

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
                    CmdUnlockRunestone.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_runestone);
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

