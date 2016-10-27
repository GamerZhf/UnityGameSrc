namespace GameLogic
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LeaderboardViewFacebook : ILeaderboardView, IComparer<LeaderboardEntry>
    {
        [CompilerGenerated]
        private List<LeaderboardEntry> <SortedLeaderboardEntries>k__BackingField;
        private List<LeaderboardEntry> m_bestableEntries;
        private bool m_initialized;
        private string m_leaderboardPlayerId;
        private LeaderboardEntry m_playerLeaderboardEntry;
        private List<LeaderboardEntry> m_usersBestableByActivePlayer = new List<LeaderboardEntry>(ConfigLeaderboard.MAX_NUM_VISIBLE_LEADERBOARD_CELLS);

        public LeaderboardViewFacebook()
        {
            this.SortedLeaderboardEntries = new List<LeaderboardEntry>();
        }

        public int Compare(LeaderboardEntry x, LeaderboardEntry y)
        {
            if (x.HighestFloor > y.HighestFloor)
            {
                return -1;
            }
            if (x.HighestFloor < y.HighestFloor)
            {
                return 1;
            }
            if (x.IsSelf)
            {
                return 1;
            }
            if (y.IsSelf)
            {
                return -1;
            }
            return x.UserId.CompareTo(y.UserId);
        }

        public int getLeaderboardIndexForPlayer(Player player)
        {
            if (this.SortedLeaderboardEntries != null)
            {
                for (int i = 0; i < this.SortedLeaderboardEntries.Count; i++)
                {
                    if (this.m_leaderboardPlayerId == this.SortedLeaderboardEntries[i].UserId)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public int getLeaderboardRankForPlayer(Player player)
        {
            int num = this.getLeaderboardIndexForPlayer(player);
            if (num != -1)
            {
                return (num + 1);
            }
            return -1;
        }

        public LeaderboardEntry getNextLeaderboardTargetForPlayer(Player player)
        {
            int num = this.getLeaderboardIndexForPlayer(player);
            if (num > 0)
            {
                return this.SortedLeaderboardEntries[num - 1];
            }
            return null;
        }

        public void initialize()
        {
            this.m_initialized = false;
            if (Service.Binder.FacebookAdapter.Initialized)
            {
                this.NotifyLeaderboardLoaded();
            }
        }

        public void NotifyCharacterUpdated(CharacterInstance activeCharacter)
        {
            if (this.m_playerLeaderboardEntry != null)
            {
                this.m_playerLeaderboardEntry.setDefaultPlayerHeroAvatarSprite();
            }
        }

        public void NotifyLeaderboardLoaded()
        {
            if (((Service.Binder.FacebookAdapter != null) && (Service.Binder.FacebookAdapter.Identity != null)) && (Service.Binder.FacebookAdapter.Leaderboard != null))
            {
                this.SortedLeaderboardEntries = Service.Binder.FacebookAdapter.Leaderboard;
                this.m_leaderboardPlayerId = Service.Binder.FacebookAdapter.Identity.id;
                int num = this.getLeaderboardIndexForPlayer(null);
                this.m_playerLeaderboardEntry = this.SortedLeaderboardEntries[num];
                Player player = GameLogic.Binder.GameState.Player;
                FbPlatformUser identity = Service.Binder.FacebookAdapter.Identity;
                string str = !(player.SocialData.Name == _.L(ConfigLoca.HERO_KNIGHT, null, false)) ? player.SocialData.Name : identity.userName;
                this.m_playerLeaderboardEntry.Name = str;
                this.refresh();
                this.m_initialized = true;
            }
        }

        public void NotifyPlayerRenamed(Player player)
        {
            this.NotifyLeaderboardLoaded();
        }

        public void NotifyPlayerScoreUpdated(int newScore)
        {
            if (this.m_playerLeaderboardEntry != null)
            {
                this.m_playerLeaderboardEntry.HighestFloor = newScore;
            }
            this.refresh();
        }

        public void refresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = this.m_usersBestableByActivePlayer.Count - 1; i >= 0; i--)
            {
                LeaderboardEntry opponent = this.m_usersBestableByActivePlayer[i];
                if (opponent.HighestFloor < this.m_playerLeaderboardEntry.HighestFloor)
                {
                    CmdBestLeaderboardUser.ExecuteStatic(player, opponent);
                    this.m_usersBestableByActivePlayer.Remove(opponent);
                }
            }
            this.refreshUserListBestableByActivePlayer();
            LeaderboardSorter.Instance.Sort(this.SortedLeaderboardEntries);
        }

        private void refreshUserListBestableByActivePlayer()
        {
            if (this.SortedLeaderboardEntries != null)
            {
                Player player = GameLogic.Binder.GameState.Player;
                bool flag = false;
                this.m_usersBestableByActivePlayer.Clear();
                for (int i = 0; i < this.SortedLeaderboardEntries.Count; i++)
                {
                    LeaderboardEntry item = this.SortedLeaderboardEntries[i];
                    if ((item.UserId == this.m_playerLeaderboardEntry.UserId) || (item.HighestFloor < this.m_playerLeaderboardEntry.HighestFloor))
                    {
                        continue;
                    }
                    if (item.Dummy)
                    {
                        if (!player.BestedLeaderboardUserIds.ContainsKey(item.UserId))
                        {
                            goto Label_00DB;
                        }
                        continue;
                    }
                    if (player.BestedLeaderboardUserIds.ContainsKey(item.UserId))
                    {
                        int num2 = player.BestedLeaderboardUserIds[item.UserId];
                        if (item.HighestFloor <= num2)
                        {
                            continue;
                        }
                        flag = true;
                    }
                Label_00DB:
                    this.m_usersBestableByActivePlayer.Add(item);
                }
                if (flag)
                {
                    GameLogic.Binder.EventBus.LeaderboardOutperformed(this.Type);
                }
            }
        }

        public bool Initialized
        {
            get
            {
                return this.m_initialized;
            }
        }

        public List<LeaderboardEntry> SortedLeaderboardEntries
        {
            [CompilerGenerated]
            get
            {
                return this.<SortedLeaderboardEntries>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SortedLeaderboardEntries>k__BackingField = value;
            }
        }

        public LeaderboardType Type
        {
            get
            {
                return LeaderboardType.Royal;
            }
        }
    }
}

