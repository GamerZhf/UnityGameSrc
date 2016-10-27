namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class RemotePromotion
    {
        [CompilerGenerated]
        private CustomParams <ParsedCustomParams>k__BackingField;
        [CompilerGenerated]
        private PromoLoca <ParsedLoca>k__BackingField;
        [CompilerGenerated]
        private EPromotionType <PromotionType>k__BackingField;
        [CompilerGenerated]
        private PromotionState <State>k__BackingField;
        public string abTestTrackingParameter;
        public Dictionary<string, object> customParams;
        public bool hasPopup;
        public Dictionary<string, object> localized;
        private const string PARAM_TYPE = "promotion-type";
        private const string PROMO_TYPE_DEEPLINK = "deeplink";
        private const string PROMO_TYPE_EVENT = "event";
        private const string PROMO_TYPE_IAP = "iap-placement";
        private const string PROMO_TYPE_INFO = "info";
        private const string PROMO_TYPE_REWARD = "reward";
        private const string PROMO_TYPE_TOURNAMENT = "tournament";
        public List<PromotionPremiumProduct> promotedPremiumProducts;
        public ActionType promotionActionType;
        public string promotionid;
        public PromotionTiming timing;

        public static string FixPromoSlot(string input)
        {
            return input.Replace('_', '.');
        }

        public bool HasTriggers()
        {
            return (this.ParsedCustomParams.Triggers.Count != 0);
        }

        public void InitAfterDataAvailable()
        {
            this.InitPromotionType(this.customParams);
            this.ParsedLoca = new PromoLoca(this.localized);
            switch (this.PromotionType)
            {
                case EPromotionType.IapPlacement:
                    this.ParsedCustomParams = new IapPlacementParams(this.customParams);
                    return;

                case EPromotionType.Reward:
                    this.ParsedCustomParams = new RewardParams(this.customParams);
                    return;

                case EPromotionType.Deeplink:
                    this.ParsedCustomParams = new DeeplinkParams(this.customParams);
                    return;

                case EPromotionType.Event:
                    this.ParsedCustomParams = new EventParams(this.customParams);
                    return;

                case EPromotionType.Tournament:
                    this.ParsedCustomParams = new TournamentParams(this.customParams);
                    return;
            }
            this.ParsedCustomParams = new CustomParams(this.customParams);
        }

        private void InitPromotionType(Dictionary<string, object> promoParams)
        {
            if ((promoParams != null) && promoParams.ContainsKey("promotion-type"))
            {
                string str = promoParams["promotion-type"].ToString();
                promoParams.Remove("promotion-type");
                if (str.Equals("iap-placement"))
                {
                    this.PromotionType = EPromotionType.IapPlacement;
                }
                else if (str.Equals("reward"))
                {
                    this.PromotionType = EPromotionType.Reward;
                }
                else if (str.Equals("deeplink"))
                {
                    this.PromotionType = EPromotionType.Deeplink;
                }
                else if (str.Equals("info"))
                {
                    this.PromotionType = EPromotionType.Info;
                }
                else if (str.Equals("event"))
                {
                    this.PromotionType = EPromotionType.Event;
                }
                else if (str.Equals("tournament"))
                {
                    this.PromotionType = EPromotionType.Tournament;
                }
            }
        }

        public bool IsValid()
        {
            if ((this.PromotionType == EPromotionType.Tournament) && ((this.ParsedCustomParams as TournamentParams).TournamentId != null))
            {
                return true;
            }
            bool allowsNoButton = ((this.PromotionType == EPromotionType.IapPlacement) || (this.PromotionType == EPromotionType.Info)) || (this.PromotionType == EPromotionType.Event);
            return ((((this.ParsedCustomParams != null) && this.ParsedCustomParams.Validate()) && (this.ParsedLoca != null)) && this.ParsedLoca.ValidatePopupLoca(allowsNoButton));
        }

        public void ProcessTrigger(string trigger)
        {
            List<string> triggers = this.ParsedCustomParams.Triggers;
            for (int i = 0; i < triggers.Count; i++)
            {
                if (trigger.Equals(triggers[i]))
                {
                    triggers.RemoveAt(i--);
                }
            }
        }

        public CustomParams ParsedCustomParams
        {
            [CompilerGenerated]
            get
            {
                return this.<ParsedCustomParams>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ParsedCustomParams>k__BackingField = value;
            }
        }

        public PromoLoca ParsedLoca
        {
            [CompilerGenerated]
            get
            {
                return this.<ParsedLoca>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ParsedLoca>k__BackingField = value;
            }
        }

        public EPromotionType PromotionType
        {
            [CompilerGenerated]
            get
            {
                return this.<PromotionType>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PromotionType>k__BackingField = value;
            }
        }

        public PromotionState State
        {
            [CompilerGenerated]
            get
            {
                return this.<State>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<State>k__BackingField = value;
            }
        }
    }
}

