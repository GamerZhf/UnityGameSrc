namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class TalentPopupContent : MenuContent
    {
        private List<SkillDualCell> m_skillCells = new List<SkillDualCell>();
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform VerticalGroup;

        private void addSkillDualCellToList(SkillType skillTypeLeft, SkillType skillTypeRight)
        {
            SkillDualCell item = PlayerView.Binder.SkillDualCellPool.getObject();
            item.transform.SetParent(this.VerticalGroup, false);
            bool stripedRow = (this.VerticalGroup.childCount % 2) == 1;
            item.initialize(skillTypeLeft, skillTypeRight, stripedRow, null, null);
            this.m_skillCells.Add(item);
            item.gameObject.SetActive(true);
        }

        protected override void onAwake()
        {
        }

        private void onCharacterSkillsChanged(CharacterInstance character)
        {
            base.refresh();
        }

        protected override void onCleanup()
        {
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnCharacterSkillsChanged -= new GameLogic.Events.CharacterSkillsChanged(this.onCharacterSkillsChanged);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnCharacterSkillsChanged += new GameLogic.Events.CharacterSkillsChanged(this.onCharacterSkillsChanged);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.ScrollRect.verticalNormalizedPosition = 1f;
            this.onRefresh();
        }

        protected override void onPreWarm([Optional, DefaultParameterValue(null)] object param)
        {
            for (int i = 0; i < ConfigSkills.PASSIVE_HERO_SKILLS_SORTED_BY_GROUP.Count; i += 2)
            {
                this.addSkillDualCellToList(ConfigSkills.PASSIVE_HERO_SKILLS_SORTED_BY_GROUP[i], ConfigSkills.PASSIVE_HERO_SKILLS_SORTED_BY_GROUP[i + 1]);
            }
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTabs();
            base.m_contentMenu.refreshTitle("TALENTS", string.Empty, string.Empty);
            for (int i = 0; i < this.m_skillCells.Count; i++)
            {
                this.m_skillCells[i].refresh(false, false);
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.TalentPopupContent;
            }
        }

        public override MenuContent.TabSpriteParameters TabSprite
        {
            get
            {
                MenuContent.TabSpriteParameters parameters = new MenuContent.TabSpriteParameters();
                parameters.SpriteAtlasId = "Menu";
                parameters.SpriteId = "sprite_icon_talentbook";
                return parameters;
            }
        }

        public override bool UnlockNotificationActive
        {
            get
            {
                return false;
            }
        }
    }
}

