namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class DungeonResources : CsvResources<string, Dungeon>
    {
        public const string DEBUG_DUNGEON_ID = "Debug1";
        public const int DEFAULT_BOSS_POOL_WEIGHT = 0x22c;
        private Dictionary<DungeonThemeType, List<CharacterType>> m_dungeonThemeMinionCharacterTypes = new Dictionary<DungeonThemeType, List<CharacterType>>(new DungeonThemeTypeBoxAvoidanceComparer());
        private List<Dungeon> m_orderedRegularDungeons = new List<Dungeon>();

        public DungeonResources([Optional, DefaultParameterValue("Rules/Dungeon-Instances")] string sourcePath)
        {
            string[,] strArray = CsvUtils.Deserialize(ResourceUtil.LoadSafe<TextAsset>(sourcePath, false).text);
            for (int i = 0; i < strArray.GetLength(1); i++)
            {
                if (strArray[0, i] != null)
                {
                    int num4;
                    RowEntry entry = new RowEntry();
                    int num2 = 0;
                    entry.Id = strArray[num2++, i];
                    entry.Name = _.L(strArray[num2++, i], null, false);
                    entry.LevelRequirement = base.parseInt(strArray[num2++, i]);
                    entry.ExploreCost = base.parseInt(strArray[num2++, i]);
                    entry.BaseDifficulty = base.parseInt(strArray[num2++, i]);
                    entry.EnergyCost = base.parseInt(strArray[num2++, i]);
                    entry.FloorCount = ConfigDungeons.DEFAULT_DUNGEON_ROOM_COUNT;
                    num2++;
                    entry.PrimaryMinionType = base.parseEnumType<CharacterType>(strArray[num2++, i]);
                    entry.SecondaryMinionType = base.parseEnumType<CharacterType>(strArray[num2++, i]);
                    List<KeyValuePair<string, int>> list = base.parseStringIntList(strArray[num2++, i], 0x22c);
                    entry.BossPool = new Dictionary<string, int>();
                    for (int j = 0; j < list.Count; j++)
                    {
                        KeyValuePair<string, int> pair = list[j];
                        KeyValuePair<string, int> pair2 = list[j];
                        entry.BossPool.Add(pair.Key, pair2.Value);
                    }
                    entry.BossType = strArray[num2++, i];
                    entry.FtueBoss = ((list.Count <= 0) || !(entry.BossType != "Elite")) ? null : list[0].Key;
                    entry.Theme = base.parseEnumType<DungeonThemeType>(strArray[num2++, i]);
                    entry.Mood = strArray[num2++, i];
                    entry.MapStyle = strArray[num2++, i];
                    entry.LootPool = base.parseStringList(strArray[num2++, i]);
                    entry.LayoutPool = base.parseStringList(strArray[num2++, i]);
                    Dungeon dungeon = new Dungeon();
                    dungeon.Id = entry.Id;
                    dungeon.Name = entry.Name;
                    dungeon.LevelRequirement = entry.LevelRequirement;
                    dungeon.ExploreCost = entry.ExploreCost;
                    dungeon.EnergyCost = entry.EnergyCost;
                    dungeon.LootPool = entry.LootPool;
                    dungeon.BaseDifficulty = entry.BaseDifficulty;
                    dungeon.FloorCount = entry.FloorCount;
                    dungeon.LayoutPool = entry.LayoutPool;
                    dungeon.Theme = entry.Theme;
                    dungeon.Mood = GameLogic.Binder.DungeonMoodResources.getMood(entry.Mood);
                    dungeon.MapStyle = entry.MapStyle;
                    dungeon.PrimaryMinionType = entry.PrimaryMinionType;
                    dungeon.SecondaryMinionType = entry.SecondaryMinionType;
                    dungeon.FtueBoss = entry.FtueBoss;
                    dungeon.BossPool = entry.BossPool;
                    dungeon.EliteTag = entry.BossType == "Elite";
                    bool flag = int.TryParse(dungeon.Id, out num4);
                    if (!flag || (this.m_orderedRegularDungeons.Count < ConfigDungeons.MAX_DUNGEON_COUNT))
                    {
                        base.m_resources.Add(dungeon.Id, dungeon);
                    }
                    if (flag && (this.m_orderedRegularDungeons.Count < ConfigDungeons.MAX_DUNGEON_COUNT))
                    {
                        this.m_orderedRegularDungeons.Add(dungeon);
                    }
                    if (!this.m_dungeonThemeMinionCharacterTypes.ContainsKey(dungeon.Theme))
                    {
                        this.m_dungeonThemeMinionCharacterTypes.Add(dungeon.Theme, new List<CharacterType>());
                    }
                    List<CharacterType> list2 = this.m_dungeonThemeMinionCharacterTypes[dungeon.Theme];
                    if (!list2.Contains(entry.PrimaryMinionType))
                    {
                        list2.Add(entry.PrimaryMinionType);
                    }
                    if (!list2.Contains(entry.SecondaryMinionType))
                    {
                        list2.Add(entry.SecondaryMinionType);
                    }
                }
            }
            this.createDebugDungeon();
        }

        public void createDebugDungeon()
        {
            Dungeon dungeon2 = new Dungeon();
            dungeon2.Id = "Debug1";
            dungeon2.PrimaryMinionType = CharacterType.Shroom;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            dictionary.Add("Goblin002", 100);
            dungeon2.BossPool = dictionary;
            dungeon2.EliteTag = true;
            List<string> list = new List<string>();
            list.Add("Layout005_001");
            dungeon2.LayoutPool = list;
            dungeon2.Theme = DungeonThemeType.Theme002;
            dungeon2.Mood = GameLogic.Binder.DungeonMoodResources.getMood("MoodIceDay");
            Dungeon dungeon = dungeon2;
            base.m_resources.Add(dungeon.Id, dungeon);
        }

        public Dictionary<string, Dungeon> getDungeons()
        {
            return base.m_resources;
        }

        public List<CharacterType> getMinionCharacterTypes(DungeonThemeType dungeonThemeType)
        {
            return this.m_dungeonThemeMinionCharacterTypes[dungeonThemeType];
        }

        public class LocationBracket
        {
            public int EndDungeonId;
            public int StartDungeonId;
        }

        public class RowEntry
        {
            public int BaseDifficulty;
            public Dictionary<string, int> BossPool;
            public string BossType;
            public int EnergyCost;
            public int ExploreCost;
            public int FloorCount;
            public string FtueBoss;
            public string Id;
            public List<string> LayoutPool;
            public int LevelRequirement;
            public List<string> LootPool;
            public string MapStyle;
            public string Mood;
            public string Name;
            public CharacterType PrimaryMinionType;
            public CharacterType SecondaryMinionType;
            public DungeonThemeType Theme;
        }
    }
}

