namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [ConsoleCommand("temp1")]
    public class CmdCheatTemp1 : ICommand
    {
        private int m_amount;

        public CmdCheatTemp1(int amount)
        {
            this.m_amount = amount;
        }

        public CmdCheatTemp1(string[] serialized)
        {
            this.m_amount = int.Parse(serialized[0]);
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator85 iterator = new <executeRoutine>c__Iterator85();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(int amount)
        {
            Player player = Binder.GameState.Player;
            if (player.Tournaments.hasTournamentSelected())
            {
                player.Tournaments.SelectedTournament.HeroStats.BossesBeat += amount;
            }
            else
            {
                player.ActiveCharacter.HeroStats.BossesBeat += amount;
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator85 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCheatTemp1 <>f__this;

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
                    CmdCheatTemp1.ExecuteStatic(this.<>f__this.m_amount);
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

