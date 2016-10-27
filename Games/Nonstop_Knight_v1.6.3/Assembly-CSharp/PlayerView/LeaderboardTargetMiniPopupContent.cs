namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine.UI;

    public class LeaderboardTargetMiniPopupContent : MenuContent
    {
        public Text Description;
        public PlayerView.LeaderboardImage LeaderboardImage;

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
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
            LeaderboardEntry entry = (LeaderboardEntry) param;
            this.LeaderboardImage.refresh(entry.AvatarSpriteId, entry.ImageTexture);
            object[] objArray1 = new object[] { "Reach <color=#", ConfigUi.TEXT_HIGHLIGHT_COLOR_HEX, ">floor ", entry.HighestFloor + 1, "</color> to beat <color=#", ConfigUi.TEXT_HIGHLIGHT_COLOR_HEX, ">", base.name, "</color> on the Leaderboard!" };
            this.Description.text = string.Concat(objArray1);
            base.refresh();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("NEXT TARGET", string.Empty, string.Empty);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.LeaderboardTargetMiniPopupContent;
            }
        }
    }
}

