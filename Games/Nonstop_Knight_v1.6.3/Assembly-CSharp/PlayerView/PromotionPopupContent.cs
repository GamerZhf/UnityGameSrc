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

    public class PromotionPopupContent : RewardedPopupContent
    {
        public GameObject Button;
        public Image ButtonIcon;
        public Text ButtonText;
        public GameObject DividerRoot;
        public Text DividerTitle;
        public Text HeaderTitle;
        private float m_defaultHeight;
        private float m_defaultWidth;
        private InputParameters m_inputParameters;
        public Text MessageText;
        public RawImage RawBg;

        private Sprite GetIconForSprite(SpriteAtlasEntry spriteEntry)
        {
            try
            {
                return PlayerView.Binder.SpriteResources.getSprite(spriteEntry);
            }
            catch (Exception)
            {
                Debug.LogError("Sprite not found " + spriteEntry.AtlasId + "." + spriteEntry.SpriteId);
            }
            return null;
        }

        private ResourceType GetResourceType(string costResourceType)
        {
            if (ResourceType.Coin.ToString().Equals(costResourceType))
            {
                return ResourceType.Coin;
            }
            if (!ResourceType.Diamond.ToString().Equals(costResourceType) && ResourceType.Diamond.ToString().Equals(costResourceType))
            {
                return ResourceType.Token;
            }
            return ResourceType.Diamond;
        }

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
                if (this.m_inputParameters.Promotion.PromotionType == EPromotionType.Info)
                {
                    Service.Binder.PromotionManager.ConsumePromotion(this.m_inputParameters.Promotion, true);
                    Service.Binder.EventBus.PromotionAction(this.m_inputParameters.Promotion.promotionid, "info");
                }
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onDeeplinkButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Service.Binder.PromotionManager.ConsumePromotion(this.m_inputParameters.Promotion, true);
                PlayerView.Binder.MenuSystem.instantCloseAllMenus();
                this.onUrlButtonClick(this.m_inputParameters.DeeplinkUrl);
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_inputParameters = (InputParameters) param;
            if (!string.IsNullOrEmpty(this.m_inputParameters.Headline))
            {
                this.HeaderTitle.text = StringExtensions.ToUpperLoca(this.m_inputParameters.Headline);
                this.HeaderTitle.enabled = true;
            }
            else
            {
                this.HeaderTitle.enabled = false;
            }
            if (!string.IsNullOrEmpty(this.m_inputParameters.Title))
            {
                this.DividerRoot.SetActive(true);
                this.DividerTitle.enabled = true;
                this.DividerTitle.text = StringExtensions.ToUpperLoca(this.m_inputParameters.Title);
            }
            else
            {
                this.DividerRoot.SetActive(false);
                this.DividerTitle.enabled = false;
            }
            if (!string.IsNullOrEmpty(this.m_inputParameters.Message))
            {
                this.MessageText.enabled = true;
                this.MessageText.text = this.m_inputParameters.Message;
            }
            else
            {
                this.MessageText.enabled = false;
            }
            if (this.m_inputParameters.CostAmount > 0.0)
            {
                this.Button.SetActive(true);
                this.ButtonIcon.gameObject.SetActive(true);
                this.ButtonText.text = MenuHelpers.BigValueToString(Math.Round(this.m_inputParameters.CostAmount));
                this.SetIconForResource(ref this.ButtonIcon, this.m_inputParameters.CostResourceType);
            }
            else if (!string.IsNullOrEmpty(this.m_inputParameters.ButtonText))
            {
                this.Button.SetActive(true);
                this.ButtonIcon.gameObject.SetActive(false);
                this.ButtonText.text = this.m_inputParameters.ButtonText;
                if (this.m_inputParameters.ButtonIcon != null)
                {
                    Sprite iconForSprite = this.GetIconForSprite(this.m_inputParameters.ButtonIcon);
                    if (iconForSprite != null)
                    {
                        this.ButtonIcon.gameObject.SetActive(true);
                        this.ButtonIcon.sprite = iconForSprite;
                    }
                }
            }
            else
            {
                this.Button.SetActive(false);
            }
            if (this.m_inputParameters.DisposableBackgroundTexture != null)
            {
                this.RawBg.enabled = true;
                this.RawBg.texture = this.m_inputParameters.DisposableBackgroundTexture;
                float num = ((float) this.m_inputParameters.DisposableBackgroundTexture.width) / ((float) this.m_inputParameters.DisposableBackgroundTexture.height);
                float newSize = Mathf.Clamp((float) (this.m_defaultWidth / num), (float) 200f, (float) 1880f);
                RectTransformExtensions.SetHeight(base.RectTm, newSize);
                base.RewardsGrid.gameObject.SetActive(false);
            }
            else
            {
                this.RawBg.enabled = false;
                RectTransformExtensions.SetHeight(base.RectTm, this.m_defaultHeight);
                List<Reward> rewards = Service.Binder.PromotionManager.GetRewards(this.m_inputParameters.Promotion);
                if ((rewards != null) && (rewards.Count > 0))
                {
                    base.RewardsGrid.gameObject.SetActive(true);
                    base.InitRewardGrid(rewards);
                }
                else
                {
                    base.RewardsGrid.gameObject.SetActive(false);
                }
            }
        }

        protected override void onRefresh()
        {
        }

        public void onRewardButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                if (this.m_inputParameters.Promotion.PromotionType == EPromotionType.Deeplink)
                {
                    this.onDeeplinkButtonClicked();
                }
                else
                {
                    PlayerView.Binder.MenuSystem.instantCloseAllMenus();
                    Player player = GameLogic.Binder.GameState.Player;
                    double num = player.getResourceAmount(this.GetResourceType(this.m_inputParameters.CostResourceType));
                    double num2 = this.m_inputParameters.CostAmount - num;
                    if (num2 > 0.0)
                    {
                        MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                        parameters2.MenuContentParams = num2;
                        MiniPopupMenu.InputParameters parameter = parameters2;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.ShopOfferMiniPopupContent, parameter, 0f, true, true);
                    }
                    else
                    {
                        CmdBuyPromotion.ExecuteStatic(player, this.m_inputParameters.Promotion);
                        Service.Binder.PromotionManager.ConsumePromotion(this.m_inputParameters.Promotion, true);
                        Service.Binder.EventBus.PromotionAction(this.m_inputParameters.Promotion.promotionid, "reward");
                    }
                }
            }
        }

        public void onSingleButtonClicked()
        {
            if (this.m_inputParameters.Promotion.PromotionType == EPromotionType.Reward)
            {
                this.onRewardButtonClicked();
            }
            else
            {
                this.onDeeplinkButtonClicked();
            }
        }

        private void onUrlButtonClick(string url)
        {
            DeepLinkHandler.ExecuteDeepLink(url);
            Service.Binder.EventBus.PromotionAction(this.m_inputParameters.Promotion.promotionid, "deeplink");
        }

        private void SetIconForResource(ref Image costIcon, string costResourceType)
        {
            SpriteAtlasEntry atlasEntry = ConfigUi.RESOURCE_TYPE_SPRITES[this.GetResourceType(costResourceType)];
            if (atlasEntry != null)
            {
                costIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(atlasEntry);
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.PromotionPopupContent;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public RemotePromotion Promotion;
            public string Headline;
            public string Title;
            public string Message;
            public string ButtonText;
            public string DeeplinkUrl;
            public SpriteAtlasEntry ButtonIcon;
            public Texture2D DisposableBackgroundTexture;
            public string CostResourceType;
            public double CostAmount;
        }
    }
}

