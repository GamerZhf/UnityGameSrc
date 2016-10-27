namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine.UI;

    public class StarterBundlePopupContent : MenuContent
    {
        public Text CritterName;
        public Text DescriptionText;
        public Text DividerTitle;
        public Text GemAmount;
        private ShopPurchaseController m_starterBundleSpc;
        public Text PopupTitle;
        public Text PurchaseButtonText;
        public Text TokenAmount;

        protected override void onAwake()
        {
            this.PopupTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.IAP_STARTERBUNDLE_POPUP_TITLE, null, false));
            this.DividerTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.IAP_STARTERBUNDLE_DIVIDER_TITLE, null, false));
            this.DescriptionText.text = _.L(ConfigLoca.IAP_STARTERBUNDLE_DESCRIPTION, null, false);
        }

        protected override void onCleanup()
        {
        }

        public void onCustomCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            ShopEntry shopEntryBySlot = Service.Binder.ShopManager.GetShopEntryBySlot(ShopManager.ValidBundleSlots[0]);
            this.m_starterBundleSpc = new ShopPurchaseController(shopEntryBySlot, null, PathToShopType.Vendor, null, new Action<ShopEntry, PurchaseResult>(this.onPurchaseComplete));
            this.PurchaseButtonText.text = this.m_starterBundleSpc.getPriceText(1);
            this.CritterName.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.IAP_STARTERBUNDLE_CRITTER_NAME, null, false));
            double v = this.m_starterBundleSpc.ShopEntry.BuyResourceAmounts[ResourceType.Token];
            this.TokenAmount.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.RESOURCES_TOKENS_MULTILINE, new <>__AnonType9<string>(MenuHelpers.BigValueToString(v)), false));
            double num2 = this.m_starterBundleSpc.ShopEntry.BuyResourceAmounts[ResourceType.Diamond];
            this.GemAmount.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.RESOURCES_DIAMONDS_MULTILINE, new <>__AnonType9<string>(MenuHelpers.BigValueToString(num2)), false));
        }

        public void onPurchaseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && this.m_starterBundleSpc.isPurchaseable())
            {
                this.m_starterBundleSpc.purchase(1);
            }
        }

        private void onPurchaseComplete(ShopEntry entry, PurchaseResult result)
        {
            if (result == PurchaseResult.Success)
            {
                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.NONE, MenuContentType.NONE, null);
            }
        }

        protected override void onRefresh()
        {
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.StarterBundlePopupContent;
            }
        }
    }
}

