namespace Service
{
    using System;

    public class SocialLeaderboardEntry
    {
        public LeaderboardEntryPlayerData playerData;
        public int score;

        public SocialLeaderboardEntry()
        {
        }

        public SocialLeaderboardEntry(LeaderboardEntryPlayerData _playerData, int _score)
        {
            this.playerData = _playerData;
            this.score = _score;
        }
    }
}

