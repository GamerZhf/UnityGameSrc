namespace Prime31
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GooglePurchase
    {
        [CompilerGenerated]
        private string <developerPayload>k__BackingField;
        [CompilerGenerated]
        private string <orderId>k__BackingField;
        [CompilerGenerated]
        private string <originalJson>k__BackingField;
        [CompilerGenerated]
        private string <packageName>k__BackingField;
        [CompilerGenerated]
        private string <productId>k__BackingField;
        [CompilerGenerated]
        private GooglePurchaseState <purchaseState>k__BackingField;
        [CompilerGenerated]
        private long <purchaseTime>k__BackingField;
        [CompilerGenerated]
        private string <purchaseToken>k__BackingField;
        [CompilerGenerated]
        private string <signature>k__BackingField;
        [CompilerGenerated]
        private string <type>k__BackingField;

        public GooglePurchase(Dictionary<string, object> dict)
        {
            if (dict.ContainsKey("packageName"))
            {
                this.packageName = dict["packageName"].ToString();
            }
            if (dict.ContainsKey("orderId"))
            {
                this.orderId = dict["orderId"].ToString();
            }
            if (dict.ContainsKey("productId"))
            {
                this.productId = dict["productId"].ToString();
            }
            if (dict.ContainsKey("developerPayload"))
            {
                this.developerPayload = dict["developerPayload"].ToString();
            }
            if (dict.ContainsKey("type"))
            {
                this.type = dict["type"] as string;
            }
            if (dict.ContainsKey("purchaseTime"))
            {
                this.purchaseTime = long.Parse(dict["purchaseTime"].ToString());
            }
            if (dict.ContainsKey("purchaseState"))
            {
                this.purchaseState = (GooglePurchaseState) int.Parse(dict["purchaseState"].ToString());
            }
            if (dict.ContainsKey("purchaseToken"))
            {
                this.purchaseToken = dict["purchaseToken"].ToString();
            }
            if (dict.ContainsKey("signature"))
            {
                this.signature = dict["signature"].ToString();
            }
            if (dict.ContainsKey("originalJson"))
            {
                this.originalJson = dict["originalJson"].ToString();
            }
        }

        public static List<GooglePurchase> fromList(List<object> items)
        {
            List<GooglePurchase> list = new List<GooglePurchase>();
            foreach (Dictionary<string, object> dictionary in items)
            {
                list.Add(new GooglePurchase(dictionary));
            }
            return list;
        }

        public override string ToString()
        {
            object[] args = new object[] { this.packageName, this.orderId, this.productId, this.developerPayload, this.purchaseToken, this.purchaseState, this.signature, this.type, this.originalJson };
            return string.Format("<GooglePurchase> packageName: {0}, orderId: {1}, productId: {2}, developerPayload: {3}, purchaseToken: {4}, purchaseState: {5}, signature: {6}, type: {7}, json: {8}", args);
        }

        public string developerPayload
        {
            [CompilerGenerated]
            get
            {
                return this.<developerPayload>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<developerPayload>k__BackingField = value;
            }
        }

        public string orderId
        {
            [CompilerGenerated]
            get
            {
                return this.<orderId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<orderId>k__BackingField = value;
            }
        }

        public string originalJson
        {
            [CompilerGenerated]
            get
            {
                return this.<originalJson>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<originalJson>k__BackingField = value;
            }
        }

        public string packageName
        {
            [CompilerGenerated]
            get
            {
                return this.<packageName>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<packageName>k__BackingField = value;
            }
        }

        public string productId
        {
            [CompilerGenerated]
            get
            {
                return this.<productId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<productId>k__BackingField = value;
            }
        }

        public GooglePurchaseState purchaseState
        {
            [CompilerGenerated]
            get
            {
                return this.<purchaseState>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<purchaseState>k__BackingField = value;
            }
        }

        public long purchaseTime
        {
            [CompilerGenerated]
            get
            {
                return this.<purchaseTime>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<purchaseTime>k__BackingField = value;
            }
        }

        public string purchaseToken
        {
            [CompilerGenerated]
            get
            {
                return this.<purchaseToken>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<purchaseToken>k__BackingField = value;
            }
        }

        public string signature
        {
            [CompilerGenerated]
            get
            {
                return this.<signature>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<signature>k__BackingField = value;
            }
        }

        public string type
        {
            [CompilerGenerated]
            get
            {
                return this.<type>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<type>k__BackingField = value;
            }
        }

        public enum GooglePurchaseState
        {
            Purchased,
            Canceled,
            Refunded
        }
    }
}

