namespace GameLogic
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ConsoleCommand("boost")]
    public class CmdActivateBoost : ICommand
    {
        private string m_analyticsSourceId;
        private BoostType m_boostType;
        private Player m_player;

        public CmdActivateBoost(string[] serialized)
        {
            this.m_player = GameLogic.Binder.GameState.Player;
            this.m_boostType = (BoostType) ((int) Enum.Parse(typeof(BoostType), LangUtil.FirstLetterToUpper(serialized[0])));
        }

        public CmdActivateBoost(Player player, BoostType boost, [Optional, DefaultParameterValue("")] string analyticsSourceId)
        {
            this.m_player = player;
            this.m_boostType = boost;
            this.m_analyticsSourceId = analyticsSourceId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator3C iteratorc = new <executeRoutine>c__Iterator3C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public static void ExecuteStatic(Player player, BoostType boost, [Optional, DefaultParameterValue("")] string analyticsSourceId)
        {
            long gameTime = Service.Binder.ServerTime.GameTime;
            LangUtil.AddOrUpdateDictionaryEntry<string, long>(player.BoostStartTimestamps, boost.ToString(), gameTime);
            GameLogic.Binder.EventBus.BoostActivated(player, boost, analyticsSourceId);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator3C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdActivateBoost <>f__this;

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
                    CmdActivateBoost.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_boostType, this.<>f__this.m_analyticsSourceId);
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

