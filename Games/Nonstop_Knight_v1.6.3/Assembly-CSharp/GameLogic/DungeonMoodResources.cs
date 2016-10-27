namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DungeonMoodResources
    {
        private Dictionary<string, DungeonMood> m_dungeonMoods = new Dictionary<string, DungeonMood>();

        public DungeonMoodResources()
        {
            foreach (UnityEngine.Object obj2 in ResourceUtil.LoadResourcesAtPath("DungeonMoods"))
            {
                if (obj2 is TextAsset)
                {
                    DungeonMood mood = JsonUtils.Deserialize<DungeonMood>(((TextAsset) obj2).text, true);
                    mood.Id = obj2.name;
                    mood.postDeserializeInitialization();
                    this.m_dungeonMoods.Add(mood.Id, mood);
                }
            }
        }

        public DungeonMood getMood(string id)
        {
            if (this.m_dungeonMoods.ContainsKey(id))
            {
                return this.m_dungeonMoods[id];
            }
            return null;
        }
    }
}

