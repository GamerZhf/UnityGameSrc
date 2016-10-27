namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Runtime.InteropServices;

    public class VendorMiniPopupContent : MenuContent
    {
        public PrettyButton DualButtonLeft;
        public PrettyButton DualButtonRight;
        private object m_param;
        private ShopPurchaseController m_shopPurchaseController;
        private TournamentView m_tournamentView;

        protected override void onAwake()
        {
        }

        public override bool onBackgroundOverlayClicked()
        {
            return this.onCloseButtonClicked();
        }

        protected override void onCleanup()
        {
        }

        public void onDualButtonLeftClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (this.m_shopPurchaseController != null)
                {
                    if (this.m_shopPurchaseController.isPurchaseable())
                    {
                        int num = this.m_shopPurchaseController.getPurchasesRemaining();
                        this.m_shopPurchaseController.purchase(1);
                        if (num == 1)
                        {
                            this.setShopEntryStackExhausted();
                        }
                    }
                }
                else if (this.m_tournamentView != null)
                {
                    double totalPrice = this.m_tournamentView.Instance.getDonationPrice();
                    double num3 = player.getResourceAmount(ResourceType.Diamond);
                    if (this.m_tournamentView.Instance.getDonationsRemaining() <= 0)
                    {
                        PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                    }
                    else if (totalPrice <= num3)
                    {
                        CmdDonateToTournamentBucket.ExecuteStatic(player, this.m_tournamentView.TournamentInfo.Id, 1, totalPrice);
                        if (!this.tryTriggerPostDonationCeremony())
                        {
                            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                        }
                    }
                    else
                    {
                        double num4 = Math.Ceiling(totalPrice - num3);
                        MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                        parameters2.PathToShop = PathToShopType.Donate;
                        parameters2.CloseCallback = new System.Action(this.onShopClosed);
                        parameters2.PurchaseCallback = new Action<ShopEntry, PurchaseResult>(this.onShopPurchaseCompleted);
                        parameters2.MenuContentParams = num4;
                        MiniPopupMenu.InputParameters parameter = parameters2;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.ShopOfferMiniPopupContent, parameter, 0f, true, true);
                    }
                }
            }
        }

        public void onDualButtonRightClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (this.m_shopPurchaseController != null)
                {
                    if (this.m_shopPurchaseController.isPurchaseable())
                    {
                        this.m_shopPurchaseController.purchase(this.m_shopPurchaseController.getPurchasesRemaining());
                        this.setShopEntryStackExhausted();
                    }
                }
                else if (this.m_tournamentView != null)
                {
                    int count = this.m_tournamentView.Instance.getDonationsRemaining();
                    double totalPrice = this.m_tournamentView.Instance.getDonationPrice() * count;
                    double num3 = player.getResourceAmount(ResourceType.Diamond);
                    if (this.m_tournamentView.Instance.getDonationsRemaining() <= 0)
                    {
                        PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                    }
                    else if (totalPrice <= num3)
                    {
                        CmdDonateToTournamentBucket.ExecuteStatic(player, this.m_tournamentView.TournamentInfo.Id, count, totalPrice);
                        if (!this.tryTriggerPostDonationCeremony())
                        {
                            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                        }
                    }
                    else
                    {
                        double num4 = Math.Ceiling(totalPrice - num3);
                        MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                        parameters2.PathToShop = PathToShopType.Donate;
                        parameters2.CloseCallback = new System.Action(this.onShopClosed);
                        parameters2.PurchaseCallback = new Action<ShopEntry, PurchaseResult>(this.onShopPurchaseCompleted);
                        parameters2.MenuContentParams = num4;
                        MiniPopupMenu.InputParameters parameter = parameters2;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.ShopOfferMiniPopupContent, parameter, 0f, true, true);
                    }
                }
            }
        }

        public override bool onMainButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                if (this.m_param is PlayerAugmentation)
                {
                    PlayerAugmentation param = (PlayerAugmentation) this.m_param;
                    Player player = GameLogic.Binder.GameState.Player;
                    if (player.Augmentations.canBuy(param.Id))
                    {
                        PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_BuyUpgrade, (float) 0f);
                        CmdBuyAugmentation.ExecuteStatic(player, param.Id);
                        PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                    }
                }
                else if ((this.m_shopPurchaseController != null) && this.m_shopPurchaseController.isPurchaseable())
                {
                    int num = this.m_shopPurchaseController.getPurchasesRemaining();
                    this.m_shopPurchaseController.purchase(1);
                    if (num == 1)
                    {
                        this.setShopEntryStackExhausted();
                    }
                }
            }
            return true;
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            RewardGalleryCell.Content content4;
            MiniPopupEntry entry3;
            MiniPopupMenu contentMenu = (MiniPopupMenu) base.m_contentMenu;
            this.m_param = param;
            this.m_shopPurchaseController = null;
            if (param is PlayerAugmentation)
            {
                contentMenu.MainButton.gameObject.SetActive(true);
                this.DualButtonLeft.gameObject.SetActive(false);
                this.DualButtonRight.gameObject.SetActive(false);
                PlayerAugmentation augmentation = (PlayerAugmentation) param;
                SpriteAtlasEntry sprite = null;
                string overrideDescriptionTextLocalized = null;
                string str2 = null;
                if (augmentation.PerkInstance != null)
                {
                    ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[augmentation.PerkInstance.Type];
                    sprite = data.Sprite;
                    overrideDescriptionTextLocalized = _.L(ConfigLoca.MINIPOPUP_AUGMENTATIONS_DESCRIPTION, new <>__AnonType24<string>(_.L(data.ShortDescription, null, false)), false);
                    str2 = MenuHelpers.BigModifierToString(augmentation.PerkInstance.Modifier, true);
                }
                content4 = new RewardGalleryCell.Content();
                content4.Sprite = sprite;
                content4.Text = str2;
                RewardGalleryCell.Content rewardContent = content4;
                string overrideButtonText = MenuHelpers.BigValueToString(App.Binder.ConfigMeta.GetAugmentationPrice(augmentation.Id));
                SpriteAtlasEntry atlasEntry = ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Token];
                entry3 = new MiniPopupEntry();
                entry3.TitleText = ConfigLoca.MINIPOPUP_AUGMENTATIONS_TITLE;
                contentMenu.populateLayout(entry3, true, rewardContent, overrideButtonText, PlayerView.Binder.SpriteResources.getSprite(atlasEntry), overrideDescriptionTextLocalized);
            }
            else if (param is ShopPurchaseController)
            {
                ShopPurchaseController controller = (ShopPurchaseController) param;
                this.m_shopPurchaseController = new ShopPurchaseController(controller.ShopEntry, controller.ShopEntryInstance, PathToShopType.Vendor, new System.Action(this.onShopClosed), new Action<ShopEntry, PurchaseResult>(this.onShopPurchaseCompleted));
                content4 = new RewardGalleryCell.Content();
                content4.Sprite = controller.getSprite();
                content4.StickerText = controller.getStickerText();
                RewardGalleryCell.Content content2 = content4;
                if (controller.payWithAd())
                {
                    contentMenu.MainButton.gameObject.SetActive(true);
                    this.DualButtonLeft.gameObject.SetActive(false);
                    this.DualButtonRight.gameObject.SetActive(false);
                    double v = controller.getAmount();
                    content2.Text = (v <= 1.0) ? null : ("+" + MenuHelpers.BigValueToString(v));
                    entry3 = new MiniPopupEntry();
                    entry3.TitleText = ConfigLoca.MINIPOPUP_ADS_VENDOR_TITLE;
                    contentMenu.populateLayout(entry3, true, content2, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_WATCH, null, false)), null, _.L(ConfigLoca.MINIPOPUP_SPECIAL_OFFER_DESCRIPTION, null, false));
                }
                else if (controller.getRefShopEntry().Type == ShopEntryType.MegaBoxBundle)
                {
                    contentMenu.MainButton.gameObject.SetActive(true);
                    this.DualButtonLeft.gameObject.SetActive(false);
                    this.DualButtonRight.gameObject.SetActive(false);
                    double num2 = ConfigShops.CalculateMegaBoxBundleSize(controller.getRefShopEntry().Id);
                    string str4 = null;
                    if (num2 > 1.0)
                    {
                        str4 = _.L(ConfigLoca.MINIPOPUP_MEGABOX_DESCRIPTION_MANY, new <>__AnonType9<string>(num2.ToString("0")), false);
                    }
                    else
                    {
                        str4 = _.L(ConfigLoca.MINIPOPUP_MEGABOX_DESCRIPTION_SINGLE, null, false);
                    }
                    entry3 = new MiniPopupEntry();
                    entry3.TitleText = ConfigLoca.MINIPOPUP_MEGABOX_TITLE;
                    contentMenu.populateLayout(entry3, true, content2, controller.getPriceText(1), PlayerView.Binder.SpriteResources.getSprite(controller.getPriceIcon()), str4);
                }
                else if (controller.getPurchasesRemaining() > 1)
                {
                    contentMenu.MainButton.gameObject.SetActive(false);
                    this.DualButtonLeft.gameObject.SetActive(true);
                    this.DualButtonRight.gameObject.SetActive(true);
                    this.DualButtonLeft.CornerText.text = "1x";
                    this.DualButtonLeft.Text.text = controller.getPriceText(1);
                    this.DualButtonLeft.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(controller.getPriceIcon());
                    int numPurchases = controller.getPurchasesRemaining();
                    this.DualButtonRight.CornerText.text = numPurchases + "x";
                    this.DualButtonRight.Text.text = controller.getPriceText(numPurchases);
                    this.DualButtonRight.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(controller.getPriceIcon());
                    double num4 = controller.getAmount();
                    content2.Text = (num4 <= 1.0) ? null : ("+" + MenuHelpers.BigValueToString(num4));
                    entry3 = new MiniPopupEntry();
                    entry3.TitleText = ConfigLoca.MINIPOPUP_BASIC_VENDOR_TITLE;
                    contentMenu.populateLayout(entry3, false, content2, null, null, _.L(ConfigLoca.MINIPOPUP_BASIC_VENDOR_DESCRIPTION, null, false));
                }
                else
                {
                    contentMenu.MainButton.gameObject.SetActive(true);
                    this.DualButtonLeft.gameObject.SetActive(false);
                    this.DualButtonRight.gameObject.SetActive(false);
                    double num5 = controller.getAmount();
                    content2.Text = (num5 <= 1.0) ? null : ("+" + MenuHelpers.BigValueToString(num5));
                    entry3 = new MiniPopupEntry();
                    entry3.TitleText = ConfigLoca.MINIPOPUP_BASIC_VENDOR_TITLE;
                    contentMenu.populateLayout(entry3, true, content2, controller.getPriceText(1), PlayerView.Binder.SpriteResources.getSprite(controller.getPriceIcon()), _.L(ConfigLoca.MINIPOPUP_BASIC_VENDOR_DESCRIPTION, null, false));
                }
            }
            else if (param is TournamentView)
            {
                this.m_tournamentView = (TournamentView) param;
                contentMenu.MainButton.gameObject.SetActive(false);
                this.DualButtonLeft.gameObject.SetActive(true);
                this.DualButtonRight.gameObject.SetActive(true);
                double num6 = this.m_tournamentView.Instance.getDonationPrice();
                this.DualButtonLeft.CornerText.text = "1x";
                this.DualButtonLeft.Text.text = num6.ToString("0");
                this.DualButtonLeft.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Diamond]);
                int num7 = this.m_tournamentView.Instance.getDonationsRemaining();
                this.DualButtonRight.CornerText.text = num7 + "x";
                this.DualButtonRight.Text.text = (num7 * num6).ToString("0");
                this.DualButtonRight.Icon.sprite = this.DualButtonLeft.Icon.sprite;
                content4 = new RewardGalleryCell.Content();
                content4.Sprite = new SpriteAtlasEntry("Menu", "icon_cardpack_floater");
                RewardGalleryCell.Content content3 = content4;
                entry3 = new MiniPopupEntry();
                entry3.TitleText = ConfigLoca.UI_BUTTON_INFO;
                contentMenu.populateLayout(entry3, false, content3, null, null, _.L(ConfigLoca.ADVPANEL_DONATE_DESCRIPTION, null, false));
            }
            this.onRefresh();
        }

        protected override void onRefresh()
        {
        }

        private void onShopClosed()
        {
            if ((PlayerView.Binder.MenuSystem.topmostActiveMenu() != null) && (PlayerView.Binder.MenuSystem.topmostActiveMenu().activeContentType() == MenuContentType.VendorPopupContent))
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.NONE, MenuContentType.NONE, null, 0f, false, true);
            }
            else
            {
                MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                parameters2.MenuContentParams = this.m_param;
                MiniPopupMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.VendorMiniPopupContent, parameter, 0f, false, true);
            }
        }

        private void onShopPurchaseCompleted(ShopEntry shopEntry, PurchaseResult purchaseResult)
        {
            if (ConfigShops.IsIapShopEntry(shopEntry))
            {
                MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                parameters2.MenuContentParams = this.m_param;
                MiniPopupMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.waitAndTransitionToNewMenu(MenuType.MiniPopupMenu, MenuContentType.VendorMiniPopupContent, parameter);
            }
        }

        private void setShopEntryStackExhausted()
        {
            if (this.m_shopPurchaseController != null)
            {
                Player player = GameLogic.Binder.GameState.Player;
                ShopEntry entry = this.m_shopPurchaseController.getRefShopEntry();
                player.setStackExhausted(entry.Id);
            }
        }

        private bool tryTriggerPostDonationCeremony()
        {
            Reward reward = GameLogic.Binder.GameState.Player.getFirstUnclaimedReward(ChestType.TournamentCards, RewardSourceType.SelfReward);
            if (reward != null)
            {
                string localizedTitle = StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_DONATION_TITLE, null, false));
                string localizedDescription = _.L(ConfigLoca.BH_GIFT_FROM_YOU, null, false) + "\n" + _.L(ConfigLoca.UI_PROMPT_CHOOSE_ONE, null, false);
                RewardCeremonyMenu.StartCardPackCeremony(localizedTitle, localizedDescription, reward, true);
                return true;
            }
            return false;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.VendorMiniPopupContent;
            }
        }
    }
}

