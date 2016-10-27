namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PremiumShopContainer
    {
        [CompilerGenerated]
        private Dictionary<string, string> <placement>k__BackingField;
        [CompilerGenerated]
        private List<PremiumProduct> <products>k__BackingField;

        public Dictionary<string, string> placement
        {
            [CompilerGenerated]
            get
            {
                return this.<placement>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<placement>k__BackingField = value;
            }
        }

        public List<PremiumProduct> products
        {
            [CompilerGenerated]
            get
            {
                return this.<products>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<products>k__BackingField = value;
            }
        }
    }
}

