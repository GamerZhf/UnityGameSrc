namespace GameLogic
{
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;

    public class GameState
    {
        public GameLogic.ActiveDungeon ActiveDungeon;
        [JsonIgnore]
        public List<CharacterInstance> PersistentCharacters = new List<CharacterInstance>();
        public GameLogic.Player Player;
    }
}

