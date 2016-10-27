namespace PlayerView
{
    using App;
    using Appboy;
    using Service;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class AppboyPopupContent : MenuContent
    {
        public Image DefaultIcon;
        public GameObject DividerRoot;
        public Text DividerTitle;
        public GameObject DualButtonLeft;
        public Text DualButtonLeftText;
        public GameObject DualButtonRight;
        public Text DualButtonRightText;
        public GameObject DualButtonRoot;
        public Text HeaderTitle;
        private float m_defaultHeight;
        private float m_defaultWidth;
        private InputParameters m_inputParameters;
        public Text MessageText;
        public RawImage RawBg;
        public RawImage RawIcon;
        public GameObject SingleButton;
        public Text SingleButtonText;

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
            if (this.m_inputParameters.DisposableIconTexture != null)
            {
                UnityEngine.Object.Destroy(this.m_inputParameters.DisposableIconTexture);
                this.m_inputParameters.DisposableIconTexture = null;
                this.RawIcon.texture = null;
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
                PlayerView.Binder.EventBus.AppboyAction("message-dismissed", this.m_inputParameters.CampaignId);
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onDualButtonLeftClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                AppboyBinding.LogInAppMessageButtonClicked(this.m_inputParameters.OriginalMessage.JSONData, 0);
                PlayerView.Binder.EventBus.AppboyAction("button1-clicked", this.m_inputParameters.CampaignId);
                this.onUrlButtonClick(this.m_inputParameters.DualButtonLeftUrl);
            }
        }

        public void onDualButtonRightClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                AppboyBinding.LogInAppMessageButtonClicked(this.m_inputParameters.OriginalMessage.JSONData, 1);
                PlayerView.Binder.EventBus.AppboyAction("button2-clicked", this.m_inputParameters.CampaignId);
                this.onUrlButtonClick(this.m_inputParameters.DualButtonRightUrl);
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_inputParameters = (InputParameters) param;
            if (!string.IsNullOrEmpty(this.m_inputParameters.Headline))
            {
                this.HeaderTitle.text = StringExtensions.ToUpperLoca(this.m_inputParameters.Headline);
            }
            else
            {
                this.HeaderTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.APPBOY_POPUP_HEADER, null, false));
            }
            if (!string.IsNullOrEmpty(this.m_inputParameters.Title))
            {
                this.DividerRoot.SetActive(true);
                this.DividerTitle.text = StringExtensions.ToUpperLoca(this.m_inputParameters.Title);
            }
            else
            {
                this.DividerRoot.SetActive(false);
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
            if (!string.IsNullOrEmpty(this.m_inputParameters.SingleButtonText))
            {
                this.SingleButton.SetActive(true);
                this.DualButtonRoot.SetActive(false);
                this.SingleButtonText.text = this.m_inputParameters.SingleButtonText;
            }
            else
            {
                this.SingleButton.gameObject.SetActive(false);
                if (!string.IsNullOrEmpty(this.m_inputParameters.DualButtonLeftText))
                {
                    this.DualButtonRoot.SetActive(true);
                    this.DualButtonLeftText.text = this.m_inputParameters.DualButtonLeftText;
                    this.DualButtonRightText.text = this.m_inputParameters.DualButtonRightText;
                }
                else
                {
                    this.DualButtonRoot.SetActive(false);
                }
            }
            if (this.m_inputParameters.DisposableIconTexture != null)
            {
                this.DefaultIcon.enabled = false;
                this.RawIcon.enabled = true;
                this.RawIcon.texture = this.m_inputParameters.DisposableIconTexture;
            }
            else
            {
                this.DefaultIcon.enabled = true;
                this.RawIcon.enabled = false;
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
            PlayerView.Binder.EventBus.AppboyAction("message-displayed", this.m_inputParameters.CampaignId);
        }

        protected override void onRefresh()
        {
        }

        public void onSingleButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                AppboyBinding.LogInAppMessageButtonClicked(this.m_inputParameters.OriginalMessage.JSONData, 0);
                PlayerView.Binder.EventBus.AppboyAction("button1-clicked", this.m_inputParameters.CampaignId);
                this.onUrlButtonClick(this.m_inputParameters.SingleButtonUrl);
            }
        }

        private void onUrlButtonClick(string url)
        {
            DeepLinkHandler.ExecuteDeepLink(url);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.AppboyPopupContent;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public AppboyIngameMessage OriginalMessage;
            public string CampaignId;
            public string Headline;
            public string Title;
            public string Message;
            public string SingleButtonText;
            public string SingleButtonUrl;
            public string DualButtonLeftText;
            public string DualButtonLeftUrl;
            public string DualButtonRightText;
            public string DualButtonRightUrl;
            public Texture2D DisposableIconTexture;
            public Texture2D DisposableBackgroundTexture;
        }
    }
}

