namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class RunestoneCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private Content <ActiveContent>k__BackingField;
        [CompilerGenerated]
        private Action<RunestoneCell> <ClickCallback>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public CanvasGroup AlphaGroup;
        public Text Description;
        public Image Icon;
        public Image IconBg;
        public Button IconButton;
        public GameObject IconSelectedBorder;
        public GameObject Notifier;
        public GameObject PetSourceIconRoot;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(Content content, Action<RunestoneCell> clickCallback)
        {
            this.ActiveContent = content;
            this.ClickCallback = clickCallback;
            this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(this.ActiveContent.IconSprite);
            this.refresh(this.ActiveContent.Selected, this.ActiveContent.Highlighted, this.ActiveContent.Interactable, this.ActiveContent.Grayscale, this.ActiveContent.Notify, this.ActiveContent.Description, this.ActiveContent.SelectionSource);
        }

        public void onClick()
        {
            if (this.ClickCallback != null)
            {
                this.ClickCallback(this);
            }
        }

        public void refresh(bool selected, bool highlighted, bool interactable, bool grayscale, bool notify, string description, RunestoneSelectionSource selectionSource)
        {
            this.ActiveContent.Selected = selected;
            this.ActiveContent.Highlighted = highlighted;
            this.ActiveContent.Interactable = interactable;
            this.ActiveContent.Grayscale = grayscale;
            this.ActiveContent.Notify = notify;
            this.ActiveContent.Description = description;
            this.ActiveContent.SelectionSource = selectionSource;
            if (this.Description != null)
            {
                this.Description.text = this.ActiveContent.Description;
            }
            else
            {
                this.Description.text = string.Empty;
            }
            this.IconSelectedBorder.SetActive(this.ActiveContent.Selected);
            this.IconButton.interactable = this.ActiveContent.Interactable;
            this.Icon.material = !this.ActiveContent.Grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
            this.IconBg.material = !this.ActiveContent.Grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
            this.Notifier.SetActive(notify);
            if (highlighted)
            {
                this.AlphaGroup.alpha = 1f;
            }
            else if (interactable)
            {
                this.AlphaGroup.alpha = 0.5f;
            }
            else
            {
                this.AlphaGroup.alpha = 0.3f;
            }
            this.PetSourceIconRoot.SetActive(this.ActiveContent.SelectionSource == RunestoneSelectionSource.Pet);
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

        public Action<RunestoneCell> ClickCallback
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

        public class Content
        {
            public string Description;
            public bool Grayscale;
            public bool Highlighted;
            public SpriteAtlasEntry IconSprite;
            public bool Interactable;
            public bool Notify;
            public object Obj;
            public string RunestoneId;
            public bool Selected;
            public RunestoneSelectionSource SelectionSource;
        }
    }
}

