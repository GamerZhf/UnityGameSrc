namespace PlayerView
{
    using App;
    using System;

    public class ChestBlueprint
    {
        public SpriteAtlasEntry BaseSprite;
        public float CeremonyScale = 1f;
        public SpriteAtlasEntry DropSprite;
        public SpriteAtlasEntry Icon;
        public SpriteAtlasEntry LockSprite;
        public string Name = string.Empty;
        public string ShortName = string.Empty;
        public SpriteAtlasEntry SoloSprite;
        public SpriteAtlasEntry TopSprite;

        public bool isMultiPart()
        {
            return (this.TopSprite != null);
        }
    }
}

