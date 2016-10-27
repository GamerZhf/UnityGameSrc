namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class PlayerLoader
    {
        public const string DEFAULT_PLAYER_DATA_JSON_FILE = "Players/humanPlayer1";

        public static Player CreateAndMigrate(string json, out MigrationResult migrationResult)
        {
            Player player = null;
            migrationResult = MigrationResult.Unspecified;
            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    if (IsSl1LegacyPlayer(json))
                    {
                        Debug.Log("Legacy player data detected, skipping data migration..");
                        migrationResult = MigrationResult.LegacyPlayer;
                        return null;
                    }
                    int version = JsonUtils.Deserialize<Versionable>(json, true).Version;
                    if (version < ConfigApp.InternalClientVersion)
                    {
                        if (!ConfigApp.MigratePlayerProgressUponDataModelVersionChange)
                        {
                            json = ResourceUtil.LoadSafe<TextAsset>("Players/humanPlayer1", false).text;
                            migrationResult = MigrationResult.OverridenWithDefaultData;
                        }
                        else
                        {
                            json = PreMigrateToCurrentDataModelVersion(version, json);
                            migrationResult = MigrationResult.Migrated;
                        }
                    }
                    else
                    {
                        migrationResult = MigrationResult.UpToDate;
                    }
                    player = JsonUtils.Deserialize<Player>(json, true);
                    if (((player.Version == 0) && (player.CharacterInstances != null)) && (player.CharacterInstances.Count == 0))
                    {
                        migrationResult = MigrationResult.Repaired;
                        Player player2 = CreateNew();
                        player2.Version = ConfigApp.InternalClientVersion;
                        return player2;
                    }
                    if ((version < ConfigApp.InternalClientVersion) && ConfigApp.MigratePlayerProgressUponDataModelVersionChange)
                    {
                        PostMigrateToCurrentDataModelVersion(player, json);
                    }
                    player.Version = ConfigApp.InternalClientVersion;
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(string.Concat(new object[] { "CreateAndMigrate error: ", exception, " -- ", json }));
            }
            if (((player == null) || (player.CharacterInstances == null)) || (player.CharacterInstances.Count == 0))
            {
                player = null;
                migrationResult = MigrationResult.Error;
            }
            return player;
        }

        public static Player CreateNew()
        {
            return JsonUtils.Deserialize<Player>(ResourceUtil.LoadSafe<TextAsset>("Players/humanPlayer1", false).text, true);
        }

        public static Player CreateProtoPlayer()
        {
            Player player = CreateNew();
            player.Version = ConfigApp.InternalClientVersion;
            player.Rank = 50;
            player.Resources[ResourceType.Diamond.ToString()] = 25.0;
            foreach (KeyValuePair<string, ConfigAchievements.SharedData> pair in ConfigAchievements.SHARED_DATA)
            {
                player.Achievements.Notified.Add(pair.Key, 1);
                player.Achievements.Claimed.Add(pair.Key, 1);
            }
            for (int i = 1; i <= 15; i++)
            {
                PetInstance instance3 = new PetInstance();
                instance3.CharacterId = "Pet" + i.ToString("000");
                instance3.Level = 1;
                instance3.InspectedByPlayer = true;
                PetInstance item = instance3;
                player.Pets.Instances.Add(item);
            }
            player.CumulativeRetiredHeroStats.HighestFloor = 0x3e8;
            player.CumulativeRetiredHeroStats.HeroesRetired = 3;
            player.CumulativeRetiredHeroStats.HighestTokenGainWithRetirement = 1000.0;
            player.ActiveCharacter.Inventory.RevivePotions = 2;
            player.ActiveCharacter.Inventory.BossPotions = 0;
            player.ActiveCharacter.Inventory.FrenzyPotions = 1;
            IEnumerator enumerator = Enum.GetValues(typeof(ChestType)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    player.CumulativeRetiredHeroStats.EncounteredChestTypes.Add(((ChestType) ((int) enumerator.Current)).ToString());
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            player.UnlockedSkills.Clear();
            player.ActiveCharacter.SkillInstances.Clear();
            player.ActiveCharacter.ActiveSkillTypes.Clear();
            for (int j = 0; j < ConfigSkills.ALL_HERO_SKILLS.Count; j++)
            {
                SkillType type2 = ConfigSkills.ALL_HERO_SKILLS[j];
                if (player.Rank >= ConfigSkills.SHARED_DATA[type2].UnlockRank)
                {
                    player.UnlockedSkills.Add(type2);
                    SkillInstance instance4 = new SkillInstance();
                    instance4.SkillType = type2;
                    instance4.Rank = 1;
                    instance4.InspectedByPlayer = true;
                    player.ActiveCharacter.SkillInstances.Add(instance4);
                }
            }
            player.ActiveCharacter.ActiveSkillTypes.Add(SkillType.Slam);
            player.ActiveCharacter.ActiveSkillTypes.Add(SkillType.Leap);
            player.ActiveCharacter.ActiveSkillTypes.Add(SkillType.Clone);
            player.Runestones.RunestoneInstances.Clear();
            for (int k = 0; k < ConfigRunestones.RUNESTONES.Length; k++)
            {
                if (player.Rank >= ConfigRunestones.RUNESTONES[k].UnlockRank)
                {
                    RunestoneInstance instance5 = new RunestoneInstance();
                    instance5.Id = ConfigRunestones.RUNESTONES[k].Id;
                    instance5.Level = 1;
                    instance5.Unlocked = true;
                    instance5.InspectedByPlayer = true;
                    RunestoneInstance instance2 = instance5;
                    player.Runestones.RunestoneInstances.Add(instance2);
                }
            }
            ConfigTutorials.CheatCompleteAllFtueTutorials(player);
            ConfigTutorials.CheatCompleteAllContextTutorials(player);
            return player;
        }

        private static bool IsSl1LegacyPlayer(string json)
        {
            return (JsonUtils.Deserialize<LegacyPlayerFingerprint>(json, true).RelicIds != null);
        }

        private static string JsonMigrate_v0_to_v1(string from)
        {
            return from;
        }

        private static string JsonMigrate_v1_to_v2(string from)
        {
            return from;
        }

        private static string JsonMigrate_v10_to_v11(string from)
        {
            return from;
        }

        private static string JsonMigrate_v2_to_v3(string from)
        {
            return from;
        }

        private static string JsonMigrate_v3_to_v4(string from)
        {
            return from;
        }

        private static string JsonMigrate_v4_to_v5(string from)
        {
            return from;
        }

        private static string JsonMigrate_v5_to_v6(string from)
        {
            return from;
        }

        private static string JsonMigrate_v6_to_v7(string from)
        {
            return from;
        }

        private static string JsonMigrate_v7_to_v8(string from)
        {
            return from;
        }

        private static string JsonMigrate_v8_to_v9(string from)
        {
            return from;
        }

        private static string JsonMigrate_v9_to_v10(string from)
        {
            return from;
        }

        private static Player Load()
        {
            Player player = null;
            string json = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;
            bool flag = IOUtil.FileExistsInPersistentStorage(ConfigApp.LocalPlayerProfileFilePrimary);
            bool flag2 = IOUtil.FileExistsInPersistentStorage(ConfigApp.LocalPlayerProfileFileBackup);
            bool flag3 = false;
            if (flag)
            {
                try
                {
                    MigrationResult result;
                    json = IOUtil.LoadFromPersistentStorage(ConfigApp.LocalPlayerProfileFilePrimary);
                    player = CreateAndMigrate(json, out result);
                    if (result == MigrationResult.Error)
                    {
                        flag3 = true;
                    }
                }
                catch (Exception exception)
                {
                    flag3 = true;
                    str3 = str3 + exception;
                }
            }
            bool flag4 = false;
            if ((player == null) && flag2)
            {
                try
                {
                    MigrationResult result2;
                    str2 = IOUtil.LoadFromPersistentStorage(ConfigApp.LocalPlayerProfileFileBackup);
                    player = CreateAndMigrate(str2, out result2);
                    if (result2 == MigrationResult.Error)
                    {
                        flag4 = true;
                    }
                }
                catch (Exception exception2)
                {
                    flag4 = true;
                    str3 = str3 + exception2;
                }
            }
            if (flag3 && flag4)
            {
                Debug.LogError("Error in player data PRIMARY_FILE and BACKUP_FILE loading:\n\nexception=" + str3 + "\n\nprimaryJson=" + json + "\n\nbackupJson=" + str2);
                return player;
            }
            if (flag3)
            {
                Debug.LogError("Error in player data PRIMARY_FILE loading:\n\nexception=" + str3 + "\n\nprimaryJson=" + json);
                return player;
            }
            if (flag4)
            {
                Debug.LogError("Error in player data BACKUP_FILE loading:\n\nexception=" + str3 + "\n\nbackupJson=" + str2);
            }
            return player;
        }

        public static Player LoadExistingOrCreateNew()
        {
            Player player = Load();
            if (player == null)
            {
                player = CreateNew();
            }
            return player;
        }

        private static void PlayerMigrate_v0_to_v1(Player player)
        {
            if (!player.CompletedTutorials.Contains("CTUT002"))
            {
                player.CompletedTutorials.Add("CTUT002");
            }
        }

        private static void PlayerMigrate_v1_to_v2(Player player)
        {
        }

        private static void PlayerMigrate_v10_to_v11(Player player)
        {
        }

        private static void PlayerMigrate_v2_to_v3(Player player)
        {
        }

        private static void PlayerMigrate_v3_to_v4(Player player)
        {
            for (int i = 0; i < player.CumulativeRetiredHeroStats.EncounteredChestTypes.Count; i++)
            {
                if (player.CumulativeRetiredHeroStats.EncounteredChestTypes[i] == ChestType.Wooden001.ToString())
                {
                    player.CumulativeRetiredHeroStats.EncounteredChestTypes[i] = ChestType.Basic001.ToString();
                }
            }
            for (int j = 0; j < player.UnclaimedRewards.Count; j++)
            {
                if (player.UnclaimedRewards[j].ChestType == ChestType.Wooden001)
                {
                    player.UnclaimedRewards[j].ChestType = ChestType.Basic001;
                }
            }
            for (int k = 0; k < player.CharacterInstances.Count; k++)
            {
                CharacterInstance instance = player.CharacterInstances[k];
                for (int num4 = 0; num4 < instance.HeroStats.EncounteredChestTypes.Count; num4++)
                {
                    if (instance.HeroStats.EncounteredChestTypes[num4] == ChestType.Wooden001.ToString())
                    {
                        instance.HeroStats.EncounteredChestTypes[num4] = ChestType.Basic001.ToString();
                    }
                }
            }
            for (int m = 0; m < player.Vendor.Inventory.Count; m++)
            {
                if (player.Vendor.Inventory[m].PrerolledChestType == ChestType.Wooden001)
                {
                    player.Vendor.Inventory[m].PrerolledChestType = ChestType.Basic001;
                }
            }
            if (player.CompletedTutorials.Contains("CTUT001"))
            {
                player.SocialData.HeroNamingCount = 1;
            }
            int num6 = 0;
            for (int n = player.Runestones.RunestoneInstances.Count - 1; n >= 0; n--)
            {
                RunestoneInstance instance2 = player.Runestones.RunestoneInstances[n];
                ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(instance2.Id);
                num6 += ConfigRunestones.RUNESTONE_UPDATE_V3_RARITY_TO_V4_GEM_REWARD[runestoneData.Rarity];
                if (runestoneData.LinkedToSkill == SkillType.NONE)
                {
                    player.Runestones.RunestoneInstances.RemoveAt(n);
                }
                else
                {
                    instance2.InspectedByPlayer = false;
                }
            }
            if (num6 > 0)
            {
                Reward item = new Reward();
                item.ChestType = ChestType.RunestoneUpdateReward;
                item.addResourceDrop(ResourceType.Diamond, (double) num6);
                player.UnclaimedUpdateRewards.Add(item);
            }
            player.ActiveCharacter.Inventory.FrenzyPotions = player.UnusedFrenzyPotions;
            player.ActiveCharacter.Inventory.RevivePotions = player.UnusedFreeRevives;
        }

        private static void PlayerMigrate_v4_to_v5(Player player)
        {
        }

        private static void PlayerMigrate_v5_to_v6(Player player)
        {
            Reward reward = player.getFirstUnclaimedRetirementTriggerChest();
            if (reward != null)
            {
                reward.FrenzyPotions = 1 + (player.LastCompletedFloor / 60);
            }
            player.rerollRetirementRandomSeed();
        }

        private static void PlayerMigrate_v6_to_v7(Player player)
        {
            if (player.HasPurchasedStarterBundle)
            {
                PetInstance item = new PetInstance();
                item.CharacterId = "Pet001";
                item.Level = 1;
                player.Pets.Instances.Add(item);
            }
            for (int i = 0; i < player.Runestones.SelectedRunestoneIds.Count; i++)
            {
                RunestoneSelection selection = new RunestoneSelection();
                selection.Id = player.Runestones.SelectedRunestoneIds[i];
                selection.Source = RunestoneSelectionSource.Player;
                player.Runestones.SelectedRunestones.Add(selection);
            }
            player.Runestones.SelectedRunestoneIds.Clear();
        }

        private static void PlayerMigrate_v7_to_v8(Player player)
        {
        }

        private static void PlayerMigrate_v8_to_v9(Player player)
        {
            Reward reward = player.getFirstUnclaimedRetirementTriggerChest();
            if (reward != null)
            {
                for (int i = 0; i < reward.FrenzyPotions; i++)
                {
                    Reward reward3 = new Reward();
                    reward3.ChestType = ChestType.RewardBoxCommon;
                    reward3.FrenzyPotions = 1;
                    Reward item = reward3;
                    player.UnclaimedRewards.Add(item);
                }
                reward.FrenzyPotions = 0;
            }
        }

        private static void PlayerMigrate_v9_to_v10(Player player)
        {
            player.Missions.MissionType = MissionType.Adventure;
        }

        private static void PostMigrateToCurrentDataModelVersion(Player player, string json)
        {
            if (Application.isPlaying)
            {
                Debug.Log(string.Concat(new object[] { "Post-migrating player profile from v", player.Version, " to v", ConfigApp.InternalClientVersion }));
            }
            if (player.Version < ConfigApp.InternalClientVersion)
            {
                if (player.Version <= 0)
                {
                    try
                    {
                        PlayerMigrate_v0_to_v1(player);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v0_to_v1: ", exception, " -- ", json }));
                    }
                }
                if (player.Version <= 1)
                {
                    try
                    {
                        PlayerMigrate_v1_to_v2(player);
                    }
                    catch (Exception exception2)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v1_to_v2: ", exception2, " -- ", json }));
                    }
                }
                if (player.Version <= 2)
                {
                    try
                    {
                        PlayerMigrate_v2_to_v3(player);
                    }
                    catch (Exception exception3)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v2_to_v3: ", exception3, " -- ", json }));
                    }
                }
                if (player.Version <= 3)
                {
                    try
                    {
                        PlayerMigrate_v3_to_v4(player);
                    }
                    catch (Exception exception4)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v3_to_v4: ", exception4, " -- ", json }));
                    }
                }
                if (player.Version <= 4)
                {
                    try
                    {
                        PlayerMigrate_v4_to_v5(player);
                    }
                    catch (Exception exception5)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v4_to_v5: ", exception5, " -- ", json }));
                    }
                }
                if (player.Version <= 5)
                {
                    try
                    {
                        PlayerMigrate_v5_to_v6(player);
                    }
                    catch (Exception exception6)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v5_to_v6: ", exception6, " -- ", json }));
                    }
                }
                if (player.Version <= 6)
                {
                    try
                    {
                        PlayerMigrate_v6_to_v7(player);
                    }
                    catch (Exception exception7)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v6_to_v7: ", exception7, " -- ", json }));
                    }
                }
                if (player.Version <= 7)
                {
                    try
                    {
                        PlayerMigrate_v7_to_v8(player);
                    }
                    catch (Exception exception8)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v7_to_v8: ", exception8, " -- ", json }));
                    }
                }
                if (player.Version <= 8)
                {
                    try
                    {
                        PlayerMigrate_v8_to_v9(player);
                    }
                    catch (Exception exception9)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v8_to_v9: ", exception9, " -- ", json }));
                    }
                }
                if (player.Version <= 9)
                {
                    try
                    {
                        PlayerMigrate_v9_to_v10(player);
                    }
                    catch (Exception exception10)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v9_to_v10: ", exception10, " -- ", json }));
                    }
                }
                if (player.Version <= 10)
                {
                    try
                    {
                        PlayerMigrate_v10_to_v11(player);
                    }
                    catch (Exception exception11)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error PlayerMigrate_v10_to_v11: ", exception11, " -- ", json }));
                    }
                }
            }
        }

        private static string PreMigrateToCurrentDataModelVersion(int version, string json)
        {
            if (Application.isPlaying)
            {
                Debug.Log(string.Concat(new object[] { "Pre-migrating player profile from v", version, " to v", ConfigApp.InternalClientVersion }));
            }
            if (version < ConfigApp.InternalClientVersion)
            {
                if (version <= 0)
                {
                    try
                    {
                        json = JsonMigrate_v0_to_v1(json);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v0_to_v1: ", exception, " -- ", json }));
                    }
                }
                if (version <= 1)
                {
                    try
                    {
                        json = JsonMigrate_v1_to_v2(json);
                    }
                    catch (Exception exception2)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v1_to_v2: ", exception2, " -- ", json }));
                    }
                }
                if (version <= 2)
                {
                    try
                    {
                        json = JsonMigrate_v2_to_v3(json);
                    }
                    catch (Exception exception3)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v2_to_v3: ", exception3, " -- ", json }));
                    }
                }
                if (version <= 3)
                {
                    try
                    {
                        json = JsonMigrate_v3_to_v4(json);
                    }
                    catch (Exception exception4)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v3_to_v4: ", exception4, " -- ", json }));
                    }
                }
                if (version <= 4)
                {
                    try
                    {
                        json = JsonMigrate_v4_to_v5(json);
                    }
                    catch (Exception exception5)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v4_to_v5: ", exception5, " -- ", json }));
                    }
                }
                if (version <= 5)
                {
                    try
                    {
                        json = JsonMigrate_v5_to_v6(json);
                    }
                    catch (Exception exception6)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v5_to_v6: ", exception6, " -- ", json }));
                    }
                }
                if (version <= 6)
                {
                    try
                    {
                        json = JsonMigrate_v6_to_v7(json);
                    }
                    catch (Exception exception7)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v6_to_v7: ", exception7, " -- ", json }));
                    }
                }
                if (version <= 7)
                {
                    try
                    {
                        json = JsonMigrate_v7_to_v8(json);
                    }
                    catch (Exception exception8)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v7_to_v8: ", exception8, " -- ", json }));
                    }
                }
                if (version <= 8)
                {
                    try
                    {
                        json = JsonMigrate_v8_to_v9(json);
                    }
                    catch (Exception exception9)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v8_to_v9: ", exception9, " -- ", json }));
                    }
                }
                if (version <= 9)
                {
                    try
                    {
                        json = JsonMigrate_v9_to_v10(json);
                    }
                    catch (Exception exception10)
                    {
                        Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v9_to_v10: ", exception10, " -- ", json }));
                    }
                }
                if (version > 10)
                {
                    return json;
                }
                try
                {
                    json = JsonMigrate_v10_to_v11(json);
                }
                catch (Exception exception11)
                {
                    Debug.LogError(string.Concat(new object[] { "Error JsonMigrate_v10_to_v11: ", exception11, " -- ", json }));
                }
            }
            return json;
        }

        public enum MigrationResult
        {
            Unspecified,
            UpToDate,
            Migrated,
            OverridenWithDefaultData,
            LegacyPlayer,
            Repaired,
            Error
        }
    }
}

