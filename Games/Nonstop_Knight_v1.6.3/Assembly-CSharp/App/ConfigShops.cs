namespace App
{
    using GameLogic;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class ConfigShops
    {
        public static ShopEntry[] IAP_SHOP_ENTRIES;
        public static string IAP_STARTER_BUNDLE_ID = "com.koplagames.kopla02.dragonbundle";
        public static ShopEntry[] VENDOR_SHOP_ENTRIES;

        static ConfigShops()
        {
            ShopEntry[] entryArray1 = new ShopEntry[7];
            ShopEntry entry = new ShopEntry();
            entry.Id = IAP_STARTER_BUNDLE_ID;
            entry.Type = ShopEntryType.IapStarterBundle;
            entry.CostAmount = 4.99;
            entry.FormattedPrice = string.Empty;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_pet001");
            entryArray1[0] = entry;
            entry = new ShopEntry();
            entry.Id = "com.koplagames.kopla01.diamondssmall";
            entry.Type = ShopEntryType.IapDiamonds;
            Dictionary<ResourceType, double> dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Diamond, 100.0);
            entry.BuyResourceAmounts = dictionary;
            entry.CostAmount = 1.99;
            entry.FormattedPrice = string.Empty;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_gem_pile1");
            entry.NumBursts = 2;
            entry.PurchaseDisabled = true;
            entryArray1[1] = entry;
            entry = new ShopEntry();
            entry.Id = "com.koplagames.kopla01.diamondsmedium";
            entry.Type = ShopEntryType.IapDiamonds;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Diamond, 275.0);
            entry.BuyResourceAmounts = dictionary;
            entry.CostAmount = 4.99;
            entry.FormattedPrice = string.Empty;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_gem_pile2");
            entry.NumBursts = 4;
            entry.PurchaseDisabled = true;
            entryArray1[2] = entry;
            entry = new ShopEntry();
            entry.Id = "com.koplagames.kopla01.diamondsmediumplus";
            entry.Type = ShopEntryType.IapDiamonds;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Diamond, 600.0);
            entry.BuyResourceAmounts = dictionary;
            entry.CostAmount = 8.99;
            entry.FormattedPrice = string.Empty;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_gem_pile3");
            entry.NumBursts = 6;
            entry.PurchaseDisabled = true;
            entryArray1[3] = entry;
            entry = new ShopEntry();
            entry.Id = "com.koplagames.kopla01.diamondslarge";
            entry.Type = ShopEntryType.IapDiamonds;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Diamond, 1250.0);
            entry.BuyResourceAmounts = dictionary;
            entry.CostAmount = 19.99;
            entry.FormattedPrice = string.Empty;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_gem_pile4");
            entry.NumBursts = 8;
            entry.PurchaseDisabled = true;
            entryArray1[4] = entry;
            entry = new ShopEntry();
            entry.Id = "com.koplagames.kopla01.diamondsxlarge";
            entry.Type = ShopEntryType.IapDiamonds;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Diamond, 3250.0);
            entry.BuyResourceAmounts = dictionary;
            entry.CostAmount = 49.99;
            entry.FormattedPrice = string.Empty;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_gem_pile5");
            entry.NumBursts = 12;
            entry.PurchaseDisabled = true;
            entryArray1[5] = entry;
            entry = new ShopEntry();
            entry.Id = "com.koplagames.kopla01.diamondsxlargeplus";
            entry.Type = ShopEntryType.IapDiamonds;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Diamond, 6900.0);
            entry.BuyResourceAmounts = dictionary;
            entry.CostAmount = 79.99;
            entry.FormattedPrice = string.Empty;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_gem_pile6");
            entry.NumBursts = 0x10;
            entry.PurchaseDisabled = true;
            entryArray1[6] = entry;
            IAP_SHOP_ENTRIES = entryArray1;
            ShopEntry[] entryArray2 = new ShopEntry[0x31];
            entry = new ShopEntry();
            entry.Id = "ReviveBundleSmall";
            entry.Type = ShopEntryType.ReviveBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_bottle_red");
            entryArray2[0] = entry;
            entry = new ShopEntry();
            entry.Id = "ReviveBundleMedium";
            entry.Type = ShopEntryType.ReviveBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_bottle_red_pile2");
            entryArray2[1] = entry;
            entry = new ShopEntry();
            entry.Id = "ReviveBundleLarge";
            entry.Type = ShopEntryType.ReviveBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_bottle_red_pile3");
            entryArray2[2] = entry;
            entry = new ShopEntry();
            entry.Id = "AnyUnlockedSpecialChest";
            entry.Type = ShopEntryType.SpecialChest;
            entryArray2[3] = entry;
            entry = new ShopEntry();
            entry.Id = "RewardBoxCommon";
            entry.Type = ShopEntryType.RewardBox;
            entry.ChestType = ChestType.RewardBoxCommon;
            entryArray2[4] = entry;
            entry = new ShopEntry();
            entry.Id = "RewardBoxRare";
            entry.Type = ShopEntryType.RewardBox;
            entry.ChestType = ChestType.RewardBoxRare;
            entryArray2[5] = entry;
            entry = new ShopEntry();
            entry.Id = "RewardBoxEpic";
            entry.Type = ShopEntryType.RewardBox;
            entry.ChestType = ChestType.RewardBoxEpic;
            entryArray2[6] = entry;
            entry = new ShopEntry();
            entry.Id = "RewardBoxMulti";
            entry.Type = ShopEntryType.RewardBox;
            entry.ChestType = ChestType.RewardBoxMulti;
            entryArray2[7] = entry;
            entry = new ShopEntry();
            entry.Id = "CoinBundleXSmall";
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Coin, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_coin_pile1");
            entry.NumBursts = 1;
            entryArray2[8] = entry;
            entry = new ShopEntry();
            entry.Id = "CoinBundleSmall";
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Coin, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_coin_pile2");
            entry.NumBursts = 2;
            entryArray2[9] = entry;
            entry = new ShopEntry();
            entry.Id = "CoinBundleMedium";
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Coin, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_coin_pile3");
            entry.NumBursts = 4;
            entryArray2[10] = entry;
            entry = new ShopEntry();
            entry.Id = "CoinBundleLarge";
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Coin, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_coin_pile4");
            entry.NumBursts = 8;
            entryArray2[11] = entry;
            entry = new ShopEntry();
            entry.Id = "CoinBundleLargeSale";
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Coin, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_coin_pile4");
            entry.NumBursts = 8;
            entryArray2[12] = entry;
            entry = new ShopEntry();
            entry.Id = "CoinBundlePetConversion";
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Coin, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_coin_pile2");
            entry.NumBursts = 1;
            entryArray2[13] = entry;
            entry = new ShopEntry();
            entry.Id = "CoinBundleDungeonBoostBox";
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Coin, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_coin_pile1");
            entry.NumBursts = 1;
            entryArray2[14] = entry;
            entry = new ShopEntry();
            entry.Id = "TokenBundleXSmall";
            entry.Type = ShopEntryType.TokenBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Token, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Token];
            entry.NumBursts = 1;
            entryArray2[15] = entry;
            entry = new ShopEntry();
            entry.Id = "TokenBundleSmall";
            entry.Type = ShopEntryType.TokenBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Token, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Token];
            entry.NumBursts = 4;
            entryArray2[0x10] = entry;
            entry = new ShopEntry();
            entry.Id = "TokenBundleMedium";
            entry.Type = ShopEntryType.TokenBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Token, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Token];
            entry.NumBursts = 6;
            entryArray2[0x11] = entry;
            entry = new ShopEntry();
            entry.Id = "TokenBundleLarge";
            entry.Type = ShopEntryType.TokenBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Token, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Token];
            entry.NumBursts = 8;
            entryArray2[0x12] = entry;
            entry = new ShopEntry();
            entry.Id = "TokenBundleLargeSale";
            entry.Type = ShopEntryType.TokenBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Token, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Token];
            entry.NumBursts = 8;
            entryArray2[0x13] = entry;
            entry = new ShopEntry();
            entry.Id = "TokenBundleDragon";
            entry.Type = ShopEntryType.TokenBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Token, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Token];
            entry.NumBursts = 6;
            entryArray2[20] = entry;
            entry = new ShopEntry();
            entry.Id = "TokenBundleBossHunt";
            entry.Type = ShopEntryType.TokenBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Token, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = ConfigUi.DEFAULT_RESOURCE_PILE_SPRITES[ResourceType.Token];
            entry.NumBursts = 4;
            entryArray2[0x15] = entry;
            entry = new ShopEntry();
            entry.Id = "MysteryItemWeapon";
            entry.Type = ShopEntryType.MysteryItem;
            entry.ItemType = ItemType.Weapon;
            entry.Title = ConfigLoca.ITEMS_WEAPON;
            entry.Sprite = ConfigUi.ITEM_TYPE_ABSTRACT_SPRITES_IDS[ItemType.Weapon];
            entryArray2[0x16] = entry;
            entry = new ShopEntry();
            entry.Id = "MysteryItemArmor";
            entry.Type = ShopEntryType.MysteryItem;
            entry.ItemType = ItemType.Armor;
            entry.Title = ConfigLoca.ITEMS_ARMOR;
            entry.Sprite = ConfigUi.ITEM_TYPE_ABSTRACT_SPRITES_IDS[ItemType.Armor];
            entryArray2[0x17] = entry;
            entry = new ShopEntry();
            entry.Id = "MysteryItemCloak";
            entry.Type = ShopEntryType.MysteryItem;
            entry.ItemType = ItemType.Cloak;
            entry.Title = ConfigLoca.ITEMS_CLOAK;
            entry.Sprite = ConfigUi.ITEM_TYPE_ABSTRACT_SPRITES_IDS[ItemType.Cloak];
            entryArray2[0x18] = entry;
            entry = new ShopEntry();
            entry.Id = "VendorBoostMidas";
            entry.Type = ShopEntryType.Boost;
            entry.Boost = BoostType.Midas;
            entry.Title = "MIDAS";
            entryArray2[0x19] = entry;
            entry = new ShopEntry();
            entry.Id = "VendorBoostShield";
            entry.Type = ShopEntryType.Boost;
            entry.Boost = BoostType.Shield;
            entry.Title = "SHIELD";
            entryArray2[0x1a] = entry;
            entry = new ShopEntry();
            entry.Id = "VendorBoostEnlightment";
            entry.Type = ShopEntryType.Boost;
            entry.Boost = BoostType.Xp;
            entry.Title = "ENLIGHTMENT";
            entryArray2[0x1b] = entry;
            entry = new ShopEntry();
            entry.Id = "VendorBoostFrenzy";
            entry.Type = ShopEntryType.Boost;
            entry.Boost = BoostType.Damage;
            entry.Title = "FRENZY";
            entryArray2[0x1c] = entry;
            entry = new ShopEntry();
            entry.Id = "VendorBoostHaste";
            entry.Type = ShopEntryType.Boost;
            entry.Boost = BoostType.Speed;
            entry.Title = "HASTE";
            entryArray2[0x1d] = entry;
            entry = new ShopEntry();
            entry.Id = "FrenzyBundleSmall";
            entry.Type = ShopEntryType.FrenzyBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_bottle_frenzy");
            entryArray2[30] = entry;
            entry = new ShopEntry();
            entry.Id = "FrenzyBundleMedium";
            entry.Type = ShopEntryType.FrenzyBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_bottle_frenzy");
            entryArray2[0x1f] = entry;
            entry = new ShopEntry();
            entry.Id = "FrenzyBundleLarge";
            entry.Type = ShopEntryType.FrenzyBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_bottle_frenzy");
            entryArray2[0x20] = entry;
            entry = new ShopEntry();
            entry.Id = "DustBundleSmall";
            entry.Type = ShopEntryType.DustBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Dust, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_dust");
            entry.NumBursts = 4;
            entryArray2[0x21] = entry;
            entry = new ShopEntry();
            entry.Id = "DustBundleMedium";
            entry.Type = ShopEntryType.DustBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Dust, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_dust");
            entry.NumBursts = 6;
            entryArray2[0x22] = entry;
            entry = new ShopEntry();
            entry.Id = "DustBundleLarge";
            entry.Type = ShopEntryType.DustBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Dust, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_dust");
            entry.NumBursts = 8;
            entryArray2[0x23] = entry;
            entry = new ShopEntry();
            entry.Id = "DiamondBundleAds";
            entry.Type = ShopEntryType.DiamondBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Diamond, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_gem_pile2");
            entry.NumBursts = 4;
            entryArray2[0x24] = entry;
            entry = new ShopEntry();
            entry.Id = "DiamondBundleBossHunt";
            entry.Type = ShopEntryType.DiamondBundle;
            dictionary = new Dictionary<ResourceType, double>();
            dictionary.Add(ResourceType.Diamond, 0.0);
            entry.BuyResourceAmounts = dictionary;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_gem_pile2");
            entry.NumBursts = 4;
            entryArray2[0x25] = entry;
            entry = new ShopEntry();
            entry.Id = "XpBundleSmall";
            entry.Type = ShopEntryType.XpBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_xp_pile1");
            entryArray2[0x26] = entry;
            entry = new ShopEntry();
            entry.Id = "BossBundleSmall";
            entry.Type = ShopEntryType.BossBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_bossticket");
            entryArray2[0x27] = entry;
            entry = new ShopEntry();
            entry.Id = "PetBundleSmall";
            entry.Type = ShopEntryType.PetBundle;
            entry.Sprite = new SpriteAtlasEntry("DungeonHud", "icon_pets");
            entryArray2[40] = entry;
            entry = new ShopEntry();
            entry.Id = "PetBundleMedium";
            entry.Type = ShopEntryType.PetBundle;
            entry.Sprite = new SpriteAtlasEntry("DungeonHud", "icon_pets");
            entryArray2[0x29] = entry;
            entry = new ShopEntry();
            entry.Id = "PetBundleLarge";
            entry.Type = ShopEntryType.PetBundle;
            entry.Sprite = new SpriteAtlasEntry("DungeonHud", "icon_pets");
            entryArray2[0x2a] = entry;
            entry = new ShopEntry();
            entry.Id = "PetBundleBossHunt";
            entry.Type = ShopEntryType.PetBundle;
            entry.Sprite = new SpriteAtlasEntry("DungeonHud", "icon_pets");
            entryArray2[0x2b] = entry;
            entry = new ShopEntry();
            entry.Id = "PetBoxSmall";
            entry.Type = ShopEntryType.PetBox;
            entry.ChestType = ChestType.PetBoxSmall;
            entryArray2[0x2c] = entry;
            entry = new ShopEntry();
            entry.Id = "MegaBoxBundleSmall";
            entry.Type = ShopEntryType.MegaBoxBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_megabox_pile1");
            entryArray2[0x2d] = entry;
            entry = new ShopEntry();
            entry.Id = "MegaBoxBundleMedium";
            entry.Type = ShopEntryType.MegaBoxBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_megabox_pile2");
            entryArray2[0x2e] = entry;
            entry = new ShopEntry();
            entry.Id = "MegaBoxBundleLarge";
            entry.Type = ShopEntryType.MegaBoxBundle;
            entry.Sprite = new SpriteAtlasEntry("Menu", "icon_megabox_pile3");
            entryArray2[0x2f] = entry;
            entry = new ShopEntry();
            entry.Id = "LootBoxBossHunt";
            entry.Type = ShopEntryType.LootBox;
            entry.ChestType = ChestType.LootBoxBossHunt;
            entryArray2[0x30] = entry;
            VENDOR_SHOP_ENTRIES = entryArray2;
        }

        public static void AddRewardsFromProduct(ref List<Reward> rewards, PremiumProduct product)
        {
            ShopEntry shopEntryByFlareProductId = Service.Binder.ShopManager.GetShopEntryByFlareProductId(product.flareProductId);
            List<ProductReward> rewardsFromProduct = GetRewardsFromProduct(product);
            for (int i = 0; i < rewardsFromProduct.Count; i++)
            {
                ProductReward reward = rewardsFromProduct[i];
                string key = reward.key;
                int amount = reward.amount;
                Reward item = new Reward();
                item.ShopEntryId = product.flareProductId;
                if (key.Equals(ResourceType.Diamond.ToString()))
                {
                    item.Sprite = product.GetRewardIcon(key);
                    if (shopEntryByFlareProductId != null)
                    {
                        MathUtil.DistributeValuesIntoChunksInt(amount, shopEntryByFlareProductId.NumBursts, ref item.DiamondDrops);
                    }
                    else
                    {
                        MathUtil.DistributeValuesIntoChunksInt(amount, 1, ref item.DiamondDrops);
                    }
                }
                else if (key.Equals(ResourceType.Token.ToString()))
                {
                    item.Sprite = product.GetRewardIcon(key);
                    if (shopEntryByFlareProductId != null)
                    {
                        MathUtil.DistributeValuesIntoChunksInt(amount, shopEntryByFlareProductId.NumBursts, ref item.TokenDrops);
                    }
                    else
                    {
                        MathUtil.DistributeValuesIntoChunksInt(amount, 1, ref item.TokenDrops);
                    }
                }
                else if (key.Equals(BuyableItemType.Dragon01.ToString()))
                {
                    item.Sprite = (shopEntryByFlareProductId == null) ? product.GetRewardIcon(key) : shopEntryByFlareProductId.Sprite.SpriteId;
                }
                else if (App.Binder.ConfigMeta.IsActivePetId(key))
                {
                    PetReward reward3 = new PetReward();
                    reward3.PetId = key;
                    reward3.Amount = amount;
                    item.Pets.Add(reward3);
                    item.Sprite = "icon_" + key.ToLower();
                }
                else if (key.Equals(ShopEntryType.MegaBoxBundle.ToString()))
                {
                    item.MegaBoxes = amount;
                    item.Sprite = "icon_megabox_pile1";
                }
                else if (key.Equals(BuyableItemType.FrenzyPotion.ToString()))
                {
                    item.ShopEntryId = null;
                    item.FrenzyPotions = amount;
                    item.Sprite = (product == null) ? "icon_bottle_frenzy" : product.GetRewardIcon(key);
                }
                else if (key.Equals(BuyableItemType.RevivePotion.ToString()))
                {
                    item.ShopEntryId = null;
                    item.Revives = amount;
                    item.Sprite = (product == null) ? "icon_bottle_red" : product.GetRewardIcon(key);
                }
                else
                {
                    Debug.LogError("Unsupported resource id: " + key);
                }
                rewards.Add(item);
            }
        }

        public static double CalculateBossBundleSize(string shopEntryId)
        {
            double d = App.Binder.ConfigMeta.VENDOR_BOSS_BUNDLES[shopEntryId];
            return Math.Floor(d);
        }

        public static double CalculateCoinBundleSize(Player player, string shopEntryId, int amount)
        {
            double coinAmountNotAddedToPlayer = 0.0;
            for (int i = 0; i < amount; i++)
            {
                coinAmountNotAddedToPlayer += App.Binder.ConfigMeta.CoinBundleSize(player, shopEntryId, coinAmountNotAddedToPlayer);
            }
            return coinAmountNotAddedToPlayer;
        }

        public static double CalculateDiamondBundleSize(Player player, string shopEntryId)
        {
            double d = App.Binder.ConfigMeta.VENDOR_DIAMOND_BUNDLES[shopEntryId];
            return Math.Floor(d);
        }

        public static double CalculateDustBundleSize(CharacterInstance character, string shopEntryId)
        {
            double baseAmount = App.Binder.ConfigMeta.VENDOR_DUST_BUNDLES[shopEntryId];
            return Math.Floor(CharacterStatModifierUtil.ApplyDustBonuses(character, baseAmount));
        }

        public static double CalculateFrenzyBundleSize(string shopEntryId)
        {
            double d = App.Binder.ConfigMeta.VENDOR_FRENZY_BUNDLES[shopEntryId];
            return Math.Floor(d);
        }

        public static double CalculateMegaBoxBundleSize(string shopEntryId)
        {
            return App.Binder.ConfigMeta.VENDOR_MEGA_BOX_BUNDLES[shopEntryId];
        }

        public static int CalculatePetBundleSize(Player player, string shopEntryId)
        {
            return App.Binder.ConfigMeta.VENDOR_PET_BUNDLES[shopEntryId].getRandom();
        }

        public static double CalculateReviveBundleSize(string shopEntryId)
        {
            double d = App.Binder.ConfigMeta.VENDOR_REVIVE_BUNDLES[shopEntryId];
            return Math.Floor(d);
        }

        public static double CalculateTokenBundleSize(Player player, string shopEntryId)
        {
            return App.Binder.ConfigMeta.TokenBundleSize(player, shopEntryId);
        }

        public static double CalculateXpBundleSize(string shopEntryId)
        {
            double d = App.Binder.ConfigMeta.VENDOR_XP_BUNDLES[shopEntryId];
            return Math.Floor(d);
        }

        public static ItemInstance CreateNewMysteryItemInstance(Player player, string shopEntryId)
        {
            return ItemInstance.Create(GetRandomMysteryItem(player, shopEntryId), player, -1);
        }

        public static List<string> GetIapProductIds()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < IAP_SHOP_ENTRIES.Length; i++)
            {
                list.Add(IAP_SHOP_ENTRIES[i].Id);
            }
            return list;
        }

        public static ShopEntry GetIapShopEntry(string id)
        {
            for (int i = 0; i < IAP_SHOP_ENTRIES.Length; i++)
            {
                if (IAP_SHOP_ENTRIES[i].Id == id)
                {
                    return IAP_SHOP_ENTRIES[i];
                }
            }
            return null;
        }

        public static Item GetRandomMysteryItem(Player player, string shopEntryId)
        {
            ShopEntry shopEntry = GetShopEntry(shopEntryId);
            int floor = player.getLastCompletedFloor(false) + 1;
            int rarity = player.clampItemRarityToMaxAllowed(UnityEngine.Random.Range(1, 3), floor, false);
            return GameLogic.Binder.ItemResources.getRandomItemOfRarity(rarity, shopEntry.ItemType);
        }

        public static Reward GetRewardFromProductReward(ProductReward productReward, [Optional, DefaultParameterValue(null)] PremiumProduct product)
        {
            Reward reward = new Reward();
            string key = productReward.key;
            int amount = productReward.amount;
            if (key.Equals("Dragon01"))
            {
                key = "Pet001";
                amount = 10;
            }
            if (key.Equals(ResourceType.Diamond.ToString()))
            {
                MathUtil.DistributeValuesIntoChunksInt(amount, 1, ref reward.DiamondDrops);
                reward.Sprite = (product == null) ? "icon_gem_pile2" : product.GetRewardIcon(key);
                return reward;
            }
            if (key.Equals(ResourceType.Token.ToString()))
            {
                MathUtil.DistributeValuesIntoChunksInt(amount, 1, ref reward.TokenDrops);
                reward.Sprite = (product == null) ? "icon_token" : product.GetRewardIcon(key);
                return reward;
            }
            if (App.Binder.ConfigMeta.IsActivePetId(key))
            {
                reward.ChestType = ChestType.NONE;
                List<string> list = new List<string>();
                list.Add("PetBundleSmall");
                reward.ShopEntryDrops = list;
                PetReward item = new PetReward();
                item.PetId = key;
                item.Amount = amount;
                reward.Pets.Add(item);
                reward.Sprite = "icon_" + key.ToLower();
                return reward;
            }
            if (key.Equals(ShopEntryType.MegaBoxBundle.ToString()))
            {
                reward.MegaBoxes = amount;
                reward.Sprite = "icon_megabox_pile1";
                return reward;
            }
            if (key.Equals(BuyableItemType.FrenzyPotion.ToString()))
            {
                reward.FrenzyPotions = amount;
                reward.Sprite = (product == null) ? "icon_bottle_frenzy" : product.GetRewardIcon(key);
                return reward;
            }
            if (key.Equals(BuyableItemType.RevivePotion.ToString()))
            {
                reward.Revives = amount;
                reward.Sprite = (product == null) ? "icon_bottle_red" : product.GetRewardIcon(key);
                return reward;
            }
            Debug.LogError("GetRewardFromProductReward::Unsupported resource id: " + key);
            return reward;
        }

        public static List<ProductReward> GetRewardsFromProduct(PremiumProduct prod)
        {
            if (prod.flareProductId == IAP_STARTER_BUNDLE_ID)
            {
                Player player = GameLogic.Binder.GameState.Player;
                List<ProductReward> list = new List<ProductReward>();
                ProductReward item = new ProductReward();
                item.key = ResourceType.Diamond.ToString();
                item.amount = (int) App.Binder.ConfigMeta.STARTER_BUNDLE_DIAMOND_COUNT;
                list.Add(item);
                item = new ProductReward();
                item.key = ResourceType.Token.ToString();
                item.amount = (int) App.Binder.ConfigMeta.TokenBundleSize(player, "TokenBundleDragon");
                list.Add(item);
                item = new ProductReward();
                item.key = BuyableItemType.Dragon01.ToString();
                item.amount = 1;
                list.Add(item);
                return list;
            }
            return prod.rewards;
        }

        public static ShopEntry GetShopEntry(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            ShopEntry vendorShopEntry = GetVendorShopEntry(id);
            if (vendorShopEntry != null)
            {
                return vendorShopEntry;
            }
            return GetIapShopEntry(id);
        }

        public static ShopEntry GetShopEntryForIapProductId(string productId)
        {
            for (int i = 0; i < IAP_SHOP_ENTRIES.Length; i++)
            {
                if (IAP_SHOP_ENTRIES[i].Id == productId)
                {
                    return IAP_SHOP_ENTRIES[i];
                }
            }
            return null;
        }

        public static ShopEntry GetVendorShopEntry(string id)
        {
            for (int i = 0; i < VENDOR_SHOP_ENTRIES.Length; i++)
            {
                if (VENDOR_SHOP_ENTRIES[i].Id == id)
                {
                    return VENDOR_SHOP_ENTRIES[i];
                }
            }
            return null;
        }

        public static List<Reward> InstantiateRewardsFromServerCommands(List<InboxCommand> eventList)
        {
            List<Reward> list = new List<Reward>();
            foreach (InboxCommand command in eventList)
            {
                if (command.CommandId == InboxCommandIdType.RewardShopProduct)
                {
                    ProductRewardCollection rewards = null;
                    try
                    {
                        rewards = JsonUtils.Deserialize<ProductRewardCollection>(command.Parameters["RewardCollection"].ToString(), true);
                    }
                    catch (Exception exception)
                    {
                        Debug.Log("Could not parse premium resource from InboxCommand: " + exception.ToString());
                        return list;
                    }
                    PremiumProduct product = Service.Binder.ShopService.GetProduct(rewards.Pid);
                    if (product == null)
                    {
                        return list;
                    }
                    AddRewardsFromProduct(ref list, product);
                }
            }
            return list;
        }

        public static bool IsIapShopEntry(ShopEntry entry)
        {
            return ((entry.Type == ShopEntryType.IapDiamonds) || (entry.Type == ShopEntryType.IapStarterBundle));
        }

        public static bool IsVendorShopEntry(string id)
        {
            for (int i = 0; i < VENDOR_SHOP_ENTRIES.Length; i++)
            {
                if (VENDOR_SHOP_ENTRIES[i].Id == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

