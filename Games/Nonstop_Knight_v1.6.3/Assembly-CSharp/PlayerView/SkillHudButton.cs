namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillHudButton : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private bool <IsInventoryButton>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private GameLogic.SkillInstance <SkillInstance>k__BackingField;
        public UnityEngine.UI.Image Borders;
        public UnityEngine.UI.Button Button;
        public ImageFlashEffect CooldownFlash;
        public UnityEngine.UI.Image CooldownOverlay;
        public Text CooldownText;
        public GameObject ExtraChargesRoot;
        public Text ExtraChargesText;
        public ParticleSystem Glow;
        public PulsatingGraphic IconPulseGraphic;
        public RectTransform IconRect;
        public ScalePulse IconScalePulse;
        public UnityEngine.UI.Image Image;
        public Shadow ImageShadow;
        private Sprite m_dottedLinesSprite;
        private Sprite m_emptyButtonSprite;
        private Sprite m_inventorySprite;
        private Vector2 m_originalBorderSize;
        private Sprite m_skillSlotSprite;
        public Text Name;
        public GameObject NotifierUnlock;
        public PulsatingGraphic NotifierUnlockPulsating;
        public GameObject NotifierUpgrade;
        public PulsatingGraphic NotifierUpgradePulsating;
        public PlayerView.OffscreenOpenClose OffscreenOpenClose;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.NotifierUnlock.SetActive(false);
            this.NotifierUpgrade.SetActive(false);
            this.NotifierUnlockPulsating.enabled = false;
            this.NotifierUpgradePulsating.enabled = false;
            this.m_emptyButtonSprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "uix_button_empty");
            Sprite sprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "icon_bag_floater");
            this.Image.sprite = sprite;
            this.m_inventorySprite = sprite;
            this.m_dottedLinesSprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "uix_skillslotlines");
            this.m_skillSlotSprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "sprite_skill_slot");
            this.ExtraChargesRoot.SetActive(false);
            this.m_originalBorderSize = RectTransformExtensions.GetSize(this.Borders.rectTransform);
        }

        public void cleanUpForReuse()
        {
            this.Button.onClick.RemoveAllListeners();
            RectTransformExtensions.SetSize(this.Borders.rectTransform, this.m_originalBorderSize);
            base.StopAllCoroutines();
        }

        public void initialize(GameLogic.SkillInstance skillInstance, bool isInventoryButton)
        {
            this.SkillInstance = skillInstance;
            this.IsInventoryButton = isInventoryButton;
            this.ImageShadow.enabled = this.IsInventoryButton;
            if (this.IsInventoryButton)
            {
                this.Name.text = string.Empty;
                this.Image.sprite = this.m_inventorySprite;
                RectTransformExtensions.SetSize(this.Borders.rectTransform, new Vector2(280f, 280f));
                this.Borders.sprite = this.m_emptyButtonSprite;
            }
            else if (skillInstance != null)
            {
                ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[skillInstance.SkillType];
                this.Name.text = StringExtensions.ToUpperLoca(_.L(data.Name, null, false));
                this.Image.sprite = PlayerView.Binder.SpriteResources.getSprite(data.Spritesheet, data.Sprite);
                this.Borders.sprite = this.m_skillSlotSprite;
                this.NotifierUnlock.SetActive(false);
                this.NotifierUnlockPulsating.enabled = false;
                this.NotifierUpgrade.SetActive(false);
                this.NotifierUpgradePulsating.enabled = false;
            }
            else
            {
                this.Image.sprite = this.m_dottedLinesSprite;
                this.NotifierUnlock.SetActive(false);
                this.NotifierUnlockPulsating.enabled = false;
                this.NotifierUpgrade.SetActive(false);
                this.NotifierUpgradePulsating.enabled = false;
            }
            this.refresh(true, false);
            this.refreshNotifiers(false);
        }

        protected void OnDisable()
        {
            this.NotifierUnlockPulsating.enabled = false;
            this.NotifierUpgradePulsating.enabled = false;
            this.NotifierUnlock.SetActive(false);
            this.NotifierUpgrade.SetActive(false);
        }

        protected void OnEnable()
        {
        }

        public void refresh(bool visible, bool interactable)
        {
            if (!visible || ((this.SkillInstance == null) && !this.IsInventoryButton))
            {
                this.Image.overrideSprite = this.m_dottedLinesSprite;
                this.Button.interactable = false;
                this.Borders.enabled = false;
                this.Name.enabled = false;
                this.CooldownOverlay.enabled = false;
                this.CooldownText.enabled = false;
                this.ExtraChargesRoot.SetActive(false);
            }
            else if (this.IsInventoryButton)
            {
                this.Image.overrideSprite = this.m_inventorySprite;
                this.Borders.enabled = true;
                this.Name.enabled = false;
                this.CooldownText.enabled = false;
                if (interactable)
                {
                    this.Button.interactable = true;
                    this.CooldownOverlay.enabled = false;
                }
                else
                {
                    this.Button.interactable = false;
                    this.CooldownOverlay.enabled = true;
                    this.CooldownOverlay.fillAmount = 1f;
                }
            }
            else
            {
                this.Image.overrideSprite = null;
                this.Borders.enabled = true;
                this.Name.enabled = true;
                float num = GameLogic.Binder.SkillSystem.getSkillCooldownTimeRemaining(this.SkillInstance.SkillType);
                if (num > 0f)
                {
                    float num2 = GameLogic.Binder.SkillSystem.getSkillCooldownNormalizedProgress(this.SkillInstance.SkillType);
                    this.Button.interactable = num2 >= 1f;
                    this.CooldownText.enabled = true;
                    this.CooldownText.text = num.ToString("0.0");
                    this.CooldownOverlay.fillAmount = 1f - num2;
                    this.CooldownOverlay.enabled = this.CooldownOverlay.fillAmount < 1f;
                }
                else if (interactable)
                {
                    this.Button.interactable = true;
                    this.CooldownText.enabled = false;
                    this.CooldownOverlay.enabled = false;
                }
                else
                {
                    this.Button.interactable = false;
                    this.CooldownText.enabled = false;
                    this.CooldownOverlay.enabled = true;
                    this.CooldownOverlay.fillAmount = 1f;
                }
                if (this.Button.interactable)
                {
                    int num4 = Mathf.Clamp(GameLogic.Binder.GameState.Player.ActiveCharacter.getSkillExtraCharges(this.SkillInstance.SkillType) - GameLogic.Binder.SkillSystem.getNumberOfUsedCharges(this.SkillInstance.SkillType), 0, 0x7fffffff);
                    if (num4 > 0)
                    {
                        this.ExtraChargesRoot.SetActive(true);
                        this.ExtraChargesText.text = "x" + (num4 + 1);
                    }
                    else
                    {
                        this.ExtraChargesRoot.SetActive(false);
                    }
                }
                else
                {
                    this.ExtraChargesRoot.SetActive(false);
                }
            }
        }

        public void refreshNotifiers(bool visible)
        {
            if (this.IsInventoryButton)
            {
                Player player = GameLogic.Binder.GameState.Player;
                GameObjectExtensions.SetActiveIfDifferent(this.NotifierUnlock, visible && (!player.Notifiers.PotionsInspected || (player.Notifiers.getNumberOfGoldItemNotifications() > 0)));
                this.NotifierUnlockPulsating.enabled = this.NotifierUnlock.activeSelf;
                GameObjectExtensions.SetActiveIfDifferent(this.NotifierUpgrade, visible && (player.Notifiers.getNumberOfGreenItemNotifications() > 0));
                this.NotifierUpgradePulsating.enabled = this.NotifierUpgrade.activeSelf;
            }
        }

        public void setInitialPosition()
        {
            this.OffscreenOpenClose.refreshInitialPosition();
            this.OffscreenOpenClose.close(0f, Easing.Function.LINEAR, 0f);
        }

        public void startGlow()
        {
            this.Glow.Play(true);
        }

        public void stopGlow()
        {
            this.Glow.Stop(true);
            this.Glow.Clear(true);
        }

        public bool IsInventoryButton
        {
            [CompilerGenerated]
            get
            {
                return this.<IsInventoryButton>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<IsInventoryButton>k__BackingField = value;
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

        public GameLogic.SkillInstance SkillInstance
        {
            [CompilerGenerated]
            get
            {
                return this.<SkillInstance>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<SkillInstance>k__BackingField = value;
            }
        }
    }
}

