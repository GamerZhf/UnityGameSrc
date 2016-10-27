namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChestGalleryRow : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private GameLogic.ChestType <ChestType>k__BackingField;
        public RectTransform ChestRoot;
        public Text Info;
        public RectTransform ItemGrid;
        private ChestContentComparer m_chestContentComparer = new ChestContentComparer();
        private List<RewardGalleryCell> m_rewardGalleryCells = new List<RewardGalleryCell>();
        public Text Title;

        private void addItemToGrid(Item item, int rarity, SpriteAtlasEntry sprite)
        {
            RewardGalleryCell.Content content2 = new RewardGalleryCell.Content();
            content2.Obj = item;
            content2.Sprite = sprite;
            content2.StarRank = rarity;
            content2.DoUseSmallStars = true;
            RewardGalleryCell.Content content = content2;
            RewardGalleryCell cell = PlayerView.Binder.RewardGalleryCellPool.getObject(RewardGalleryCellType.RewardGalleryCellFazer);
            cell.transform.SetParent(this.ItemGrid, false);
            cell.initialize(content, new Action<RewardGalleryCell>(this.onItemCellClick));
            this.m_rewardGalleryCells.Add(cell);
            cell.gameObject.SetActive(true);
        }

        public void cleanUpForReuse()
        {
            for (int i = this.m_rewardGalleryCells.Count - 1; i >= 0; i--)
            {
                RewardGalleryCell item = this.m_rewardGalleryCells[i];
                this.m_rewardGalleryCells.Remove(item);
                PlayerView.Binder.RewardGalleryCellPool.returnObject(item, item.Type);
            }
        }

        public void initialize(GameLogic.ChestType chestType)
        {
            this.ChestType = chestType;
            ChestBlueprint blueprint = ConfigUi.CHEST_BLUEPRINTS[chestType];
            LootTable table = GameLogic.Binder.ItemResources.getDynamicChestLootTable(chestType);
            List<Item> list = new List<Item>(table.Items.Count);
            for (int i = 0; i < table.Items.Count; i++)
            {
                LootTableItem item = table.Items[i];
                if (!GameLogic.Binder.ItemResources.itemExists(item.Id))
                {
                    Debug.LogError("Only fixed item visualization supported for chests: " + item.Id);
                }
                else
                {
                    Item item2 = GameLogic.Binder.ItemResources.getItemForLootTableRollId(item.Id, ItemType.UNSPECIFIED);
                    list.Add(item2);
                }
            }
            list.Sort(this.m_chestContentComparer);
            for (int j = 0; j < list.Count; j++)
            {
                Item item3 = list[j];
                SpriteAtlasEntry sprite = new SpriteAtlasEntry("Menu", item3.SpriteId);
                this.addItemToGrid(item3, item3.Rarity, sprite);
            }
            this.Title.text = _.L(blueprint.Name, null, false);
            this.setupChest(chestType);
        }

        private void onItemCellClick(RewardGalleryCell cell)
        {
            Item item = (Item) cell.ActiveContent.Obj;
            TooltipMenu.InputParameters parameters2 = new TooltipMenu.InputParameters();
            parameters2.CenterOnTm = cell.RectTm;
            parameters2.MenuContentParams = item.Name;
            TooltipMenu.InputParameters parameter = parameters2;
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.InfoTooltip, parameter, 0f, false, true);
        }

        public void refresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = 0; i < (this.m_rewardGalleryCells.Count - 1); i++)
            {
                Item item = (Item) this.m_rewardGalleryCells[i].ActiveContent.Obj;
                bool interactable = player.hasEncounteredItem(item.Id);
                this.m_rewardGalleryCells[i].refresh(false, false, !interactable, interactable);
            }
            bool flag2 = player.hasEncounteredChest(this.ChestType);
            if (flag2)
            {
                this.Info.enabled = false;
            }
            else
            {
                int num2 = -1;
                if (App.Binder.ConfigMeta.CHEST_UNLOCK_FLOOR.ContainsKey(this.ChestType))
                {
                    num2 = App.Binder.ConfigMeta.CHEST_UNLOCK_FLOOR[this.ChestType];
                }
                this.Info.text = _.L(ConfigLoca.CHEST_GALLERY_DROP_FLOOR, new <>__AnonType12<string>(MenuHelpers.ColoredText(num2.ToString())), false);
                this.Info.enabled = true;
            }
            this.m_rewardGalleryCells[this.m_rewardGalleryCells.Count - 1].refresh(false, false, !flag2, false);
        }

        private void setupChest(GameLogic.ChestType chestType)
        {
            ChestBlueprint blueprint = ConfigUi.CHEST_BLUEPRINTS[chestType];
            RewardGalleryCell.Content content2 = new RewardGalleryCell.Content();
            content2.Sprite = blueprint.Icon;
            RewardGalleryCell.Content content = content2;
            RewardGalleryCell item = PlayerView.Binder.RewardGalleryCellPool.getObject(RewardGalleryCellType.RewardGalleryCellFazer);
            RectTransformExtensions.SetSize(item.RectTm, new Vector2(300f, 300f));
            item.transform.SetParent(this.ChestRoot, false);
            item.initialize(content, null);
            this.m_rewardGalleryCells.Add(item);
            item.gameObject.SetActive(true);
        }

        public GameLogic.ChestType ChestType
        {
            [CompilerGenerated]
            get
            {
                return this.<ChestType>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ChestType>k__BackingField = value;
            }
        }

        private class ChestContentComparer : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                if (x.Rarity > y.Rarity)
                {
                    return -1;
                }
                if (x.Rarity < y.Rarity)
                {
                    return 1;
                }
                return x.Type.CompareTo(y.Type);
            }
        }
    }
}

