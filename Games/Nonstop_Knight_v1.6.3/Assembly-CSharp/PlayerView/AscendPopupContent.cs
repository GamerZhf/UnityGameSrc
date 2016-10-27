namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class AscendPopupContent : MenuContent
    {
        public Button AscendButton;
        public Image AscendButtonImage;
        public Text AscendButtonText;
        public RectTransform AscendButtonTm;
        public Text AscendingDescription;
        public Text FrenzyPotionsText;
        public Image LockedIcon;
        public Text LockedText;
        public Text RewardBoxesText;
        public Text Subtitle;
        public Text Title;
        public Text TokensFromBosses;
        public Text TokensFromEliteBosses;
        public Text TokensFromItems;
        public Text TokensMultiplier;
        public Text TokensTotal;
        public Text TokensTotalTitle;
        public RectTransform TooltipIconTm;

        public void onAscendButtonClicked()
        {
            PlayerView.Binder.TransitionSystem.retire();
        }

        protected override void onAwake()
        {
            this.Title.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ASCEND_POPUP_TITLE, null, false));
            this.Subtitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ASCEND_TITLE_ADVENTURE, null, false));
            this.TokensTotalTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ASCEND_TITLE_REWARDS, null, false));
            this.AscendingDescription.text = _.L(ConfigLoca.ASCEND_DESCRIPTION, null, false);
            this.AscendButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_ASCEND, null, false));
        }

        public void onCancelButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected override void onCleanup()
        {
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.onRefresh();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            GameLogic.Binder.GameState.Player.Notifiers.HeroRetirementsInspected = true;
            this.AscendButton.interactable = false;
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            double num;
            double num2;
            double num3;
            Player player = GameLogic.Binder.GameState.Player;
            this.TokensFromBosses.text = _.L(ConfigLoca.ASCEND_FROM_BOSSES, new <>__AnonType9<int>(player.ActiveCharacter.HeroStats.BossesBeat), false);
            this.TokensFromEliteBosses.text = _.L(ConfigLoca.ASCEND_FROM_ELITE_BOSSES, new <>__AnonType9<int>(player.ActiveCharacter.HeroStats.EliteBossesBeat), false);
            this.TokensFromItems.text = _.L(ConfigLoca.ASCEND_FROM_ITEMS, new <>__AnonType9<int>(LangUtil.TryGetIntValueFromDictionary<int>(player.ActiveCharacter.HeroStats.ItemsGainedByRarity, ConfigMeta.ITEM_HIGHEST_RARITY)), false);
            double v = player.getTotalTokensFromRetirement(out num, out num2, out num3);
            this.TokensMultiplier.text = _.L(ConfigLoca.ASCEND_MULTIPLIER, new <>__AnonType9<string>(num3.ToString("0.0") + "X"), false);
            this.TokensTotal.text = "\x00d7" + MenuHelpers.BigValueToString(v);
            int num5 = 0;
            for (int i = 0; i < player.UnclaimedRewards.Count; i++)
            {
                if (player.UnclaimedRewards[i].ChestType == ChestType.RewardBoxCommon)
                {
                    num5++;
                }
            }
            this.RewardBoxesText.text = "\x00d7" + num5.ToString();
            Reward reward = player.getFirstUnclaimedRetirementTriggerChest();
            if (reward != null)
            {
                this.FrenzyPotionsText.text = "\x00d7" + reward.FrenzyPotions;
            }
            else
            {
                this.FrenzyPotionsText.text = "0";
            }
        }

        public void onTooltipButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                int tokenMultiplierEveryXthFloor = 0;
                if (App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS.Count >= 2)
                {
                    tokenMultiplierEveryXthFloor = Mathf.RoundToInt(((float) (App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS[1].Key - App.Binder.ConfigMeta.TOKEN_REWARD_FLOOR_MULTIPLIERS[0].Key)) / 3f);
                }
                int rewardBoxEveryXthFloor = Mathf.RoundToInt((1f / ((1f / ((float) App.Binder.ConfigMeta.REWARD_BOX_BONUS_EVERY_X_FLOOR)) + (1f / ((float) App.Binder.ConfigMeta.FRENZY_BONUS_POTION_EVERY_X_FLOOR)))) / 3f);
                TooltipMenu.InputParameters parameters2 = new TooltipMenu.InputParameters();
                parameters2.CenterOnTm = this.TooltipIconTm;
                parameters2.MenuContentParams = _.L(ConfigLoca.ASCEND_TOOLTIP, new <>__AnonType18<int, int>(tokenMultiplierEveryXthFloor, rewardBoxEveryXthFloor), false);
                TooltipMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.InfoTooltip, parameter, 0f, false, true);
            }
        }

        protected void Update()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            int num = player.getLastCompletedFloor(false) + 1;
            int floor = player.getGatedRetirementMinFloor();
            if (num < floor)
            {
                this.AscendButton.interactable = false;
                this.AscendButtonText.gameObject.SetActive(false);
                this.LockedIcon.gameObject.SetActive(true);
                this.LockedText.gameObject.SetActive(true);
                this.LockedText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ASCEND_UNLOCKED_AT_FLOOR, new <>__AnonType15<int>(floor), false));
            }
            else
            {
                if (!player.hasCompletedTutorial("TUT351A"))
                {
                    this.AscendButton.interactable = false;
                }
                else if (PlayerView.Binder.TransitionSystem.InCriticalTransition)
                {
                    if ((activeDungeon.CurrentGameplayState == GameplayState.START_CEREMONY_STEP1) || (activeDungeon.CurrentGameplayState == GameplayState.START_CEREMONY_STEP2))
                    {
                        this.AscendButton.interactable = true;
                    }
                    else
                    {
                        this.AscendButton.interactable = false;
                    }
                }
                else if (activeDungeon.CurrentGameplayState == GameplayState.ACTION)
                {
                    this.AscendButton.interactable = true;
                }
                else
                {
                    this.AscendButton.interactable = false;
                }
                this.AscendButtonText.gameObject.SetActive(true);
                this.LockedIcon.gameObject.SetActive(false);
                this.LockedText.gameObject.SetActive(false);
            }
            this.AscendButtonImage.material = !this.AscendButton.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.AscendPopupContent;
            }
        }
    }
}

