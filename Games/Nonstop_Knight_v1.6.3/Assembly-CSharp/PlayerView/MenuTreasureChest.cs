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
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class MenuTreasureChest : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler, IPointerExitHandler, IPoolable
    {
        [CompilerGenerated]
        private UnityEngine.Canvas <Canvas>k__BackingField;
        [CompilerGenerated]
        private RewardCeremonyEntry <CeremonyEntry>k__BackingField;
        [CompilerGenerated]
        private RewardCeremonyMenu <CeremonyMenu>k__BackingField;
        [CompilerGenerated]
        private HoverTransformAnimation <ChestLockHoverTransformAnimation>k__BackingField;
        [CompilerGenerated]
        private HoverTransformAnimation <ChestTopHoverTransformAnimation>k__BackingField;
        [CompilerGenerated]
        private bool <FinalizationRoutineSkippedByPlayer>k__BackingField;
        [CompilerGenerated]
        private bool <FinalizationRunning>k__BackingField;
        [CompilerGenerated]
        private bool <Interactable>k__BackingField;
        [CompilerGenerated]
        private bool <IsOpen>k__BackingField;
        [CompilerGenerated]
        private bool <OpenAtStart>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private GameLogic.Reward <Reward>k__BackingField;
        [CompilerGenerated]
        private bool <RewardClaimed>k__BackingField;
        [CompilerGenerated]
        private List<RewardGalleryCell> <RewardGalleryCells>k__BackingField;
        [CompilerGenerated]
        private HoverTransformAnimation <RootHoverTransformAnimation>k__BackingField;
        [CompilerGenerated]
        private TransformAnimation <TransformAnimation>k__BackingField;
        public PrettyButton ButtonGetAll;
        public PrettyButton ButtonOkay;
        public Animation ChestAnimator;
        public Image ChestBase;
        public TransformAnimation ChestContentLeft;
        public TransformAnimation ChestContentMiddle;
        public TransformAnimation ChestContentRight;
        public TransformAnimation ChestContentRoot;
        public ParticleSystem ChestGlowEffect;
        public Image ChestIcon;
        public Image ChestLock;
        public RectTransform ChestRootTm;
        public Image ChestTop;
        public List<PrettyButton> GetCardButtons;
        public static bool HOLD_SHAKING_ENABLED = true;
        public static HoverTransformAnimation.ParameterSet IdleHoverTransformParams;
        private bool m_cardChoiceOkayButtonClicked;
        private Coroutine m_cardChoiceRoutine;
        private bool m_cardChoiceRoutinePendingClickFlag;
        private List<string> m_chestAnimClipNames = new List<string>();
        private Action<MenuTreasureChest> m_clickCallback;
        private bool m_clicked;
        private bool m_dragging;
        private List<IEnumerator> m_enumeratorList = new List<IEnumerator>();
        private Coroutine m_genericSpreadRoutine;
        private bool m_getAllPaidWithAd;
        private RewardGalleryCell m_lastClickedCell;
        private Dictionary<RectTransform, Vector3> m_origLocalPositions = new Dictionary<RectTransform, Vector3>();
        private Dictionary<RectTransform, Vector2> m_origSizeDeltas = new Dictionary<RectTransform, Vector2>();
        private Coroutine m_petAnimationRoutine;
        private bool m_upScaled;
        public static HoverTransformAnimation.ParameterSet RumblyHoverTransformParams;
        public static float SCALE_DURATION = 0.2f;
        public static HoverTransformAnimation.ParameterSet ShakyHoverTransformParams;
        public static float TARGET_UPSCALE = 1.1f;

        static MenuTreasureChest()
        {
            HoverTransformAnimation.ParameterSet set = new HoverTransformAnimation.ParameterSet();
            set.WaveFrequencyX = 1f;
            set.WaveLengthX = 1f;
            set.WaveAmplitudeX = 4f;
            set.WaveFrequencyY = 1.5f;
            set.WaveLengthY = 2f;
            set.WaveAmplitudeY = 8f;
            set.Translate = true;
            IdleHoverTransformParams = set;
            HoverTransformAnimation.ParameterSet set2 = new HoverTransformAnimation.ParameterSet();
            set2.WaveFrequencyX = 60f;
            set2.WaveLengthX = 10f;
            set2.WaveAmplitudeX = 3.5f;
            set2.WaveFrequencyY = 6f;
            set2.WaveLengthY = 10f;
            set2.WaveAmplitudeY = 1f;
            set2.Translate = true;
            set2.Rotate = true;
            ShakyHoverTransformParams = set2;
            HoverTransformAnimation.ParameterSet set3 = new HoverTransformAnimation.ParameterSet();
            set3.WaveFrequencyX = 60f;
            set3.WaveLengthX = 10f;
            set3.WaveAmplitudeX = 3.5f;
            set3.WaveFrequencyY = 6f;
            set3.WaveLengthY = 10f;
            set3.WaveAmplitudeY = 1f;
            set3.Translate = true;
            RumblyHoverTransformParams = set3;
        }

        private void addPetRewardGalleryCellToGrid(PetReward petReward, bool openAtStart, LeaderboardEntry lbe, bool consumeRewardsAfterChestOpen)
        {
            int duplicates;
            float num4;
            float num5;
            int level;
            Player player = GameLogic.Binder.GameState.Player;
            string petId = petReward.PetId;
            int num = 0;
            for (int i = 0; i < this.Reward.Pets.Count; i++)
            {
                if (this.Reward.Pets[i].PetId == petId)
                {
                    num += this.Reward.Pets[i].Amount;
                }
            }
            Character character = GameLogic.Binder.CharacterResources.getResource(petId);
            PetInstance instance = player.Pets.getPetInstance(petId);
            string str2 = null;
            if (instance != null)
            {
                if (consumeRewardsAfterChestOpen)
                {
                    duplicates = instance.Duplicates + num;
                }
                else
                {
                    duplicates = instance.Duplicates;
                }
                level = instance.Level;
                int num7 = App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(instance.Level + 1);
                num4 = Mathf.Clamp01(((float) Mathf.Max(duplicates - petReward.Amount, 0)) / ((float) num7));
                num5 = Mathf.Clamp01(((float) duplicates) / ((float) num7));
                if (instance.isAtMaxLevel())
                {
                    str2 = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_MAX, null, false));
                    num4 = 1f;
                    num5 = 1f;
                }
                else
                {
                    str2 = duplicates + " / " + num7;
                }
            }
            else
            {
                duplicates = num;
                level = 0;
                int num9 = App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(1);
                num4 = 0f;
                num5 = Mathf.Clamp01(((float) duplicates) / ((float) num9));
                str2 = duplicates + " / " + num9;
            }
            RewardGalleryCell.Content content2 = new RewardGalleryCell.Content();
            content2.Sprite = character.AvatarSprite;
            content2.StarRank = level;
            content2.ShowHiddenStars = true;
            content2.AnimateStarGain = (this.CeremonyEntry == ConfigUi.CeremonyEntries.PET_UNLOCK) || (this.CeremonyEntry == ConfigUi.CeremonyEntries.PET_LEVEL_UP);
            content2.CornerText = (petReward.Amount <= 0) ? null : ("+" + petReward.Amount);
            content2.ProgressBarText = (petReward.Amount <= 0) ? null : str2;
            content2.FromProgressBarNormalizedValue = (petReward.Amount <= 0) ? -1f : num4;
            content2.ToProgressBarNormalizedValue = (petReward.Amount <= 0) ? -1f : num5;
            content2.LeaderboardEntry = lbe;
            RewardGalleryCell.Content content = content2;
            this.addRewardGalleryCellToGrid(content, openAtStart);
        }

        private RewardGalleryCell addRewardGalleryCellToGrid(RewardGalleryCell.Content content, bool visibleAtStart)
        {
            RewardGalleryCell item = PlayerView.Binder.RewardGalleryCellPool.getObject(content.Type);
            int count = this.RewardGalleryCells.Count;
            float z = 0f;
            switch (this.RewardGalleryCells.Count)
            {
                case 0:
                    item.RectTm.SetParent(this.ChestContentMiddle.RectTm, false);
                    z = 13.5f;
                    break;

                case 1:
                    item.RectTm.SetParent(this.ChestContentLeft.RectTm, false);
                    z = -7f;
                    break;

                case 2:
                    item.RectTm.SetParent(this.ChestContentRight.RectTm, false);
                    z = 1.7f;
                    break;

                default:
                    UnityEngine.Debug.LogWarning("Adding more RewardGalleryCells that can be visualized");
                    item.RectTm.SetParent(this.ChestContentMiddle.RectTm, false);
                    break;
            }
            item.RectTm.anchorMin = new Vector2(0.5f, 0.5f);
            item.RectTm.anchorMax = new Vector2(0.5f, 0.5f);
            item.RectTm.pivot = new Vector2(0.5f, 0.5f);
            RectTransformExtensions.SetSize(item.RectTm, new Vector2(300f, 300f));
            if (this.usesCardChoiceFlow())
            {
                content.Obj = count;
                RectTransform component = item.RectTm.parent.GetComponent<RectTransform>();
                component.anchoredPosition = new Vector2(0f, -50f);
                component.localRotation = Quaternion.Euler(0f, 0f, z);
                this.setInteractable(false);
                item.DescriptionCanvasGroup.setTransparent(true);
                if (ConfigTournaments.TOURNAMENT_CARD_SNEAK_PEAK_ENABLED)
                {
                    item.Flipside.SetActive(false);
                    component.localScale = Vector3.zero;
                }
                else
                {
                    item.Flipside.SetActive(true);
                    component.localScale = (Vector3) (Vector3.one * 0.9f);
                }
            }
            else if (content.Type == RewardGalleryCellType.RewardGalleryCellTournamentCard)
            {
                item.RectTm.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -40f);
            }
            else
            {
                item.RectTm.anchoredPosition = Vector2.zero;
            }
            content.Interactable = true;
            item.initialize(content, new Action<RewardGalleryCell>(this.onRewardGalleryCellClick));
            this.RewardGalleryCells.Add(item);
            item.gameObject.SetActive(visibleAtStart);
            if (this.usesPetAnimationFlow())
            {
                item.ProgressBarCanvasGroup.setTransparent(true);
                item.StarCanvasGroup.setTransparent(true);
            }
            return item;
        }

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.RootHoverTransformAnimation = GameObjectExtensions.AddOrGetComponent<HoverTransformAnimation>(base.gameObject);
            this.ChestLockHoverTransformAnimation = GameObjectExtensions.AddOrGetComponent<HoverTransformAnimation>(this.ChestLock.gameObject);
            this.ChestTopHoverTransformAnimation = GameObjectExtensions.AddOrGetComponent<HoverTransformAnimation>(this.ChestTop.gameObject);
            this.TransformAnimation = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(base.gameObject);
            this.RewardGalleryCells = new List<RewardGalleryCell>(4);
            this.ChestGlowEffect.gameObject.SetActive(false);
            LangUtil.AddOrUpdateDictionaryEntry<RectTransform, Vector3>(this.m_origLocalPositions, this.ChestBase.rectTransform, this.ChestBase.rectTransform.localPosition);
            LangUtil.AddOrUpdateDictionaryEntry<RectTransform, Vector3>(this.m_origLocalPositions, this.ChestLock.rectTransform, this.ChestLock.rectTransform.localPosition);
            LangUtil.AddOrUpdateDictionaryEntry<RectTransform, Vector3>(this.m_origLocalPositions, this.ChestTop.rectTransform, this.ChestTop.rectTransform.localPosition);
            LangUtil.AddOrUpdateDictionaryEntry<RectTransform, Vector2>(this.m_origSizeDeltas, this.ChestIcon.rectTransform, this.ChestIcon.rectTransform.sizeDelta);
            LangUtil.AddOrUpdateDictionaryEntry<RectTransform, Vector2>(this.m_origSizeDeltas, this.ChestBase.rectTransform, this.ChestBase.rectTransform.sizeDelta);
            LangUtil.AddOrUpdateDictionaryEntry<RectTransform, Vector2>(this.m_origSizeDeltas, this.ChestLock.rectTransform, this.ChestLock.rectTransform.sizeDelta);
            LangUtil.AddOrUpdateDictionaryEntry<RectTransform, Vector2>(this.m_origSizeDeltas, this.ChestTop.rectTransform, this.ChestTop.rectTransform.sizeDelta);
            IEnumerator enumerator = this.ChestAnimator.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
                    this.m_chestAnimClipNames.Add(current.name);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
        }

        [DebuggerHidden]
        private IEnumerator blindCardChoiceRoutine()
        {
            <blindCardChoiceRoutine>c__Iterator146 iterator = new <blindCardChoiceRoutine>c__Iterator146();
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator blindCardChoiceUtilRoutine_HideCard(RewardGalleryCell cell, float duration, float targetLocalRotationZ, [Optional, DefaultParameterValue(null)] Vector3? targetLocalScale, [Optional, DefaultParameterValue(0f)] float delay)
        {
            <blindCardChoiceUtilRoutine_HideCard>c__Iterator148 iterator = new <blindCardChoiceUtilRoutine_HideCard>c__Iterator148();
            iterator.cell = cell;
            iterator.delay = delay;
            iterator.duration = duration;
            iterator.targetLocalRotationZ = targetLocalRotationZ;
            iterator.targetLocalScale = targetLocalScale;
            iterator.<$>cell = cell;
            iterator.<$>delay = delay;
            iterator.<$>duration = duration;
            iterator.<$>targetLocalRotationZ = targetLocalRotationZ;
            iterator.<$>targetLocalScale = targetLocalScale;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator blindCardChoiceUtilRoutine_RevelCard(RewardGalleryCell cell, TransformAnimation ta, float duration, bool isSelected)
        {
            <blindCardChoiceUtilRoutine_RevelCard>c__Iterator147 iterator = new <blindCardChoiceUtilRoutine_RevelCard>c__Iterator147();
            iterator.ta = ta;
            iterator.duration = duration;
            iterator.cell = cell;
            iterator.isSelected = isSelected;
            iterator.<$>ta = ta;
            iterator.<$>duration = duration;
            iterator.<$>cell = cell;
            iterator.<$>isSelected = isSelected;
            iterator.<>f__this = this;
            return iterator;
        }

        private void cleanupCells()
        {
            for (int i = this.RewardGalleryCells.Count - 1; i >= 0; i--)
            {
                RewardGalleryCell item = this.RewardGalleryCells[i];
                this.RewardGalleryCells.Remove(item);
                PlayerView.Binder.RewardGalleryCellPool.returnObject(item, item.Type);
            }
        }

        public void cleanUpForReuse()
        {
            this.cleanupCells();
            this.RectTm.localPosition = Vector3.zero;
            this.ChestContentRoot.stopAll();
            this.ChestContentLeft.stopAll();
            this.ChestContentMiddle.stopAll();
            this.ChestContentRight.stopAll();
            UnityUtils.StopCoroutine(this, ref this.m_petAnimationRoutine);
            UnityUtils.StopCoroutine(this, ref this.m_cardChoiceRoutine);
            UnityUtils.StopCoroutine(this, ref this.m_genericSpreadRoutine);
            this.FinalizationRunning = false;
        }

        [DebuggerHidden]
        public IEnumerator finalizationRoutine(float displayRewardContentForSeconds)
        {
            <finalizationRoutine>c__Iterator142 iterator = new <finalizationRoutine>c__Iterator142();
            iterator.displayRewardContentForSeconds = displayRewardContentForSeconds;
            iterator.<$>displayRewardContentForSeconds = displayRewardContentForSeconds;
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator genericSpreadRoutine()
        {
            <genericSpreadRoutine>c__Iterator144 iterator = new <genericSpreadRoutine>c__Iterator144();
            iterator.<>f__this = this;
            return iterator;
        }

        private void getAllAdWatchCompleteCallback(List<GameLogic.Reward> rewards, bool awardReward, int numPurchases)
        {
            if (((rewards != null) && (rewards.Count > 0)) && awardReward)
            {
                for (int i = 0; i < rewards.Count; i++)
                {
                    GameLogic.Reward reward = rewards[i];
                    for (int j = 0; j < reward.TournamentUpgradeReward.Choices.Count; j++)
                    {
                        reward.TournamentUpgradeReward.Choices[j].Selected = true;
                    }
                }
            }
            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            this.m_cardChoiceRoutinePendingClickFlag = false;
        }

        private RewardGalleryCell getCellForTournamentUpgradeChoice(int idx)
        {
            for (int i = 0; i < this.RewardGalleryCells.Count; i++)
            {
                if (idx == ((int) this.RewardGalleryCells[i].ActiveContent.Obj))
                {
                    return this.RewardGalleryCells[i];
                }
            }
            return null;
        }

        public static string GetShopEntryDropTitle(ShopEntry shopEntry, [Optional, DefaultParameterValue(null)] GameLogic.Reward reward)
        {
            double num2;
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            switch (shopEntry.Type)
            {
                case ShopEntryType.CoinBundle:
                    if (reward == null)
                    {
                        num2 = ConfigShops.CalculateCoinBundleSize(player, shopEntry.Id, 1);
                        break;
                    }
                    num2 = reward.getTotalCoinAmount();
                    break;

                case ShopEntryType.IapDiamonds:
                {
                    double num8 = shopEntry.BuyResourceAmounts[ResourceType.Diamond];
                    return ("+" + num8.ToString("0"));
                }
                case ShopEntryType.TokenBundle:
                {
                    double v = ConfigShops.CalculateTokenBundleSize(player, shopEntry.Id);
                    return ("+" + MenuHelpers.BigValueToString(v));
                }
                case ShopEntryType.ReviveBundle:
                {
                    double num4 = ConfigShops.CalculateReviveBundleSize(shopEntry.Id);
                    return ("+" + num4.ToString("0"));
                }
                case ShopEntryType.FrenzyBundle:
                {
                    double num5 = ConfigShops.CalculateFrenzyBundleSize(shopEntry.Id);
                    return ("+" + num5.ToString("0"));
                }
                case ShopEntryType.DustBundle:
                {
                    double num6 = ConfigShops.CalculateDustBundleSize(activeCharacter, shopEntry.Id);
                    return ("+" + MenuHelpers.BigValueToString(num6));
                }
                case ShopEntryType.DiamondBundle:
                {
                    double num = ConfigShops.CalculateDiamondBundleSize(player, shopEntry.Id);
                    return ("+" + MenuHelpers.BigValueToString(num));
                }
                case ShopEntryType.XpBundle:
                    return ("+" + _.L(ConfigLoca.RESOURCES_XP, null, false));

                case ShopEntryType.BossBundle:
                {
                    double num7 = ConfigShops.CalculateBossBundleSize(shopEntry.Id);
                    return ("+" + num7.ToString("0"));
                }
                case ShopEntryType.MegaBoxBundle:
                    return string.Empty;

                default:
                    return StringExtensions.ToUpperLoca(_.L(shopEntry.Title, null, false));
            }
            return ("+" + MenuHelpers.BigValueToString(num2));
        }

        public void initialize(RewardCeremonyMenu menu, UnityEngine.Canvas canvas, Action<MenuTreasureChest> clickCallback, GameLogic.Reward reward, bool openAtStart, RewardCeremonyEntry ceremonyEntry, LeaderboardEntry lbe, bool consumeRewardsAfterChestOpen)
        {
            RewardGalleryCell.Content content6;
            this.CeremonyMenu = menu;
            this.Canvas = canvas;
            this.m_clicked = false;
            this.m_clickCallback = clickCallback;
            this.m_cardChoiceOkayButtonClicked = false;
            this.ButtonOkay.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_DONE, null, false));
            this.Reward = reward;
            this.OpenAtStart = openAtStart;
            if (!this.OpenAtStart && (this.Reward.ChestType == ChestType.NONE))
            {
                UnityEngine.Debug.LogError("Reward specified as not-open-at-start without a chest");
                this.OpenAtStart = true;
            }
            this.CeremonyEntry = ceremonyEntry;
            this.Interactable = false;
            this.RewardClaimed = false;
            this.IsOpen = this.OpenAtStart;
            this.ChestRootTm.gameObject.SetActive(true);
            this.ChestContentRoot.gameObject.SetActive(true);
            this.FinalizationRoutineSkippedByPlayer = false;
            this.m_getAllPaidWithAd = false;
            this.ButtonGetAll.gameObject.SetActive(false);
            this.ButtonOkay.gameObject.SetActive(false);
            for (int i = 0; i < this.GetCardButtons.Count; i++)
            {
                this.GetCardButtons[i].gameObject.SetActive(false);
            }
            if (this.OpenAtStart)
            {
                this.ChestIcon.enabled = false;
                this.ChestBase.enabled = false;
                this.ChestTop.enabled = false;
                this.ChestLock.enabled = false;
                this.ChestGlowEffect.gameObject.SetActive(false);
            }
            else
            {
                ChestBlueprint blueprint = ConfigUi.CHEST_BLUEPRINTS[this.Reward.getVisualChestType()];
                if (blueprint.SoloSprite != null)
                {
                    this.ChestBase.enabled = false;
                    this.ChestIcon.enabled = true;
                    this.ChestIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(blueprint.SoloSprite);
                    this.ChestIcon.rectTransform.sizeDelta = (Vector2) (this.m_origSizeDeltas[this.ChestIcon.rectTransform] * blueprint.CeremonyScale);
                }
                else
                {
                    this.ChestBase.enabled = true;
                    this.ChestIcon.enabled = false;
                    this.ChestBase.sprite = PlayerView.Binder.SpriteResources.getSprite(blueprint.BaseSprite);
                    this.ChestBase.rectTransform.localPosition = this.m_origLocalPositions[this.ChestBase.rectTransform];
                    this.ChestBase.rectTransform.sizeDelta = (Vector2) (this.m_origSizeDeltas[this.ChestBase.rectTransform] * blueprint.CeremonyScale);
                }
                if (blueprint.TopSprite != null)
                {
                    this.ChestTop.enabled = true;
                    this.ChestTop.rectTransform.localPosition = this.m_origLocalPositions[this.ChestTop.rectTransform];
                    this.ChestTop.rectTransform.sizeDelta = (Vector2) (this.m_origSizeDeltas[this.ChestTop.rectTransform] * blueprint.CeremonyScale);
                }
                else
                {
                    this.ChestTop.enabled = false;
                }
                if (blueprint.LockSprite != null)
                {
                    this.ChestLock.enabled = true;
                    this.ChestLock.rectTransform.localPosition = this.m_origLocalPositions[this.ChestLock.rectTransform];
                    this.ChestLock.rectTransform.sizeDelta = (Vector2) (this.m_origSizeDeltas[this.ChestLock.rectTransform] * blueprint.CeremonyScale);
                }
                else
                {
                    this.ChestLock.enabled = false;
                }
                this.ChestGlowEffect.gameObject.SetActive(true);
            }
            this.RootHoverTransformAnimation.cacheStartingOrientation();
            this.RootHoverTransformAnimation.Enabled = !openAtStart;
            this.RootHoverTransformAnimation.loadParameterSet(IdleHoverTransformParams);
            this.ChestLockHoverTransformAnimation.cacheStartingOrientation();
            this.ChestLockHoverTransformAnimation.Enabled = false;
            this.ChestLockHoverTransformAnimation.loadParameterSet(ShakyHoverTransformParams);
            this.ChestTopHoverTransformAnimation.cacheStartingOrientation();
            this.ChestTopHoverTransformAnimation.Enabled = false;
            this.ChestTopHoverTransformAnimation.loadParameterSet(ShakyHoverTransformParams);
            this.cleanupCells();
            this.ChestContentRoot.stopAll();
            this.ChestContentLeft.stopAll();
            this.ChestContentMiddle.stopAll();
            this.ChestContentRight.stopAll();
            if (this.usesPetAnimationFlow() && (this.Reward.Pets.Count > 1))
            {
                this.ChestContentRoot.RectTm.localScale = (Vector3) (Vector3.one * 0.75f);
            }
            else
            {
                this.ChestContentRoot.RectTm.localScale = Vector3.one;
            }
            this.ChestContentLeft.RectTm.localRotation = Quaternion.identity;
            this.ChestContentLeft.RectTm.anchoredPosition = Vector2.zero;
            this.ChestContentLeft.RectTm.localScale = Vector3.one;
            this.ChestContentMiddle.RectTm.localRotation = Quaternion.identity;
            this.ChestContentMiddle.RectTm.anchoredPosition = Vector2.zero;
            this.ChestContentMiddle.RectTm.localScale = Vector3.one;
            this.ChestContentRight.RectTm.localRotation = Quaternion.identity;
            this.ChestContentRight.RectTm.anchoredPosition = Vector2.zero;
            this.ChestContentRight.RectTm.localScale = Vector3.one;
            this.m_cardChoiceRoutinePendingClickFlag = false;
            this.m_lastClickedCell = null;
            if (this.Reward.ShopEntryDrops.Count > 0)
            {
                int num2 = 0;
                int num3 = 0;
                for (int k = 0; k < this.Reward.ShopEntryDrops.Count; k++)
                {
                    ShopEntry shopEntry = ConfigShops.GetShopEntry(this.Reward.ShopEntryDrops[k]);
                    if (shopEntry.Type == ShopEntryType.MysteryItem)
                    {
                        ItemInstance instance = this.Reward.ItemDrops[num3++];
                        content6 = new RewardGalleryCell.Content();
                        content6.Sprite = new SpriteAtlasEntry("Menu", instance.Item.SpriteId);
                        content6.StarRank = instance.Rarity;
                        content6.LeaderboardEntry = lbe;
                        RewardGalleryCell.Content content = content6;
                        this.addRewardGalleryCellToGrid(content, openAtStart);
                    }
                    else if ((shopEntry.Type == ShopEntryType.PetBundle) || (shopEntry.Type == ShopEntryType.PetBox))
                    {
                        this.addPetRewardGalleryCellToGrid(this.Reward.Pets[num2++], openAtStart, lbe, consumeRewardsAfterChestOpen);
                    }
                    else
                    {
                        content6 = new RewardGalleryCell.Content();
                        content6.Sprite = new SpriteAtlasEntry("Menu", shopEntry.Sprite.SpriteId);
                        content6.Text = GetShopEntryDropTitle(shopEntry, this.Reward);
                        content6.LeaderboardEntry = lbe;
                        RewardGalleryCell.Content content2 = content6;
                        this.addRewardGalleryCellToGrid(content2, openAtStart);
                    }
                }
            }
            else if (this.Reward.isEmpty() && !string.IsNullOrEmpty(this.Reward.Sprite))
            {
                content6 = new RewardGalleryCell.Content();
                content6.Sprite = new SpriteAtlasEntry("Menu", this.Reward.Sprite);
                content6.LeaderboardEntry = lbe;
                RewardGalleryCell.Content content3 = content6;
                this.addRewardGalleryCellToGrid(content3, openAtStart);
            }
            else if (this.usesCardChoiceFlow())
            {
                for (int m = 0; m < this.Reward.TournamentUpgradeReward.Choices.Count; m++)
                {
                    TournamentUpgradeReward.Entry entry2 = this.Reward.TournamentUpgradeReward.Choices[m];
                    TournamentUpgrade tournamentUpgrade = App.Binder.ConfigMeta.GetTournamentUpgrade(entry2.TourmanentUpgradeId);
                    ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[tournamentUpgrade.PerkType];
                    float num6 = App.Binder.ConfigMeta.GetTournamentUpgradeModifier(entry2.TourmanentUpgradeId, entry2.IsEpicUpgrade, this.Reward.TournamentUpgradeReward.NumMilestonesCompleted);
                    content6 = new RewardGalleryCell.Content();
                    content6.Type = RewardGalleryCellType.RewardGalleryCellTournamentCard;
                    content6.Sprite = new SpriteAtlasEntry("Menu", tournamentUpgrade.getSpriteId());
                    content6.Text = MenuHelpers.BigModifierToString(num6, true) + "\n" + StringExtensions.ToUpperLoca(_.L(data.ShortDescription, null, false));
                    RewardGalleryCell.Content content4 = content6;
                    RewardGalleryCell cell = this.addRewardGalleryCellToGrid(content4, true);
                    cell.CornerTextBg.gameObject.SetActive(true);
                    if (entry2.IsEpicUpgrade)
                    {
                        cell.IconBg.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_card_gold");
                        cell.CornerTextBg.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_kulmajuusto_gold");
                        cell.Tier1Fx.SetActive(ConfigTournaments.TOURNAMENT_CARD_SNEAK_PEAK_ENABLED);
                        cell.Tier2Fx.SetActive(ConfigTournaments.TOURNAMENT_CARD_SNEAK_PEAK_ENABLED);
                    }
                    else
                    {
                        cell.IconBg.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_card_silver");
                        cell.CornerTextBg.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_kulmajuusto_silver");
                        cell.Tier1Fx.SetActive(false);
                        cell.Tier2Fx.SetActive(false);
                    }
                }
                this.RootHoverTransformAnimation.Enabled = false;
                this.ChestLockHoverTransformAnimation.Enabled = false;
                this.ChestTopHoverTransformAnimation.Enabled = false;
                this.ChestGlowEffect.gameObject.SetActive(false);
                this.ChestRootTm.gameObject.SetActive(false);
            }
            else if (this.usesPetAnimationFlow())
            {
                for (int n = 0; n < this.Reward.Pets.Count; n++)
                {
                    this.addPetRewardGalleryCellToGrid(this.Reward.Pets[n], openAtStart, lbe, consumeRewardsAfterChestOpen);
                }
            }
            else
            {
                RewardGalleryCell.Content content5 = RewardGalleryCell.CreateDefaultContentForReward(this.Reward, false, lbe);
                this.addRewardGalleryCellToGrid(content5, openAtStart);
            }
            for (int j = 0; j < this.m_chestAnimClipNames.Count; j++)
            {
                string str = this.m_chestAnimClipNames[j];
                this.ChestAnimator[str].normalizedTime = 0f;
                this.ChestAnimator[str].wrapMode = WrapMode.Once;
                this.ChestAnimator[str].weight = 1f;
                this.ChestAnimator[str].speed = 0f;
                this.ChestAnimator[str].enabled = true;
            }
        }

        public void onClick()
        {
            this.m_clicked = true;
            if (this.Interactable && !this.RewardClaimed)
            {
                if (!this.usesCardChoiceFlow())
                {
                    if (!this.OpenAtStart)
                    {
                        this.RootHoverTransformAnimation.Enabled = false;
                        this.ChestLockHoverTransformAnimation.Enabled = false;
                        this.ChestTopHoverTransformAnimation.Enabled = false;
                        this.ChestGlowEffect.gameObject.SetActive(false);
                        this.ChestRootTm.gameObject.SetActive(false);
                        UnityUtils.StopCoroutine(this, ref this.m_petAnimationRoutine);
                        for (int i = 0; i < this.NumFilledChestItems; i++)
                        {
                            this.RewardGalleryCells[i].gameObject.SetActive(true);
                        }
                        if (this.usesPetAnimationFlow())
                        {
                            this.m_petAnimationRoutine = UnityUtils.StartCoroutine(this, this.petAnimationRoutine());
                        }
                        else if (ConfigUi.CHEST_BLUEPRINTS[this.Reward.getVisualChestType()].isMultiPart())
                        {
                            string str = this.m_chestAnimClipNames[0];
                            this.ChestAnimator[str].enabled = true;
                            this.ChestAnimator[str].speed = 1f / Time.timeScale;
                        }
                        else
                        {
                            this.ChestIcon.enabled = false;
                        }
                        this.IsOpen = true;
                    }
                    if (this.Reward.isWrappedInsideChest())
                    {
                        if (ConfigMeta.IsRetirementChest(this.Reward.ChestType) && (this.Reward.ChestType != ChestType.RetirementTrigger))
                        {
                            PlayerView.Binder.EffectSystem.playEffectStatic(this.RectTm.position, EffectType.UI_BoxPlosion, -1f, true, 1f, null);
                        }
                        else if (ConfigUi.CHEST_BLUEPRINTS[this.Reward.getVisualChestType()].isMultiPart())
                        {
                            PlayerView.Binder.EffectSystem.playEffectStatic(this.RectTm.position, EffectType.UI_ChestPlosion, -1f, true, 1f, null);
                        }
                        else
                        {
                            PlayerView.Binder.EffectSystem.playEffectStatic(this.RectTm.position, EffectType.UI_Bling, -1f, true, 1f, null);
                        }
                    }
                    else
                    {
                        PlayerView.Binder.EffectSystem.playEffectStatic(this.RectTm.position, EffectType.UI_Bling, -1f, true, 1f, null);
                    }
                }
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_RewardClaim, (float) 0f);
                this.RewardClaimed = true;
                this.m_clickCallback(this);
            }
        }

        public void onGetAllButtonClicked()
        {
            if (this.m_cardChoiceRoutinePendingClickFlag && !PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                bool flag = false;
                if (player.Tournaments.hasTournamentSelected())
                {
                    flag = player.Tournaments.SelectedTournament.cardPackGetAllAdViewAllowed();
                }
                if (flag && Service.Binder.AdsSystem.adReady())
                {
                    FullscreenAdMenu.InputParameters parameters2 = new FullscreenAdMenu.InputParameters();
                    parameters2.AdZoneId = AdsSystem.ADS_VENDOR_ZONE;
                    parameters2.AdCategory = AdsData.Category.TOURNAMENT_CARDS;
                    parameters2.Reward = this.Reward;
                    parameters2.CompleteCallback = new Action<List<GameLogic.Reward>, bool, int>(this.getAllAdWatchCompleteCallback);
                    FullscreenAdMenu.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.FullscreenAdMenu, MenuContentType.NONE, parameter, 0f, false, true);
                    this.m_getAllPaidWithAd = true;
                    if (player.Tournaments.hasTournamentSelected())
                    {
                        player.Tournaments.SelectedTournament.LastCardPackAdViewTimestamp = Service.Binder.ServerTime.GameTime;
                    }
                }
                else
                {
                    CmdGainResources.ExecuteStatic(player, ResourceType.Diamond, (double) -App.Binder.ConfigMeta.TOURNAMENT_CARD_PRICE_NORMAL, false, string.Empty, null);
                    for (int j = 0; j < this.Reward.TournamentUpgradeReward.Choices.Count; j++)
                    {
                        this.Reward.TournamentUpgradeReward.Choices[j].Selected = true;
                    }
                    this.m_cardChoiceRoutinePendingClickFlag = false;
                }
                this.CeremonyMenu.DescriptionCanvasGroup.setTransparent(true);
                this.ButtonGetAll.gameObject.SetActive(false);
                this.ButtonOkay.gameObject.SetActive(false);
                for (int i = 0; i < this.GetCardButtons.Count; i++)
                {
                    this.GetCardButtons[i].gameObject.SetActive(false);
                }
            }
        }

        public void onGetCardButtonClicked(int idx)
        {
            if (this.m_cardChoiceRoutinePendingClickFlag && !PlayerView.Binder.MenuSystem.InTransition)
            {
                this.m_lastClickedCell = this.RewardGalleryCells[idx];
                int num = (int) this.m_lastClickedCell.ActiveContent.Obj;
                Player player = GameLogic.Binder.GameState.Player;
                double price = this.Reward.TournamentUpgradeReward.getPriceForChoice(num);
                double num3 = player.getResourceAmount(ResourceType.Diamond);
                if (price <= num3)
                {
                    this.Reward.TournamentUpgradeReward.Choices[num].Selected = true;
                    CmdGainResources.ExecuteStatic(player, ResourceType.Diamond, -price, false, string.Empty, null);
                    Service.Binder.TrackingSystem.sendPurchaseItemEvent(player, this.Reward.TournamentUpgradeReward.getTrackingIdForChoice(num), "card_pack", ResourceType.Diamond, price, 1, 0);
                    this.m_cardChoiceRoutinePendingClickFlag = false;
                    this.ButtonOkay.gameObject.SetActive(false);
                    for (int i = 0; i < this.GetCardButtons.Count; i++)
                    {
                        this.GetCardButtons[i].gameObject.SetActive(false);
                    }
                    this.CeremonyMenu.DescriptionCanvasGroup.setTransparent(true);
                }
                else
                {
                    double num5 = Math.Ceiling(price - num3);
                    MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                    parameters2.PathToShop = PathToShopType.CardPack;
                    parameters2.MenuContentParams = num5;
                    MiniPopupMenu.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.ShopOfferMiniPopupContent, parameter, 0f, false, true);
                }
            }
        }

        public void onOkayButtonClicked()
        {
            if (this.m_cardChoiceRoutinePendingClickFlag)
            {
                this.CeremonyMenu.DescriptionCanvasGroup.setTransparent(true);
                this.ButtonGetAll.gameObject.SetActive(false);
                this.ButtonOkay.gameObject.SetActive(false);
                for (int i = 0; i < this.GetCardButtons.Count; i++)
                {
                    this.GetCardButtons[i].gameObject.SetActive(false);
                }
                this.m_cardChoiceRoutinePendingClickFlag = false;
                this.m_cardChoiceOkayButtonClicked = true;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!this.RewardClaimed)
            {
                this.scaleUp();
                this.m_dragging = true;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (this.m_dragging)
            {
                this.scaleUp();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (this.m_dragging)
            {
                this.scaleDown();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this.scaleDown();
            this.m_dragging = false;
            this.onClick();
        }

        private void onRewardGalleryCellClick(RewardGalleryCell cell)
        {
            this.m_lastClickedCell = cell;
            if (this.usesCardChoiceFlow() && cell.ActiveContent.Interactable)
            {
                int num = (int) cell.ActiveContent.Obj;
                this.Reward.TournamentUpgradeReward.Choices[num].Selected = true;
                this.ButtonOkay.gameObject.SetActive(false);
                for (int i = 0; i < this.GetCardButtons.Count; i++)
                {
                    this.GetCardButtons[i].gameObject.SetActive(false);
                }
                this.CeremonyMenu.DescriptionCanvasGroup.setTransparent(true);
                if (this.m_cardChoiceRoutinePendingClickFlag)
                {
                    this.setInteractable(false);
                    this.m_cardChoiceRoutinePendingClickFlag = false;
                    return;
                }
            }
            this.onClick();
        }

        public void onShow()
        {
            if ((this.CeremonyEntry == ConfigUi.CeremonyEntries.PET_UNLOCK) || (this.CeremonyEntry == ConfigUi.CeremonyEntries.PET_LEVEL_UP))
            {
                for (int i = 0; i < this.RewardGalleryCells.Count; i++)
                {
                    if (this.RewardGalleryCells[i].ActiveContent.AnimateStarGain)
                    {
                        this.RewardGalleryCells[i].animateStarGain();
                    }
                }
            }
            if (this.usesCardChoiceFlow())
            {
                if (ConfigTournaments.TOURNAMENT_CARD_SNEAK_PEAK_ENABLED)
                {
                    this.m_cardChoiceRoutine = UnityUtils.StartCoroutine(this, this.sneakPeakCardChoiceRoutine());
                }
                else
                {
                    this.m_cardChoiceRoutine = UnityUtils.StartCoroutine(this, this.blindCardChoiceRoutine());
                }
            }
            else if (this.OpenAtStart && this.usesPetAnimationFlow())
            {
                this.m_petAnimationRoutine = UnityUtils.StartCoroutine(this, this.petAnimationRoutine());
            }
            else if ((this.RewardGalleryCells.Count == 2) || (this.RewardGalleryCells.Count == 3))
            {
                this.m_genericSpreadRoutine = UnityUtils.StartCoroutine(this, this.genericSpreadRoutine());
            }
        }

        [DebuggerHidden]
        private IEnumerator petAnimationRoutine()
        {
            <petAnimationRoutine>c__Iterator143 iterator = new <petAnimationRoutine>c__Iterator143();
            iterator.<>f__this = this;
            return iterator;
        }

        private void refreshGetCardButtons()
        {
            for (int i = 0; i < this.GetCardButtons.Count; i++)
            {
                PrettyButton button = this.GetCardButtons[i];
                if (App.Binder.ConfigMeta.TOURNAMENT_DIRECT_CARD_PURCHASING_ENABLED)
                {
                    int idx = (int) this.RewardGalleryCells[i].ActiveContent.Obj;
                    if (this.Reward.TournamentUpgradeReward.Choices[idx].Selected)
                    {
                        button.gameObject.SetActive(false);
                    }
                    else
                    {
                        button.CornerText.text = this.Reward.TournamentUpgradeReward.getPriceForChoice(idx).ToString("0");
                        button.Button.interactable = true;
                        button.Bg.material = !button.Button.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
                        button.gameObject.SetActive(true);
                    }
                }
                else
                {
                    button.gameObject.SetActive(false);
                }
            }
        }

        private void scaleDown()
        {
            if (this.m_upScaled)
            {
                TransformAnimationTask animationTask = new TransformAnimationTask(this.RectTm, SCALE_DURATION, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                animationTask.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                this.TransformAnimation.stopAll();
                this.TransformAnimation.addTask(animationTask);
                this.RootHoverTransformAnimation.loadParameterSet(IdleHoverTransformParams);
                this.ChestLockHoverTransformAnimation.Enabled = false;
                this.ChestTopHoverTransformAnimation.Enabled = false;
                this.m_upScaled = false;
            }
        }

        private void scaleUp()
        {
            if (!this.m_upScaled)
            {
                TransformAnimationTask animationTask = new TransformAnimationTask(this.RectTm, SCALE_DURATION, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                animationTask.scale((Vector3) (Vector3.one * TARGET_UPSCALE), true, Easing.Function.OUT_BOUNCE);
                this.TransformAnimation.stopAll();
                this.TransformAnimation.addTask(animationTask);
                this.RootHoverTransformAnimation.loadParameterSet(RumblyHoverTransformParams);
                this.ChestLockHoverTransformAnimation.Enabled = true;
                this.ChestTopHoverTransformAnimation.Enabled = true;
                this.m_upScaled = true;
            }
        }

        private void setInteractable(bool interactable)
        {
            for (int i = 0; i < this.RewardGalleryCells.Count; i++)
            {
                this.RewardGalleryCells[i].ActiveContent.Interactable = interactable;
                this.RewardGalleryCells[i].Button.interactable = interactable;
            }
            this.Interactable = interactable;
            this.ButtonGetAll.Button.interactable = interactable;
            this.ButtonOkay.Button.interactable = interactable;
            for (int j = 0; j < this.GetCardButtons.Count; j++)
            {
                this.GetCardButtons[j].Button.interactable = interactable;
            }
        }

        [DebuggerHidden]
        private IEnumerator sneakPeakCardChoiceRoutine()
        {
            <sneakPeakCardChoiceRoutine>c__Iterator145 iterator = new <sneakPeakCardChoiceRoutine>c__Iterator145();
            iterator.<>f__this = this;
            return iterator;
        }

        public bool usesCardChoiceFlow()
        {
            return ((this.Reward.ChestType == ChestType.TournamentCards) && (this.Reward.TournamentUpgradeReward != null));
        }

        public bool usesPetAnimationFlow()
        {
            return (((this.Reward.ChestType == ChestType.RewardBoxMulti) || (this.Reward.ChestType == ChestType.PetBoxSmall)) || (this.Reward.ChestType == ChestType.TournamentContributorReward));
        }

        public UnityEngine.Canvas Canvas
        {
            [CompilerGenerated]
            get
            {
                return this.<Canvas>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Canvas>k__BackingField = value;
            }
        }

        public RewardCeremonyEntry CeremonyEntry
        {
            [CompilerGenerated]
            get
            {
                return this.<CeremonyEntry>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CeremonyEntry>k__BackingField = value;
            }
        }

        public RewardCeremonyMenu CeremonyMenu
        {
            [CompilerGenerated]
            get
            {
                return this.<CeremonyMenu>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CeremonyMenu>k__BackingField = value;
            }
        }

        private HoverTransformAnimation ChestLockHoverTransformAnimation
        {
            [CompilerGenerated]
            get
            {
                return this.<ChestLockHoverTransformAnimation>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ChestLockHoverTransformAnimation>k__BackingField = value;
            }
        }

        private HoverTransformAnimation ChestTopHoverTransformAnimation
        {
            [CompilerGenerated]
            get
            {
                return this.<ChestTopHoverTransformAnimation>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ChestTopHoverTransformAnimation>k__BackingField = value;
            }
        }

        public bool FinalizationRoutineSkippedByPlayer
        {
            [CompilerGenerated]
            get
            {
                return this.<FinalizationRoutineSkippedByPlayer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<FinalizationRoutineSkippedByPlayer>k__BackingField = value;
            }
        }

        public bool FinalizationRunning
        {
            [CompilerGenerated]
            get
            {
                return this.<FinalizationRunning>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<FinalizationRunning>k__BackingField = value;
            }
        }

        public bool Interactable
        {
            [CompilerGenerated]
            get
            {
                return this.<Interactable>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Interactable>k__BackingField = value;
            }
        }

        public bool IsOpen
        {
            [CompilerGenerated]
            get
            {
                return this.<IsOpen>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<IsOpen>k__BackingField = value;
            }
        }

        public int NumFilledChestItems
        {
            get
            {
                return this.RewardGalleryCells.Count;
            }
        }

        public bool OpenAtStart
        {
            [CompilerGenerated]
            get
            {
                return this.<OpenAtStart>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<OpenAtStart>k__BackingField = value;
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

        public GameLogic.Reward Reward
        {
            [CompilerGenerated]
            get
            {
                return this.<Reward>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Reward>k__BackingField = value;
            }
        }

        public bool RewardClaimed
        {
            [CompilerGenerated]
            get
            {
                return this.<RewardClaimed>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RewardClaimed>k__BackingField = value;
            }
        }

        public List<RewardGalleryCell> RewardGalleryCells
        {
            [CompilerGenerated]
            get
            {
                return this.<RewardGalleryCells>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RewardGalleryCells>k__BackingField = value;
            }
        }

        private HoverTransformAnimation RootHoverTransformAnimation
        {
            [CompilerGenerated]
            get
            {
                return this.<RootHoverTransformAnimation>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<RootHoverTransformAnimation>k__BackingField = value;
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

        [CompilerGenerated]
        private sealed class <blindCardChoiceRoutine>c__Iterator146 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuTreasureChest <>f__this;
            internal bool <adAllowed>__13;
            internal RewardGalleryCell <cell>__11;
            internal RewardGalleryCell <cell>__17;
            internal RewardGalleryCell <cell>__21;
            internal RewardGalleryCell <cell>__25;
            internal RewardGalleryCell <cell>__29;
            internal TournamentUpgradeReward.Entry <choice>__28;
            internal TransformAnimation <clickedCellParentTa>__7;
            internal int <i>__10;
            internal int <i>__16;
            internal int <i>__19;
            internal int <i>__20;
            internal int <i>__23;
            internal int <i>__24;
            internal int <i>__27;
            internal int <i>__6;
            internal int <i>__8;
            internal IEnumerator <ie>__0;
            internal TransformAnimationTask <leftTt>__2;
            internal TransformAnimationTask <leftTt>__5;
            internal TransformAnimationTask <middleTt>__1;
            internal TransformAnimationTask <middleTt>__4;
            internal TransformAnimation <parentTa>__12;
            internal TransformAnimation <parentTa>__22;
            internal TransformAnimation <parentTa>__26;
            internal Player <player>__14;
            internal double <price>__15;
            internal TransformAnimationTask <rightTt>__3;
            internal bool <running>__18;
            internal TransformAnimationTask <tt>__9;

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
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.25f);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0380;

                    case 3:
                        goto Label_03C0;

                    case 4:
                        goto Label_04F6;

                    case 5:
                        goto Label_051E;

                    case 6:
                        goto Label_058E;

                    case 7:
                        goto Label_0646;

                    case 8:
                        goto Label_0688;

                    case 9:
                        goto Label_08BD;

                    case 10:
                        goto Label_092B;

                    case 11:
                        if (this.<running>__18)
                        {
                            goto Label_0A10;
                        }
                        this.<>f__this.m_enumeratorList.Clear();
                        this.<i>__20 = 0;
                        while (this.<i>__20 < this.<>f__this.RewardGalleryCells.Count)
                        {
                            this.<cell>__21 = this.<>f__this.RewardGalleryCells[this.<i>__20];
                            if (this.<cell>__21 != this.<>f__this.m_lastClickedCell)
                            {
                                this.<parentTa>__22 = this.<cell>__21.RectTm.parent.GetComponent<TransformAnimation>();
                                this.<>f__this.m_enumeratorList.Add(this.<>f__this.blindCardChoiceUtilRoutine_RevelCard(this.<cell>__21, this.<parentTa>__22, 0.05f, true));
                            }
                            this.<i>__20++;
                        }
                        goto Label_0B55;

                    case 12:
                        if (this.<running>__18)
                        {
                            goto Label_0B55;
                        }
                        this.<i>__24 = 0;
                        while (this.<i>__24 < this.<>f__this.RewardGalleryCells.Count)
                        {
                            this.<cell>__25 = this.<>f__this.RewardGalleryCells[this.<i>__24];
                            this.<parentTa>__26 = this.<cell>__25.RectTm.parent.GetComponent<TransformAnimation>();
                            this.<tt>__9 = new TransformAnimationTask(this.<parentTa>__26.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                            this.<tt>__9.scale(Vector3.one, true, Easing.Function.OUT_CUBIC);
                            this.<parentTa>__26.stopAll();
                            this.<parentTa>__26.addTask(this.<tt>__9);
                            this.<i>__24++;
                        }
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(1.25f);
                        goto Label_0CC0;

                    case 13:
                        goto Label_0CC0;

                    default:
                        goto Label_0E05;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_0E07;
                }
                if (this.<>f__this.RewardGalleryCells.Count == 3)
                {
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ChestUnfold, (float) 0f);
                    this.<middleTt>__1 = new TransformAnimationTask(this.<>f__this.ChestContentMiddle.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<middleTt>__1.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                    this.<middleTt>__1.translateToAnchoredPos(new Vector2(0f, -50f), Easing.Function.IN_CUBIC);
                    this.<>f__this.ChestContentMiddle.addTask(this.<middleTt>__1);
                    this.<leftTt>__2 = new TransformAnimationTask(this.<>f__this.ChestContentLeft.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<leftTt>__2.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                    this.<leftTt>__2.translateToAnchoredPos(new Vector2(-380f, -50f), Easing.Function.IN_CUBIC);
                    this.<>f__this.ChestContentLeft.addTask(this.<leftTt>__2);
                    this.<rightTt>__3 = new TransformAnimationTask(this.<>f__this.ChestContentRight.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<rightTt>__3.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                    this.<rightTt>__3.translateToAnchoredPos(new Vector2(380f, -50f), Easing.Function.IN_CUBIC);
                    this.<>f__this.ChestContentRight.addTask(this.<rightTt>__3);
                }
                else if (this.<>f__this.RewardGalleryCells.Count == 2)
                {
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ChestUnfold, (float) 0f);
                    this.<middleTt>__4 = new TransformAnimationTask(this.<>f__this.ChestContentMiddle.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<middleTt>__4.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                    this.<middleTt>__4.translateToAnchoredPos(new Vector2(225f, -50f), Easing.Function.IN_CUBIC);
                    this.<>f__this.ChestContentMiddle.addTask(this.<middleTt>__4);
                    this.<leftTt>__5 = new TransformAnimationTask(this.<>f__this.ChestContentLeft.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<leftTt>__5.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                    this.<leftTt>__5.translateToAnchoredPos(new Vector2(-225f, -50f), Easing.Function.IN_CUBIC);
                    this.<>f__this.ChestContentLeft.addTask(this.<leftTt>__5);
                }
                this.<i>__6 = 0;
                while (this.<i>__6 < this.<>f__this.RewardGalleryCells.Count)
                {
                    this.<>f__this.RewardGalleryCells[this.<i>__6].HoverTransformAnimation.Enabled = true;
                    this.<i>__6++;
                }
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_0380:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_0E07;
                }
                this.<>f__this.m_cardChoiceRoutinePendingClickFlag = true;
                this.<>f__this.setInteractable(true);
            Label_03C0:
                while (this.<>f__this.m_cardChoiceRoutinePendingClickFlag)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0E07;
                }
                this.<clickedCellParentTa>__7 = this.<>f__this.m_lastClickedCell.RectTm.parent.GetComponent<TransformAnimation>();
                this.<>f__this.setInteractable(false);
                this.<i>__8 = 0;
                while (this.<i>__8 < this.<>f__this.RewardGalleryCells.Count)
                {
                    this.<>f__this.RewardGalleryCells[this.<i>__8].HoverTransformAnimation.Enabled = false;
                    this.<i>__8++;
                }
                this.<tt>__9 = new TransformAnimationTask(this.<clickedCellParentTa>__7.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__9.scale((Vector3) (Vector3.one * 1.1f), true, Easing.Function.OUT_CUBIC);
                this.<clickedCellParentTa>__7.stopAll();
                this.<clickedCellParentTa>__7.addTask(this.<tt>__9);
                this.<ie>__0 = this.<>f__this.blindCardChoiceUtilRoutine_RevelCard(this.<>f__this.m_lastClickedCell, this.<clickedCellParentTa>__7, 0.05f, true);
            Label_04F6:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 4;
                    goto Label_0E07;
                }
            Label_051E:
                while (this.<clickedCellParentTa>__7.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_0E07;
                }
                PlayerView.Binder.EffectSystem.playEffectStatic(this.<>f__this.m_lastClickedCell.RectTm.position, EffectType.UI_Bling, -1f, true, 1f, null);
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.3f);
            Label_058E:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 6;
                    goto Label_0E07;
                }
                this.<i>__10 = 0;
                while (this.<i>__10 < this.<>f__this.RewardGalleryCells.Count)
                {
                    this.<cell>__11 = this.<>f__this.RewardGalleryCells[this.<i>__10];
                    if (this.<cell>__11 == this.<>f__this.m_lastClickedCell)
                    {
                        goto Label_0698;
                    }
                    this.<parentTa>__12 = this.<cell>__11.RectTm.parent.GetComponent<TransformAnimation>();
                    this.<ie>__0 = this.<>f__this.blindCardChoiceUtilRoutine_RevelCard(this.<cell>__11, this.<parentTa>__12, 0.05f, false);
                Label_0646:
                    while (this.<ie>__0.MoveNext())
                    {
                        this.$current = this.<ie>__0.Current;
                        this.$PC = 7;
                        goto Label_0E07;
                    }
                    this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.1f);
                Label_0688:
                    while (this.<ie>__0.MoveNext())
                    {
                        this.$current = this.<ie>__0.Current;
                        this.$PC = 8;
                        goto Label_0E07;
                    }
                Label_0698:
                    this.<i>__10++;
                }
                this.<adAllowed>__13 = true;
                this.<player>__14 = GameLogic.Binder.GameState.Player;
                if (this.<player>__14.Tournaments.hasTournamentSelected())
                {
                    this.<adAllowed>__13 = this.<player>__14.Tournaments.SelectedTournament.cardPackGetAllAdViewAllowed();
                }
                if (this.<adAllowed>__13 && Service.Binder.AdsSystem.adReady())
                {
                    this.<>f__this.ButtonGetAll.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(PlayerView.Binder.SpriteResources.IconWatchVideo);
                    this.<>f__this.ButtonGetAll.CornerText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_WATCH, null, false));
                    this.<>f__this.ButtonGetAll.Button.interactable = true;
                }
                else
                {
                    this.<price>__15 = App.Binder.ConfigMeta.TOURNAMENT_CARD_PRICE_NORMAL;
                    this.<>f__this.ButtonGetAll.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Diamond]);
                    this.<>f__this.ButtonGetAll.CornerText.text = this.<price>__15.ToString("0");
                    this.<>f__this.ButtonGetAll.Button.interactable = GameLogic.Binder.GameState.Player.getResourceAmount(ResourceType.Diamond) >= this.<price>__15;
                }
                this.<>f__this.ButtonGetAll.Bg.material = !this.<>f__this.ButtonGetAll.Button.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
                this.<>f__this.ButtonGetAll.gameObject.SetActive(true);
                this.<>f__this.ButtonOkay.Button.interactable = true;
                this.<>f__this.ButtonOkay.gameObject.SetActive(true);
                this.<>f__this.m_cardChoiceRoutinePendingClickFlag = true;
            Label_08BD:
                while (this.<>f__this.m_cardChoiceRoutinePendingClickFlag)
                {
                    this.$current = null;
                    this.$PC = 9;
                    goto Label_0E07;
                }
                if (this.<>f__this.Reward.TournamentUpgradeReward.countNumSelected() <= 1)
                {
                    goto Label_0CE0;
                }
                if (!this.<>f__this.m_getAllPaidWithAd)
                {
                    goto Label_093B;
                }
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_092B:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 10;
                    goto Label_0E07;
                }
            Label_093B:
                this.<>f__this.m_enumeratorList.Clear();
                this.<i>__16 = 0;
                while (this.<i>__16 < this.<>f__this.RewardGalleryCells.Count)
                {
                    this.<cell>__17 = this.<>f__this.RewardGalleryCells[this.<i>__16];
                    if (this.<cell>__17 != this.<>f__this.m_lastClickedCell)
                    {
                        Vector3? targetLocalScale = null;
                        this.<>f__this.m_enumeratorList.Add(this.<>f__this.blindCardChoiceUtilRoutine_HideCard(this.<cell>__17, this.<cell>__17.RectTm.localRotation.eulerAngles.z, 0.05f, targetLocalScale, 0f));
                    }
                    this.<i>__16++;
                }
            Label_0A10:
                this.<running>__18 = false;
                this.<i>__19 = 0;
                while (this.<i>__19 < this.<>f__this.m_enumeratorList.Count)
                {
                    this.<running>__18 = this.<>f__this.m_enumeratorList[this.<i>__19].MoveNext();
                    this.<i>__19++;
                }
                this.$current = null;
                this.$PC = 11;
                goto Label_0E07;
            Label_0B55:
                this.<running>__18 = false;
                this.<i>__23 = 0;
                while (this.<i>__23 < this.<>f__this.m_enumeratorList.Count)
                {
                    this.<running>__18 = this.<>f__this.m_enumeratorList[this.<i>__23].MoveNext();
                    this.<i>__23++;
                }
                this.$current = null;
                this.$PC = 12;
                goto Label_0E07;
            Label_0CC0:
                while (this.<ie>__0.MoveNext() && !this.<>f__this.m_clicked)
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 13;
                    goto Label_0E07;
                }
            Label_0CE0:
                this.<i>__27 = 0;
                while (this.<i>__27 < this.<>f__this.Reward.TournamentUpgradeReward.Choices.Count)
                {
                    this.<choice>__28 = this.<>f__this.Reward.TournamentUpgradeReward.Choices[this.<i>__27];
                    if (this.<choice>__28.Selected)
                    {
                        this.<cell>__29 = this.<>f__this.getCellForTournamentUpgradeChoice(this.<i>__27);
                        PlayerView.Binder.EffectSystem.playEffectStatic(this.<cell>__29.RectTm.position, EffectType.UI_Bling, -1f, true, 1f, null);
                        PlayerView.Binder.DungeonHud.flyToHudTournamentCardGain(this.<choice>__28.TourmanentUpgradeId, RectTransformUtility.WorldToScreenPoint(this.<>f__this.Canvas.worldCamera, this.<cell>__29.RectTm.position), false, true);
                    }
                    this.<i>__27++;
                }
                this.<>f__this.Interactable = true;
                this.<>f__this.onClick();
                this.<>f__this.m_cardChoiceRoutine = null;
                goto Label_0E05;
                this.$PC = -1;
            Label_0E05:
                return false;
            Label_0E07:
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

        [CompilerGenerated]
        private sealed class <blindCardChoiceUtilRoutine_HideCard>c__Iterator148 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RewardGalleryCell <$>cell;
            internal float <$>delay;
            internal float <$>duration;
            internal float <$>targetLocalRotationZ;
            internal Vector3? <$>targetLocalScale;
            internal IEnumerator <ie>__3;
            internal Vector3 <origLocalScale>__2;
            internal TransformAnimation <ta>__0;
            internal RectTransform <tm>__1;
            internal TransformAnimationTask <tt>__4;
            internal RewardGalleryCell cell;
            internal float delay;
            internal float duration;
            internal float targetLocalRotationZ;
            internal Vector3? targetLocalScale;

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
                        this.<ta>__0 = this.cell.RectTm.parent.GetComponent<TransformAnimation>();
                        this.<tm>__1 = this.<ta>__0.RectTm;
                        this.<origLocalScale>__2 = this.<ta>__0.RectTm.localScale;
                        if (this.delay <= 0f)
                        {
                            goto Label_00BE;
                        }
                        this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(this.delay);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0155;

                    case 3:
                        goto Label_022A;

                    default:
                        goto Label_0241;
                }
                if (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 1;
                    goto Label_0243;
                }
            Label_00BE:
                this.<tt>__4 = new TransformAnimationTask(this.<tm>__1, this.duration * 0.5f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__4.rotate(Quaternion.Euler(0f, 90f, this.targetLocalRotationZ), true, Easing.Function.LINEAR);
                if (this.targetLocalScale.HasValue)
                {
                    this.<tt>__4.scale(this.targetLocalScale.Value, true, Easing.Function.LINEAR);
                }
                this.<ta>__0.addTask(this.<tt>__4);
            Label_0155:
                while (this.<ta>__0.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0243;
                }
                this.cell.Flipside.SetActive(true);
                this.cell.Tier1Fx.SetActive(false);
                this.cell.Tier2Fx.SetActive(false);
                this.<tt>__4 = new TransformAnimationTask(this.<tm>__1, this.duration * 0.5f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__4.rotate(Quaternion.Euler(0f, 0f, this.targetLocalRotationZ), true, Easing.Function.LINEAR);
                if (this.targetLocalScale.HasValue)
                {
                    this.<tt>__4.scale(this.<origLocalScale>__2, true, Easing.Function.LINEAR);
                }
                this.<ta>__0.addTask(this.<tt>__4);
            Label_022A:
                while (this.<ta>__0.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0243;
                }
                this.$PC = -1;
            Label_0241:
                return false;
            Label_0243:
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

        [CompilerGenerated]
        private sealed class <blindCardChoiceUtilRoutine_RevelCard>c__Iterator147 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RewardGalleryCell <$>cell;
            internal float <$>duration;
            internal bool <$>isSelected;
            internal TransformAnimation <$>ta;
            internal MenuTreasureChest <>f__this;
            internal int <choice>__2;
            internal bool <isEpic>__3;
            internal RectTransform <tm>__0;
            internal TransformAnimationTask <tt>__1;
            internal RewardGalleryCell cell;
            internal float duration;
            internal bool isSelected;
            internal TransformAnimation ta;

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
                        this.<tm>__0 = this.ta.RectTm;
                        this.<tt>__1 = new TransformAnimationTask(this.<tm>__0, this.duration * 0.5f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__1.rotate(Quaternion.Euler(0f, 90f, 0f), true, Easing.Function.LINEAR);
                        this.ta.addTask(this.<tt>__1);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_01F8;

                    default:
                        goto Label_020F;
                }
                if (this.ta.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0211;
                }
                this.cell.Flipside.SetActive(false);
                this.<choice>__2 = (int) this.cell.ActiveContent.Obj;
                this.<isEpic>__3 = this.<>f__this.Reward.TournamentUpgradeReward.Choices[this.<choice>__2].IsEpicUpgrade;
                this.cell.Tier1Fx.SetActive(this.<isEpic>__3);
                this.cell.Tier2Fx.SetActive(this.<isEpic>__3 && this.isSelected);
                if (this.isSelected)
                {
                    this.cell.ContentOverlay.SetActive(false);
                    this.cell.ModeOverlay.SetActive(false);
                }
                else
                {
                    this.cell.ContentOverlay.SetActive(true);
                    this.cell.ModeOverlay.SetActive(true);
                }
                this.<tt>__1 = new TransformAnimationTask(this.<tm>__0, this.duration * 0.5f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__1.rotate(Quaternion.identity, true, Easing.Function.LINEAR);
                this.ta.addTask(this.<tt>__1);
            Label_01F8:
                while (this.ta.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0211;
                }
                this.$PC = -1;
            Label_020F:
                return false;
            Label_0211:
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

        [CompilerGenerated]
        private sealed class <finalizationRoutine>c__Iterator142 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>displayRewardContentForSeconds;
            internal MenuTreasureChest <>f__this;
            internal int <i>__2;
            internal IEnumerator <ie>__0;
            internal Vector3 <sourceWorldPos>__1;
            internal float displayRewardContentForSeconds;

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
                        this.<>f__this.FinalizationRunning = true;
                        if (this.displayRewardContentForSeconds <= 0f)
                        {
                            goto Label_00FF;
                        }
                        this.<>f__this.m_clicked = false;
                        if (this.displayRewardContentForSeconds >= float.MaxValue)
                        {
                            goto Label_00E3;
                        }
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(this.displayRewardContentForSeconds);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00E3;

                    default:
                        goto Label_01F9;
                }
                if (this.<ie>__0.MoveNext() && !this.<>f__this.m_clicked)
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_01FB;
                }
                this.<>f__this.FinalizationRoutineSkippedByPlayer = this.<ie>__0.MoveNext();
                goto Label_00FF;
            Label_00E3:
                while (!this.<>f__this.m_clicked)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01FB;
                }
                this.<>f__this.FinalizationRoutineSkippedByPlayer = true;
            Label_00FF:
                if (!this.<>f__this.usesCardChoiceFlow())
                {
                    this.<sourceWorldPos>__1 = this.<>f__this.ChestContentRoot.RectTm.position;
                    PlayerView.Binder.EffectSystem.playEffectStatic(this.<sourceWorldPos>__1, EffectType.UI_Bling, -1f, true, 1f, null);
                }
                this.<>f__this.ChestContentRoot.gameObject.SetActive(false);
                this.<>f__this.ButtonGetAll.gameObject.SetActive(false);
                this.<>f__this.ButtonOkay.gameObject.SetActive(false);
                this.<i>__2 = 0;
                while (this.<i>__2 < this.<>f__this.GetCardButtons.Count)
                {
                    this.<>f__this.GetCardButtons[this.<i>__2].gameObject.SetActive(false);
                    this.<i>__2++;
                }
                this.<>f__this.FinalizationRunning = false;
                goto Label_01F9;
                this.$PC = -1;
            Label_01F9:
                return false;
            Label_01FB:
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

        [CompilerGenerated]
        private sealed class <genericSpreadRoutine>c__Iterator144 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuTreasureChest <>f__this;
            internal IEnumerator <ie>__0;
            internal TransformAnimationTask <leftTt>__2;
            internal TransformAnimationTask <leftTt>__5;
            internal TransformAnimationTask <middleTt>__1;
            internal TransformAnimationTask <middleTt>__4;
            internal TransformAnimationTask <rightTt>__3;

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
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.25f);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_031C;

                    default:
                        goto Label_0350;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_0352;
                }
                if (this.<>f__this.RewardGalleryCells.Count == 3)
                {
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ChestUnfold, (float) 0f);
                    this.<middleTt>__1 = new TransformAnimationTask(this.<>f__this.ChestContentMiddle.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<middleTt>__1.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                    this.<middleTt>__1.translateToAnchoredPos(new Vector2(0f, 0f), Easing.Function.IN_CUBIC);
                    this.<>f__this.ChestContentMiddle.addTask(this.<middleTt>__1);
                    this.<leftTt>__2 = new TransformAnimationTask(this.<>f__this.ChestContentLeft.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<leftTt>__2.rotate(Quaternion.Euler(0f, 0f, 5f), true, Easing.Function.IN_CUBIC);
                    this.<leftTt>__2.translateToAnchoredPos(new Vector2(-320f, -100f), Easing.Function.IN_CUBIC);
                    this.<>f__this.ChestContentLeft.addTask(this.<leftTt>__2);
                    this.<rightTt>__3 = new TransformAnimationTask(this.<>f__this.ChestContentRight.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<rightTt>__3.rotate(Quaternion.Euler(0f, 0f, -5f), true, Easing.Function.IN_CUBIC);
                    this.<rightTt>__3.translateToAnchoredPos(new Vector2(320f, -100f), Easing.Function.IN_CUBIC);
                    this.<>f__this.ChestContentRight.addTask(this.<rightTt>__3);
                }
                else if (this.<>f__this.RewardGalleryCells.Count == 2)
                {
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ChestUnfold, (float) 0f);
                    this.<middleTt>__4 = new TransformAnimationTask(this.<>f__this.ChestContentMiddle.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<middleTt>__4.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                    this.<middleTt>__4.translateToAnchoredPos(new Vector2(225f, -50f), Easing.Function.IN_CUBIC);
                    this.<>f__this.ChestContentMiddle.addTask(this.<middleTt>__4);
                    this.<leftTt>__5 = new TransformAnimationTask(this.<>f__this.ChestContentLeft.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<leftTt>__5.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                    this.<leftTt>__5.translateToAnchoredPos(new Vector2(-225f, -50f), Easing.Function.IN_CUBIC);
                    this.<>f__this.ChestContentLeft.addTask(this.<leftTt>__5);
                }
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_031C:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_0352;
                }
                this.<>f__this.setInteractable(true);
                this.<>f__this.m_genericSpreadRoutine = null;
                goto Label_0350;
                this.$PC = -1;
            Label_0350:
                return false;
            Label_0352:
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

        [CompilerGenerated]
        private sealed class <petAnimationRoutine>c__Iterator143 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuTreasureChest <>f__this;
            internal RewardGalleryCell <cell>__10;
            internal RewardGalleryCell <cell>__13;
            internal RewardGalleryCell <cell>__17;
            internal RewardGalleryCell <cell>__8;
            internal int <i>__11;
            internal int <i>__12;
            internal int <i>__14;
            internal int <i>__7;
            internal int <i>__9;
            internal IEnumerator <ie>__0;
            internal TransformAnimationTask <leftTt>__2;
            internal TransformAnimationTask <leftTt>__5;
            internal TransformAnimationTask <middleTt>__4;
            internal PetReward <petReward>__15;
            internal Player <player>__18;
            internal string <prevCornerText>__21;
            internal TransformAnimationTask <rightTt>__3;
            internal TransformAnimationTask <rightTt>__6;
            internal TransformAnimationTask <rootTt>__1;
            internal ShopEntry <shopEntry>__16;
            internal RewardGalleryCell.Content <tempContent>__20;
            internal Reward <tempReward>__19;

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
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0f);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_01BE;

                    case 3:
                        goto Label_0371;

                    case 4:
                        goto Label_0454;

                    case 5:
                        goto Label_0525;

                    case 6:
                        goto Label_064C;

                    default:
                        goto Label_083A;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_083C;
                }
                this.<rootTt>__1 = new TransformAnimationTask(this.<>f__this.ChestContentRoot.RectTm, 0.12f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<rootTt>__1.scale((Vector3) (Vector3.one * 1.25f), true, Easing.Function.IN_QUART);
                this.<>f__this.ChestContentRoot.addTask(this.<rootTt>__1);
                this.<leftTt>__2 = new TransformAnimationTask(this.<>f__this.ChestContentLeft.RectTm, 0.12f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<leftTt>__2.rotate(new Vector3(0f, 0f, 10f), true, Easing.Function.IN_QUART);
                this.<>f__this.ChestContentLeft.addTask(this.<leftTt>__2);
                this.<rightTt>__3 = new TransformAnimationTask(this.<>f__this.ChestContentRight.RectTm, 0.12f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<rightTt>__3.rotate(new Vector3(0f, 0f, -10f), true, Easing.Function.IN_QUART);
                this.<>f__this.ChestContentRight.addTask(this.<rightTt>__3);
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.75f);
            Label_01BE:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_083C;
                }
                if (this.<>f__this.RewardGalleryCells.Count == 3)
                {
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ChestUnfold, (float) 0f);
                    this.<middleTt>__4 = new TransformAnimationTask(this.<>f__this.ChestContentMiddle.RectTm, 0.12f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<middleTt>__4.translateToAnchoredPos(new Vector2(0f, 50f), Easing.Function.IN_QUART);
                    this.<>f__this.ChestContentMiddle.addTask(this.<middleTt>__4);
                    this.<leftTt>__5 = new TransformAnimationTask(this.<>f__this.ChestContentLeft.RectTm, 0.12f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<leftTt>__5.rotate(new Vector3(0f, 0f, -5f), true, Easing.Function.IN_QUART);
                    this.<leftTt>__5.translateToAnchoredPos(new Vector2(-320f, -50f), Easing.Function.IN_QUART);
                    this.<>f__this.ChestContentLeft.addTask(this.<leftTt>__5);
                    this.<rightTt>__6 = new TransformAnimationTask(this.<>f__this.ChestContentRight.RectTm, 0.12f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<rightTt>__6.rotate(new Vector3(0f, 0f, 5f), true, Easing.Function.IN_QUART);
                    this.<rightTt>__6.translateToAnchoredPos(new Vector2(320f, -50f), Easing.Function.IN_QUART);
                    this.<>f__this.ChestContentRight.addTask(this.<rightTt>__6);
                }
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_0371:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 3;
                    goto Label_083C;
                }
                this.<i>__7 = 0;
                while (this.<i>__7 < this.<>f__this.RewardGalleryCells.Count)
                {
                    this.<cell>__8 = this.<>f__this.RewardGalleryCells[this.<i>__7];
                    if (this.<cell>__8.ActiveContent.ToProgressBarNormalizedValue > -1f)
                    {
                        this.<cell>__8.ProgressBarCanvasGroup.animateToBlack(0.15f, 0f);
                    }
                    this.<cell>__8.StarCanvasGroup.animateToBlack(0.15f, 0f);
                    this.<i>__7++;
                }
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_0454:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 4;
                    goto Label_083C;
                }
                this.<i>__9 = 0;
                while (this.<i>__9 < this.<>f__this.RewardGalleryCells.Count)
                {
                    this.<cell>__10 = this.<>f__this.RewardGalleryCells[this.<i>__9];
                    if (this.<cell>__10.ActiveContent.ToProgressBarNormalizedValue > -1f)
                    {
                        this.<cell>__10.ProgressBar.animateToNormalizedValue(this.<cell>__10.ActiveContent.ToProgressBarNormalizedValue, 0.4f, 10, null, 0f);
                    }
                    this.<i>__9++;
                }
                this.<i>__11 = 0;
                goto Label_0558;
            Label_0525:
                if (this.<>f__this.RewardGalleryCells[this.<i>__11].ProgressBar.Animating)
                {
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_083C;
                }
                this.<i>__11++;
            Label_0558:
                if (this.<i>__11 < this.<>f__this.RewardGalleryCells.Count)
                {
                    goto Label_0525;
                }
                this.<i>__12 = 0;
                while (this.<i>__12 < this.<>f__this.RewardGalleryCells.Count)
                {
                    this.<cell>__13 = this.<>f__this.RewardGalleryCells[this.<i>__12];
                    if ((this.<cell>__13.ActiveContent.FromProgressBarNormalizedValue < 1f) && (this.<cell>__13.ActiveContent.ToProgressBarNormalizedValue >= 1f))
                    {
                        this.<cell>__13.ProgressBarFgDefault.SetActive(false);
                        this.<cell>__13.ProgressBarFgFull.SetActive(true);
                    }
                    this.<i>__12++;
                }
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.15f);
            Label_064C:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 6;
                    goto Label_083C;
                }
                this.<i>__14 = 0;
                while (this.<i>__14 < this.<>f__this.Reward.Pets.Count)
                {
                    this.<petReward>__15 = this.<>f__this.Reward.Pets[this.<i>__14];
                    if (!string.IsNullOrEmpty(this.<petReward>__15.ConvertIntoShopEntryId))
                    {
                        this.<shopEntry>__16 = ConfigShops.GetShopEntry(this.<petReward>__15.ConvertIntoShopEntryId);
                        if (this.<shopEntry>__16 != null)
                        {
                            this.<cell>__17 = this.<>f__this.RewardGalleryCells[this.<i>__14];
                            this.<player>__18 = GameLogic.Binder.GameState.Player;
                            this.<tempReward>__19 = new Reward();
                            this.<tempReward>__19.addShopEntryDrop(this.<player>__18, this.<petReward>__15.ConvertIntoShopEntryId, false);
                            this.<tempContent>__20 = RewardGalleryCell.CreateDefaultContentForReward(this.<tempReward>__19, false, null);
                            this.<prevCornerText>__21 = this.<cell>__17.CornerText.text;
                            PlayerView.Binder.EffectSystem.playEffectStatic(this.<cell>__17.RectTm.position, EffectType.UI_Bling, -1f, true, 1f, null);
                            this.<cell>__17.initialize(this.<tempContent>__20, this.<cell>__17.ClickCallback);
                            this.<cell>__17.Button.interactable = true;
                            this.<cell>__17.ActiveContent.Interactable = true;
                            this.<cell>__17.CornerText.text = this.<prevCornerText>__21.Replace("+", "x");
                            this.<cell>__17.CornerText.gameObject.SetActive(true);
                            this.<cell>__17.CornerTextBg.gameObject.SetActive(true);
                        }
                    }
                    this.<i>__14++;
                }
                this.<>f__this.m_petAnimationRoutine = null;
                goto Label_083A;
                this.$PC = -1;
            Label_083A:
                return false;
            Label_083C:
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

        [CompilerGenerated]
        private sealed class <sneakPeakCardChoiceRoutine>c__Iterator145 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal IEnumerator <_ie>__4;
            internal TransformAnimationTask <_tt>__3;
            internal MenuTreasureChest <>f__this;
            internal RewardGalleryCell <cell>__16;
            internal RewardGalleryCell <cell>__22;
            internal TournamentUpgradeReward.Entry <choice>__21;
            internal TransformAnimation <clickedCellParentTa>__12;
            internal int <i>__1;
            internal int <i>__11;
            internal int <i>__13;
            internal int <i>__15;
            internal int <i>__18;
            internal int <i>__19;
            internal int <i>__20;
            internal int <i>__7;
            internal IEnumerator <ie>__5;
            internal TransformAnimationTask <leftTt>__9;
            internal TransformAnimationTask <middleTt>__8;
            internal TransformAnimation <parentTa>__17;
            internal Player <player>__0;
            internal TransformAnimationTask <rightTt>__10;
            internal bool <running>__6;
            internal TransformAnimation <ta>__2;
            internal TransformAnimationTask <tt>__14;

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
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        this.<i>__1 = this.<>f__this.RewardGalleryCells.Count - 1;
                        goto Label_0197;

                    case 1:
                        break;

                    case 2:
                        goto Label_01D5;

                    case 3:
                        if (this.<running>__6)
                        {
                            goto Label_02F3;
                        }
                        this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.25f);
                        goto Label_03A0;

                    case 4:
                        goto Label_03A0;

                    case 5:
                        goto Label_0517;

                    case 6:
                        goto Label_0672;

                    case 7:
                        goto Label_06B2;

                    case 8:
                        goto Label_07E8;

                    case 9:
                        goto Label_0811;

                    case 10:
                        goto Label_0882;

                    case 11:
                        goto Label_093B;

                    case 12:
                        goto Label_097E;

                    case 13:
                        goto Label_0A5F;

                    case 14:
                        goto Label_0BBC;

                    case 15:
                        goto Label_0BE5;

                    case 0x10:
                        goto Label_0C56;

                    case 0x11:
                        goto Label_0CFF;

                    case 0x12:
                        goto Label_0E5C;

                    case 0x13:
                        goto Label_0E85;

                    case 20:
                        goto Label_0EF6;

                    case 0x15:
                        goto Label_0F62;

                    case 0x16:
                        goto Label_0F9C;

                    default:
                        goto Label_10D1;
                }
            Label_0179:
                while (this.<_ie>__4.MoveNext())
                {
                    this.$current = this.<_ie>__4.Current;
                    this.$PC = 1;
                    goto Label_10D3;
                }
                this.<i>__1--;
            Label_0197:
                if (this.<i>__1 >= 0)
                {
                    this.<ta>__2 = this.<>f__this.RewardGalleryCells[this.<i>__1].RectTm.parent.GetComponent<TransformAnimation>();
                    this.<ta>__2.RectTm.localScale = (Vector3) (Vector3.one * 3f);
                    this.<_tt>__3 = new TransformAnimationTask(this.<ta>__2.transform, 0.1f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<_tt>__3.scale((Vector3) (Vector3.one * 0.9f), true, Easing.Function.OUT_BOUNCE);
                    this.<ta>__2.stopAll();
                    this.<ta>__2.addTask(this.<_tt>__3);
                    this.<_ie>__4 = TimeUtil.WaitForUnscaledSeconds(0.15f);
                    goto Label_0179;
                }
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_01D5:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 2;
                    goto Label_10D3;
                }
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ChestUnfold, (float) 0f);
                this.<>f__this.m_enumeratorList.Clear();
                this.<>f__this.m_enumeratorList.Add(this.<>f__this.blindCardChoiceUtilRoutine_HideCard(this.<>f__this.RewardGalleryCells[0], 0.1f, -1.5f, new Vector3?((Vector3) (Vector3.one * 1.4f)), 0f));
                this.<>f__this.m_enumeratorList.Add(this.<>f__this.blindCardChoiceUtilRoutine_HideCard(this.<>f__this.RewardGalleryCells[1], 0.1f, -15f, new Vector3?((Vector3) (Vector3.one * 1.3f)), 0.025f));
                this.<>f__this.m_enumeratorList.Add(this.<>f__this.blindCardChoiceUtilRoutine_HideCard(this.<>f__this.RewardGalleryCells[2], 0.1f, 10f, new Vector3?((Vector3) (Vector3.one * 1.2f)), 0.05f));
            Label_02F3:
                this.<running>__6 = false;
                this.<i>__7 = 0;
                while (this.<i>__7 < this.<>f__this.m_enumeratorList.Count)
                {
                    this.<running>__6 = this.<>f__this.m_enumeratorList[this.<i>__7].MoveNext();
                    this.<i>__7++;
                }
                this.$current = null;
                this.$PC = 3;
                goto Label_10D3;
            Label_03A0:
                if (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 4;
                    goto Label_10D3;
                }
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ChestUnfold, (float) 0f);
                this.<middleTt>__8 = new TransformAnimationTask(this.<>f__this.ChestContentMiddle.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<middleTt>__8.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                this.<middleTt>__8.translateToAnchoredPos(new Vector2(0f, -50f), Easing.Function.IN_CUBIC);
                this.<>f__this.ChestContentMiddle.addTask(this.<middleTt>__8);
                this.<leftTt>__9 = new TransformAnimationTask(this.<>f__this.ChestContentLeft.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<leftTt>__9.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                this.<leftTt>__9.translateToAnchoredPos(new Vector2(-380f, -50f), Easing.Function.IN_CUBIC);
                this.<>f__this.ChestContentLeft.addTask(this.<leftTt>__9);
                this.<rightTt>__10 = new TransformAnimationTask(this.<>f__this.ChestContentRight.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<rightTt>__10.rotate(Quaternion.identity, true, Easing.Function.IN_CUBIC);
                this.<rightTt>__10.translateToAnchoredPos(new Vector2(380f, -50f), Easing.Function.IN_CUBIC);
                this.<>f__this.ChestContentRight.addTask(this.<rightTt>__10);
            Label_0517:
                while ((this.<>f__this.ChestContentLeft.HasTasks || this.<>f__this.ChestContentMiddle.HasTasks) || this.<>f__this.ChestContentRight.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_10D3;
                }
                LangUtil.Shuffle<RewardGalleryCell>(this.<>f__this.RewardGalleryCells);
                this.<>f__this.RewardGalleryCells[0].RectTm.SetParent(this.<>f__this.ChestContentMiddle.RectTm, false);
                this.<>f__this.RewardGalleryCells[1].RectTm.SetParent(this.<>f__this.ChestContentLeft.RectTm, false);
                this.<>f__this.RewardGalleryCells[2].RectTm.SetParent(this.<>f__this.ChestContentRight.RectTm, false);
                this.<i>__11 = 0;
                while (this.<i>__11 < this.<>f__this.RewardGalleryCells.Count)
                {
                    this.<>f__this.RewardGalleryCells[this.<i>__11].HoverTransformAnimation.Enabled = true;
                    this.<i>__11++;
                }
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_0672:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 6;
                    goto Label_10D3;
                }
                this.<>f__this.setInteractable(true);
                this.<>f__this.m_cardChoiceRoutinePendingClickFlag = true;
            Label_06B2:
                while (this.<>f__this.m_cardChoiceRoutinePendingClickFlag)
                {
                    this.$current = null;
                    this.$PC = 7;
                    goto Label_10D3;
                }
                this.<clickedCellParentTa>__12 = this.<>f__this.m_lastClickedCell.RectTm.parent.GetComponent<TransformAnimation>();
                this.<>f__this.setInteractable(false);
                this.<i>__13 = 0;
                while (this.<i>__13 < this.<>f__this.RewardGalleryCells.Count)
                {
                    this.<>f__this.RewardGalleryCells[this.<i>__13].HoverTransformAnimation.Enabled = false;
                    this.<i>__13++;
                }
                this.<tt>__14 = new TransformAnimationTask(this.<clickedCellParentTa>__12.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__14.scale((Vector3) (Vector3.one * 1.1f), true, Easing.Function.OUT_CUBIC);
                this.<clickedCellParentTa>__12.stopAll();
                this.<clickedCellParentTa>__12.addTask(this.<tt>__14);
                this.<ie>__5 = this.<>f__this.blindCardChoiceUtilRoutine_RevelCard(this.<>f__this.m_lastClickedCell, this.<clickedCellParentTa>__12, 0.05f, true);
            Label_07E8:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 8;
                    goto Label_10D3;
                }
            Label_0811:
                while (this.<clickedCellParentTa>__12.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 9;
                    goto Label_10D3;
                }
                PlayerView.Binder.EffectSystem.playEffectStatic(this.<>f__this.m_lastClickedCell.RectTm.position, EffectType.UI_Bling, -1f, true, 1f, null);
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.3f);
            Label_0882:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 10;
                    goto Label_10D3;
                }
                this.<i>__15 = 0;
                while (this.<i>__15 < this.<>f__this.RewardGalleryCells.Count)
                {
                    this.<cell>__16 = this.<>f__this.RewardGalleryCells[this.<i>__15];
                    if (this.<cell>__16 == this.<>f__this.m_lastClickedCell)
                    {
                        goto Label_098E;
                    }
                    this.<parentTa>__17 = this.<cell>__16.RectTm.parent.GetComponent<TransformAnimation>();
                    this.<ie>__5 = this.<>f__this.blindCardChoiceUtilRoutine_RevelCard(this.<cell>__16, this.<parentTa>__17, 0.05f, false);
                Label_093B:
                    while (this.<ie>__5.MoveNext())
                    {
                        this.$current = this.<ie>__5.Current;
                        this.$PC = 11;
                        goto Label_10D3;
                    }
                    this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.1f);
                Label_097E:
                    while (this.<ie>__5.MoveNext())
                    {
                        this.$current = this.<ie>__5.Current;
                        this.$PC = 12;
                        goto Label_10D3;
                    }
                Label_098E:
                    this.<i>__15++;
                }
                this.<>f__this.ButtonOkay.Button.interactable = true;
                this.<>f__this.ButtonOkay.gameObject.SetActive(true);
                this.<>f__this.refreshGetCardButtons();
                if (!App.Binder.ConfigMeta.TOURNAMENT_DIRECT_CARD_PURCHASING_ENABLED)
                {
                    this.<>f__this.m_cardChoiceRoutinePendingClickFlag = true;
                    goto Label_0F9C;
                }
                this.<>f__this.CeremonyMenu.Descriptions[0].text = _.L(ConfigLoca.UI_PROMPT_CHOOSE_ANOTHER, null, false);
                this.<>f__this.CeremonyMenu.DescriptionCanvasGroup.setTransparent(false);
                this.<>f__this.m_cardChoiceRoutinePendingClickFlag = true;
            Label_0A5F:
                while (this.<>f__this.m_cardChoiceRoutinePendingClickFlag)
                {
                    this.$current = null;
                    this.$PC = 13;
                    goto Label_10D3;
                }
                this.<clickedCellParentTa>__12 = this.<>f__this.m_lastClickedCell.RectTm.parent.GetComponent<TransformAnimation>();
                this.<>f__this.ButtonOkay.gameObject.SetActive(false);
                this.<i>__18 = 0;
                while (this.<i>__18 < this.<>f__this.GetCardButtons.Count)
                {
                    this.<>f__this.GetCardButtons[this.<i>__18].gameObject.SetActive(false);
                    this.<i>__18++;
                }
                this.<>f__this.setInteractable(false);
                if (this.<>f__this.m_cardChoiceOkayButtonClicked)
                {
                    goto Label_0FAC;
                }
                this.<tt>__14 = new TransformAnimationTask(this.<clickedCellParentTa>__12.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__14.scale((Vector3) (Vector3.one * 1.1f), true, Easing.Function.OUT_CUBIC);
                this.<clickedCellParentTa>__12.stopAll();
                this.<clickedCellParentTa>__12.addTask(this.<tt>__14);
                this.<ie>__5 = this.<>f__this.blindCardChoiceUtilRoutine_RevelCard(this.<>f__this.m_lastClickedCell, this.<clickedCellParentTa>__12, 0.05f, true);
            Label_0BBC:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 14;
                    goto Label_10D3;
                }
            Label_0BE5:
                while (this.<clickedCellParentTa>__12.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 15;
                    goto Label_10D3;
                }
                PlayerView.Binder.EffectSystem.playEffectStatic(this.<>f__this.m_lastClickedCell.RectTm.position, EffectType.UI_Bling, -1f, true, 1f, null);
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.3f);
            Label_0C56:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 0x10;
                    goto Label_10D3;
                }
                this.<>f__this.ButtonOkay.Button.interactable = true;
                this.<>f__this.ButtonOkay.gameObject.SetActive(true);
                this.<>f__this.refreshGetCardButtons();
                this.<>f__this.CeremonyMenu.Descriptions[0].text = _.L(ConfigLoca.UI_PROMPT_CHOOSE_ANOTHER, null, false);
                this.<>f__this.CeremonyMenu.DescriptionCanvasGroup.setTransparent(false);
                this.<>f__this.m_cardChoiceRoutinePendingClickFlag = true;
            Label_0CFF:
                while (this.<>f__this.m_cardChoiceRoutinePendingClickFlag)
                {
                    this.$current = null;
                    this.$PC = 0x11;
                    goto Label_10D3;
                }
                this.<clickedCellParentTa>__12 = this.<>f__this.m_lastClickedCell.RectTm.parent.GetComponent<TransformAnimation>();
                this.<>f__this.ButtonOkay.gameObject.SetActive(false);
                this.<i>__19 = 0;
                while (this.<i>__19 < this.<>f__this.GetCardButtons.Count)
                {
                    this.<>f__this.GetCardButtons[this.<i>__19].gameObject.SetActive(false);
                    this.<i>__19++;
                }
                this.<>f__this.setInteractable(false);
                if (this.<>f__this.m_cardChoiceOkayButtonClicked)
                {
                    goto Label_0FAC;
                }
                this.<tt>__14 = new TransformAnimationTask(this.<clickedCellParentTa>__12.RectTm, 0.05f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__14.scale((Vector3) (Vector3.one * 1.1f), true, Easing.Function.OUT_CUBIC);
                this.<clickedCellParentTa>__12.stopAll();
                this.<clickedCellParentTa>__12.addTask(this.<tt>__14);
                this.<ie>__5 = this.<>f__this.blindCardChoiceUtilRoutine_RevelCard(this.<>f__this.m_lastClickedCell, this.<clickedCellParentTa>__12, 0.05f, true);
            Label_0E5C:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 0x12;
                    goto Label_10D3;
                }
            Label_0E85:
                while (this.<clickedCellParentTa>__12.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 0x13;
                    goto Label_10D3;
                }
                PlayerView.Binder.EffectSystem.playEffectStatic(this.<>f__this.m_lastClickedCell.RectTm.position, EffectType.UI_Bling, -1f, true, 1f, null);
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.3f);
            Label_0EF6:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 20;
                    goto Label_10D3;
                }
                this.<>f__this.ButtonOkay.Button.interactable = true;
                this.<>f__this.ButtonOkay.gameObject.SetActive(true);
                this.<>f__this.refreshGetCardButtons();
                this.<>f__this.m_cardChoiceRoutinePendingClickFlag = true;
            Label_0F62:
                while (this.<>f__this.m_cardChoiceRoutinePendingClickFlag)
                {
                    this.$current = null;
                    this.$PC = 0x15;
                    goto Label_10D3;
                }
                goto Label_0FAC;
            Label_0F9C:
                while (this.<>f__this.m_cardChoiceRoutinePendingClickFlag)
                {
                    this.$current = null;
                    this.$PC = 0x16;
                    goto Label_10D3;
                }
            Label_0FAC:
                this.<i>__20 = 0;
                while (this.<i>__20 < this.<>f__this.Reward.TournamentUpgradeReward.Choices.Count)
                {
                    this.<choice>__21 = this.<>f__this.Reward.TournamentUpgradeReward.Choices[this.<i>__20];
                    if (this.<choice>__21.Selected)
                    {
                        this.<cell>__22 = this.<>f__this.getCellForTournamentUpgradeChoice(this.<i>__20);
                        PlayerView.Binder.EffectSystem.playEffectStatic(this.<cell>__22.RectTm.position, EffectType.UI_Bling, -1f, true, 1f, null);
                        PlayerView.Binder.DungeonHud.flyToHudTournamentCardGain(this.<choice>__21.TourmanentUpgradeId, RectTransformUtility.WorldToScreenPoint(this.<>f__this.Canvas.worldCamera, this.<cell>__22.RectTm.position), false, true);
                    }
                    this.<i>__20++;
                }
                this.<>f__this.Interactable = true;
                this.<>f__this.onClick();
                this.<>f__this.m_cardChoiceRoutine = null;
                goto Label_10D1;
                this.$PC = -1;
            Label_10D1:
                return false;
            Label_10D3:
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
    }
}

