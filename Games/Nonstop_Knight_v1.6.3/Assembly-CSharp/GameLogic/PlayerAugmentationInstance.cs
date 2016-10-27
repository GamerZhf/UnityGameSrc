namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PlayerAugmentationInstance : IJsonData
    {
        [CompilerGenerated]
        private GameLogic.PlayerAugmentation <PlayerAugmentation>k__BackingField;
        public string PlayerAugmentationId;

        public PlayerAugmentationInstance()
        {
        }

        public PlayerAugmentationInstance(string playerAugId)
        {
            this.PlayerAugmentationId = playerAugId;
            this.postDeserializeInitialization();
        }

        public void postDeserializeInitialization()
        {
            this.PlayerAugmentation = GameLogic.Binder.PlayerAugmentationResources.getResource(this.PlayerAugmentationId);
            if (this.PlayerAugmentation == null)
            {
                Debug.LogError("PlayerAugmentation not found: " + this.PlayerAugmentationId);
            }
        }

        [JsonIgnore]
        public GameLogic.PlayerAugmentation PlayerAugmentation
        {
            [CompilerGenerated]
            get
            {
                return this.<PlayerAugmentation>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PlayerAugmentation>k__BackingField = value;
            }
        }
    }
}

