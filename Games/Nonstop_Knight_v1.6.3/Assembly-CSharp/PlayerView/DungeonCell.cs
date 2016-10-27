namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class DungeonCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private string <DungeonId>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private bool <Unlocked>k__BackingField;
        public Image Background;
        public GameObject BossProgressionRoot;
        public Image BossRadialProgressionImage;
        public UnityEngine.UI.Button Button;
        public Image LeftIcon;
        public Image LockIcon;
        private Material m_greyscaleMaterial;
        private Sprite m_origBgSprite;
        public Text Title1;
        public Text Title2;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.m_greyscaleMaterial = Resources.Load<Material>("Materials/image_greyscale");
            this.m_origBgSprite = this.Background.sprite;
        }

        public void cleanUpForReuse()
        {
        }

        public void onButtonClicked()
        {
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.DungeonPopupContent, this.DungeonId, 0f, false, true);
        }

        public void refresh(string dungeonId, bool unlocked)
        {
            this.DungeonId = dungeonId;
            Dungeon dungeon = GameLogic.Binder.DungeonResources.getResource(dungeonId);
            this.Unlocked = unlocked;
            this.Button.interactable = unlocked;
            this.LeftIcon.material = !unlocked ? this.m_greyscaleMaterial : null;
            if (!this.Unlocked)
            {
                this.Background.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", ConfigUi.RARITY_POPUP_TITLE_SPRITES[0]);
            }
            else
            {
                this.Background.sprite = this.m_origBgSprite;
            }
            this.Title1.text = "AREA " + dungeon.Id;
            string[] strArray = new string[] { "Training grounds", "Odin's peaks", "Darkwoods", "Magic meadows", "Deep dungeons", "Fairy forest", "Jelly land", "Hammerheim", "Salt flats", "Ice plateau" };
            int index = dungeonId.GetHashCode() % strArray.Length;
            this.Title2.text = StringExtensions.ToUpperLoca(strArray[index]);
            this.LockIcon.gameObject.SetActive(!this.Unlocked);
            this.BossProgressionRoot.gameObject.SetActive(this.Unlocked);
            if (this.Unlocked)
            {
                this.BossRadialProgressionImage.fillAmount = 0.5f;
            }
        }

        public string DungeonId
        {
            [CompilerGenerated]
            get
            {
                return this.<DungeonId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DungeonId>k__BackingField = value;
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

        public bool Unlocked
        {
            [CompilerGenerated]
            get
            {
                return this.<Unlocked>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Unlocked>k__BackingField = value;
            }
        }
    }
}

