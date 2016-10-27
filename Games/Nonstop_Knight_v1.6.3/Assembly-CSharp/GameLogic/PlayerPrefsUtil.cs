namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class PlayerPrefsUtil
    {
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        private static bool GetObject<T>(string key, out T target, bool encrypted)
        {
            string str = PlayerPrefs.GetString(key);
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    if (encrypted)
                    {
                        str = AesEncryptor.Decrypt(str);
                    }
                    target = JsonUtils.Deserialize<T>(str, true);
                    return true;
                }
                catch (Exception)
                {
                    Debug.LogError("Cannot get object from PlayerPrefs: " + key);
                }
            }
            else
            {
                Debug.LogError("Player field not in PlayerPrefs: " + key);
            }
            target = default(T);
            return false;
        }

        public static void PopulatePlayerWithCriticalValues(Player player, bool encrypted)
        {
            int num;
            CharacterInstance[] instanceArray;
            Dictionary<string, double> dictionary;
            int num2;
            SkillType[] typeArray;
            string[] strArray;
            HeroStats stats;
            int num3;
            SkillType[] typeArray2;
            string[] strArray2;
            Achievements achievements;
            string[] strArray3;
            Dictionary<string, int> dictionary2;
            LeaderboardEntry[] entryArray;
            Runestones runestones;
            Reward[] rewardArray;
            if (GetObject<int>("Rank", out num, encrypted))
            {
                player.Rank = num;
            }
            if (GetObject<CharacterInstance[]>("CharacterInstances", out instanceArray, encrypted))
            {
                player.CharacterInstances = new List<CharacterInstance>(instanceArray);
            }
            if (GetObject<Dictionary<string, double>>("Resources", out dictionary, encrypted))
            {
                player.Resources = dictionary;
            }
            if (GetObject<int>("LastCompletedFloor", out num2, encrypted))
            {
                player.LastCompletedFloor = num2;
            }
            if (GetObject<SkillType[]>("UnlockedSkills", out typeArray, encrypted))
            {
                player.UnlockedSkills = new List<SkillType>(typeArray);
            }
            if (GetObject<string[]>("CompletedTutorials", out strArray, encrypted))
            {
                player.CompletedTutorials = new List<string>(strArray);
            }
            if (GetObject<HeroStats>("CumulativeRetiredHeroStats", out stats, encrypted))
            {
                player.CumulativeRetiredHeroStats = stats;
            }
            if (GetObject<int>("NumPendingRankUpCeremonies", out num3, encrypted))
            {
                player.NumPendingRankUpCeremonies = num3;
            }
            if (GetObject<SkillType[]>("PendingSkillUnlocks", out typeArray2, encrypted))
            {
                player.PendingSkillUnlocks = new List<SkillType>(typeArray2);
            }
            if (GetObject<string[]>("OneShotAnalyticsEvents", out strArray2, encrypted))
            {
                player.OneShotAnalyticsEvents = new List<string>(strArray2);
            }
            if (GetObject<Achievements>("Achievements", out achievements, encrypted))
            {
                player.Achievements = achievements;
            }
            if (GetObject<string[]>("ClaimedIapTransactionIds", out strArray3, encrypted))
            {
                player.ClaimedIapTransactionIds = new List<string>(strArray3);
            }
            if (GetObject<Dictionary<string, int>>("BestedLeaderboardUserIds", out dictionary2, encrypted))
            {
                player.BestedLeaderboardUserIds = new Dictionary<string, int>(dictionary2);
            }
            if (GetObject<LeaderboardEntry[]>("UnclaimedLeaderboardRewards", out entryArray, encrypted))
            {
                player.UnclaimedLeaderboardRewards = new List<LeaderboardEntry>(entryArray);
            }
            if (GetObject<Runestones>("Upgrades", out runestones, encrypted))
            {
                player.Runestones = runestones;
            }
            if (GetObject<Reward[]>("UnclaimedRewards", out rewardArray, encrypted))
            {
                player.UnclaimedRewards = new List<Reward>(rewardArray);
            }
        }

        public static void SaveCriticalPlayerValues(Player player, bool encrypted)
        {
            StoreObject("Rank", player.Rank, encrypted);
            StoreObject("CharacterInstances", player.CharacterInstances, encrypted);
            StoreObject("Resources", player.Resources, encrypted);
            StoreObject("LastCompletedFloor", player.LastCompletedFloor, encrypted);
            StoreObject("UnlockedSkills", player.UnlockedSkills, encrypted);
            StoreObject("CompletedTutorials", player.CompletedTutorials, encrypted);
            StoreObject("CumulativeRetiredHeroStats", player.CumulativeRetiredHeroStats, encrypted);
            StoreObject("NumPendingRankUpCeremonies", player.NumPendingRankUpCeremonies, encrypted);
            StoreObject("PendingSkillUnlocks", player.PendingSkillUnlocks, encrypted);
            StoreObject("OneShotAnalyticsEvents", player.OneShotAnalyticsEvents, encrypted);
            StoreObject("Achievements", player.Achievements, encrypted);
            StoreObject("ClaimedIapTransactionIds", player.ClaimedIapTransactionIds, encrypted);
            StoreObject("BestedLeaderboardUserIds", player.BestedLeaderboardUserIds, encrypted);
            StoreObject("UnclaimedLeaderboardRewards", player.UnclaimedLeaderboardRewards, encrypted);
            StoreObject("Upgrades", player.Runestones, encrypted);
            StoreObject("UnclaimedRewards", player.UnclaimedRewards, encrypted);
            PlayerPrefs.Save();
        }

        private static void StoreObject(string key, object obj, bool encrypted)
        {
            string unencrypted = JsonUtils.Serialize(obj);
            try
            {
                if (encrypted)
                {
                    unencrypted = AesEncryptor.Encrypt(unencrypted);
                }
                PlayerPrefs.SetString(key, unencrypted);
            }
            catch (Exception)
            {
                Debug.LogError("Cannot store object to PlayerPrefs: key=" + key + " -- json=" + unencrypted);
            }
        }
    }
}

