namespace Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IPlatformShopConnector
    {
        Purchase CreatePurchase(PremiumProduct product);
        List<Purchase> GetPending();
        IEnumerator Initialize(string _refProduct);
        bool MergeProductData(PremiumProduct product);
        IEnumerator RequestPlatformProducts(string[] prodIds);
        IEnumerator ResumePending(Purchase purchase);
        void UnInitialize();

        bool Available { get; }

        string CurrencyCode { get; }

        int PendingCount { get; }

        ShopManagerState State { get; }
    }
}

