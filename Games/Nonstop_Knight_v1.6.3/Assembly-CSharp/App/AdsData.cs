namespace App
{
    using GameLogic;
    using System;

    public class AdsData
    {
        public Category AdCategory;
        public Reward AdReward;
        public string AdZone;

        public enum Category
        {
            UNDEFINED,
            VENDOR,
            ADVENTURE_MYSTERY,
            TOURNAMENT_MYSTERY,
            TOURNAMENT_CARDS
        }
    }
}

