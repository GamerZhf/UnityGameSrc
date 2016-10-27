namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillDualCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public Image Bg;
        public CellData LeftCell = new CellData();
        private System.Action m_auxButtonClickCallback;
        private Action<SkillType> m_skillCellClickCallback;
        public CellData RightCell = new CellData();

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(SkillType skillLeft, SkillType skillRight, bool stripedRow, System.Action auxButtonClickCallback, Action<SkillType> skillCellClickCallback)
        {
            this.LeftCell.SkillType = skillLeft;
            this.RightCell.SkillType = skillRight;
            if (stripedRow)
            {
                this.Bg.enabled = true;
                this.Bg.color = ConfigUi.LIST_CELL_STRIPED_COLOR;
            }
            else
            {
                this.Bg.enabled = false;
            }
            this.m_auxButtonClickCallback = auxButtonClickCallback;
            this.m_skillCellClickCallback = skillCellClickCallback;
            this.reconstruct();
        }

        public void onAuxButtonClicked()
        {
            this.m_auxButtonClickCallback();
        }

        public void onLeftButtonClicked()
        {
            this.m_skillCellClickCallback(this.LeftCell.SkillType);
        }

        public void onRightButtonClicked()
        {
            this.m_skillCellClickCallback(this.RightCell.SkillType);
        }

        public void reconstruct()
        {
            this.reconstructCell(this.LeftCell);
            this.reconstructCell(this.RightCell);
            this.refresh(false, false);
        }

        public void reconstructCell(CellData cell)
        {
            ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[cell.SkillType];
            cell.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(data.Spritesheet, data.Sprite);
            cell.Name.text = StringExtensions.ToUpperLoca(_.L(data.Name, null, false));
            cell.Name.color = Color.white;
        }

        public void refresh(bool leftHighlighted, bool rightHighlighted)
        {
            this.refreshCell(this.LeftCell, leftHighlighted);
            this.refreshCell(this.RightCell, rightHighlighted);
        }

        public void refreshCell(CellData cell, bool highlighted)
        {
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[cell.SkillType];
            List<RunestoneSelection> list = player.Runestones.getRunestoneSelectionsForSkillType(cell.SkillType, RunestoneSelectionSource.None);
            if (list.Count == 0)
            {
                cell.Header.text = _.L(ConfigLoca.RUNESTONE_NAME_BASIC, null, false);
            }
            else
            {
                cell.Header.text = string.Empty;
                for (int i = 0; i < list.Count; i++)
                {
                    cell.Header.text = cell.Header.text + _.L(ConfigRunestones.GetShortDescription(list[i].Id), null, false);
                    if (i < (list.Count - 1))
                    {
                        cell.Header.text = cell.Header.text + " + ";
                    }
                }
            }
            bool flag = player.Runestones.getRunestoneSelectionsForSkillType(cell.SkillType, RunestoneSelectionSource.Pet).Count > 0;
            bool flag2 = player.hasUnlockedSkill(cell.SkillType);
            cell.Button.interactable = true;
            if (activeCharacter.isSkillActive(cell.SkillType))
            {
                cell.SelectedBorder.gameObject.SetActive(true);
                cell.SelectedBorder.color = ConfigUi.COLOR_HIGHLIGHTED;
                cell.TextAlphaGroup.alpha = 1f;
                cell.Icon.material = null;
                cell.IconBG.material = null;
                cell.LockIcon.SetActive(false);
                cell.LockText.text = string.Empty;
                cell.Icon.color = ConfigUi.COLOR_HIGHLIGHTED;
                cell.IconBG.color = ConfigUi.COLOR_HIGHLIGHTED;
            }
            else
            {
                cell.SelectedBorder.gameObject.SetActive(false);
                if (flag2)
                {
                    cell.TextAlphaGroup.alpha = 0.5f;
                    cell.Icon.material = null;
                    cell.IconBG.material = null;
                    cell.Icon.color = !highlighted ? ConfigUi.COLOR_DEHIGHLIGHTED_UNLOCKED : ConfigUi.COLOR_HIGHLIGHTED;
                    cell.IconBG.color = !highlighted ? ConfigUi.COLOR_DEHIGHLIGHTED_UNLOCKED : ConfigUi.COLOR_HIGHLIGHTED;
                    cell.LockIcon.SetActive(false);
                    cell.LockText.text = string.Empty;
                }
                else
                {
                    cell.TextAlphaGroup.alpha = 0.3f;
                    cell.Icon.material = PlayerView.Binder.DisabledUiMaterial;
                    cell.IconBG.material = PlayerView.Binder.DisabledUiMaterial;
                    cell.Icon.color = ConfigUi.COLOR_DEHIGHLIGHTED_LOCKED;
                    cell.IconBG.color = ConfigUi.COLOR_DEHIGHLIGHTED_LOCKED;
                    cell.LockIcon.SetActive(true);
                    cell.LockText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.SKILL_POPUP_UNLOCKS_AT_LEVEL_SHORT, new <>__AnonType8<int>(data.UnlockRank), false));
                }
            }
            cell.UnlockNotifier.SetActive(player.Notifiers.isSkillGoldNotificationActive(cell.SkillType));
            cell.PetSourceIconRoot.SetActive(flag && flag2);
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

        [Serializable]
        public class CellData
        {
            [CompilerGenerated]
            private GameLogic.SkillType <SkillType>k__BackingField;
            public UnityEngine.UI.Button Button;
            public Text Header;
            public Image Icon;
            public Image IconBG;
            public GameObject LockIcon;
            public Text LockText;
            public Text Name;
            public GameObject PetSourceIconRoot;
            public Image SelectedBorder;
            public CanvasGroup TextAlphaGroup;
            public GameObject UnlockNotifier;

            public GameLogic.SkillType SkillType
            {
                [CompilerGenerated]
                get
                {
                    return this.<SkillType>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<SkillType>k__BackingField = value;
                }
            }
        }
    }
}

