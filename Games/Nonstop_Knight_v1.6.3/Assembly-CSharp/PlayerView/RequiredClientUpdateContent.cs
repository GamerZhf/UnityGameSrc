namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class RequiredClientUpdateContent : MenuContent
    {
        [CompilerGenerated]
        private string <DownloadUrl>k__BackingField;
        public Text ContinueOfflineButtonText;
        public Text Description;
        public Text UpdateNowButtonText;

        protected override void onAwake()
        {
            this.Description.text = _.L(ConfigLoca.CLIENT_UPDATE_DESCRIPTION, null, false);
            this.UpdateNowButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.CLIENT_UPDATE_PROMPT_UPDATE_NOW, null, false));
            this.ContinueOfflineButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.CLIENT_UPDATE_PROMPT_CONTINUE_OFFLINE, null, false));
        }

        public override bool onBackgroundOverlayClicked()
        {
            return true;
        }

        protected override void onCleanup()
        {
        }

        public void onContinueOfflineButtonClicked()
        {
            Binder.MenuSystem.returnToPreviousMenu(true);
        }

        protected void OnDisable()
        {
            Binder.InputSystem.PopBackNavigationListener();
        }

        protected void OnEnable()
        {
            Binder.InputSystem.PushBackNavigationListener(new System.Action(this.onContinueOfflineButtonClicked));
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object parameters)
        {
            InputParameters parameters2 = (InputParameters) parameters;
            this.DownloadUrl = parameters2.DownloadUrl;
            base.m_contentMenu.setCloseButtonVisibility(false);
        }

        protected override void onRefresh()
        {
        }

        public void onUpdateNowButtonClicked()
        {
            Application.OpenURL(this.DownloadUrl);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.RequiredClientUpdateContent;
            }
        }

        public string DownloadUrl
        {
            [CompilerGenerated]
            get
            {
                return this.<DownloadUrl>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DownloadUrl>k__BackingField = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public string DownloadUrl;
        }
    }
}

