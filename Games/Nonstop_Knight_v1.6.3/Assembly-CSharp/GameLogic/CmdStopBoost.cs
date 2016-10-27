namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdStopBoost : ICommand
    {
        private BoostType m_boostType;
        private Player m_player;

        public CmdStopBoost(Player player, BoostType boost)
        {
            this.m_player = player;
            this.m_boostType = boost;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator3D iteratord = new <executeRoutine>c__Iterator3D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        public static void ExecuteStatic(Player player, BoostType boost)
        {
            string key = boost.ToString();
            if (player.BoostStartTimestamps.ContainsKey(key))
            {
                player.BoostStartTimestamps.Remove(key);
                Binder.EventBus.BoostStopped(player, boost);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator3D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdStopBoost <>f__this;

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
                    CmdStopBoost.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_boostType);
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

