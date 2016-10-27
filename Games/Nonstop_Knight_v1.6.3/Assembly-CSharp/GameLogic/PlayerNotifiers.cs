namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PlayerNotifiers
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        public bool AugmentationShopInspected = true;
        public bool HeroRetirementsInspected = true;
        private Dictionary<PlayerAugmentation, bool> m_inspectedAugNotifiers = new Dictionary<PlayerAugmentation, bool>();
        private Dictionary<ItemInstance, bool> m_inspectedItemNotifiers = new Dictionary<ItemInstance, bool>();
        public bool PotionsInspected = true;
        public bool ShopInspected = true;

        public PlayerNotifiers(GameLogic.Player player)
        {
            this.Player = player;
            List<PlayerAugmentation> list = GameLogic.Binder.PlayerAugmentationResources.getOrderedList();
            for (int i = 0; i < list.Count; i++)
            {
                this.m_inspectedAugNotifiers.Add(list[i], false);
            }
            this.refreshAugShopInspectedFlag();
        }

        public int getNumberOfGoldItemNotifications()
        {
            int num = 0;
            List<ItemSlot> list = this.Player.ActiveCharacter.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (this.isItemGoldNotificationActive(itemInstance))
                {
                    num++;
                }
            }
            List<ItemInstance> list2 = this.Player.ActiveCharacter.getItemInstances(false);
            for (int j = 0; j < list2.Count; j++)
            {
                ItemInstance ii = list2[j];
                if (this.isItemGoldNotificationActive(ii))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfGoldItemNotifications(ItemType itemType)
        {
            int num = 0;
            List<ItemSlot> list = this.Player.ActiveCharacter.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (((itemInstance != null) && (itemInstance.Item.Type == itemType)) && this.isItemGoldNotificationActive(itemInstance))
                {
                    num++;
                }
            }
            List<ItemInstance> list2 = this.Player.ActiveCharacter.getItemInstances(false);
            for (int j = 0; j < list2.Count; j++)
            {
                ItemInstance ii = list2[j];
                if (((ii != null) && (ii.Item.Type == itemType)) && this.isItemGoldNotificationActive(ii))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfGoldSkillNotifications()
        {
            int num = 0;
            for (int i = 0; i < ConfigSkills.ALL_HERO_SKILLS.Count; i++)
            {
                SkillType skillType = ConfigSkills.ALL_HERO_SKILLS[i];
                if (this.isSkillGoldNotificationActive(skillType))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumberOfGreenItemNotifications()
        {
            double coins = this.Player.getResourceAmount(ResourceType.Coin);
            int num2 = 0;
            List<ItemSlot> list = this.Player.ActiveCharacter.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (this.isItemGreenNotificationActive(itemInstance, coins))
                {
                    num2++;
                }
            }
            return num2;
        }

        public int getNumberOfGreenItemNotifications(ItemType itemType)
        {
            double coins = this.Player.getResourceAmount(ResourceType.Coin);
            int num2 = 0;
            List<ItemSlot> list = this.Player.ActiveCharacter.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (((itemInstance != null) && (itemInstance.Item.Type == itemType)) && this.isItemGreenNotificationActive(itemInstance, coins))
                {
                    num2++;
                }
            }
            return num2;
        }

        public bool isItemGoldNotificationActive(ItemInstance ii)
        {
            if (ii != null)
            {
                ItemType type = ii.Item.Type;
                if (type != ItemType.Armor)
                {
                    if ((type == ItemType.Cloak) && !this.Player.hasCompletedTutorial("TUT052A"))
                    {
                        return false;
                    }
                    goto Label_005F;
                }
                if (this.Player.hasCompletedTutorial("TUT051A"))
                {
                    goto Label_005F;
                }
            }
            return false;
        Label_005F:
            if (this.Player.canEvolveItem(ii))
            {
                return true;
            }
            return this.Player.canUnlockItemInstance(ii);
        }

        public bool isItemGreenNotificationActive(ItemInstance ii, double coins)
        {
            if (ii == null)
            {
                return false;
            }
            if (this.m_inspectedItemNotifiers.ContainsKey(ii) && this.m_inspectedItemNotifiers[ii])
            {
                return false;
            }
            if (!this.Player.ActiveCharacter.isItemInstanceEquipped(ii))
            {
                return false;
            }
            return this.Player.canUpgradeItemInstance(ii, coins);
        }

        public bool isSkillGoldNotificationActive(SkillType skillType)
        {
            SkillInstance instance = this.Player.ActiveCharacter.getSkillInstance(skillType);
            return (((instance != null) && !instance.InspectedByPlayer) || (this.Player.Runestones.getNumberOfUninspectedRunestonesAffectingSkill(skillType) > 0));
        }

        public void markAllAugNotificationsThatWeCannotPurchaseAsUninspected()
        {
            List<PlayerAugmentation> list = GameLogic.Binder.PlayerAugmentationResources.getOrderedList();
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                PlayerAugmentation augmentation = list[i];
                if (!this.Player.Augmentations.hasAugmentation(augmentation.Id))
                {
                    num++;
                    if (!this.Player.Augmentations.canBuy(augmentation.Id))
                    {
                        this.m_inspectedAugNotifiers[augmentation] = false;
                    }
                    if (num > 6)
                    {
                        break;
                    }
                }
            }
            this.refreshAugShopInspectedFlag();
        }

        public void markAllAugNotificationsThatWeCanPurchaseAsInspected()
        {
            List<PlayerAugmentation> list = GameLogic.Binder.PlayerAugmentationResources.getOrderedList();
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                PlayerAugmentation augmentation = list[i];
                if (!this.Player.Augmentations.hasAugmentation(augmentation.Id))
                {
                    num++;
                    if (this.Player.Augmentations.canBuy(augmentation.Id))
                    {
                        this.m_inspectedAugNotifiers[augmentation] = true;
                    }
                    if (num > 6)
                    {
                        break;
                    }
                }
            }
            this.refreshAugShopInspectedFlag();
        }

        public void markAllNotificationsForItemsThatWeCannotUpgradeAsNonInspected(ItemType itemType)
        {
            double coins = this.Player.getResourceAmount(ResourceType.Coin);
            CharacterInstance activeCharacter = this.Player.ActiveCharacter;
            List<ItemSlot> list = activeCharacter.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (((itemInstance != null) && (itemInstance.Item.Type == itemType)) && !this.Player.canUpgradeItemInstance(itemInstance, coins))
                {
                    this.markItemNotificationsAsNonInspected(itemInstance);
                }
            }
            List<ItemInstance> list2 = activeCharacter.getItemInstances(false);
            for (int j = 0; j < list2.Count; j++)
            {
                ItemInstance ii = list2[j];
                if (((ii != null) && (ii.Item.Type == itemType)) && !this.Player.canUpgradeItemInstance(ii, coins))
                {
                    this.markItemNotificationsAsNonInspected(ii);
                }
            }
        }

        public void markAllNotificationsForItemsThatWeCanUpgradeAsInspected(ItemType itemType)
        {
            double coins = this.Player.getResourceAmount(ResourceType.Coin);
            CharacterInstance activeCharacter = this.Player.ActiveCharacter;
            List<ItemSlot> list = activeCharacter.getItemSlots(false);
            for (int i = 0; i < list.Count; i++)
            {
                ItemInstance itemInstance = list[i].ItemInstance;
                if (((itemInstance != null) && (itemInstance.Item.Type == itemType)) && this.Player.canUpgradeItemInstance(itemInstance, coins))
                {
                    LangUtil.AddOrUpdateDictionaryEntry<ItemInstance, bool>(this.m_inspectedItemNotifiers, itemInstance, true);
                }
            }
            List<ItemInstance> list2 = activeCharacter.getItemInstances(false);
            for (int j = 0; j < list2.Count; j++)
            {
                ItemInstance ii = list2[j];
                if (((ii != null) && (ii.Item.Type == itemType)) && this.Player.canUpgradeItemInstance(ii, coins))
                {
                    LangUtil.AddOrUpdateDictionaryEntry<ItemInstance, bool>(this.m_inspectedItemNotifiers, ii, true);
                }
            }
        }

        public void markItemNotificationsAsInspected(ItemInstance ii)
        {
            LangUtil.AddOrUpdateDictionaryEntry<ItemInstance, bool>(this.m_inspectedItemNotifiers, ii, true);
        }

        public void markItemNotificationsAsNonInspected(ItemInstance ii)
        {
            LangUtil.AddOrUpdateDictionaryEntry<ItemInstance, bool>(this.m_inspectedItemNotifiers, ii, false);
        }

        public void refreshAugShopInspectedFlag()
        {
            List<PlayerAugmentation> list = GameLogic.Binder.PlayerAugmentationResources.getOrderedList();
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                PlayerAugmentation aug = list[i];
                if (!this.Player.Augmentations.hasAugmentation(aug.Id))
                {
                    num++;
                    if (this.shouldNotifyAug(aug))
                    {
                        this.AugmentationShopInspected = false;
                        return;
                    }
                    if (num > 6)
                    {
                        break;
                    }
                }
            }
            this.AugmentationShopInspected = true;
        }

        public bool runestoneNotificationsActive()
        {
            return (this.Player.Runestones.getNumberOfUninspectedRunestones() > 0);
        }

        public bool shouldNotifyAug(PlayerAugmentation aug)
        {
            if (this.m_inspectedAugNotifiers[aug])
            {
                return false;
            }
            return this.Player.Augmentations.canBuy(aug.Id);
        }

        public GameLogic.Player Player
        {
            [CompilerGenerated]
            get
            {
                return this.<Player>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Player>k__BackingField = value;
            }
        }
    }
}

