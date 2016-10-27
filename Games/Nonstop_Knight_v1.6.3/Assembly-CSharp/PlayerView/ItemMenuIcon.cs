namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemMenuIcon : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public GameObject ActiveIndicator;
        public Image Borders;
        public UnityEngine.UI.Button Button;
        public ItemType CompatibleItemType;
        public Image Icon;
        public GameObject NotifierRoot;
        public Text NotifierText;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void refresh(bool active, ItemInstance itemInstance, int notificationCount)
        {
            this.ActiveIndicator.SetActive(active);
            this.NotifierRoot.SetActive(notificationCount > 0);
            this.Button.interactable = !active;
            if ((itemInstance != null) && (notificationCount > 0))
            {
                this.NotifierText.text = notificationCount.ToString();
            }
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
    }
}

