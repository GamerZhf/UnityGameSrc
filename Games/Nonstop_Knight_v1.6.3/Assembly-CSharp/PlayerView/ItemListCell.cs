namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemListCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private GameLogic.Item <Item>k__BackingField;
        [CompilerGenerated]
        private GameLogic.ItemInstance <ItemInstance>k__BackingField;
        public Image Background;
        public UnityEngine.UI.Button Button;
        public Image Icon;
        public Image IconBorders;
        public Image IndicatorNew;
        public Image IndicatorNewIcon;
        public Text Level;
        public Image LevelIcon;
        private Material m_greyscaleMaterial;
        private List<Vector3> m_originalStarLocalPositions = new List<Vector3>();
        public Text Name;
        public AnimatedProgressBar ProgressBar;
        public Image ProgressBarFill;
        public Text ProgressText;
        public List<Image> Stars = new List<Image>();
        public Text Type;
        public GameObject UnlockButtonRoot;
        public Text UnlockCoinCost;

        protected void Awake()
        {
            this.m_greyscaleMaterial = Resources.Load<Material>("Materials/image_greyscale");
            for (int i = 0; i < this.Stars.Count; i++)
            {
                this.m_originalStarLocalPositions.Add(this.Stars[i].transform.localPosition);
            }
        }

        public void cleanUpForReuse()
        {
        }

        void IPoolable.cleanUpForReuse()
        {
            this.cleanUpForReuse();
        }

        private bool isItemBetterThanEquipped(CharacterInstance owner, GameLogic.ItemInstance itemInstance)
        {
            return true;
        }

        public void refresh(CharacterInstance owner, GameLogic.ItemInstance itemInstance, GameLogic.Item item)
        {
            this.ItemInstance = itemInstance;
            this.Item = item;
            if (itemInstance != null)
            {
                this.Background.gameObject.SetActive(true);
                if (owner.isItemEquipped(itemInstance.Item))
                {
                    this.Background.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_ui_itemcard_equipped");
                }
                else
                {
                    this.Background.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_ui_itemcard_active");
                }
                this.Button.interactable = true;
                this.Icon.gameObject.SetActive(true);
                this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", itemInstance.Item.SpriteId);
                this.Icon.color = Color.white;
                this.IconBorders.gameObject.SetActive(true);
                this.IconBorders.enabled = true;
                this.IconBorders.material = null;
                if (owner.hasReachedUnlockFloorForItem(itemInstance))
                {
                    this.Icon.material = null;
                }
                else
                {
                    this.Icon.material = !ConfigUi.MENU_UNEQUIPPABLE_ITEM_GREYSCALE_ENABLED ? null : this.m_greyscaleMaterial;
                }
                this.LevelIcon.gameObject.SetActive(false);
                this.Level.gameObject.SetActive(false);
                MenuHelpers.RefreshStarContainer(this.Stars, this.m_originalStarLocalPositions, itemInstance.Rarity, false);
                this.IndicatorNew.enabled = !itemInstance.InspectedByPlayer;
                this.IndicatorNewIcon.enabled = !itemInstance.InspectedByPlayer;
                this.ProgressBar.gameObject.SetActive(true);
                this.ProgressText.gameObject.SetActive(true);
                this.ProgressBarFill.color = Color.white;
                this.ProgressText.text = "Lv 0";
                this.ProgressBar.setNormalizedValue(0f);
                this.ProgressBarFill.sprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_ui_progressbar_front_gold");
                this.UnlockButtonRoot.SetActive(false);
                this.Name.gameObject.SetActive(true);
                this.Name.text = StringExtensions.ToUpperLoca(itemInstance.Item.Name);
                this.Type.text = itemInstance.Item.Type.ToString();
            }
            else if (item != null)
            {
                this.Background.gameObject.SetActive(true);
                this.Background.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_ui_itemcard_inactive");
                this.Button.interactable = true;
                this.Icon.gameObject.SetActive(true);
                this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", item.SpriteId);
                this.Icon.material = this.m_greyscaleMaterial;
                this.Icon.color = Color.white;
                this.IconBorders.gameObject.SetActive(true);
                this.IconBorders.enabled = true;
                this.LevelIcon.gameObject.SetActive(false);
                this.Level.gameObject.SetActive(false);
                MenuHelpers.RefreshStarContainer(this.Stars, this.m_originalStarLocalPositions, item.Rarity, false);
                this.IconBorders.material = this.m_greyscaleMaterial;
                this.LevelIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", ConfigUi.RARITY_ITEM_LEVEL_BACKGROUND_SPRITES[0]);
                this.IndicatorNew.enabled = false;
                this.IndicatorNewIcon.enabled = false;
                int num = owner.getShardCountForItem(item);
                this.UnlockButtonRoot.SetActive(false);
                this.ProgressBar.gameObject.SetActive(true);
                this.ProgressText.gameObject.SetActive(true);
                this.ProgressBar.setNormalizedValue(Mathf.Clamp01(((float) num) / 1f));
                this.ProgressText.text = num + "/" + 1f;
                this.ProgressBarFill.sprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_ui_progressbar_front_blue");
                this.Name.gameObject.SetActive(true);
                this.Name.text = StringExtensions.ToUpperLoca(item.Name);
                this.Type.text = item.Type.ToString();
            }
            else
            {
                this.Background.gameObject.SetActive(false);
                this.Button.interactable = false;
                this.Icon.gameObject.SetActive(false);
                this.IconBorders.gameObject.SetActive(false);
                for (int i = 0; i < 5; i++)
                {
                    this.Stars[i].enabled = false;
                }
                this.LevelIcon.gameObject.SetActive(false);
                this.Level.gameObject.SetActive(false);
                this.IndicatorNew.enabled = false;
                this.IndicatorNewIcon.enabled = false;
                this.ProgressBar.gameObject.SetActive(false);
                this.ProgressText.gameObject.SetActive(false);
                this.UnlockButtonRoot.SetActive(false);
                this.Name.gameObject.SetActive(false);
                this.Type.gameObject.SetActive(false);
            }
        }

        public GameLogic.Item Item
        {
            [CompilerGenerated]
            get
            {
                return this.<Item>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Item>k__BackingField = value;
            }
        }

        public GameLogic.ItemInstance ItemInstance
        {
            [CompilerGenerated]
            get
            {
                return this.<ItemInstance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ItemInstance>k__BackingField = value;
            }
        }
    }
}

