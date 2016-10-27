namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class DungeonTooltip : MenuContent
    {
        public Text EnergyCost;
        private Dungeon m_dungeon;
        public List<MaterialCell> Materials;

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
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
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(this.m_dungeon.Name), string.Empty, string.Empty);
            this.EnergyCost.text = this.m_dungeon.EnergyCost.ToString();
            int num = 0;
            for (int i = 0; i < this.m_dungeon.LootPool.Count; i++)
            {
                if (i >= 4)
                {
                    Debug.LogWarning("Dungeon " + this.m_dungeon.Id + " has more than 4 rewards in loot pool, cannot visualize.");
                    break;
                }
                string lootTableRollId = this.m_dungeon.LootPool[i];
                Item item = GameLogic.Binder.ItemResources.getItemForLootTableRollId(lootTableRollId, ItemType.UNSPECIFIED);
                this.Materials[num].gameObject.SetActive(true);
                this.Materials[num].initialize(item);
                this.Materials[num].Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", item.SpriteId);
                num++;
            }
            for (int j = num; j < this.Materials.Count; j++)
            {
                this.Materials[j].gameObject.SetActive(false);
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.DungeonTooltip;
            }
        }
    }
}

