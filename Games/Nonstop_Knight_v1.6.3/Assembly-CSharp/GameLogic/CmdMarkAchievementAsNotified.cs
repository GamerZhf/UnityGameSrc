namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdMarkAchievementAsNotified : ICommand
    {
        private string m_id;
        private Player m_player;
        private int m_tier;

        public CmdMarkAchievementAsNotified(Player player, string id, int tier)
        {
            this.m_player = player;
            this.m_id = id;
            this.m_tier = tier;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorA9 ra = new <executeRoutine>c__IteratorA9();
            ra.<>f__this = this;
            return ra;
        }

        public static void ExecuteStatic(Player player, string id, int tier)
        {
            if (!player.Achievements.Notified.ContainsKey(id))
            {
                player.Achievements.Notified.Add(id, tier);
            }
            else
            {
                player.Achievements.Notified[id] = tier;
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorA9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdMarkAchievementAsNotified <>f__this;

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
                    CmdMarkAchievementAsNotified.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_id, this.<>f__this.m_tier);
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

