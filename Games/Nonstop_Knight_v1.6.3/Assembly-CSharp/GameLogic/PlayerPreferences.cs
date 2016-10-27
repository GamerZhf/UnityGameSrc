namespace GameLogic
{
    using Pathfinding.Serialization.JsonFx;
    using System;

    public class PlayerPreferences
    {
        [JsonIgnore]
        public bool AutoSummonBosses;
        public bool CombatStatsEnabled;
        public string DevLanguageId;
        public string DevServerId;
        public int SlidingMenuSortingOrder;
    }
}

