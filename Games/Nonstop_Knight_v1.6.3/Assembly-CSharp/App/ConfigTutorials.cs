namespace App
{
    using GameLogic;
    using System;
    using System.Collections.Generic;

    public static class ConfigTutorials
    {
        public static List<string> AFK_TUTORIALS;
        public static List<string> ASCEND_AND_TOKENS_TUTORIALS;
        public const string CONTEXT_TUTORIAL_BOSS_HUNT_FIRST_TIME = "CTUT006";
        public const string CONTEXT_TUTORIAL_BOSS_HUNT_VETERAN = "CTUT007";
        public const string CONTEXT_TUTORIAL_BOSS_SUMMONING = "CTUT004";
        public const string CONTEXT_TUTORIAL_CLONE_UNLOCKING = "CTUT002";
        public const string CONTEXT_TUTORIAL_FRENZY_POTION = "CTUT005";
        public const string CONTEXT_TUTORIAL_HERO_NAMING = "CTUT001";
        public const string CONTEXT_TUTORIAL_WEAPON_UPGRADING = "CTUT003";
        public static List<string> CORE_GAMEPLAY_TUTORIALS;
        public static List<string> CORE_LOOP_TUTORIALS;
        public static float CUTSCENE_BORDER_CLOSE_DURATION = 0.4f;
        public static Easing.Function CUTSCENE_BORDER_EASING_CLOSE = Easing.Function.SMOOTHSTEP;
        public static Easing.Function CUTSCENE_BORDER_EASING_OPEN = Easing.Function.SMOOTHSTEP;
        public static float CUTSCENE_BORDER_OPEN_DURATION = 0.4f;
        public static string FIXED_TUTORIAL_CLOAK_ITEM_ID = "Cloak003";
        public static List<string> MISSION_AND_PET_TUTORIALS;
        public static List<string> SKILLS_TUTORIALS;
        public static float TUTORIAL_BOSS_DPH_MULTIPLIER = 0.64f;
        public static float TUTORIAL_BOSS_LIFE_MULTIPLIER = 0.5f;
        public static string TUTORIAL_DUNGEON_ID = "T1";
        public static float TUTORIAL_MINION_DPH_MULTIPLIER = 0.15f;
        public static float TUTORIAL_MINION_LIFE_MULTIPLIER = 0.15f;
        public static float TUTORIAL_PREPARATION_TO_ACTION_DELAY = 1f;
        public static int TUTORIAL_REQUIRED_MINION_KILLS_UNTIL_BOSS_SUMMON = 10;
        public static int TUTORIAL_TIME_SLOWDOWN_FRAME_COUNT = 12;

        static ConfigTutorials()
        {
            List<string> list = new List<string>();
            list.Add("TUT000A");
            list.Add("TUT001A");
            list.Add("TUT001B");
            list.Add("TUT001C");
            list.Add("TUT001D");
            list.Add("TUT001E");
            list.Add("TUT001F");
            list.Add("TUT002A");
            list.Add("TUT002B");
            list.Add("TUT003A");
            list.Add("TUT003B");
            list.Add("TUT003C");
            CORE_GAMEPLAY_TUTORIALS = list;
            list = new List<string>();
            list.Add("TUT004A");
            list.Add("TUT004B");
            list.Add("TUT050A");
            list.Add("TUT050B");
            list.Add("TUT050C");
            list.Add("TUT050D");
            list.Add("TUT051A");
            list.Add("TUT051B");
            list.Add("TUT051C");
            list.Add("TUT052A");
            list.Add("TUT052B");
            list.Add("TUT052C");
            list.Add("TUT052D");
            CORE_LOOP_TUTORIALS = list;
            list = new List<string>();
            list.Add("TUT149A");
            list.Add("TUT150D");
            SKILLS_TUTORIALS = list;
            list = new List<string>();
            list.Add("TUT200A");
            list.Add("TUT200B");
            AFK_TUTORIALS = list;
            list = new List<string>();
            list.Add("TUT351A");
            list.Add("TUT351B");
            list.Add("TUT351C");
            list.Add("TUT360A");
            list.Add("TUT360B");
            list.Add("TUT360C");
            ASCEND_AND_TOKENS_TUTORIALS = list;
            list = new List<string>();
            list.Add("TUT451A");
            list.Add("TUT451B");
            MISSION_AND_PET_TUTORIALS = list;
        }

        public static void CheatCompleteAllContextTutorials(Player player)
        {
            if (!player.CompletedTutorials.Contains("CTUT001"))
            {
                player.CompletedTutorials.Add("CTUT001");
                if (!player.SocialData.HasGivenCustomName)
                {
                    CmdRenamePlayer.ExecuteStatic(player, "Knight");
                }
            }
            if (!player.CompletedTutorials.Contains("CTUT002"))
            {
                player.CompletedTutorials.Add("CTUT002");
            }
            if (!player.UnlockedSkills.Contains(SkillType.Clone))
            {
                CmdUnlockSkill.ExecuteStatic(player, SkillType.Clone, true);
            }
            if (!player.CompletedTutorials.Contains("CTUT003"))
            {
                player.CompletedTutorials.Add("CTUT003");
            }
            if (!player.CompletedTutorials.Contains("CTUT004"))
            {
                player.CompletedTutorials.Add("CTUT004");
            }
            if (!player.CompletedTutorials.Contains("CTUT005"))
            {
                player.CompletedTutorials.Add("CTUT005");
            }
            if (!player.CompletedTutorials.Contains("CTUT006"))
            {
                player.CompletedTutorials.Add("CTUT006");
            }
            if (!player.CompletedTutorials.Contains("CTUT007"))
            {
                player.CompletedTutorials.Add("CTUT007");
            }
        }

        public static void CheatCompleteAllFtueTutorials(Player player)
        {
            for (int i = 0; i < CORE_GAMEPLAY_TUTORIALS.Count; i++)
            {
                string item = CORE_GAMEPLAY_TUTORIALS[i];
                if (!player.CompletedTutorials.Contains(item))
                {
                    player.CompletedTutorials.Add(item);
                }
            }
            for (int j = 0; j < CORE_LOOP_TUTORIALS.Count; j++)
            {
                string str2 = CORE_LOOP_TUTORIALS[j];
                if (!player.CompletedTutorials.Contains(str2))
                {
                    player.CompletedTutorials.Add(str2);
                }
            }
            for (int k = 0; k < SKILLS_TUTORIALS.Count; k++)
            {
                string str3 = SKILLS_TUTORIALS[k];
                if (!player.CompletedTutorials.Contains(str3))
                {
                    player.CompletedTutorials.Add(str3);
                }
            }
            for (int m = 0; m < AFK_TUTORIALS.Count; m++)
            {
                string str4 = AFK_TUTORIALS[m];
                if (!player.CompletedTutorials.Contains(str4))
                {
                    player.CompletedTutorials.Add(str4);
                }
            }
            for (int n = 0; n < ASCEND_AND_TOKENS_TUTORIALS.Count; n++)
            {
                string str5 = ASCEND_AND_TOKENS_TUTORIALS[n];
                if (!player.CompletedTutorials.Contains(str5))
                {
                    player.CompletedTutorials.Add(str5);
                }
            }
            for (int num6 = 0; num6 < MISSION_AND_PET_TUTORIALS.Count; num6++)
            {
                string str6 = MISSION_AND_PET_TUTORIALS[num6];
                if (!player.CompletedTutorials.Contains(str6))
                {
                    player.CompletedTutorials.Add(str6);
                }
            }
        }
    }
}

