namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine.UI;

    public class NonWeaponItemTooltip : MenuContent
    {
        [CompilerGenerated]
        private GameLogic.Item <Item>k__BackingField;
        public Text Name;

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
            this.Item = (GameLogic.Item) param;
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            if (this.Item != null)
            {
                this.Name.text = StringExtensions.ToUpperLoca(this.Item.Name);
            }
            else
            {
                this.Name.text = "RANDOM ITEM";
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
                return MenuContentType.NonWeaponItemTooltip;
            }
        }

        public GameLogic.Item Item
        {
            [CompilerGenerated]
            get
            {
                return this.<Item>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Item>k__BackingField = value;
            }
        }
    }
}

