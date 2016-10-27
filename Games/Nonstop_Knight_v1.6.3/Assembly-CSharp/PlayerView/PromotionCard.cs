namespace PlayerView
{
    using App;
    using Service;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class PromotionCard : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private Content <ActiveContent>k__BackingField;
        [CompilerGenerated]
        private LayoutElement <BannerImageLayoutElement>k__BackingField;
        [CompilerGenerated]
        private RectTransform <BannerImageRectTransform>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.UI.VerticalLayoutGroup <VerticalLayoutGroup>k__BackingField;
        public RawImage BannerImage;
        public GameObject CallToActionRoot;
        public Text CallToActionText;
        private float m_bannerImageDefaultHeight;

        protected void Awake()
        {
            this.BannerImageLayoutElement = this.BannerImage.GetComponent<LayoutElement>();
            this.BannerImageRectTransform = this.BannerImage.GetComponent<RectTransform>();
            this.VerticalLayoutGroup = base.GetComponent<UnityEngine.UI.VerticalLayoutGroup>();
            this.m_bannerImageDefaultHeight = this.BannerImageLayoutElement.preferredHeight;
        }

        public void cleanUpForReuse()
        {
            bool flag = false;
            if (this.BannerImage.texture != null)
            {
                UnityEngine.Object.DestroyImmediate(this.BannerImage.texture);
                this.BannerImage.texture = null;
                flag = true;
            }
            if ((ConfigDevice.DeviceQuality() <= DeviceQualityType.Low) && flag)
            {
                Resources.UnloadUnusedAssets();
            }
        }

        public void initialize(Content content, Action<Card> clickCallback)
        {
            this.ActiveContent = content;
            string shopBannerCta = content.Promotion.ParsedLoca.ShopBannerCta;
            this.CallToActionRoot.SetActive(!string.IsNullOrEmpty(shopBannerCta));
            this.CallToActionText.text = shopBannerCta;
            this.BannerImage.texture = this.ActiveContent.RemoteTexture;
            if (this.BannerImage.texture != null)
            {
                float num = ((float) this.BannerImage.texture.width) / ((float) this.BannerImage.texture.height);
                float num2 = this.BannerImageLayoutElement.preferredWidth / num;
                this.BannerImageLayoutElement.preferredHeight = num2;
            }
            else
            {
                this.BannerImageLayoutElement.preferredHeight = this.m_bannerImageDefaultHeight;
            }
        }

        public void onClick()
        {
            Service.Binder.PromotionManager.ShowPromotionPopup(this.ActiveContent.Promotion);
        }

        public Content ActiveContent
        {
            [CompilerGenerated]
            get
            {
                return this.<ActiveContent>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ActiveContent>k__BackingField = value;
            }
        }

        public LayoutElement BannerImageLayoutElement
        {
            [CompilerGenerated]
            get
            {
                return this.<BannerImageLayoutElement>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<BannerImageLayoutElement>k__BackingField = value;
            }
        }

        public RectTransform BannerImageRectTransform
        {
            [CompilerGenerated]
            get
            {
                return this.<BannerImageRectTransform>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<BannerImageRectTransform>k__BackingField = value;
            }
        }

        public UnityEngine.UI.VerticalLayoutGroup VerticalLayoutGroup
        {
            [CompilerGenerated]
            get
            {
                return this.<VerticalLayoutGroup>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<VerticalLayoutGroup>k__BackingField = value;
            }
        }

        public class Content
        {
            public RemotePromotion Promotion;
            public Texture2D RemoteTexture;
        }
    }
}

