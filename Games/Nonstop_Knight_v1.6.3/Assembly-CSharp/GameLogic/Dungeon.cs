namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Dungeon
    {
        public int BaseDifficulty;
        public Dictionary<string, int> BossPool = new Dictionary<string, int>();
        public bool EliteTag;
        public int EnergyCost;
        public int ExploreCost;
        public int FloorCount;
        public string FtueBoss;
        public string Id = string.Empty;
        public List<string> LayoutPool = new List<string>();
        public int LevelRequirement;
        public List<string> LootPool = new List<string>();
        public string MapStyle = string.Empty;
        public DungeonMood Mood = new DungeonMood();
        public string Name = string.Empty;
        public GameLogic.CharacterType PrimaryMinionType;
        public GameLogic.CharacterType SecondaryMinionType;
        public DungeonThemeType Theme;

        public string getBossId(Player player, int floor)
        {
            return ((player.hasRetired() || (this.FtueBoss == null)) ? LangUtil.GetKeyFromDictionaryWithWeights<string>(this.BossPool, new uint?(player.RetirementHash.GetHash(floor))) : this.FtueBoss);
        }

        public string getRandomMinionId()
        {
            if ((this.SecondaryMinionType != GameLogic.CharacterType.UNSPECIFIED) && (UnityEngine.Random.Range((float) 0f, (float) 1f) > ConfigGameplay.SPAWN_CHANCE_FROM_PRIMARY_MINION_POOL))
            {
                return LangUtil.GetKeyFromDictionaryWithWeights<string>(ConfigGameplay.CHARACTER_SPAWN_WEIGHTS[this.SecondaryMinionType], null);
            }
            return LangUtil.GetKeyFromDictionaryWithWeights<string>(ConfigGameplay.CHARACTER_SPAWN_WEIGHTS[this.PrimaryMinionType], null);
        }

        public int getTotalRoomCount()
        {
            return this.FloorCount;
        }

        public bool hasBoss()
        {
            return ((this.BossPool != null) && (this.BossPool.Count > 0));
        }

        public bool hasEliteTag()
        {
            if (!App.Binder.ConfigMeta.ELITE_BOSSES_ENABLED)
            {
                return false;
            }
            return this.EliteTag;
        }
    }
}

