namespace GameLogic
{
    using Pathfinding.Serialization.JsonFx;
    using System;
    using UnityEngine;

    public class LeaderboardEntry
    {
        public string AvatarSpriteId;
        public bool Dummy;
        public int HighestFloor = 1;
        [JsonIgnore]
        public Texture2D ImageTexture;
        [JsonIgnore]
        public bool IsSelf;
        public string Name;
        [JsonIgnore]
        public int Rank = -1;
        public string UserId;

        public string getPrettyName()
        {
            if (this.Dummy)
            {
                return _.L(this.Name, null, false);
            }
            return this.Name.Substring(0, Mathf.Min(this.Name.Length, 0x20));
        }

        public bool isDummy()
        {
            return this.Dummy;
        }

        public void setDefaultPlayerHeroAvatarSprite()
        {
            this.AvatarSpriteId = "sprite_knight_player_256";
        }
    }
}

