namespace GameLogic
{
    using System;
    using System.Collections.Generic;

    public class ServerStats
    {
        public int AdPostbacksReceived;
        public Dictionary<string, string> AssignedABTestGroups = new Dictionary<string, string>();
        public bool Banned = false;
        public bool Cheater = false;
        public int CheatReason;
        public long CheatStateChanged;
        public string Country;
        public long FirstLogin;
        public long LastLogin;
        public long LastPostback;
        public long LastPurchase;
        public int LoginCount;
        public int LogLevel = -1;
        public bool NonCheater = false;
        public float PurchasedAmount;
        public int PurchasedPremium;
        public string SessionSource;
        public int StaticBucketId;
        public int TotalPurchases;
        public long UpdateStamp;
        public string UserHandle;

        public bool IsCheater
        {
            get
            {
                return (this.Cheater && !this.NonCheater);
            }
        }
    }
}

