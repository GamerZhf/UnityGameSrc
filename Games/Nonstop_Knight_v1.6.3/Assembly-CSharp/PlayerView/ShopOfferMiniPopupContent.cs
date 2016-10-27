namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ShopOfferMiniPopupContent : MenuContent
    {
        [CompilerGenerated]
        private bool <CloseAllMenusBeforeTransitioningToIapShop>k__BackingField;
        [CompilerGenerated]
        private ShopEntry <OfferedShopEntry>k__BackingField;

        private double GetDiamondAmount(ShopEntry entry)
        {
            if (entry.BuyResourceAmounts.ContainsKey(ResourceType.Diamond))
            {
                return entry.BuyResourceAmounts[ResourceType.Diamond];
            }
            return 0.0;
        }

        private ShopEntry GetIAPShopEntryByMissingAmount(double missing)
        {
            List<ShopEntry> list = new List<ShopEntry>();
            for (int i = 0; i < ShopManager.ValidPlacementSlots.Length; i++)
            {
                string slot = ShopManager.ValidPlacementSlots[i];
                ShopEntry shopEntryBySlot = Service.Binder.ShopManager.GetShopEntryBySlot(slot);
                if (shopEntryBySlot != null)
                {
                    list.Add(shopEntryBySlot);
                }
            }
            list.Sort(new Comparison<ShopEntry>(this.SortByGemAmount));
            for (int j = 0; j < list.Count; j++)
            {
                if (this.GetDiamondAmount(list[j]) >= missing)
                {
                    return list[j];
                }
            }
            return ConfigShops.GetIapShopEntry("com.koplagames.kopla01.diamondssmall");
        }

        protected override void onAwake()
        {
        }

        public override bool onBackgroundOverlayClicked()
        {
            return true;
        }

        protected override void onCleanup()
        {
        }

        public override bool onMainButtonClicked()
        {
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
                return false;
            }
            MiniPopupMenu contentMenu = (MiniPopupMenu) base.m_contentMenu;
            IapMiniPopupContent.InputParameters parameters2 = new IapMiniPopupContent.InputParameters();
            parameters2.ShopEntry = this.OfferedShopEntry;
            parameters2.PathToShop = contentMenu.InputParams.PathToShop;
            parameters2.PurchaseCallback = contentMenu.InputParams.PurchaseCallback;
            IapMiniPopupContent.InputParameters parameter = parameters2;
            if (this.CloseAllMenusBeforeTransitioningToIapShop)
            {
                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.TechPopupMenu, MenuContentType.IapMiniPopupContent, parameter);
            }
            else
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TechPopupMenu, MenuContentType.IapMiniPopupContent, parameter, 0f, true, true);
            }
            return true;
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            bool flag;
            string formattedPrice;
            Sprite sprite;
            double missing = (param == null) ? 0.0 : ((double) param);
            this.OfferedShopEntry = null;
            if (Service.Binder.ShopService.ProductsAvailable)
            {
                this.OfferedShopEntry = this.GetIAPShopEntryByMissingAmount(missing);
            }
            else if (!ConfigApp.ProductionBuild && ConfigApp.CHEAT_IAP_TEST_MODE)
            {
                this.OfferedShopEntry = ConfigShops.IAP_SHOP_ENTRIES[ConfigShops.IAP_SHOP_ENTRIES.Length - 1];
            }
            RewardGalleryCell.Content rewardContent = new RewardGalleryCell.Content();
            if (this.OfferedShopEntry == null)
            {
                flag = false;
                formattedPrice = null;
                sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_connectionerror");
                rewardContent.Sprite = ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Diamond];
                rewardContent.Text = string.Empty;
            }
            else
            {
                flag = true;
                formattedPrice = this.OfferedShopEntry.FormattedPrice;
                sprite = null;
                ResourceType diamond = ResourceType.Diamond;
                foreach (ResourceType type2 in this.OfferedShopEntry.BuyResourceAmounts.Keys)
                {
                    diamond = type2;
                    break;
                }
                rewardContent.Sprite = ConfigUi.RESOURCE_TYPE_SPRITES[diamond];
                rewardContent.Text = "+" + MenuHelpers.BigValueToString(this.OfferedShopEntry.BuyResourceAmounts[diamond]);
            }
            MiniPopupMenu contentMenu = (MiniPopupMenu) base.m_contentMenu;
            contentMenu.populateLayout(ConfigUi.MiniPopupEntries.SHOP_OFFER, flag, rewardContent, formattedPrice, sprite, null);
            if (PlayerView.Binder.MenuSystem.menuTypeInActiveStack(MenuType.RewardCeremonyMenu))
            {
                contentMenu.ShopButton.SetActive(false);
                this.CloseAllMenusBeforeTransitioningToIapShop = false;
            }
            else
            {
                this.CloseAllMenusBeforeTransitioningToIapShop = true;
            }
        }

        protected override void onRefresh()
        {
        }

        private int SortByGemAmount(ShopEntry a, ShopEntry b)
        {
            return (int) (this.GetDiamondAmount(a) - this.GetDiamondAmount(b));
        }

        public bool CloseAllMenusBeforeTransitioningToIapShop
        {
            [CompilerGenerated]
            get
            {
                return this.<CloseAllMenusBeforeTransitioningToIapShop>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CloseAllMenusBeforeTransitioningToIapShop>k__BackingField = value;
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.ShopOfferMiniPopupContent;
            }
        }

        public ShopEntry OfferedShopEntry
        {
            [CompilerGenerated]
            get
            {
                return this.<OfferedShopEntry>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<OfferedShopEntry>k__BackingField = value;
            }
        }
    }
}

