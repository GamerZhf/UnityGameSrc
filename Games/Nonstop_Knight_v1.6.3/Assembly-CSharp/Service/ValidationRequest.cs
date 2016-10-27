namespace Service
{
    using System;
    using System.Collections.Generic;

    public class ValidationRequest
    {
        public string gameUserId;
        public string isoCurrencyCode;
        public string receipt;
        public string signature;
        public string storeId;
        public Dictionary<string, string> trackingPayload;
        public string trackingUserId;
    }
}

