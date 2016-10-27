namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class AchievementCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private string <AchievementId>k__BackingField;
        [CompilerGenerated]
        private int <AchievementTier>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private bool <Visible>k__BackingField;
        public CanvasGroup AlphaGroup;
        public Image Bg;
        public PulsatingGraphic ButtonPulsatingGraphic;
        public PlayerView.CellButton CellButton;
        public Text Description;
        public Image GoldBorders;
        public UnityEngine.UI.LayoutElement LayoutElement;
        public AnimatedProgressBar ProgressBar;
        public Image ProgressBarFg;
        public Text ProgressText;
        public Image RewardResourceIcon;
        public Text RewardResourceText;
        public List<Image> Stars;
        public Text Title;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.ButtonPulsatingGraphic.enabled = false;
        }

        public void cleanUpForReuse()
        {
        }

        public string getFormattedDescription()
        {
            ConfigAchievements.SharedData data = ConfigAchievements.SHARED_DATA[this.AchievementId];
            return _.L(data.Description, new <>__AnonType9<string>(MenuHelpers.BigValueToString(data.TierRequirements[this.AchievementTier])), false);
        }

        public void initialize(string achievementId, int achievementTier, bool stripedRow)
        {
            this.AchievementId = achievementId;
            this.AchievementTier = achievementTier;
            this.Bg.color = !stripedRow ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            ConfigAchievements.SharedData data = ConfigAchievements.SHARED_DATA[this.AchievementId];
            this.Title.text = StringExtensions.ToUpperLoca(_.L(data.Title, null, false));
            this.refresh(false, achievementTier);
        }

        public void onButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                double amount = CmdClaimAchievement.ExecuteStatic(GameLogic.Binder.GameState.Player, this.AchievementId, this.AchievementTier, false);
                Vector2 sourceScreenPos = RectTransformUtility.WorldToScreenPoint(PlayerView.Binder.MenuSystem.MenuCamera, this.CellButton.RectTm.position);
                PlayerView.Binder.DungeonHud.flyToHudDiamondGain(amount, sourceScreenPos, true);
            }
        }

        public void refresh(bool visible, int achievementTier)
        {
            this.Visible = visible;
            if (!this.Visible)
            {
                this.ButtonPulsatingGraphic.enabled = false;
            }
            else
            {
                double num3;
                double num4;
                this.AchievementTier = achievementTier;
                Player player = GameLogic.Binder.GameState.Player;
                ConfigAchievements.SharedData data = ConfigAchievements.SHARED_DATA[this.AchievementId];
                double v = App.Binder.ConfigMeta.ACHIEVEMENT_TIER_DIAMOND_REWARDS[this.AchievementTier];
                this.RewardResourceText.text = MenuHelpers.BigValueToString(v);
                for (int i = 0; i < this.Stars.Count; i++)
                {
                    Image image = this.Stars[i];
                    image.enabled = true;
                    image.color = !player.Achievements.isClaimed(this.AchievementId, i + 1) ? new Color(0.1686275f, 0.1686275f, 0.1686275f, 0.8392157f) : Color.white;
                }
                float num5 = data.Progress(player, this.AchievementId, this.AchievementTier, out num3, out num4);
                if (num5 > 0f)
                {
                    this.ProgressBar.setNormalizedValue(Mathf.Clamp(num5, 0.1f, 1f));
                }
                else
                {
                    this.ProgressBar.setNormalizedValue(num5);
                }
                if (player.Achievements.isAtMaxTier(this.AchievementId) && player.Achievements.isClaimed(this.AchievementId, this.AchievementTier))
                {
                    this.ButtonPulsatingGraphic.enabled = false;
                    this.RewardResourceIcon.gameObject.SetActive(false);
                    this.RewardResourceText.gameObject.SetActive(false);
                    this.ProgressText.text = string.Empty;
                    this.Description.text = _.L(ConfigLoca.ACHIEVEMENT_COMPLETED, null, false);
                    this.GoldBorders.gameObject.SetActive(true);
                    this.ProgressBarFg.sprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_ui_progressbar_front_gold");
                    this.ProgressBarFg.color = new Color(1f, 0.8784314f, 0.4901961f, 1f);
                    this.CellButton.setCellButtonStyle(CellButtonType.SelectedDisabled, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_DONE, null, false)));
                }
                else
                {
                    this.Description.text = this.getFormattedDescription();
                    this.GoldBorders.gameObject.SetActive(false);
                    this.ProgressBarFg.sprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_ui_progressbar_front_blue");
                    this.ProgressBarFg.color = Color.white;
                    if (data.BooleanProgress)
                    {
                        this.ProgressText.text = string.Empty;
                    }
                    else
                    {
                        this.ProgressText.text = MenuHelpers.BigValueToString(num3) + " / " + MenuHelpers.BigValueToString(num4);
                    }
                    this.RewardResourceIcon.gameObject.SetActive(true);
                    this.RewardResourceText.gameObject.SetActive(true);
                    if (player.Achievements.canComplete(this.AchievementId, this.AchievementTier))
                    {
                        this.ButtonPulsatingGraphic.enabled = true;
                        this.CellButton.setCellButtonStyle(CellButtonType.Unlock, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_CLAIM, null, false)));
                    }
                    else
                    {
                        this.ButtonPulsatingGraphic.enabled = false;
                        this.CellButton.setCellButtonStyle(CellButtonType.UnlockDisabled, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_CLAIM, null, false)));
                    }
                }
            }
        }

        public string AchievementId
        {
            [CompilerGenerated]
            get
            {
                return this.<AchievementId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AchievementId>k__BackingField = value;
            }
        }

        public int AchievementTier
        {
            [CompilerGenerated]
            get
            {
                return this.<AchievementTier>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AchievementTier>k__BackingField = value;
            }
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }

        public bool Visible
        {
            [CompilerGenerated]
            get
            {
                return this.<Visible>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Visible>k__BackingField = value;
            }
        }
    }
}

