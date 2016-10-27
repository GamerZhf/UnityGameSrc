namespace PlayerView
{
    using System;
    using System.Runtime.InteropServices;

    public class DailyDungeonMiniPopupContent : MenuContent
    {
        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("DAILY DUNGEONS", string.Empty, string.Empty);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.DailyDungeonMiniPopupContent;
            }
        }
    }
}

