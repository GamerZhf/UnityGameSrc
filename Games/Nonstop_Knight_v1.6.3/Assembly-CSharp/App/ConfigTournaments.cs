namespace App
{
    using GameLogic;
    using PlayerView;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ConfigTournaments
    {
        public static Dictionary<DungeonThemeType, DungeonMood> DUNGEON_MOOD_OVERRIDES;
        public static long TOURNAMENT_CARD_PACK_AD_VIEW_COOLDOWN_MINUTES = 120L;
        public static bool TOURNAMENT_CARD_SNEAK_PEAK_ENABLED = true;
        public static bool TOURNAMENT_GET_ALL_AD_VIEWING_ENABLED = false;
        public const string TOURNAMENT_MAIN_REWARD_DEFAULT_SHOP_ENTRY = "PetBoxSmall";
        public const int TOURNAMENT_NUM_CARDS_IN_PACK = 3;
        public static int TOURNAMENT_WILD_BOSS_CAP_BASE = 2;
        public static int TOURNAMENT_WILD_BOSS_CAP_INCREASE_PER_COMPLETED_MILESTONE = 2;
        public static float TOURNAMENT_WILD_BOSS_ESCAPE_HP_BOSS_THRESHOLD;
        public static float TOURNAMENT_WILD_BOSS_ESCAPE_HP_HERO_THRESHOLD;
        public static int TOURNAMENT_WILD_BOSS_MAX_KILLS_PER_FLOOR = 1;
        public static float TOURNAMENT_WILD_BOSS_SPAWN_CHANCE_BASE = 0f;
        public static Dictionary<string, float> TOURNAMENT_WILD_BOSS_SPAWN_CHANCE_INCREASE_PER_MOB;

        static ConfigTournaments()
        {
            Dictionary<string, float> dictionary = new Dictionary<string, float>();
            dictionary.Add("impossible", 0.005f);
            dictionary.Add("very_hard", 0.01f);
            dictionary.Add("hard", 0.015f);
            dictionary.Add("ready", 0.025f);
            TOURNAMENT_WILD_BOSS_SPAWN_CHANCE_INCREASE_PER_MOB = dictionary;
            TOURNAMENT_WILD_BOSS_ESCAPE_HP_BOSS_THRESHOLD = 0.66f;
            TOURNAMENT_WILD_BOSS_ESCAPE_HP_HERO_THRESHOLD = 0.33f;
            Dictionary<DungeonThemeType, DungeonMood> dictionary2 = new Dictionary<DungeonThemeType, DungeonMood>(new DungeonThemeTypeBoxAvoidanceComparer());
            DungeonMood mood = new DungeonMood();
            mood.HeroLightColor = new Color(1f, 0.3921569f, 0f, 0f);
            mood.HeroLightIntensity = 5f;
            mood.HeroLightRange = 9f;
            mood.FogColor = new Color(0.2941177f, 0.4509804f, 1f, 1f);
            mood.HorizontalFogStartTerm = 8f;
            mood.HorizontalFogEndTerm = 42f;
            mood.PropColor = new Color(1f, 0.3921569f, 0f, 0f);
            mood.Weather = EffectType.Weather_Confetti;
            dictionary2.Add(DungeonThemeType.Theme001, mood);
            mood = new DungeonMood();
            mood.HeroLightColor = new Color(1f, 0.3921569f, 0f, 0f);
            mood.HeroLightIntensity = 5f;
            mood.HeroLightRange = 10f;
            mood.FogColor = new Color(0.254902f, 0.4117647f, 1f, 1f);
            mood.HorizontalFogStartTerm = 10f;
            mood.HorizontalFogEndTerm = 30f;
            mood.PropColor = new Color(0.3333333f, 0.5686275f, 0.8627451f, 0f);
            mood.Weather = EffectType.Weather_Confetti;
            dictionary2.Add(DungeonThemeType.Theme002, mood);
            mood = new DungeonMood();
            mood.HeroLightColor = new Color(0.09803922f, 0.4509804f, 1f, 0f);
            mood.HeroLightIntensity = 3f;
            mood.HeroLightRange = 10f;
            mood.FogColor = new Color(1f, 0.3137255f, 0.1764706f, 1f);
            mood.HorizontalFogStartTerm = 10f;
            mood.HorizontalFogEndTerm = 40f;
            mood.PropColor = new Color(1f, 0.3921569f, 0f, 0f);
            mood.Weather = EffectType.Weather_Confetti;
            dictionary2.Add(DungeonThemeType.Theme003, mood);
            DUNGEON_MOOD_OVERRIDES = dictionary2;
        }
    }
}

