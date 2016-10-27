namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdBestLeaderboardUser : ICommand
    {
        private LeaderboardEntry m_opponent;
        private Player m_player;

        public CmdBestLeaderboardUser(Player player, LeaderboardEntry opponent)
        {
            this.m_player = player;
            this.m_opponent = opponent;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator91 iterator = new <executeRoutine>c__Iterator91();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(Player player, LeaderboardEntry opponent)
        {
            if (!player.BestedLeaderboardUserIds.ContainsKey(opponent.UserId))
            {
                player.BestedLeaderboardUserIds.Add(opponent.UserId, opponent.HighestFloor);
            }
            else if (opponent.Dummy)
            {
                UnityEngine.Debug.LogWarning("Re-besting leaderboard dummy knight: " + opponent.UserId);
            }
            player.BestedLeaderboardUserIds[opponent.UserId] = opponent.HighestFloor;
            player.UnclaimedLeaderboardRewards.Add(opponent);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator91 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdBestLeaderboardUser <>f__this;

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
                    CmdBestLeaderboardUser.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_opponent);
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

