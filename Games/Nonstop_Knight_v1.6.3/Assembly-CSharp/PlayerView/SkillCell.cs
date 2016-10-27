namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private GameLogic.SkillType <SkillType>k__BackingField;
        [CompilerGenerated]
        private bool <StripedRow>k__BackingField;
        public CanvasGroup AlphaGroup;
        public static Color BACKGROUND_REGULAR_COLOR = new Color(0.3960784f, 0.4666667f, 0.6117647f, 0.1215686f);
        public static Color BACKGROUND_STRIPED_COLOR = new Color(0.2784314f, 0.3215686f, 0.4117647f, 0.1529412f);
        public Image Bg;
        public PlayerView.CellButton CellButton;
        public Text Description;
        public static Color DIMMED_PERK_ICON_COLOR = new Color(0.8313726f, 0.8313726f, 0.8313726f, 0.4980392f);
        public Image Icon;
        public Image IconSelectedBorder;
        public Button InfoButton;
        private Action<GameLogic.SkillType> m_infoClickCallback;
        public PulsatingGraphic Notifier;
        public List<Image> PerkIcons;
        public AnimatedProgressBar PerkProgressBar;
        public GameObject PerkProgressRoot;
        public Text Title;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(GameLogic.SkillType skillType, bool stripedRow, Action<GameLogic.SkillType> infoClickCallback)
        {
            this.SkillType = skillType;
            this.StripedRow = stripedRow;
            this.Bg.sprite = null;
            this.Bg.color = !this.StripedRow ? BACKGROUND_REGULAR_COLOR : BACKGROUND_STRIPED_COLOR;
            this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigSkills.SHARED_DATA[skillType].Spritesheet, ConfigSkills.SHARED_DATA[skillType].Sprite);
            this.m_infoClickCallback = infoClickCallback;
        }

        private void onCharacterSkillRankUpped(CharacterInstance character, SkillInstance si)
        {
            this.refresh();
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnCharacterSkillRankUpped -= new GameLogic.Events.CharacterSkillRankUpped(this.onCharacterSkillRankUpped);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnCharacterSkillRankUpped += new GameLogic.Events.CharacterSkillRankUpped(this.onCharacterSkillRankUpped);
        }

        public void onInfoButtonClicked()
        {
            this.m_infoClickCallback(this.SkillType);
        }

        public void onUpgradeButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
                SkillInstance skillInstance = activeCharacter.getSkillInstance(this.SkillType);
                if (skillInstance != null)
                {
                    CmdRankUpSkill.ExecuteStatic(activeCharacter, skillInstance);
                    PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_ItemRankUp, (float) 0f);
                }
            }
        }

        public void refresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[this.SkillType];
            SkillInstance instance2 = activeCharacter.getSkillInstance(this.SkillType);
            this.Title.text = StringExtensions.ToUpperLoca(_.L(data.Name, null, false));
            if (instance2 != null)
            {
                this.Description.text = _.L(ConfigLoca.ITEMINFO_ITEM_LEVEL_EXTENDED, new <>__AnonType8<int>(instance2.Rank), false);
                this.AlphaGroup.alpha = 1f;
                this.Icon.material = null;
                this.IconSelectedBorder.enabled = activeCharacter.isSkillActive(this.SkillType);
                this.InfoButton.gameObject.SetActive(true);
                this.CellButton.gameObject.SetActive(true);
                this.PerkProgressRoot.SetActive(true);
                double v = App.Binder.ConfigMeta.SkillUpgradeDustCost(instance2.Rank + 1);
                if (player.getResourceAmount(ResourceType.Dust) >= v)
                {
                    this.CellButton.setCellButtonStyle(CellButtonType.Upgrade, MenuHelpers.BigValueToString(v), PlayerView.Binder.SpriteResources.getSprite(ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Dust]));
                    this.Notifier.enabled = true;
                }
                else
                {
                    this.CellButton.setCellButtonStyle(CellButtonType.UpgradeDisabled, MenuHelpers.BigValueToString(v), PlayerView.Binder.SpriteResources.getSprite(ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Dust]));
                    this.Notifier.enabled = false;
                }
            }
            else
            {
                this.Description.text = _.L(ConfigLoca.SKILL_POPUP_UNLOCKS_AT_LEVEL, new <>__AnonType8<int>(data.UnlockRank), false);
                this.AlphaGroup.alpha = 0.5f;
                this.Icon.material = PlayerView.Binder.DisabledUiMaterial;
                this.IconSelectedBorder.enabled = false;
                this.InfoButton.gameObject.SetActive(false);
                this.PerkProgressRoot.SetActive(false);
                this.CellButton.gameObject.SetActive(false);
                this.Notifier.gameObject.SetActive(false);
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

        public GameLogic.SkillType SkillType
        {
            [CompilerGenerated]
            get
            {
                return this.<SkillType>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SkillType>k__BackingField = value;
            }
        }

        public bool StripedRow
        {
            [CompilerGenerated]
            get
            {
                return this.<StripedRow>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<StripedRow>k__BackingField = value;
            }
        }
    }
}

