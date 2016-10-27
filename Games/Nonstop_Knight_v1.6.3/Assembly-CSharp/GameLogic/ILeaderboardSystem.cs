namespace GameLogic
{
    using System;
    using System.Collections.Generic;

    public interface ILeaderboardSystem
    {
        int getLeaderboardIndexForPlayer(LeaderboardType leaderboardType, Player player);
        int getLeaderboardRankForPlayer(LeaderboardType leaderboardType, Player player);
        LeaderboardEntry getNextLeaderboardTargetForPlayer(LeaderboardType leaderboardType, Player player);
        List<LeaderboardEntry> getSortedLeaderboardEntries(LeaderboardType leaderboardType);
        void initialize();

        bool Initialized { get; }
    }
}

