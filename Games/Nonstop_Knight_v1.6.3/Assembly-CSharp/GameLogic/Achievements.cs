namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Achievements : IJsonData
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        public Dictionary<string, int> Claimed;
        public Dictionary<string, int> Notified;

        public Achievements()
        {
            this.Claimed = new Dictionary<string, int>();
            this.Notified = new Dictionary<string, int>();
        }

        public Achievements(Achievements another)
        {
            this.Claimed = new Dictionary<string, int>();
            this.Notified = new Dictionary<string, int>();
            this.copyFrom(another);
        }

        public bool canComplete(string id, int tier)
        {
            double num;
            double num2;
            if (this.isClaimed(id, tier))
            {
                return false;
            }
            if (tier > ConfigAchievements.MAX_TIER)
            {
                return false;
            }
            return (ConfigAchievements.SHARED_DATA[id].Progress(this.Player, id, tier, out num, out num2) >= 1f);
        }

        public void copyFrom(Achievements another)
        {
            this.Claimed = new Dictionary<string, int>(another.Claimed);
            this.Notified = new Dictionary<string, int>(another.Notified);
        }

        public int getCurrentTier(string id)
        {
            if (!this.Claimed.ContainsKey(id))
            {
                return 1;
            }
            return Mathf.Clamp(this.Claimed[id] + 1, 1, ConfigAchievements.MAX_TIER);
        }

        public string getFirstCompletableAchievement(out int tier)
        {
            for (int i = 0; i < ConfigAchievements.ACHIEVEMENT_IDS.Count; i++)
            {
                string id = ConfigAchievements.ACHIEVEMENT_IDS[i];
                tier = this.getCurrentTier(id);
                if (this.canComplete(id, tier))
                {
                    return id;
                }
            }
            tier = 0;
            return null;
        }

        public string getFirstCompletableNonNotifiedAchievement(out int tier)
        {
            for (int i = 0; i < ConfigAchievements.ACHIEVEMENT_IDS.Count; i++)
            {
                string id = ConfigAchievements.ACHIEVEMENT_IDS[i];
                tier = this.getCurrentTier(id);
                if (this.canComplete(id, tier) && !this.isNotified(id, tier))
                {
                    return id;
                }
            }
            tier = 0;
            return null;
        }

        public int getNumberOfCompletableAchievements()
        {
            int num = 0;
            for (int i = 0; i < ConfigAchievements.ACHIEVEMENT_IDS.Count; i++)
            {
                string id = ConfigAchievements.ACHIEVEMENT_IDS[i];
                for (int j = this.getCurrentTier(id); j <= ConfigAchievements.MAX_TIER; j++)
                {
                    if (!this.canComplete(id, j))
                    {
                        break;
                    }
                    num++;
                }
            }
            return num;
        }

        public bool isAtMaxTier(string id)
        {
            return (this.getCurrentTier(id) >= ConfigAchievements.MAX_TIER);
        }

        public bool isClaimed(string id, int tier)
        {
            return (this.Claimed.ContainsKey(id) && (this.Claimed[id] >= tier));
        }

        public bool isNotified(string id, int tier)
        {
            return (this.Notified.ContainsKey(id) && (this.Notified[id] >= tier));
        }

        public float normalizedAllAchievementCompletion()
        {
            return Mathf.Clamp01(((float) this.numAchievementsCompleted()) / ((float) ConfigAchievements.GetTotalNumAchievements()));
        }

        public int numAchievementsCompleted()
        {
            int num = 0;
            for (int i = 0; i < ConfigAchievements.ACHIEVEMENT_IDS.Count; i++)
            {
                string key = ConfigAchievements.ACHIEVEMENT_IDS[i];
                if (this.Claimed.ContainsKey(key))
                {
                    num += this.Claimed[key];
                }
            }
            return num;
        }

        public void postDeserializeInitialization()
        {
        }

        public static float Progress_BeatBoss(GameLogic.Player player, string id, int tier, out double current, out double required)
        {
            int num = player.getHighestFloorReached();
            required = ConfigAchievements.SHARED_DATA[id].TierRequirements[tier];
            if (num > required)
            {
                current = required;
            }
            else
            {
                current = 0.0;
            }
            return Mathf.Clamp01((float) (current / required));
        }

        public static float Progress_CollectCoins(GameLogic.Player player, string id, int tier, out double current, out double required)
        {
            current = player.CumulativeRetiredHeroStats.CoinsEarned + player.ActiveCharacter.HeroStats.CoinsEarned;
            required = ConfigAchievements.SHARED_DATA[id].TierRequirements[tier];
            return Mathf.Clamp01((float) (current / required));
        }

        public static float Progress_CompleteFloors(GameLogic.Player player, string id, int tier, out double current, out double required)
        {
            current = player.CumulativeRetiredHeroStats.FloorsCompleted + player.ActiveCharacter.HeroStats.FloorsCompleted;
            required = ConfigAchievements.SHARED_DATA[id].TierRequirements[tier];
            return Mathf.Clamp01((float) (current / required));
        }

        public static float Progress_IdentifyItems(GameLogic.Player player, string id, int tier, out double current, out double required)
        {
            current = player.CumulativeRetiredHeroStats.ItemsUnlocked + player.ActiveCharacter.HeroStats.ItemsUnlocked;
            required = ConfigAchievements.SHARED_DATA[id].TierRequirements[tier];
            return Mathf.Clamp01((float) (current / required));
        }

        public static float Progress_KillEnemies(GameLogic.Player player, string id, int tier, out double current, out double required)
        {
            current = player.CumulativeRetiredHeroStats.MonstersKilled + player.ActiveCharacter.HeroStats.MonstersKilled;
            required = ConfigAchievements.SHARED_DATA[id].TierRequirements[tier];
            return Mathf.Clamp01((float) (current / required));
        }

        public static float Progress_MultikillEnemies(GameLogic.Player player, string id, int tier, out double current, out double required)
        {
            current = player.CumulativeRetiredHeroStats.MultikilledMonsters + player.ActiveCharacter.HeroStats.MultikilledMonsters;
            required = ConfigAchievements.SHARED_DATA[id].TierRequirements[tier];
            return Mathf.Clamp01((float) (current / required));
        }

        public static float Progress_OpenSilverChests(GameLogic.Player player, string id, int tier, out double current, out double required)
        {
            current = player.CumulativeRetiredHeroStats.SilverChestsOpened + player.ActiveCharacter.HeroStats.SilverChestsOpened;
            required = ConfigAchievements.SHARED_DATA[id].TierRequirements[tier];
            return Mathf.Clamp01((float) (current / required));
        }

        public static float Progress_RankUps(GameLogic.Player player, string id, int tier, out double current, out double required)
        {
            current = player.CumulativeRetiredHeroStats.RankUps + player.ActiveCharacter.HeroStats.RankUps;
            required = ConfigAchievements.SHARED_DATA[id].TierRequirements[tier];
            return Mathf.Clamp01((float) (current / required));
        }

        public static float Progress_RetireHero(GameLogic.Player player, string id, int tier, out double current, out double required)
        {
            current = player.CumulativeRetiredHeroStats.HeroesRetired;
            required = ConfigAchievements.SHARED_DATA[id].TierRequirements[tier];
            return Mathf.Clamp01((float) (current / required));
        }

        public static float Progress_UpgradeItems(GameLogic.Player player, string id, int tier, out double current, out double required)
        {
            current = player.CumulativeRetiredHeroStats.ItemUpgrades + player.ActiveCharacter.HeroStats.ItemUpgrades;
            required = ConfigAchievements.SHARED_DATA[id].TierRequirements[tier];
            return Mathf.Clamp01((float) (current / required));
        }

        [JsonIgnore]
        public GameLogic.Player Player
        {
            [CompilerGenerated]
            get
            {
                return this.<Player>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Player>k__BackingField = value;
            }
        }
    }
}

