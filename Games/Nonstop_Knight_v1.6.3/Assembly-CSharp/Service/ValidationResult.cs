namespace Service
{
    using System;
    using System.Collections.Generic;

    public class ValidationResult
    {
        public string flareProductId;
        public ICollection<ProductReward> rewards;
        public string storeProductId;
        public string transactionId;
        public ValidationResultCode validity;
    }
}

