namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ShopPurchaseController
    {
        [CompilerGenerated]
        private System.Action <CloseCallback>k__BackingField;
        [CompilerGenerated]
        private PathToShopType <PathToShop>k__BackingField;
        [CompilerGenerated]
        private ResourceType? <PayWithResource>k__BackingField;
        [CompilerGenerated]
        private double <Price>k__BackingField;
        [CompilerGenerated]
        private Action<GameLogic.ShopEntry, PurchaseResult> <PurchaseCallback>k__BackingField;
        [CompilerGenerated]
        private GameLogic.ShopEntry <ShopEntry>k__BackingField;
        [CompilerGenerated]
        private GameLogic.ShopEntryInstance <ShopEntryInstance>k__BackingField;
        [CompilerGenerated]
        private string <StackText>k__BackingField;
        [CompilerGenerated]
        private string <StickerText>k__BackingField;

        public ShopPurchaseController(GameLogic.ShopEntry shopEntry, GameLogic.ShopEntryInstance shopEntryInstance, PathToShopType pathToShop, [Optional, DefaultParameterValue(null)] System.Action closeCallback, [Optional, DefaultParameterValue(null)] Action<GameLogic.ShopEntry, PurchaseResult> purchaseCallback)
        {
            this.ShopEntry = shopEntry;
            this.ShopEntryInstance = shopEntryInstance;
            this.PathToShop = pathToShop;
            this.CloseCallback = closeCallback;
            this.PurchaseCallback = purchaseCallback;
            this.updateDetails();
        }

        private void boostPostPaymentCallback(List<Reward> rewards, bool awardReward, int numPurchases)
        {
            if (awardReward)
            {
                this.markShopEntryInstanceAsSold(numPurchases);
                this.transitionToRewardCeremony(rewards, ConfigUi.CeremonyEntries.SHOP_PURCHASE_BOOST);
            }
            else
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        private void genericPostPaymentCallback(List<Reward> rewards, bool awardReward, int numPurchases)
        {
            if (awardReward)
            {
                this.markShopEntryInstanceAsSold(numPurchases);
                this.transitionToRewardCeremony(rewards, ConfigUi.CeremonyEntries.SHOP_PURCHASE);
            }
            else
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public double getAmount()
        {
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            GameLogic.ShopEntry entry = this.getRefShopEntry();
            switch (entry.Type)
            {
                case ShopEntryType.CoinBundle:
                    return ConfigShops.CalculateCoinBundleSize(player, entry.Id, 1);

                case ShopEntryType.IapDiamonds:
                    return entry.BuyResourceAmounts[ResourceType.Diamond];

                case ShopEntryType.TokenBundle:
                    return ConfigShops.CalculateTokenBundleSize(player, entry.Id);

                case ShopEntryType.SpecialChest:
                    return 1.0;

                case ShopEntryType.RewardBox:
                    return 1.0;

                case ShopEntryType.ReviveBundle:
                    return ConfigShops.CalculateReviveBundleSize(entry.Id);

                case ShopEntryType.FrenzyBundle:
                    return ConfigShops.CalculateFrenzyBundleSize(entry.Id);

                case ShopEntryType.DustBundle:
                    return ConfigShops.CalculateDustBundleSize(activeCharacter, entry.Id);

                case ShopEntryType.DiamondBundle:
                    return ConfigShops.CalculateDiamondBundleSize(player, entry.Id);

                case ShopEntryType.XpBundle:
                    return 1.0;

                case ShopEntryType.BossBundle:
                    return 1.0;

                case ShopEntryType.PetBox:
                    return 1.0;

                case ShopEntryType.MegaBoxBundle:
                    return ConfigShops.CalculateMegaBoxBundleSize(entry.Id);

                case ShopEntryType.LootBox:
                    return 1.0;
            }
            return 0.0;
        }

        private int getDefaultNumAllowedPurchases()
        {
            if ((this.ShopEntryInstance != null) && ConfigShops.IsVendorShopEntry(this.ShopEntryInstance.ShopEntry.Id))
            {
                Player player = GameLogic.Binder.GameState.Player;
                switch (player.Vendor.getSlotNumber(this.ShopEntryInstance))
                {
                    case 1:
                        return App.Binder.ConfigMeta.VENDOR_SLOT1_NUM_ALLOWED_PURCHASES;

                    case 2:
                    {
                        int num = player.getNumberOfAllowedPurchases(this.ShopEntryInstance.ShopEntry.Id);
                        return ((num <= 0) ? App.Binder.ConfigMeta.VENDOR_SLOT2_NUM_ALLOWED_PURCHASES : num);
                    }
                    case 3:
                    {
                        int num2 = player.getNumberOfAllowedPurchases(this.ShopEntryInstance.ShopEntry.Id);
                        return ((num2 <= 0) ? App.Binder.ConfigMeta.VENDOR_SLOT3_NUM_ALLOWED_PURCHASES : num2);
                    }
                }
            }
            return 1;
        }

        public SpriteAtlasEntry getPriceIcon()
        {
            if (this.payWithAd())
            {
                return PlayerView.Binder.SpriteResources.IconWatchVideo;
            }
            if (this.PayWithResource.HasValue)
            {
                return ConfigUi.RESOURCE_TYPE_SPRITES[this.PayWithResource.Value];
            }
            return null;
        }

        public string getPriceText([Optional, DefaultParameterValue(1)] int numPurchases)
        {
            if (this.payWithAd())
            {
                return StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_WATCH, null, false));
            }
            GameLogic.ShopEntry entry = this.getRefShopEntry();
            if (ConfigShops.IsIapShopEntry(entry))
            {
                return entry.FormattedPrice;
            }
            return MenuHelpers.BigValueToString(this.Price * numPurchases);
        }

        public int getPurchasesRemaining()
        {
            if (this.ShopEntryInstance != null)
            {
                return Mathf.Max(this.getDefaultNumAllowedPurchases() - this.ShopEntryInstance.NumTimesPurchased, 0);
            }
            return 1;
        }

        public GameLogic.ShopEntry getRefShopEntry()
        {
            if (this.ShopEntryInstance != null)
            {
                return this.ShopEntryInstance.ShopEntry;
            }
            return this.ShopEntry;
        }

        public SpriteAtlasEntry getSprite()
        {
            GameLogic.ShopEntry entry = this.getRefShopEntry();
            if (entry.Type == ShopEntryType.Boost)
            {
                return ConfigBoosts.SHARED_DATA[entry.Boost].Sprite;
            }
            if (entry.Type == ShopEntryType.SpecialChest)
            {
                if ((this.ShopEntryInstance != null) && (this.ShopEntryInstance.PrerolledChestType != ChestType.NONE))
                {
                    return ConfigUi.CHEST_BLUEPRINTS[this.ShopEntryInstance.PrerolledChestType].Icon;
                }
                return ConfigUi.CHEST_BLUEPRINTS[entry.ChestType].Icon;
            }
            if (((entry.Type != ShopEntryType.RewardBox) && (entry.Type != ShopEntryType.PetBox)) && (entry.Type != ShopEntryType.LootBox))
            {
                return entry.Sprite;
            }
            return ConfigUi.CHEST_BLUEPRINTS[entry.ChestType].Icon;
        }

        public string getStackText()
        {
            return this.StackText;
        }

        public string getStickerText()
        {
            return this.StickerText;
        }

        public string getTitle()
        {
            GameLogic.ShopEntry entry = this.getRefShopEntry();
            double v = this.getAmount();
            switch (entry.Type)
            {
                case ShopEntryType.CoinBundle:
                    return _.L(ConfigLoca.RESOURCES_COINS_MULTILINE, new <>__AnonType9<string>(MenuHelpers.BigValueToString(v)), false);

                case ShopEntryType.IapDiamonds:
                    return _.L(ConfigLoca.RESOURCES_DIAMONDS_MULTILINE, new <>__AnonType9<string>(v.ToString("0")), false);

                case ShopEntryType.TokenBundle:
                    return _.L(ConfigLoca.RESOURCES_TOKENS_MULTILINE, new <>__AnonType9<string>(MenuHelpers.BigValueToString(v)), false);

                case ShopEntryType.SpecialChest:
                    if ((this.ShopEntryInstance == null) || (this.ShopEntryInstance.PrerolledChestType == ChestType.NONE))
                    {
                        return _.L(ConfigUi.CHEST_BLUEPRINTS[entry.ChestType].ShortName, null, false);
                    }
                    return _.L(ConfigUi.CHEST_BLUEPRINTS[this.ShopEntryInstance.PrerolledChestType].ShortName, null, false);

                case ShopEntryType.RewardBox:
                    return _.L(ConfigUi.CHEST_BLUEPRINTS[entry.ChestType].ShortName, null, false);

                case ShopEntryType.ReviveBundle:
                    return _.L(ConfigLoca.VENDOR_REVIVES, new <>__AnonType9<string>(v.ToString("0")), false);

                case ShopEntryType.FrenzyBundle:
                    return _.L(ConfigLoca.VENDOR_FRENZY_POTIONS, new <>__AnonType9<string>(v.ToString("0")), false);

                case ShopEntryType.DustBundle:
                    return _.L(ConfigLoca.RESOURCES_DUSTS_MULTILINE, new <>__AnonType9<string>(MenuHelpers.BigValueToString(v)), false);

                case ShopEntryType.DiamondBundle:
                    return _.L(ConfigLoca.RESOURCES_DIAMONDS_MULTILINE, new <>__AnonType9<string>(MenuHelpers.BigValueToString(v)), false);

                case ShopEntryType.XpBundle:
                    return StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_XP_POTIONS, null, false));

                case ShopEntryType.BossBundle:
                    return StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_BOSS_BUNDLE, null, false));

                case ShopEntryType.PetBox:
                    return _.L(ConfigUi.CHEST_BLUEPRINTS[entry.ChestType].ShortName, null, false);

                case ShopEntryType.MegaBoxBundle:
                    if (v != 1.0)
                    {
                        return _.L(ConfigLoca.VENDOR_MEGA_BOX_BUNDLE_PLURAL, new <>__AnonType9<double>(v), false);
                    }
                    return _.L(ConfigLoca.VENDOR_MEGA_BOX_BUNDLE, new <>__AnonType9<double>(v), false);

                case ShopEntryType.LootBox:
                    return _.L(ConfigUi.CHEST_BLUEPRINTS[entry.ChestType].ShortName, null, false);
            }
            return StringExtensions.ToUpperLoca(_.L(entry.Title, null, false));
        }

        public bool isPurchaseable()
        {
            if (this.getRefShopEntry().PurchaseDisabled)
            {
                return false;
            }
            if (this.isSold())
            {
                return false;
            }
            return true;
        }

        public bool isSold()
        {
            return ((this.ShopEntryInstance != null) && this.ShopEntryInstance.Sold);
        }

        private void markShopEntryInstanceAsSold(int numPurchases)
        {
            if (this.ShopEntryInstance != null)
            {
                GameLogic.ShopEntryInstance shopEntryInstance = this.ShopEntryInstance;
                shopEntryInstance.NumTimesPurchased += numPurchases;
                int num = this.getDefaultNumAllowedPurchases();
                if (this.ShopEntryInstance.NumTimesPurchased >= num)
                {
                    this.ShopEntryInstance.Sold = true;
                }
            }
        }

        private void mysteryOrChestPostPaymentCallback(List<Reward> rewards, bool awardReward, int numPurchases)
        {
            if (awardReward)
            {
                this.markShopEntryInstanceAsSold(numPurchases);
                GameLogic.Binder.GameState.Player.LastMysteryChestDropTimestamp = Service.Binder.ServerTime.GameTime;
                this.transitionToRewardCeremony(rewards, ConfigUi.CeremonyEntries.VENDOR_ITEM_OR_CHEST);
            }
            else
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public bool payWithAd()
        {
            return (!ConfigShops.IsIapShopEntry(this.getRefShopEntry()) && (!this.PayWithResource.HasValue || (this.Price <= 0.0)));
        }

        public void purchase([Optional, DefaultParameterValue(1)] int numPurchases)
        {
            ChestType nONE;
            int num15;
            List<ShopEntryType> list4;
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
                return;
            }
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            if (GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom == null)
            {
                return;
            }
            GameLogic.ShopEntry entry = this.getRefShopEntry();
            bool flag = this.payWithAd();
            double price = this.Price * numPurchases;
            if (ConfigShops.IsIapShopEntry(entry))
            {
                IapMiniPopupContent.InputParameters parameters4 = new IapMiniPopupContent.InputParameters();
                parameters4.ShopEntry = entry;
                parameters4.PathToShop = this.PathToShop;
                parameters4.PurchaseCallback = this.PurchaseCallback;
                IapMiniPopupContent.InputParameters parameter = parameters4;
                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.TechPopupMenu, MenuContentType.IapMiniPopupContent, parameter);
                return;
            }
            if ((!flag && this.PayWithResource.HasValue) && (player.getResourceAmount(this.PayWithResource.Value) < price))
            {
                MiniPopupMenu.InputParameters parameters5 = new MiniPopupMenu.InputParameters();
                parameters5.PathToShop = this.PathToShop;
                parameters5.CloseCallback = this.CloseCallback;
                parameters5.PurchaseCallback = this.PurchaseCallback;
                parameters5.MenuContentParams = price - player.getResourceAmount(this.PayWithResource.Value);
                MiniPopupMenu.InputParameters parameters2 = parameters5;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.ShopOfferMiniPopupContent, parameters2, 0f, true, true);
                return;
            }
            List<Reward> list = new List<Reward>(numPurchases);
            Action<List<Reward>, bool, int> action = null;
            bool flag2 = false;
            switch (entry.Type)
            {
                case ShopEntryType.CoinBundle:
                {
                    Reward item = new Reward();
                    MathUtil.DistributeValuesIntoChunksDouble(ConfigShops.CalculateCoinBundleSize(player, entry.Id, numPurchases), entry.NumBursts, ref item.CoinDrops);
                    list.Add(item);
                    action = new Action<List<Reward>, bool, int>(this.genericPostPaymentCallback);
                    goto Label_07F8;
                }
                case ShopEntryType.Boost:
                    for (int j = 0; j < numPurchases; j++)
                    {
                        Reward reward4 = new Reward();
                        reward4.Boost = entry.Boost;
                        list.Add(reward4);
                    }
                    action = new Action<List<Reward>, bool, int>(this.boostPostPaymentCallback);
                    goto Label_07F8;

                case ShopEntryType.MysteryItem:
                    for (int k = 0; k < numPurchases; k++)
                    {
                        ItemInstance instance2 = ItemInstance.Create(ConfigShops.GetRandomMysteryItem(player, entry.Id), player, -1);
                        Reward reward5 = new Reward();
                        reward5.ChestType = ChestType.Vendor;
                        reward5.ItemDrops.Add(instance2);
                        list.Add(reward5);
                    }
                    action = new Action<List<Reward>, bool, int>(this.mysteryOrChestPostPaymentCallback);
                    goto Label_07F8;

                case ShopEntryType.TokenBundle:
                {
                    Reward reward2 = new Reward();
                    double num3 = ConfigShops.CalculateTokenBundleSize(player, entry.Id) * numPurchases;
                    MathUtil.DistributeValuesIntoChunksInt((int) num3, entry.NumBursts, ref reward2.TokenDrops);
                    list.Add(reward2);
                    action = new Action<List<Reward>, bool, int>(this.genericPostPaymentCallback);
                    goto Label_07F8;
                }
                case ShopEntryType.SpecialChest:
                    if (this.ShopEntryInstance == null)
                    {
                        Debug.LogError("ShopEntryType.SpecialChest only supported for ShopEntryInstances: " + entry.Id);
                        flag2 = true;
                        goto Label_07F8;
                    }
                    nONE = ChestType.NONE;
                    if (this.ShopEntryInstance.PrerolledChestType == ChestType.NONE)
                    {
                        nONE = this.ShopEntryInstance.ShopEntry.ChestType;
                        break;
                    }
                    nONE = this.ShopEntryInstance.PrerolledChestType;
                    break;

                case ShopEntryType.RewardBox:
                    if (this.ShopEntryInstance == null)
                    {
                        Debug.LogError("ShopEntryType.RewardBox only supported for ShopEntryInstances: " + entry.Id);
                        flag2 = true;
                    }
                    else
                    {
                        ChestType chestType = this.ShopEntryInstance.ShopEntry.ChestType;
                        List<ShopEntryType> list3 = null;
                        if (this.PayWithResource.HasValue && (((ResourceType) this.PayWithResource) == ResourceType.Diamond))
                        {
                            list4 = new List<ShopEntryType>();
                            list4.Add(ShopEntryType.DiamondBundle);
                            list3 = list4;
                        }
                        for (int m = 0; m < numPurchases; m++)
                        {
                            Reward reward = new Reward();
                            reward.ChestType = chestType;
                            CmdRollChestLootTable.ExecuteStatic(chestType, player, false, ref reward, list3);
                            list.Add(reward);
                        }
                        action = new Action<List<Reward>, bool, int>(this.mysteryOrChestPostPaymentCallback);
                    }
                    goto Label_07F8;

                case ShopEntryType.ReviveBundle:
                {
                    Reward reward8 = new Reward();
                    reward8.Revives = ((int) ConfigShops.CalculateReviveBundleSize(entry.Id)) * numPurchases;
                    list.Add(reward8);
                    action = new Action<List<Reward>, bool, int>(this.genericPostPaymentCallback);
                    goto Label_07F8;
                }
                case ShopEntryType.FrenzyBundle:
                {
                    Reward reward9 = new Reward();
                    reward9.FrenzyPotions = ((int) ConfigShops.CalculateFrenzyBundleSize(entry.Id)) * numPurchases;
                    list.Add(reward9);
                    action = new Action<List<Reward>, bool, int>(this.genericPostPaymentCallback);
                    goto Label_07F8;
                }
                case ShopEntryType.DustBundle:
                {
                    Reward reward10 = new Reward();
                    double num9 = ConfigShops.CalculateDustBundleSize(activeCharacter, entry.Id) * numPurchases;
                    MathUtil.DistributeValuesIntoChunksInt((int) num9, entry.NumBursts, ref reward10.DustDrops);
                    list.Add(reward10);
                    action = new Action<List<Reward>, bool, int>(this.genericPostPaymentCallback);
                    goto Label_07F8;
                }
                case ShopEntryType.DiamondBundle:
                {
                    Reward reward3 = new Reward();
                    double num4 = ConfigShops.CalculateDiamondBundleSize(player, entry.Id) * numPurchases;
                    MathUtil.DistributeValuesIntoChunksInt((int) num4, entry.NumBursts, ref reward3.DiamondDrops);
                    list.Add(reward3);
                    action = new Action<List<Reward>, bool, int>(this.genericPostPaymentCallback);
                    goto Label_07F8;
                }
                case ShopEntryType.XpBundle:
                {
                    Reward reward11 = new Reward();
                    reward11.XpPotions = ((int) ConfigShops.CalculateXpBundleSize(entry.Id)) * numPurchases;
                    list.Add(reward11);
                    action = new Action<List<Reward>, bool, int>(this.genericPostPaymentCallback);
                    goto Label_07F8;
                }
                case ShopEntryType.BossBundle:
                {
                    Reward reward12 = new Reward();
                    reward12.BossPotions = ((int) ConfigShops.CalculateBossBundleSize(entry.Id)) * numPurchases;
                    list.Add(reward12);
                    action = new Action<List<Reward>, bool, int>(this.genericPostPaymentCallback);
                    goto Label_07F8;
                }
                case ShopEntryType.PetBundle:
                    for (int n = 0; n < numPurchases; n++)
                    {
                        string str = CmdRollPetBundleLootTable.ExecuteStatic(App.Binder.ConfigLootTables.PetBundleLootTables[entry.Id], player);
                        int num11 = ConfigShops.CalculatePetBundleSize(player, entry.Id);
                        Reward reward13 = new Reward();
                        PetReward reward17 = new PetReward();
                        reward17.PetId = str;
                        reward17.Amount = num11;
                        reward13.Pets.Add(reward17);
                        list.Add(reward13);
                    }
                    action = new Action<List<Reward>, bool, int>(this.mysteryOrChestPostPaymentCallback);
                    goto Label_07F8;

                case ShopEntryType.PetBox:
                    for (int num12 = 0; num12 < numPurchases; num12++)
                    {
                        Reward reward14 = new Reward();
                        reward14.ChestType = entry.ChestType;
                        CmdRollChestLootTable.ExecuteStatic(entry.ChestType, player, false, ref reward14, null);
                        list.Add(reward14);
                    }
                    action = new Action<List<Reward>, bool, int>(this.mysteryOrChestPostPaymentCallback);
                    goto Label_07F8;

                case ShopEntryType.MegaBoxBundle:
                    for (int num13 = 0; num13 < numPurchases; num13++)
                    {
                        Reward reward15 = new Reward();
                        reward15.MegaBoxes = (int) ConfigShops.CalculateMegaBoxBundleSize(entry.Id);
                        list.Add(reward15);
                    }
                    action = new Action<List<Reward>, bool, int>(this.skipCeremonyPostPaymentCallback);
                    goto Label_07F8;

                case ShopEntryType.LootBox:
                    for (int num14 = 0; num14 < numPurchases; num14++)
                    {
                        Reward reward16 = new Reward();
                        reward16.ChestType = entry.ChestType;
                        CmdRollChestLootTable.ExecuteStatic(entry.ChestType, player, false, ref reward16, null);
                        list.Add(reward16);
                    }
                    action = new Action<List<Reward>, bool, int>(this.mysteryOrChestPostPaymentCallback);
                    goto Label_07F8;

                default:
                    flag2 = true;
                    goto Label_07F8;
            }
            List<ShopEntryType> disallowedShopEntryTypes = null;
            if (this.PayWithResource.HasValue && (((ResourceType) this.PayWithResource) == ResourceType.Diamond))
            {
                list4 = new List<ShopEntryType>();
                list4.Add(ShopEntryType.DiamondBundle);
                disallowedShopEntryTypes = list4;
            }
            for (int i = 0; i < numPurchases; i++)
            {
                Reward reward6 = new Reward();
                reward6.ChestType = nONE;
                CmdRollChestLootTable.ExecuteStatic(nONE, player, false, ref reward6, disallowedShopEntryTypes);
                if (reward6.ItemDrops.Count > 0)
                {
                    player.updateItemRollHistory(reward6.ItemDrops[0]);
                }
                list.Add(reward6);
            }
            action = new Action<List<Reward>, bool, int>(this.mysteryOrChestPostPaymentCallback);
        Label_07F8:
            num15 = 0;
            while (num15 < list.Count)
            {
                list[num15].ShopEntryId = entry.Id;
                num15++;
            }
            if (!flag2)
            {
                if (flag)
                {
                    FullscreenAdMenu.InputParameters parameters6 = new FullscreenAdMenu.InputParameters();
                    parameters6.AdZoneId = AdsSystem.ADS_VENDOR_ZONE;
                    parameters6.AdCategory = AdsData.Category.VENDOR;
                    parameters6.Reward = list[0];
                    parameters6.CompleteCallback = action;
                    FullscreenAdMenu.InputParameters parameters3 = parameters6;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.FullscreenAdMenu, MenuContentType.NONE, parameters3, 0f, false, true);
                }
                else
                {
                    CmdGainResources.ExecuteStatic(player, this.PayWithResource.Value, -price, false, string.Empty, null);
                    PlayerView.Binder.EventBus.ShopEntryPurchased(player, this.ShopEntryInstance, this.ShopEntry, this.PayWithResource.Value, price, numPurchases);
                    action(list, true, numPurchases);
                }
            }
            if (this.PurchaseCallback != null)
            {
                if (flag2)
                {
                    this.PurchaseCallback(entry, PurchaseResult.Fail);
                }
                else
                {
                    this.PurchaseCallback(entry, PurchaseResult.Success);
                }
            }
        }

        private void skipCeremonyPostPaymentCallback(List<Reward> rewards, bool awardReward, int numPurchases)
        {
            if (awardReward)
            {
                this.markShopEntryInstanceAsSold(numPurchases);
                for (int i = 0; i < rewards.Count; i++)
                {
                    CmdConsumeReward.ExecuteStatic(GameLogic.Binder.GameState.Player, rewards[i], false, string.Empty);
                }
            }
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.NONE, MenuContentType.NONE, null, 0f, false, true);
        }

        private void transitionToRewardCeremony(List<Reward> rewards, RewardCeremonyEntry rce)
        {
            if ((rewards != null) && (rewards.Count > 0))
            {
                RewardCeremonyMenu.InputParameters parameters3;
                Player player = GameLogic.Binder.GameState.Player;
                if (rewards.Count == 1)
                {
                    CmdConsumeReward.ExecuteStatic(player, rewards[0], true, string.Empty);
                    CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
                    parameters3 = new RewardCeremonyMenu.InputParameters();
                    parameters3.Title = StringExtensions.ToUpperLoca(_.L(rce.Title, null, false));
                    parameters3.Description = _.L(rce.Description, null, false);
                    parameters3.SingleRewardOpenAtStart = rce.ChestOpenAtStart;
                    parameters3.SingleReward = rewards[0];
                    RewardCeremonyMenu.InputParameters parameter = parameters3;
                    PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter);
                }
                else
                {
                    Dictionary<Reward, bool> dictionary = new Dictionary<Reward, bool>(rewards.Count);
                    for (int i = 0; i < rewards.Count; i++)
                    {
                        CmdConsumeReward.ExecuteStatic(player, rewards[i], true, string.Empty);
                        dictionary.Add(rewards[i], false);
                    }
                    CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
                    parameters3 = new RewardCeremonyMenu.InputParameters();
                    parameters3.Title = StringExtensions.ToUpperLoca(_.L(rce.Title, null, false));
                    parameters3.Description = _.L(rce.Description, null, false);
                    parameters3.MultiRewards = dictionary;
                    RewardCeremonyMenu.InputParameters parameters2 = parameters3;
                    PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameters2);
                }
            }
            else
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void updateDetails()
        {
            GameLogic.ShopEntry entry = this.getRefShopEntry();
            if (entry != null)
            {
                Player player = GameLogic.Binder.GameState.Player;
                this.PayWithResource = null;
                this.Price = 0.0;
                this.StickerText = null;
                this.StackText = null;
                if (this.ShopEntryInstance != null)
                {
                    int num = this.getPurchasesRemaining();
                    this.StackText = (num <= 1) ? null : num.ToString();
                }
                else
                {
                    this.StackText = null;
                }
                if (!ConfigShops.IsVendorShopEntry(entry.Id))
                {
                    this.PayWithResource = entry.CostResource;
                    this.Price = entry.CostAmount;
                }
                else
                {
                    switch (player.Vendor.getSlotNumber(this.ShopEntryInstance))
                    {
                        case 1:
                            if ((!App.Binder.ConfigMeta.VENDOR_SLOT1_ADS || !Service.Binder.AdsSystem.adReady()) || (player.DailyAdCountVendor >= App.Binder.ConfigMeta.DAILY_ADS_LIMIT_VENDOR))
                            {
                                this.Price = App.Binder.ConfigMeta.VENDOR_SLOT1_PRICE;
                                this.PayWithResource = 2;
                                return;
                            }
                            this.Price = 0.0;
                            this.PayWithResource = null;
                            return;

                        case 2:
                        {
                            VendorPriceData vendorPriceData = App.Binder.ConfigMeta.GetVendorPriceData(entry.Id);
                            if (vendorPriceData == null)
                            {
                                this.Price = App.Binder.ConfigMeta.VENDOR_SLOT2_PRICE;
                                break;
                            }
                            this.Price = vendorPriceData.Price;
                            this.StickerText = vendorPriceData.StickerText;
                            break;
                        }
                        case 3:
                        {
                            VendorPriceData data2 = App.Binder.ConfigMeta.GetVendorPriceData(entry.Id);
                            if (data2 == null)
                            {
                                this.Price = App.Binder.ConfigMeta.VENDOR_SLOT3_PRICE;
                            }
                            else
                            {
                                this.Price = data2.Price;
                                this.StickerText = data2.StickerText;
                            }
                            this.PayWithResource = 2;
                            return;
                        }
                        default:
                        {
                            VendorPriceData data3 = App.Binder.ConfigMeta.GetVendorPriceData(entry.Id);
                            if (data3 != null)
                            {
                                this.Price = data3.Price;
                                this.PayWithResource = 2;
                                this.StickerText = data3.StickerText;
                            }
                            else
                            {
                                this.Price = 100.0;
                                this.PayWithResource = 2;
                                Debug.LogError("Cannot update price details for entry: " + entry.Id);
                            }
                            return;
                        }
                    }
                    this.PayWithResource = 2;
                }
            }
        }

        public System.Action CloseCallback
        {
            [CompilerGenerated]
            get
            {
                return this.<CloseCallback>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CloseCallback>k__BackingField = value;
            }
        }

        public PathToShopType PathToShop
        {
            [CompilerGenerated]
            get
            {
                return this.<PathToShop>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PathToShop>k__BackingField = value;
            }
        }

        public ResourceType? PayWithResource
        {
            [CompilerGenerated]
            get
            {
                return this.<PayWithResource>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PayWithResource>k__BackingField = value;
            }
        }

        public double Price
        {
            [CompilerGenerated]
            get
            {
                return this.<Price>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Price>k__BackingField = value;
            }
        }

        public Action<GameLogic.ShopEntry, PurchaseResult> PurchaseCallback
        {
            [CompilerGenerated]
            get
            {
                return this.<PurchaseCallback>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PurchaseCallback>k__BackingField = value;
            }
        }

        public GameLogic.ShopEntry ShopEntry
        {
            [CompilerGenerated]
            get
            {
                return this.<ShopEntry>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ShopEntry>k__BackingField = value;
            }
        }

        public GameLogic.ShopEntryInstance ShopEntryInstance
        {
            [CompilerGenerated]
            get
            {
                return this.<ShopEntryInstance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ShopEntryInstance>k__BackingField = value;
            }
        }

        public string StackText
        {
            [CompilerGenerated]
            get
            {
                return this.<StackText>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<StackText>k__BackingField = value;
            }
        }

        public string StickerText
        {
            [CompilerGenerated]
            get
            {
                return this.<StickerText>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<StickerText>k__BackingField = value;
            }
        }
    }
}

