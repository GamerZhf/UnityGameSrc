namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private GameLogic.ItemInstance <ItemInstance>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private bool <StripedRow>k__BackingField;
        public CanvasGroup AlphaGroup;
        public Image Bg;
        public PlayerView.CellButton CellButton;
        public Image Icon;
        public Image IconBackground;
        public Image IconSelectedBorder;
        public GameObject InfoButton;
        public Text LevelText;
        private int m_numAllowedUpgrades = -1;
        private List<Vector3> m_originalStarLocalPositions = new List<Vector3>();
        private TurboClick m_turboClick;
        public PulsatingGraphic Notifier;
        public Text PowerBonus;
        public Image PowerIcon;
        public List<Image> Stars = new List<Image>();
        public Text Title;
        public ParticleSystem UpgradeParticleEffect;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.Notifier.enabled = false;
            this.Bg.enabled = ConfigDevice.DeviceQuality() >= DeviceQualityType.Med;
            this.IconBackground.enabled = ConfigDevice.DeviceQuality() >= DeviceQualityType.Med;
            this.m_turboClick = GameObjectExtensions.AddOrGetComponent<TurboClick>(this.CellButton.gameObject);
            for (int i = 0; i < this.Stars.Count; i++)
            {
                this.m_originalStarLocalPositions.Add(this.Stars[i].transform.localPosition);
            }
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(GameLogic.ItemInstance itemInstance, bool stripedRow)
        {
            this.ItemInstance = itemInstance;
            this.StripedRow = stripedRow;
            this.Bg.sprite = null;
            this.Bg.color = !this.StripedRow ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            if (this.ItemInstance != null)
            {
                this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", this.ItemInstance.Item.SpriteId);
                switch (this.ItemInstance.Item.Type)
                {
                    case ItemType.Weapon:
                        this.PowerBonus.enabled = true;
                        this.PowerIcon.enabled = true;
                        this.PowerIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_weapon");
                        goto Label_016F;

                    case ItemType.Armor:
                        this.PowerBonus.enabled = true;
                        this.PowerIcon.enabled = true;
                        this.PowerIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_armor");
                        goto Label_016F;

                    case ItemType.Cloak:
                        this.PowerBonus.enabled = true;
                        this.PowerIcon.enabled = true;
                        this.PowerIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_spell");
                        goto Label_016F;
                }
                this.PowerBonus.enabled = false;
                this.PowerIcon.enabled = false;
            }
        Label_016F:
            this.refresh();
        }

        protected void OnDisable()
        {
            this.m_turboClick.OnClick -= new TurboClick.ClickCallback(this.onTurboClick);
            this.m_turboClick.enabled = false;
            this.Notifier.enabled = false;
        }

        protected void OnEnable()
        {
            this.m_turboClick.enabled = true;
            this.m_turboClick.OnClick += new TurboClick.ClickCallback(this.onTurboClick);
        }

        public void onTooltipButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && this.ItemInstance.Unlocked)
            {
                ItemInfoContent.InputParameters parameters2 = new ItemInfoContent.InputParameters();
                parameters2.ItemInstance = this.ItemInstance;
                ItemInfoContent.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.ItemInfoContent, parameter, 0f, false, true);
            }
        }

        private void onTurboClick()
        {
            if (this.CellButton.ButtonScript.enabled)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (((this.CellButton.ActiveType != CellButtonType.Unlock) && (this.CellButton.ActiveType != CellButtonType.Select)) && player.hasCompletedAllTutorialsInCategory(ConfigTutorials.CORE_LOOP_TUTORIALS))
                {
                    this.onUpgradeButtonClicked();
                }
            }
        }

        public void onUpgradeButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                ItemInfoContent.InputParameters parameters3;
                Player player = GameLogic.Binder.GameState.Player;
                CharacterInstance activeCharacter = player.ActiveCharacter;
                double coins = player.getResourceAmount(ResourceType.Coin);
                player.Notifiers.markAllNotificationsForItemsThatWeCanUpgradeAsInspected(this.ItemInstance.Item.Type);
                if (player.canUnlockItemInstance(this.ItemInstance))
                {
                    this.UpgradeParticleEffect.Play(true);
                    CmdUnlockItem.ExecuteStatic(this.ItemInstance);
                    parameters3 = new ItemInfoContent.InputParameters();
                    parameters3.ItemInstance = this.ItemInstance;
                    ItemInfoContent.InputParameters parameter = parameters3;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.ItemInfoContent, parameter, 0f, false, true);
                }
                else if (player.canEvolveItem(this.ItemInstance))
                {
                    parameters3 = new ItemInfoContent.InputParameters();
                    parameters3.ItemInstance = this.ItemInstance;
                    ItemInfoContent.InputParameters parameters2 = parameters3;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.ItemInfoContent, parameters2, 0f, false, true);
                }
                else if (player.canUpgradeItemInstance(this.ItemInstance, coins))
                {
                    if (this.m_numAllowedUpgrades != 0)
                    {
                        if (this.m_numAllowedUpgrades != -1)
                        {
                            this.m_numAllowedUpgrades--;
                        }
                        this.UpgradeParticleEffect.Play(true);
                        CmdRankUpItem.ExecuteStatic(activeCharacter, this.ItemInstance, 1, false);
                    }
                }
                else if (player.ActiveCharacter.canEquipItem(this.ItemInstance))
                {
                    CmdEquipItem.ExecuteStatic(player.ActiveCharacter, this.ItemInstance);
                    PlayerView.Binder.AudioSystem.playItemEquipSfx(this.ItemInstance.Item.Type);
                }
            }
        }

        public void refresh()
        {
            if (this.ItemInstance != null)
            {
                Player player = GameLogic.Binder.GameState.Player;
                CharacterInstance activeCharacter = player.ActiveCharacter;
                double coins = player.getResourceAmount(ResourceType.Coin);
                int rank = this.ItemInstance.Rank;
                int num3 = rank + 1;
                this.InfoButton.SetActive(this.ItemInstance.Unlocked);
                if (this.ItemInstance.Unlocked)
                {
                    this.AlphaGroup.alpha = 1f;
                    this.Title.text = StringExtensions.ToUpperLoca(this.ItemInstance.Item.Name);
                    this.Icon.material = null;
                    this.IconBackground.material = null;
                    this.IconSelectedBorder.enabled = activeCharacter.isItemInstanceEquipped(this.ItemInstance);
                    this.LevelText.enabled = true;
                    this.LevelText.text = _.L(ConfigLoca.ITEMINFO_ITEM_LEVEL_EXTENDED, new <>__AnonType8<int>(player.getRiggedItemLevel(this.ItemInstance) + this.ItemInstance.Rank), false);
                    double v = activeCharacter.getAdjustedItemUpgradeCost(this.ItemInstance.Item.Type, player.getRiggedItemLevel(this.ItemInstance), num3);
                    this.CellButton.setActive(true);
                    if (player.ActiveCharacter.isItemInstanceEquipped(this.ItemInstance))
                    {
                        this.Notifier.enabled = player.Notifiers.isItemGreenNotificationActive(this.ItemInstance, coins);
                        if (this.ItemInstance.isAtMaxRank())
                        {
                            this.CellButton.setCellButtonStyle(CellButtonType.UpgradeDisabled, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_MAX, null, false)));
                        }
                        else if (player.canUpgradeItemInstance(this.ItemInstance, coins))
                        {
                            this.CellButton.setCellButtonStyle(CellButtonType.Upgrade, MenuHelpers.BigValueToString(v), PlayerView.Binder.SpriteResources.getSprite(ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Coin]));
                        }
                        else
                        {
                            this.CellButton.setCellButtonStyle(CellButtonType.UpgradeDisabled, MenuHelpers.BigValueToString(v), PlayerView.Binder.SpriteResources.getSprite(ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Coin]));
                        }
                    }
                    else
                    {
                        this.Notifier.enabled = false;
                        this.CellButton.setCellButtonStyle(CellButtonType.Select, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_EQUIP, null, false)));
                    }
                }
                else
                {
                    this.Title.text = !this.ItemInstance.InspectedByPlayer ? StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEM_UNIDENTIFIED, null, false)) : StringExtensions.ToUpperLoca(this.ItemInstance.Item.Name);
                    this.Icon.material = PlayerView.Binder.DisabledUiMaterial;
                    this.IconBackground.material = PlayerView.Binder.DisabledUiMaterial;
                    this.IconSelectedBorder.enabled = activeCharacter.isItemInstanceEquipped(this.ItemInstance);
                    this.Notifier.enabled = player.Notifiers.isItemGoldNotificationActive(this.ItemInstance);
                    this.LevelText.enabled = false;
                    this.AlphaGroup.alpha = 0.5f;
                    if (player.canUnlockItemInstance(this.ItemInstance))
                    {
                        this.CellButton.setCellButtonStyle(CellButtonType.Unlock, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_UNLOCK, null, false)));
                    }
                    else
                    {
                        this.CellButton.setCellButtonStyle(CellButtonType.UnlockLocked, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_FLOOR_X, new <>__AnonType15<int>(this.ItemInstance.UnlockFloor), false)));
                    }
                }
                string[] textArray1 = new string[] { MenuHelpers.BigValueToString(App.Binder.ConfigMeta.ItemPowerCurve(this.ItemInstance.Item.Type, player.getRiggedItemLevel(this.ItemInstance), rank)) };
                this.PowerBonus.text = string.Concat(textArray1);
                MenuHelpers.RefreshStarContainer(this.Stars, this.m_originalStarLocalPositions, this.ItemInstance.Rarity, false);
            }
        }

        public void setNumAllowedUpgrades(int count)
        {
            this.m_numAllowedUpgrades = count;
        }

        public GameLogic.ItemInstance ItemInstance
        {
            [CompilerGenerated]
            get
            {
                return this.<ItemInstance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ItemInstance>k__BackingField = value;
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

