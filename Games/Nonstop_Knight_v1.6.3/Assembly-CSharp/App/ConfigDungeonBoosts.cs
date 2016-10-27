namespace App
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ConfigDungeonBoosts
    {
        public static float BOX_RADIUS = 0.5f;
        public static List<DungeonBoostType> BOX_TYPES;
        public static List<DungeonBoostType> EMPTY_BOX_VIEW_POOL;

        static ConfigDungeonBoosts()
        {
            List<DungeonBoostType> list = new List<DungeonBoostType>();
            list.Add(DungeonBoostType.EmptyBox);
            list.Add(DungeonBoostType.BuffBox);
            list.Add(DungeonBoostType.ExplosiveBox);
            list.Add(DungeonBoostType.ResourceBox);
            BOX_TYPES = list;
            list = new List<DungeonBoostType>();
            list.Add(DungeonBoostType.BuffBox);
            list.Add(DungeonBoostType.ExplosiveBox);
            list.Add(DungeonBoostType.ResourceBox);
            EMPTY_BOX_VIEW_POOL = list;
        }

        public static int GetBoxSpawnCountForFloor(Player player, int floor, int numValidSpawnpoints)
        {
            if (player.hasRetired())
            {
                return numValidSpawnpoints;
            }
            if (floor < App.Binder.ConfigMeta.DUNGEON_BOOST_BOX_FIRST_RUN_MIN_SPAWN_COUNT_FLOOR)
            {
                return 0;
            }
            float num = Mathf.InverseLerp((float) App.Binder.ConfigMeta.DUNGEON_BOOST_BOX_FIRST_RUN_MIN_SPAWN_COUNT_FLOOR, (float) App.Binder.ConfigMeta.DUNGEON_BOOST_BOX_FIRST_RUN_MAX_SPAWN_COUNT_FLOOR, (float) floor);
            return Mathf.Clamp(Mathf.FloorToInt(numValidSpawnpoints * num), 1, numValidSpawnpoints);
        }

        public static bool IsBox(DungeonBoostType dungeonBoostType)
        {
            return BOX_TYPES.Contains(dungeonBoostType);
        }
    }
}

