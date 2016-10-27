namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class RewardParams : CustomParams
    {
        [CompilerGenerated]
        private List<ProductReward> <Costs>k__BackingField;
        [CompilerGenerated]
        private List<ProductReward> <Rewards>k__BackingField;
        private const string PARAM_COST = "cost";
        private const string PARAM_REWARD = "reward";

        public RewardParams(Dictionary<string, object> promoParams) : base(promoParams)
        {
            if (promoParams != null)
            {
                this.InitResources(promoParams);
            }
        }

        private void InitResources(Dictionary<string, object> promoParams)
        {
            this.Costs = new List<ProductReward>();
            this.Rewards = new List<ProductReward>();
            foreach (string str in promoParams.Keys)
            {
                char[] separator = new char[] { '-' };
                string[] strArray = str.Split(separator);
                if (strArray.Length >= 2)
                {
                    ProductReward reward;
                    if (string.Equals(strArray[0], "reward"))
                    {
                        reward = new ProductReward();
                        reward.key = strArray[1];
                        reward.amount = int.Parse(promoParams[str] as string);
                        this.Rewards.Add(reward);
                    }
                    else if (string.Equals(strArray[0], "cost"))
                    {
                        reward = new ProductReward();
                        reward.key = strArray[1];
                        reward.amount = int.Parse(promoParams[str] as string);
                        this.Costs.Add(reward);
                    }
                }
            }
        }

        public override bool Validate()
        {
            bool flag = this.Rewards.Count > 0;
            if (!flag)
            {
                Debug.LogWarning("Promotion Validation failed: at least one Reward must be set");
            }
            return (flag && base.Validate());
        }

        public List<ProductReward> Costs
        {
            [CompilerGenerated]
            get
            {
                return this.<Costs>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Costs>k__BackingField = value;
            }
        }

        public List<ProductReward> Rewards
        {
            [CompilerGenerated]
            get
            {
                return this.<Rewards>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Rewards>k__BackingField = value;
            }
        }
    }
}

