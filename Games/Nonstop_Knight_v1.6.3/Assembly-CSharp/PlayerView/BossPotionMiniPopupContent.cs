namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;

    public class BossPotionMiniPopupContent : MenuContent
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
            Player player = GameLogic.Binder.GameState.Player;
            CmdGainPotions.ExecuteStatic(player.ActiveCharacter, PotionType.Boss, -1);
            CmdStartBossTrain.ExecuteStatic(GameLogic.Binder.GameState.ActiveDungeon, player, App.Binder.ConfigMeta.BOSS_POTION_NUM_BOSSES);
            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            return true;
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            MiniPopupMenu contentMenu = (MiniPopupMenu) base.m_contentMenu;
            RewardGalleryCell.Content content2 = new RewardGalleryCell.Content();
            content2.Sprite = new SpriteAtlasEntry("Menu", "icon_bossticket");
            content2.Text = string.Empty;
            RewardGalleryCell.Content rewardContent = content2;
            int num = App.Binder.ConfigMeta.BOSS_POTION_NUM_BOSSES;
            string overrideDescriptionTextLocalized = MenuHelpers.GetFormattedDescription(_.L(ConfigUi.MiniPopupEntries.BOSS_POTION.DescriptionText, null, false), "$Amount$", num);
            contentMenu.populateLayout(ConfigUi.MiniPopupEntries.BOSS_POTION, true, rewardContent, null, null, overrideDescriptionTextLocalized);
        }

        protected override void onRefresh()
        {
        }

        protected void Update()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            MiniPopupMenu contentMenu = (MiniPopupMenu) base.m_contentMenu;
            if ((PlayerView.Binder.TransitionSystem.InCriticalTransition || GameLogic.Binder.FrenzySystem.isFrenzyActive()) || (activeDungeon.ActiveRoom.MainBossSummoned || (activeCharacter.Inventory.BossPotions == 0)))
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
                return MenuContentType.BossPotionMiniPopupContent;
            }
        }
    }
}

