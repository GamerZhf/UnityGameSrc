namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemGalleryContent : MenuContent
    {
        private List<ItemCell> m_itemCells = new List<ItemCell>();
        private InputParameters m_params;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public Text TokenCount;
        public RectTransform VerticalGroup;

        private void addItemCellToList(ItemInstance ii)
        {
            ItemCell item = PlayerView.Binder.ItemCellPool.getObject();
            item.transform.SetParent(this.VerticalGroup, false);
            bool stripedRow = (this.VerticalGroup.childCount % 2) != 0;
            item.initialize(ii, stripedRow);
            this.m_itemCells.Add(item);
            item.gameObject.SetActive(true);
        }

        private void cleanupCells()
        {
            for (int i = this.m_itemCells.Count - 1; i >= 0; i--)
            {
                ItemCell item = this.m_itemCells[i];
                this.m_itemCells.Remove(item);
                PlayerView.Binder.ItemCellPool.returnObject(item);
            }
        }

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
            GameLogic.Binder.GameState.Player.Notifiers.markAllNotificationsForItemsThatWeCanUpgradeAsInspected(this.m_params.ItemType);
            this.cleanupCells();
            GameLogic.Binder.EventBus.OnItemUnlocked -= new GameLogic.Events.ItemUnlocked(this.onItemUnlocked);
            GameLogic.Binder.EventBus.OnItemEquipped -= new GameLogic.Events.ItemEquipped(this.onItemEquipped);
            GameLogic.Binder.EventBus.OnItemRankUpped -= new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnItemInspected -= new GameLogic.Events.ItemInspected(this.onItemInspected);
            GameLogic.Binder.EventBus.OnItemSold -= new GameLogic.Events.ItemSold(this.onItemSold);
            GameLogic.Binder.EventBus.OnResourcesGained -= new GameLogic.Events.ResourcesGained(this.onResourcesGained);
        }

        private void onItemEquipped(CharacterInstance character, ItemInstance itemInstance, ItemInstance replacedItemInstance)
        {
            character.OwningPlayer.Notifiers.markItemNotificationsAsInspected(itemInstance);
            this.onRefresh();
        }

        private void onItemInspected(ItemInstance itemInstance)
        {
            this.onRefresh();
        }

        private void onItemRankUpped(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free)
        {
            Player owningPlayer = character.OwningPlayer;
            double coins = owningPlayer.getResourceAmount(ResourceType.Coin);
            if (!owningPlayer.canUpgradeItemInstance(itemInstance, coins))
            {
                owningPlayer.Notifiers.markItemNotificationsAsNonInspected(itemInstance);
            }
            this.onRefresh();
        }

        private void onItemSold(CharacterInstance character, ItemInstance itemInstance, double amount, RectTransform flyToHudOrigin)
        {
            this.reconstructContent();
        }

        private void onItemUnlocked(CharacterInstance character, ItemInstance itemInstance)
        {
            this.onRefresh();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_params = (InputParameters) param;
            GameLogic.Binder.EventBus.OnItemUnlocked += new GameLogic.Events.ItemUnlocked(this.onItemUnlocked);
            GameLogic.Binder.EventBus.OnItemEquipped += new GameLogic.Events.ItemEquipped(this.onItemEquipped);
            GameLogic.Binder.EventBus.OnItemRankUpped += new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnItemInspected += new GameLogic.Events.ItemInspected(this.onItemInspected);
            GameLogic.Binder.EventBus.OnItemSold += new GameLogic.Events.ItemSold(this.onItemSold);
            GameLogic.Binder.EventBus.OnResourcesGained += new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            this.ScrollRect.verticalNormalizedPosition = 1f;
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(this.m_params.ItemType.ToString()) + "S", string.Empty, string.Empty);
            this.reconstructContent();
        }

        protected override void onPreWarm([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_params = (InputParameters) param;
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            base.m_contentMenu.refreshTabs();
            for (int i = 0; i < this.m_itemCells.Count; i++)
            {
                this.m_itemCells[i].refresh();
            }
            this.TokenCount.text = MenuHelpers.BigValueToString(player.getResourceAmount(ResourceType.Token));
        }

        private void onResourcesGained(Player player, ResourceType resourceType, double amount, bool instant, string analyticsSourceId, Vector3? worldPt)
        {
            if ((amount < 0.0) && (resourceType == ResourceType.Coin))
            {
                player.Notifiers.markAllNotificationsForItemsThatWeCannotUpgradeAsNonInspected(this.m_params.ItemType);
            }
            this.onRefresh();
        }

        private void reconstructContent()
        {
            this.cleanupCells();
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            List<ItemInstance> list = new List<ItemInstance>();
            List<ItemInstance> list2 = activeCharacter.getItemInstances(false);
            for (int i = 0; i < list2.Count; i++)
            {
                if (list2[i].Item.Type == this.m_params.ItemType)
                {
                    list.Add(list2[i]);
                }
            }
            List<ItemSlot> list3 = activeCharacter.getItemSlots(false);
            for (int j = 0; j < list3.Count; j++)
            {
                if ((list3[j].ItemInstance != null) && (list3[j].ItemInstance.Item.Type == this.m_params.ItemType))
                {
                    list.Add(list3[j].ItemInstance);
                }
            }
            list.Sort();
            for (int k = 0; k < list.Count; k++)
            {
                this.addItemCellToList(list[k]);
            }
            base.refresh();
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.ItemGalleryContent;
            }
        }

        public override MenuContent.TabSpriteParameters TabSprite
        {
            get
            {
                MenuContent.TabSpriteParameters parameters = new MenuContent.TabSpriteParameters();
                parameters.SpriteAtlasId = ConfigUi.ITEM_TYPE_ABSTRACT_SPRITES_IDS[this.m_params.ItemType].AtlasId;
                parameters.SpriteId = ConfigUi.ITEM_TYPE_ABSTRACT_SPRITES_IDS[this.m_params.ItemType].SpriteId;
                parameters.SpriteSize = new Vector2(85f, 85f);
                return parameters;
            }
        }

        public override bool UnlockNotificationActive
        {
            get
            {
                return (GameLogic.Binder.GameState.Player.Notifiers.getNumberOfGoldItemNotifications(this.m_params.ItemType) > 0);
            }
        }

        public override bool UpgradeNotificationActive
        {
            get
            {
                return false;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public GameLogic.ItemType ItemType;
        }
    }
}

