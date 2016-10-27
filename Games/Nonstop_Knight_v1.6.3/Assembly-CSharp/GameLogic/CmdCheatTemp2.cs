namespace GameLogic
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [ConsoleCommand("temp2")]
    public class CmdCheatTemp2 : ICommand
    {
        public CmdCheatTemp2()
        {
        }

        public CmdCheatTemp2(string[] serialized)
        {
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            return new <executeRoutine>c__Iterator86();
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator86 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Player <player>__0;

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
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    if (this.<player>__0.Tournaments.hasTournamentSelected())
                    {
                        this.<player>__0.Tournaments.SelectedTournament.CurrentState = TournamentInstance.State.PENDING_END_ANNOUNCEMENT;
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

