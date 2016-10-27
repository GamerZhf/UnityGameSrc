namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine.UI;

    public class LevelTooltip : MenuContent
    {
        public Text Description1;
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
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.Title.text = "PLAYER LEVEL " + player.Rank;
            double num = GameLogic.Binder.LevelUpRules.getProgressTowardsNextLevel(player.Rank, player.getResourceAmount(ResourceType.Xp));
            double num2 = GameLogic.Binder.LevelUpRules.getNeededXpForTargetLevel(player.Rank, player.Rank + 1);
            object[] objArray1 = new object[] { "Experience to next level:\n", num, " / ", num2 };
            this.Description1.text = string.Concat(objArray1);
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
                return MenuContentType.LevelTooltip;
            }
        }
    }
}

