namespace GameLogic
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LeaderboardView : ILeaderboardView
    {
        [CompilerGenerated]
        private bool <Initialized>k__BackingField;
        [CompilerGenerated]
        private List<LeaderboardEntry> <SortedLeaderboardEntries>k__BackingField;
        [CompilerGenerated]
        private LeaderboardType <Type>k__BackingField;
        private LeaderboardEntry activePlayerLBEntry;

        public LeaderboardView(LeaderboardType type)
        {
            this.Type = type;
            this.Initialized = false;
        }

        public int getLeaderboardIndexForPlayer(Player player)
        {
            if (this.SortedLeaderboardEntries != null)
            {
                for (int i = 0; i < this.SortedLeaderboardEntries.Count; i++)
                {
                    if (player._id == this.SortedLeaderboardEntries[i].UserId)
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
            this.Initialized = false;
            this.SortedLeaderboardEntries = new List<LeaderboardEntry>();
            this.NotifyLeaderboardLoaded();
        }

        public void NotifyCharacterUpdated(CharacterInstance activeCharacter)
        {
            if (this.activePlayerLBEntry != null)
            {
                this.activePlayerLBEntry.setDefaultPlayerHeroAvatarSprite();
            }
        }

        public void NotifyLeaderboardLoaded()
        {
            Player player = GameLogic.Binder.GameState.Player;
            List<LeaderboardEntry> localLeaderboard = Service.Binder.LeaderboardService.LocalLeaderboard;
            this.SortedLeaderboardEntries.Clear();
            this.activePlayerLBEntry = null;
            foreach (LeaderboardEntry entry in localLeaderboard)
            {
                if (entry.UserId == player._id)
                {
                    this.activePlayerLBEntry = entry;
                }
                this.SortedLeaderboardEntries.Add(entry);
            }
            if (this.activePlayerLBEntry == null)
            {
                this.activePlayerLBEntry = new LeaderboardEntry();
                this.SortedLeaderboardEntries.Add(this.activePlayerLBEntry);
            }
            this.activePlayerLBEntry.HighestFloor = player.getHighestFloorReached();
            if (player.SocialData.Name == null)
            {
            }
            this.activePlayerLBEntry.Name = _.L(ConfigLoca.LEADERBOARD_YOU, null, false);
            this.activePlayerLBEntry.Dummy = false;
            this.activePlayerLBEntry.IsSelf = true;
            this.activePlayerLBEntry.setDefaultPlayerHeroAvatarSprite();
            this.activePlayerLBEntry.UserId = player._id;
            Debug.Log("updated local leaderboards:" + this.SortedLeaderboardEntries.Count);
            this.Initialized = true;
            this.refresh();
        }

        public void NotifyPlayerRenamed(Player player)
        {
            if (this.activePlayerLBEntry != null)
            {
                if (player.SocialData.Name == null)
                {
                }
                this.activePlayerLBEntry.Name = _.L(ConfigLoca.LEADERBOARD_YOU, null, false);
            }
        }

        public void NotifyPlayerScoreUpdated(int newScore)
        {
            if (this.activePlayerLBEntry != null)
            {
                this.activePlayerLBEntry.HighestFloor = newScore;
                this.refresh();
            }
        }

        public void refresh()
        {
            LeaderboardSorter.Instance.Sort(this.SortedLeaderboardEntries);
        }

        public bool Initialized
        {
            [CompilerGenerated]
            get
            {
                return this.<Initialized>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Initialized>k__BackingField = value;
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
            [CompilerGenerated]
            get
            {
                return this.<Type>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Type>k__BackingField = value;
            }
        }

        private class LeaderboardEntryComparer : IComparer<LeaderboardEntry>
        {
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
        }
    }
}

