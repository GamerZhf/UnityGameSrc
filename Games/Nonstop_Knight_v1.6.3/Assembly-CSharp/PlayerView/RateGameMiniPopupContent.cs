namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class RateGameMiniPopupContent : MenuContent
    {
        protected override void onAwake()
        {
        }

        public override bool onBackgroundOverlayClicked()
        {
            return this.onCloseButtonClicked();
        }

        protected override void onCleanup()
        {
        }

        public override bool onCloseButtonClicked()
        {
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
                return false;
            }
            Player player = GameLogic.Binder.GameState.Player;
            Service.Binder.TrackingSystem.sendRateGameEvent(player, "decline");
            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            return true;
        }

        public override bool onMainButtonClicked()
        {
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
                return false;
            }
            Player player = GameLogic.Binder.GameState.Player;
            Service.Binder.TrackingSystem.sendRateGameEvent(player, "accept");
            Application.OpenURL(App.Binder.ConfigMeta.STORE_URL_ANDROID);
            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            return true;
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            MiniPopupMenu contentMenu = (MiniPopupMenu) base.m_contentMenu;
            RewardGalleryCell.Content content2 = new RewardGalleryCell.Content();
            content2.Sprite = new SpriteAtlasEntry("Menu", "icon_rategame");
            content2.Text = string.Empty;
            RewardGalleryCell.Content rewardContent = content2;
            contentMenu.populateLayout(ConfigUi.MiniPopupEntries.RATE_GAME, true, rewardContent, null, null, null);
        }

        protected override void onRefresh()
        {
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.RateGameMiniPopupContent;
            }
        }
    }
}

