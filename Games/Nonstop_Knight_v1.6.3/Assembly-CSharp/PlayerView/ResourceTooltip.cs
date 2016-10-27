namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine.UI;

    public class ResourceTooltip : MenuContent
    {
        public Text Description;
        private ResourceType m_resourceType;
        public Text Title;

        protected override void onAwake()
        {
        }

        public void onBackgroundClicked()
        {
            ((TooltipMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.TooltipMenu)).onCloseButtonClicked();
        }

        protected override void onCleanup()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_resourceType = (ResourceType) ((int) param);
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            switch (this.m_resourceType)
            {
                case ResourceType.Energy:
                    this.Title.text = "ENERGY";
                    this.Description.text = "Increase max energy by leveling up your hero!";
                    break;

                case ResourceType.Coin:
                    this.Title.text = "COINS";
                    this.Description.text = "Earn more coins by raiding dungeons!";
                    break;
            }
        }

        public override bool CapturesInput
        {
            get
            {
                return false;
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.ResourceTooltip;
            }
        }
    }
}

