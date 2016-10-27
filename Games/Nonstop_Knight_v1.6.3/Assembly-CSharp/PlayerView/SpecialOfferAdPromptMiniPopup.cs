namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class SpecialOfferAdPromptMiniPopup : MenuContent
    {
        private InputParams m_params;

        private void adWatchCompleteCallback(List<Reward> rewards, bool awardReward, int numPurchases)
        {
            if (((rewards != null) && (rewards.Count > 0)) && awardReward)
            {
                PlayerView.Binder.EventBus.SpecialOfferAdWatched(rewards[0]);
                CmdConsumeReward.ExecuteStatic(GameLogic.Binder.GameState.Player, rewards[0], true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(GameLogic.Binder.GameState.Player);
                RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.AD_REWARD.Title, null, false));
                parameters2.Description = _.L(ConfigUi.CeremonyEntries.AD_REWARD.Description, null, false);
                parameters2.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.AD_REWARD.ChestOpenAtStart;
                parameters2.SingleReward = rewards[0];
                RewardCeremonyMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.instantCloseAllMenus();
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, false, true);
            }
            else
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

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
            if (this.m_params.CancelCallback != null)
            {
                PlayerView.Binder.EventBus.SpecialOfferAdRejected(this.m_params.Reward);
                this.m_params.CancelCallback();
                return true;
            }
            return false;
        }

        public override bool onMainButtonClicked()
        {
            AdsData.Category category;
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
                return false;
            }
            if (GameLogic.Binder.GameState.ActiveDungeon.ActiveTournament != null)
            {
                category = AdsData.Category.TOURNAMENT_MYSTERY;
            }
            else
            {
                category = AdsData.Category.ADVENTURE_MYSTERY;
            }
            PlayerView.Binder.EventBus.SpecialOfferAdAccepted(this.m_params.Reward);
            FullscreenAdMenu.InputParameters parameters2 = new FullscreenAdMenu.InputParameters();
            parameters2.AdZoneId = AdsSystem.ADS_DEFAULT_ZONE;
            parameters2.AdCategory = category;
            parameters2.Reward = this.m_params.Reward;
            parameters2.CompleteCallback = new Action<List<Reward>, bool, int>(this.adWatchCompleteCallback);
            FullscreenAdMenu.InputParameters parameter = parameters2;
            PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.FullscreenAdMenu, MenuContentType.NONE, parameter);
            return true;
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_params = (InputParams) param;
            RewardGalleryCell.Content rewardContent = RewardGalleryCell.CreateDefaultContentForReward(this.m_params.Reward, true, null);
            ((MiniPopupMenu) base.m_contentMenu).populateLayout(ConfigUi.MiniPopupEntries.SPECIAL_OFFER, true, rewardContent, null, null, null);
            this.onRefresh();
            PlayerView.Binder.EventBus.SpecialOfferAdOffered(this.m_params.Reward);
        }

        protected override void onRefresh()
        {
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.SpecialOfferAdPromptMiniPopup;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParams
        {
            public GameLogic.Reward Reward;
            public System.Action CancelCallback;
        }
    }
}

