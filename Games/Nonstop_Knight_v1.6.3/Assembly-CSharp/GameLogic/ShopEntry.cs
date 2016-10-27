namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShopEntry : IComparable<ShopEntry>
    {
        public BoostType Boost;
        public Dictionary<ResourceType, double> BuyResourceAmounts;
        public GameLogic.ChestType ChestType;
        public double CostAmount;
        public ResourceType? CostResource;
        public string FormattedPrice;
        public string Id;
        public GameLogic.ItemType ItemType;
        public int MinFloor;
        public int NumBursts;
        public bool PurchaseDisabled;
        public int Rarity;
        public SpriteAtlasEntry Sprite;
        public string Title;
        public ShopEntryType Type;

        public ShopEntry()
        {
            this.Title = string.Empty;
            this.NumBursts = 1;
            this.MinFloor = 1;
            this.FormattedPrice = string.Empty;
            this.BuyResourceAmounts = new Dictionary<ResourceType, double>();
        }

        public ShopEntry(ShopEntry another)
        {
            this.Title = string.Empty;
            this.NumBursts = 1;
            this.MinFloor = 1;
            this.FormattedPrice = string.Empty;
            this.Id = another.Id;
            this.Type = another.Type;
            this.BuyResourceAmounts = another.BuyResourceAmounts;
            this.CostAmount = another.CostAmount;
            this.CostResource = another.CostResource;
            this.Title = another.Title;
            this.Boost = another.Boost;
            this.ItemType = another.ItemType;
            this.Sprite = another.Sprite;
            this.NumBursts = another.NumBursts;
            this.MinFloor = another.MinFloor;
            this.FormattedPrice = another.FormattedPrice;
        }

        public int CompareTo(ShopEntry other)
        {
            if (this.CostAmount < other.CostAmount)
            {
                return -1;
            }
            if (this.CostAmount > other.CostAmount)
            {
                return 1;
            }
            return this.Id.GetHashCode().CompareTo(other.Id.GetHashCode());
        }

        internal void initBuyResourceAmounts(PremiumProduct prod)
        {
            this.BuyResourceAmounts.Clear();
            foreach (ProductReward reward in ConfigShops.GetRewardsFromProduct(prod))
            {
                try
                {
                    ResourceType key = (ResourceType) ((int) Enum.Parse(typeof(ResourceType), reward.key));
                    this.BuyResourceAmounts.Add(key, (double) reward.amount);
                }
                catch (Exception)
                {
                    Debug.Log("ShopEntry - Can't parse shop product reward. " + reward.key + " is not a ResourceType (Which is fine for now).");
                }
            }
        }
    }
}

