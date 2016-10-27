namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillInfoContent : MenuContent
    {
        [CompilerGenerated]
        private List<RunestoneCell> <RunestoneCells>k__BackingField;
        public Text CooldownText;
        public Text CooldownTitle;
        public Text DamageText;
        public Text DamageTitle;
        public Text Description;
        public Button EquipButton;
        public Image EquipButtonImage;
        public Text EquipButtonText;
        public Image Icon;
        public Image IconSelectedBorder;
        private InputParameters m_inputParameters;
        public ScrollRect RunestonesScrollRect;
        public Text RunestonesTitle;
        public GameObject RunestonesTitleRoot;
        public RectTransform RunestonesVerticalGroupRectTm;
        public Text Title;

        private void addRunestoneCellToList(RunestoneCell.Content content)
        {
            RunestoneCell item = PlayerView.Binder.RunestoneCellPool.getObject();
            item.transform.SetParent(this.RunestonesVerticalGroupRectTm, false);
            item.initialize(content, new Action<RunestoneCell>(this.onRunestoneCellClicked));
            this.RunestoneCells.Add(item);
            item.gameObject.SetActive(true);
        }

        private void cleanupCells()
        {
            for (int i = this.RunestoneCells.Count - 1; i >= 0; i--)
            {
                RunestoneCell item = this.RunestoneCells[i];
                this.RunestoneCells.Remove(item);
                PlayerView.Binder.RunestoneCellPool.returnObject(item);
            }
        }

        protected override void onAwake()
        {
            this.RunestonesTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.RUNESTONES, null, false));
            this.CooldownTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.SKILL_POPUP_COOLDOWN, null, false));
            this.DamageTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.SKILL_POPUP_DAMAGE, null, false));
            this.RunestoneCells = new List<RunestoneCell>();
        }

        protected override void onCleanup()
        {
            this.cleanupCells();
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnPlayerSkillUpgradeGained -= new GameLogic.Events.PlayerSkillUpgradeGained(this.onPlayerSkillUpgradeGained);
            GameLogic.Binder.EventBus.OnRunestoneSelected -= new GameLogic.Events.RunestoneSelected(this.onRunestoneSelected);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnPlayerSkillUpgradeGained += new GameLogic.Events.PlayerSkillUpgradeGained(this.onPlayerSkillUpgradeGained);
            GameLogic.Binder.EventBus.OnRunestoneSelected += new GameLogic.Events.RunestoneSelected(this.onRunestoneSelected);
        }

        public void onEquipButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                CmdAssignActiveSkill.ExecuteStatic(GameLogic.Binder.GameState.Player.ActiveCharacter, this.m_inputParameters.SkillInstance.SkillType);
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_SkillEquip, (float) 0f);
                if ((base.m_contentMenu.MenuType == MenuType.StackedPopupMenu) && ((StackedPopupMenu) base.m_contentMenu).Smcc.canPopContent())
                {
                    ((StackedPopupMenu) base.m_contentMenu).Smcc.popContent(false);
                }
                else
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
            }
        }

        private void onPlayerSkillUpgradeGained(Player player, string id)
        {
            this.reconstructContent();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_inputParameters = (InputParameters) param;
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            SkillInstance skillInstance = this.m_inputParameters.SkillInstance;
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.SKILL, null, false)), string.Empty, string.Empty);
            ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[skillInstance.SkillType];
            this.Title.text = StringExtensions.ToUpperLoca(_.L(data.Name, null, false));
            this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(data.Spritesheet, data.Sprite);
            if (!skillInstance.InspectedByPlayer)
            {
                CmdInspectSkill.ExecuteStatic(skillInstance);
            }
            if (!activeCharacter.isSkillActive(skillInstance.SkillType))
            {
                CmdAssignActiveSkill.ExecuteStatic(activeCharacter, skillInstance.SkillType);
            }
            this.EquipButton.gameObject.SetActive(false);
            this.EquipButtonImage.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_button_selected");
            this.IconSelectedBorder.enabled = activeCharacter.isSkillActive(skillInstance.SkillType);
            this.reconstructContent();
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            SkillInstance skillInstance = this.m_inputParameters.SkillInstance;
            string runestoneId = player.Runestones.getSelectedRunestoneId(skillInstance.SkillType, RunestoneSelectionSource.Player);
            ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(runestoneId);
            if (runestoneId != null)
            {
                ConfigPerks.SharedData data2 = ConfigPerks.SHARED_DATA[ConfigRunestones.GetRunestoneData(runestoneId).PerkInstance.Type];
                this.Description.text = MenuHelpers.GetFormattedSkillDescription(_.L(data2.Description, null, false), activeCharacter, skillInstance.SkillType, PerkType.NONE, false, true);
            }
            else
            {
                this.Description.text = MenuHelpers.GetFormattedSkillDescription(_.L(ConfigSkills.SHARED_DATA[skillInstance.SkillType].Description, null, false), activeCharacter, skillInstance.SkillType, runestoneData.PerkInstance.Type, false, true);
            }
            this.DamageText.text = "CHANGE ME";
            this.CooldownText.text = activeCharacter.getSkillCooldown(skillInstance.SkillType).ToString("0") + " " + _.L(ConfigLoca.UNIT_SECONDS_SHORT, null, false);
            if (activeCharacter.isSkillActive(skillInstance.SkillType))
            {
                this.EquipButton.interactable = false;
                this.EquipButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_EQUIPPED, null, false));
            }
            else
            {
                this.EquipButton.interactable = true;
                this.EquipButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_EQUIP, null, false));
            }
            this.EquipButtonImage.material = !this.EquipButton.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
            for (int i = 0; i < this.RunestoneCells.Count; i++)
            {
                RunestoneCell cell = this.RunestoneCells[i];
                bool selected = false;
                if (cell.ActiveContent.Obj != null)
                {
                    RunestoneInstance instance3 = (RunestoneInstance) cell.ActiveContent.Obj;
                    selected = player.Runestones.isRunestoneSelected(instance3.Id);
                }
                cell.refresh(selected, selected, cell.ActiveContent.Interactable, cell.ActiveContent.Grayscale, cell.ActiveContent.Notify, cell.ActiveContent.Description, cell.ActiveContent.SelectionSource);
            }
        }

        private void onRunestoneCellClicked(RunestoneCell cell)
        {
            Player player = GameLogic.Binder.GameState.Player;
            RunestoneInstance runestoneInstance = (RunestoneInstance) cell.ActiveContent.Obj;
            if (!runestoneInstance.InspectedByPlayer)
            {
                CmdInspectRunestone.ExecuteStatic(runestoneInstance);
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_SkillEquip, (float) 0f);
                cell.refresh(cell.ActiveContent.Selected, cell.ActiveContent.Selected, cell.ActiveContent.Interactable, cell.ActiveContent.Grayscale, false, cell.ActiveContent.Description, cell.ActiveContent.SelectionSource);
            }
            else
            {
                PlayerView.Binder.AudioSystem.playSfx(!player.Runestones.isRunestoneSelected(runestoneInstance.Id) ? AudioSourceType.SfxUi_RunestoneSelect : AudioSourceType.SfxUi_ButtonTab, (float) 0f);
            }
            SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(runestoneInstance.Id);
            CmdSelectRunestone.ExecuteStatic(player, skillTypeForRunestone, RunestoneSelectionSource.Player, runestoneInstance);
        }

        private void onRunestoneSelected(Player player, RunestoneInstance runestone)
        {
            this.onRefresh();
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator17C iteratorc = new <onShow>c__Iterator17C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        private void reconstructContent()
        {
            this.cleanupCells();
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = 0; i < ConfigRunestones.RUNESTONES.Length; i++)
            {
                ConfigRunestones.SharedData data = ConfigRunestones.RUNESTONES[i];
                PerkType key = data.PerkInstance.Type;
                if (!ConfigPerks.SHARED_DATA.ContainsKey(key))
                {
                    UnityEngine.Debug.LogWarning("Config for runestone perk " + key + " not found");
                }
                else
                {
                    ConfigPerks.SharedData data2 = ConfigPerks.SHARED_DATA[key];
                    if (data2.LinkedToSkill == this.m_inputParameters.SkillInstance.SkillType)
                    {
                        RunestoneInstance instance = player.Runestones.getRunestoneInstance(data.Id);
                        bool flag = instance == null;
                        bool flag2 = player.Runestones.isRunestoneSelected(data.Id);
                        bool flag3 = (instance != null) && !instance.InspectedByPlayer;
                        string str = string.Empty;
                        if (flag)
                        {
                            str = _.L(ConfigLoca.SKILL_POPUP_UNLOCKS_AT_LEVEL_SHORT, new <>__AnonType8<int>(data.UnlockRank), false);
                        }
                        else
                        {
                            str = StringExtensions.ToUpperLoca(_.L(data2.ShortDescription, null, false));
                        }
                        RunestoneCell.Content content = new RunestoneCell.Content();
                        content.Obj = instance;
                        content.Description = str;
                        content.IconSprite = data.Sprite;
                        content.Selected = flag2;
                        content.Highlighted = flag2;
                        content.Interactable = !flag;
                        content.Grayscale = flag;
                        content.Notify = flag3;
                        content.SelectionSource = RunestoneSelectionSource.Player;
                        this.addRunestoneCellToList(content);
                    }
                }
            }
            this.RunestonesTitleRoot.SetActive(this.RunestoneCells.Count > 0);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.SkillInfoContent;
            }
        }

        public List<RunestoneCell> RunestoneCells
        {
            [CompilerGenerated]
            get
            {
                return this.<RunestoneCells>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RunestoneCells>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator17C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SkillInfoContent <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_Popup, (float) 0f);
                    this.<>f__this.RunestonesScrollRect.verticalNormalizedPosition = 1f;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public GameLogic.SkillInstance SkillInstance;
        }
    }
}

