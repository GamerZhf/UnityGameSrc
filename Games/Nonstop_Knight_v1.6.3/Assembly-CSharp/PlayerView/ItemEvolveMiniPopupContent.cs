namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine.UI;

    public class ItemEvolveMiniPopupContent : MenuContent
    {
        [CompilerGenerated]
        private GameLogic.ItemInstance <ItemInstance>k__BackingField;
        public Text CornerText;
        public Image Icon;
        public Text PerkEvolveDescription;
        public Text PerkStatChange;

        protected override void onAwake()
        {
        }

        public override bool onBackgroundOverlayClicked()
        {
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
                return false;
            }
            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            return true;
        }

        protected override void onCleanup()
        {
        }

        public void onFuseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                CharacterInstance activeCharacter = player.ActiveCharacter;
                while (player.canEvolveItem(this.ItemInstance))
                {
                    CmdEvolveItem.ExecuteStatic(activeCharacter, this.ItemInstance);
                }
                ItemInfoContent.InputParameters parameters2 = new ItemInfoContent.InputParameters();
                parameters2.ItemInstance = this.ItemInstance;
                ItemInfoContent.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.ItemInfoContent, parameter, 0f, true, true);
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.ItemInstance = (GameLogic.ItemInstance) param;
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", this.ItemInstance.Item.SpriteId);
            int num = activeCharacter.getNumberOfItemInstancesOwnedWithItemId(this.ItemInstance.ItemId) - 1;
            this.CornerText.text = "x" + (num + 1);
            if (this.ItemInstance.Item.FixedPerks.count() > 0)
            {
                this.PerkStatChange.text = "CHANGE ME";
            }
            else
            {
                this.PerkEvolveDescription.text = string.Empty;
                this.PerkStatChange.text = string.Empty;
            }
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("DUPLICATE", string.Empty, string.Empty);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.ItemEvolveMiniPopupContent;
            }
        }

        public GameLogic.ItemInstance ItemInstance
        {
            [CompilerGenerated]
            get
            {
                return this.<ItemInstance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ItemInstance>k__BackingField = value;
            }
        }
    }
}

