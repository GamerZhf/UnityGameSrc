namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class PlayerAugmentations : QuickLookableCharacterStatModifier, IJsonData
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        public List<PlayerAugmentationInstance> Instances = new List<PlayerAugmentationInstance>();
        private Dictionary<string, bool> m_augs = new Dictionary<string, bool>();
        private Dictionary<PerkInstance, PlayerAugmentation> m_perkInstanceToAugMap = new Dictionary<PerkInstance, PlayerAugmentation>();

        public void addAugmentation(PlayerAugmentationInstance ai)
        {
            this.Instances.Add(ai);
            this.reconstruct();
        }

        public bool canBuy(string id)
        {
            if (this.hasAugmentation(id))
            {
                return false;
            }
            double augmentationPrice = App.Binder.ConfigMeta.GetAugmentationPrice(id);
            double num2 = this.Player.getResourceAmount(ResourceType.Token);
            if (augmentationPrice > num2)
            {
                return false;
            }
            return true;
        }

        public void cheatAddAugmentations([Optional, DefaultParameterValue(true)] bool doClear, [Optional, DefaultParameterValue(0x7fffffff)] int maxCount)
        {
            if (doClear)
            {
                this.Instances.Clear();
            }
            List<PlayerAugmentation> list = GameLogic.Binder.PlayerAugmentationResources.getOrderedList();
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (!this.hasAugmentation(list[i].Id))
                {
                    num = i;
                    break;
                }
            }
            for (int j = num; (j < list.Count) && (j < (num + maxCount)); j++)
            {
                this.Instances.Add(new PlayerAugmentationInstance(list[j].Id));
            }
            this.reconstruct();
        }

        protected override IBuffIconProvider getBuffIconProvideForPerkInstance(PerkInstance perkInstance)
        {
            return this.m_perkInstanceToAugMap[perkInstance];
        }

        public int getTotalNumberOwned()
        {
            return this.Instances.Count;
        }

        public bool hasAugmentation(string id)
        {
            return this.m_augs[id];
        }

        public void postDeserializeInitialization()
        {
            for (int i = this.Instances.Count - 1; i >= 0; i--)
            {
                string playerAugmentationId = this.Instances[i].PlayerAugmentationId;
                if (string.IsNullOrEmpty(playerAugmentationId) || !GameLogic.Binder.PlayerAugmentationResources.containsResource(playerAugmentationId))
                {
                    this.Instances.RemoveAt(i);
                }
            }
            for (int j = 0; j < this.Instances.Count; j++)
            {
                this.Instances[j].postDeserializeInitialization();
            }
            this.reconstruct();
        }

        private void reconstruct()
        {
            base.clearQuickLookup();
            this.m_augs.Clear();
            this.m_perkInstanceToAugMap.Clear();
            List<PlayerAugmentation> list = GameLogic.Binder.PlayerAugmentationResources.getOrderedList();
            for (int i = 0; i < list.Count; i++)
            {
                PlayerAugmentation augmentation = list[i];
                bool flag = false;
                for (int j = 0; j < this.Instances.Count; j++)
                {
                    if (this.Instances[j].PlayerAugmentationId == augmentation.Id)
                    {
                        flag = true;
                        break;
                    }
                }
                this.m_augs.Add(augmentation.Id, flag);
                this.m_perkInstanceToAugMap.Add(augmentation.PerkInstance, augmentation);
                if (flag)
                {
                    base.addQuickLookupPerkInstance(augmentation.PerkInstance, false);
                }
            }
            base.refreshQuickLookup();
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

