namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine.UI;

    public class PauseMiniPopupContent : MenuContent
    {
        private bool m_quitButtonClicked;
        public Button QuitButton;

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
            if (!this.m_quitButtonClicked)
            {
                GameLogic.Binder.TimeSystem.pause(false);
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_quitButtonClicked = false;
            this.QuitButton.interactable = !GameLogic.Binder.GameState.ActiveDungeon.isTutorialDungeon();
            GameLogic.Binder.TimeSystem.pause(true);
            this.onRefresh();
        }

        public void onQuitButtonClicked()
        {
            GameLogic.Binder.CommandProcessor.execute(new CmdEndGameplay(GameLogic.Binder.GameState.ActiveDungeon, false), 0f);
            this.QuitButton.interactable = false;
            this.m_quitButtonClicked = true;
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("PAUSED", string.Empty, string.Empty);
        }

        public void onResumeButtonClicked()
        {
            GameLogic.Binder.TimeSystem.pause(false);
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.NONE, MenuContentType.NONE, null, 0f, false, true);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.PauseMiniPopup;
            }
        }
    }
}

