namespace App
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ConfigDungeons
    {
        public static float BOSS_TARGET_LIGHTING_INTENSITY = 0.6f;
        public static List<string> DEFAULT_DUNGEON_LAYOUT_POOL;
        public static int DEFAULT_DUNGEON_ROOM_COUNT = 1;
        public static Dictionary<DungeonThemeType, Dictionary<DungeonBlockType, Dictionary<string, int>>> DUNGEON_BLOCK_POOLS;
        public static List<float> DUNGEON_BLOCK_RANDOMIZED_STARTING_ROTATION;
        public static string DUNGEON_BLOCKS_RESOURCE_PATH = "Prefabs/DungeonBlocks";
        public static List<DungeonDecoCategoryType> DUNGEON_DECO_CATEGORIES_AS_DUNGEON_BOOST_SPAWNPOINTS;
        public static Dictionary<DungeonDecoCategoryType, float> DUNGEON_DECO_CATEGORY_SPAWN_PROBABILITY;
        public static List<DungeonDecoCategoryType> DUNGEON_DECO_CATEGORY_TYPES;
        public static string DUNGEON_DECO_RESOURCE_PATH = "Prefabs/DungeonDecos";
        public static string DUNGEON_ISLAND_RESOURCE_PATH = "DungeonIslands";
        public static List<DungeonThemeType> DUNGEON_THEME_TYPES;
        public static int MAX_DUNGEON_COUNT = 90;

        static ConfigDungeons()
        {
            List<string> list = new List<string>();
            list.Add("Layout005_001");
            DEFAULT_DUNGEON_LAYOUT_POOL = list;
            Dictionary<DungeonThemeType, Dictionary<DungeonBlockType, Dictionary<string, int>>> dictionary = new Dictionary<DungeonThemeType, Dictionary<DungeonBlockType, Dictionary<string, int>>>(new DungeonThemeTypeBoxAvoidanceComparer());
            Dictionary<DungeonBlockType, Dictionary<string, int>> dictionary2 = new Dictionary<DungeonBlockType, Dictionary<string, int>>(new DungeonBlockTypeBoxAvoidanceComparer());
            Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptSmall0", 100);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptSmall1", 50);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptSmall2", 50);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptSmall3", 20);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptSmall4", 20);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptSmall5", 20);
            dictionary2.Add(DungeonBlockType.FloorSmall, dictionary3);
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptBig0", 100);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptBig1", 50);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptBig2", 50);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptBig3", 20);
            dictionary2.Add(DungeonBlockType.FloorBig, dictionary3);
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptSmall0", 100);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptBridge1", 50);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptBridge2", 50);
            dictionary2.Add(DungeonBlockType.FloorBridge, dictionary3);
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptSmall0", 100);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme001/FloortileCryptSpecial1", 100);
            dictionary2.Add(DungeonBlockType.FloorSpecial, dictionary3);
            dictionary.Add(DungeonThemeType.Theme001, dictionary2);
            dictionary2 = new Dictionary<DungeonBlockType, Dictionary<string, int>>(new DungeonBlockTypeBoxAvoidanceComparer());
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileSmall0", 100);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileSmall1", 50);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileSmall2", 10);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileSmall3", 20);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileSmall4", 20);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileSmall5", 20);
            dictionary2.Add(DungeonBlockType.FloorSmall, dictionary3);
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileBig0", 100);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileBig1", 0x19);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileBig2", 0x19);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileBig3", 0x19);
            dictionary2.Add(DungeonBlockType.FloorBig, dictionary3);
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileSmall0", 0x19);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileBridge1", 50);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileBridge2", 50);
            dictionary2.Add(DungeonBlockType.FloorBridge, dictionary3);
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme002/FloortileSpecial1", 100);
            dictionary2.Add(DungeonBlockType.FloorSpecial, dictionary3);
            dictionary.Add(DungeonThemeType.Theme002, dictionary2);
            dictionary2 = new Dictionary<DungeonBlockType, Dictionary<string, int>>(new DungeonBlockTypeBoxAvoidanceComparer());
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterSmall0", 100);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterSmall1", 100);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterSmall2", 50);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterSmall3", 50);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterSmall4", 50);
            dictionary2.Add(DungeonBlockType.FloorSmall, dictionary3);
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterBig0", 100);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterBig1", 20);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterBig2", 50);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterBig3", 100);
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterBig4", 50);
            dictionary2.Add(DungeonBlockType.FloorBig, dictionary3);
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterBridge0", 100);
            dictionary2.Add(DungeonBlockType.FloorBridge, dictionary3);
            dictionary3 = new Dictionary<string, int>();
            dictionary3.Add("Prefabs/DungeonBlocks/Theme003/FloortileWinterSpecial0", 100);
            dictionary2.Add(DungeonBlockType.FloorSpecial, dictionary3);
            dictionary.Add(DungeonThemeType.Theme003, dictionary2);
            DUNGEON_BLOCK_POOLS = dictionary;
            DUNGEON_THEME_TYPES = LangUtil.GetEnumValues<DungeonThemeType>();
            DUNGEON_DECO_CATEGORY_TYPES = LangUtil.GetEnumValues<DungeonDecoCategoryType>();
            Dictionary<DungeonDecoCategoryType, float> dictionary4 = new Dictionary<DungeonDecoCategoryType, float>(new DungeonDecoCategoryTypeBoxAvoidanceComparer());
            dictionary4.Add(DungeonDecoCategoryType.FloorDeco1B, 1f);
            dictionary4.Add(DungeonDecoCategoryType.FloorDeco2B, 0.4f);
            dictionary4.Add(DungeonDecoCategoryType.Pillar1B, 0.5f);
            dictionary4.Add(DungeonDecoCategoryType.Obstacle1B, 0.5f);
            dictionary4.Add(DungeonDecoCategoryType.WallCorner1B, 1f);
            dictionary4.Add(DungeonDecoCategoryType.WallEntrance2B, 0.75f);
            dictionary4.Add(DungeonDecoCategoryType.Wall1B, 1f);
            DUNGEON_DECO_CATEGORY_SPAWN_PROBABILITY = dictionary4;
            List<DungeonDecoCategoryType> list2 = new List<DungeonDecoCategoryType>();
            list2.Add(DungeonDecoCategoryType.FloorDeco1B);
            list2.Add(DungeonDecoCategoryType.FloorDeco2B);
            list2.Add(DungeonDecoCategoryType.Obstacle1B);
            list2.Add(DungeonDecoCategoryType.Pillar1B);
            DUNGEON_DECO_CATEGORIES_AS_DUNGEON_BOOST_SPAWNPOINTS = list2;
            List<float> list3 = new List<float>();
            list3.Add(0f);
            list3.Add(90f);
            list3.Add(180f);
            list3.Add(270f);
            DUNGEON_BLOCK_RANDOMIZED_STARTING_ROTATION = list3;
        }

        public static bool FloorHasBoss(int floor)
        {
            return ((floor == 0) || GameLogic.Binder.DungeonResources.getResource(GetDungeonIdForFloor(floor)).hasBoss());
        }

        public static bool FloorHasEliteTag(int floor)
        {
            return GameLogic.Binder.DungeonResources.getResource(GetDungeonIdForFloor(floor)).hasEliteTag();
        }

        public static string GetDungeonIdForFloor(int floor)
        {
            int num = floor % MAX_DUNGEON_COUNT;
            if (num == 0)
            {
                return MAX_DUNGEON_COUNT.ToString();
            }
            return Mathf.Clamp(num, 1, 0x7fffffff).ToString();
        }

        public static string GetFogIdForTheme(DungeonThemeType theme)
        {
            switch (theme)
            {
                case DungeonThemeType.Theme001:
                    return "Prefabs/Dungeon/CryptFog";

                case DungeonThemeType.Theme003:
                    return "Prefabs/Dungeon/WinterSea";
            }
            return null;
        }

        public static int GetNextFloorWithBoss(int fromFloor)
        {
            int floor = fromFloor;
            while (!GameLogic.Binder.DungeonResources.getResource(GetDungeonIdForFloor(floor)).hasBoss())
            {
                floor++;
            }
            return floor;
        }

        public static int GetNextFloorWithEliteTag(int fromFloor)
        {
            int floor = fromFloor;
            while (!GameLogic.Binder.DungeonResources.getResource(GetDungeonIdForFloor(floor)).hasEliteTag())
            {
                floor++;
            }
            return floor;
        }

        public static bool ParallaxCloudsEnabledForTheme(DungeonThemeType theme)
        {
            return string.IsNullOrEmpty(GetFogIdForTheme(theme));
        }
    }
}

