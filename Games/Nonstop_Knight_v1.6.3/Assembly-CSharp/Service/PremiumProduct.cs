namespace Service
{
    using System;
    using System.Collections.Generic;

    public class PremiumProduct : DisplayInfo
    {
        public string currencyCode;
        public string flareProductId;
        public string formattedPrice;
        public float price;
        public string productDescription;
        public string productName;
        public List<ProductReward> rewards;
        public string storeProductId;
        public float trackingAmount;
        public string trackingIsoCurrencyCode;

        public int CountRewards(string _key)
        {
            int num = 0;
            foreach (ProductReward reward in this.rewards)
            {
                num += !(reward.key == _key) ? 0 : reward.amount;
            }
            return num;
        }
    }
}

