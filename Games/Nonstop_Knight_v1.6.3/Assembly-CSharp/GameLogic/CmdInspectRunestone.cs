namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdInspectRunestone : ICommand
    {
        private RunestoneInstance m_runestoneInstance;

        public CmdInspectRunestone(RunestoneInstance runestoneInstance)
        {
            this.m_runestoneInstance = runestoneInstance;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorA5 ra = new <executeRoutine>c__IteratorA5();
            ra.<>f__this = this;
            return ra;
        }

        public static void ExecuteStatic(RunestoneInstance runestoneInstance)
        {
            Player player = Binder.GameState.Player;
            runestoneInstance.InspectedByPlayer = true;
            Binder.EventBus.RunestoneInspected(player, runestoneInstance);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorA5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdInspectRunestone <>f__this;

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
                    CmdInspectRunestone.ExecuteStatic(this.<>f__this.m_runestoneInstance);
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

