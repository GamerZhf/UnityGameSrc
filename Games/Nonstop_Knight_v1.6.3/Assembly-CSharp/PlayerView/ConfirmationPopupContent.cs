namespace PlayerView
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine.UI;

    public class ConfirmationPopupContent : MenuContent
    {
        public Text DescriptionText;
        public Text LeftButtonText;
        private InputParameters m_inputParameters;
        private bool m_tutorialCircleActive;
        public Text RightButtonText;
        public Text Title;

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
            Binder.InputSystem.PopBackNavigationListener();
            if (this.m_tutorialCircleActive)
            {
                Binder.DungeonHud.TutorialCircle.enableEmission = true;
            }
            TechPopupMenu contentMenu = base.m_contentMenu as TechPopupMenu;
            if (contentMenu != null)
            {
                contentMenu.TitleText.transform.parent.gameObject.SetActive(true);
            }
        }

        public void onLeftButtonClicked()
        {
            if (!Binder.MenuSystem.InTransition)
            {
                Binder.MenuSystem.returnToPreviousMenu(true);
                if (this.m_inputParameters.LeftButtonCallback != null)
                {
                    this.m_inputParameters.LeftButtonCallback();
                }
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_inputParameters = (InputParameters) param;
            if (this.m_inputParameters.NavigateBackEqualsToLeftButton)
            {
                Binder.InputSystem.PushBackNavigationListener(new System.Action(this.onLeftButtonClicked));
            }
            else
            {
                Binder.InputSystem.PushBackNavigationListener(new System.Action(this.onRightButtonClicked));
            }
            if (this.m_inputParameters.DisableCloseButton)
            {
                base.m_contentMenu.setCloseButtonVisibility(false);
            }
            this.Title.text = StringExtensions.ToUpperLoca(this.m_inputParameters.TitleText);
            this.DescriptionText.text = this.m_inputParameters.DescriptionText;
            this.LeftButtonText.text = StringExtensions.ToUpperLoca(this.m_inputParameters.LeftButtonText);
            this.RightButtonText.text = StringExtensions.ToUpperLoca(this.m_inputParameters.RightButtonText);
            if (Binder.DungeonHud.TutorialCircle != null)
            {
                this.m_tutorialCircleActive = Binder.DungeonHud.TutorialCircle.enableEmission;
                if (this.m_tutorialCircleActive)
                {
                    Binder.DungeonHud.TutorialCircle.enableEmission = false;
                    Binder.DungeonHud.TutorialCircle.Clear();
                }
            }
            TechPopupMenu contentMenu = base.m_contentMenu as TechPopupMenu;
            if (contentMenu != null)
            {
                contentMenu.TitleText.transform.parent.gameObject.SetActive(false);
            }
        }

        protected override void onRefresh()
        {
        }

        public void onRightButtonClicked()
        {
            if (!Binder.MenuSystem.InTransition)
            {
                Binder.MenuSystem.returnToPreviousMenu(true);
                if (this.m_inputParameters.RightButtonCallback != null)
                {
                    this.m_inputParameters.RightButtonCallback();
                }
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.ConfirmationPopupContent;
            }
        }

        public override bool PauseGame
        {
            get
            {
                return !this.m_inputParameters.SkipPauseGame;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public string TitleText;
            public string DescriptionText;
            public string LeftButtonText;
            public string RightButtonText;
            public System.Action LeftButtonCallback;
            public System.Action RightButtonCallback;
            public bool NavigateBackEqualsToLeftButton;
            public bool DisableCloseButton;
            public bool SkipPauseGame;
        }
    }
}

