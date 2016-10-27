namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdCompleteTutorial : ICommand
    {
        private Player m_player;
        private string m_tutorialId;

        public CmdCompleteTutorial(Player player, string tutorialId)
        {
            this.m_player = player;
            this.m_tutorialId = tutorialId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator96 iterator = new <executeRoutine>c__Iterator96();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(Player player, string tutorialId)
        {
            if (!player.CompletedTutorials.Contains(tutorialId))
            {
                player.CompletedTutorials.Add(tutorialId);
                Binder.EventBus.TutorialCompleted(player, tutorialId);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator96 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCompleteTutorial <>f__this;

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
                    CmdCompleteTutorial.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_tutorialId);
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

