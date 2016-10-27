namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;

    public class AchievementThirdPartyService
    {
        private static readonly Dictionary<string, string> GPLAY_MAPPING;
        private static readonly HashSet<string> REPORTED;

        static AchievementThirdPartyService()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("CollectCoins", "CgkI7oWAl_wcEAIQAQ");
            dictionary.Add("CompleteFloors", "CgkI7oWAl_wcEAIQAg");
            dictionary.Add("KillEnemies", "CgkI7oWAl_wcEAIQAw");
            dictionary.Add("MultikillEnemies", "CgkI7oWAl_wcEAIQBA");
            dictionary.Add("BeatBoss", "CgkI7oWAl_wcEAIQBQ");
            dictionary.Add("IdentifyItems", "CgkI7oWAl_wcEAIQBg");
            dictionary.Add("UpgradeItems", "CgkI7oWAl_wcEAIQBw");
            dictionary.Add("OpenSilverChests", "CgkI7oWAl_wcEAIQCA");
            dictionary.Add("RetireHero", "CgkI7oWAl_wcEAIQCQ");
            dictionary.Add("RankUps", "CgkI7oWAl_wcEAIQCg");
            GPLAY_MAPPING = dictionary;
            REPORTED = new HashSet<string>();
        }

        public void CompleteAchievement(string _id)
        {
            if (!REPORTED.Contains(_id))
            {
                string str = null;
                GPLAY_MAPPING.TryGetValue(_id, out str);
                if (str == null)
                {
                }
                if (App.Binder.SocialSystem.ReportAchievement(_id, 1f))
                {
                    REPORTED.Add(_id);
                }
            }
        }

        public void UnlockAchievements()
        {
            int tier = ConfigAchievements.MAX_TIER;
            Player player = GameLogic.Binder.GameState.Player;
            if (player != null)
            {
                foreach (KeyValuePair<string, ConfigAchievements.SharedData> pair in ConfigAchievements.SHARED_DATA)
                {
                    double num2;
                    double num3;
                    if (!REPORTED.Contains(pair.Key) && (pair.Value.Progress(player, pair.Key, tier, out num2, out num3) >= 1f))
                    {
                        this.CompleteAchievement(pair.Key);
                    }
                }
            }
        }
    }
}

