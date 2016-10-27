namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class DungeonPopupContent : MenuContent
    {
        public Text EnergyCost;
        private Dungeon m_dungeon;
        private List<DungeonEncounterCell> m_dungeonEncounterCells = new List<DungeonEncounterCell>();
        public GameObject PanelContentRoot;
        public GameObject PanelVerticalLayout;
        public Button PlayButton;
        public AnimatedProgressBar ProgressBar;
        public Text RankCounterText;
        public Text RankTitleText;
        public GridLayoutGroup TreasurePoolGrid;

        private DungeonEncounterCell addDungeonEncounterCellToPanel(Transform parentTm, Item item, Sprite sprite, int rarityMax, bool isRandom, int rankRequirement, bool rankReached)
        {
            DungeonEncounterCell cell = PlayerView.Binder.DungeonEncounterCellPool.getObject();
            cell.transform.SetParent(parentTm);
            cell.transform.localScale = Vector3.one;
            cell.transform.localPosition = Vector3.zero;
            this.m_dungeonEncounterCells.Add(cell);
            cell.gameObject.SetActive(true);
            cell.initialize(item, sprite, rarityMax, isRandom, rankRequirement, rankReached);
            return cell;
        }

        private void cleanupPanel()
        {
            for (int i = this.m_dungeonEncounterCells.Count - 1; i >= 0; i--)
            {
                DungeonEncounterCell item = this.m_dungeonEncounterCells[i];
                this.m_dungeonEncounterCells.Remove(item);
                PlayerView.Binder.DungeonEncounterCellPool.returnObject(item);
            }
        }

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
            this.cleanupPanel();
        }

        public void onPlayButtonClicked()
        {
            MultiHeroMenu.InputParams params2 = new MultiHeroMenu.InputParams();
            params2.PreDungeonModeActive = true;
            params2.DungeonId = this.m_dungeon.Id;
            MultiHeroMenu.InputParams parameter = params2;
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MultiHeroMenu, MenuContentType.NONE, parameter, 0f, true, true);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            string id = (string) param;
            this.m_dungeon = GameLogic.Binder.DungeonResources.getResource(id);
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            this.refreshPanel();
        }

        private void refreshPanel()
        {
            this.cleanupPanel();
            this.PanelContentRoot.SetActive(true);
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(this.m_dungeon.Name), string.Empty, string.Empty);
            this.refreshPossibleLoot(this.m_dungeon);
            this.RankTitleText.text = "RANK " + 0;
            this.RankCounterText.text = 0.ToString();
            this.ProgressBar.setNormalizedValue(0.5f);
        }

        private void refreshPossibleLoot(Dungeon dungeon)
        {
            for (int i = 0; i < dungeon.LootPool.Count; i++)
            {
                string lootTableRollId = dungeon.LootPool[i];
                Item item = GameLogic.Binder.ItemResources.getItemForLootTableRollId(lootTableRollId, ItemType.UNSPECIFIED);
                int rarity = item.Rarity;
                Sprite sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", item.SpriteId);
                this.addDungeonEncounterCellToPanel(this.TreasurePoolGrid.transform, item, sprite, rarity, false, 0, true);
            }
            this.m_dungeonEncounterCells.Sort();
            for (int j = 0; j < this.m_dungeonEncounterCells.Count; j++)
            {
                this.m_dungeonEncounterCells[j].transform.SetSiblingIndex(j);
            }
            int constraintCount = this.TreasurePoolGrid.constraintCount;
            int num5 = this.m_dungeonEncounterCells.Count % constraintCount;
            if (num5 > 0)
            {
                int num6 = constraintCount - num5;
                for (int k = 0; k < num6; k++)
                {
                    this.addDungeonEncounterCellToPanel(this.TreasurePoolGrid.transform, null, null, 0, false, 0, true);
                }
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.DungeonPopupContent;
            }
        }
    }
}

