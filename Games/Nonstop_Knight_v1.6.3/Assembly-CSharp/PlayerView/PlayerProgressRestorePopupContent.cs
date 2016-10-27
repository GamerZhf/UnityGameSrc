namespace PlayerView
{
    using App;
    using Service;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerProgressRestorePopupContent : MenuContent
    {
        public GameObject CenterButton;
        public UnityEngine.UI.Text ContinueButtonText;
        public GameObject DualButtons;
        private InputParameters m_params;
        public UnityEngine.UI.Text OkButtonText;
        private string platform;
        public UnityEngine.UI.Text RestoreButtonText;
        private bool restoreError;
        public UnityEngine.UI.Text Text;

        protected override void onAwake()
        {
            this.platform = _.L(ConfigLoca.PLAYER_PROGRESS_PLATFORM_GOOGLE, null, false);
            this.Text.text = _.L(ConfigLoca.PLAYER_PROGRESS_RESTORE_DESCRIPTION, new <>__AnonType19<string>(this.platform), false);
            this.ContinueButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PLAYER_PROGRESS_PROMPT_CONTINUE, null, false));
            this.RestoreButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PLAYER_PROGRESS_PROMPT_RESTORE, null, false));
        }

        protected override void onCleanup()
        {
            PlayerView.Binder.InputSystem.PopBackNavigationListener();
        }

        public void onContinueButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Service.Binder.PlayerService.ResolveLoginConflict(false);
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onOkButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.restoreError = false;
            this.m_params = param as InputParameters;
            App.IEventBus eventBus = App.Binder.EventBus;
            PlayerView.Binder.InputSystem.PushBackNavigationListener(new System.Action(eventBus.ApplicationQuitRequested));
            this.DualButtons.SetActive(true);
            this.CenterButton.SetActive(false);
            if (this.m_params.Status == LoginStatus.CONFLICT_CHOOSE)
            {
                this.Text.text = _.L(ConfigLoca.PLAYER_PROGRESS_RESTORE_DESCRIPTION, new <>__AnonType19<string>(this.platform), false);
                this.RestoreButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PLAYER_PROGRESS_PROMPT_RESTORE, new <>__AnonType20<int>(this.m_params.OtherLevel), false));
            }
            else if (this.m_params.Status == LoginStatus.CONFLICT_NEW)
            {
                this.Text.text = _.L(ConfigLoca.PLAYER_PROGRESS_NEW_DESCRIPTION, null, false);
                this.RestoreButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PLAYER_PROGRESS_PROMPT_NEW, new <>__AnonType20<int>(this.m_params.OtherLevel), false));
            }
            else if (this.m_params.Status == LoginStatus.CONFLICT_UPDATE)
            {
                this.Text.text = _.L(ConfigLoca.PLAYER_PROGRESS_UPDATE_DESCRIPTION, new <>__AnonType19<string>(this.platform), false);
                this.RestoreButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PLAYER_PROGRESS_PROMPT_UPDATE, new <>__AnonType20<int>(this.m_params.OtherLevel), false));
            }
            this.ContinueButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PLAYER_PROGRESS_PROMPT_CONTINUE, new <>__AnonType20<int>(this.m_params.CurrentLevel), false));
            base.m_contentMenu.setCloseButtonVisibility(false);
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_BUTTON_INFO, null, false)), string.Empty, string.Empty);
        }

        public void onRestoreButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Service.Binder.PlayerService.ResolveLoginConflict(true);
            }
        }

        private void Update()
        {
            if (Service.Binder.PlayerService.HadRestoreError && !this.restoreError)
            {
                this.restoreError = true;
                this.DualButtons.SetActive(false);
                this.CenterButton.SetActive(true);
                this.Text.text = _.L(ConfigLoca.PLAYER_PROGRESS_ERROR_DESCRIPTION, null, false);
                this.OkButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PLAYER_PROGRESS_PROMPT_CONTINUE, new <>__AnonType20<int>(this.m_params.CurrentLevel), false));
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.PlayerProgressRestorePopupContent;
            }
        }

        public class InputParameters
        {
            public int CurrentLevel;
            public int OtherLevel;
            public LoginStatus Status;
        }
    }
}

