namespace Prime31
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GoogleSkuInfo
    {
        [CompilerGenerated]
        private string <description>k__BackingField;
        [CompilerGenerated]
        private string <price>k__BackingField;
        [CompilerGenerated]
        private long <priceAmountMicros>k__BackingField;
        [CompilerGenerated]
        private string <priceCurrencyCode>k__BackingField;
        [CompilerGenerated]
        private string <productId>k__BackingField;
        [CompilerGenerated]
        private string <title>k__BackingField;
        [CompilerGenerated]
        private string <type>k__BackingField;

        public GoogleSkuInfo(Dictionary<string, object> dict)
        {
            if (dict.ContainsKey("title"))
            {
                this.title = dict["title"] as string;
            }
            if (dict.ContainsKey("price"))
            {
                this.price = dict["price"] as string;
            }
            if (dict.ContainsKey("type"))
            {
                this.type = dict["type"] as string;
            }
            if (dict.ContainsKey("description"))
            {
                this.description = dict["description"] as string;
            }
            if (dict.ContainsKey("productId"))
            {
                this.productId = dict["productId"] as string;
            }
            if (dict.ContainsKey("price_currency_code"))
            {
                this.priceCurrencyCode = dict["price_currency_code"] as string;
            }
            if (dict.ContainsKey("price_amount_micros"))
            {
                long? nullable = dict["price_amount_micros"] as long?;
                if (nullable.HasValue)
                {
                    this.priceAmountMicros = nullable.Value;
                }
            }
        }

        public static List<GoogleSkuInfo> fromList(List<object> items)
        {
            List<GoogleSkuInfo> list = new List<GoogleSkuInfo>();
            foreach (Dictionary<string, object> dictionary in items)
            {
                list.Add(new GoogleSkuInfo(dictionary));
            }
            return list;
        }

        public override string ToString()
        {
            object[] args = new object[] { this.title, this.price, this.type, this.description, this.productId, this.priceCurrencyCode };
            return string.Format("<GoogleSkuInfo> title: {0}, price: {1}, type: {2}, description: {3}, productId: {4}, priceCurrencyCode: {5}", args);
        }

        public string description
        {
            [CompilerGenerated]
            get
            {
                return this.<description>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<description>k__BackingField = value;
            }
        }

        public string price
        {
            [CompilerGenerated]
            get
            {
                return this.<price>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<price>k__BackingField = value;
            }
        }

        public long priceAmountMicros
        {
            [CompilerGenerated]
            get
            {
                return this.<priceAmountMicros>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<priceAmountMicros>k__BackingField = value;
            }
        }

        public string priceCurrencyCode
        {
            [CompilerGenerated]
            get
            {
                return this.<priceCurrencyCode>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<priceCurrencyCode>k__BackingField = value;
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

        public string title
        {
            [CompilerGenerated]
            get
            {
                return this.<title>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<title>k__BackingField = value;
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
    }
}

