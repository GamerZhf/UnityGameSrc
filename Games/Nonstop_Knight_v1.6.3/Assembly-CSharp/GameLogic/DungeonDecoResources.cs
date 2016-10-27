namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DungeonDecoResources
    {
        private static Dictionary<DungeonThemeType, Dictionary<DungeonDecoCategoryType, List<string>>> s_decosPerThemePerCategory = new Dictionary<DungeonThemeType, Dictionary<DungeonDecoCategoryType, List<string>>>(new DungeonThemeTypeBoxAvoidanceComparer());
        private static List<DungeonThemeType> s_loadedThemes = new List<DungeonThemeType>(ConfigDungeons.DUNGEON_THEME_TYPES.Count);

        public static string GetRandomDeco(DungeonThemeType theme, DungeonDecoCategoryType decoCategory)
        {
            return LangUtil.GetRandomValueFromList<string>(s_decosPerThemePerCategory[theme][decoCategory]);
        }

        public static void LoadTheme(DungeonThemeType theme)
        {
            if (!ThemeLoaded(theme))
            {
                Dictionary<DungeonDecoCategoryType, List<string>> dictionary = new Dictionary<DungeonDecoCategoryType, List<string>>(new DungeonDecoCategoryTypeBoxAvoidanceComparer());
                s_decosPerThemePerCategory.Add(theme, dictionary);
                for (int i = 0; i < ConfigDungeons.DUNGEON_DECO_CATEGORY_TYPES.Count; i++)
                {
                    DungeonDecoCategoryType key = ConfigDungeons.DUNGEON_DECO_CATEGORY_TYPES[i];
                    dictionary.Add(key, new List<string>());
                    UnityEngine.Object[] objArray = ResourceUtil.LoadResourcesAtPath(string.Concat(new object[] { ConfigDungeons.DUNGEON_DECO_RESOURCE_PATH, "/", theme, "/", key }));
                    for (int j = 0; j < objArray.Length; j++)
                    {
                        string name = objArray[j].name;
                        object[] objArray2 = new object[] { ConfigDungeons.DUNGEON_DECO_RESOURCE_PATH, "/", theme, "/", key, "/", name };
                        s_decosPerThemePerCategory[theme][key].Add(string.Concat(objArray2));
                    }
                }
                s_loadedThemes.Add(theme);
            }
        }

        public static void ReleaseTheme(DungeonThemeType theme)
        {
            if (ThemeLoaded(theme))
            {
                s_decosPerThemePerCategory.Remove(theme);
                s_loadedThemes.Remove(theme);
            }
        }

        public static bool ThemeLoaded(DungeonThemeType theme)
        {
            return s_loadedThemes.Contains(theme);
        }
    }
}

