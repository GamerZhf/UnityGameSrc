namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LeaderboardService
    {
        [CompilerGenerated]
        private List<LeaderboardEntry> <FriendsLeaderboard>k__BackingField;
        [CompilerGenerated]
        private List<LeaderboardEntry> <LocalLeaderboard>k__BackingField;

        public LeaderboardService()
        {
            this.FriendsLeaderboard = new List<LeaderboardEntry>();
            this.LocalLeaderboard = new List<LeaderboardEntry>();
        }

        [DebuggerHidden]
        public IEnumerator LoadFriendsLeaderboard(PlatformConnectType platform, List<string> friends)
        {
            <LoadFriendsLeaderboard>c__Iterator216 iterator = new <LoadFriendsLeaderboard>c__Iterator216();
            iterator.friends = friends;
            iterator.platform = platform;
            iterator.<$>friends = friends;
            iterator.<$>platform = platform;
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        public IEnumerator LoadLocalLeaderboard()
        {
            <LoadLocalLeaderboard>c__Iterator217 iterator = new <LoadLocalLeaderboard>c__Iterator217();
            iterator.<>f__this = this;
            return iterator;
        }

        public List<LeaderboardEntry> FriendsLeaderboard
        {
            [CompilerGenerated]
            get
            {
                return this.<FriendsLeaderboard>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<FriendsLeaderboard>k__BackingField = value;
            }
        }

        public List<LeaderboardEntry> LocalLeaderboard
        {
            [CompilerGenerated]
            get
            {
                return this.<LocalLeaderboard>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LocalLeaderboard>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <LoadFriendsLeaderboard>c__Iterator216 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal List<string> <$>friends;
            internal PlatformConnectType <$>platform;
            internal List<RemoteLeaderboardEntry>.Enumerator <$s_489>__3;
            internal LeaderboardService <>f__this;
            internal RemoteLeaderboardEntry <entry>__4;
            internal LeaderboardEntry <lbEntry>__5;
            internal Player <player>__0;
            internal FriendsRequest <req>__1;
            internal Request<LeaderboardResponse> <resp>__2;
            internal List<string> friends;
            internal PlatformConnectType platform;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    {
                        UnityEngine.Debug.Log("load friends leaderboard");
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        FriendsRequest request = new FriendsRequest();
                        request.Friends = this.friends;
                        request.PlatformType = this.platform;
                        this.<req>__1 = request;
                        this.<resp>__2 = Request<LeaderboardResponse>.Post("/player/{sessionId}/leaderboard/social/HighestFloor/" + this.<player>__0._id, this.<req>__1);
                        this.$current = this.<resp>__2.Task;
                        this.$PC = 1;
                        return true;
                    }
                    case 1:
                        if (!this.<resp>__2.Success)
                        {
                            UnityEngine.Debug.Log("failed " + this.<resp>__2.ErrorMsg);
                            break;
                        }
                        UnityEngine.Debug.Log("success");
                        this.<>f__this.FriendsLeaderboard.Clear();
                        this.<$s_489>__3 = this.<resp>__2.Result.entries.GetEnumerator();
                        try
                        {
                            while (this.<$s_489>__3.MoveNext())
                            {
                                this.<entry>__4 = this.<$s_489>__3.Current;
                                this.<lbEntry>__5 = new LeaderboardEntry();
                                this.<lbEntry>__5.Dummy = false;
                                this.<lbEntry>__5.HighestFloor = this.<entry>__4.score;
                                this.<lbEntry>__5.IsSelf = false;
                                this.<lbEntry>__5.Name = this.<entry>__4.player.name;
                                this.<lbEntry>__5.UserId = this.<entry>__4.player.userId;
                                this.<>f__this.FriendsLeaderboard.Add(this.<lbEntry>__5);
                            }
                        }
                        finally
                        {
                            this.<$s_489>__3.Dispose();
                        }
                        break;

                    default:
                        goto Label_01CF;
                }
                this.$PC = -1;
            Label_01CF:
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

        [CompilerGenerated]
        private sealed class <LoadLocalLeaderboard>c__Iterator217 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal List<RemoteLeaderboardEntry>.Enumerator <$s_490>__3;
            internal LeaderboardService <>f__this;
            internal RemoteLeaderboardEntry <entry>__4;
            internal LeaderboardEntry <lbEntry>__5;
            internal string <path>__1;
            internal Player <player>__0;
            internal Request<LeaderboardResponse> <resp>__2;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        UnityEngine.Debug.Log("load local leaderboard");
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        if (this.<player>__0.ServerStats.Country == null)
                        {
                        }
                        this.<path>__1 = "/player/{sessionId}/leaderboard/" + "unknown" + "/HighestFloor";
                        this.<resp>__2 = Request<LeaderboardResponse>.Get(this.<path>__1);
                        this.$current = this.<resp>__2.Task;
                        this.$PC = 1;
                        return true;

                    case 1:
                        if (!this.<resp>__2.Success)
                        {
                            UnityEngine.Debug.Log("failed " + this.<resp>__2.ErrorMsg);
                            break;
                        }
                        UnityEngine.Debug.Log("success");
                        this.<>f__this.LocalLeaderboard.Clear();
                        this.<$s_490>__3 = this.<resp>__2.Result.entries.GetEnumerator();
                        try
                        {
                            while (this.<$s_490>__3.MoveNext())
                            {
                                this.<entry>__4 = this.<$s_490>__3.Current;
                                this.<lbEntry>__5 = new LeaderboardEntry();
                                this.<lbEntry>__5.Dummy = false;
                                this.<lbEntry>__5.HighestFloor = this.<entry>__4.score;
                                this.<lbEntry>__5.IsSelf = false;
                                if (this.<entry>__4.player.name == null)
                                {
                                }
                                this.<lbEntry>__5.Name = _.L(ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_05, null, false);
                                this.<lbEntry>__5.UserId = this.<entry>__4.player.userId;
                                this.<>f__this.LocalLeaderboard.Add(this.<lbEntry>__5);
                            }
                        }
                        finally
                        {
                            this.<$s_490>__3.Dispose();
                        }
                        break;

                    default:
                        goto Label_01E4;
                }
                App.Binder.EventBus.LeaderboardLoaded(LeaderboardType.Local);
                this.$PC = -1;
            Label_01E4:
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

