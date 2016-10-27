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
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class DungeonHud : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Canvas <Canvas>k__BackingField;
        [CompilerGenerated]
        private float <PanelOriginalPosX>k__BackingField;
        [CompilerGenerated]
        private float <PanelOriginalWidth>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public HudTopButton AdventureButton;
        public PlayerView.AnnouncementBanner AnnouncementBanner;
        public HudTopButton AscendButton;
        public CanvasGroupAlphaFading BgOverlay;
        public Image BgOverlayImage;
        public PlayerView.BossHuntTicker BossHuntTicker;
        public GridLayoutGroup BuffTimerGrid;
        public AnimatedTextCounter CoinAmount;
        public ScalePulse CoinAmountScalePulse;
        public RectTransform CoinIconRectTm;
        public ScalePulse CoinIconScalePulse;
        public PulsatingGraphic CoinPulseGraphic;
        public GameObject CoinRoot;
        public DungeonHudCombatStats CombatStats;
        public OffscreenOpenClose CutsceneBottom;
        public OffscreenOpenClose CutsceneTop;
        public AnimatedTextCounter DiamondAmount;
        public ScalePulse DiamondAmountScalePulse;
        public RectTransform DiamondIconRect;
        public ScalePulse DiamondIconScalePulse;
        public PulsatingGraphic DiamondPulseGraphic;
        public GameObject DiamondRoot;
        public ImageFlashEffect FlashEffect;
        public PlayerView.FloaterText FloaterText;
        public PlayerView.FloorProgressionRibbon FloorProgressionRibbon;
        public ParticleSystem FloorTransitionFx;
        public GameObject FpsCounterRoot;
        public AnimatedProgressBar FrenzyBar;
        public Text FrenzyBarText;
        public Text KnockedDownText;
        public OffscreenOpenClose LeftPanel;
        private List<CombatTextIngame> m_activeIngameCombatTexts = new List<CombatTextIngame>();
        private List<CombatTextMenu> m_activeMenuCombatTexts = new List<CombatTextMenu>();
        private Dictionary<Buff, BuffHudTimer> m_buffHudTimers = new Dictionary<Buff, BuffHudTimer>();
        private Dictionary<CharacterInstance, HpIndicator> m_characterHpIndicators = new Dictionary<CharacterInstance, HpIndicator>();
        private CheatConsole m_cheatConsole;
        private Action<int, int, double, bool> m_flyToHudAction_coinGain;
        private Action<int, int, double, bool> m_flyToHudAction_diamondGain;
        private Action<int, int, double, bool> m_flyToHudAction_itemGain;
        private Action<int, int, double, bool> m_flyToHudAction_petGain;
        private Action<int, int, double, bool> m_flyToHudAction_potionGain;
        private Action<int, int, double, bool> m_flyToHudAction_runestoneGain;
        private Action<int, int, double, bool> m_flyToHudAction_skillGain;
        private Action<int, int, double, bool> m_flyToHudAction_tokenGain;
        private Action<int, int, double, bool> m_flyToHudAction_tokensIntoRetirementChest;
        private Action<int, int, double, bool> m_flyToHudAction_tournamentCardGain;
        private Action<int, int, double, bool> m_flyToHudAction_xpGain;
        private Coroutine m_frenzyActivationRoutine;
        private ResourceGainVisualizer m_gameplayResourceGainVisualizer;
        private Sprite m_itemBorderSprite;
        private Coroutine m_mainElementVisibilityAnimationRoutine;
        private ResourceGainVisualizer m_menuResourceGainVisualizer;
        private float m_nextCombatStatUpdate;
        private int m_numCheatOptionsClicks;
        private Vector2 m_origAscendButtonAnchoredPos;
        private int m_originalSortingOrder;
        private Vector2 m_origVendorButtonAnchoredPos;
        private Dictionary<SkillHudButton, bool> m_overridenSkillHudButtonInteractable = new Dictionary<SkillHudButton, bool>();
        private bool m_pendingResourceBarReset;
        private float m_resetCheatOptionsClickTime;
        private List<Sprite> m_resourceGainCoinSprites;
        private List<Sprite> m_resourceGainDiamondSprites;
        private List<Sprite> m_resourceGainTokenSprites;
        private List<Sprite> m_resourceGainXpSprites;
        private TransformAnimationTask m_rootPanelMoveToCenterTask_InCubic;
        private TransformAnimationTask m_rootPanelMoveToCenterTask_OutCubic;
        private TransformAnimationTask m_rootPanelMoveToLeftTask;
        private TransformAnimationTask m_rootPanelMoveToRightTask;
        private TransformAnimationTask m_rootPanelRetractFromLeftTask;
        private Coroutine m_skillActivationRoutine;
        private Dictionary<int, bool> m_skillButtonHidden = new Dictionary<int, bool>();
        private List<SkillHudButton> m_skillButtons = new List<SkillHudButton>();
        private Sprite m_topButtonSpriteGold;
        private Sprite m_topButtonSpriteNormal;
        public HudTopButton MenuButton;
        public HudTopButton MissionsButton;
        public HudTopButton PetsButton;
        public PlayerView.PlayerXpBar PlayerXpBar;
        public HudTopButton PromotionEventsButton;
        public Text RememberToUpgradeYourItemsText;
        public UnityEngine.Canvas ResourcesCanvas;
        public ParticleSystem RetirementFx;
        public OffscreenOpenClose RightPanel;
        public TransformAnimation RootPanelTa;
        public RectTransform SkillButtonContainerTm;
        public HudTopButton SkillsButton;
        public PulsatingGraphic SlidingPanelArrowNotifier;
        public OffscreenOpenClose SlidingPanelArrowRoot;
        public IconWithText SpeedCheatButton;
        public PlayerView.TaskPanel TaskPanel;
        public AnimatedTextCounter TokenAmount;
        public ScalePulse TokenAmountScalePulse;
        public RectTransform TokenIconRect;
        public ScalePulse TokenIconScalePulse;
        public PulsatingGraphic TokenPulseGraphic;
        public GameObject TokenRoot;
        public OffscreenOpenClose TopPanel;
        public ParticleSystem TutorialCircle;
        public HudTopButton VendorButton;
        public Image Vignetting;
        public CanvasGroupAlphaFading YoureDeadText;
        public CanvasGroupAlphaFading YoureDeadText2;
        public Text YoureDeadText2Text;

        public SkillHudButton addSkillHudButton(SkillInstance skillInstance, int hudIndex, bool isInventoryButton)
        {
            <addSkillHudButton>c__AnonStorey2E8 storeye = new <addSkillHudButton>c__AnonStorey2E8();
            storeye.hudIndex = hudIndex;
            storeye.<>f__this = this;
            SkillHudButton item = PlayerView.Binder.SkillHudButtonPool.getObject();
            item.transform.SetParent(this.SkillButtonContainerTm);
            item.transform.localScale = Vector3.one;
            item.transform.position = this.SkillButtonContainerTm.GetChild(storeye.hudIndex).transform.position;
            item.initialize(skillInstance, isInventoryButton);
            item.setInitialPosition();
            this.m_skillButtons.Add(item);
            if (isInventoryButton)
            {
                item.Button.onClick.AddListener(new UnityAction(storeye.<>m__187));
            }
            else
            {
                item.Button.onClick.AddListener(new UnityAction(storeye.<>m__188));
            }
            item.gameObject.SetActive(true);
            return item;
        }

        public Coroutine animateOverlay(bool fadeToBlack, float duration, [Optional, DefaultParameterValue(null)] Color? targetColor)
        {
            if (!fadeToBlack)
            {
                return this.BgOverlay.animateToTransparent(duration, 0f);
            }
            if (targetColor.HasValue)
            {
                this.BgOverlayImage.color = targetColor.Value;
            }
            return this.BgOverlay.animateToBlack(duration, 0f);
        }

        public void applyTutorialRestrictions()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player.hasCompletedTutorial("TUT003C"))
            {
                this.CoinRoot.SetActive(true);
                this.DiamondRoot.SetActive(true);
                this.TokenRoot.SetActive(true);
                this.PlayerXpBar.gameObject.SetActive(true);
                this.MenuButton.Tm.gameObject.SetActive(true);
                this.SkillsButton.Tm.gameObject.SetActive(true);
                this.VendorButton.Tm.gameObject.SetActive(true);
                this.PetsButton.Tm.gameObject.SetActive(true);
                this.AdventureButton.Tm.gameObject.SetActive(true);
                if (player.shopUnlocked())
                {
                    this.VendorButton.Button.interactable = true;
                    this.VendorButton.CanvasGroup.alpha = 1f;
                    this.VendorButton.ButtonImage.material = null;
                    this.VendorButton.Icon.material = null;
                }
                else
                {
                    this.VendorButton.Button.interactable = false;
                    this.VendorButton.CanvasGroup.alpha = 0.5f;
                    this.VendorButton.ButtonImage.material = PlayerView.Binder.DisabledUiMaterial;
                    this.VendorButton.Icon.material = PlayerView.Binder.DisabledUiMaterial;
                }
                if (player.isHeroOrSkillPopupUnlocked())
                {
                    this.MenuButton.Button.interactable = true;
                    this.MenuButton.CanvasGroup.alpha = 1f;
                    this.MenuButton.ButtonImage.material = null;
                    this.MenuButton.Icon.material = null;
                    this.SkillsButton.Button.interactable = true;
                    this.SkillsButton.CanvasGroup.alpha = 1f;
                    this.SkillsButton.ButtonImage.material = null;
                    this.SkillsButton.Icon.material = null;
                    this.SlidingPanelArrowRoot.gameObject.SetActive(ConfigUi.DHUD_PANEL_ARROW_VISIBLE);
                }
                else
                {
                    this.MenuButton.Button.interactable = false;
                    this.MenuButton.CanvasGroup.alpha = 0.5f;
                    this.MenuButton.ButtonImage.material = PlayerView.Binder.DisabledUiMaterial;
                    this.MenuButton.Icon.material = PlayerView.Binder.DisabledUiMaterial;
                    this.SkillsButton.Button.interactable = false;
                    this.SkillsButton.CanvasGroup.alpha = 0.5f;
                    this.SkillsButton.ButtonImage.material = PlayerView.Binder.DisabledUiMaterial;
                    this.SkillsButton.Icon.material = PlayerView.Binder.DisabledUiMaterial;
                    this.SlidingPanelArrowRoot.gameObject.SetActive(false);
                }
                if (player.isPetPopupUnlocked())
                {
                    this.PetsButton.Button.interactable = true;
                    this.PetsButton.CanvasGroup.alpha = 1f;
                    this.PetsButton.ButtonImage.material = null;
                    this.PetsButton.Icon.material = null;
                }
                else
                {
                    this.PetsButton.Button.interactable = false;
                    this.PetsButton.CanvasGroup.alpha = 0.5f;
                    this.PetsButton.ButtonImage.material = PlayerView.Binder.DisabledUiMaterial;
                    this.PetsButton.Icon.material = PlayerView.Binder.DisabledUiMaterial;
                }
            }
            else
            {
                this.CoinRoot.SetActive(false);
                this.DiamondRoot.SetActive(false);
                this.TokenRoot.SetActive(false);
                this.PlayerXpBar.gameObject.SetActive(false);
                this.MenuButton.Button.interactable = false;
                this.MenuButton.Tm.gameObject.SetActive(false);
                this.AdventureButton.Button.interactable = false;
                this.AdventureButton.Tm.gameObject.SetActive(false);
                this.SkillsButton.Tm.gameObject.SetActive(false);
                this.VendorButton.Tm.gameObject.SetActive(false);
                this.PetsButton.Tm.gameObject.SetActive(false);
                this.SlidingPanelArrowRoot.gameObject.SetActive(false);
            }
            this.refreshAdventureButton();
        }

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            RectTransform component = this.RootPanelTa.GetComponent<RectTransform>();
            this.PanelOriginalWidth = RectTransformExtensions.GetWidth(component);
            this.PanelOriginalPosX = component.anchoredPosition.x;
            this.m_origAscendButtonAnchoredPos = this.AdventureButton.RootTm.anchoredPosition;
            this.m_origVendorButtonAnchoredPos = this.VendorButton.RootTm.anchoredPosition;
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                UnityEngine.Object.Destroy(this.ResourcesCanvas);
            }
            List<Sprite> list = new List<Sprite>();
            list.Add(PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "icon_coin_floater"));
            this.m_resourceGainCoinSprites = list;
            list = new List<Sprite>();
            list.Add(PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "icon_gem_floater"));
            this.m_resourceGainDiamondSprites = list;
            list = new List<Sprite>();
            list.Add(PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "icon_token_floater"));
            this.m_resourceGainTokenSprites = list;
            list = new List<Sprite>();
            list.Add(PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_xpshard1"));
            list.Add(PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_xpshard2"));
            list.Add(PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_xpshard3"));
            this.m_resourceGainXpSprites = list;
            this.m_itemBorderSprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_uix_slot");
            if (ConfigApp.CHEAT_CONSOLE_ENABLED)
            {
                this.m_cheatConsole = UnityUtils.InstantiateGameObjectWithComponent<CheatConsole>(this.RectTm);
                this.m_cheatConsole.transform.SetSiblingIndex(this.RectTm.childCount - 2);
                this.m_cheatConsole.Visible = true;
            }
            this.m_flyToHudAction_coinGain = new Action<int, int, double, bool>(this.flyToHudCoinGainCallback);
            this.m_flyToHudAction_diamondGain = new Action<int, int, double, bool>(this.flyToHudDiamondGainCallback);
            this.m_flyToHudAction_tokenGain = new Action<int, int, double, bool>(this.flyToHudTokenGainCallback);
            this.m_flyToHudAction_tokensIntoRetirementChest = new Action<int, int, double, bool>(this.flyToHudTokensIntoRetirementChestCallback);
            this.m_flyToHudAction_xpGain = new Action<int, int, double, bool>(this.flyToHudXpGainCallback);
            this.m_flyToHudAction_skillGain = new Action<int, int, double, bool>(this.flyToHudSkillGainCallback);
            this.m_flyToHudAction_itemGain = new Action<int, int, double, bool>(this.flyToHudItemGainCallback);
            this.m_flyToHudAction_potionGain = new Action<int, int, double, bool>(this.flyToHudPotionGainCallback);
            this.m_flyToHudAction_petGain = new Action<int, int, double, bool>(this.flyToHudPetGainCallback);
            this.m_flyToHudAction_runestoneGain = new Action<int, int, double, bool>(this.flyToHudRunestoneGainCallback);
            this.m_flyToHudAction_tournamentCardGain = new Action<int, int, double, bool>(this.flyToHudTournamentCardGainCallback);
            if (this.FlashEffect != null)
            {
                this.FlashEffect.EndCallback = new Action<ImageFlashEffect>(this.flashEffectEndCallback);
                this.FlashEffect.gameObject.SetActive(false);
            }
            this.FloaterText.CanvasGroup.alpha = 0f;
            this.SpeedCheatButton.Text.text = "1.0x";
            this.SpeedCheatButton.gameObject.SetActive(ConfigApp.CHEAT_SPEED_BUTTON_VISIBLE);
            this.MenuButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_BUTTON_MENU, null, false));
            this.VendorButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_BUTTON_SHOP, null, false));
            this.SkillsButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_BUTTON_SKILLS, null, false));
            this.RememberToUpgradeYourItemsText.text = _.L(ConfigLoca.DHUD_REMEMBER_TO_UPGRADE_ITEMS, null, false);
            this.YoureDeadText.setTransparent(true);
            this.YoureDeadText2.setTransparent(true);
            this.Vignetting.enabled = ConfigDevice.DeviceQuality() >= DeviceQualityType.High;
            this.FrenzyBar.gameObject.SetActive(false);
            this.FrenzyBarText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PROGR_RIBBON_FRENZY, null, false));
            this.SlidingPanelArrowRoot.gameObject.SetActive(ConfigUi.DHUD_PANEL_ARROW_VISIBLE);
            this.SlidingPanelArrowNotifier.gameObject.SetActive(false);
            this.m_topButtonSpriteNormal = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_uih_heroview_bar");
            this.m_topButtonSpriteGold = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_uih_heroview_bar_gold");
            this.CombatStats.DpsTotalTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROSTATS_DPS_TOTAL, null, false));
            string[] textArray1 = new string[] { _.L(ConfigLoca.ITEMS_WEAPON, null, false), "\n", _.L(ConfigLoca.SKILLS, null, false), "\n", _.L(ConfigLoca.HEROSTATS_SIDEKICKS, null, false) };
            this.CombatStats.DpsBreakdownTitle.text = string.Concat(textArray1);
            this.CombatStats.CpsTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROSTATS_COINS_PER_SECOND, null, false));
            this.refreshVendorNotifiers();
            this.refreshMenuNotifiers();
            this.refreshSkillNotifiers();
            this.refreshFpsCounter();
            this.TaskPanel.refreshTaskPanelAppboy();
            this.refreshPetNotifiers();
        }

        private void cleanupCombatTextIngame(CombatTextIngame ct)
        {
            this.m_activeIngameCombatTexts.Remove(ct);
            PlayerView.Binder.CombatTextPoolIngame.returnObject(ct);
        }

        private void cleanupCombatTextMenu(CombatTextMenu ct)
        {
            this.m_activeMenuCombatTexts.Remove(ct);
            PlayerView.Binder.CombatTextPoolMenu.returnObject(ct);
        }

        public void cleanupSkillButtons()
        {
            for (int i = this.m_skillButtons.Count - 1; i >= 0; i--)
            {
                SkillHudButton item = this.m_skillButtons[i];
                this.m_skillButtons.Remove(item);
                PlayerView.Binder.SkillHudButtonPool.returnObject(item);
            }
        }

        public void closeSlidingPanelArrowRoot(float duration, Easing.Function easing)
        {
            if (ConfigUi.DHUD_PANEL_ARROW_VISIBLE)
            {
                this.SlidingPanelArrowRoot.close(duration, easing, 0f);
                this.SlidingPanelArrowNotifier.enabled = false;
            }
        }

        private void combatTextIngameEndCallback(CombatTextIngame ct)
        {
            this.cleanupCombatTextIngame(ct);
        }

        private void combatTextMenuEndCallback(CombatTextMenu ct)
        {
            this.cleanupCombatTextMenu(ct);
        }

        private void createHpIndicator(CharacterInstance c)
        {
            HpIndicator indicator = PlayerView.Binder.HpIndicatorPool.getObject();
            indicator.transform.SetParent(this.RectTm);
            indicator.transform.SetAsFirstSibling();
            indicator.transform.localPosition = Vector3.zero;
            indicator.initialize(this.Canvas.worldCamera, c);
            this.m_characterHpIndicators.Add(c, indicator);
            indicator.gameObject.SetActive(c.IsBoss);
        }

        private void flashEffectEndCallback(ImageFlashEffect ife)
        {
            ife.gameObject.SetActive(false);
        }

        [DebuggerHidden]
        public IEnumerator flyToHud(Reward reward, Vector2 sourceScreenPos, bool fromMenu, [Optional, DefaultParameterValue(true)] bool doFlyResourcesToHud)
        {
            <flyToHud>c__IteratorFA rfa = new <flyToHud>c__IteratorFA();
            rfa.reward = reward;
            rfa.sourceScreenPos = sourceScreenPos;
            rfa.fromMenu = fromMenu;
            rfa.doFlyResourcesToHud = doFlyResourcesToHud;
            rfa.<$>reward = reward;
            rfa.<$>sourceScreenPos = sourceScreenPos;
            rfa.<$>fromMenu = fromMenu;
            rfa.<$>doFlyResourcesToHud = doFlyResourcesToHud;
            rfa.<>f__this = this;
            return rfa;
        }

        [DebuggerHidden]
        public IEnumerator flyToHud(List<Reward> rewards, Vector2 sourceScreenPos, bool fromMenu, [Optional, DefaultParameterValue(true)] bool doFlyResourcesToHud)
        {
            <flyToHud>c__IteratorFB rfb = new <flyToHud>c__IteratorFB();
            rfb.rewards = rewards;
            rfb.sourceScreenPos = sourceScreenPos;
            rfb.fromMenu = fromMenu;
            rfb.doFlyResourcesToHud = doFlyResourcesToHud;
            rfb.<$>rewards = rewards;
            rfb.<$>sourceScreenPos = sourceScreenPos;
            rfb.<$>fromMenu = fromMenu;
            rfb.<$>doFlyResourcesToHud = doFlyResourcesToHud;
            rfb.<>f__this = this;
            return rfb;
        }

        private void flyToHud(double resourceAmount, List<Sprite> sprites, Sprite borders, Color color, bool grayscale, Vector2 sourceScreenPos, Vector2 targetScreenPos, float localScaleMin, float localScaleMax, float referenceDuration, bool fromMenu, Action<int, int, double, bool> endCallback, [Optional, DefaultParameterValue(0f)] float overrideMaxOffsetDistance, [Optional, DefaultParameterValue(false)] bool overrideMaxCount)
        {
            if (!ConfigApp.CHEAT_MARKETING_MODE_ENABLED || !PlayerView.Binder.MarketingBuildController.FlyToHudDisabled)
            {
                int num;
                float num4;
                if (!overrideMaxCount && (resourceAmount > ConfigUi.MAX_RESOURCE_FLY_TO_HUD_COUNT))
                {
                    num = ConfigUi.MAX_RESOURCE_FLY_TO_HUD_COUNT + UnityEngine.Random.Range(-ConfigUi.RESOURCE_FLY_TO_HUD_COUNT_RANDOM_VARIATION, ConfigUi.RESOURCE_FLY_TO_HUD_COUNT_RANDOM_VARIATION);
                }
                else
                {
                    num = (int) resourceAmount;
                }
                if (GameLogic.Binder.FrenzySystem.isFrenzyActive())
                {
                    num = Mathf.Max(Mathf.RoundToInt(num * 0.75f), 1);
                }
                if (ConfigDevice.DeviceQuality() == DeviceQualityType.Med)
                {
                    num = Mathf.Max(Mathf.RoundToInt(num * 0.75f), 1);
                }
                else if (ConfigDevice.DeviceQuality() == DeviceQualityType.Low)
                {
                    num = Mathf.Max(Mathf.RoundToInt(num * 0.5f), 1);
                }
                float durationMin = (num <= 1) ? referenceDuration : (referenceDuration * 0.8f);
                float durationMax = (num <= 1) ? referenceDuration : (referenceDuration * 1.4f);
                if (overrideMaxOffsetDistance > 0f)
                {
                    num4 = overrideMaxOffsetDistance;
                }
                else if (num > 1)
                {
                    num4 = UnityEngine.Random.Range((float) 100f, (float) 600f) + (num * 4f);
                }
                else
                {
                    num4 = UnityEngine.Random.Range((float) 50f, (float) 30f) + (num * 2f);
                }
                if (fromMenu)
                {
                    this.m_menuResourceGainVisualizer.animate(sprites, borders, color, grayscale, sourceScreenPos, targetScreenPos, resourceAmount, num, durationMin, durationMax, localScaleMin, localScaleMax, num4, fromMenu, endCallback, this.m_menuResourceGainVisualizer.RootTm.childCount - 2);
                }
                else
                {
                    this.m_gameplayResourceGainVisualizer.animate(sprites, borders, color, grayscale, sourceScreenPos, targetScreenPos, resourceAmount, num, durationMin, durationMax, localScaleMin, localScaleMax, num4, fromMenu, endCallback, this.m_gameplayResourceGainVisualizer.RootTm.childCount - 2);
                }
                PlayerView.Binder.EventBus.FlyToHudStarted(sourceScreenPos);
            }
        }

        public void flyToHudCoinGain(double amount, Vector2 sourceScreenPos, bool fromMenu)
        {
            if (amount > 0.0)
            {
                Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.CoinIconRectTm.position);
                this.flyToHud(amount, this.m_resourceGainCoinSprites, null, Color.white, false, sourceScreenPos, targetScreenPos, 0.32f, 0.32f, 0.6f, fromMenu, this.m_flyToHudAction_coinGain, 0f, false);
                this.showResourceGainCombatText(sourceScreenPos, amount, ResourceType.Coin);
            }
        }

        public void flyToHudCoinGain(double amount, Vector3 sourceWorldPos, bool fromMenu)
        {
            Vector2 sourceScreenPos = RectTransformUtility.WorldToScreenPoint(PlayerView.Binder.RoomView.RoomCamera.Camera, sourceWorldPos);
            this.flyToHudCoinGain(amount, sourceScreenPos, fromMenu);
        }

        private void flyToHudCoinGainCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.CoinPulseGraphic.play();
            this.CoinIconScalePulse.play();
            this.CoinAmountScalePulse.play();
            AudioSystem.PlaybackParameters parameters2 = new AudioSystem.PlaybackParameters();
            parameters2.PitchMin = 0.95f;
            parameters2.PitchMax = 1.05f;
            AudioSystem.PlaybackParameters pp = parameters2;
            if (fromMenu)
            {
                PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpUi_ResourceCoin, pp, 2);
            }
            else
            {
                PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_ResourceCoin, pp, 2);
            }
            this.CoinAmount.queue(resourceAmount);
        }

        public void flyToHudDiamondGain(double amount, Vector2 sourceScreenPos, bool fromMenu)
        {
            Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.DiamondIconRect.position);
            this.flyToHud(amount, this.m_resourceGainDiamondSprites, null, Color.white, false, sourceScreenPos, targetScreenPos, 0.4f, 0.4f, 0.8f, fromMenu, this.m_flyToHudAction_diamondGain, 0f, false);
            this.showResourceGainCombatText(sourceScreenPos, amount, ResourceType.Diamond);
        }

        public void flyToHudDiamondGain(double amount, Vector3 sourceWorldPos, bool fromMenu)
        {
            Vector2 sourceScreenPos = RectTransformUtility.WorldToScreenPoint(PlayerView.Binder.RoomView.RoomCamera.Camera, sourceWorldPos);
            this.flyToHudDiamondGain(amount, sourceScreenPos, fromMenu);
        }

        private void flyToHudDiamondGainCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.DiamondPulseGraphic.play();
            this.DiamondIconScalePulse.play();
            this.DiamondAmountScalePulse.play();
            if (fromMenu)
            {
                PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpUi_ResourceDiamond, 3);
            }
            else
            {
                PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_ResourceDiamond, 3);
            }
            this.DiamondAmount.queue(resourceAmount);
        }

        public void flyToHudItemGain(ItemInstance ii, Vector2 sourceScreenPos, bool grayscale, bool fromMenu)
        {
            Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.getInventorySkillHudButton().IconRect.position);
            List<Sprite> list2 = new List<Sprite>();
            list2.Add(PlayerView.Binder.SpriteResources.getSprite("Menu", ii.Item.SpriteId));
            List<Sprite> sprites = list2;
            this.flyToHud(1.0, sprites, this.m_itemBorderSprite, Color.white, grayscale, sourceScreenPos, targetScreenPos, 0.6f, 0.6f, 0.9f, fromMenu, this.m_flyToHudAction_itemGain, 0f, false);
        }

        private void flyToHudItemGainCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.getInventorySkillHudButton().IconPulseGraphic.play();
            this.getInventorySkillHudButton().IconScalePulse.play();
            this.getInventorySkillHudButton().refreshNotifiers(true);
            PlayerView.Binder.AudioSystem.playSfxIfNotAlreadyPlaying(AudioSourceType.SfxUi_ItemGain);
        }

        public void flyToHudPetGain(string characterId, Vector2 sourceScreenPos, bool grayscale, bool fromMenu)
        {
            Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.PetsButton.Icon.rectTransform.position);
            Character character = GameLogic.Binder.CharacterResources.getResource(characterId);
            List<Sprite> list2 = new List<Sprite>();
            list2.Add(PlayerView.Binder.SpriteResources.getSprite(character.AvatarSprite));
            List<Sprite> sprites = list2;
            this.flyToHud(1.0, sprites, this.m_itemBorderSprite, Color.white, grayscale, sourceScreenPos, targetScreenPos, 0.6f, 0.6f, 0.9f, fromMenu, this.m_flyToHudAction_petGain, 0f, false);
        }

        private void flyToHudPetGainCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.PetsButton.PulseGraphic.play();
            this.PetsButton.ScalePulse.play();
            PlayerView.Binder.AudioSystem.playSfxIfNotAlreadyPlaying(AudioSourceType.SfxUi_ItemGain);
        }

        public void flyToHudPotionGain(PotionType potionType, Vector2 sourceScreenPos, bool grayscale, bool fromMenu)
        {
            Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.getInventorySkillHudButton().IconRect.position);
            List<Sprite> list2 = new List<Sprite>();
            list2.Add(PlayerView.Binder.SpriteResources.getSprite(ConfigUi.POTION_TYPE_SPRITES[potionType]));
            List<Sprite> sprites = list2;
            this.flyToHud(1.0, sprites, this.m_itemBorderSprite, Color.white, grayscale, sourceScreenPos, targetScreenPos, 0.6f, 0.6f, 0.9f, fromMenu, this.m_flyToHudAction_potionGain, 0f, false);
        }

        private void flyToHudPotionGainCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.getInventorySkillHudButton().IconPulseGraphic.play();
            this.getInventorySkillHudButton().IconScalePulse.play();
            this.getInventorySkillHudButton().refreshNotifiers(true);
            PlayerView.Binder.AudioSystem.playSfxIfNotAlreadyPlaying(AudioSourceType.SfxUi_ItemGain);
        }

        public void flyToHudRunestoneGain(string runestoneId, Vector2 sourceScreenPos, bool grayscale, bool fromMenu, [Optional, DefaultParameterValue(0)] int numExtraRunestones)
        {
            ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(runestoneId);
            Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.SkillsButton.Tm.position);
            List<Sprite> list2 = new List<Sprite>();
            list2.Add(PlayerView.Binder.SpriteResources.getSprite(runestoneData.Sprite));
            List<Sprite> sprites = list2;
            float overrideMaxOffsetDistance = 0f;
            if (numExtraRunestones > 0)
            {
                overrideMaxOffsetDistance = UnityEngine.Random.Range((float) 100f, (float) 600f) + (numExtraRunestones * 4f);
            }
            this.flyToHud(1.0, sprites, this.m_itemBorderSprite, Color.white, grayscale, sourceScreenPos, targetScreenPos, 0.6f, 0.6f, 0.9f, fromMenu, this.m_flyToHudAction_runestoneGain, overrideMaxOffsetDistance, false);
        }

        private void flyToHudRunestoneGainCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.SkillsButton.PulseGraphic.play();
            this.SkillsButton.ScalePulse.play();
            PlayerView.Binder.AudioSystem.playSfxIfNotAlreadyPlaying(AudioSourceType.SfxUi_RunestoneGain);
        }

        public void flyToHudSkillGain(SkillType skill, Vector2 sourceScreenPos, bool grayscale, bool fromMenu)
        {
            ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[skill];
            Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.MenuButton.Tm.position);
            List<Sprite> list2 = new List<Sprite>();
            list2.Add(PlayerView.Binder.SpriteResources.getSprite(data.Spritesheet, data.Sprite));
            List<Sprite> sprites = list2;
            this.flyToHud(1.0, sprites, this.m_itemBorderSprite, Color.white, grayscale, sourceScreenPos, targetScreenPos, 0.6f, 0.6f, 0.9f, fromMenu, this.m_flyToHudAction_skillGain, 0f, false);
        }

        private void flyToHudSkillGainCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.SkillsButton.PulseGraphic.play();
            this.SkillsButton.ScalePulse.play();
            PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_SkillGain, (float) 0f);
        }

        public void flyToHudTokenGain(double amount, Vector2 sourceScreenPos, bool fromMenu, bool toRetirementChest)
        {
            Vector2 vector;
            Action<int, int, double, bool> action;
            if (toRetirementChest)
            {
                vector = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.AdventureButton.Icon.rectTransform.position);
                action = this.m_flyToHudAction_tokensIntoRetirementChest;
            }
            else
            {
                vector = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.TokenIconRect.position);
                action = this.m_flyToHudAction_tokenGain;
                this.showResourceGainCombatText(sourceScreenPos, amount, ResourceType.Token);
            }
            this.flyToHud(amount, this.m_resourceGainTokenSprites, null, Color.white, false, sourceScreenPos, vector, 0.4f, 0.4f, 0.8f, fromMenu, action, 0f, false);
        }

        public void flyToHudTokenGain(double amount, Vector3 sourceWorldPos, bool fromMenu, bool toRetirementChest)
        {
            Vector2 sourceScreenPos = RectTransformUtility.WorldToScreenPoint(PlayerView.Binder.RoomView.RoomCamera.Camera, sourceWorldPos);
            this.flyToHudTokenGain(amount, sourceScreenPos, fromMenu, toRetirementChest);
        }

        private void flyToHudTokenGainCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.TokenPulseGraphic.play();
            this.TokenIconScalePulse.play();
            this.TokenAmountScalePulse.play();
            if (fromMenu)
            {
                PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpUi_ResourceDiamond, 3);
            }
            else
            {
                PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_ResourceDiamond, 3);
            }
            this.TokenAmount.queue(resourceAmount);
        }

        private void flyToHudTokensIntoRetirementChestCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.refreshAdventureButton();
            this.AdventureButton.ScalePulse.play();
            this.AdventureButton.PulseGraphic.play();
            if (fromMenu)
            {
                PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpUi_ResourceDiamond, 3);
            }
            else
            {
                PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_ResourceDiamond, 3);
            }
        }

        public void flyToHudTournamentCardGain(string tournamentUpgradeId, Vector2 sourceScreenPos, bool grayscale, bool fromMenu)
        {
            Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.AdventureButton.Icon.rectTransform.position);
            TournamentUpgrade upgrade = App.Binder.ConfigMeta.TOURNAMENT_UPGRADES[tournamentUpgradeId];
            List<Sprite> list2 = new List<Sprite>();
            list2.Add(PlayerView.Binder.SpriteResources.getSprite(upgrade.getSprite()));
            List<Sprite> sprites = list2;
            this.flyToHud(1.0, sprites, null, Color.white, grayscale, sourceScreenPos, targetScreenPos, 0.6f, 0.6f, 0.9f, fromMenu, this.m_flyToHudAction_tournamentCardGain, 0f, false);
        }

        private void flyToHudTournamentCardGainCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.AdventureButton.PulseGraphic.play();
            this.AdventureButton.ScalePulse.play();
            PlayerView.Binder.AudioSystem.playSfxIfNotAlreadyPlaying(AudioSourceType.SfxUi_ItemGain);
        }

        public void flyToHudXpGain(double amount, Vector2 sourceScreenPos, bool fromMenu, bool overrideMaxCount)
        {
            Vector2 targetScreenPos = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.PlayerXpBar.PlayerRankIconTm.position);
            this.flyToHud(amount, this.m_resourceGainXpSprites, null, Color.white, false, sourceScreenPos, targetScreenPos, 0.17f, 0.23f, 0.7f, fromMenu, this.m_flyToHudAction_xpGain, 0f, overrideMaxCount);
        }

        public void flyToHudXpGain(double amount, Vector3 sourceWorldPos, bool fromMenu, bool overrideMaxCount)
        {
            Vector2 sourceScreenPos = RectTransformUtility.WorldToScreenPoint(PlayerView.Binder.RoomView.RoomCamera.Camera, sourceWorldPos);
            this.flyToHudXpGain(amount, sourceScreenPos, fromMenu, overrideMaxCount);
        }

        private void flyToHudXpGainCallback(int spritesCompletedThisFrame, int spriteCountTotal, double resourceAmount, bool fromMenu)
        {
            this.PlayerXpBar.PlayerRankPulseGraphic.play();
            this.PlayerXpBar.PlayerRankScalePulse.play();
            this.PlayerXpBar.queue(resourceAmount);
            AudioSystem.PlaybackParameters parameters2 = new AudioSystem.PlaybackParameters();
            parameters2.PitchMin = 0.95f;
            parameters2.PitchMax = 1.05f;
            AudioSystem.PlaybackParameters pp = parameters2;
            if (fromMenu)
            {
                PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpUi_ResourceCoin, pp, 2);
            }
            else
            {
                PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_ResourceCoin, pp, 2);
            }
        }

        private SkillHudButton getInventorySkillHudButton()
        {
            return this.m_skillButtons[3];
        }

        private SkillHudButton getSkillHudButtonForSkillGroup(int skillGroup)
        {
            for (int i = 0; i < this.m_skillButtons.Count; i++)
            {
                SkillInstance skillInstance = this.m_skillButtons[i].SkillInstance;
                if ((skillInstance != null) && (ConfigSkills.SHARED_DATA[skillInstance.SkillType].Group == skillGroup))
                {
                    return this.m_skillButtons[i];
                }
            }
            return null;
        }

        public void hideHpBars()
        {
            foreach (KeyValuePair<CharacterInstance, HpIndicator> pair in this.m_characterHpIndicators)
            {
                pair.Value.gameObject.SetActive(false);
            }
        }

        public void initialize(Camera camera)
        {
            this.Canvas = base.GetComponent<UnityEngine.Canvas>();
            this.Canvas.worldCamera = camera;
            this.m_originalSortingOrder = this.Canvas.sortingOrder;
            this.refreshCamera(this.Canvas.worldCamera);
            this.m_gameplayResourceGainVisualizer = base.gameObject.AddComponent<ResourceGainVisualizer>();
            this.m_gameplayResourceGainVisualizer.initialize(camera);
            this.m_menuResourceGainVisualizer = this.ResourcesCanvas.gameObject.AddComponent<ResourceGainVisualizer>();
            this.m_menuResourceGainVisualizer.initialize(camera);
            SlidingTaskPanel panel = (SlidingTaskPanel) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingTaskPanel);
            this.m_rootPanelMoveToRightTask = new TransformAnimationTask(this.RootPanelTa.RectTm, ConfigUi.SLIDING_PANEL_ENTRY_DURATION, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_rootPanelMoveToRightTask.translateToAnchoredPos(new Vector2(RectTransformExtensions.GetWidth(panel.PanelRoot.GetComponent<RectTransform>()), 0f), Easing.Function.OUT_CUBIC);
            SlidingAdventurePanel panel2 = (SlidingAdventurePanel) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingAdventurePanel);
            this.m_rootPanelMoveToLeftTask = new TransformAnimationTask(this.RootPanelTa.RectTm, ConfigUi.SLIDING_PANEL_ENTRY_DURATION, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_rootPanelMoveToLeftTask.translateToAnchoredPos(new Vector2(-RectTransformExtensions.GetWidth(panel2.PanelRoot.GetComponent<RectTransform>()), 0f), Easing.Function.OUT_CUBIC);
            this.m_rootPanelMoveToCenterTask_InCubic = new TransformAnimationTask(this.RootPanelTa.RectTm, ConfigUi.SLIDING_PANEL_EXIT_DURATION, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_rootPanelMoveToCenterTask_InCubic.translateToAnchoredPos(Vector2.zero, Easing.Function.IN_CUBIC);
            this.m_rootPanelMoveToCenterTask_OutCubic = new TransformAnimationTask(this.RootPanelTa.RectTm, ConfigUi.SLIDING_PANEL_EXIT_DURATION, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_rootPanelMoveToCenterTask_OutCubic.translateToAnchoredPos(Vector2.zero, Easing.Function.OUT_CUBIC);
            this.setElementVisibility(false, true);
        }

        [DebuggerHidden]
        private IEnumerator mainElementVisibilityAnimationRoutine(bool visible, bool instant)
        {
            <mainElementVisibilityAnimationRoutine>c__IteratorFD rfd = new <mainElementVisibilityAnimationRoutine>c__IteratorFD();
            rfd.visible = visible;
            rfd.instant = instant;
            rfd.<$>visible = visible;
            rfd.<$>instant = instant;
            rfd.<>f__this = this;
            return rfd;
        }

        public void onAscendButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.AscendPopupContent, null, 0f, false, true);
            }
        }

        private void onBuffPreEnd(CharacterInstance c, Buff buff)
        {
            if (c.IsPrimaryPlayerCharacter && (buff.HudSprite != null))
            {
                Buff key = null;
                if (buff.HudShowStacked)
                {
                    if (GameLogic.Binder.BuffSystem.getNumberOfBuffsFromSource(c, buff.Source) <= 1)
                    {
                        foreach (KeyValuePair<Buff, BuffHudTimer> pair in this.m_buffHudTimers)
                        {
                            if (pair.Key.Source.Object == buff.Source.Object)
                            {
                                key = pair.Key;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    key = buff;
                }
                if (key != null)
                {
                    BuffHudTimer timer = this.m_buffHudTimers[key];
                    PlayerView.Binder.BuffHudTimerPool.returnObject(timer);
                    this.m_buffHudTimers.Remove(key);
                    if (this.BuffTimerGrid.gameObject.activeSelf)
                    {
                        this.BuffTimerGrid.gameObject.SetActive(false);
                        this.BuffTimerGrid.gameObject.SetActive(true);
                    }
                }
            }
        }

        private void onBuffStarted(CharacterInstance c, Buff buff)
        {
            if ((c.IsPrimaryPlayerCharacter && (buff.HudSprite != null)) && (!buff.HudShowStacked || (GameLogic.Binder.BuffSystem.getNumberOfBuffsFromSource(c, buff.Source) <= 1)))
            {
                BuffHudTimer timer = PlayerView.Binder.BuffHudTimerPool.getObject();
                timer.RectTm.SetParent(this.BuffTimerGrid.transform);
                timer.gameObject.SetActive(true);
                timer.initialize(buff);
                this.m_buffHudTimers.Add(buff, timer);
            }
        }

        private void onCharacterBlinked(CharacterInstance c)
        {
            this.m_characterHpIndicators[c].refreshCurrentHp(true);
        }

        private void onCharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            if (this.m_characterHpIndicators.ContainsKey(targetCharacter) && this.m_characterHpIndicators[targetCharacter].gameObject.activeSelf)
            {
                this.m_characterHpIndicators[targetCharacter].refreshCurrentHp(false);
            }
            this.showDamageCombatText(sourceCharacter, targetCharacter, worldPos, amount, critted, damageReduced, damageType);
        }

        private void onCharacterHpGained(CharacterInstance c, double amount, bool silent)
        {
            if (this.m_characterHpIndicators.ContainsKey(c) && this.m_characterHpIndicators[c].gameObject.activeSelf)
            {
                this.m_characterHpIndicators[c].refreshCurrentHp(false);
            }
            if (!silent)
            {
                this.showHealingCombatText(c.PhysicsBody.Transform.position, amount);
            }
        }

        private void onCharacterKilled(CharacterInstance character, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            if (!character.IsPersistent)
            {
                this.removeHpIndicator(character);
            }
            else
            {
                this.m_characterHpIndicators[character].gameObject.SetActive(false);
            }
        }

        private void onCharacterPreDestroyed(CharacterInstance character)
        {
            this.removeHpIndicator(character);
        }

        private void onCharacterRevived(CharacterInstance c)
        {
            this.m_characterHpIndicators[c].refreshCurrentHp(true);
        }

        private void onCharacterSkillCooldownEnded(CharacterInstance character, SkillType skillType)
        {
            if (character.IsPrimaryPlayerCharacter)
            {
                int group = ConfigSkills.SHARED_DATA[skillType].Group;
                if (group >= 0)
                {
                    SkillHudButton button = this.getSkillHudButtonForSkillGroup(group);
                    if (((button != null) && button.gameObject.activeInHierarchy) && ((button.SkillInstance != null) && (button.SkillInstance.SkillType == skillType)))
                    {
                        button.CooldownFlash.show(ConfigUi.SKILL_COOLDOWN_END_FLASH_DURATION, ConfigUi.SKILL_COOLDOWN_END_ENTRY_EASING, ConfigUi.SKILL_COOLDOWN_END_EXIT_EASING, 0f);
                    }
                }
            }
        }

        private void onCharacterSkillsChanged(CharacterInstance character)
        {
            for (int i = 0; i < ConfigSkills.SkillGroupCount; i++)
            {
                this.m_skillButtons[i].initialize(character.getActiveSkillInstanceForGroup(i), false);
            }
            this.refreshSkillButtons();
        }

        private void onCharacterSpawned(CharacterInstance c)
        {
            this.createHpIndicator(c);
        }

        private void onCharacterTargetUpdated(CharacterInstance character, CharacterInstance oldTarget)
        {
            if (character == GameLogic.Binder.GameState.Player.ActiveCharacter)
            {
                if (((oldTarget != null) && (character.TargetCharacter != oldTarget)) && (!oldTarget.IsBoss && this.m_characterHpIndicators.ContainsKey(oldTarget)))
                {
                    this.m_characterHpIndicators[oldTarget].gameObject.SetActive(false);
                }
                if ((character.TargetCharacter != null) && this.m_characterHpIndicators.ContainsKey(character.TargetCharacter))
                {
                    this.m_characterHpIndicators[character.TargetCharacter].gameObject.SetActive(true);
                    this.m_characterHpIndicators[character.TargetCharacter].refreshCurrentHp(true);
                }
            }
        }

        private void onCharacterUpgraded(CharacterInstance character)
        {
            this.m_characterHpIndicators[character].refreshCurrentHp(true);
        }

        public void onCheatButtonClicked()
        {
            if ((ConfigApp.CHEAT_POPUP_ENABLED && !PlayerView.Binder.MenuSystem.InTransition) && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
            {
                this.m_numCheatOptionsClicks++;
                if (this.m_numCheatOptionsClicks >= 2)
                {
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.CheatPopupContent, null, 0f, false, true);
                    this.m_numCheatOptionsClicks = 0;
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStateChangeStarted -= new GameLogic.Events.GameplayStateChangeStarted(this.onGameplayStateChangeStarted);
            GameLogic.Binder.EventBus.OnGameplayStateChanged -= new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            GameLogic.Binder.EventBus.OnCharacterDealtDamage -= new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            GameLogic.Binder.EventBus.OnCharacterHpGained -= new GameLogic.Events.CharacterHpGained(this.onCharacterHpGained);
            GameLogic.Binder.EventBus.OnCharacterRevived -= new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            GameLogic.Binder.EventBus.OnCharacterBlinked -= new GameLogic.Events.CharacterBlinked(this.onCharacterBlinked);
            GameLogic.Binder.EventBus.OnCharacterSpawned -= new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
            GameLogic.Binder.EventBus.OnCharacterPreDestroyed -= new GameLogic.Events.CharacterPreDestroyed(this.onCharacterPreDestroyed);
            GameLogic.Binder.EventBus.OnCharacterTargetUpdated -= new GameLogic.Events.CharacterTargetUpdated(this.onCharacterTargetUpdated);
            GameLogic.Binder.EventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnCharacterSkillsChanged -= new GameLogic.Events.CharacterSkillsChanged(this.onCharacterSkillsChanged);
            GameLogic.Binder.EventBus.OnResourcesGained -= new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            GameLogic.Binder.EventBus.OnCharacterUpgraded -= new GameLogic.Events.CharacterUpgraded(this.onCharacterUpgraded);
            GameLogic.Binder.EventBus.OnCharacterSkillCooldownEnded -= new GameLogic.Events.CharacterSkillCooldownEnded(this.onCharacterSkillCooldownEnded);
            GameLogic.Binder.EventBus.OnRewardConsumed -= new GameLogic.Events.RewardConsumed(this.onRewardConsumed);
            GameLogic.Binder.EventBus.OnItemInspected -= new GameLogic.Events.ItemInspected(this.onItemInspected);
            GameLogic.Binder.EventBus.OnItemRankUpped -= new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnMultikillBonusGranted -= new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            GameLogic.Binder.EventBus.OnTutorialCompleted -= new GameLogic.Events.TutorialCompleted(this.onTutorialCompleted);
            GameLogic.Binder.EventBus.OnBuffStarted -= new GameLogic.Events.BuffStarted(this.onBuffStarted);
            GameLogic.Binder.EventBus.OnBuffPreEnd -= new GameLogic.Events.BuffPreEnd(this.onBuffPreEnd);
            GameLogic.Binder.EventBus.OnPlayerActiveCharacterSwitched -= new GameLogic.Events.PlayerActiveCharacterSwitched(this.onPlayerActiveCharacterSwitched);
            GameLogic.Binder.EventBus.OnFrenzyDeactivated -= new GameLogic.Events.FrenzyDeactivated(this.onFrenzyDeactivated);
            GameLogic.Binder.EventBus.OnPlayerRankUpped -= new GameLogic.Events.PlayerRankUpped(this.onPlayerRankUpped);
            GameLogic.Binder.EventBus.OnPetGained -= new GameLogic.Events.PetGained(this.onPetGained);
            GameLogic.Binder.EventBus.OnPetLevelUpped -= new GameLogic.Events.PetLevelUpped(this.onPetLevelUpped);
            GameLogic.Binder.EventBus.OnPetInspected -= new GameLogic.Events.PetInspected(this.onPetInspected);
            GameLogic.Binder.EventBus.OnItemSold -= new GameLogic.Events.ItemSold(this.onItemSold);
            GameLogic.Binder.EventBus.OnPromotionEventStarted -= new GameLogic.Events.PromotionEventStarted(this.onPromotionEventStarted);
            GameLogic.Binder.EventBus.OnPromotionEventEnded -= new GameLogic.Events.PromotionEventEnded(this.onPromotionEventEnded);
            GameLogic.Binder.EventBus.OnPromotionEventInspected -= new GameLogic.Events.PromotionEventInspected(this.onPromotionEventInspected);
            GameLogic.Binder.EventBus.OnPromotionEventRefreshed -= new GameLogic.Events.PromotionEventRefreshed(this.onPromotionEventRefreshed);
            GameLogic.Binder.EventBus.OnMissionStarted -= new GameLogic.Events.MissionStarted(this.onMissionStarted);
            GameLogic.Binder.EventBus.OnMissionInspected -= new GameLogic.Events.MissionInspected(this.onMissionInspected);
            PlayerView.Binder.EventBus.OnMenuChangeStarted -= new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChanged -= new PlayerView.Events.MenuChanged(this.onMenuChanged);
            PlayerView.Binder.EventBus.OnDungeonDropViewResourcesCollected -= new PlayerView.Events.DungeonDropViewResourcesCollected(this.onDungeonDropViewResourcesCollected);
        }

        public void onDungeonDropViewChestDropStart(Reward ltd)
        {
            if (!ConfigMeta.IsRetirementChest(ltd.ChestType))
            {
                TaskPanelItemType type = !ConfigMeta.IsMysteryChest(ltd.ChestType) ? TaskPanelItemType.BossChestDrop : TaskPanelItemType.NormalChestDrop;
                if (!this.TaskPanel.doesTaskPanelItemExist(type))
                {
                    this.TaskPanel.refreshTaskPanelItem(type, 1, null).ContentRoot.SetActive(false);
                }
            }
        }

        public void onDungeonDropViewChestUiPulseMidway(Reward reward)
        {
            if (ConfigMeta.IsRetirementChest(reward.ChestType))
            {
                this.refreshAdventureButton();
                this.AdventureButton.PulseGraphic.play();
                this.AdventureButton.ScalePulse.play();
            }
            else
            {
                this.TaskPanel.refreshTaskPanelMysteryAndBossChests();
            }
        }

        public void onDungeonDropViewResourcesCollected(ResourceType resourceType, double amount, Vector3 worldPos)
        {
            if (resourceType == ResourceType.Coin)
            {
                this.flyToHudCoinGain(amount, worldPos, false);
            }
            else if (resourceType == ResourceType.Diamond)
            {
                this.flyToHudDiamondGain(amount, worldPos, false);
            }
            else if (resourceType == ResourceType.Token)
            {
                this.flyToHudTokenGain(amount, worldPos, false, false);
            }
            else if (resourceType == ResourceType.Xp)
            {
                this.flyToHudXpGain(amount, worldPos, false, false);
            }
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStateChangeStarted += new GameLogic.Events.GameplayStateChangeStarted(this.onGameplayStateChangeStarted);
            GameLogic.Binder.EventBus.OnGameplayStateChanged += new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            GameLogic.Binder.EventBus.OnCharacterDealtDamage += new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            GameLogic.Binder.EventBus.OnCharacterHpGained += new GameLogic.Events.CharacterHpGained(this.onCharacterHpGained);
            GameLogic.Binder.EventBus.OnCharacterRevived += new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            GameLogic.Binder.EventBus.OnCharacterBlinked += new GameLogic.Events.CharacterBlinked(this.onCharacterBlinked);
            GameLogic.Binder.EventBus.OnCharacterSpawned += new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
            GameLogic.Binder.EventBus.OnCharacterPreDestroyed += new GameLogic.Events.CharacterPreDestroyed(this.onCharacterPreDestroyed);
            GameLogic.Binder.EventBus.OnCharacterTargetUpdated += new GameLogic.Events.CharacterTargetUpdated(this.onCharacterTargetUpdated);
            GameLogic.Binder.EventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnCharacterSkillsChanged += new GameLogic.Events.CharacterSkillsChanged(this.onCharacterSkillsChanged);
            GameLogic.Binder.EventBus.OnResourcesGained += new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            GameLogic.Binder.EventBus.OnCharacterUpgraded += new GameLogic.Events.CharacterUpgraded(this.onCharacterUpgraded);
            GameLogic.Binder.EventBus.OnCharacterSkillCooldownEnded += new GameLogic.Events.CharacterSkillCooldownEnded(this.onCharacterSkillCooldownEnded);
            GameLogic.Binder.EventBus.OnRewardConsumed += new GameLogic.Events.RewardConsumed(this.onRewardConsumed);
            GameLogic.Binder.EventBus.OnItemInspected += new GameLogic.Events.ItemInspected(this.onItemInspected);
            GameLogic.Binder.EventBus.OnItemRankUpped += new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnMultikillBonusGranted += new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            GameLogic.Binder.EventBus.OnTutorialCompleted += new GameLogic.Events.TutorialCompleted(this.onTutorialCompleted);
            GameLogic.Binder.EventBus.OnBuffStarted += new GameLogic.Events.BuffStarted(this.onBuffStarted);
            GameLogic.Binder.EventBus.OnBuffPreEnd += new GameLogic.Events.BuffPreEnd(this.onBuffPreEnd);
            GameLogic.Binder.EventBus.OnPlayerActiveCharacterSwitched += new GameLogic.Events.PlayerActiveCharacterSwitched(this.onPlayerActiveCharacterSwitched);
            GameLogic.Binder.EventBus.OnFrenzyDeactivated += new GameLogic.Events.FrenzyDeactivated(this.onFrenzyDeactivated);
            GameLogic.Binder.EventBus.OnPlayerRankUpped += new GameLogic.Events.PlayerRankUpped(this.onPlayerRankUpped);
            GameLogic.Binder.EventBus.OnPetGained += new GameLogic.Events.PetGained(this.onPetGained);
            GameLogic.Binder.EventBus.OnPetLevelUpped += new GameLogic.Events.PetLevelUpped(this.onPetLevelUpped);
            GameLogic.Binder.EventBus.OnPetInspected += new GameLogic.Events.PetInspected(this.onPetInspected);
            GameLogic.Binder.EventBus.OnItemSold += new GameLogic.Events.ItemSold(this.onItemSold);
            GameLogic.Binder.EventBus.OnPromotionEventStarted += new GameLogic.Events.PromotionEventStarted(this.onPromotionEventStarted);
            GameLogic.Binder.EventBus.OnPromotionEventEnded += new GameLogic.Events.PromotionEventEnded(this.onPromotionEventEnded);
            GameLogic.Binder.EventBus.OnPromotionEventInspected += new GameLogic.Events.PromotionEventInspected(this.onPromotionEventInspected);
            GameLogic.Binder.EventBus.OnPromotionEventRefreshed += new GameLogic.Events.PromotionEventRefreshed(this.onPromotionEventRefreshed);
            GameLogic.Binder.EventBus.OnMissionStarted += new GameLogic.Events.MissionStarted(this.onMissionStarted);
            GameLogic.Binder.EventBus.OnMissionInspected += new GameLogic.Events.MissionInspected(this.onMissionInspected);
            PlayerView.Binder.EventBus.OnMenuChangeStarted += new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChanged += new PlayerView.Events.MenuChanged(this.onMenuChanged);
            PlayerView.Binder.EventBus.OnDungeonDropViewResourcesCollected += new PlayerView.Events.DungeonDropViewResourcesCollected(this.onDungeonDropViewResourcesCollected);
        }

        private void onFrenzyDeactivated()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player.hasRetired())
            {
                player.IsReadyForRateGamePopup = true;
            }
        }

        public void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            for (int i = this.m_activeIngameCombatTexts.Count - 1; i >= 0; i--)
            {
                CombatTextIngame ct = this.m_activeIngameCombatTexts[i];
                this.cleanupCombatTextIngame(ct);
            }
            for (int j = 0; j < GameLogic.Binder.GameState.PersistentCharacters.Count; j++)
            {
                this.removeHpIndicator(GameLogic.Binder.GameState.PersistentCharacters[j]);
            }
            this.m_overridenSkillHudButtonInteractable.Clear();
        }

        public void onGameplayStarted(ActiveDungeon ad)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.Canvas.worldCamera.enabled = true;
            this.refreshCamera(this.Canvas.worldCamera);
            if (this.m_skillButtons.Count == 0)
            {
                CharacterInstance primaryPlayerCharacter = ad.PrimaryPlayerCharacter;
                for (int j = 0; j < ConfigSkills.SkillGroupCount; j++)
                {
                    SkillInstance skillInstance = primaryPlayerCharacter.getActiveSkillInstanceForGroup(j);
                    if (skillInstance != null)
                    {
                        this.addSkillHudButton(skillInstance, j, false);
                    }
                    else
                    {
                        this.addSkillHudButton(null, j, false);
                    }
                }
                this.addSkillHudButton(null, ConfigSkills.SkillGroupCount, true);
                this.m_skillButtonHidden.Clear();
                for (int k = 0; k < this.m_skillButtons.Count; k++)
                {
                    this.m_skillButtonHidden.Add(k, false);
                }
            }
            else
            {
                for (int m = 0; m < this.m_skillButtons.Count; m++)
                {
                    this.m_skillButtons[m].gameObject.SetActive(true);
                }
            }
            for (int i = 0; i < GameLogic.Binder.GameState.PersistentCharacters.Count; i++)
            {
                CharacterInstance key = GameLogic.Binder.GameState.PersistentCharacters[i];
                if (!this.m_characterHpIndicators.ContainsKey(key))
                {
                    this.createHpIndicator(key);
                }
            }
            if (App.Binder.ConfigMeta.SHOW_SHOP_BUTTON_IN_TOP)
            {
                this.VendorButton.RootTm.anchoredPosition = this.m_origAscendButtonAnchoredPos;
                this.AdventureButton.RootTm.anchoredPosition = this.m_origVendorButtonAnchoredPos;
            }
            else
            {
                this.VendorButton.RootTm.anchoredPosition = this.m_origVendorButtonAnchoredPos;
                this.AdventureButton.RootTm.anchoredPosition = this.m_origAscendButtonAnchoredPos;
            }
            this.FloorProgressionRibbon.initialize(ad.Floor);
            this.FloorProgressionRibbon.gameObject.SetActive(player.hasCompletedTutorial("TUT003C"));
            this.resetResourceBar();
            this.getInventorySkillHudButton().refreshNotifiers(false);
            this.refreshAdventureButton();
            this.refreshPromotionEventsButton();
            this.TaskPanel.refreshTaskPanelUnclaimedLevelUpRewards();
            this.TaskPanel.refreshTaskPanelHighestFloorReached();
            this.TaskPanel.refreshTaskPanelTournamentMainReward();
            this.TaskPanel.refreshTaskPanelTournamentContributorReward();
            this.TaskPanel.refreshTaskPanelTournamentCardPack();
            if (!ad.SeamlessTransition)
            {
                this.TaskPanel.refreshTaskPanelMysteryAndBossChests();
            }
            this.YoureDeadText.setTransparent(true);
            this.YoureDeadText2.setTransparent(true);
            this.FrenzyBar.gameObject.SetActive(GameLogic.Binder.FrenzySystem.isFrenzyActive());
            this.openCloseCutsceneBorders(false, 0f);
            this.applyTutorialRestrictions();
            this.TaskPanel.refreshTaskPanelVendorNewItems();
            this.TaskPanel.refreshTaskPanelUpdateReward();
            if (ad.ActiveTournament != null)
            {
                this.AdventureButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_BUTTON_BOSS_HUNT, null, false));
                this.AdventureButton.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_logo_bosshunt_floater");
            }
            else
            {
                this.AdventureButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_BUTTON_ADVENTURE, null, false));
                this.AdventureButton.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "icon_logo_adventure_floater");
                this.BossHuntTicker.gameObject.SetActive(false);
            }
        }

        private void onGameplayStateChanged(GameplayState oldState, GameplayState newState)
        {
            this.Canvas.sortingOrder = this.m_originalSortingOrder;
            if (newState == GameplayState.START_CEREMONY_STEP1)
            {
                this.refreshPetNotifiers();
            }
        }

        private void onGameplayStateChangeStarted(GameplayState fromState, GameplayState targetState, float transitionDelay)
        {
            Player player = GameLogic.Binder.GameState.Player;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (targetState == GameplayState.START_CEREMONY_STEP1)
            {
                this.m_characterHpIndicators[activeDungeon.PrimaryPlayerCharacter].refreshCurrentHp(true);
            }
            else if (targetState == GameplayState.START_CEREMONY_STEP2)
            {
                if (player.hasCompletedTutorial("TUT004A"))
                {
                    this.openCloseSkillBar(true, ConfigUi.MENU_TRANSITION_DURATION);
                }
            }
            else if ((targetState == GameplayState.ROOM_COMPLETION) || (targetState == GameplayState.RETIREMENT))
            {
                this.m_characterHpIndicators[activeDungeon.PrimaryPlayerCharacter].gameObject.SetActive(false);
            }
            else if (targetState == GameplayState.ACTION)
            {
                this.m_characterHpIndicators[activeDungeon.PrimaryPlayerCharacter].gameObject.SetActive(true);
                if (activeDungeon.ActiveTournament != null)
                {
                    this.BossHuntTicker.gameObject.SetActive(true);
                }
            }
            if (((targetState == GameplayState.ACTION) || (targetState == GameplayState.BOSS_FIGHT)) || (targetState == GameplayState.BOSS_START))
            {
                this.BuffTimerGrid.gameObject.SetActive(true);
            }
            else
            {
                this.BuffTimerGrid.gameObject.SetActive(false);
            }
        }

        private void onGameStateInitialized()
        {
            this.FloaterText.prewarm();
            this.openSlidingPanelArrowRoot(0f, Easing.Function.LINEAR);
            this.refreshAdventureButton();
            this.TaskPanel.refreshTaskPanelMysteryAndBossChests();
            this.TaskPanel.refreshTaskPanelUnclaimedLevelUpRewards();
        }

        public void onHeroButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                if ((PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingTaskPanel) || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingAdventurePanel))
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
                else
                {
                    GameLogic.Binder.GameState.Player.TrackingData.NumMainMenuOpensTopButton++;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingTaskPanel, MenuContentType.NONE, null, 0f, false, true);
                }
            }
        }

        public void onInventoryButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingInventoryMenu, MenuContentType.NONE, null, 0f, false, true);
            }
        }

        private void onItemInspected(ItemInstance itemInstance)
        {
            this.getInventorySkillHudButton().refreshNotifiers(true);
        }

        private void onItemRankUpped(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free)
        {
            this.m_pendingResourceBarReset = true;
        }

        private void onItemSold(CharacterInstance character, ItemInstance itemInstance, double amount, RectTransform flyToHudOrigin)
        {
            if (flyToHudOrigin != null)
            {
                Vector2 sourceScreenPos = RectTransformUtility.WorldToScreenPoint(PlayerView.Binder.MenuSystem.MenuCamera, flyToHudOrigin.position);
                this.flyToHudCoinGain(amount, sourceScreenPos, true);
            }
        }

        public void onMenuArrowButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                if ((PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingTaskPanel) || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingAdventurePanel))
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
                else if (PlayerView.Binder.SlidingTaskPanelController.SlidingPanel.canBeOpened())
                {
                    GameLogic.Binder.GameState.Player.TrackingData.NumMainMenuOpensArrowButton++;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingTaskPanel, MenuContentType.NONE, null, 0f, false, true);
                }
            }
        }

        private void onMenuChanged(Menu sourceMenu, Menu targetMenu)
        {
            if ((targetMenu != null) && (targetMenu.MenuType == MenuType.SlidingInventoryMenu))
            {
                this.SkillButtonContainerTm.gameObject.SetActive(false);
            }
        }

        private void onMenuChangeStarted(MenuType sourceMenuType, MenuType targetMenuType)
        {
            if ((sourceMenuType == MenuType.SlidingInventoryMenu) && (((targetMenuType == MenuType.NONE) || (targetMenuType == MenuType.SlidingTaskPanel)) || (targetMenuType == MenuType.SlidingAdventurePanel)))
            {
                this.SkillButtonContainerTm.gameObject.SetActive(true);
            }
            if (((sourceMenuType == MenuType.SlidingTaskPanel) || (sourceMenuType == MenuType.SlidingAdventurePanel)) && ((targetMenuType == MenuType.NONE) || (targetMenuType == MenuType.SlidingInventoryMenu)))
            {
                if (sourceMenuType == MenuType.SlidingTaskPanel)
                {
                    this.shiftRootPanelToCenter(!PlayerView.Binder.SlidingTaskPanelController.LastClosingTriggeredFromSwipe ? Easing.Function.IN_CUBIC : Easing.Function.OUT_CUBIC);
                }
                else if (sourceMenuType == MenuType.SlidingAdventurePanel)
                {
                    this.shiftRootPanelToCenter(!PlayerView.Binder.SlidingAdventurePanelController.LastClosingTriggeredFromSwipe ? Easing.Function.IN_CUBIC : Easing.Function.OUT_CUBIC);
                }
            }
            else if (targetMenuType == MenuType.SlidingTaskPanel)
            {
                if (!PlayerView.Binder.SlidingTaskPanelController.PanningActive)
                {
                    this.shiftRootPanelToRight(Easing.Function.OUT_CUBIC);
                }
            }
            else if ((targetMenuType == MenuType.SlidingAdventurePanel) && !PlayerView.Binder.SlidingAdventurePanelController.PanningActive)
            {
                this.shiftRootPanelToLeft(Easing.Function.OUT_CUBIC);
            }
        }

        private void onMissionInspected(Player player, MissionInstance mission)
        {
            if (mission.MissionType == MissionType.PromotionEvent)
            {
                this.refreshPromotionEventsButton();
            }
        }

        private void onMissionStarted(Player player, MissionInstance mission)
        {
            if (mission.MissionType == MissionType.PromotionEvent)
            {
                this.refreshPromotionEventsButton();
            }
        }

        private void onMultikillBonusGranted(Player player, int killCount, double coinAmount)
        {
            this.FloaterText.showOrRefresh(killCount);
            Vector2 sourceScreenPos = RectTransformUtility.WorldToScreenPoint(this.Canvas.worldCamera, this.FloaterText.transform.position);
            this.flyToHudCoinGain(coinAmount, sourceScreenPos, false);
        }

        private void onPetGained(Player player, string petId, bool cheated)
        {
            this.refreshPetNotifiers();
            this.applyTutorialRestrictions();
        }

        private void onPetInspected(Player player, string petId, bool cheated)
        {
            this.refreshPetNotifiers();
        }

        private void onPetLevelUpped(Player player, string petId, bool cheated)
        {
            this.refreshPetNotifiers();
            this.applyTutorialRestrictions();
        }

        public void onPetsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.PetPopupContent, null, 0f, false, true);
            }
        }

        private void onPlayerActiveCharacterSwitched(CharacterInstance activeCharacter)
        {
        }

        private void onPlayerRankUpped(Player player, bool cheated)
        {
            if (cheated)
            {
                this.PlayerXpBar.initialize(player);
                this.TaskPanel.refreshTaskPanelUnclaimedLevelUpRewards();
                this.applyTutorialRestrictions();
            }
        }

        private void onPromotionEventEnded(Player player, string promotionId)
        {
            this.refreshPromotionEventsButton();
        }

        private void onPromotionEventInspected(Player player, string promotionId)
        {
            this.refreshPromotionEventsButton();
        }

        private void onPromotionEventRefreshed(Player player, string promotionId)
        {
            this.refreshPromotionEventsButton();
        }

        public void onPromotionEventsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.PromotionEventPopupContent, null, 0f, false, true);
            }
        }

        private void onPromotionEventStarted(Player player, string promotionId)
        {
            this.refreshPromotionEventsButton();
        }

        private void onResourcesGained(Player player, ResourceType resourceType, double amount, bool visualizationManuallyControlled, string analyticsSourceId, Vector3? worldPt)
        {
            if (worldPt.HasValue)
            {
                if (resourceType == ResourceType.Coin)
                {
                    this.flyToHudCoinGain(amount, worldPt.Value, false);
                }
                else if (resourceType == ResourceType.Diamond)
                {
                    this.flyToHudDiamondGain(amount, worldPt.Value, false);
                }
                else if (resourceType == ResourceType.Xp)
                {
                    this.flyToHudXpGain(amount, worldPt.Value, false, false);
                }
            }
            if (!visualizationManuallyControlled)
            {
                if (resourceType == ResourceType.Coin)
                {
                    this.CoinAmount.queue(amount);
                    if (amount > 0.0)
                    {
                        this.CoinPulseGraphic.play();
                        this.CoinIconScalePulse.play();
                        this.CoinAmountScalePulse.play();
                    }
                }
                else if (resourceType == ResourceType.Diamond)
                {
                    this.DiamondAmount.queue(amount);
                    if (amount > 0.0)
                    {
                        this.DiamondPulseGraphic.play();
                        this.DiamondIconScalePulse.play();
                        this.DiamondAmountScalePulse.play();
                    }
                }
                else if (resourceType == ResourceType.Token)
                {
                    this.TokenAmount.queue(amount);
                    if (amount > 0.0)
                    {
                        this.TokenPulseGraphic.play();
                        this.TokenIconScalePulse.play();
                        this.TokenAmountScalePulse.play();
                    }
                }
                else if ((resourceType == ResourceType.Xp) && (amount > 0.0))
                {
                    this.PlayerXpBar.queue(amount);
                    this.PlayerXpBar.PlayerRankPulseGraphic.play();
                    this.PlayerXpBar.PlayerRankScalePulse.play();
                }
            }
            this.refreshSkillButtons();
            if (resourceType == ResourceType.Token)
            {
                this.refreshVendorNotifiers();
                this.refreshAdventureButton();
            }
        }

        public void onRetirementButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.AscendPopupContent, null, 0f, false, true);
            }
        }

        private void onRewardConsumed(Player player, Reward drop)
        {
            if (drop.isWrappedInsideChest())
            {
                this.refreshAdventureButton();
                this.TaskPanel.refreshTaskPanelMysteryAndBossChests();
                this.TaskPanel.refreshTaskPanelTournamentMainReward();
                this.TaskPanel.refreshTaskPanelTournamentContributorReward();
                this.TaskPanel.refreshTaskPanelTournamentCardPack();
            }
        }

        public void onSkillButtonClicked(int group)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (activeDungeon != null)
            {
                SkillInstance skillInstance = this.m_skillButtons[group].SkillInstance;
                if ((skillInstance != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
                {
                    CharacterInstance primaryPlayerCharacter = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter;
                    SkillType skillType = skillInstance.SkillType;
                    if ((!primaryPlayerCharacter.IsDead && !primaryPlayerCharacter.isBlinking()) && !primaryPlayerCharacter.isExecutingSkill(skillType))
                    {
                        if (UnityUtils.CoroutineRunning(ref this.m_skillActivationRoutine))
                        {
                            UnityEngine.Debug.LogWarning("Skill activation already active.");
                        }
                        else
                        {
                            this.m_skillActivationRoutine = UnityUtils.StartCoroutine(this, this.skillActivationRoutine(skillType));
                        }
                    }
                }
            }
        }

        public void onSkillButtonDraggingEnded(SkillType skillType, Vector3 worldPos)
        {
            GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter.ManualTargetPos = worldPos;
        }

        public void onSkillButtonDraggingStarted(SkillType skillType, int skillIndex)
        {
            this.onSkillButtonClicked(skillIndex);
        }

        public void onSkillsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.SkillPopupContent, null, 0f, false, true);
            }
        }

        public void onTimescaleButtonClicked()
        {
            float[] array = new float[] { 1f, 2f, 4f, 8f };
            int index = Array.IndexOf<float>(array, Time.timeScale);
            if (index < 0)
            {
                index = 0;
            }
            float targetTimescale = array[(index + 1) % array.Length];
            GameLogic.Binder.TimeSystem.speedCheat(targetTimescale);
            this.SpeedCheatButton.Text.text = targetTimescale.ToString("0.0") + "x";
        }

        public void onTournamentsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                if ((PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingTaskPanel) || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingAdventurePanel))
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
                else
                {
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingAdventurePanel, MenuContentType.NONE, null, 0f, false, true);
                }
            }
        }

        private void onTutorialCompleted(Player player, string tutorialId)
        {
            this.applyTutorialRestrictions();
        }

        public void onVendorButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && GameLogic.Binder.GameState.Player.shopUnlocked())
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.VendorPopupContent, null, 0f, false, true);
            }
        }

        public void openCloseCutsceneBorders(bool open, float duration)
        {
            this.CutsceneTop.gameObject.SetActive(true);
            this.CutsceneBottom.gameObject.SetActive(true);
            if (open)
            {
                this.CutsceneTop.open(duration, ConfigTutorials.CUTSCENE_BORDER_EASING_OPEN, 0f);
                this.CutsceneBottom.open(duration, ConfigTutorials.CUTSCENE_BORDER_EASING_OPEN, 0f);
            }
            else
            {
                this.CutsceneTop.close(duration, ConfigTutorials.CUTSCENE_BORDER_EASING_CLOSE, 0f);
                this.CutsceneBottom.close(duration, ConfigTutorials.CUTSCENE_BORDER_EASING_CLOSE, 0f);
            }
        }

        public void openCloseSkillBar(bool open, float duration)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (open)
            {
                for (int i = 0; i < this.m_skillButtons.Count; i++)
                {
                    float delay = ConfigUi.MENU_ELEMENT_DROP_INTERVAL;
                    switch (i)
                    {
                        case 0:
                        case 3:
                            delay += ConfigUi.MENU_ELEMENT_DROP_INTERVAL * 2f;
                            break;
                    }
                    this.m_skillButtons[i].OffscreenOpenClose.open(duration, ConfigUi.MENU_EASING_OUT, delay);
                }
                this.CombatStats.gameObject.SetActive(player.combatStatsEnabled());
                this.openSlidingPanelArrowRoot(duration, ConfigUi.MENU_EASING_OUT);
            }
            else
            {
                for (int j = 0; j < this.m_skillButtons.Count; j++)
                {
                    float num4 = ConfigUi.MENU_ELEMENT_DROP_INTERVAL;
                    switch (j)
                    {
                        case 1:
                        case 2:
                            num4 += ConfigUi.MENU_ELEMENT_DROP_INTERVAL;
                            break;
                    }
                    this.m_skillButtons[j].OffscreenOpenClose.close(duration, ConfigUi.MENU_EASING_IN, num4);
                }
                this.CombatStats.gameObject.SetActive(false);
                this.closeSlidingPanelArrowRoot(duration, ConfigUi.MENU_EASING_IN);
            }
        }

        public void openCloseTopPanel(bool open, float duration)
        {
            if (open && !this.TopPanel.IsOpen)
            {
                PlayerView.Binder.EventBus.DungeonHudProgressBarShowingStarted();
                this.TopPanel.open(duration, ConfigUi.MENU_EASING_OUT, 0f);
            }
            else if (!open)
            {
                PlayerView.Binder.EventBus.DungeonHudProgressBarHidingStarted();
                this.TopPanel.close(duration, ConfigUi.MENU_EASING_IN, 0f);
            }
        }

        public void openSlidingPanelArrowRoot(float duration, Easing.Function easing)
        {
            if (ConfigUi.DHUD_PANEL_ARROW_VISIBLE)
            {
                this.SlidingPanelArrowRoot.open(duration, easing, 0f);
                this.SlidingPanelArrowNotifier.enabled = true;
            }
        }

        public void overrideSkillButtonInteractState(int index, bool interactable)
        {
            if (this.m_overridenSkillHudButtonInteractable.ContainsKey(this.m_skillButtons[index]))
            {
                this.m_overridenSkillHudButtonInteractable[this.m_skillButtons[index]] = interactable;
            }
            else
            {
                this.m_overridenSkillHudButtonInteractable.Add(this.m_skillButtons[index], interactable);
            }
        }

        public void refreshAdventureButton()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player.hasRetired() || player.canRetire())
            {
                this.AdventureButton.Button.interactable = true;
                this.AdventureButton.CanvasGroup.alpha = 1f;
                this.AdventureButton.ButtonImage.material = null;
                this.AdventureButton.Icon.material = null;
            }
            else
            {
                this.AdventureButton.Button.interactable = false;
                this.AdventureButton.CanvasGroup.alpha = 0.5f;
                this.AdventureButton.ButtonImage.material = PlayerView.Binder.DisabledUiMaterial;
                this.AdventureButton.Icon.material = PlayerView.Binder.DisabledUiMaterial;
            }
            bool flag = this.AdventureButton.Button.interactable && (PlayerView.Binder.InputSystem.getInputRequirement(InputSystem.Layer.LocationEndTransition) != InputSystem.Requirement.MustBeDisabled);
            bool flag2 = (App.Binder.ConfigMeta.RETIREMENT_NOTIFICATION_ENABLED && !player.Tournaments.hasTournamentSelected()) && !player.Notifiers.HeroRetirementsInspected;
            bool flag3 = false;
            if (App.Binder.ConfigMeta.SELL_AUGMENTATIONS_IN_ADVENTURE_PANEL)
            {
                flag3 = !player.Tournaments.hasTournamentSelected() && !player.Notifiers.AugmentationShopInspected;
            }
            bool notifyAdventurePanelTournamentTab = PlayerView.Binder.NotificationSystem.NotifyAdventurePanelTournamentTab;
            if (flag && ((flag2 || flag3) || notifyAdventurePanelTournamentTab))
            {
                this.AdventureButton.ButtonImage.sprite = this.m_topButtonSpriteGold;
                this.AdventureButton.Notifier.SetActive(true);
            }
            else
            {
                this.AdventureButton.ButtonImage.sprite = this.m_topButtonSpriteNormal;
                this.AdventureButton.Notifier.SetActive(false);
            }
        }

        private void refreshCamera(Camera camera)
        {
            float num = 2208f;
            float num2 = num / ((float) camera.pixelHeight);
            this.RectTm.sizeDelta = new Vector2(num2 * camera.pixelWidth, num2 * camera.pixelHeight);
            this.Canvas.worldCamera.transform.position = base.transform.position + new Vector3(this.RectTm.sizeDelta.x * 0.5f, this.RectTm.sizeDelta.y * 0.5f, -10f);
            this.Canvas.worldCamera.orthographicSize = this.RectTm.sizeDelta.y * 0.5f;
        }

        public void refreshFpsCounter()
        {
            this.FpsCounterRoot.SetActive(ConfigApp.CHEAT_SHOW_FPS_COUNTER);
        }

        private void refreshMenuNotifiers()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player != null)
            {
                bool flag = player.isHeroOrSkillPopupUnlocked() && (PlayerView.Binder.InputSystem.getInputRequirement(InputSystem.Layer.LocationEndTransition) != InputSystem.Requirement.MustBeDisabled);
                bool flag2 = (!App.Binder.ConfigMeta.NOTIFY_NEW_MISSIONS_DURING_WELCOME_BACK && player.HasUnlockedMissions) && player.Missions.hasUninspectedMissions();
                if (flag && flag2)
                {
                    this.MenuButton.ButtonImage.sprite = this.m_topButtonSpriteGold;
                    this.MenuButton.Notifier.SetActive(true);
                    this.SlidingPanelArrowNotifier.gameObject.SetActive(true);
                }
                else
                {
                    this.MenuButton.ButtonImage.sprite = this.m_topButtonSpriteNormal;
                    this.MenuButton.Notifier.SetActive(false);
                    this.SlidingPanelArrowNotifier.gameObject.SetActive(false);
                }
            }
        }

        public void refreshPetNotifiers()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player != null)
            {
                if ((PlayerView.Binder.InputSystem.getInputRequirement(InputSystem.Layer.LocationEndTransition) != InputSystem.Requirement.MustBeDisabled) && player.Pets.doNotify())
                {
                    this.PetsButton.ButtonImage.sprite = this.m_topButtonSpriteGold;
                    this.PetsButton.Notifier.SetActive(true);
                }
                else
                {
                    this.PetsButton.ButtonImage.sprite = this.m_topButtonSpriteNormal;
                    this.PetsButton.Notifier.SetActive(false);
                }
            }
        }

        public void refreshPromotionEventsButton()
        {
            Player player = GameLogic.Binder.GameState.Player;
            bool flag = player.PromotionEvents.hasEvents();
            this.PromotionEventsButton.CanvasGroup.gameObject.SetActive(flag);
            if (flag)
            {
                PromotionEventInstance instance = player.PromotionEvents.getNewestEventInstance();
                bool flag2 = player.PromotionEvents.hasUninspectedEvents() || instance.Missions.hasUninspectedMissions();
                this.PromotionEventsButton.Notifier.SetActive(flag2);
            }
        }

        public void refreshPromotionEventsButtonTimer()
        {
            PromotionEventInstance instance = GameLogic.Binder.GameState.Player.PromotionEvents.getNewestEventInstance();
            if (instance != null)
            {
                this.PromotionEventsButton.Text.gameObject.SetActive(!instance.getData().HideTimer);
                this.PromotionEventsButton.Text.text = instance.getTimerString();
            }
        }

        private void refreshSkillButtons()
        {
            Player player = GameLogic.Binder.GameState.Player;
            bool flag = player.hasCompletedTutorial("TUT050B");
            for (int i = 0; i < this.m_skillButtons.Count; i++)
            {
                SkillHudButton button = this.m_skillButtons[i];
                if (player.ActiveCharacter.Stunned)
                {
                    button.refresh(!this.m_skillButtonHidden[i], false);
                }
                else if (button.IsInventoryButton)
                {
                    if (flag)
                    {
                        button.refresh(!this.m_skillButtonHidden[i], true);
                        button.refreshNotifiers(!this.m_skillButtonHidden[i]);
                    }
                    else
                    {
                        button.refresh(false, false);
                        button.refreshNotifiers(false);
                    }
                }
                else
                {
                    CharacterInstance activeCharacter = player.ActiveCharacter;
                    button.refresh(!this.m_skillButtonHidden[i], !activeCharacter.isExecutingSkillWithExternalControl());
                }
            }
        }

        private void refreshSkillNotifiers()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player != null)
            {
                bool flag = player.Notifiers.runestoneNotificationsActive() || (player.Notifiers.getNumberOfGoldSkillNotifications() > 0);
                bool flag2 = player.isHeroOrSkillPopupUnlocked() && (PlayerView.Binder.InputSystem.getInputRequirement(InputSystem.Layer.LocationEndTransition) != InputSystem.Requirement.MustBeDisabled);
                if (flag && flag2)
                {
                    this.SkillsButton.ButtonImage.sprite = this.m_topButtonSpriteGold;
                    this.SkillsButton.Notifier.SetActive(true);
                }
                else
                {
                    this.SkillsButton.ButtonImage.sprite = this.m_topButtonSpriteNormal;
                    this.SkillsButton.Notifier.SetActive(false);
                }
            }
        }

        public void refreshVendorNotifiers()
        {
            if (GameLogic.Binder.GameState.Player != null)
            {
                if (this.shouldNotifyShop())
                {
                    this.VendorButton.ButtonImage.sprite = this.m_topButtonSpriteGold;
                    this.VendorButton.Notifier.SetActive(true);
                }
                else
                {
                    this.VendorButton.ButtonImage.sprite = this.m_topButtonSpriteNormal;
                    this.VendorButton.Notifier.SetActive(false);
                }
            }
        }

        private void removeHpIndicator(CharacterInstance c)
        {
            if (this.m_characterHpIndicators.ContainsKey(c))
            {
                HpIndicator indicator = this.m_characterHpIndicators[c];
                this.m_characterHpIndicators.Remove(c);
                PlayerView.Binder.HpIndicatorPool.returnObject(indicator);
            }
        }

        public void resetResourceBar()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.m_gameplayResourceGainVisualizer.emptyAllActiveAnimationResourceChunks();
            this.m_menuResourceGainVisualizer.emptyAllActiveAnimationResourceChunks();
            this.CoinAmount.set(player.getResourceAmount(ResourceType.Coin), true);
            this.DiamondAmount.set(player.getResourceAmount(ResourceType.Diamond), true);
            this.TokenAmount.set(player.getResourceAmount(ResourceType.Token), true);
            this.PlayerXpBar.initialize(player);
        }

        public void setCharacterHpIndicatorVisibility(CharacterInstance character, bool visible)
        {
            this.m_characterHpIndicators[character].gameObject.SetActive(visible);
        }

        public void setElementVisibility(bool visible, [Optional, DefaultParameterValue(false)] bool instant)
        {
            if (visible)
            {
                this.SkillButtonContainerTm.gameObject.SetActive(true);
            }
            if (UnityUtils.CoroutineRunning(ref this.m_mainElementVisibilityAnimationRoutine))
            {
                UnityUtils.StopCoroutine(this, ref this.m_mainElementVisibilityAnimationRoutine);
                instant = true;
            }
            this.m_mainElementVisibilityAnimationRoutine = UnityUtils.StartCoroutine(this, this.mainElementVisibilityAnimationRoutine(visible, instant));
        }

        public void setSkillHudButtonSoftLock(int idx, bool locked)
        {
            this.m_skillButtonHidden[idx] = locked;
        }

        public void shiftRootPanelToCenter([Optional, DefaultParameterValue(4)] Easing.Function easing)
        {
            this.RootPanelTa.stopAll();
            if (easing == Easing.Function.OUT_CUBIC)
            {
                this.m_rootPanelMoveToCenterTask_OutCubic.reset();
                this.RootPanelTa.addTask(this.m_rootPanelMoveToCenterTask_OutCubic);
            }
            else
            {
                this.m_rootPanelMoveToCenterTask_InCubic.reset();
                this.RootPanelTa.addTask(this.m_rootPanelMoveToCenterTask_InCubic);
            }
            this.openSlidingPanelArrowRoot(ConfigUi.SLIDING_PANEL_EXIT_DURATION, easing);
        }

        public void shiftRootPanelToLeft([Optional, DefaultParameterValue(5)] Easing.Function easing)
        {
            this.RootPanelTa.stopAll();
            this.m_rootPanelMoveToLeftTask.reset();
            this.RootPanelTa.addTask(this.m_rootPanelMoveToLeftTask);
            this.closeSlidingPanelArrowRoot(ConfigUi.SLIDING_PANEL_ENTRY_DURATION, easing);
        }

        public void shiftRootPanelToRight([Optional, DefaultParameterValue(5)] Easing.Function easing)
        {
            this.RootPanelTa.stopAll();
            this.m_rootPanelMoveToRightTask.reset();
            this.RootPanelTa.addTask(this.m_rootPanelMoveToRightTask);
            this.closeSlidingPanelArrowRoot(ConfigUi.SLIDING_PANEL_ENTRY_DURATION, easing);
        }

        public bool shouldNotifyShop()
        {
            if (PlayerView.Binder.InputSystem.getInputRequirement(InputSystem.Layer.LocationEndTransition) == InputSystem.Requirement.MustBeDisabled)
            {
                return false;
            }
            Player player = GameLogic.Binder.GameState.Player;
            bool flag2 = false;
            if (!App.Binder.ConfigMeta.SELL_AUGMENTATIONS_IN_ADVENTURE_PANEL)
            {
                flag2 = !player.Tournaments.hasTournamentSelected() && !player.Notifiers.AugmentationShopInspected;
            }
            bool flag3 = false;
            if (!App.Binder.ConfigMeta.ALLOW_VENDOR_TASKPANEL_NOTIFIER)
            {
                flag3 = Service.Binder.AdsSystem.adReady() && !player.Vendor.InventoryInspected;
            }
            return (flag2 || flag3);
        }

        private void showDamageCombatText(CharacterInstance source, CharacterInstance c, Vector3 worldPos, double dmg, bool critted, bool damageReduced, DamageType damageType)
        {
            if (!ConfigApp.CHEAT_MARKETING_MODE_ENABLED || !PlayerView.Binder.MarketingBuildController.CombatTextDisabled)
            {
                Color gray;
                CombatTextIngame item = PlayerView.Binder.CombatTextPoolIngame.getObject();
                item.gameObject.SetActive(true);
                item.initialize(PlayerView.Binder.RoomView.RoomCamera.Camera);
                this.m_activeIngameCombatTexts.Add(item);
                item.EndCallback = new Action<CombatTextIngame>(this.combatTextIngameEndCallback);
                if (c.IsPlayerCharacter)
                {
                    if (damageReduced && (dmg == 0.0))
                    {
                        gray = Color.gray;
                        item.setFontSize(40);
                    }
                    else if (critted)
                    {
                        gray = new Color(1f, 0.5882353f, 0f, 1f);
                        item.setFontSize(0x38);
                    }
                    else
                    {
                        gray = Color.red;
                        item.setFontSize(40);
                    }
                }
                else if (critted)
                {
                    gray = Color.yellow;
                    item.setFontSize(0x38);
                }
                else if (damageReduced)
                {
                    gray = Color.gray;
                    item.setFontSize(40);
                }
                else
                {
                    if ((source != null) && source.IsPet)
                    {
                        gray = ConfigUi.COMBAT_TEXT_PET_DAMAGE_COLOR;
                    }
                    else if (damageType == DamageType.Magic)
                    {
                        gray = ConfigUi.COMBAT_TEXT_MAGIC_DAMAGE_COLOR;
                    }
                    else
                    {
                        gray = Color.white;
                    }
                    item.setFontSize(40);
                }
                if (damageReduced && (dmg == 0.0))
                {
                    item.setText(StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMBAT_TEXT_DODGE, null, false)), gray);
                }
                else
                {
                    item.setText(MenuHelpers.BigValueToString(dmg), gray);
                }
                item.Tm.position = worldPos + ConfigUi.COMBAT_TEXT_WORLD_OFFSET;
                if (critted)
                {
                    item.show(0.35f, 1f, 1f);
                }
                else
                {
                    item.show(0.05f, 0.2f, 4f);
                }
            }
        }

        private void showHealingCombatText(Vector3 worldPoint, double healAmount)
        {
            if (!ConfigApp.CHEAT_MARKETING_MODE_ENABLED || !PlayerView.Binder.MarketingBuildController.CombatTextDisabled)
            {
                CombatTextIngame item = PlayerView.Binder.CombatTextPoolIngame.getObject();
                item.gameObject.SetActive(true);
                item.initialize(PlayerView.Binder.RoomView.RoomCamera.Camera);
                this.m_activeIngameCombatTexts.Add(item);
                item.EndCallback = new Action<CombatTextIngame>(this.combatTextIngameEndCallback);
                item.setText("+", Color.green);
                item.setFontSize(70);
                item.Tm.position = worldPoint + ConfigUi.COMBAT_TEXT_WORLD_OFFSET;
                item.show(0.05f, 0.2f, 4f);
            }
        }

        public void showInfoCombatText(Vector3 worldPoint, string text)
        {
            if (!ConfigApp.CHEAT_MARKETING_MODE_ENABLED || !PlayerView.Binder.MarketingBuildController.CombatTextDisabled)
            {
                CombatTextIngame item = PlayerView.Binder.CombatTextPoolIngame.getObject();
                item.gameObject.SetActive(true);
                item.initialize(PlayerView.Binder.RoomView.RoomCamera.Camera);
                this.m_activeIngameCombatTexts.Add(item);
                item.EndCallback = new Action<CombatTextIngame>(this.combatTextIngameEndCallback);
                item.setText(text, new Color(0.3764706f, 0.8196079f, 1f, 1f));
                item.setFontSize(50);
                item.Tm.position = worldPoint + ConfigUi.COMBAT_TEXT_WORLD_OFFSET;
                item.show(0.35f, 1f, 1f);
            }
        }

        private void showResourceGainCombatText(Vector2 screenPos, double amount, ResourceType resourceType)
        {
            if (!ConfigApp.CHEAT_MARKETING_MODE_ENABLED || !PlayerView.Binder.MarketingBuildController.CombatTextDisabled)
            {
                Vector3 vector = PlayerView.Binder.MenuSystem.MenuCamera.ScreenToWorldPoint((Vector3) screenPos);
                vector.z = 0f;
                CombatTextMenu item = PlayerView.Binder.CombatTextPoolMenu.getObject();
                item.RectTm.SetParent(this.RectTm);
                item.RectTm.SetSiblingIndex(this.RectTm.childCount - 2);
                item.gameObject.SetActive(true);
                item.initialize();
                this.m_activeMenuCombatTexts.Add(item);
                item.EndCallback = new Action<CombatTextMenu>(this.combatTextMenuEndCallback);
                item.setText("+" + MenuHelpers.BigValueToString(amount));
                if (resourceType == ResourceType.Coin)
                {
                    item.setColor(ConfigUi.COIN_COLOR);
                }
                else if (resourceType == ResourceType.Diamond)
                {
                    item.setColor(ConfigUi.DIAMOND_COLOR);
                }
                else if (resourceType == ResourceType.Token)
                {
                    item.setColor(ConfigUi.TOKEN_COLOR);
                }
                else if (resourceType == ResourceType.Crown)
                {
                    item.setColor(ConfigUi.CROWN_COLOR);
                }
                else if (resourceType == ResourceType.Dust)
                {
                    item.setColor(ConfigUi.DUST_COLOR);
                }
                else if (resourceType == ResourceType.Xp)
                {
                    item.setColor(ConfigUi.XP_COLOR);
                }
                item.RectTm.position = vector;
                item.show();
            }
        }

        [DebuggerHidden]
        private IEnumerator skillActivationRoutine(SkillType skillType)
        {
            <skillActivationRoutine>c__IteratorFC rfc = new <skillActivationRoutine>c__IteratorFC();
            rfc.skillType = skillType;
            rfc.<$>skillType = skillType;
            rfc.<>f__this = this;
            return rfc;
        }

        protected void Update()
        {
            GameState gameState = GameLogic.Binder.GameState;
            if ((gameState.ActiveDungeon != null) && (gameState.ActiveDungeon.ActiveRoom != null))
            {
                this.refreshSkillButtons();
                this.refreshMenuNotifiers();
                this.refreshSkillNotifiers();
                this.refreshVendorNotifiers();
                this.refreshPromotionEventsButtonTimer();
                if (this.FrenzyBar.gameObject.activeSelf)
                {
                    this.FrenzyBar.setNormalizedValue(GameLogic.Binder.FrenzySystem.getNormalizedFrenzyGauge());
                }
                if (Time.time > this.m_resetCheatOptionsClickTime)
                {
                    this.m_numCheatOptionsClicks = 0;
                    this.m_resetCheatOptionsClickTime = Time.time + 2.5f;
                }
                if (ConfigApp.CHEAT_MARKETING_MODE_ENABLED)
                {
                    foreach (KeyValuePair<CharacterInstance, HpIndicator> pair in this.m_characterHpIndicators)
                    {
                        pair.Value.RectTm.localScale = !PlayerView.Binder.MarketingBuildController.HpIndicatorsDisabled ? Vector3.one : Vector3.zero;
                    }
                }
                Service.Binder.AchievementThirdPartyService.UnlockAchievements();
                if (this.CombatStats.gameObject.activeSelf && (Time.unscaledTime >= this.m_nextCombatStatUpdate))
                {
                    double num;
                    double num2;
                    double num3;
                    double num4;
                    GameLogic.Binder.HeroStatRecordingSystem.RealtimeCombatStats.getStats(out num, out num2, out num3, out num4);
                    double v = (num + num2) + num3;
                    this.CombatStats.DpsTotalValue.text = MenuHelpers.BigValueToString(v);
                    string[] textArray1 = new string[] { MenuHelpers.BigValueToString(num), "\n", MenuHelpers.BigValueToString(num2), "\n", MenuHelpers.BigValueToString(num3) };
                    this.CombatStats.DpsBreakdownValue.text = string.Concat(textArray1);
                    this.CombatStats.CpsValue.text = MenuHelpers.BigValueToString(num4);
                    this.m_nextCombatStatUpdate = Time.unscaledTime + 2f;
                }
                if ((this.m_pendingResourceBarReset && !this.m_gameplayResourceGainVisualizer.HasActiveAnimations) && !this.m_menuResourceGainVisualizer.HasActiveAnimations)
                {
                    this.resetResourceBar();
                    this.m_pendingResourceBarReset = false;
                }
            }
        }

        public bool Animating
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_mainElementVisibilityAnimationRoutine);
            }
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

        public bool IsOpen
        {
            get
            {
                return (this.SkillBarOpen && this.TopPanel.IsOpen);
            }
        }

        public float PanelOriginalPosX
        {
            [CompilerGenerated]
            get
            {
                return this.<PanelOriginalPosX>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PanelOriginalPosX>k__BackingField = value;
            }
        }

        public float PanelOriginalWidth
        {
            [CompilerGenerated]
            get
            {
                return this.<PanelOriginalWidth>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PanelOriginalWidth>k__BackingField = value;
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

        public bool SkillBarAnimating
        {
            get
            {
                for (int i = 0; i < this.m_skillButtons.Count; i++)
                {
                    if (this.m_skillButtons[i].gameObject.activeSelf && this.m_skillButtons[i].OffscreenOpenClose.Animating)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool SkillBarOpen
        {
            get
            {
                for (int i = 0; i < this.m_skillButtons.Count; i++)
                {
                    if (this.m_skillButtons[i].gameObject.activeSelf && !this.m_skillButtons[i].OffscreenOpenClose.IsOpen)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public List<SkillHudButton> SkillHudButtons
        {
            get
            {
                return this.m_skillButtons;
            }
        }

        [CompilerGenerated]
        private sealed class <addSkillHudButton>c__AnonStorey2E8
        {
            internal DungeonHud <>f__this;
            internal int hudIndex;

            internal void <>m__187()
            {
                this.<>f__this.onInventoryButtonClicked();
            }

            internal void <>m__188()
            {
                this.<>f__this.onSkillButtonClicked(this.hudIndex);
            }
        }

        [CompilerGenerated]
        private sealed class <flyToHud>c__IteratorFA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>doFlyResourcesToHud;
            internal bool <$>fromMenu;
            internal Reward <$>reward;
            internal Vector2 <$>sourceScreenPos;
            internal DungeonHud <>f__this;
            internal IEnumerator <ie>__0;
            internal bool doFlyResourcesToHud;
            internal bool fromMenu;
            internal Reward reward;
            internal Vector2 sourceScreenPos;

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
                    {
                        List<Reward> rewards = new List<Reward>();
                        rewards.Add(this.reward);
                        this.<ie>__0 = this.<>f__this.flyToHud(rewards, this.sourceScreenPos, this.fromMenu, this.doFlyResourcesToHud);
                        break;
                    }
                    case 1:
                        break;

                    default:
                        goto Label_0090;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                this.$PC = -1;
            Label_0090:
                return false;
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
        private sealed class <flyToHud>c__IteratorFB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>doFlyResourcesToHud;
            internal bool <$>fromMenu;
            internal List<Reward> <$>rewards;
            internal Vector2 <$>sourceScreenPos;
            internal DungeonHud <>f__this;
            internal List<double> <chunks>__27;
            internal ConfigRunestones.SharedData <data>__11;
            internal int <i>__25;
            internal int <i>__28;
            internal int <i>__6;
            internal int <i>__8;
            internal int <i>__9;
            internal IEnumerator <ie>__13;
            internal IEnumerator <ie>__15;
            internal IEnumerator <ie>__17;
            internal IEnumerator <ie>__19;
            internal IEnumerator <ie>__26;
            internal ItemInstance <ii>__7;
            internal int <j>__14;
            internal int <j>__16;
            internal int <j>__18;
            internal int <j>__20;
            internal int <petFlyToHudCount>__2;
            internal Player <player>__21;
            internal int <r>__1;
            internal int <r>__4;
            internal Reward <reward>__5;
            internal string <runestoneId>__10;
            internal double <tokens>__12;
            internal double <totalAmount>__23;
            internal double <totalConvertedPetCoins>__3;
            internal int <totalRunestonesInAllChests>__0;
            internal double <xpPerBurst>__24;
            internal double <xpRequiredForRankUp>__22;
            internal bool doFlyResourcesToHud;
            internal bool fromMenu;
            internal List<Reward> rewards;
            internal Vector2 sourceScreenPos;

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
                        this.<totalRunestonesInAllChests>__0 = 0;
                        this.<r>__1 = 0;
                        while (this.<r>__1 < this.rewards.Count)
                        {
                            this.<totalRunestonesInAllChests>__0 += this.rewards[this.<r>__1].RunestoneDrops.Count;
                            this.<r>__1++;
                        }
                        this.<petFlyToHudCount>__2 = 0;
                        this.<totalConvertedPetCoins>__3 = 0.0;
                        this.<r>__4 = 0;
                        while (this.<r>__4 < this.rewards.Count)
                        {
                            this.<reward>__5 = this.rewards[this.<r>__4];
                            this.<i>__6 = 0;
                            while (this.<i>__6 < this.<reward>__5.ItemDrops.Count)
                            {
                                this.<ii>__7 = this.<reward>__5.ItemDrops[this.<i>__6];
                                this.<>f__this.flyToHudItemGain(this.<ii>__7, this.sourceScreenPos, false, this.fromMenu);
                                this.<i>__6++;
                            }
                            if (this.<reward>__5.Revives > 0)
                            {
                                this.<>f__this.flyToHudPotionGain(PotionType.Revive, this.sourceScreenPos, false, this.fromMenu);
                            }
                            if (this.<reward>__5.FrenzyPotions > 0)
                            {
                                this.<>f__this.flyToHudPotionGain(PotionType.Frenzy, this.sourceScreenPos, false, this.fromMenu);
                            }
                            if (this.<reward>__5.BossPotions > 0)
                            {
                                this.<>f__this.flyToHudPotionGain(PotionType.Boss, this.sourceScreenPos, false, this.fromMenu);
                            }
                            this.<i>__8 = 0;
                            while (this.<i>__8 < this.<reward>__5.Pets.Count)
                            {
                                if (!string.IsNullOrEmpty(this.<reward>__5.Pets[this.<i>__8].ConvertIntoShopEntryId))
                                {
                                    this.<totalConvertedPetCoins>__3 += ConfigShops.CalculateCoinBundleSize(GameLogic.Binder.GameState.Player, this.<reward>__5.Pets[this.<i>__8].ConvertIntoShopEntryId, this.<reward>__5.Pets[this.<i>__8].Amount);
                                }
                                else if (this.<petFlyToHudCount>__2 < 4)
                                {
                                    this.<>f__this.flyToHudPetGain(this.<reward>__5.Pets[this.<i>__8].PetId, this.sourceScreenPos, false, this.fromMenu);
                                    this.<petFlyToHudCount>__2++;
                                }
                                this.<i>__8++;
                            }
                            this.<i>__9 = 0;
                            while (this.<i>__9 < this.<reward>__5.RunestoneDrops.Count)
                            {
                                this.<runestoneId>__10 = this.<reward>__5.RunestoneDrops[this.<i>__9];
                                if (!this.<reward>__5.isRunestoneAtIndexConvertedIntoTokens(this.<i>__9))
                                {
                                    goto Label_0383;
                                }
                                this.<data>__11 = ConfigRunestones.GetRunestoneData(this.<runestoneId>__10);
                                this.<tokens>__12 = App.Binder.ConfigMeta.RunestoneTokenGainCurve(this.<data>__11.Rarity);
                                this.<>f__this.flyToHudTokenGain(this.<tokens>__12, this.sourceScreenPos, this.fromMenu, false);
                                this.<ie>__13 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.HUD_RESOURCE_GAIN_VISUALIZATION_INTERVAL);
                            Label_036E:
                                while (this.<ie>__13.MoveNext())
                                {
                                    this.$current = this.<ie>__13.Current;
                                    this.$PC = 1;
                                    goto Label_07C5;
                                }
                                goto Label_03A7;
                            Label_0383:
                                this.<>f__this.flyToHudRunestoneGain(this.<runestoneId>__10, this.sourceScreenPos, false, this.fromMenu, this.<totalRunestonesInAllChests>__0);
                            Label_03A7:
                                this.<i>__9++;
                            }
                            if (this.<reward>__5.Skill != SkillType.NONE)
                            {
                                this.<>f__this.flyToHudSkillGain(this.<reward>__5.Skill, this.sourceScreenPos, false, this.fromMenu);
                            }
                            if (this.doFlyResourcesToHud)
                            {
                                this.<j>__14 = 0;
                                while (this.<j>__14 < this.<reward>__5.CoinDrops.Count)
                                {
                                    this.<>f__this.flyToHudCoinGain(this.<reward>__5.CoinDrops[this.<j>__14], this.sourceScreenPos, this.fromMenu);
                                    this.<ie>__15 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.HUD_RESOURCE_GAIN_VISUALIZATION_INTERVAL);
                                Label_0479:
                                    while (this.<ie>__15.MoveNext())
                                    {
                                        this.$current = this.<ie>__15.Current;
                                        this.$PC = 2;
                                        goto Label_07C5;
                                    }
                                    this.<j>__14++;
                                }
                                this.<j>__16 = 0;
                                while (this.<j>__16 < this.<reward>__5.DiamondDrops.Count)
                                {
                                    this.<>f__this.flyToHudDiamondGain(this.<reward>__5.DiamondDrops[this.<j>__16], this.sourceScreenPos, this.fromMenu);
                                    this.<ie>__17 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.HUD_RESOURCE_GAIN_VISUALIZATION_INTERVAL);
                                Label_051D:
                                    while (this.<ie>__17.MoveNext())
                                    {
                                        this.$current = this.<ie>__17.Current;
                                        this.$PC = 3;
                                        goto Label_07C5;
                                    }
                                    this.<j>__16++;
                                }
                                this.<j>__18 = 0;
                                while (this.<j>__18 < this.<reward>__5.TokenDrops.Count)
                                {
                                    this.<>f__this.flyToHudTokenGain(this.<reward>__5.TokenDrops[this.<j>__18], this.sourceScreenPos, this.fromMenu, false);
                                    this.<ie>__19 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.HUD_RESOURCE_GAIN_VISUALIZATION_INTERVAL);
                                Label_05C2:
                                    while (this.<ie>__19.MoveNext())
                                    {
                                        this.$current = this.<ie>__19.Current;
                                        this.$PC = 4;
                                        goto Label_07C5;
                                    }
                                    this.<j>__18++;
                                }
                                this.<j>__20 = 0;
                                while (this.<j>__20 < this.<reward>__5.XpPotions)
                                {
                                    this.<player>__21 = GameLogic.Binder.GameState.Player;
                                    this.<xpRequiredForRankUp>__22 = App.Binder.ConfigMeta.XpRequiredForRankUp(this.<player>__21.Rank);
                                    this.<totalAmount>__23 = Math.Floor(this.<xpRequiredForRankUp>__22 * App.Binder.ConfigMeta.XP_GAIN_PER_POTION);
                                    this.<xpPerBurst>__24 = this.<totalAmount>__23 / 3.0;
                                    this.<i>__25 = 0;
                                    while (this.<i>__25 < 3)
                                    {
                                        this.<>f__this.flyToHudXpGain(this.<xpPerBurst>__24, this.sourceScreenPos, this.fromMenu, false);
                                        this.<ie>__26 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.HUD_RESOURCE_GAIN_VISUALIZATION_INTERVAL);
                                    Label_06C0:
                                        while (this.<ie>__26.MoveNext())
                                        {
                                            this.$current = this.<ie>__26.Current;
                                            this.$PC = 5;
                                            goto Label_07C5;
                                        }
                                        this.<i>__25++;
                                    }
                                    this.<j>__20++;
                                }
                            }
                            this.<r>__4++;
                        }
                        if (this.<totalConvertedPetCoins>__3 > 0.0)
                        {
                            this.<chunks>__27 = new List<double>(4);
                            MathUtil.DistributeValuesIntoChunksDouble(this.<totalConvertedPetCoins>__3, 4, ref this.<chunks>__27);
                            this.<i>__28 = 0;
                            while (this.<i>__28 < this.<chunks>__27.Count)
                            {
                                this.<>f__this.flyToHudCoinGain(this.<chunks>__27[this.<i>__28], this.sourceScreenPos, this.fromMenu);
                                this.<i>__28++;
                            }
                        }
                        this.$PC = -1;
                        break;

                    case 1:
                        goto Label_036E;

                    case 2:
                        goto Label_0479;

                    case 3:
                        goto Label_051D;

                    case 4:
                        goto Label_05C2;

                    case 5:
                        goto Label_06C0;
                }
                return false;
            Label_07C5:
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
        private sealed class <mainElementVisibilityAnimationRoutine>c__IteratorFD : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal bool <$>visible;
            internal DungeonHud <>f__this;
            internal bool instant;
            internal bool visible;

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
                        if (!this.visible)
                        {
                            this.<>f__this.openCloseSkillBar(false, !this.instant ? ConfigUi.MENU_TRANSITION_DURATION : 0f);
                            goto Label_0194;
                        }
                        this.<>f__this.openCloseTopPanel(true, !this.instant ? ConfigUi.MENU_TRANSITION_DURATION : 0f);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0194;

                    case 3:
                        goto Label_024A;

                    default:
                        goto Label_02B1;
                }
                if (this.<>f__this.TopPanel.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_02B3;
                }
                this.<>f__this.openCloseSkillBar(true, !this.instant ? ConfigUi.MENU_TRANSITION_DURATION : 0f);
                if (((GameLogic.Binder.GameState.ActiveDungeon != null) && (GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom != null)) && !GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.MainBossSummoned)
                {
                    this.<>f__this.LeftPanel.open(!this.instant ? ConfigUi.MENU_TRANSITION_DURATION : 0f, ConfigUi.MENU_EASING_OUT, 0f);
                    this.<>f__this.RightPanel.open(!this.instant ? ConfigUi.MENU_TRANSITION_DURATION : 0f, ConfigUi.MENU_EASING_OUT, 0f);
                }
                goto Label_024A;
            Label_0194:
                while (this.<>f__this.SkillBarAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_02B3;
                }
                this.<>f__this.openCloseTopPanel(false, !this.instant ? ConfigUi.MENU_TRANSITION_DURATION : 0f);
                this.<>f__this.LeftPanel.close(!this.instant ? ConfigUi.MENU_TRANSITION_DURATION : 0f, ConfigUi.MENU_EASING_IN, 0f);
                this.<>f__this.RightPanel.close(!this.instant ? ConfigUi.MENU_TRANSITION_DURATION : 0f, ConfigUi.MENU_EASING_IN, 0f);
            Label_024A:
                while ((this.<>f__this.TopPanel.Animating || this.<>f__this.SkillBarAnimating) || (this.<>f__this.LeftPanel.Animating || this.<>f__this.RightPanel.Animating))
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_02B3;
                }
                this.<>f__this.m_mainElementVisibilityAnimationRoutine = null;
                goto Label_02B1;
                this.$PC = -1;
            Label_02B1:
                return false;
            Label_02B3:
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
        private sealed class <skillActivationRoutine>c__IteratorFC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SkillType <$>skillType;
            internal DungeonHud <>f__this;
            internal int <i>__1;
            internal int <i>__2;
            internal CharacterInstance <pc>__0;
            internal SkillType skillType;

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
                        this.<pc>__0 = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter;
                        if (!ConfigSkills.SHARED_DATA[this.skillType].ManualTargeting)
                        {
                            goto Label_0192;
                        }
                        this.<i>__1 = 0;
                        while (this.<i>__1 < this.<>f__this.m_skillButtons.Count)
                        {
                            if (this.<>f__this.m_skillButtons[this.<i>__1].SkillInstance.SkillType == this.skillType)
                            {
                                this.<>f__this.m_skillButtons[this.<i>__1].startGlow();
                                break;
                            }
                            this.<i>__1++;
                        }
                        break;

                    case 1:
                        goto Label_00F8;

                    default:
                        goto Label_01C6;
                }
                this.<pc>__0.ManualTargetPos = Vector3.zero;
            Label_00F8:
                while (this.<pc>__0.ManualTargetPos == Vector3.zero)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<i>__2 = 0;
                while (this.<i>__2 < this.<>f__this.m_skillButtons.Count)
                {
                    if (this.<>f__this.m_skillButtons[this.<i>__2].SkillInstance.SkillType == this.skillType)
                    {
                        this.<>f__this.m_skillButtons[this.<i>__2].stopGlow();
                        break;
                    }
                    this.<i>__2++;
                }
            Label_0192:
                GameLogic.Binder.SkillSystem.activateSkill(this.<pc>__0, this.skillType, -1f, null);
                this.<>f__this.m_skillActivationRoutine = null;
                goto Label_01C6;
                this.$PC = -1;
            Label_01C6:
                return false;
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

        [Serializable]
        public class HudTopButton
        {
            public UnityEngine.UI.Button Button;
            public Image ButtonImage;
            public UnityEngine.CanvasGroup CanvasGroup;
            public Image Icon;
            public GameObject Notifier;
            public PulsatingGraphic PulseGraphic;
            public RectTransform RootTm;
            public PlayerView.ScalePulse ScalePulse;
            public UnityEngine.UI.Text Text;
            public RectTransform Tm;
        }
    }
}

