namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;

    public class Episode : IJsonData
    {
        public List<string> DungeonIds = new List<string>();
        public string Id = string.Empty;
        public string Name = string.Empty;

        public void postDeserializeInitialization()
        {
        }
    }
}

