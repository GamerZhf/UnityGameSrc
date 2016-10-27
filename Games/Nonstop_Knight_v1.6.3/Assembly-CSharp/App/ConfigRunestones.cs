namespace App
{
    using GameLogic;
    using System;
    using System.Collections.Generic;

    public static class ConfigRunestones
    {
        public static Dictionary<SkillType, SpriteAtlasEntry> BASIC_RUNESTONE_SPRITES;
        public static Dictionary<int, int> RUNESTONE_UPDATE_V3_RARITY_TO_V4_GEM_REWARD;
        public static SharedData[] RUNESTONES;
        private static List<SharedData> sm_tempList;
        private static SharedData.UnlockRankComparer sm_unlockRankComparer;

        static ConfigRunestones()
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            dictionary.Add(0, 0);
            dictionary.Add(1, 3);
            dictionary.Add(2, 7);
            dictionary.Add(3, 20);
            RUNESTONE_UPDATE_V3_RARITY_TO_V4_GEM_REWARD = dictionary;
            SharedData[] dataArray1 = new SharedData[0x18];
            SharedData data = new SharedData();
            data.UnlockRank = 3;
            data.Id = "Runestone001";
            data.Rarity = 1;
            SpriteAtlasEntry entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_whirlwind_1";
            data.Sprite = entry;
            PerkInstance instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeWhirlwind2;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeWhirlwind2);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Whirlwind;
            dataArray1[0] = data;
            data = new SharedData();
            data.UnlockRank = 4;
            data.Id = "Runestone003";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_whirlwind_2";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeWhirlwind3;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeWhirlwind3);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Whirlwind;
            dataArray1[1] = data;
            data = new SharedData();
            data.UnlockRank = 14;
            data.Id = "Runestone002";
            data.Rarity = 2;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_whirlwind_3";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeWhirlwind1;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeWhirlwind1);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Whirlwind;
            dataArray1[2] = data;
            data = new SharedData();
            data.UnlockRank = 0x18;
            data.Id = "Runestone019";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_whirlwind_4";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeWhirlwind4;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeWhirlwind4);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Whirlwind;
            dataArray1[3] = data;
            data = new SharedData();
            data.UnlockRank = 5;
            data.Id = "Runestone004";
            data.Rarity = 1;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_leap_1";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeLeap1;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeLeap1);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Leap;
            dataArray1[4] = data;
            data = new SharedData();
            data.UnlockRank = 6;
            data.Id = "Runestone005";
            data.Rarity = 2;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_leap_2";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeLeap2;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeLeap2);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Leap;
            dataArray1[5] = data;
            data = new SharedData();
            data.UnlockRank = 0x16;
            data.Id = "Runestone006";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_leap_3";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeLeap3;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeLeap3);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Leap;
            dataArray1[6] = data;
            data = new SharedData();
            data.UnlockRank = 0x21;
            data.Id = "Runestone020";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_leap_4";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeLeap4;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeLeap4);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Leap;
            dataArray1[7] = data;
            data = new SharedData();
            data.UnlockRank = 8;
            data.Id = "Runestone008";
            data.Rarity = 2;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_clone_2";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeClone2;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeClone2);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Clone;
            dataArray1[8] = data;
            data = new SharedData();
            data.UnlockRank = 0x10;
            data.Id = "Runestone007";
            data.Rarity = 1;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_clone_1";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeClone1;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeClone1);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Clone;
            dataArray1[9] = data;
            data = new SharedData();
            data.UnlockRank = 0x1f;
            data.Id = "Runestone009";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_clone_3";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeClone3;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeClone3);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Clone;
            dataArray1[10] = data;
            data = new SharedData();
            data.UnlockRank = 0x29;
            data.Id = "Runestone021";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_clone_4";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeClone4;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeClone4);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Clone;
            dataArray1[11] = data;
            data = new SharedData();
            data.UnlockRank = 11;
            data.Id = "Runestone010";
            data.Rarity = 1;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_slam_1";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeSlam1;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeSlam1);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Slam;
            dataArray1[12] = data;
            data = new SharedData();
            data.UnlockRank = 12;
            data.Id = "Runestone022";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_slam_4";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeSlam4;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeSlam4);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Slam;
            dataArray1[13] = data;
            data = new SharedData();
            data.UnlockRank = 0x19;
            data.Id = "Runestone011";
            data.Rarity = 2;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_slam_2";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeSlam2;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeSlam2);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Slam;
            dataArray1[14] = data;
            data = new SharedData();
            data.UnlockRank = 0x23;
            data.Id = "Runestone012";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_slam_3";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeSlam3;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeSlam3);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Slam;
            dataArray1[15] = data;
            data = new SharedData();
            data.UnlockRank = 0x13;
            data.Id = "Runestone013";
            data.Rarity = 1;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_omnislash_3";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeOmnislash3;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeOmnislash3);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Omnislash;
            dataArray1[0x10] = data;
            data = new SharedData();
            data.UnlockRank = 20;
            data.Id = "Runestone014";
            data.Rarity = 2;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_omnislash_1";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeOmnislash1;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeOmnislash1);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Omnislash;
            dataArray1[0x11] = data;
            data = new SharedData();
            data.UnlockRank = 0x26;
            data.Id = "Runestone015";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_omnislash_2";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeOmnislash2;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeOmnislash2);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Omnislash;
            dataArray1[0x12] = data;
            data = new SharedData();
            data.UnlockRank = 0x2f;
            data.Id = "Runestone023";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_omnislash_4";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeOmnislash4;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeOmnislash4);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Omnislash;
            dataArray1[0x13] = data;
            data = new SharedData();
            data.UnlockRank = 0x1c;
            data.Id = "Runestone016";
            data.Rarity = 1;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_implosion_1";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeImplosion2;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeImplosion2);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Implosion;
            dataArray1[20] = data;
            data = new SharedData();
            data.UnlockRank = 0x1d;
            data.Id = "Runestone017";
            data.Rarity = 2;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_implosion_2";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeImplosion1;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeImplosion1);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Implosion;
            dataArray1[0x15] = data;
            data = new SharedData();
            data.UnlockRank = 0x2c;
            data.Id = "Runestone018";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_implosion_3";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeImplosion4;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeImplosion4);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Implosion;
            dataArray1[0x16] = data;
            data = new SharedData();
            data.UnlockRank = 50;
            data.Id = "Runestone024";
            data.Rarity = 3;
            entry = new SpriteAtlasEntry();
            entry.AtlasId = "Menu";
            entry.SpriteId = "icon_rune_implosion_4";
            data.Sprite = entry;
            instance = new PerkInstance();
            instance.Type = PerkType.SkillUpgradeImplosion3;
            instance.Modifier = ConfigPerks.GetBestModifier(PerkType.SkillUpgradeImplosion3);
            data.PerkInstance = instance;
            data.LinkedToSkill = SkillType.Implosion;
            dataArray1[0x17] = data;
            RUNESTONES = dataArray1;
            Dictionary<SkillType, SpriteAtlasEntry> dictionary2 = new Dictionary<SkillType, SpriteAtlasEntry>(new SkillTypeBoxAvoidanceComparer());
            dictionary2.Add(SkillType.Whirlwind, new SpriteAtlasEntry("Menu", "icon_rune_whirlwind_0"));
            dictionary2.Add(SkillType.Leap, new SpriteAtlasEntry("Menu", "icon_rune_leap_0"));
            dictionary2.Add(SkillType.Clone, new SpriteAtlasEntry("Menu", "icon_rune_clone_0"));
            dictionary2.Add(SkillType.Slam, new SpriteAtlasEntry("Menu", "icon_rune_slam_0"));
            dictionary2.Add(SkillType.Omnislash, new SpriteAtlasEntry("Menu", "icon_rune_omnislash_0"));
            dictionary2.Add(SkillType.Implosion, new SpriteAtlasEntry("Menu", "icon_rune_implosion_0"));
            BASIC_RUNESTONE_SPRITES = dictionary2;
            sm_tempList = new List<SharedData>(0x20);
            sm_unlockRankComparer = new SharedData.UnlockRankComparer();
        }

        public static string GetDescription(string runestoneId)
        {
            SharedData runestoneData = GetRunestoneData(runestoneId);
            if (runestoneData.PerkInstance != null)
            {
                return ConfigPerks.SHARED_DATA[runestoneData.PerkInstance.Type].Description;
            }
            return "CHANGE ME";
        }

        public static string GetName(string runestoneId)
        {
            SharedData runestoneData = GetRunestoneData(runestoneId);
            if (runestoneData.PerkInstance != null)
            {
                return ConfigPerks.SHARED_DATA[runestoneData.PerkInstance.Type].Name;
            }
            return "CHANGE ME";
        }

        public static SharedData GetRandomRunestoneOfRarity(int rarity)
        {
            sm_tempList.Clear();
            for (int i = 0; i < RUNESTONES.Length; i++)
            {
                SharedData item = RUNESTONES[i];
                if (item.Rarity == rarity)
                {
                    sm_tempList.Add(item);
                }
            }
            if (sm_tempList.Count > 0)
            {
                return LangUtil.GetRandomValueFromList<SharedData>(sm_tempList);
            }
            return null;
        }

        public static SharedData GetRunestoneData(string runestoneId)
        {
            for (int i = 0; i < RUNESTONES.Length; i++)
            {
                if (RUNESTONES[i].Id == runestoneId)
                {
                    return RUNESTONES[i];
                }
            }
            return null;
        }

        public static SharedData GetRunestoneForLootTableRollId(string lootTableRollId)
        {
            SharedData runestoneData = GetRunestoneData(lootTableRollId);
            if (runestoneData != null)
            {
                return runestoneData;
            }
            return null;
        }

        public static string GetRunestoneId(SkillType skillType, int orderNumber)
        {
            List<SharedData> runestonesForSkillType = GetRunestonesForSkillType(skillType);
            return runestonesForSkillType[orderNumber % runestonesForSkillType.Count].Id;
        }

        public static int GetRunestoneOrderNumberForSkillType(string runestoneId, SkillType skillType)
        {
            List<SharedData> runestonesForSkillType = GetRunestonesForSkillType(skillType);
            for (int i = 0; i < runestonesForSkillType.Count; i++)
            {
                if (runestonesForSkillType[i].Id == runestoneId)
                {
                    return i;
                }
            }
            return -1;
        }

        public static List<SharedData> GetRunestonesForSkillType(SkillType skillType)
        {
            sm_tempList.Clear();
            for (int i = 0; i < RUNESTONES.Length; i++)
            {
                SharedData item = RUNESTONES[i];
                if (item.LinkedToSkill == skillType)
                {
                    sm_tempList.Add(item);
                }
            }
            sm_tempList.Sort(sm_unlockRankComparer);
            return sm_tempList;
        }

        public static string GetShortDescription(string runestoneId)
        {
            SharedData runestoneData = GetRunestoneData(runestoneId);
            if (runestoneData.PerkInstance != null)
            {
                return ConfigPerks.SHARED_DATA[runestoneData.PerkInstance.Type].ShortDescription;
            }
            return "CHANGE ME";
        }

        public static SkillType GetSkillTypeForRunestone(string runestoneId)
        {
            return GetRunestoneData(runestoneId).LinkedToSkill;
        }

        public static bool RunestoneIdIsValid(string runestoneId)
        {
            for (int i = 0; i < RUNESTONES.Length; i++)
            {
                if (RUNESTONES[i].Id == runestoneId)
                {
                    return true;
                }
            }
            return false;
        }

        public class SharedData : IBuffIconProvider
        {
            public string Id;
            public SkillType LinkedToSkill;
            public GameLogic.PerkInstance PerkInstance;
            public int Rarity;
            public SpriteAtlasEntry Sprite;
            public int UnlockRank;

            public string getSpriteId()
            {
                return this.Sprite.SpriteId;
            }

            public class UnlockRankComparer : IComparer<ConfigRunestones.SharedData>
            {
                public int Compare(ConfigRunestones.SharedData x, ConfigRunestones.SharedData y)
                {
                    if (x.UnlockRank < y.UnlockRank)
                    {
                        return -1;
                    }
                    if (x.UnlockRank > y.UnlockRank)
                    {
                        return 1;
                    }
                    return x.Id.CompareTo(y.Id);
                }
            }
        }
    }
}

