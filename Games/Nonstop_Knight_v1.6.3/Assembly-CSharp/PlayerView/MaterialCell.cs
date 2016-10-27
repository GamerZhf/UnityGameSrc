namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class MaterialCell : MonoBehaviour
    {
        [CompilerGenerated]
        private GameLogic.Item <Item>k__BackingField;
        public Image Background;
        public Image Borders;
        public UnityEngine.UI.Button Button;
        public Image Icon;
        public UnityEngine.UI.Text Text;

        protected void Awake()
        {
        }

        public void initialize(GameLogic.Item item)
        {
            this.Item = item;
            this.Button.interactable = this.Item != null;
        }

        public void onButtonClicked()
        {
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.NonWeaponItemTooltip, this.Item, 0f, false, true);
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
    }
}

