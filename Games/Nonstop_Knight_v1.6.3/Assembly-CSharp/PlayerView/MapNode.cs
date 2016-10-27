namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class MapNode : MonoBehaviour
    {
        [CompilerGenerated]
        private Image <BossIcon>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.UI.Button <Button>k__BackingField;
        [CompilerGenerated]
        private Image <DebugImage>k__BackingField;
        [CompilerGenerated]
        private string <DungeonId>k__BackingField;
        [CompilerGenerated]
        private Image <Icon>k__BackingField;
        [CompilerGenerated]
        private RectTransform <MapNodePrototypeRectTm>k__BackingField;
        [CompilerGenerated]
        private ParticleSystem <OpenEffect>k__BackingField;
        [CompilerGenerated]
        private ParticleSystem <ParticleEffect>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private string <RewardId>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.UI.Text <Text>k__BackingField;
        [CompilerGenerated]
        private bool <Unlocked>k__BackingField;
        private Sprite m_chestSprite;
        private Sprite m_litSprite;
        private Vector2 m_origProtoSizeDelta;
        private Sprite m_unlitSprite;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.DebugImage = base.GetComponent<Image>();
            this.DebugImage.enabled = false;
            this.clear();
            this.initialize();
        }

        private void clear()
        {
            if (this.Button != null)
            {
                this.Button.onClick.RemoveAllListeners();
            }
            TransformExtensions.DestroyChildren(this.RectTm);
        }

        private void initialize()
        {
            this.m_litSprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "map_node_lit");
            this.m_unlitSprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "map_node_unlit");
            this.m_chestSprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_ui_mapmarker");
            GameObject obj2 = App.Binder.AssetBundleLoader.instantiatePrefab("Prefabs/Menu/MapNodePrototype");
            obj2.transform.SetParent(this.RectTm, false);
            obj2.transform.localPosition = Vector3.zero;
            this.MapNodePrototypeRectTm = obj2.GetComponent<RectTransform>();
            this.m_origProtoSizeDelta = this.MapNodePrototypeRectTm.sizeDelta;
            this.Button = obj2.transform.GetComponentInChildren<UnityEngine.UI.Button>();
            this.Icon = obj2.transform.FindChild("Icon").GetComponent<Image>();
            this.BossIcon = obj2.transform.FindChild("BossIcon").GetComponent<Image>();
            this.Text = obj2.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>();
            this.ParticleEffect = obj2.transform.FindChild("ParticleEffect").GetComponent<ParticleSystem>();
            this.OpenEffect = obj2.transform.FindChild("OpenEffect").GetComponent<ParticleSystem>();
            this.Button.interactable = false;
            this.Text.text = "0";
            this.Icon.sprite = this.m_litSprite;
        }

        public void initialize(string dungeonId, string rewardId)
        {
            this.DungeonId = dungeonId;
            this.RewardId = rewardId;
        }

        protected void LateUpdate()
        {
        }

        public void refresh(MapMenu mapMenu, bool unlocked)
        {
            <refresh>c__AnonStorey2ED storeyed = new <refresh>c__AnonStorey2ED();
            storeyed.mapMenu = mapMenu;
            storeyed.<>f__this = this;
            this.Unlocked = unlocked;
            Dungeon dungeon = null;
            if (this.IsDungeonNode)
            {
                dungeon = GameLogic.Binder.DungeonResources.getResource(this.DungeonId);
            }
            Player player = (GameLogic.Binder.GameState == null) ? null : GameLogic.Binder.GameState.Player;
            if (!this.IsDungeonNode)
            {
                this.Text.text = string.Empty;
            }
            else
            {
                this.Text.text = this.DungeonId;
            }
            if (!this.IsDungeonNode)
            {
                this.BossIcon.gameObject.SetActive(false);
                if (player.hasClaimedReward(this.RewardId))
                {
                    this.Icon.enabled = false;
                    this.MapNodePrototypeRectTm.sizeDelta = this.m_origProtoSizeDelta;
                }
                else
                {
                    this.Icon.enabled = true;
                    this.Icon.sprite = this.m_chestSprite;
                    this.MapNodePrototypeRectTm.sizeDelta = (Vector2) (this.m_origProtoSizeDelta * 1.25f);
                }
            }
            else
            {
                this.BossIcon.gameObject.SetActive(false);
                if (this.Unlocked)
                {
                    this.Icon.sprite = this.m_litSprite;
                }
                else
                {
                    this.Icon.sprite = this.m_unlitSprite;
                }
                this.MapNodePrototypeRectTm.sizeDelta = this.m_origProtoSizeDelta;
            }
            if (dungeon != null)
            {
                this.Button.onClick.AddListener(new UnityAction(storeyed.<>m__193));
            }
            else
            {
                this.Button.onClick.AddListener(new UnityAction(storeyed.<>m__194));
            }
            if (this.IsDungeonNode)
            {
                this.Button.interactable = this.Unlocked;
            }
            else if (player.hasClaimedReward(this.RewardId))
            {
                this.Button.interactable = false;
            }
            else
            {
                this.Button.interactable = this.Unlocked;
            }
            if (this.IsDungeonNode && this.Unlocked)
            {
                this.ParticleEffect.enableEmission = true;
                this.ParticleEffect.Play(true);
            }
            else
            {
                this.ParticleEffect.enableEmission = false;
            }
        }

        public Image BossIcon
        {
            [CompilerGenerated]
            get
            {
                return this.<BossIcon>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<BossIcon>k__BackingField = value;
            }
        }

        public UnityEngine.UI.Button Button
        {
            [CompilerGenerated]
            get
            {
                return this.<Button>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Button>k__BackingField = value;
            }
        }

        public Image DebugImage
        {
            [CompilerGenerated]
            get
            {
                return this.<DebugImage>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DebugImage>k__BackingField = value;
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

        public Image Icon
        {
            [CompilerGenerated]
            get
            {
                return this.<Icon>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Icon>k__BackingField = value;
            }
        }

        public bool IsDungeonNode
        {
            get
            {
                return !string.IsNullOrEmpty(this.DungeonId);
            }
        }

        public RectTransform MapNodePrototypeRectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<MapNodePrototypeRectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<MapNodePrototypeRectTm>k__BackingField = value;
            }
        }

        public ParticleSystem OpenEffect
        {
            [CompilerGenerated]
            get
            {
                return this.<OpenEffect>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<OpenEffect>k__BackingField = value;
            }
        }

        public ParticleSystem ParticleEffect
        {
            [CompilerGenerated]
            get
            {
                return this.<ParticleEffect>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ParticleEffect>k__BackingField = value;
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

        public string RewardId
        {
            [CompilerGenerated]
            get
            {
                return this.<RewardId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RewardId>k__BackingField = value;
            }
        }

        public UnityEngine.UI.Text Text
        {
            [CompilerGenerated]
            get
            {
                return this.<Text>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Text>k__BackingField = value;
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

        [CompilerGenerated]
        private sealed class <refresh>c__AnonStorey2ED
        {
            internal MapNode <>f__this;
            internal MapMenu mapMenu;

            internal void <>m__193()
            {
                this.mapMenu.onLevelButtonClicked(this.<>f__this.DungeonId);
            }

            internal void <>m__194()
            {
                this.mapMenu.onRewardButtonClicked(this.<>f__this.RewardId);
            }
        }
    }
}

