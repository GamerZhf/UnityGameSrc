namespace PlayerView
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class Card : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private Content <ActiveContent>k__BackingField;
        [CompilerGenerated]
        private PlayerView.CanvasGroupAlphaFading <CanvasGroupAlphaFading>k__BackingField;
        [CompilerGenerated]
        private Action<Card> <ClickCallback>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private TransformAnimation <TransformAnimation>k__BackingField;
        public UnityEngine.UI.Button Button;
        public GameObject ButtonRoot;
        public Image Icon;
        public Image IconBg;
        private List<Vector3> m_originalStarLocalPositions = new List<Vector3>();
        private int m_originalTextFontSize;
        public Image ModeIcon;
        public Image ModeIconBg;
        public Image PriceIcon;
        public UnityEngine.UI.Text PriceText;
        public UnityEngine.UI.Text PriceTextNoIcon;
        public AnimatedProgressBar ProgressBar;
        public Image ProgressBarFgImage;
        public UnityEngine.UI.Text ProgressBarText;
        public Image SlotBg;
        public GameObject SoldRoot;
        public UnityEngine.UI.Text SoldText;
        public GameObject StackRoot;
        public UnityEngine.UI.Text StackText;
        public List<Image> Stars = new List<Image>();
        public GameObject StarsRoot;
        public GameObject StickerRoot;
        public UnityEngine.UI.Text StickerText;
        public UnityEngine.UI.Text Text;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            for (int i = 0; i < this.Stars.Count; i++)
            {
                this.m_originalStarLocalPositions.Add(this.Stars[i].transform.localPosition);
            }
            this.m_originalTextFontSize = this.Text.fontSize;
        }

        public void cleanUpForReuse()
        {
            this.Button.transform.localScale = Vector3.one;
        }

        public void initialize(Content content, Action<Card> clickCallback)
        {
            this.ActiveContent = content;
            this.ClickCallback = clickCallback;
            if (this.ActiveContent.Sprite != null)
            {
                this.SlotBg.enabled = false;
                this.Icon.enabled = true;
                this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(this.ActiveContent.Sprite);
                if (this.ActiveContent.NormalizedProgress >= 0f)
                {
                    this.ProgressBar.gameObject.SetActive(true);
                    this.ProgressBar.setNormalizedValue(this.ActiveContent.NormalizedProgress);
                    if (!string.IsNullOrEmpty(this.ActiveContent.ProgressBarText))
                    {
                        this.ProgressBarText.enabled = true;
                        this.ProgressBarText.text = this.ActiveContent.ProgressBarText;
                    }
                    else
                    {
                        this.ProgressBarText.enabled = false;
                    }
                    if (this.ActiveContent.NormalizedProgress < 1f)
                    {
                        this.ProgressBarFgImage.sprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_ui_progressbar_front_blue");
                    }
                    else
                    {
                        this.ProgressBarFgImage.sprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_ui_progressbar_front_gold");
                    }
                }
                else
                {
                    this.ProgressBar.gameObject.SetActive(false);
                }
                if (this.ActiveContent.SoldText != null)
                {
                    this.ButtonRoot.SetActive(false);
                    this.SoldRoot.SetActive(true);
                    this.SoldText.text = this.ActiveContent.SoldText;
                }
                else
                {
                    this.ButtonRoot.SetActive(true);
                    this.SoldRoot.SetActive(false);
                }
                if (!string.IsNullOrEmpty(this.ActiveContent.StickerText))
                {
                    this.StickerRoot.SetActive(true);
                    this.StickerText.text = this.ActiveContent.StickerText;
                }
                else
                {
                    this.StickerRoot.SetActive(false);
                }
                if (this.StackRoot != null)
                {
                    if (!string.IsNullOrEmpty(this.ActiveContent.StackText))
                    {
                        this.StackRoot.SetActive(true);
                        this.StackText.text = this.ActiveContent.StackText;
                    }
                    else
                    {
                        this.StackRoot.SetActive(false);
                    }
                }
            }
            else
            {
                this.ButtonRoot.SetActive(true);
                this.SoldRoot.SetActive(false);
                this.SlotBg.enabled = true;
                this.Icon.enabled = false;
                this.Text.enabled = false;
                this.Button.interactable = false;
                this.ProgressBar.gameObject.SetActive(false);
                this.StickerRoot.SetActive(false);
                if (this.StackRoot != null)
                {
                    this.StackRoot.SetActive(false);
                }
                MenuHelpers.RefreshStarContainer(this.Stars, this.m_originalStarLocalPositions, -1, false);
            }
            this.refresh(this.ActiveContent.Text, this.ActiveContent.PriceText, this.ActiveContent.PriceIcon, this.ActiveContent.Interactable, this.ActiveContent.Grayscale);
        }

        public void onClick()
        {
            if (this.ClickCallback != null)
            {
                this.ClickCallback(this);
            }
        }

        public void refresh(string text, string priceText, SpriteAtlasEntry priceIcon, bool interactable, bool grayscale)
        {
            this.ActiveContent.Text = text;
            this.ActiveContent.PriceText = priceText;
            this.ActiveContent.PriceIcon = priceIcon;
            this.ActiveContent.Interactable = interactable;
            this.ActiveContent.Grayscale = grayscale;
            if (!string.IsNullOrEmpty(this.ActiveContent.Text))
            {
                this.Text.enabled = true;
                if (this.ActiveContent.TextFontSize > -1)
                {
                    this.Text.fontSize = this.ActiveContent.TextFontSize;
                }
                else
                {
                    this.Text.fontSize = this.m_originalTextFontSize;
                }
                this.Text.text = this.ActiveContent.Text;
                this.StarsRoot.SetActive(false);
            }
            else
            {
                this.Text.enabled = false;
                this.StarsRoot.SetActive(true);
                MenuHelpers.RefreshStarContainer(this.Stars, this.m_originalStarLocalPositions, this.ActiveContent.Rarity, this.ActiveContent.Grayscale);
            }
            this.Button.interactable = this.ActiveContent.Interactable;
            this.Icon.material = !this.ActiveContent.Grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
            this.IconBg.material = !this.ActiveContent.Grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
            if (this.ModeIcon != null)
            {
                this.ModeIcon.material = !this.ActiveContent.Grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
                this.ModeIconBg.material = !this.ActiveContent.Grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
            }
            if (!string.IsNullOrEmpty(this.ActiveContent.PriceText))
            {
                if (this.ActiveContent.PriceIcon != null)
                {
                    this.PriceIcon.enabled = true;
                    this.PriceIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(this.ActiveContent.PriceIcon);
                    this.PriceText.enabled = true;
                    this.PriceText.text = this.ActiveContent.PriceText;
                    this.PriceTextNoIcon.gameObject.SetActive(false);
                }
                else
                {
                    this.PriceIcon.enabled = false;
                    this.PriceText.enabled = false;
                    this.PriceTextNoIcon.gameObject.SetActive(true);
                    this.PriceTextNoIcon.text = this.ActiveContent.PriceText;
                }
            }
            else
            {
                this.PriceText.enabled = false;
                this.PriceIcon.enabled = false;
                this.PriceTextNoIcon.gameObject.SetActive(false);
            }
        }

        public Content ActiveContent
        {
            [CompilerGenerated]
            get
            {
                return this.<ActiveContent>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ActiveContent>k__BackingField = value;
            }
        }

        public PlayerView.CanvasGroupAlphaFading CanvasGroupAlphaFading
        {
            [CompilerGenerated]
            get
            {
                return this.<CanvasGroupAlphaFading>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CanvasGroupAlphaFading>k__BackingField = value;
            }
        }

        public Action<Card> ClickCallback
        {
            [CompilerGenerated]
            get
            {
                return this.<ClickCallback>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ClickCallback>k__BackingField = value;
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

        public TransformAnimation TransformAnimation
        {
            [CompilerGenerated]
            get
            {
                return this.<TransformAnimation>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TransformAnimation>k__BackingField = value;
            }
        }

        public class Content
        {
            public bool Grayscale;
            public string Id;
            public bool Interactable;
            public float NormalizedProgress = -1f;
            public object Obj;
            public SpriteAtlasEntry PriceIcon;
            public string PriceText;
            public string ProgressBarText;
            public int Rarity = -1;
            public string SoldText;
            public SpriteAtlasEntry Sprite;
            public string StackText;
            public string StickerText;
            public string Text;
            public int TextFontSize = -1;
        }
    }
}

