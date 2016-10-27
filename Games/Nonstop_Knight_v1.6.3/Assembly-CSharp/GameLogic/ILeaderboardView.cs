namespace GameLogic
{
    using System;
    using System.Collections.Generic;

    public interface ILeaderboardView
    {
        int getLeaderboardIndexForPlayer(Player player);
        int getLeaderboardRankForPlayer(Player player);
        LeaderboardEntry getNextLeaderboardTargetForPlayer(Player player);
        void initialize();
        void NotifyCharacterUpdated(CharacterInstance activeCharater);
        void NotifyLeaderboardLoaded();
        void NotifyPlayerRenamed(Player player);
        void NotifyPlayerScoreUpdated(int newScore);
        void refresh();

        bool Initialized { get; }

        List<LeaderboardEntry> SortedLeaderboardEntries { get; }

        LeaderboardType Type { get; }
    }
}

