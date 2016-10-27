namespace GameLogic
{
    using App;
    using System;

    public class TournamentUpgrade : IBuffIconProvider
    {
        public float MilestoneMultiplier = 1f;
        public float Modifier;
        public float ModifierEpic;
        public GameLogic.PerkType PerkType;
        public int Weight;

        public SpriteAtlasEntry getSprite()
        {
            return ConfigPerks.SHARED_DATA[this.PerkType].Sprite;
        }

        public string getSpriteId()
        {
            return ConfigPerks.SHARED_DATA[this.PerkType].Sprite.SpriteId;
        }
    }
}

