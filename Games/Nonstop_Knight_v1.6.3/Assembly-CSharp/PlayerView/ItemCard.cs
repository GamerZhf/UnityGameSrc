namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemCard : MonoBehaviour
    {
        [CompilerGenerated]
        private GameLogic.ItemInstance <ItemInstance>k__BackingField;
        public UnityEngine.UI.Button Button;
        public Text EquippedText;
        public Image Icon;
        public Text LevelValue;
        public Text Name;
        public GameObject Notifier;
        public Text PrimaryStatText;
        public Text PrimaryStatValue;
        public List<Image> Stars = new List<Image>();

        protected void Awake()
        {
        }

        public void refresh(GameLogic.ItemInstance itemInstance)
        {
            this.ItemInstance = itemInstance;
            Item item = this.ItemInstance.Item;
            this.Notifier.SetActive(!this.ItemInstance.InspectedByPlayer);
            this.Name.text = StringExtensions.ToUpperLoca(item.Name);
            this.LevelValue.text = this.ItemInstance.Level + "/" + App.Binder.ConfigMeta.GLOBAL_LEVEL_CAP;
            for (int i = 0; i < 5; i++)
            {
                this.Stars[i].enabled = item.Rarity >= i;
            }
            if (item.Type == ItemType.Weapon)
            {
                this.PrimaryStatText.text = "Attack";
                this.PrimaryStatValue.text = "CHANGE ME";
            }
            else
            {
                this.PrimaryStatText.text = "Defense";
                this.PrimaryStatValue.text = "CHANGE ME";
            }
            this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", itemInstance.Item.SpriteId);
            if (GameLogic.Binder.GameState.Player.ActiveCharacter.isItemInstanceEquipped(this.ItemInstance))
            {
                this.EquippedText.gameObject.SetActive(true);
            }
            else
            {
                this.EquippedText.gameObject.SetActive(false);
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

