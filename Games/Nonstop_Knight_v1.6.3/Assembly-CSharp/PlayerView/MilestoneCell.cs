namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class MilestoneCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private RunestoneMilestone <UpgradeMilestone>k__BackingField;
        public Image Background;
        public UnityEngine.CanvasGroup CanvasGroup;
        public Text Description;
        public Image Icon;
        public Text LevelReq;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(string upgradeId, RunestoneMilestone upgradeMilestone, int levelRequirement, bool stripedRow, bool isUnlocked)
        {
            this.UpgradeMilestone = upgradeMilestone;
            this.LevelReq.text = "Rank " + levelRequirement;
            this.Icon.enabled = true;
            if (this.UpgradeMilestone.Perk != null)
            {
                PerkInstance perk = this.UpgradeMilestone.Perk;
                PerkType perkType = perk.Type;
                ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[perkType];
                this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", data.SmallSprite);
                this.Description.text = MenuHelpers.GetFormattedPerkDescription(perkType, perk.Modifier, data.DurationSeconds, data.Threshold, 0f, true);
            }
            else
            {
                this.Icon.sprite = null;
                this.Description.text = "CHANGE ME";
            }
            this.Background.color = !stripedRow ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            if (isUnlocked)
            {
                this.CanvasGroup.alpha = 1f;
                this.Icon.material = null;
            }
            else
            {
                this.CanvasGroup.alpha = 0.5f;
                this.Icon.material = PlayerView.Binder.DisabledUiMaterial;
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

        public RunestoneMilestone UpgradeMilestone
        {
            [CompilerGenerated]
            get
            {
                return this.<UpgradeMilestone>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<UpgradeMilestone>k__BackingField = value;
            }
        }
    }
}

