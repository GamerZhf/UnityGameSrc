namespace App
{
    using System;

    public class SpriteAtlasEntry
    {
        public string AtlasId;
        public string SpriteId;

        public SpriteAtlasEntry()
        {
        }

        public SpriteAtlasEntry(string atlasId, string spriteId)
        {
            this.AtlasId = atlasId;
            this.SpriteId = spriteId;
        }
    }
}

