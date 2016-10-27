namespace PlayerView
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SpriteResources
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map6;
        [CompilerGenerated]
        private SpriteAtlasEntry <IconWatchVideo>k__BackingField;
        private Dictionary<string, bool> m_atlasLoadedState = new Dictionary<string, bool>();
        private Dictionary<string, SpriteAtlas> m_loadedAtlases = new Dictionary<string, SpriteAtlas>();
        private Dictionary<string, Dictionary<string, Sprite>> m_quickLookupSprites = new Dictionary<string, Dictionary<string, Sprite>>(3);

        public SpriteResources()
        {
            this.loadAtlas("Splash");
            this.loadAtlas("Menu");
            this.loadAtlas("DungeonHud");
            this.loadAtlas("Gameplay");
            this.IconWatchVideo = new SpriteAtlasEntry("Menu", "icon_video");
        }

        public bool atlasLoaded(string atlasId)
        {
            return this.m_loadedAtlases.ContainsKey(atlasId);
        }

        public Sprite getSprite(SpriteAtlasEntry atlasEntry)
        {
            if (atlasEntry == null)
            {
                return null;
            }
            return this.getSprite(atlasEntry.AtlasId, atlasEntry.SpriteId);
        }

        public Sprite getSprite(string atlasId, string spriteId)
        {
            if (string.IsNullOrEmpty(atlasId) || string.IsNullOrEmpty(spriteId))
            {
                return null;
            }
            if (!this.m_atlasLoadedState.ContainsKey(atlasId))
            {
                return null;
            }
            if (!this.m_atlasLoadedState[atlasId])
            {
                return null;
            }
            if (!this.m_quickLookupSprites[atlasId].ContainsKey(spriteId))
            {
                return null;
            }
            return this.m_quickLookupSprites[atlasId][spriteId];
        }

        public void loadAtlas(string atlasId)
        {
            if (!this.m_loadedAtlases.ContainsKey(atlasId))
            {
                SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Sprites/" + atlasId);
                this.m_loadedAtlases.Add(atlasId, atlas);
                if (this.m_atlasLoadedState.ContainsKey(atlasId))
                {
                    this.m_atlasLoadedState[atlasId] = true;
                }
                else
                {
                    this.m_atlasLoadedState.Add(atlasId, true);
                }
                int capacity = 1;
                string key = atlasId;
                if (key != null)
                {
                    int num3;
                    if (<>f__switch$map6 == null)
                    {
                        Dictionary<string, int> dictionary2 = new Dictionary<string, int>(2);
                        dictionary2.Add("Menu", 0);
                        dictionary2.Add("DungeonHud", 1);
                        <>f__switch$map6 = dictionary2;
                    }
                    if (<>f__switch$map6.TryGetValue(key, out num3))
                    {
                        if (num3 == 0)
                        {
                            capacity = 0x200;
                        }
                        else if (num3 == 1)
                        {
                            capacity = 0x80;
                        }
                    }
                }
                Dictionary<string, Sprite> dictionary = new Dictionary<string, Sprite>(capacity);
                for (int i = 0; i < atlas.Sprites.Count; i++)
                {
                    dictionary.Add(atlas.Sprites[i].name, atlas.Sprites[i]);
                }
                this.m_quickLookupSprites.Add(atlasId, dictionary);
            }
        }

        public void releaseAtlas(string atlasId)
        {
            if (this.m_loadedAtlases.ContainsKey(atlasId))
            {
                HashSet<Texture2D> set = new HashSet<Texture2D>();
                SpriteAtlas atlas = this.m_loadedAtlases[atlasId];
                for (int i = 0; i < atlas.Sprites.Count; i++)
                {
                    set.Add(atlas.Sprites[i].texture);
                }
                Resources.UnloadAsset(this.m_loadedAtlases[atlasId]);
                foreach (Texture2D textured in set)
                {
                    Resources.UnloadAsset(textured);
                }
                this.m_loadedAtlases.Remove(atlasId);
                this.m_atlasLoadedState[atlasId] = false;
                this.m_quickLookupSprites.Remove(atlasId);
            }
        }

        public SpriteAtlasEntry IconWatchVideo
        {
            [CompilerGenerated]
            get
            {
                return this.<IconWatchVideo>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<IconWatchVideo>k__BackingField = value;
            }
        }
    }
}

