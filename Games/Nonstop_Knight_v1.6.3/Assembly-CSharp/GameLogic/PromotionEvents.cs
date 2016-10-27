namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PromotionEvents : IJsonData
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        public Dictionary<string, PromotionEventInstance> Instances = new Dictionary<string, PromotionEventInstance>();

        public PromotionEventInstance getEventInstance(string promotionId)
        {
            if (string.IsNullOrEmpty(promotionId))
            {
                return null;
            }
            return (!this.Instances.ContainsKey(promotionId) ? null : this.Instances[promotionId]);
        }

        public PromotionEventInstance getNewestEventInstance()
        {
            PromotionEventInstance instance = null;
            long startTimestamp = -9223372036854775808L;
            foreach (KeyValuePair<string, PromotionEventInstance> pair in this.Instances)
            {
                if (pair.Value.StartTimestamp > startTimestamp)
                {
                    instance = pair.Value;
                    startTimestamp = pair.Value.StartTimestamp;
                }
            }
            return instance;
        }

        public bool hasEvents()
        {
            return (this.Instances.Count > 0);
        }

        public bool hasUninspectedEvents()
        {
            foreach (KeyValuePair<string, PromotionEventInstance> pair in this.Instances)
            {
                if (!pair.Value.Inspected)
                {
                    return true;
                }
            }
            return false;
        }

        public void postDeserializeInitialization()
        {
            foreach (KeyValuePair<string, PromotionEventInstance> pair in this.Instances)
            {
                pair.Value.PromotionEvents = this;
                pair.Value.postDeserializeInitialization();
            }
        }

        [JsonIgnore]
        public GameLogic.Player Player
        {
            [CompilerGenerated]
            get
            {
                return this.<Player>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Player>k__BackingField = value;
            }
        }
    }
}

