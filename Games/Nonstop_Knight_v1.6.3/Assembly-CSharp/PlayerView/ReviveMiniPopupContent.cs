namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ReviveMiniPopupContent : MenuContent
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache1;
        [CompilerGenerated]
        private bool <ViewOnlyMode>k__BackingField;

        protected override void onAwake()
        {
        }

        public override bool onBackgroundOverlayClicked()
        {
            if (this.ViewOnlyMode)
            {
                this.onCloseButtonClicked();
            }
            return true;
        }

        protected override void onCleanup()
        {
            PlayerView.Binder.InputSystem.PopBackNavigationListener();
        }

        public override bool onCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                if (this.ViewOnlyMode)
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
                else
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                    PlayerView.Binder.TransitionSystem.enqueueDungeonReload();
                }
            }
            return true;
        }

        public override bool onMainButtonClicked()
        {
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
                return false;
            }
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            if ((player.ActiveCharacter.Inventory.RevivePotions > 0) || (activeDungeon.getCurrentRevivePrice() <= player.getResourceAmount(ResourceType.Diamond)))
            {
                PlayerView.Binder.TransitionSystem.enqueueRevivePrimaryPlayerCharacter(ConfigGameplay.DEFAULT_NORMALIZED_HP_AFTER_REVIVE, false);
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
            else
            {
                MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                parameters2.PathToShop = PathToShopType.Revive;
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate {
                        PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.MiniPopupMenu, MenuContentType.ReviveMiniPopup, null);
                    };
                }
                parameters2.CloseCallback = <>f__am$cache1;
                parameters2.PurchaseCallback = new Action<ShopEntry, PurchaseResult>(this.onShopPurchaseCompleted);
                MiniPopupMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.ShopOfferMiniPopupContent, parameter, 0f, true, true);
            }
            return true;
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            if (param != null)
            {
                this.ViewOnlyMode = (bool) param;
            }
            else
            {
                this.ViewOnlyMode = false;
            }
            ((MiniPopupMenu) base.m_contentMenu).MainButton.gameObject.SetActive(!this.ViewOnlyMode);
            this.onRefresh();
            PlayerView.Binder.InputSystem.PushBackNavigationListener(delegate {
                this.onCloseButtonClicked();
            });
        }

        protected override void onRefresh()
        {
            string str;
            MiniPopupMenu contentMenu = (MiniPopupMenu) base.m_contentMenu;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            RewardGalleryCell.Content content2 = new RewardGalleryCell.Content();
            content2.Sprite = new SpriteAtlasEntry("Menu", "icon_bottle_red");
            content2.Text = string.Empty;
            RewardGalleryCell.Content rewardContent = content2;
            Sprite overrideButtonSprite = null;
            if (player.ActiveCharacter.Inventory.RevivePotions > 0)
            {
                str = StringExtensions.ToUpperLoca(_.L(ConfigLoca.REVIVE_AVAILABLE_FREE_REVIVES, new <>__AnonType4<int>(player.ActiveCharacter.Inventory.RevivePotions), false));
            }
            else
            {
                str = MenuHelpers.BigValueToString(activeDungeon.getCurrentRevivePrice());
                overrideButtonSprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Diamond]);
            }
            contentMenu.populateLayout(ConfigUi.MiniPopupEntries.REVIVE, true, rewardContent, str, overrideButtonSprite, null);
        }

        private void onShopPurchaseCompleted(ShopEntry shopEntry, PurchaseResult purchaseResult)
        {
            if (purchaseResult == PurchaseResult.AskToBuy)
            {
                PlayerView.Binder.TransitionSystem.enqueueRevivePrimaryPlayerCharacter(ConfigGameplay.DEFAULT_NORMALIZED_HP_AFTER_REVIVE, true);
            }
            else
            {
                PlayerView.Binder.MenuSystem.waitAndTransitionToNewMenu(MenuType.MiniPopupMenu, MenuContentType.ReviveMiniPopup, null);
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.ReviveMiniPopup;
            }
        }

        public bool ViewOnlyMode
        {
            [CompilerGenerated]
            get
            {
                return this.<ViewOnlyMode>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ViewOnlyMode>k__BackingField = value;
            }
        }
    }
}

