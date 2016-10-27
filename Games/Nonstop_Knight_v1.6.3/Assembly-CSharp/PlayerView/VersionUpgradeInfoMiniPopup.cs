namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class VersionUpgradeInfoMiniPopup : MenuContent
    {
        public GameObject Button;
        public UnityEngine.UI.Text Text;

        protected override void onAwake()
        {
        }

        public void onButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected override void onCleanup()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (string.IsNullOrEmpty(player.UnshownVersionUpgradeNotification))
            {
                Debug.LogError("No upgrade info text to show player.");
            }
            base.m_contentMenu.setCloseButtonVisibility(false);
            this.Button.gameObject.SetActive(true);
            this.Text.text = player.UnshownVersionUpgradeNotification;
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("GAME UPDATE!", string.Empty, string.Empty);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.VersionUpgradeInfoMiniPopup;
            }
        }
    }
}

