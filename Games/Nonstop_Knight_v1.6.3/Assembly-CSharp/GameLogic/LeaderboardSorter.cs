namespace GameLogic
{
    using System;
    using System.Collections.Generic;

    public class LeaderboardSorter : IComparer<LeaderboardEntry>
    {
        private static LeaderboardSorter instance = new LeaderboardSorter();

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
            if (x.isDummy() && !y.isDummy())
            {
                return 1;
            }
            if (!x.isDummy() && y.isDummy())
            {
                return -1;
            }
            return x.UserId.CompareTo(y.UserId);
        }

        public void Sort(List<LeaderboardEntry> entries)
        {
            entries.Sort(this);
            int num = 1;
            foreach (LeaderboardEntry entry in entries)
            {
                entry.Rank = num++;
            }
        }

        public static LeaderboardSorter Instance
        {
            get
            {
                return instance;
            }
        }
    }
}

