namespace Service
{
    using System;
    using System.Collections.Generic;

    public class RewardMilestone
    {
        public int CardPackAmount;
        public List<Entry> ContributorRewards;
        public Entry MainReward;
        public int Threshold;

        public RewardMilestone()
        {
            this.MainReward = new Entry();
            this.ContributorRewards = new List<Entry>();
        }

        public RewardMilestone(int cardPackAmount, Entry mainReward, List<Entry> contributorRewards)
        {
            this.MainReward = new Entry();
            this.ContributorRewards = new List<Entry>();
            this.CardPackAmount = cardPackAmount;
            this.MainReward = mainReward;
            this.ContributorRewards = contributorRewards;
        }

        public class Entry
        {
            public int Amount;
            public string Id;
        }
    }
}

