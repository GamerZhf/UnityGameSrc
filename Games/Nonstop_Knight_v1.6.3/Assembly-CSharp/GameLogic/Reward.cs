namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Reward : IJsonData
    {
        public BoostType Boost;
        public int BossPotions;
        public GameLogic.ChestType ChestType;
        public GameLogic.ChestType ChestTypeVisualOverride;
        public List<double> CoinDrops;
        public List<double> CrownDrops;
        public List<double> DiamondDrops;
        public List<double> DustDrops;
        public int FrenzyPotions;
        public List<ItemInstance> ItemDrops;
        public int MegaBoxes;
        public List<PetReward> Pets;
        public int Revives;
        public RewardSourceType RewardSource;
        public string RewardSourceId;
        public List<int> RunestoneAtIndexConvertedIntoTokens;
        public List<string> RunestoneDrops;
        public List<string> ShopEntryDrops;
        public string ShopEntryId;
        public SkillType Skill;
        public string Sprite;
        public List<double> TokenDrops;
        public GameLogic.TournamentUpgradeReward TournamentUpgradeReward;
        public int XpPotions;

        public Reward()
        {
            this.CoinDrops = new List<double>();
            this.DiamondDrops = new List<double>();
            this.TokenDrops = new List<double>();
            this.ItemDrops = new List<ItemInstance>();
            this.RunestoneDrops = new List<string>();
            this.CrownDrops = new List<double>();
            this.DustDrops = new List<double>();
            this.ShopEntryDrops = new List<string>();
            this.Pets = new List<PetReward>();
        }

        public Reward(Reward another)
        {
            this.CoinDrops = new List<double>();
            this.DiamondDrops = new List<double>();
            this.TokenDrops = new List<double>();
            this.ItemDrops = new List<ItemInstance>();
            this.RunestoneDrops = new List<string>();
            this.CrownDrops = new List<double>();
            this.DustDrops = new List<double>();
            this.ShopEntryDrops = new List<string>();
            this.Pets = new List<PetReward>();
            this.copyFrom(another);
        }

        public void addResourceDrop(ResourceType resourceType, double amount)
        {
            switch (resourceType)
            {
                case ResourceType.Coin:
                    this.CoinDrops.Add(amount);
                    return;

                case ResourceType.Diamond:
                    this.DiamondDrops.Add(amount);
                    return;

                case ResourceType.Token:
                    this.TokenDrops.Add(amount);
                    return;

                case ResourceType.Crown:
                    this.CrownDrops.Add(amount);
                    return;

                case ResourceType.Dust:
                    this.DustDrops.Add(amount);
                    return;
            }
            Debug.LogError("Unsupported resource type: " + resourceType);
        }

        public void addShopEntryDrop(Player player, string shopEntryId, [Optional, DefaultParameterValue(false)] bool applyMysteryChestDiminisher)
        {
            double num;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            ShopEntry shopEntry = ConfigShops.GetShopEntry(shopEntryId);
            this.ShopEntryDrops.Add(shopEntryId);
            if (string.IsNullOrEmpty(this.ShopEntryId))
            {
                this.ShopEntryId = shopEntryId;
            }
            switch (shopEntry.Type)
            {
                case ShopEntryType.CoinBundle:
                {
                    if (!applyMysteryChestDiminisher)
                    {
                        num = ConfigShops.CalculateCoinBundleSize(player, shopEntry.Id, 1);
                        break;
                    }
                    ConfigMeta configMeta = App.Binder.ConfigMeta;
                    double numUpgrades = player.getMysteryCoinOfferMultiplier(shopEntryId);
                    num = configMeta.CoinBundleSize(activeCharacter.HighestLevelItemOwnedAtFloorStart, player, shopEntryId, numUpgrades, 0.0);
                    break;
                }
                case ShopEntryType.IapDiamonds:
                case ShopEntryType.IapStarterBundle:
                case ShopEntryType.SpecialChest:
                case ShopEntryType.RewardBox:
                    return;

                case ShopEntryType.Boost:
                    this.Boost = shopEntry.Boost;
                    return;

                case ShopEntryType.MysteryItem:
                {
                    ItemInstance item = ConfigShops.CreateNewMysteryItemInstance(player, shopEntry.Id);
                    this.ItemDrops.Add(item);
                    return;
                }
                case ShopEntryType.TokenBundle:
                    MathUtil.DistributeValuesIntoChunksInt((int) ConfigShops.CalculateTokenBundleSize(player, shopEntry.Id), shopEntry.NumBursts, ref this.TokenDrops);
                    return;

                case ShopEntryType.ReviveBundle:
                    this.Revives = (int) ConfigShops.CalculateReviveBundleSize(shopEntry.Id);
                    return;

                case ShopEntryType.FrenzyBundle:
                    this.FrenzyPotions = (int) ConfigShops.CalculateFrenzyBundleSize(shopEntry.Id);
                    return;

                case ShopEntryType.DustBundle:
                    MathUtil.DistributeValuesIntoChunksInt((int) ConfigShops.CalculateDustBundleSize(activeCharacter, shopEntry.Id), shopEntry.NumBursts, ref this.DustDrops);
                    return;

                case ShopEntryType.DiamondBundle:
                    MathUtil.DistributeValuesIntoChunksInt((int) ConfigShops.CalculateDiamondBundleSize(player, shopEntry.Id), shopEntry.NumBursts, ref this.DiamondDrops);
                    return;

                case ShopEntryType.XpBundle:
                    this.XpPotions = (int) ConfigShops.CalculateXpBundleSize(shopEntry.Id);
                    return;

                case ShopEntryType.BossBundle:
                    this.BossPotions = (int) ConfigShops.CalculateBossBundleSize(shopEntry.Id);
                    return;

                case ShopEntryType.PetBundle:
                {
                    string str = CmdRollPetBundleLootTable.ExecuteStatic(App.Binder.ConfigLootTables.PetBundleLootTables[shopEntry.Id], player);
                    int num6 = ConfigShops.CalculatePetBundleSize(player, shopEntry.Id);
                    PetReward reward3 = new PetReward();
                    reward3.PetId = str;
                    reward3.Amount = num6;
                    this.Pets.Add(reward3);
                    return;
                }
                case ShopEntryType.PetBox:
                {
                    this.ChestType = shopEntry.ChestType;
                    Reward reward = new Reward();
                    CmdRollChestLootTable.ExecuteStatic(shopEntry.ChestType, player, false, ref reward, null);
                    this.Pets.AddRange(reward.Pets);
                    return;
                }
                case ShopEntryType.MegaBoxBundle:
                    this.MegaBoxes = (int) ConfigShops.CalculateMegaBoxBundleSize(shopEntry.Id);
                    return;

                case ShopEntryType.LootBox:
                {
                    Reward reward2 = new Reward();
                    CmdRollChestLootTable.ExecuteStatic(shopEntry.ChestType, player, false, ref reward2, null);
                    this.copyFrom(reward2);
                    this.ChestType = shopEntry.ChestType;
                    return;
                }
                default:
                    return;
            }
            MathUtil.DistributeValuesIntoChunksDouble(num, shopEntry.NumBursts, ref this.CoinDrops);
        }

        public void clearContent()
        {
            this.CoinDrops.Clear();
            this.DiamondDrops.Clear();
            this.TokenDrops.Clear();
            this.Boost = BoostType.UNSPECIFIED;
            this.Skill = SkillType.NONE;
            this.ItemDrops.Clear();
            this.RunestoneDrops.Clear();
            this.CrownDrops.Clear();
            this.Revives = 0;
            this.RunestoneAtIndexConvertedIntoTokens = null;
            this.ShopEntryId = null;
            this.Sprite = null;
            this.FrenzyPotions = 0;
            this.DustDrops.Clear();
            this.ShopEntryDrops.Clear();
            this.XpPotions = 0;
            this.BossPotions = 0;
            this.Pets.Clear();
            this.MegaBoxes = 0;
            this.TournamentUpgradeReward = null;
        }

        public void copyFrom(Reward another)
        {
            this.ChestType = another.ChestType;
            this.CoinDrops = new List<double>(another.CoinDrops);
            this.DiamondDrops = new List<double>(another.DiamondDrops);
            this.TokenDrops = new List<double>(another.TokenDrops);
            this.Boost = another.Boost;
            this.Skill = another.Skill;
            this.ItemDrops = new List<ItemInstance>(another.ItemDrops);
            this.RunestoneDrops = new List<string>(another.RunestoneDrops);
            this.CrownDrops = new List<double>(another.CrownDrops);
            if (another.RunestoneAtIndexConvertedIntoTokens != null)
            {
                this.RunestoneAtIndexConvertedIntoTokens = new List<int>(another.RunestoneAtIndexConvertedIntoTokens);
            }
            this.ShopEntryId = another.ShopEntryId;
            this.Revives = another.Revives;
            this.Sprite = another.Sprite;
            this.FrenzyPotions = another.FrenzyPotions;
            this.DustDrops = new List<double>(another.DustDrops);
            this.ShopEntryDrops = new List<string>(another.ShopEntryDrops);
            this.XpPotions = another.XpPotions;
            this.BossPotions = another.BossPotions;
            this.Pets = new List<PetReward>(another.Pets);
            this.MegaBoxes = another.MegaBoxes;
            this.RewardSource = another.RewardSource;
            this.RewardSourceId = another.RewardSourceId;
            this.ChestTypeVisualOverride = another.ChestTypeVisualOverride;
            this.TournamentUpgradeReward = another.TournamentUpgradeReward;
        }

        public double getTotalCoinAmount()
        {
            double num = 0.0;
            for (int i = 0; i < this.CoinDrops.Count; i++)
            {
                num += this.CoinDrops[i];
            }
            return num;
        }

        public double getTotalCrownAmount()
        {
            double num = 0.0;
            for (int i = 0; i < this.CrownDrops.Count; i++)
            {
                num += this.CrownDrops[i];
            }
            return num;
        }

        public double getTotalDiamondAmount()
        {
            double num = 0.0;
            for (int i = 0; i < this.DiamondDrops.Count; i++)
            {
                num += this.DiamondDrops[i];
            }
            return num;
        }

        public double getTotalDustAmount()
        {
            double num = 0.0;
            for (int i = 0; i < this.DustDrops.Count; i++)
            {
                num += this.DustDrops[i];
            }
            return num;
        }

        public int getTotalPetAmount(string petId)
        {
            int num = 0;
            for (int i = 0; i < this.Pets.Count; i++)
            {
                if (this.Pets[i].PetId == petId)
                {
                    num += this.Pets[i].Amount;
                }
            }
            return num;
        }

        public double getTotalTokenAmount()
        {
            double num = 0.0;
            for (int i = 0; i < this.TokenDrops.Count; i++)
            {
                num += this.TokenDrops[i];
            }
            return num;
        }

        public GameLogic.ChestType getVisualChestType()
        {
            return ((this.ChestTypeVisualOverride == GameLogic.ChestType.NONE) ? this.ChestType : this.ChestTypeVisualOverride);
        }

        public bool isEmpty()
        {
            return ((((((this.CoinDrops.Count == 0) && (this.DiamondDrops.Count == 0)) && ((this.TokenDrops.Count == 0) && (this.Boost == BoostType.UNSPECIFIED))) && (((this.Skill == SkillType.NONE) && (this.ItemDrops.Count == 0)) && ((this.RunestoneDrops.Count == 0) && (this.CrownDrops.Count == 0)))) && ((((this.Revives == 0) && (this.FrenzyPotions == 0)) && ((this.DustDrops.Count == 0) && (this.ShopEntryDrops.Count == 0))) && (((this.XpPotions == 0) && (this.BossPotions == 0)) && ((this.Pets.Count == 0) && (this.MegaBoxes == 0))))) && (this.TournamentUpgradeReward == null));
        }

        public bool isRunestoneAtIndexConvertedIntoTokens(int idx)
        {
            if (this.RunestoneAtIndexConvertedIntoTokens != null)
            {
                for (int i = 0; i < this.RunestoneAtIndexConvertedIntoTokens.Count; i++)
                {
                    if (this.RunestoneAtIndexConvertedIntoTokens[i] == idx)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isSocialGift()
        {
            return (this.RewardSource == RewardSourceType.FacebookFriend);
        }

        public bool isWrappedInsideChest()
        {
            return (this.ChestType != GameLogic.ChestType.NONE);
        }

        public void postDeserializeInitialization()
        {
            for (int i = 0; i < this.ItemDrops.Count; i++)
            {
                this.ItemDrops[i].postDeserializeInitialization();
            }
        }
    }
}

