namespace GameLogic
{
    using App;
    using System;

    public class PlayerAugmentation : IBuffIconProvider
    {
        public string Id;
        public GameLogic.PerkInstance PerkInstance;
        public double Price;

        public string getSpriteId()
        {
            return ConfigPerks.SHARED_DATA[this.PerkInstance.Type].Sprite.SpriteId;
        }
    }
}

