namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemInfoContent : MenuContent
    {
        public Button EquipButton;
        public Image EquipButtonImage;
        public PulsatingGraphic EquipButtonPulse;
        public Text EquipButtonText;
        public Text Header;
        public Image Icon;
        public Image IconBackground;
        public Image IconSelectedBorder;
        public CellButton InstantUpgradeButton;
        public Text InstantUpgradeDescription;
        public Text InstantUpgradeText;
        public Text InstantUpgradeTitle;
        public Text InstantUpgradeTitle2;
        public Text LevelText;
        public Text LevelTitle;
        private InputParameters m_inputParameters;
        public List<IconWithText> Perks = new List<IconWithText>();
        public Text PerksTitle;
        public GameObject PerksTitleDivier;
        public Text PowerDescription;
        public Image PowerIcon;
        public Text PowerText;
        public Button RerollButton;
        public Image RerollButtonImage;
        public Text RerollButtonText;
        public Text RerollCost;
        public CellButton SellButton;
        public GameObject SpecialOfferPanel;
        public List<Image> Stars = new List<Image>();
        public Text Title;
        public ParticleSystem UpgradeParticleEffect;

        private GameLogic.Item getReferenceItem()
        {
            if (this.ItemInstance != null)
            {
                return this.ItemInstance.Item;
            }
            return this.Item;
        }

        protected override void onAwake()
        {
            this.LevelTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEMINFO_ITEM_LEVEL, null, false));
            this.PerksTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEMINFO_PERKS_TITLE, null, false));
            this.SellButton.ButtonHeader.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_SELL, null, false));
            this.RerollButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_REROLL, null, false));
            if (this.InstantUpgradeTitle != null)
            {
                this.InstantUpgradeTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEMINFO_INSTANT_UPGRADE_TITLE1, null, false));
            }
            this.InstantUpgradeTitle2.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEMINFO_INSTANT_UPGRADE_TITLE2, null, false));
        }

        protected override void onCleanup()
        {
        }

        public void onCustomCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnItemRankUpped -= new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnItemEvolved -= new GameLogic.Events.ItemEvolved(this.onItemEvolved);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnItemRankUpped += new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnItemEvolved += new GameLogic.Events.ItemEvolved(this.onItemEvolved);
        }

        public void onEquipButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                CharacterInstance activeCharacter = player.ActiveCharacter;
                if (player.canEvolveItem(this.ItemInstance))
                {
                    while (player.canEvolveItem(this.ItemInstance))
                    {
                        CmdEvolveItem.ExecuteStatic(activeCharacter, this.ItemInstance);
                    }
                }
                else
                {
                    GameLogic.Binder.CommandProcessor.execute(new CmdEquipItem(activeCharacter, this.ItemInstance), 0f);
                    PlayerView.Binder.AudioSystem.playItemEquipSfx(this.ItemInstance.Item.Type);
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(false);
                }
            }
        }

        public void onFuseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && (this.ItemInstance != null))
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.ItemEvolveMiniPopupContent, this.ItemInstance, 0f, false, true);
            }
        }

        public void onInstantUpgradeButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (App.Binder.ConfigMeta.ItemInstantUpgradeCostCurve() <= player.getResourceAmount(ResourceType.Diamond))
                {
                    CmdInstantUpgradeItem.ExecuteStatic(this.ItemInstance, player.ActiveCharacter);
                }
                else
                {
                    MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                    parameters2.PathToShop = PathToShopType.ItemInstantUpgrade;
                    parameters2.CloseCallback = new System.Action(this.onShopPurchaseCancelled);
                    parameters2.PurchaseCallback = new Action<ShopEntry, PurchaseResult>(this.onShopPurchaseCompleted);
                    MiniPopupMenu.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.ShopOfferMiniPopupContent, parameter, 0f, true, true);
                }
            }
        }

        private void onItemEvolved(CharacterInstance character, GameLogic.ItemInstance itemInstance)
        {
            if (!this.UpgradeParticleEffect.isPlaying)
            {
                this.UpgradeParticleEffect.Play();
            }
            this.onRefresh();
        }

        private void onItemRankUpped(CharacterInstance character, GameLogic.ItemInstance itemInstance, int rankUpCount, bool free)
        {
            if (!this.UpgradeParticleEffect.isPlaying)
            {
                this.UpgradeParticleEffect.Play();
            }
            this.onRefresh();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_inputParameters = (InputParameters) param;
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            GameLogic.Item item = this.getReferenceItem();
            switch (item.Type)
            {
                case ItemType.Weapon:
                    this.Header.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEMS_WEAPON, null, false));
                    this.PowerIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_weapon");
                    this.PowerDescription.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_POWER_ATTACK, null, false));
                    break;

                case ItemType.Armor:
                    this.Header.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEMS_ARMOR, null, false));
                    this.PowerIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_armor");
                    this.PowerDescription.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_POWER_DEFENSE, null, false));
                    break;

                case ItemType.Cloak:
                    this.Header.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEMS_CLOAK, null, false));
                    this.PowerIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_spell");
                    this.PowerDescription.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_POWER_SKILLDMG, null, false));
                    break;

                default:
                    this.Header.text = "CHANGE ME";
                    this.PowerIcon.sprite = null;
                    break;
            }
            this.Title.text = StringExtensions.ToUpperLoca(item.Name);
            this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", item.SpriteId);
            MenuHelpers.RefreshStarContainer(this.Stars, item.Rarity);
            if (this.ItemInstance != null)
            {
                double num;
                double num2;
                if (!this.ItemInstance.InspectedByPlayer)
                {
                    CmdInspectItem.ExecuteStatic(this.ItemInstance);
                }
                App.Binder.ConfigMeta.ItemSellCurve(activeCharacter, this.ItemInstance.Item.Type, this.ItemInstance.Rarity, this.ItemInstance.Level, this.ItemInstance.Rank, out num, out num2);
                this.SellButton.gameObject.SetActive(true);
                if (num > 0.0)
                {
                    this.SellButton.ButtonLabelIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Token]);
                    this.SellButton.ButtonLabel.text = MenuHelpers.BigValueToString(num);
                }
                else
                {
                    this.SellButton.ButtonLabelIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Coin]);
                    this.SellButton.ButtonLabel.text = MenuHelpers.BigValueToString(num2);
                }
                if (!ConfigMeta.CAN_SELL_HIGHEST_RARITY_ITEM && (this.ItemInstance.Rarity == ConfigMeta.ITEM_HIGHEST_RARITY))
                {
                    this.SellButton.ButtonHeader.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_VALUE, null, false));
                }
                else
                {
                    this.SellButton.ButtonHeader.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_SELL, null, false));
                }
                this.EquipButton.gameObject.SetActive(true);
                this.EquipButtonImage.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_button_selected");
            }
            else
            {
                this.SellButton.gameObject.SetActive(false);
                this.EquipButton.gameObject.SetActive(false);
            }
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            GameLogic.Item item = this.getReferenceItem();
            if (this.ItemInstance != null)
            {
                int num;
                this.PowerText.text = MenuHelpers.BigValueToString(App.Binder.ConfigMeta.ItemPowerCurve(this.ItemInstance.Item.Type, player.getRiggedItemLevel(this.ItemInstance), this.ItemInstance.Rank));
                int num2 = activeCharacter.getItemInstantUpgradeCount(this.ItemInstance, out num);
                if (num2 > ConfigMeta.ITEM_INSTANT_UPGRADE_LEVEL_THRESHOLD)
                {
                    this.SpecialOfferPanel.SetActive(true);
                    this.InstantUpgradeText.text = _.L(ConfigLoca.ITEMINFO_INSTANT_UPGRADE_DESCIPTION, new <>__AnonType8<int>(num), false);
                    this.InstantUpgradeButton.ButtonScript.interactable = num2 > 0;
                    this.InstantUpgradeButton.ButtonImage.material = !this.InstantUpgradeButton.ButtonScript.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
                    this.InstantUpgradeButton.ButtonIcon.material = this.InstantUpgradeButton.ButtonImage.material;
                }
                else
                {
                    this.SpecialOfferPanel.SetActive(false);
                }
                if (player.canEvolveItem(this.ItemInstance))
                {
                    this.EquipButton.enabled = true;
                    this.EquipButtonImage.material = null;
                    this.EquipButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_UPGRADE, null, false));
                    this.EquipButtonPulse.enabled = true;
                }
                else if (activeCharacter.isItemInstanceEquipped(this.ItemInstance))
                {
                    this.EquipButton.enabled = false;
                    this.EquipButtonImage.material = PlayerView.Binder.DisabledUiMaterial;
                    this.EquipButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_EQUIPPED, null, false));
                    this.EquipButtonPulse.enabled = false;
                    this.IconSelectedBorder.enabled = true;
                }
                else if (!activeCharacter.hasReachedUnlockFloorForItem(this.ItemInstance))
                {
                    this.EquipButton.enabled = false;
                    this.EquipButtonImage.material = PlayerView.Binder.DisabledUiMaterial;
                    this.EquipButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_FLOOR_X, new <>__AnonType15<int>(this.ItemInstance.UnlockFloor), false));
                    this.EquipButtonPulse.enabled = false;
                    this.IconSelectedBorder.enabled = false;
                }
                else if (!this.ItemInstance.Unlocked)
                {
                    this.EquipButton.enabled = false;
                    this.EquipButtonImage.material = PlayerView.Binder.DisabledUiMaterial;
                    this.EquipButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_LOCKED, null, false));
                    this.EquipButtonPulse.enabled = false;
                    this.IconSelectedBorder.enabled = false;
                }
                else
                {
                    this.EquipButton.enabled = true;
                    this.EquipButtonImage.material = null;
                    this.EquipButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_EQUIP, null, false));
                    this.EquipButtonPulse.enabled = false;
                    this.IconSelectedBorder.enabled = false;
                }
                this.LevelText.text = (player.getRiggedItemLevel(this.ItemInstance) + this.ItemInstance.Rank).ToString();
            }
            else
            {
                this.PowerText.text = "0";
                this.LevelText.text = "1";
                this.SpecialOfferPanel.SetActive(false);
            }
            if ((this.ItemInstance != null) && (this.ItemInstance.Perks.count() > 0))
            {
                this.RerollButton.interactable = true;
                this.RerollCost.text = App.Binder.ConfigMeta.ItemRerollDiamondCostCurve(this.ItemInstance.Rarity, this.ItemInstance.RerollCount).ToString();
            }
            else
            {
                this.RerollButton.interactable = false;
                this.RerollCost.text = "-";
            }
            this.RerollButtonImage.material = !this.RerollButton.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
            int num3 = item.FixedPerks.count();
            if (num3 > 0)
            {
                IconWithText text = this.Perks[0];
                PerkInstance instance2 = item.FixedPerks.PerkInstances[0];
                ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[instance2.Type];
                text.gameObject.SetActive(true);
                text.Text.text = MenuHelpers.GetFormattedPerkDescription(instance2.Type, instance2.Modifier, data.DurationSeconds, data.Threshold, 0f, false);
            }
            else
            {
                this.Perks[0].gameObject.SetActive(false);
            }
            int count = this.ItemInstance.Perks.PerkInstances.Count;
            for (int i = 1; i < this.Perks.Count; i++)
            {
                IconWithText text2 = this.Perks[i];
                PerkInstance instance3 = null;
                if ((this.ItemInstance != null) && ((i - 1) < count))
                {
                    instance3 = this.ItemInstance.Perks.PerkInstances[i - 1];
                }
                if (instance3 != null)
                {
                    ConfigPerks.SharedData data2 = ConfigPerks.SHARED_DATA[instance3.Type];
                    text2.gameObject.SetActive(true);
                    text2.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", data2.SmallSprite);
                    if (this.ItemInstance != null)
                    {
                        text2.Text.text = MenuHelpers.GetFormattedPerkDescription(instance3.Type, instance3.Modifier, data2.DurationSeconds, data2.Threshold, 0f, true);
                    }
                    else
                    {
                        text2.Text.text = "CHANGE ME";
                    }
                }
                else
                {
                    text2.gameObject.SetActive(false);
                }
            }
            this.PerksTitleDivier.SetActive((num3 + count) > 0);
        }

        public void onRerollButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                double num = App.Binder.ConfigMeta.ItemRerollDiamondCostCurve(this.ItemInstance.Rarity, this.ItemInstance.RerollCount);
                if (GameLogic.Binder.GameState.Player.getResourceAmount(ResourceType.Diamond) < num)
                {
                    MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                    parameters2.PathToShop = PathToShopType.Reroll;
                    parameters2.CloseCallback = new System.Action(this.onShopPurchaseCancelled);
                    parameters2.PurchaseCallback = new Action<ShopEntry, PurchaseResult>(this.onShopPurchaseCompleted);
                    MiniPopupMenu.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.ShopOfferMiniPopupContent, parameter, 0f, true, true);
                }
                else
                {
                    CmdRerollItemPerks.ExecuteStatic(GameLogic.Binder.GameState.Player.ActiveCharacter, this.ItemInstance);
                    this.onRefresh();
                    PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_Reroll, (float) 0f);
                    this.UpgradeParticleEffect.Play();
                }
            }
        }

        public void onSellButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                bool flag;
                TooltipMenu.InputParameters parameters5;
                Player player = GameLogic.Binder.GameState.Player;
                CharacterInstance activeCharacter = player.ActiveCharacter;
                int num = activeCharacter.getHighestItemLevelPlusRankEquippedOrInInventory(this.ItemInstance.Item.Type, out flag);
                if (activeCharacter.isItemInstanceEquipped(this.ItemInstance))
                {
                    parameters5 = new TooltipMenu.InputParameters();
                    parameters5.CenterOnTm = this.SellButton.RectTm;
                    parameters5.MenuContentParams = _.L(ConfigLoca.TOOLTIP_CANNOT_SELL_EQUIPPED, null, false);
                    TooltipMenu.InputParameters parameter = parameters5;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.InfoTooltip, parameter, 0f, false, true);
                }
                else if ((!ConfigMeta.CAN_SELL_HIGHEST_LEVEL_ITEM && (this.ItemInstance.LevelPlusRank == num)) && !flag)
                {
                    parameters5 = new TooltipMenu.InputParameters();
                    parameters5.CenterOnTm = this.SellButton.RectTm;
                    parameters5.MenuContentParams = _.L(ConfigLoca.TOOLTIP_CANNOT_SELL_HIGHEST, null, false);
                    TooltipMenu.InputParameters parameters2 = parameters5;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.InfoTooltip, parameters2, 0f, false, true);
                }
                else if (!ConfigMeta.CAN_SELL_HIGHEST_RARITY_ITEM && (this.ItemInstance.Rarity == ConfigMeta.ITEM_HIGHEST_RARITY))
                {
                    double num2;
                    double num3;
                    App.Binder.ConfigMeta.ItemSellCurve(activeCharacter, this.ItemInstance.Item.Type, this.ItemInstance.Rarity, this.ItemInstance.Level, this.ItemInstance.Rank, out num2, out num3);
                    parameters5 = new TooltipMenu.InputParameters();
                    parameters5.CenterOnTm = this.SellButton.RectTm;
                    parameters5.MenuContentParams = _.L(ConfigLoca.TOOLTIP_CANNOT_SELL_LEGENDARY, new <>__AnonType4<string>(MenuHelpers.ColoredText(MenuHelpers.BigValueToString(num3))), false);
                    TooltipMenu.InputParameters parameters3 = parameters5;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.InfoTooltip, parameters3, 0f, false, true);
                }
                else if (false)
                {
                    parameters5 = new TooltipMenu.InputParameters();
                    parameters5.CenterOnTm = this.SellButton.RectTm;
                    parameters5.MenuContentParams = this.ItemInstance;
                    TooltipMenu.InputParameters parameters4 = parameters5;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.SellConfirmationTooltip, parameters4, 0f, false, true);
                }
                else
                {
                    CmdSellItem.ExecuteStatic(player.ActiveCharacter, this.ItemInstance, this.SellButton.RectTm);
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ItemSell, (float) 0f);
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
            }
        }

        private void onShopPurchaseCancelled()
        {
            InputParameters parameters2 = new InputParameters();
            parameters2.ItemInstance = this.ItemInstance;
            InputParameters parameter = parameters2;
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.ItemInfoContent, parameter, 0f, true, true);
        }

        private void onShopPurchaseCompleted(ShopEntry shopEntry, PurchaseResult purchaseResult)
        {
            InputParameters parameters2 = new InputParameters();
            parameters2.ItemInstance = this.ItemInstance;
            InputParameters parameter = parameters2;
            PlayerView.Binder.MenuSystem.waitAndTransitionToNewMenu(MenuType.ThinPopupMenu, MenuContentType.ItemInfoContent, parameter);
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            return new <onShow>c__Iterator126();
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.ItemInfoContent;
            }
        }

        private GameLogic.Item Item
        {
            get
            {
                return this.m_inputParameters.Item;
            }
        }

        private GameLogic.ItemInstance ItemInstance
        {
            get
            {
                return this.m_inputParameters.ItemInstance;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator126 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_Popup, (float) 0f);
                }
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public GameLogic.Item Item;
            public GameLogic.ItemInstance ItemInstance;
        }
    }
}

