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

    public class SkillPopupContent : MenuContent
    {
        [CompilerGenerated]
        private List<RunestoneCell> <RunestoneCells>k__BackingField;
        [CompilerGenerated]
        private List<SkillDualCell> <SkillCells>k__BackingField;
        private SkillType m_highlightedSkillType;
        private Vector2 m_originalArrowAnchoredPos;
        private float m_skillInfoOriginalTargetHeight;
        public const int MAX_NUM_VISIBLE_SKILL_UPGRADES = 6;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public AnimatedLayoutElement SkillInfoCellAnimation;
        public RectTransform SkillInfoCellArrowTm;
        public Transform SkillInfoCellTm;
        public Text SkillInfoDescription;
        public RectTransform SkillInfoRunestonesVerticalGroupRectTm;
        public Text SkillsTitle;
        public RectTransform SkillsTitleTm;
        public RectTransform VerticalGroup;

        private void addRunestoneCellToList(RunestoneCell.Content content)
        {
            RunestoneCell item = PlayerView.Binder.RunestoneCellPool.getObject();
            item.transform.SetParent(this.SkillInfoRunestonesVerticalGroupRectTm, false);
            item.initialize(content, new Action<RunestoneCell>(this.onRunestoneCellClicked));
            this.RunestoneCells.Add(item);
            item.gameObject.SetActive(true);
        }

        private void addSkillDualCellVerticalGroup(SkillType skillTypeLeft, SkillType skillTypeRight)
        {
            SkillDualCell item = PlayerView.Binder.SkillDualCellPool.getObject();
            item.RectTm.SetParent(this.VerticalGroup, false);
            item.RectTm.SetSiblingIndex(this.SkillsTitleTm.GetSiblingIndex() + 1);
            bool stripedRow = (this.VerticalGroup.childCount % 2) == 0;
            item.initialize(skillTypeLeft, skillTypeRight, stripedRow, new System.Action(this.onAuxButtonClicked), new Action<SkillType>(this.onSkillCellClicked));
            this.SkillCells.Add(item);
            item.gameObject.SetActive(true);
        }

        private void cleanupCells()
        {
            for (int i = this.SkillCells.Count - 1; i >= 0; i--)
            {
                SkillDualCell item = this.SkillCells[i];
                this.SkillCells.Remove(item);
                PlayerView.Binder.SkillDualCellPool.returnObject(item);
            }
            for (int j = this.RunestoneCells.Count - 1; j >= 0; j--)
            {
                RunestoneCell cell2 = this.RunestoneCells[j];
                this.RunestoneCells.Remove(cell2);
                PlayerView.Binder.RunestoneCellPool.returnObject(cell2);
            }
        }

        private void closeHighlightedSkillInfo()
        {
            if (this.m_highlightedSkillType != SkillType.NONE)
            {
                this.SkillInfoCellAnimation.collapse(new System.Action(this.onSkillInfoCollapseComplete), false);
                this.m_highlightedSkillType = SkillType.NONE;
            }
        }

        public SkillDualCell.CellData getSkillCell(SkillType skillType)
        {
            for (int i = 0; i < this.SkillCells.Count; i++)
            {
                if (this.SkillCells[i].LeftCell.SkillType == skillType)
                {
                    return this.SkillCells[i].LeftCell;
                }
                if (this.SkillCells[i].RightCell.SkillType == skillType)
                {
                    return this.SkillCells[i].RightCell;
                }
            }
            return null;
        }

        public SkillDualCell getSkillDualCell(SkillType skillType)
        {
            for (int i = 0; i < this.SkillCells.Count; i++)
            {
                if (this.SkillCells[i].LeftCell.SkillType == skillType)
                {
                    return this.SkillCells[i];
                }
                if (this.SkillCells[i].RightCell.SkillType == skillType)
                {
                    return this.SkillCells[i];
                }
            }
            return null;
        }

        public void onAuxButtonClicked()
        {
            if (this.m_highlightedSkillType != SkillType.NONE)
            {
                this.closeHighlightedSkillInfo();
            }
        }

        protected override void onAwake()
        {
            this.m_skillInfoOriginalTargetHeight = this.SkillInfoCellAnimation.LayoutElement.minHeight;
            this.m_originalArrowAnchoredPos = this.SkillInfoCellArrowTm.anchoredPosition;
            this.SkillsTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.SKILLS, null, false));
            this.SkillCells = new List<SkillDualCell>(ConfigSkills.SkillGroupCount);
            this.SkillInfoCellTm.gameObject.SetActive(true);
            this.SkillInfoCellAnimation.collapse(new System.Action(this.onSkillInfoCollapseComplete), true);
            this.RunestoneCells = new List<RunestoneCell>();
        }

        protected override void onCleanup()
        {
            this.cleanupCells();
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnRunestoneSelected -= new GameLogic.Events.RunestoneSelected(this.onRunestoneSelected);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnRunestoneSelected += new GameLogic.Events.RunestoneSelected(this.onRunestoneSelected);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.reconstructContent();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_SKILLS_BUTTON_TEXT, null, false)), string.Empty, string.Empty);
            for (int i = 0; i < this.SkillCells.Count; i++)
            {
                bool leftHighlighted = this.m_highlightedSkillType == this.SkillCells[i].LeftCell.SkillType;
                bool rightHighlighted = this.m_highlightedSkillType == this.SkillCells[i].RightCell.SkillType;
                this.SkillCells[i].refresh(leftHighlighted, rightHighlighted);
            }
        }

        private void onRunestoneCellClicked(RunestoneCell cell)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (cell.ActiveContent.Obj is string)
            {
                string runestoneId = (string) cell.ActiveContent.Obj;
                RunestoneInstance runestoneInstance = player.Runestones.getRunestoneInstance(runestoneId);
                SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(runestoneId);
                if ((runestoneInstance != null) && player.hasUnlockedSkill(skillTypeForRunestone))
                {
                    bool flag = player.Runestones.isRunestoneSelected(runestoneId);
                    if (!runestoneInstance.InspectedByPlayer)
                    {
                        CmdInspectRunestone.ExecuteStatic(runestoneInstance);
                        PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_SkillEquip, (float) 0f);
                        cell.refresh(cell.ActiveContent.Selected, cell.ActiveContent.Highlighted, cell.ActiveContent.Interactable, cell.ActiveContent.Grayscale, false, cell.ActiveContent.Description, cell.ActiveContent.SelectionSource);
                    }
                    else
                    {
                        PlayerView.Binder.AudioSystem.playSfx(!flag ? AudioSourceType.SfxUi_RunestoneSelect : AudioSourceType.SfxUi_ButtonTab, (float) 0f);
                    }
                    if (!flag)
                    {
                        CmdSelectRunestone.ExecuteStatic(player, skillTypeForRunestone, RunestoneSelectionSource.Player, runestoneInstance);
                    }
                }
                else
                {
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ButtonTab, (float) 0f);
                }
            }
            else if (cell.ActiveContent.Obj is SkillType)
            {
                SkillType skillType = (SkillType) ((int) cell.ActiveContent.Obj);
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_RunestoneSelect, (float) 0f);
                CmdSelectRunestone.ExecuteStatic(player, skillType, RunestoneSelectionSource.Player, null);
            }
            for (int i = 0; i < this.RunestoneCells.Count; i++)
            {
                RunestoneCell cell2 = this.RunestoneCells[i];
                cell2.refresh(cell2.ActiveContent.Selected, cell2 == cell, cell2.ActiveContent.Interactable, cell2.ActiveContent.Grayscale, cell2.ActiveContent.Notify, cell2.ActiveContent.Description, cell2.ActiveContent.SelectionSource);
            }
            this.refreshDescription();
        }

        private void onRunestoneSelected(Player player, RunestoneInstance runestone)
        {
            this.refreshRunestones();
            this.onRefresh();
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator17F iteratorf = new <onShow>c__Iterator17F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        private void onSkillCellClicked(SkillType skillType)
        {
            if (this.m_highlightedSkillType == skillType)
            {
                this.closeHighlightedSkillInfo();
            }
            else
            {
                this.openSkillInfo(skillType);
            }
            this.onRefresh();
        }

        private void onSkillInfoCollapseComplete()
        {
            this.SkillInfoCellArrowTm.gameObject.SetActive(false);
        }

        private void onSkillInfoExpandStart()
        {
            if (this.m_highlightedSkillType != SkillType.NONE)
            {
                Player player = GameLogic.Binder.GameState.Player;
                int group = ConfigSkills.SHARED_DATA[this.m_highlightedSkillType].Group;
                this.SkillInfoCellTm.SetSiblingIndex(group + 2);
                for (int i = this.RunestoneCells.Count - 1; i >= 0; i--)
                {
                    RunestoneCell item = this.RunestoneCells[i];
                    this.RunestoneCells.Remove(item);
                    PlayerView.Binder.RunestoneCellPool.returnObject(item);
                }
                bool flag = player.hasUnlockedSkill(this.m_highlightedSkillType);
                bool flag2 = player.Runestones.isBasicRunestoneSelected(this.m_highlightedSkillType);
                string str = !flag ? _.L(ConfigLoca.SKILL_POPUP_UNLOCKS_AT_LEVEL_SHORT, new <>__AnonType8<int>(ConfigSkills.SHARED_DATA[this.m_highlightedSkillType].UnlockRank), false) : StringExtensions.ToUpperLoca(_.L(ConfigLoca.RUNESTONE_NAME_BASIC, null, false));
                RunestoneCell.Content content = new RunestoneCell.Content();
                content.Obj = this.m_highlightedSkillType;
                content.Description = str;
                content.IconSprite = ConfigRunestones.BASIC_RUNESTONE_SPRITES[this.m_highlightedSkillType];
                content.Selected = flag && flag2;
                content.Highlighted = flag2;
                content.Interactable = true;
                content.Grayscale = !flag;
                this.addRunestoneCellToList(content);
                for (int j = 0; j < ConfigRunestones.RUNESTONES.Length; j++)
                {
                    ConfigRunestones.SharedData data = ConfigRunestones.RUNESTONES[j];
                    if (data.LinkedToSkill == this.m_highlightedSkillType)
                    {
                        RunestoneInstance instance = player.Runestones.getRunestoneInstance(data.Id);
                        bool flag3 = instance == null;
                        bool flag4 = player.Runestones.isRunestoneSelected(data.Id);
                        bool flag5 = player.Runestones.isRunestoneSelected(data.Id, RunestoneSelectionSource.Player);
                        bool flag6 = (instance != null) && !instance.InspectedByPlayer;
                        if (flag3)
                        {
                            str = _.L(ConfigLoca.SKILL_POPUP_UNLOCKS_AT_LEVEL_SHORT, new <>__AnonType8<int>(data.UnlockRank), false);
                        }
                        else
                        {
                            str = StringExtensions.ToUpperLoca(_.L(ConfigRunestones.GetShortDescription(data.Id), null, false));
                        }
                        content = new RunestoneCell.Content();
                        content.Obj = data.Id;
                        content.RunestoneId = data.Id;
                        content.Description = str;
                        content.IconSprite = data.Sprite;
                        content.Selected = flag4;
                        content.Highlighted = flag5;
                        content.Interactable = true;
                        content.Grayscale = flag3;
                        content.Notify = flag6;
                        this.addRunestoneCellToList(content);
                    }
                }
                this.refreshRunestones();
                this.SkillInfoCellArrowTm.gameObject.SetActive(true);
                this.refreshArrowPosition();
            }
        }

        private void openSkillInfo(SkillType type)
        {
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            this.m_highlightedSkillType = type;
            SkillInstance skillInstance = activeCharacter.getSkillInstance(this.m_highlightedSkillType);
            if (skillInstance != null)
            {
                if (!skillInstance.InspectedByPlayer)
                {
                    CmdInspectSkill.ExecuteStatic(skillInstance);
                }
                if (!activeCharacter.isSkillActive(this.m_highlightedSkillType))
                {
                    CmdAssignActiveSkill.ExecuteStatic(activeCharacter, this.m_highlightedSkillType);
                }
            }
            PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ButtonTab, (float) 0f);
            if (this.SkillInfoCellAnimation.isCollapsed())
            {
                this.SkillInfoCellAnimation.expand(this.m_skillInfoOriginalTargetHeight, new System.Action(this.onSkillInfoExpandStart), false);
            }
            else if (ConfigSkills.SHARED_DATA[this.m_highlightedSkillType].Group != (this.SkillInfoCellTm.GetSiblingIndex() - 2))
            {
                this.SkillInfoCellAnimation.collapseAndExpand(this.m_skillInfoOriginalTargetHeight, new System.Action(this.onSkillInfoCollapseComplete), new System.Action(this.onSkillInfoExpandStart), false);
            }
            else
            {
                this.SkillInfoCellAnimation.collapseAndExpand(this.m_skillInfoOriginalTargetHeight, new System.Action(this.onSkillInfoCollapseComplete), new System.Action(this.onSkillInfoExpandStart), true);
            }
        }

        private void reconstructContent()
        {
            this.cleanupCells();
            for (int i = ConfigSkills.ACTIVE_HERO_SKILLS_SORTED_BY_GROUP.Count - 1; i >= 0; i -= 2)
            {
                this.addSkillDualCellVerticalGroup(ConfigSkills.ACTIVE_HERO_SKILLS_SORTED_BY_GROUP[i - 1], ConfigSkills.ACTIVE_HERO_SKILLS_SORTED_BY_GROUP[i]);
            }
            if (this.m_highlightedSkillType != SkillType.NONE)
            {
                this.SkillInfoCellAnimation.expand(this.m_skillInfoOriginalTargetHeight, new System.Action(this.onSkillInfoExpandStart), true);
            }
            this.onRefresh();
        }

        private void refreshArrowPosition()
        {
            if (this.m_highlightedSkillType != SkillType.NONE)
            {
                this.SkillInfoCellArrowTm.anchoredPosition = this.m_originalArrowAnchoredPos;
                Canvas.ForceUpdateCanvases();
                SkillDualCell.CellData data = this.getSkillCell(this.m_highlightedSkillType);
                Vector2 vector = RectTransformUtility.WorldToScreenPoint(PlayerView.Binder.MenuSystem.MenuCamera, this.SkillInfoCellArrowTm.position);
                Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(PlayerView.Binder.MenuSystem.MenuCamera, data.Icon.rectTransform.position);
                Vector2 vector3 = new Vector2(vector2.x, vector.y);
                Vector3 vector4 = PlayerView.Binder.MenuSystem.MenuCamera.ScreenToWorldPoint((Vector3) vector3);
                vector4.z = 0f;
                this.SkillInfoCellArrowTm.position = vector4;
            }
        }

        private void refreshDescription()
        {
            if (this.m_highlightedSkillType != SkillType.NONE)
            {
                CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
                for (int i = 0; i < this.RunestoneCells.Count; i++)
                {
                    RunestoneCell cell = this.RunestoneCells[i];
                    if (cell.ActiveContent.Highlighted)
                    {
                        if (cell.ActiveContent.Obj is string)
                        {
                            string runestoneId = (string) cell.ActiveContent.Obj;
                            ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(runestoneId);
                            if (this.m_highlightedSkillType == ConfigRunestones.GetSkillTypeForRunestone(runestoneId))
                            {
                                this.SkillInfoDescription.text = MenuHelpers.GetFormattedSkillDescription(_.L(ConfigRunestones.GetDescription(runestoneId), null, false), activeCharacter, this.m_highlightedSkillType, runestoneData.PerkInstance.Type, false, true);
                            }
                        }
                        else if ((cell.ActiveContent.Obj is SkillType) && (this.m_highlightedSkillType == ((int) cell.ActiveContent.Obj)))
                        {
                            this.SkillInfoDescription.text = MenuHelpers.GetFormattedSkillDescription(_.L(ConfigSkills.SHARED_DATA[this.m_highlightedSkillType].Description, null, false), activeCharacter, this.m_highlightedSkillType, PerkType.NONE, false, true);
                        }
                    }
                }
            }
        }

        private void refreshRunestones()
        {
            if (this.m_highlightedSkillType != SkillType.NONE)
            {
                Player player = GameLogic.Binder.GameState.Player;
                bool flag = player.hasUnlockedSkill(this.m_highlightedSkillType);
                for (int i = 0; i < this.RunestoneCells.Count; i++)
                {
                    RunestoneCell cell = this.RunestoneCells[i];
                    bool selected = false;
                    bool highlighted = false;
                    RunestoneSelectionSource selectionSource = player.Runestones.getRunestoneSelectionSource(cell.ActiveContent.RunestoneId);
                    if (cell.ActiveContent.Obj is string)
                    {
                        string runestoneId = (string) cell.ActiveContent.Obj;
                        selected = flag && player.Runestones.isRunestoneSelected(runestoneId);
                        highlighted = selectionSource == RunestoneSelectionSource.Player;
                    }
                    else if (cell.ActiveContent.Obj is SkillType)
                    {
                        bool flag4 = player.Runestones.isBasicRunestoneSelected(this.m_highlightedSkillType);
                        selected = flag && flag4;
                        highlighted = flag4;
                    }
                    else if (selectionSource == RunestoneSelectionSource.Pet)
                    {
                        selected = true;
                    }
                    cell.refresh(selected, highlighted, cell.ActiveContent.Interactable, cell.ActiveContent.Grayscale, cell.ActiveContent.Notify, cell.ActiveContent.Description, selectionSource);
                }
                this.refreshDescription();
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.SkillPopupContent;
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

        public List<SkillDualCell> SkillCells
        {
            [CompilerGenerated]
            get
            {
                return this.<SkillCells>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SkillCells>k__BackingField = value;
            }
        }

        public override MenuContent.TabSpriteParameters TabSprite
        {
            get
            {
                MenuContent.TabSpriteParameters parameters = new MenuContent.TabSpriteParameters();
                parameters.SpriteAtlasId = "DungeonHud";
                parameters.SpriteId = "sprite_icon_skillbook";
                return parameters;
            }
        }

        public override bool UnlockNotificationActive
        {
            get
            {
                return (GameLogic.Binder.GameState.Player.Notifiers.getNumberOfGoldSkillNotifications() > 0);
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator17F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SkillPopupContent <>f__this;

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
                    this.<>f__this.refreshArrowPosition();
                    this.<>f__this.ScrollRect.verticalNormalizedPosition = 1f;
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
    }
}

