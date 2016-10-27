namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DungeonBlockResources
    {
        private Dictionary<string, DungeonBlock> m_dungeonBlocksObjs = new Dictionary<string, DungeonBlock>();
        private List<DungeonThemeType> m_loadedThemes = new List<DungeonThemeType>(ConfigDungeons.DUNGEON_THEME_TYPES.Count);

        public DungeonBlock getDungeonBlockPrototype(string prefabId)
        {
            return this.m_dungeonBlocksObjs[prefabId];
        }

        public GameObject instantiateDungeonBlock(string prefabId)
        {
            return UnityEngine.Object.Instantiate<GameObject>(this.m_dungeonBlocksObjs[prefabId].gameObject);
        }

        public void loadTheme(DungeonThemeType theme)
        {
            if (!this.themeLoaded(theme))
            {
                UnityEngine.Object[] objArray = ResourceUtil.LoadResourcesAtPath(ConfigDungeons.DUNGEON_BLOCKS_RESOURCE_PATH + "/" + theme);
                for (int i = 0; i < objArray.Length; i++)
                {
                    object[] objArray1 = new object[] { ConfigDungeons.DUNGEON_BLOCKS_RESOURCE_PATH, "/", theme, "/", objArray[i].name };
                    string key = string.Concat(objArray1);
                    if (!this.m_dungeonBlocksObjs.ContainsKey(key))
                    {
                        this.m_dungeonBlocksObjs.Add(key, ((GameObject) ResourceUtil.LoadSafe(key, false)).GetComponent<DungeonBlock>());
                    }
                }
                this.m_loadedThemes.Add(theme);
            }
        }

        public void releaseTheme(DungeonThemeType theme)
        {
            if (this.themeLoaded(theme))
            {
                List<string> list = new List<string>();
                foreach (KeyValuePair<string, DungeonBlock> pair in this.m_dungeonBlocksObjs)
                {
                    string key = pair.Key;
                    object[] objArray1 = new object[] { ConfigDungeons.DUNGEON_BLOCKS_RESOURCE_PATH, "/", theme, "/" };
                    if (key.StartsWith(string.Concat(objArray1)))
                    {
                        list.Add(key);
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    this.m_dungeonBlocksObjs.Remove(list[i]);
                }
                this.m_loadedThemes.Remove(theme);
            }
        }

        public bool themeLoaded(DungeonThemeType theme)
        {
            return this.m_loadedThemes.Contains(theme);
        }
    }
}

