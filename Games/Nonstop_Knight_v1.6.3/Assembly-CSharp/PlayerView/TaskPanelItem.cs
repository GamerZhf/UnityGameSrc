namespace PlayerView
{
    using App;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class TaskPanelItem : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private Content <ActiveContent>k__BackingField;
        [CompilerGenerated]
        private System.Action <AutoRefreshCallback>k__BackingField;
        [CompilerGenerated]
        private System.Action <ClickCallback>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private TaskPanelItemType <Type>k__BackingField;
        public GameObject ContentRoot;
        public GameObject CounterRoot;
        public Text CounterText;
        public GameObject Glow;
        public Image Icon;
        public RectTransform IconRectTransform;
        public ParticleSystem Sparkles;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(TaskPanelItemType type, Content content)
        {
            this.ActiveContent = content;
            this.Type = type;
            this.IconRectTransform.anchoredPosition = this.ActiveContent.IconOffset;
            RectTransformExtensions.SetSize(this.IconRectTransform, this.ActiveContent.IconSize);
            this.Sparkles.startColor = this.ActiveContent.SparklesColor;
            this.Sparkles.gameObject.SetActive(ConfigDevice.DeviceQuality() >= DeviceQualityType.Med);
            this.Glow.SetActive(ConfigDevice.DeviceQuality() >= DeviceQualityType.Med);
            this.refresh(null, null, 1, content.IconSprite);
        }

        public void onClick()
        {
            if (this.ClickCallback != null)
            {
                this.ClickCallback();
            }
            if (this.AutoRefreshCallback != null)
            {
                this.AutoRefreshCallback();
            }
        }

        public void refresh(System.Action clickCallback, System.Action autoRefreshCallback, int counter, [Optional, DefaultParameterValue(null)] SpriteAtlasEntry iconSprite)
        {
            this.ClickCallback = clickCallback;
            this.AutoRefreshCallback = autoRefreshCallback;
            this.CounterRoot.SetActive(counter > 1);
            this.CounterText.text = counter.ToString();
            if (iconSprite != null)
            {
                this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(iconSprite);
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

        public System.Action AutoRefreshCallback
        {
            [CompilerGenerated]
            get
            {
                return this.<AutoRefreshCallback>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AutoRefreshCallback>k__BackingField = value;
            }
        }

        public System.Action ClickCallback
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

        public TaskPanelItemType Type
        {
            [CompilerGenerated]
            get
            {
                return this.<Type>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Type>k__BackingField = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Content
        {
            public Vector2 IconOffset;
            public Vector2 IconSize;
            public SpriteAtlasEntry IconSprite;
            public Color SparklesColor;
        }
    }
}

