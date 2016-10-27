namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;

    public class FrenzyMiniPopupContent : MenuContent
    {
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

        public override bool onMainButtonClicked()
        {
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
                return false;
            }
            CmdGainPotions.ExecuteStatic(GameLogic.Binder.GameState.Player.ActiveCharacter, PotionType.Frenzy, -1);
            GameLogic.Binder.FrenzySystem.activateFrenzy();
            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            return true;
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            MiniPopupMenu contentMenu = (MiniPopupMenu) base.m_contentMenu;
            RewardGalleryCell.Content content2 = new RewardGalleryCell.Content();
            content2.Sprite = new SpriteAtlasEntry("Menu", "icon_bottle_frenzy");
            content2.Text = string.Empty;
            RewardGalleryCell.Content rewardContent = content2;
            contentMenu.populateLayout(ConfigUi.MiniPopupEntries.FRENZY, true, rewardContent, null, null, null);
        }

        protected override void onRefresh()
        {
        }

        protected void Update()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            MiniPopupMenu contentMenu = (MiniPopupMenu) base.m_contentMenu;
            if ((PlayerView.Binder.TransitionSystem.InCriticalTransition || GameLogic.Binder.FrenzySystem.isFrenzyActive()) || (activeDungeon.ActiveRoom.MainBossSummoned || (activeCharacter.Inventory.FrenzyPotions == 0)))
            {
                contentMenu.MainButton.Button.interactable = false;
            }
            else if (activeDungeon.CurrentGameplayState == GameplayState.ACTION)
            {
                contentMenu.MainButton.Button.interactable = true;
            }
            else
            {
                contentMenu.MainButton.Button.interactable = false;
            }
            contentMenu.MainButton.Bg.material = !contentMenu.MainButton.Button.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.FrenzyMiniPopupContent;
            }
        }
    }
}

