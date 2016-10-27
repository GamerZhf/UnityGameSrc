namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class EpisodeResources
    {
        private Dictionary<string, Episode> m_episodes = new Dictionary<string, Episode>();

        public EpisodeResources()
        {
            foreach (UnityEngine.Object obj2 in ResourceUtil.LoadResourcesAtPath("Episodes"))
            {
                if (obj2 is TextAsset)
                {
                    Episode episode = JsonUtils.Deserialize<Episode>(((TextAsset) obj2).text, true);
                    episode.Id = obj2.name;
                    episode.postDeserializeInitialization();
                    this.m_episodes.Add(episode.Id, episode);
                }
            }
        }

        public Episode getEpisode(string id)
        {
            if (this.m_episodes.ContainsKey(id))
            {
                return this.m_episodes[id];
            }
            return null;
        }
    }
}

