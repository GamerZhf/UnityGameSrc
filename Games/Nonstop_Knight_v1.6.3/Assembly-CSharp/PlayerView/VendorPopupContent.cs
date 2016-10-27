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

    public class VendorPopupContent : MenuContent
    {
        public RectTransform AugmentationGridTm;
        public GameObject AugmentationsDividerRoot;
        public Text AugmentationTitle;
        public RectTransform FixedVendorSlotsGridTm;
        public Text FixedVendorSlotsSubtitle;
        public RectTransform FixedVendorSlotsSubtitleRootTm;
        public Text GemGridTitle;
        public RectTransform GemGridTm;
        private List<Card> m_augCards = new List<Card>();
        private float m_nextAutoRefresh;
        private InputParameters? m_params;
        private List<PromotionCard> m_promotionCards = new List<PromotionCard>();
        private List<Card> m_vendorCards = new List<Card>();
        public Text NextStockInTitle;
        public GameObject PromoDividerRoot;
        public RectTransform PromoGridTm;
        public Text PromoRunsOutTitle;
        public Text PromoRunsOutValue;
        public Text PromoTitle;
        public Text RefreshValue;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform ScrollRectTm;
        public GameObject SecondaryPromoDividerRoot;
        public RectTransform SecondaryPromoGridTm;
        public Text SecondaryPromoRunsOutTitle;
        public Text SecondaryPromoRunsOutValue;
        public Text SecondaryPromoTitle;
        public Button StarterBundleButton;
        public Text StarterBundleDescription;
        public GameObject StarterBundleDividerRoot;
        public Image StarterBundleImage;
        public GameObject StarterBundleRoot;
        public Text StarterBundleTitle;
        public RectTransform VendorGridTm;
        public Text VendorTitle;
        public RectTransform VerticalGroup;

        private void addAugmentationToGrid(Card.Content content)
        {
            Card item = PlayerView.Binder.CardButtonPool.getObject();
            item.transform.SetParent(this.AugmentationGridTm, false);
            item.initialize(content, new Action<Card>(this.onCardClicked));
            this.m_augCards.Add(item);
            item.gameObject.SetActive(true);
        }

        private void addPromotionCellToList(ShopEntry shopEntry, RemotePromotion promotion, RectTransform parentTm)
        {
            PromotionCard.Content content2 = new PromotionCard.Content();
            content2.Promotion = promotion;
            content2.RemoteTexture = promotion.ParsedCustomParams.ShopBannerImage;
            PromotionCard.Content content = content2;
            PromotionCard item = PlayerView.Binder.PromotionCardAugmentationPool.getObject();
            item.transform.SetParent(parentTm, false);
            item.initialize(content, new Action<Card>(this.onCardClicked));
            this.m_promotionCards.Add(item);
            item.gameObject.SetActive(true);
        }

        private void addSecondaryPromotions()
        {
            bool flag = false;
            foreach (string str in ShopManager.ValidSecondaryPromoSlots)
            {
                RemotePromotion promotionForPromoSlot = Service.Binder.PromotionManager.GetPromotionForPromoSlot(str);
                if (((promotionForPromoSlot != null) && promotionForPromoSlot.IsValid()) && promotionForPromoSlot.ParsedLoca.ValidateShopBannerLoca())
                {
                    flag = true;
                    this.addPromotionCellToList(null, promotionForPromoSlot, this.SecondaryPromoGridTm);
                }
            }
            this.SecondaryPromoGridTm.gameObject.SetActive(flag);
            this.SecondaryPromoDividerRoot.gameObject.SetActive(flag);
        }

        private void addShopCellToList(ShopEntry shopEntry, ShopEntryInstance shopEntryInstance, RectTransform parentTm)
        {
            ShopPurchaseController controller = this.createShopPurchaseController(shopEntry, shopEntryInstance);
            string str = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_SOLD_OUT, null, false));
            Card.Content content2 = new Card.Content();
            content2.Obj = controller;
            content2.Text = StringExtensions.ToUpperLoca(controller.getTitle());
            content2.Sprite = controller.getSprite();
            content2.Interactable = controller.isPurchaseable();
            content2.Grayscale = !controller.isPurchaseable();
            content2.PriceText = controller.getPriceText(1);
            content2.PriceIcon = controller.getPriceIcon();
            content2.SoldText = !controller.isSold() ? null : str;
            content2.StickerText = controller.getStickerText();
            Card.Content content = content2;
            if (controller.payWithAd() && !Service.Binder.AdsSystem.initialized())
            {
                content.SoldText = str;
            }
            Card item = PlayerView.Binder.CardButtonPool.getObject();
            item.transform.SetParent(parentTm, false);
            item.initialize(content, new Action<Card>(this.onCardClicked));
            this.m_vendorCards.Add(item);
            item.gameObject.SetActive(true);
        }

        public void centerOnTransform(RectTransform coTransform)
        {
            Canvas.ForceUpdateCanvases();
            float targetCenterPos = -coTransform.localPosition.y;
            float height = this.VerticalGroup.rect.height;
            float viewRectHeight = this.ScrollRectTm.rect.height;
            this.ScrollRect.verticalNormalizedPosition = UiUtil.CalculateScrollRectVerticalNormalizedPosition(targetCenterPos, height, viewRectHeight);
        }

        private void cleanupCells()
        {
            for (int i = this.m_promotionCards.Count - 1; i >= 0; i--)
            {
                PromotionCard item = this.m_promotionCards[i];
                this.m_promotionCards.Remove(item);
                PlayerView.Binder.PromotionCardAugmentationPool.returnObject(item);
            }
            for (int j = this.m_augCards.Count - 1; j >= 0; j--)
            {
                Card card2 = this.m_augCards[j];
                this.m_augCards.Remove(card2);
                PlayerView.Binder.CardButtonPool.returnObject(card2);
            }
            for (int k = this.m_vendorCards.Count - 1; k >= 0; k--)
            {
                Card card3 = this.m_vendorCards[k];
                this.m_vendorCards.Remove(card3);
                PlayerView.Binder.CardButtonPool.returnObject(card3);
            }
        }

        private ShopPurchaseController createShopPurchaseController(ShopEntry shopEntry, ShopEntryInstance shopEntryInstance)
        {
            if (this.m_params.HasValue)
            {
                return new ShopPurchaseController(shopEntry, shopEntryInstance, this.m_params.Value.PathToShop, this.m_params.Value.CloseCallback, this.m_params.Value.PurchaseCallback);
            }
            return new ShopPurchaseController(shopEntry, shopEntryInstance, PathToShopType.Vendor, new System.Action(this.onShopClosed), new Action<ShopEntry, PurchaseResult>(this.onShopPurchaseCompleted));
        }

        protected override void onAwake()
        {
            this.PromoTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_PROMO_TITLE, null, false));
            this.PromoRunsOutTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PROMOTION_TIMER, null, false));
            this.SecondaryPromoRunsOutTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PROMOTION_TIMER, null, false));
            this.SecondaryPromoTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_SECONDARY_PROMO_TITLE, null, false));
            this.VendorTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR, null, false));
            this.AugmentationTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_TITLE_AUGMENTATIONS, null, false));
            this.GemGridTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_TITLE_GEMS, null, false));
            this.NextStockInTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_NEW_STOCK_IN, null, false));
            this.StarterBundleTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_TITLE_STARTERDBUNDLE, null, false));
            this.StarterBundleDescription.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_STARTERDBUNDLE_DESCRIPTION, null, false));
            this.FixedVendorSlotsSubtitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_TITLE_FIXED_SLOTS, null, false));
        }

        private void onCardClicked(Card card)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                MiniPopupMenu.InputParameters parameters3;
                if (card.ActiveContent.Obj is PlayerAugmentation)
                {
                    Player player = GameLogic.Binder.GameState.Player;
                    PlayerAugmentation augmentation = (PlayerAugmentation) card.ActiveContent.Obj;
                    if (player.Augmentations.canBuy(augmentation.Id))
                    {
                        parameters3 = new MiniPopupMenu.InputParameters();
                        parameters3.MenuContentParams = card.ActiveContent.Obj;
                        MiniPopupMenu.InputParameters parameter = parameters3;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.VendorMiniPopupContent, parameter, 0f, false, true);
                    }
                }
                else if (card.ActiveContent.Obj is ShopPurchaseController)
                {
                    ShopPurchaseController controller = (ShopPurchaseController) card.ActiveContent.Obj;
                    if (controller.isPurchaseable())
                    {
                        bool flag = controller.getRefShopEntry().Type == ShopEntryType.IapDiamonds;
                        if (App.Binder.ConfigMeta.DISABLE_VENDOR_ADS_CONFIRMATION_POPUP && controller.payWithAd())
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            controller.purchase(1);
                        }
                        else
                        {
                            parameters3 = new MiniPopupMenu.InputParameters();
                            parameters3.MenuContentParams = card.ActiveContent.Obj;
                            MiniPopupMenu.InputParameters parameters2 = parameters3;
                            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.VendorMiniPopupContent, parameters2, 0f, false, true);
                        }
                    }
                }
            }
        }

        protected override void onCleanup()
        {
            this.cleanupCells();
        }

        public override bool onCloseButtonClicked()
        {
            if (this.m_params.HasValue && (this.m_params.Value.CloseCallback != null))
            {
                this.m_params.Value.CloseCallback();
                return true;
            }
            return false;
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnVendorInventoryRefreshed -= new GameLogic.Events.VendorInventoryRefreshed(this.onVendorInventoryRefreshed);
            GameLogic.Binder.EventBus.OnResourcesGained -= new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            GameLogic.Binder.EventBus.OnPlayerAugmentationGained -= new GameLogic.Events.PlayerAugmentationGained(this.onPlayerAugmentationGained);
            Service.Binder.EventBus.OnIapShopInitialized -= new Service.Events.IapShopInitialized(this.onIapShopInitialized);
            Service.Binder.EventBus.OnIapShopProductsUpdated -= new Service.Events.IapShopProductsUpdated(this.onIapShopInitialized);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnVendorInventoryRefreshed += new GameLogic.Events.VendorInventoryRefreshed(this.onVendorInventoryRefreshed);
            GameLogic.Binder.EventBus.OnResourcesGained += new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            GameLogic.Binder.EventBus.OnPlayerAugmentationGained += new GameLogic.Events.PlayerAugmentationGained(this.onPlayerAugmentationGained);
            Service.Binder.EventBus.OnIapShopInitialized += new Service.Events.IapShopInitialized(this.onIapShopInitialized);
            Service.Binder.EventBus.OnIapShopProductsUpdated += new Service.Events.IapShopProductsUpdated(this.onIapShopInitialized);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.onRefresh();
        }

        private void onIapShopInitialized()
        {
            this.reconstructContent();
        }

        private void onPlayerAugmentationGained(Player player, string id)
        {
            this.reconstructContent();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            Service.Binder.ShopManager.RefreshProducts();
            if (param != null)
            {
                this.m_params = new InputParameters?((InputParameters) param);
            }
            else
            {
                this.m_params = null;
            }
            Player player = GameLogic.Binder.GameState.Player;
            CmdInspectVendorInventory.ExecuteStatic(player);
            if (!App.Binder.ConfigMeta.SELL_AUGMENTATIONS_IN_ADVENTURE_PANEL && !player.Tournaments.hasTournamentSelected())
            {
                player.Notifiers.markAllAugNotificationsThatWeCanPurchaseAsInspected();
                PlayerView.Binder.DungeonHud.refreshAdventureButton();
            }
            player.TrackingData.PerSessionShopOpenings++;
            if (player.TrackingData.PerSessionShopOpenings == 1)
            {
                Service.Binder.TrackingSystem.sendCrmEvent(player, "crm_open_shop");
            }
            this.reconstructContent();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_TITLE_SHOP, null, false)), string.Empty, string.Empty);
            Player player = GameLogic.Binder.GameState.Player;
            player.Notifiers.ShopInspected = true;
            this.RefreshValue.text = MenuHelpers.SecondsToStringHoursMinutes(player.Vendor.getSecondsToNextVendorInventoryRefresh());
            for (int i = 0; i < this.m_augCards.Count; i++)
            {
                Card card = this.m_augCards[i];
                PlayerAugmentation augmentation = (PlayerAugmentation) card.ActiveContent.Obj;
                bool interactable = !player.Tournaments.hasTournamentSelected() && player.Augmentations.canBuy(augmentation.Id);
                this.m_augCards[i].refresh(card.ActiveContent.Text, card.ActiveContent.PriceText, card.ActiveContent.PriceIcon, interactable, !interactable);
            }
            for (int j = 0; j < this.m_vendorCards.Count; j++)
            {
                ShopPurchaseController controller = (ShopPurchaseController) this.m_vendorCards[j].ActiveContent.Obj;
                controller.updateDetails();
                this.m_vendorCards[j].refresh(StringExtensions.ToUpperLoca(controller.getTitle()), controller.getPriceText(1), controller.getPriceIcon(), controller.isPurchaseable(), !controller.isPurchaseable());
            }
            bool flag2 = false;
            foreach (string str in ShopManager.ValidPromoSlots)
            {
                RemotePromotion promotionForPromoSlot = Service.Binder.PromotionManager.GetPromotionForPromoSlot(str);
                if (((promotionForPromoSlot != null) && promotionForPromoSlot.ParsedLoca.ValidateShopBannerLoca()) && Service.Binder.PromotionManager.ShouldShowTimer(promotionForPromoSlot))
                {
                    flag2 = true;
                    this.PromoRunsOutValue.text = Service.Binder.PromotionManager.GetTimeLeftFormatted(promotionForPromoSlot);
                    break;
                }
            }
            this.PromoRunsOutTitle.gameObject.SetActive(flag2);
            this.PromoRunsOutValue.gameObject.SetActive(flag2);
            flag2 = false;
            foreach (string str2 in ShopManager.ValidSecondaryPromoSlots)
            {
                RemotePromotion promotion = Service.Binder.PromotionManager.GetPromotionForPromoSlot(str2);
                if (((promotion != null) && promotion.ParsedLoca.ValidateShopBannerLoca()) && Service.Binder.PromotionManager.ShouldShowTimer(promotion))
                {
                    flag2 = true;
                    this.SecondaryPromoRunsOutValue.text = Service.Binder.PromotionManager.GetTimeLeftFormatted(promotion);
                    break;
                }
            }
            this.SecondaryPromoRunsOutTitle.gameObject.SetActive(flag2);
            this.SecondaryPromoRunsOutValue.gameObject.SetActive(flag2);
        }

        private void onResourcesGained(Player player, ResourceType resourceType, double amount, bool instant, string analyticsSourceId, Vector3? worldPt)
        {
            if (resourceType == ResourceType.Token)
            {
                this.onRefresh();
            }
        }

        private void onShopClosed()
        {
            if (this.m_params.HasValue && (this.m_params.Value.CloseCallback == new System.Action(this.onShopClosed)))
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
            else
            {
                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.VendorPopupContent, null);
            }
        }

        private void onShopPurchaseCompleted(ShopEntry shopEntry, PurchaseResult purchaseResult)
        {
            if (ConfigShops.IsIapShopEntry(shopEntry))
            {
                PlayerView.Binder.MenuSystem.waitAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.VendorPopupContent, null);
            }
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator1A6 iteratora = new <onShow>c__Iterator1A6();
            iteratora.<>f__this = this;
            return iteratora;
        }

        [ContextMenu("onStarterBundleButtonClicked()")]
        public void onStarterBundleButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                GameLogic.Binder.GameState.Player.HasOpenedStarterBundleOfferFromTaskPanel = true;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.StarterBundlePopupContent, null, 0f, false, true);
            }
        }

        private void onVendorInventoryRefreshed(Player player)
        {
            if ((GameLogic.Binder.GameState.ActiveDungeon != null) && (GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom != null))
            {
                this.reconstructContent();
            }
        }

        private void reconstructContent()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.cleanupCells();
            bool flag = false;
            foreach (string str in ShopManager.ValidPromoSlots)
            {
                RemotePromotion promotionForPromoSlot = Service.Binder.PromotionManager.GetPromotionForPromoSlot(str);
                if (((promotionForPromoSlot != null) && promotionForPromoSlot.IsValid()) && promotionForPromoSlot.ParsedLoca.ValidateShopBannerLoca())
                {
                    flag = true;
                    this.addPromotionCellToList(null, promotionForPromoSlot, this.PromoGridTm);
                }
            }
            this.PromoGridTm.gameObject.SetActive(flag);
            this.PromoDividerRoot.gameObject.SetActive(flag);
            List<ShopEntryInstance> inventory = player.Vendor.Inventory;
            for (int i = 0; i < App.Binder.ConfigMeta.VENDOR_INVENTORY_SIZE; i++)
            {
                if (i < inventory.Count)
                {
                    ShopEntryInstance shopEntryInstance = inventory[i];
                    this.addShopCellToList(null, shopEntryInstance, this.VendorGridTm);
                }
            }
            bool flag2 = player.HasUnlockedMissions || App.Binder.ConfigMeta.MISSION_BIG_PRIZE_INSTANT_SHOP_AVAILABILITY;
            if (App.Binder.ConfigMeta.VENDOR_FIXED_SLOTS_ENABLED && flag2)
            {
                this.FixedVendorSlotsGridTm.gameObject.SetActive(true);
                this.FixedVendorSlotsSubtitleRootTm.gameObject.SetActive(true);
                for (int j = 1; j <= 3; j++)
                {
                    string id = null;
                    switch (j)
                    {
                        case 1:
                            id = App.Binder.ConfigMeta.VENDOR_FIXED_SLOT1_ENTRY;
                            break;

                        case 2:
                            id = App.Binder.ConfigMeta.VENDOR_FIXED_SLOT2_ENTRY;
                            break;

                        case 3:
                            id = App.Binder.ConfigMeta.VENDOR_FIXED_SLOT3_ENTRY;
                            break;
                    }
                    ShopEntry shopEntry = ConfigShops.GetShopEntry(id);
                    if (shopEntry != null)
                    {
                        this.addShopCellToList(shopEntry, null, this.FixedVendorSlotsGridTm);
                    }
                }
            }
            else
            {
                this.FixedVendorSlotsGridTm.gameObject.SetActive(false);
                this.FixedVendorSlotsSubtitleRootTm.gameObject.SetActive(false);
            }
            if (App.Binder.ConfigMeta.SELL_AUGMENTATIONS_IN_ADVENTURE_PANEL || player.Tournaments.hasTournamentSelected())
            {
                this.AugmentationsDividerRoot.SetActive(false);
            }
            else
            {
                bool flag3 = true;
                int num4 = 0;
                List<PlayerAugmentation> list2 = GameLogic.Binder.PlayerAugmentationResources.getOrderedList();
                for (int k = 0; k < list2.Count; k++)
                {
                    if (num4 >= 6)
                    {
                        break;
                    }
                    PlayerAugmentation augmentation = list2[k];
                    if (!player.Augmentations.hasAugmentation(augmentation.Id))
                    {
                        flag3 = false;
                        string str3 = null;
                        SpriteAtlasEntry sprite = null;
                        if (augmentation.PerkInstance != null)
                        {
                            ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[augmentation.PerkInstance.Type];
                            if (augmentation.PerkInstance.Type == PerkType.CoinBonusStart)
                            {
                                str3 = MenuHelpers.BigValueToString((double) augmentation.PerkInstance.Modifier) + "\n" + StringExtensions.ToUpperLoca(_.L(data.ShortDescription, null, false));
                            }
                            else
                            {
                                str3 = MenuHelpers.BigModifierToString(augmentation.PerkInstance.Modifier, true) + "\n" + StringExtensions.ToUpperLoca(_.L(data.ShortDescription, null, false));
                            }
                            sprite = data.Sprite;
                        }
                        Card.Content content2 = new Card.Content();
                        content2.Obj = augmentation;
                        content2.Text = str3;
                        content2.Sprite = sprite;
                        content2.Grayscale = true;
                        content2.PriceText = MenuHelpers.BigValueToString(App.Binder.ConfigMeta.GetAugmentationPrice(augmentation.Id));
                        content2.PriceIcon = ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Token];
                        Card.Content content = content2;
                        this.addAugmentationToGrid(content);
                        num4++;
                    }
                }
                this.AugmentationsDividerRoot.SetActive(!flag3);
            }
            bool flag4 = Service.Binder.ShopManager.StarterBundleAvailable();
            bool flag5 = Service.Binder.ShopManager.StarterBundleVisible();
            this.StarterBundleDividerRoot.SetActive(flag5);
            this.StarterBundleRoot.SetActive(flag5);
            if (Service.Binder.ShopService.ProductsAvailable)
            {
                if (flag5)
                {
                    this.StarterBundleButton.interactable = flag4;
                    this.StarterBundleImage.material = !this.StarterBundleButton.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
                }
                this.addSecondaryPromotions();
                for (int m = 0; m < ShopManager.ValidPlacementSlots.Length; m++)
                {
                    string slot = ShopManager.ValidPlacementSlots[m];
                    ShopEntry shopEntryBySlot = Service.Binder.ShopManager.GetShopEntryBySlot(slot);
                    if (shopEntryBySlot != null)
                    {
                        this.addShopCellToList(shopEntryBySlot, null, this.GemGridTm);
                    }
                }
            }
            else
            {
                if (flag5)
                {
                    this.StarterBundleButton.interactable = false;
                    this.StarterBundleImage.material = !this.StarterBundleButton.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
                }
                this.addSecondaryPromotions();
                for (int n = 0; n < ConfigShops.IAP_SHOP_ENTRIES.Length; n++)
                {
                    ShopEntry entry4 = ConfigShops.IAP_SHOP_ENTRIES[n];
                    if (entry4.Type == ShopEntryType.IapDiamonds)
                    {
                        this.addShopCellToList(entry4, null, this.GemGridTm);
                    }
                }
            }
            this.onRefresh();
        }

        protected void Update()
        {
            if (((GameLogic.Binder.GameState != null) && (GameLogic.Binder.GameState.Player != null)) && (Time.time > this.m_nextAutoRefresh))
            {
                this.onRefresh();
                if (!ConfigApp.CHEAT_DISABLE_SHOP_AUTO_REFRESH)
                {
                    this.m_nextAutoRefresh = Time.time + 2f;
                }
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.VendorPopupContent;
            }
        }

        public override MenuContent.TabSpriteParameters TabSprite
        {
            get
            {
                MenuContent.TabSpriteParameters parameters = new MenuContent.TabSpriteParameters();
                parameters.SpriteAtlasId = "DungeonHud";
                parameters.SpriteId = "uiz_icon_shop";
                return parameters;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator1A6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal VendorPopupContent <>f__this;

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
                    if (this.<>f__this.m_params.HasValue && (this.<>f__this.m_params.Value.CenterOnRectTm != null))
                    {
                        this.<>f__this.centerOnTransform(this.<>f__this.m_params.Value.CenterOnRectTm);
                    }
                    else
                    {
                        this.<>f__this.ScrollRect.verticalNormalizedPosition = 1f;
                    }
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
            public PathToShopType PathToShop;
            public System.Action CloseCallback;
            public Action<ShopEntry, PurchaseResult> PurchaseCallback;
            public RectTransform CenterOnRectTm;
        }
    }
}

