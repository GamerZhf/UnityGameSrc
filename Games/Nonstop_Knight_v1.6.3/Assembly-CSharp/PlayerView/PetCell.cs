namespace PlayerView
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class PetCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private Content <ActiveContent>k__BackingField;
        [CompilerGenerated]
        private Action<PetCell> <ClickCallback>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public UnityEngine.UI.Button Button;
        public UnityEngine.CanvasGroup CanvasGroup;
        public Image Icon;
        public Image IconBorders;
        private List<Vector3> m_originalStarLocalPositions = new List<Vector3>();
        public GameObject Notifier;
        public AnimatedProgressBar ProgressBar;
        public Text ProgressBarText;
        public Image SelectedBorders;
        public List<Image> Stars = new List<Image>();
        public GameObject StarsRoot;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            for (int i = 0; i < this.Stars.Count; i++)
            {
                this.m_originalStarLocalPositions.Add(this.Stars[i].transform.localPosition);
            }
        }

        public void cleanUpForReuse()
        {
            RectTransformExtensions.SetHeight(this.RectTm, 0f);
            RectTransformExtensions.SetWidth(this.RectTm, 0f);
            this.RectTm.anchorMin = Vector2.zero;
            this.RectTm.anchorMax = Vector2.zero;
            this.RectTm.localScale = Vector3.one;
            this.Button.transform.localScale = Vector3.one;
            this.Notifier.SetActive(false);
        }

        public void initialize(Content content, [Optional, DefaultParameterValue(null)] Action<PetCell> clickCallback)
        {
            this.ActiveContent = content;
            this.ClickCallback = clickCallback;
            this.Button.interactable = this.ActiveContent.Interactable;
            this.IconBorders.material = !this.ActiveContent.Grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
            this.Icon.material = !this.ActiveContent.Grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
            this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(this.ActiveContent.Sprite);
            this.Icon.enabled = this.Icon.sprite != null;
            this.refresh(false, this.ActiveContent.Selected, this.ActiveContent.Notify, this.ActiveContent.ProgressBarNormalizedValue, this.ActiveContent.ProgressBarText);
        }

        public void onClick()
        {
            if (this.ClickCallback != null)
            {
                this.ClickCallback(this);
            }
        }

        public void refresh(bool highlighted, bool selected, bool notify, float progressBarNormalizeValue, string progressBarText)
        {
            this.ActiveContent.Selected = selected;
            this.ActiveContent.Notify = notify;
            this.ActiveContent.ProgressBarNormalizedValue = progressBarNormalizeValue;
            this.ActiveContent.ProgressBarText = progressBarText;
            this.SelectedBorders.enabled = this.ActiveContent.Selected;
            this.Notifier.SetActive(this.ActiveContent.Notify);
            if (this.ActiveContent.Grayscale)
            {
                this.Icon.color = !highlighted ? ConfigUi.COLOR_DEHIGHLIGHTED_LOCKED : ConfigUi.COLOR_HIGHLIGHTED;
                this.IconBorders.color = !highlighted ? ConfigUi.COLOR_DEHIGHLIGHTED_LOCKED : ConfigUi.COLOR_HIGHLIGHTED;
                this.SelectedBorders.color = !highlighted ? ConfigUi.COLOR_DEHIGHLIGHTED_LOCKED : ConfigUi.COLOR_HIGHLIGHTED;
            }
            else
            {
                this.Icon.color = !highlighted ? ConfigUi.COLOR_DEHIGHLIGHTED_UNLOCKED : ConfigUi.COLOR_HIGHLIGHTED;
                this.IconBorders.color = !highlighted ? ConfigUi.COLOR_DEHIGHLIGHTED_UNLOCKED : ConfigUi.COLOR_HIGHLIGHTED;
                this.SelectedBorders.color = !highlighted ? ConfigUi.COLOR_DEHIGHLIGHTED_UNLOCKED : ConfigUi.COLOR_HIGHLIGHTED;
            }
            if (this.ActiveContent.ShowHiddenStars)
            {
                MenuHelpers.RefreshStarContainerWithBackground(this.Stars, this.ActiveContent.StarRank, highlighted);
            }
            else
            {
                MenuHelpers.RefreshStarContainer(this.Stars, this.m_originalStarLocalPositions, this.ActiveContent.StarRank, highlighted);
            }
            if (this.ActiveContent.ProgressBarNormalizedValue > -1f)
            {
                this.ProgressBar.setNormalizedValue(this.ActiveContent.ProgressBarNormalizedValue);
                this.ProgressBarText.text = this.ActiveContent.ProgressBarText;
                this.ProgressBar.gameObject.SetActive(true);
                this.StarsRoot.gameObject.SetActive(false);
            }
            else
            {
                this.ProgressBar.gameObject.SetActive(false);
                this.StarsRoot.gameObject.SetActive(true);
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

        public Action<PetCell> ClickCallback
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
            public bool Grayscale;
            public bool Interactable;
            public bool Notify;
            public object Obj;
            public float ProgressBarNormalizedValue = -1f;
            public string ProgressBarText;
            public bool Selected;
            public bool ShowHiddenStars;
            public SpriteAtlasEntry Sprite;
            public int StarRank;
        }
    }
}

