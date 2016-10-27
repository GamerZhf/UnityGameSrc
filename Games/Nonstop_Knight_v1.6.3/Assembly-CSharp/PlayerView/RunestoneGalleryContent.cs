namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class RunestoneGalleryContent : MenuContent
    {
        public RectTransform BottomGridTm;
        private List<RunestoneRowCell> m_gridRowCells = new List<RunestoneRowCell>(ConfigSkills.ACTIVE_HERO_SKILLS.Count);
        private List<Card> m_rewardGalleryCards = new List<Card>();
        private List<IconWithText> m_skillTitleCells = new List<IconWithText>(ConfigSkills.ACTIVE_HERO_SKILLS.Count);
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform TopGridTm;
        public RectTransform VerticalGroup;

        private RunestoneRowCell addGridRowCellToVerticalGroup(int rowIdx)
        {
            RunestoneRowCell cell = this.m_gridRowCells[rowIdx];
            bool flag = (rowIdx % 2) != 0;
            if (flag)
            {
                cell.Background.enabled = true;
                cell.Background.color = !flag ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            }
            else
            {
                cell.Background.enabled = false;
            }
            cell.transform.SetParent(this.VerticalGroup, false);
            cell.gameObject.SetActive(true);
            return cell;
        }

        private void addRunestoneToGrid(Card.Content content, Transform parentTm)
        {
            Card item = PlayerView.Binder.CardRewardPool.getObject();
            item.transform.SetParent(parentTm, false);
            item.initialize(content, new Action<Card>(this.onCardClicked));
            this.m_rewardGalleryCards.Add(item);
            item.gameObject.SetActive(true);
        }

        private void addSkillTitleCellToGrid(int rowIdx, Transform parentTm)
        {
            IconWithText text = this.m_skillTitleCells[rowIdx];
            SkillType nONE = SkillType.NONE;
            switch (rowIdx)
            {
                case 0:
                    nONE = SkillType.Whirlwind;
                    break;

                case 1:
                    nONE = SkillType.Leap;
                    break;

                case 2:
                    nONE = SkillType.Clone;
                    break;

                case 3:
                    nONE = SkillType.Slam;
                    break;

                case 4:
                    nONE = SkillType.Omnislash;
                    break;

                case 5:
                    nONE = SkillType.Implosion;
                    break;
            }
            ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[nONE];
            text.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(data.Spritesheet, data.Sprite);
            text.Text.text = StringExtensions.ToUpperLoca(_.L(data.Name, null, false));
            text.transform.SetParent(parentTm, false);
            text.gameObject.SetActive(true);
        }

        private void cleanupCells()
        {
            for (int i = 0; i < this.m_gridRowCells.Count; i++)
            {
                RunestoneRowCell cell = this.m_gridRowCells[i];
                cell.transform.SetParent(base.RectTm, false);
                cell.gameObject.SetActive(false);
            }
            for (int j = 0; j < this.m_skillTitleCells.Count; j++)
            {
                IconWithText text = this.m_skillTitleCells[j];
                text.transform.SetParent(base.RectTm, false);
                text.gameObject.SetActive(false);
            }
            for (int k = this.m_rewardGalleryCards.Count - 1; k >= 0; k--)
            {
                Card item = this.m_rewardGalleryCards[k];
                this.m_rewardGalleryCards.Remove(item);
                PlayerView.Binder.CardRewardPool.returnObject(item);
            }
        }

        protected override void onAwake()
        {
            GameObject original = Resources.Load<GameObject>("Prefabs/Menu/RunestoneRowCell");
            for (int i = 0; i < ConfigSkills.ACTIVE_HERO_SKILLS.Count; i++)
            {
                GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(original);
                obj3.transform.SetParent(base.transform);
                obj3.SetActive(false);
                this.m_gridRowCells.Add(obj3.GetComponent<RunestoneRowCell>());
            }
            GameObject obj4 = Resources.Load<GameObject>("Prefabs/Menu/RunestoneGallerySkillTitleCell");
            for (int j = 0; j < ConfigSkills.ACTIVE_HERO_SKILLS.Count; j++)
            {
                GameObject obj5 = UnityEngine.Object.Instantiate<GameObject>(obj4);
                obj5.transform.SetParent(base.transform);
                obj5.SetActive(false);
                this.m_skillTitleCells.Add(obj5.GetComponent<IconWithText>());
            }
        }

        private void onCardClicked(Card card)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.RunestoneInfoContent, card.ActiveContent.Id, 0f, false, true);
            }
        }

        protected override void onCleanup()
        {
            this.cleanupCells();
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnRunestoneGained -= new GameLogic.Events.RunestoneGained(this.onRunestoneGained);
            GameLogic.Binder.EventBus.OnRunestoneRankUpped -= new GameLogic.Events.RunestoneRankUpped(this.onRunestoneRankUpped);
            GameLogic.Binder.EventBus.OnRunestoneLevelUpped -= new GameLogic.Events.RunestoneLevelUpped(this.onRunestoneLevelUppped);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnRunestoneGained += new GameLogic.Events.RunestoneGained(this.onRunestoneGained);
            GameLogic.Binder.EventBus.OnRunestoneRankUpped += new GameLogic.Events.RunestoneRankUpped(this.onRunestoneRankUpped);
            GameLogic.Binder.EventBus.OnRunestoneLevelUpped += new GameLogic.Events.RunestoneLevelUpped(this.onRunestoneLevelUppped);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.reconstructContent();
            this.ScrollRect.verticalNormalizedPosition = 1f;
        }

        protected override void onRefresh()
        {
        }

        private void onRunestoneGained(Player player, RunestoneInstance runestone, bool cheated)
        {
            this.reconstructContent();
        }

        private void onRunestoneLevelUppped(Player player, RunestoneInstance runestone)
        {
            this.reconstructContent();
        }

        private void onRunestoneRankUpped(Player player, RunestoneInstance runestone)
        {
            this.reconstructContent();
        }

        private void reconstructContent()
        {
            Player player = GameLogic.Binder.GameState.Player;
            base.m_contentMenu.refreshTitle("SKILL RUNES", string.Empty, string.Empty);
            this.cleanupCells();
            int num = 0;
            int rowIdx = 0;
            Transform parentTm = null;
            int index = 0;
            while (index < ConfigRunestones.RUNESTONES.Length)
            {
                if (num == 0)
                {
                    parentTm = this.addGridRowCellToVerticalGroup(rowIdx).GridRoot.transform;
                    this.addSkillTitleCellToGrid(rowIdx, parentTm);
                    rowIdx++;
                }
                else
                {
                    Card.Content content2;
                    ConfigRunestones.SharedData data = ConfigRunestones.RUNESTONES[index];
                    string id = data.Id;
                    Card.Content content = null;
                    if (player.Runestones.ownsRunestone(id))
                    {
                        content2 = new Card.Content();
                        content2.Id = id;
                        content2.Sprite = data.Sprite;
                        content2.Rarity = data.Rarity;
                        content2.Interactable = true;
                        content = content2;
                    }
                    else
                    {
                        content2 = new Card.Content();
                        content2.Id = id;
                        content2.Sprite = data.Sprite;
                        content2.Rarity = data.Rarity;
                        content2.Interactable = true;
                        content2.Grayscale = true;
                        content = content2;
                    }
                    this.addRunestoneToGrid(content, parentTm);
                    index++;
                }
                num = (num + 1) % 4;
            }
        }

        private static int RunestoneInstanceCompareByOwnership(string x, string y)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player.Runestones.ownsRunestone(x))
            {
                return -1;
            }
            if (player.Runestones.ownsRunestone(y))
            {
                return 1;
            }
            return x.CompareTo(y);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.RunestoneGalleryContent;
            }
        }

        public override MenuContent.TabSpriteParameters TabSprite
        {
            get
            {
                MenuContent.TabSpriteParameters parameters = new MenuContent.TabSpriteParameters();
                parameters.SpriteAtlasId = "Menu";
                parameters.SpriteId = "sprite_uit_runeicon";
                parameters.SpriteSize = new Vector2?((Vector2) (Vector2.one * 95f));
                return parameters;
            }
        }
    }
}

