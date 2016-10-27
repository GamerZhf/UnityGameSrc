namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class DungeonEncounterCell : MonoBehaviour, IComparable<DungeonEncounterCell>
    {
        [CompilerGenerated]
        private GameLogic.Item <Item>k__BackingField;
        [CompilerGenerated]
        private int <Rarity>k__BackingField;
        public Image Borders;
        public Image Icon;
        private Sprite m_emptyIconSlotSprite;
        private Material m_greyscaleMaterial;
        public GameObject RandomIconRoot;
        public Text RankRequirement;
        public List<Image> Stars = new List<Image>();

        protected void Awake()
        {
            this.m_greyscaleMaterial = Resources.Load<Material>("Materials/image_greyscale");
            this.m_emptyIconSlotSprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_ui_iconslot_empty");
        }

        public int CompareTo(DungeonEncounterCell other)
        {
            if (this.Rarity > other.Rarity)
            {
                return -1;
            }
            if (this.Rarity < other.Rarity)
            {
                return 1;
            }
            if (this.Item != null)
            {
                if (other.Item == null)
                {
                    return -1;
                }
                return this.Item.CompareTo(other.Item);
            }
            if (other.Item != null)
            {
                return 1;
            }
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        public void initialize(GameLogic.Item item, Sprite sprite, int rarityMax, bool isRandom, int rankRequirement, bool rankReached)
        {
            this.Item = item;
            this.Rarity = rarityMax;
            if ((sprite == null) && !isRandom)
            {
                this.Borders.overrideSprite = this.m_emptyIconSlotSprite;
                for (int i = 0; i < this.Stars.Count; i++)
                {
                    this.Stars[i].enabled = false;
                }
            }
            else
            {
                this.Borders.overrideSprite = this.m_emptyIconSlotSprite;
                for (int j = 0; j < this.Stars.Count; j++)
                {
                    this.Stars[j].enabled = false;
                }
            }
            this.Icon.sprite = sprite;
            this.Icon.enabled = sprite != null;
            this.RandomIconRoot.SetActive(isRandom);
            if (rankRequirement > 0)
            {
                this.RankRequirement.text = "RANK " + rankRequirement;
            }
            else
            {
                this.RankRequirement.text = string.Empty;
            }
            if (rankReached)
            {
                this.Icon.material = null;
            }
            else
            {
                this.Icon.material = this.m_greyscaleMaterial;
            }
        }

        public void onButtonClicked()
        {
            if ((this.Item != null) && (this.Item.Type == ItemType.Weapon))
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.WeaponItemTooltip, this.Item, 0f, false, true);
            }
            else
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.NonWeaponItemTooltip, this.Item, 0f, false, true);
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

        public int Rarity
        {
            [CompilerGenerated]
            get
            {
                return this.<Rarity>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Rarity>k__BackingField = value;
            }
        }
    }
}

