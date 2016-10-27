namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class RunestoneInfoContent : MenuContent
    {
        [CompilerGenerated]
        private string <RunestoneId>k__BackingField;
        public Text HeaderTitle;
        public Image Icon;
        public Text LevelText;
        private List<Vector3> m_originalStarLocalPositions = new List<Vector3>();
        public PrettyButton MainButton;
        public ParticleSystem ParticleEffect;
        public IconWithText Perk;
        public AnimatedProgressBar ProgressBar;
        public Text ProgressBarText;
        public List<CellButton> SlotButtons;
        public GameObject SlotsRoot;
        public List<Image> Stars = new List<Image>();
        public Text Title;

        public void onAssignToSlotButtonClicked(int slotIdx)
        {
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
            }
        }

        protected override void onAwake()
        {
            this.HeaderTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.RUNESTONE, null, false));
            for (int i = 0; i < this.Stars.Count; i++)
            {
                this.m_originalStarLocalPositions.Add(this.Stars[i].transform.localPosition);
            }
        }

        protected override void onCleanup()
        {
        }

        public void onCustomCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnRunestoneUnlocked -= new GameLogic.Events.RunestoneUnlocked(this.onRunestoneUnlocked);
            GameLogic.Binder.EventBus.OnRunestoneLevelUpped -= new GameLogic.Events.RunestoneLevelUpped(this.onRunestoneLevelUpped);
            GameLogic.Binder.EventBus.OnRunestoneRankUpped -= new GameLogic.Events.RunestoneRankUpped(this.onRunestoneRankUpped);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnRunestoneUnlocked += new GameLogic.Events.RunestoneUnlocked(this.onRunestoneUnlocked);
            GameLogic.Binder.EventBus.OnRunestoneLevelUpped += new GameLogic.Events.RunestoneLevelUpped(this.onRunestoneLevelUpped);
            GameLogic.Binder.EventBus.OnRunestoneRankUpped += new GameLogic.Events.RunestoneRankUpped(this.onRunestoneRankUpped);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.RunestoneId = (string) param;
            Player player = GameLogic.Binder.GameState.Player;
            ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(this.RunestoneId);
            RunestoneInstance runestoneInstance = player.Runestones.getRunestoneInstance(this.RunestoneId);
            if (runestoneInstance != null)
            {
                CmdInspectRunestone.ExecuteStatic(runestoneInstance);
            }
            this.Title.text = "CHANGE ME";
            this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(runestoneData.Sprite);
            if (!ConfigMeta.RUNESTONE_DUPLICATES_AUTO_CONVERTED_INTO_TOKENS && player.Runestones.ownsRunestone(this.RunestoneId))
            {
                this.LevelText.enabled = true;
                this.ProgressBar.gameObject.SetActive(true);
                this.SlotsRoot.SetActive(true);
                this.MainButton.gameObject.SetActive(true);
            }
            else
            {
                this.LevelText.enabled = false;
                this.ProgressBar.gameObject.SetActive(false);
                this.SlotsRoot.SetActive(false);
                this.MainButton.gameObject.SetActive(false);
            }
            this.onRefresh();
        }

        public void onPrimaryButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            RunestoneInstance runestone = player.Runestones.getRunestoneInstance(this.RunestoneId);
            if (runestone != null)
            {
                if (!runestone.Unlocked)
                {
                    CmdUnlockRunestone.ExecuteStatic(player, runestone);
                }
                else if (runestone.canEvolve())
                {
                    CmdEvolveRunestone.ExecuteStatic(player, runestone);
                }
            }
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            RunestoneInstance instance = null;
            ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(this.RunestoneId);
            MenuHelpers.RefreshStarContainer(this.Stars, this.m_originalStarLocalPositions, runestoneData.Rarity, false);
            if (!ConfigMeta.RUNESTONE_DUPLICATES_AUTO_CONVERTED_INTO_TOKENS)
            {
                if (player.Runestones.ownsRunestone(this.RunestoneId))
                {
                    instance = player.Runestones.getRunestoneInstance(this.RunestoneId);
                    if (instance.Unlocked)
                    {
                        this.LevelText.text = "Level " + instance.Level;
                        this.MainButton.Text.text = "LEVEL UP";
                        this.MainButton.Button.interactable = instance.canEvolve();
                        this.ProgressBar.gameObject.SetActive(true);
                        this.ProgressBar.setNormalizedValue(instance.getNormalizedProgressToNextEvolve());
                        this.ProgressBarText.text = instance.getCompletedRankUpsForNextEvolve() + "/" + instance.getRequiredRankUpsForNextEvolve();
                        int num = 0;
                        for (int i = 0; i < this.SlotButtons.Count; i++)
                        {
                            CellButton button = this.SlotButtons[i];
                            if (i == num)
                            {
                                button.ButtonScript.interactable = false;
                            }
                            else
                            {
                                button.ButtonScript.interactable = true;
                            }
                            button.ButtonImage.material = !button.ButtonScript.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
                        }
                    }
                    else
                    {
                        this.LevelText.text = string.Empty;
                        this.ProgressBar.gameObject.SetActive(false);
                        this.MainButton.Text.text = "UNLOCK";
                        this.MainButton.Button.interactable = true;
                    }
                }
                this.MainButton.Bg.material = !this.MainButton.Button.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
            }
            PerkInstance perkInstance = ConfigRunestones.GetRunestoneData(this.RunestoneId).PerkInstance;
            ConfigPerks.SharedData data2 = ConfigPerks.SHARED_DATA[perkInstance.Type];
            this.Perk.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", data2.SmallSprite);
            if (instance != null)
            {
                this.Perk.Text.text = MenuHelpers.GetFormattedPlayerUpgradePerkDescription(perkInstance, instance.getEvolveModifierBonus(), true);
            }
            else
            {
                this.Perk.Text.text = MenuHelpers.GetFormattedPlayerUpgradePerkDescription(perkInstance, 0f, true);
            }
        }

        private void onRunestoneLevelUpped(Player player, RunestoneInstance runestone)
        {
            if (!this.ParticleEffect.isPlaying)
            {
                this.ParticleEffect.Play();
            }
            this.onRefresh();
        }

        private void onRunestoneRankUpped(Player player, RunestoneInstance runestone)
        {
            if (runestone.Id == this.RunestoneId)
            {
                if (!this.ParticleEffect.isPlaying)
                {
                    this.ParticleEffect.Play();
                }
                this.onRefresh();
            }
        }

        private void onRunestoneUnlocked(Player player, RunestoneInstance runestone)
        {
            if (!this.ParticleEffect.isPlaying)
            {
                this.ParticleEffect.Play();
            }
            this.onRefresh();
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.RunestoneInfoContent;
            }
        }

        public string RunestoneId
        {
            [CompilerGenerated]
            get
            {
                return this.<RunestoneId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RunestoneId>k__BackingField = value;
            }
        }
    }
}

