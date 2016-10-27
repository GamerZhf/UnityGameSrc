namespace App
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class ConfigAchievements
    {
        public static List<string> ACHIEVEMENT_IDS;
        public static int MAX_TIER = 3;
        public static Dictionary<string, SharedData> SHARED_DATA;

        static ConfigAchievements()
        {
            Dictionary<string, SharedData> dictionary = new Dictionary<string, SharedData>();
            SharedData data = new SharedData();
            data.Title = ConfigLoca.ACHIEVEMENT_COLLECT_COINS_TITLE;
            data.Description = ConfigLoca.ACHIEVEMENT_COLLECT_COINS_DESCRIPTION;
            Dictionary<int, double> dictionary2 = new Dictionary<int, double>();
            dictionary2.Add(1, 10000.0);
            dictionary2.Add(2, 15000000.0);
            dictionary2.Add(3, 25000000000);
            data.TierRequirements = dictionary2;
            data.RewardResource = ResourceType.Diamond;
            data.Progress = new AchievementProgression(Achievements.Progress_CollectCoins);
            dictionary.Add("CollectCoins", data);
            data = new SharedData();
            data.Title = ConfigLoca.ACHIEVEMENT_COMPLETE_FLOORS_TITLE;
            data.Description = ConfigLoca.ACHIEVEMENT_COMPLETE_FLOORS_DESCRIPTION;
            dictionary2 = new Dictionary<int, double>();
            dictionary2.Add(1, 25.0);
            dictionary2.Add(2, 250.0);
            dictionary2.Add(3, 2000.0);
            data.TierRequirements = dictionary2;
            data.RewardResource = ResourceType.Diamond;
            data.Progress = new AchievementProgression(Achievements.Progress_CompleteFloors);
            dictionary.Add("CompleteFloors", data);
            data = new SharedData();
            data.Title = ConfigLoca.ACHIEVEMENT_KILL_ENEMIES_TITLE;
            data.Description = ConfigLoca.ACHIEVEMENT_KILL_ENEMIES_DESCRIPTION;
            dictionary2 = new Dictionary<int, double>();
            dictionary2.Add(1, 1000.0);
            dictionary2.Add(2, 50000.0);
            dictionary2.Add(3, 350000.0);
            data.TierRequirements = dictionary2;
            data.RewardResource = ResourceType.Diamond;
            data.Progress = new AchievementProgression(Achievements.Progress_KillEnemies);
            dictionary.Add("KillEnemies", data);
            data = new SharedData();
            data.Title = ConfigLoca.ACHIEVEMENT_MULTIKILL_ENEMIES_TITLE;
            data.Description = ConfigLoca.ACHIEVEMENT_MULTIKILL_ENEMIES_DESCRIPTION;
            dictionary2 = new Dictionary<int, double>();
            dictionary2.Add(1, 50.0);
            dictionary2.Add(2, 1000.0);
            dictionary2.Add(3, 20000.0);
            data.TierRequirements = dictionary2;
            data.RewardResource = ResourceType.Diamond;
            data.Progress = new AchievementProgression(Achievements.Progress_MultikillEnemies);
            dictionary.Add("MultikillEnemies", data);
            data = new SharedData();
            data.Title = ConfigLoca.ACHIEVEMENT_BEAT_BOSS_TITLE;
            data.Description = ConfigLoca.ACHIEVEMENT_BEAT_BOSS_DESCRIPTION;
            dictionary2 = new Dictionary<int, double>();
            dictionary2.Add(1, 50.0);
            dictionary2.Add(2, 170.0);
            dictionary2.Add(3, 350.0);
            data.TierRequirements = dictionary2;
            data.RewardResource = ResourceType.Diamond;
            data.Progress = new AchievementProgression(Achievements.Progress_BeatBoss);
            data.BooleanProgress = true;
            dictionary.Add("BeatBoss", data);
            data = new SharedData();
            data.Title = ConfigLoca.ACHIEVEMENT_IDENTIFY_ITEMS_TITLE;
            data.Description = ConfigLoca.ACHIEVEMENT_IDENTIFY_ITEMS_DESCRIPTION;
            dictionary2 = new Dictionary<int, double>();
            dictionary2.Add(1, 10.0);
            dictionary2.Add(2, 75.0);
            dictionary2.Add(3, 400.0);
            data.TierRequirements = dictionary2;
            data.RewardResource = ResourceType.Diamond;
            data.Progress = new AchievementProgression(Achievements.Progress_IdentifyItems);
            dictionary.Add("IdentifyItems", data);
            data = new SharedData();
            data.Title = ConfigLoca.ACHIEVEMENT_UPGRADE_ITEMS_TITLE;
            data.Description = ConfigLoca.ACHIEVEMENT_UPGRADE_ITEMS_DESCRIPTION;
            dictionary2 = new Dictionary<int, double>();
            dictionary2.Add(1, 300.0);
            dictionary2.Add(2, 1000.0);
            dictionary2.Add(3, 5000.0);
            data.TierRequirements = dictionary2;
            data.RewardResource = ResourceType.Diamond;
            data.Progress = new AchievementProgression(Achievements.Progress_UpgradeItems);
            dictionary.Add("UpgradeItems", data);
            data = new SharedData();
            data.Title = ConfigLoca.ACHIEVEMENT_OPEN_SILVER_CHESTS_TITLE;
            data.Description = ConfigLoca.ACHIEVEMENT_OPEN_SILVER_CHESTS_DESCRIPTION;
            dictionary2 = new Dictionary<int, double>();
            dictionary2.Add(1, 10.0);
            dictionary2.Add(2, 40.0);
            dictionary2.Add(3, 125.0);
            data.TierRequirements = dictionary2;
            data.RewardResource = ResourceType.Diamond;
            data.Progress = new AchievementProgression(Achievements.Progress_OpenSilverChests);
            dictionary.Add("OpenSilverChests", data);
            data = new SharedData();
            data.Title = ConfigLoca.ACHIEVEMENT_RETIRE_HERO_TITLE;
            data.Description = ConfigLoca.ACHIEVEMENT_RETIRE_HERO_DESCRIPTION;
            dictionary2 = new Dictionary<int, double>();
            dictionary2.Add(1, 2.0);
            dictionary2.Add(2, 10.0);
            dictionary2.Add(3, 25.0);
            data.TierRequirements = dictionary2;
            data.RewardResource = ResourceType.Diamond;
            data.Progress = new AchievementProgression(Achievements.Progress_RetireHero);
            dictionary.Add("RetireHero", data);
            data = new SharedData();
            data.Title = ConfigLoca.ACHIEVEMENT_RANKUPS_TITLE;
            data.Description = ConfigLoca.ACHIEVEMENT_RANKUPS_DESCRIPTION;
            dictionary2 = new Dictionary<int, double>();
            dictionary2.Add(1, 8.0);
            dictionary2.Add(2, 15.0);
            dictionary2.Add(3, 25.0);
            data.TierRequirements = dictionary2;
            data.RewardResource = ResourceType.Diamond;
            data.Progress = new AchievementProgression(Achievements.Progress_RankUps);
            dictionary.Add("RankUps", data);
            SHARED_DATA = dictionary;
            ACHIEVEMENT_IDS = new List<string>(SHARED_DATA.Keys);
        }

        public static int GetTotalNumAchievements()
        {
            return (ACHIEVEMENT_IDS.Count * MAX_TIER);
        }

        public delegate float AchievementProgression(Player player, string id, int tier, out double current, out double required);

        public class SharedData
        {
            public bool BooleanProgress;
            public string Description;
            public ConfigAchievements.AchievementProgression Progress;
            public ResourceType RewardResource;
            public Dictionary<int, double> TierRequirements;
            public string Title;
        }
    }
}

