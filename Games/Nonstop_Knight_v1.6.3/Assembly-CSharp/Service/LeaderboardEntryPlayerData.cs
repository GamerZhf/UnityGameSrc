namespace Service
{
    using GameLogic;
    using System;

    public class LeaderboardEntryPlayerData
    {
        public string metadata;
        public string name;
        public string userId;

        public LeaderboardEntryPlayerData()
        {
        }

        public LeaderboardEntryPlayerData(Player player)
        {
            this.userId = player._id;
            this.name = player.SocialData.Name;
            this.metadata = string.Empty;
        }

        public LeaderboardEntryPlayerData(SocialData socialData)
        {
            this.userId = socialData.FacebookId;
            this.name = socialData.Name;
            this.metadata = string.Empty;
        }
    }
}

