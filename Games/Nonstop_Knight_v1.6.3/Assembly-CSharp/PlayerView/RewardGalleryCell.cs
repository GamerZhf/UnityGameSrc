namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class RewardGalleryCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private Content <ActiveContent>k__BackingField;
        [CompilerGenerated]
        private TransformAnimation <AnimatedStar>k__BackingField;
        [CompilerGenerated]
        private Action<RewardGalleryCell> <ClickCallback>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private RewardGalleryCellType <Type>k__BackingField;
        public ParticleSystem AnimatedStarFx;
        public UnityEngine.UI.Button Button;
        public UnityEngine.CanvasGroup CanvasGroup;
        public GameObject ContentOverlay;
        public UnityEngine.UI.Text CornerText;
        public Image CornerTextBg;
        public UnityEngine.UI.Text Description;
        public CanvasGroupAlphaFading DescriptionCanvasGroup;
        public GameObject Flipside;
        public PlayerView.HoverTransformAnimation HoverTransformAnimation;
        public Image Icon;
        public Image IconBg;
        public PlayerView.LeaderboardImage LeaderboardImage;
        private Coroutine m_animatedStarRoutine;
        private List<Vector3> m_originalSmallStarLocalPositions = new List<Vector3>();
        private List<Vector3> m_originalStarLocalPositions = new List<Vector3>();
        public GameObject ModeOverlay;
        public GameObject NotifierRoot;
        public AnimatedProgressBar ProgressBar;
        public CanvasGroupAlphaFading ProgressBarCanvasGroup;
        public GameObject ProgressBarFgDefault;
        public GameObject ProgressBarFgFull;
        public UnityEngine.UI.Text ProgressBarText;
        public Image SelectedBorders;
        public List<Image> SmallStars = new List<Image>();
        public GameObject SmallStarsRoot;
        public CanvasGroupAlphaFading StarCanvasGroup;
        public List<Image> Stars = new List<Image>();
        public GameObject StarsRoot;
        public GameObject StickerRoot;
        public UnityEngine.UI.Text StickerText;
        public UnityEngine.UI.Text Text;
        public GameObject Tier1Fx;
        public GameObject Tier2Fx;

        [DebuggerHidden]
        private IEnumerator animatedStarGainRoutine()
        {
            <animatedStarGainRoutine>c__Iterator16E iteratore = new <animatedStarGainRoutine>c__Iterator16E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        public void animateStarGain()
        {
            UnityUtils.StopCoroutine(this, ref this.m_animatedStarRoutine);
            this.m_animatedStarRoutine = UnityUtils.StartCoroutine(this, this.animatedStarGainRoutine());
        }

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            for (int i = 0; i < this.Stars.Count; i++)
            {
                this.m_originalStarLocalPositions.Add(this.Stars[i].transform.localPosition);
            }
            for (int j = 0; j < this.SmallStars.Count; j++)
            {
                this.m_originalSmallStarLocalPositions.Add(this.SmallStars[j].transform.localPosition);
            }
            if (this.HoverTransformAnimation != null)
            {
                this.HoverTransformAnimation.cacheStartingOrientation();
                this.HoverTransformAnimation.Enabled = false;
            }
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.Stars[0].gameObject);
            obj2.name = "AnimatedStar";
            obj2.transform.SetParent(this.Stars[0].transform.parent, false);
            this.AnimatedStar = obj2.AddComponent<TransformAnimation>();
            obj2.SetActive(false);
        }

        public void cleanUpForReuse()
        {
            RectTransformExtensions.SetHeight(this.RectTm, 0f);
            RectTransformExtensions.SetWidth(this.RectTm, 0f);
            this.RectTm.anchorMin = new Vector2(0.5f, 0.5f);
            this.RectTm.anchorMax = new Vector2(0.5f, 0.5f);
            this.RectTm.localScale = Vector3.one;
            this.Button.transform.localScale = Vector3.one;
            this.NotifierRoot.SetActive(false);
            this.ProgressBarCanvasGroup.setTransparent(false);
            if (this.Tier1Fx != null)
            {
                this.Tier1Fx.SetActive(false);
            }
            if (this.Tier2Fx != null)
            {
                this.Tier2Fx.SetActive(false);
            }
            if (this.ContentOverlay != null)
            {
                this.ContentOverlay.SetActive(false);
            }
            if (this.ModeOverlay != null)
            {
                this.ModeOverlay.SetActive(false);
            }
            if (this.HoverTransformAnimation != null)
            {
                this.HoverTransformAnimation.Enabled = false;
            }
            this.resetStars();
        }

        public static Content CreateDefaultContentForReward(Reward reward, bool hideItemIdentity, LeaderboardEntry lbe)
        {
            string str;
            Content content;
            if (reward.ItemDrops.Count <= 0)
            {
                SpriteAtlasEntry entry17;
                if (reward.RunestoneDrops.Count > 0)
                {
                    string runestoneId = reward.RunestoneDrops[0];
                    ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(runestoneId);
                    content = new Content();
                    content.Sprite = runestoneData.Sprite;
                    content.Text = string.Empty;
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.DiamondDrops.Count > 0)
                {
                    SpriteAtlasEntry sprite = null;
                    ShopEntry shopEntryById = null;
                    if (Service.Binder.ShopManager != null)
                    {
                        shopEntryById = Service.Binder.ShopManager.GetShopEntryById(reward.ShopEntryId);
                    }
                    if (shopEntryById != null)
                    {
                        shopEntryById = shopEntryById;
                    }
                    else
                    {
                        shopEntryById = ConfigShops.GetShopEntry(reward.ShopEntryId);
                    }
                    if ((shopEntryById != null) && (shopEntryById.Type == ShopEntryType.DiamondBundle))
                    {
                        sprite = shopEntryById.Sprite;
                    }
                    else if ((shopEntryById != null) && (shopEntryById.Type == ShopEntryType.IapDiamonds))
                    {
                        sprite = shopEntryById.Sprite;
                    }
                    if (sprite == null)
                    {
                        sprite = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Diamond];
                    }
                    content = new Content();
                    content.Sprite = sprite;
                    content.Text = "+" + MenuHelpers.BigValueToString(reward.getTotalDiamondAmount());
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.CoinDrops.Count > 0)
                {
                    SpriteAtlasEntry entry3;
                    ShopEntry shopEntry = ConfigShops.GetShopEntry(reward.ShopEntryId);
                    if ((shopEntry != null) && (shopEntry.Type == ShopEntryType.CoinBundle))
                    {
                        entry3 = shopEntry.Sprite;
                    }
                    else
                    {
                        entry3 = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Coin];
                    }
                    content = new Content();
                    content.Sprite = entry3;
                    content.Text = "+" + MenuHelpers.BigValueToString(reward.getTotalCoinAmount());
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.TokenDrops.Count > 0)
                {
                    SpriteAtlasEntry entry5;
                    ShopEntry entry6 = ConfigShops.GetShopEntry(reward.ShopEntryId);
                    if ((entry6 != null) && (entry6.Type == ShopEntryType.TokenBundle))
                    {
                        entry5 = entry6.Sprite;
                    }
                    else
                    {
                        entry5 = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Token];
                    }
                    content = new Content();
                    content.Sprite = entry5;
                    content.Text = "+" + MenuHelpers.BigValueToString(reward.getTotalTokenAmount());
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.Boost != BoostType.UNSPECIFIED)
                {
                    content = new Content();
                    content.Sprite = ConfigBoosts.SHARED_DATA[reward.Boost].Sprite;
                    content.Text = ConfigBoosts.SHARED_DATA[reward.Boost].Name;
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.Skill != SkillType.NONE)
                {
                    ConfigSkills.SharedData data2 = ConfigSkills.SHARED_DATA[reward.Skill];
                    content = new Content();
                    content.Sprite = new SpriteAtlasEntry(data2.Spritesheet, data2.Sprite);
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.Revives > 0)
                {
                    SpriteAtlasEntry entry7;
                    if (!string.IsNullOrEmpty(reward.ShopEntryId))
                    {
                        entry7 = ConfigShops.GetShopEntry(reward.ShopEntryId).Sprite;
                    }
                    else
                    {
                        entry7 = new SpriteAtlasEntry("Menu", "icon_bottle_red");
                    }
                    content = new Content();
                    content.Sprite = entry7;
                    content.Text = (reward.Revives <= 1) ? null : ("+" + reward.Revives);
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.DustDrops.Count > 0)
                {
                    SpriteAtlasEntry entry9;
                    ShopEntry entry10 = ConfigShops.GetShopEntry(reward.ShopEntryId);
                    if ((entry10 != null) && (entry10.Type == ShopEntryType.DustBundle))
                    {
                        entry9 = entry10.Sprite;
                    }
                    else
                    {
                        entry9 = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Dust];
                    }
                    content = new Content();
                    content.Sprite = entry9;
                    content.Text = "+" + MenuHelpers.BigValueToString(reward.getTotalDustAmount());
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.FrenzyPotions > 0)
                {
                    SpriteAtlasEntry entry11;
                    if (!string.IsNullOrEmpty(reward.ShopEntryId))
                    {
                        entry11 = ConfigShops.GetShopEntry(reward.ShopEntryId).Sprite;
                    }
                    else
                    {
                        entry11 = new SpriteAtlasEntry("Menu", "icon_bottle_frenzy");
                    }
                    content = new Content();
                    content.Sprite = entry11;
                    content.Text = (reward.FrenzyPotions <= 1) ? null : ("+" + reward.FrenzyPotions);
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.XpPotions > 0)
                {
                    SpriteAtlasEntry entry13;
                    if (!string.IsNullOrEmpty(reward.ShopEntryId))
                    {
                        entry13 = ConfigShops.GetShopEntry(reward.ShopEntryId).Sprite;
                    }
                    else
                    {
                        entry13 = new SpriteAtlasEntry("Menu", "icon_xp_pile1");
                    }
                    content = new Content();
                    content.Sprite = entry13;
                    content.Text = "+" + _.L(ConfigLoca.RESOURCES_XP, null, false);
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.BossPotions > 0)
                {
                    SpriteAtlasEntry entry15;
                    if (!string.IsNullOrEmpty(reward.ShopEntryId))
                    {
                        entry15 = ConfigShops.GetShopEntry(reward.ShopEntryId).Sprite;
                    }
                    else
                    {
                        entry15 = new SpriteAtlasEntry("Menu", "icon_bossticket");
                    }
                    content = new Content();
                    content.Sprite = entry15;
                    content.Text = (reward.BossPotions <= 1) ? null : ("+" + reward.BossPotions);
                    content.LeaderboardEntry = lbe;
                    return content;
                }
                if (reward.Pets.Count > 0)
                {
                    Character character = GameLogic.Binder.CharacterResources.getResource(reward.Pets[0].PetId);
                    if (character != null)
                    {
                        content = new Content();
                        content.Sprite = character.AvatarSprite;
                        content.Text = (reward.Pets[0].Amount <= 1) ? null : ("+" + reward.Pets[0].Amount);
                        content.LeaderboardEntry = lbe;
                        return content;
                    }
                }
                if (reward.MegaBoxes <= 0)
                {
                    return new Content();
                }
                if (!string.IsNullOrEmpty(reward.Sprite))
                {
                    entry17 = new SpriteAtlasEntry("Menu", reward.Sprite);
                }
                else
                {
                    entry17 = new SpriteAtlasEntry("Menu", "icon_megabox_pile1");
                }
                content = new Content();
                content.Sprite = entry17;
                content.Text = (reward.MegaBoxes <= 1) ? null : ("+" + reward.MegaBoxes);
                content.LeaderboardEntry = lbe;
                return content;
            }
            ItemInstance instance = reward.ItemDrops[0];
            Item item = instance.Item;
            if (!hideItemIdentity)
            {
                content = new Content();
                content.Sprite = new SpriteAtlasEntry("Menu", item.SpriteId);
                content.StarRank = instance.Rarity;
                content.LeaderboardEntry = lbe;
                return content;
            }
            switch (item.Type)
            {
                case ItemType.Weapon:
                    str = _.L(ConfigLoca.ITEMS_WEAPON, null, false);
                    break;

                case ItemType.Armor:
                    str = _.L(ConfigLoca.ITEMS_ARMOR, null, false);
                    break;

                case ItemType.Cloak:
                    str = _.L(ConfigLoca.ITEMS_CLOAK, null, false);
                    break;

                default:
                    str = item.Type.ToString();
                    break;
            }
            content = new Content();
            content.Sprite = ConfigUi.ITEM_TYPE_ABSTRACT_SPRITES_IDS[item.Type];
            content.Text = str;
            content.LeaderboardEntry = lbe;
            return content;
        }

        public void initialize(Content content, [Optional, DefaultParameterValue(null)] Action<RewardGalleryCell> clickCallback)
        {
            this.ActiveContent = content;
            this.ClickCallback = clickCallback;
            this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(this.ActiveContent.Sprite);
            this.Icon.enabled = this.Icon.sprite != null;
            this.resetStars();
            if (!string.IsNullOrEmpty(this.ActiveContent.Text))
            {
                this.Text.enabled = true;
                this.Text.text = this.ActiveContent.Text;
                this.StarsRoot.SetActive(false);
                this.SmallStarsRoot.SetActive(false);
            }
            else
            {
                this.Text.enabled = false;
                this.StarsRoot.SetActive(!content.DoUseSmallStars);
                this.SmallStarsRoot.SetActive(content.DoUseSmallStars);
            }
            if (this.Description != null)
            {
                if (!string.IsNullOrEmpty(this.ActiveContent.Description))
                {
                    this.Description.text = this.ActiveContent.Description;
                    this.Description.enabled = true;
                }
                else
                {
                    this.Description.enabled = false;
                }
            }
            if (this.ActiveContent.LeaderboardEntry != null)
            {
                this.LeaderboardImage.gameObject.SetActive(true);
                this.Text.enabled = false;
                this.StarsRoot.SetActive(false);
                this.SmallStarsRoot.SetActive(false);
                if (this.ActiveContent.LeaderboardEntry.Dummy)
                {
                    this.LeaderboardImage.refresh(this.ActiveContent.LeaderboardEntry);
                }
                else
                {
                    List<PlayerView.LeaderboardImage> lbImages = new List<PlayerView.LeaderboardImage>();
                    lbImages.Add(this.LeaderboardImage);
                    List<LeaderboardEntry> lbEntries = new List<LeaderboardEntry>();
                    lbEntries.Add(this.ActiveContent.LeaderboardEntry);
                    Service.Binder.FacebookAdapter.PopulateImages(lbImages, lbEntries);
                }
            }
            else
            {
                this.LeaderboardImage.gameObject.SetActive(false);
            }
            if (this.ActiveContent.ToProgressBarNormalizedValue > -1f)
            {
                this.ProgressBarText.text = this.ActiveContent.ProgressBarText;
                this.ProgressBarFgDefault.SetActive(this.ActiveContent.FromProgressBarNormalizedValue < 1f);
                this.ProgressBarFgFull.SetActive(!this.ProgressBarFgDefault.activeSelf);
                this.ProgressBar.gameObject.SetActive(true);
                this.ProgressBar.setNormalizedValue(this.ActiveContent.FromProgressBarNormalizedValue);
            }
            else
            {
                this.ProgressBar.gameObject.SetActive(false);
            }
            if (!string.IsNullOrEmpty(this.ActiveContent.CornerText))
            {
                this.CornerText.text = this.ActiveContent.CornerText;
                this.CornerText.gameObject.SetActive(true);
                this.CornerTextBg.gameObject.SetActive(true);
            }
            else
            {
                this.CornerText.gameObject.SetActive(false);
                this.CornerTextBg.gameObject.SetActive(false);
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
            if (this.ContentOverlay != null)
            {
                this.ContentOverlay.SetActive(false);
            }
            if (this.ModeOverlay != null)
            {
                this.ModeOverlay.SetActive(false);
            }
            if (this.HoverTransformAnimation != null)
            {
                this.HoverTransformAnimation.loadParameterSet(MenuTreasureChest.IdleHoverTransformParams);
                this.HoverTransformAnimation.Enabled = false;
            }
            this.refresh(this.ActiveContent.Selected, this.ActiveContent.Notify, this.ActiveContent.Grayscale, this.ActiveContent.Interactable);
        }

        public void onClick()
        {
            if (this.ClickCallback != null)
            {
                this.ClickCallback(this);
            }
        }

        public void refresh(bool selected, bool notify, bool grayscale, bool interactable)
        {
            this.ActiveContent.Selected = selected;
            this.ActiveContent.Notify = notify;
            this.ActiveContent.Grayscale = grayscale;
            this.ActiveContent.Interactable = interactable;
            this.SelectedBorders.enabled = this.ActiveContent.Selected;
            this.NotifierRoot.SetActive(this.ActiveContent.Notify);
            this.CanvasGroup.alpha = !this.ActiveContent.Grayscale ? 1f : 0.3f;
            this.Icon.material = !this.ActiveContent.Grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
            this.IconBg.material = !this.ActiveContent.Grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
            this.Button.interactable = this.ActiveContent.Interactable;
            if (this.ActiveContent.DoUseSmallStars)
            {
                if (this.ActiveContent.ShowHiddenStars)
                {
                    MenuHelpers.RefreshStarContainerWithBackground(this.SmallStars, this.ActiveContent.StarRank, true);
                }
                else
                {
                    MenuHelpers.RefreshStarContainer(this.SmallStars, this.m_originalSmallStarLocalPositions, this.ActiveContent.StarRank, this.ActiveContent.Grayscale);
                }
            }
            else if (this.ActiveContent.ShowHiddenStars)
            {
                MenuHelpers.RefreshStarContainerWithBackground(this.Stars, this.ActiveContent.StarRank, true);
            }
            else
            {
                MenuHelpers.RefreshStarContainer(this.Stars, this.m_originalStarLocalPositions, this.ActiveContent.StarRank, this.ActiveContent.Grayscale);
            }
            if (this.ActiveContent.AnimateStarGain)
            {
                int num = this.ActiveContent.StarRank - 1;
                if ((num >= 0) && (num < this.Stars.Count))
                {
                    this.Stars[num].material = PlayerView.Binder.DisabledUiMaterial;
                    this.Stars[num].color = ConfigUi.STAR_GRAYSCALE_HIGHLIGHTED;
                }
            }
        }

        private void resetStars()
        {
            for (int i = 0; i < this.Stars.Count; i++)
            {
                this.Stars[i].transform.localPosition = this.m_originalStarLocalPositions[i];
                this.Stars[i].material = null;
                this.Stars[i].color = Color.white;
                this.Stars[i].enabled = true;
            }
            for (int j = 0; j < this.SmallStars.Count; j++)
            {
                this.SmallStars[j].transform.localPosition = this.m_originalSmallStarLocalPositions[j];
                this.SmallStars[j].material = null;
                this.SmallStars[j].color = Color.white;
                this.SmallStars[j].enabled = true;
            }
            this.StarCanvasGroup.setTransparent(false);
            this.AnimatedStar.gameObject.SetActive(false);
            this.AnimatedStarFx.gameObject.SetActive(false);
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

        public TransformAnimation AnimatedStar
        {
            [CompilerGenerated]
            get
            {
                return this.<AnimatedStar>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AnimatedStar>k__BackingField = value;
            }
        }

        public Action<RewardGalleryCell> ClickCallback
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

        public RewardGalleryCellType Type
        {
            [CompilerGenerated]
            get
            {
                return this.<Type>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Type>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <animatedStarGainRoutine>c__Iterator16E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RewardGalleryCell <>f__this;
            internal int <starIdx>__0;
            internal TransformAnimationTask <tt>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<starIdx>__0 = this.<>f__this.ActiveContent.StarRank - 1;
                        if ((this.<starIdx>__0 >= 0) && (this.<starIdx>__0 < this.<>f__this.Stars.Count))
                        {
                            this.<>f__this.Stars[this.<starIdx>__0].material = PlayerView.Binder.DisabledUiMaterial;
                            this.<>f__this.Stars[this.<starIdx>__0].color = ConfigUi.STAR_GRAYSCALE_HIGHLIGHTED;
                            this.<>f__this.AnimatedStar.Tm.SetSiblingIndex(this.<>f__this.Stars.Count - this.<starIdx>__0);
                            this.<>f__this.AnimatedStar.Tm.position = this.<>f__this.Stars[this.<starIdx>__0].transform.position;
                            this.<>f__this.AnimatedStar.Tm.localScale = (Vector3) (Vector3.one * 5f);
                            this.<>f__this.AnimatedStar.gameObject.SetActive(true);
                            this.<tt>__1 = new TransformAnimationTask(this.<>f__this.AnimatedStar.transform, 0.15f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                            this.<tt>__1.scale(Vector3.one, true, Easing.Function.LINEAR);
                            this.<>f__this.AnimatedStar.addTask(this.<tt>__1);
                            break;
                        }
                        this.<>f__this.m_animatedStarRoutine = null;
                        goto Label_02B9;

                    case 1:
                        break;

                    case 2:
                        goto Label_0224;

                    default:
                        goto Label_02B9;
                }
                while (this.<>f__this.AnimatedStar.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_02BB;
                }
                this.<>f__this.AnimatedStarFx.transform.position = this.<>f__this.AnimatedStar.Tm.position;
                this.<>f__this.AnimatedStarFx.gameObject.SetActive(true);
            Label_0224:
                while (this.<>f__this.AnimatedStarFx.isPlaying)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_02BB;
                }
                this.<>f__this.Stars[this.<starIdx>__0].material = null;
                this.<>f__this.Stars[this.<starIdx>__0].color = Color.white;
                this.<>f__this.AnimatedStar.gameObject.SetActive(false);
                this.<>f__this.AnimatedStarFx.gameObject.SetActive(false);
                this.<>f__this.m_animatedStarRoutine = null;
                goto Label_02B9;
                this.$PC = -1;
            Label_02B9:
                return false;
            Label_02BB:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        public class Content
        {
            public bool AnimateStarGain;
            public string CornerText;
            public string Description;
            public bool DoUseSmallStars;
            public float FromProgressBarNormalizedValue = -1f;
            public bool Grayscale;
            public bool Interactable;
            public GameLogic.LeaderboardEntry LeaderboardEntry;
            public bool Notify;
            public object Obj;
            public string ProgressBarText;
            public bool Selected;
            public bool ShowHiddenStars;
            public SpriteAtlasEntry Sprite;
            public int StarRank;
            public string StickerText;
            public string Text;
            public float ToProgressBarNormalizedValue = -1f;
            public RewardGalleryCellType Type;
        }
    }
}

