namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class TournamentUpgradeReward
    {
        public List<Entry> Choices = new List<Entry>();
        public int NumMilestonesCompleted;

        public int countNumSelected()
        {
            int num = 0;
            for (int i = 0; i < this.Choices.Count; i++)
            {
                if (this.Choices[i].Selected)
                {
                    num++;
                }
            }
            return num;
        }

        public static TournamentUpgradeReward GenerateReward(int numCompletedMilestones)
        {
            TournamentUpgradeReward reward = new TournamentUpgradeReward();
            reward.NumMilestonesCompleted = numCompletedMilestones;
            for (int i = 0; i < 3; i++)
            {
                Entry item = new Entry();
                int num2 = 0;
                do
                {
                    item.TourmanentUpgradeId = App.Binder.ConfigMeta.GetRandomTournamentUpgrade();
                }
                while (reward.hasUpgradeIdChoice(item.TourmanentUpgradeId) && (num2++ < 0x3e8));
                item.IsEpicUpgrade = (App.Binder.ConfigMeta.TOURNAMENT_UPGRADE_EPIC_PROBABILITY > 0f) && (UnityEngine.Random.Range((float) 0f, (float) 1f) <= App.Binder.ConfigMeta.TOURNAMENT_UPGRADE_EPIC_PROBABILITY);
                reward.Choices.Add(item);
            }
            return reward;
        }

        public double getPriceForChoice(int idx)
        {
            if (this.Choices[idx].IsEpicUpgrade)
            {
                return (double) App.Binder.ConfigMeta.TOURNAMENT_CARD_PRICE_EPIC;
            }
            return (double) App.Binder.ConfigMeta.TOURNAMENT_CARD_PRICE_NORMAL;
        }

        public string getTrackingIdForChoice(int idx)
        {
            if (this.Choices[idx].IsEpicUpgrade)
            {
                return (this.Choices[idx].TourmanentUpgradeId + "_Epic");
            }
            return (this.Choices[idx].TourmanentUpgradeId + "_Normal");
        }

        public bool hasUpgradeIdChoice(string id)
        {
            for (int i = 0; i < this.Choices.Count; i++)
            {
                if (this.Choices[i].TourmanentUpgradeId == id)
                {
                    return true;
                }
            }
            return false;
        }

        [Serializable]
        public class Entry
        {
            public bool IsEpicUpgrade;
            public bool Selected;
            public string TourmanentUpgradeId;
        }
    }
}

