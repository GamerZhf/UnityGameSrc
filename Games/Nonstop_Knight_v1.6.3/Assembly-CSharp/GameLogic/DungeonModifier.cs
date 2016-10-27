namespace GameLogic
{
    using App;
    using System;

    public class DungeonModifier : IBuffIconProvider
    {
        public string Description;
        public string Name;
        public int Parameter_Int;
        public ItemType Parameter_ItemType;
        public string Parameter_String;
        public SpriteAtlasEntry Sprite;

        public string getSpriteId()
        {
            return this.Sprite.SpriteId;
        }
    }
}

