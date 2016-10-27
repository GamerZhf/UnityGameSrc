namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class PromotionIAPPopupContent : RewardedPopupContent
    {
        public Text DescriptionText;
        public Text DividerTitle;
        private float m_defaultHeight;
        private float m_defaultWidth;
        private InputParameters m_inputParameters;
        private ShopPurchaseController m_purchaseController;
        public Text PopupTitle;
        public Text PurchaseButtonText;
        public RawImage RawBg;

        protected override void onAwake()
        {
            this.m_defaultWidth = RectTransformExtensions.GetWidth(base.RectTm);
            this.m_defaultHeight = RectTransformExtensions.GetHeight(base.RectTm);
        }

        protected override void onCleanup()
        {
            bool flag = false;
            if (this.m_inputParameters.DisposableBackgroundTexture != null)
            {
                UnityEngine.Object.Destroy(this.m_inputParameters.DisposableBackgroundTexture);
                this.m_inputParameters.DisposableBackgroundTexture = null;
                this.RawBg.texture = null;
                flag = true;
            }
            if ((ConfigDevice.DeviceQuality() <= DeviceQualityType.Low) && flag)
            {
                Resources.UnloadUnusedAssets();
            }
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
            this.m_inputParameters = (InputParameters) param;
            if (!string.IsNullOrEmpty(this.m_inputParameters.Headline))
            {
                this.PopupTitle.text = StringExtensions.ToUpperLoca(this.m_inputParameters.Headline);
            }
            else
            {
                this.PopupTitle.enabled = false;
            }
            if (!string.IsNullOrEmpty(this.m_inputParameters.Title))
            {
                this.DividerTitle.text = StringExtensions.ToUpperLoca(this.m_inputParameters.Title);
            }
            else
            {
                this.DividerTitle.enabled = false;
            }
            if (!string.IsNullOrEmpty(this.m_inputParameters.Message))
            {
                this.DescriptionText.enabled = true;
                this.DescriptionText.text = this.m_inputParameters.Message;
            }
            else
            {
                this.DescriptionText.enabled = false;
            }
            if (this.m_inputParameters.DisposableBackgroundTexture != null)
            {
                this.RawBg.enabled = true;
                this.RawBg.texture = this.m_inputParameters.DisposableBackgroundTexture;
                float num = ((float) this.m_inputParameters.DisposableBackgroundTexture.width) / ((float) this.m_inputParameters.DisposableBackgroundTexture.height);
                float newSize = Mathf.Clamp((float) (this.m_defaultWidth / num), (float) 200f, (float) 1880f);
                RectTransformExtensions.SetHeight(base.RectTm, newSize);
            }
            else
            {
                this.RawBg.enabled = false;
                RectTransformExtensions.SetHeight(base.RectTm, this.m_defaultHeight);
            }
            string flareProductIdForPromoSlot = Service.Binder.PromotionManager.GetFlareProductIdForPromoSlot(this.m_inputParameters.Slot);
            if (!string.IsNullOrEmpty(flareProductIdForPromoSlot))
            {
                ShopEntry shopEntryByFlareProductId = Service.Binder.ShopManager.GetShopEntryByFlareProductId(flareProductIdForPromoSlot);
                if (shopEntryByFlareProductId != null)
                {
                    this.m_purchaseController = new ShopPurchaseController(shopEntryByFlareProductId, null, PathToShopType.Vendor, null, new Action<ShopEntry, PurchaseResult>(this.onPurchaseComplete));
                    this.PurchaseButtonText.text = this.m_purchaseController.getPriceText(1);
                    if (this.m_inputParameters.DisposableBackgroundTexture == null)
                    {
                        base.RewardsGrid.gameObject.SetActive(true);
                        List<Reward> rewards = new List<Reward>();
                        PremiumProduct prod = Service.Binder.ShopService.GetProduct(flareProductIdForPromoSlot);
                        foreach (ProductReward reward in ConfigShops.GetRewardsFromProduct(prod))
                        {
                            rewards.Add(ConfigShops.GetRewardFromProductReward(reward, prod));
                        }
                        base.InitRewardGrid(rewards);
                    }
                    else
                    {
                        base.RewardsGrid.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void onPurchaseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && this.m_purchaseController.isPurchaseable())
            {
                PlayerView.Binder.MenuSystem.instantCloseAllMenus();
                this.m_purchaseController.purchase(1);
            }
        }

        private void onPurchaseComplete(ShopEntry entry, PurchaseResult result)
        {
            if (result == PurchaseResult.Success)
            {
                Service.Binder.PromotionManager.ConsumePromotion(this.m_inputParameters.Promotion, true);
                Service.Binder.EventBus.PromotionAction(this.m_inputParameters.Promotion.promotionid, "purchase");
            }
        }

        protected override void onRefresh()
        {
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.PromotionIAPPopupContent;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public string Slot;
            public RemotePromotion Promotion;
            public string Headline;
            public string Title;
            public string Message;
            public Texture2D DisposableBackgroundTexture;
        }
    }
}

