namespace Prime31
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GoogleIABManager : AbstractManager
    {
        public static  event Action<string> billingNotSupportedEvent;

        public static  event Action billingSupportedEvent;

        public static  event Action<string> consumePurchaseFailedEvent;

        public static  event Action<GooglePurchase> consumePurchaseSucceededEvent;

        public static  event Action<string, string> purchaseCompleteAwaitingVerificationEvent;

        public static  event Action<string, int> purchaseFailedEvent;

        public static  event Action<GooglePurchase> purchaseSucceededEvent;

        public static  event Action<string> queryInventoryFailedEvent;

        public static  event Action<List<GooglePurchase>, List<GoogleSkuInfo>> queryInventorySucceededEvent;

        static GoogleIABManager()
        {
            AbstractManager.initialize(typeof(GoogleIABManager));
        }

        public void billingNotSupported(string error)
        {
            ActionExtensions.fire<string>(billingNotSupportedEvent, error);
        }

        public void billingSupported(string empty)
        {
            ActionExtensions.fire(billingSupportedEvent);
        }

        public void consumePurchaseFailed(string error)
        {
            ActionExtensions.fire<string>(consumePurchaseFailedEvent, error);
        }

        public void consumePurchaseSucceeded(string json)
        {
            if (consumePurchaseSucceededEvent != null)
            {
                ActionExtensions.fire<GooglePurchase>(consumePurchaseSucceededEvent, new GooglePurchase(JsonExtensions.dictionaryFromJson(json)));
            }
        }

        public void purchaseCompleteAwaitingVerification(string json)
        {
            if (purchaseCompleteAwaitingVerificationEvent != null)
            {
                Dictionary<string, object> dictionary = JsonExtensions.dictionaryFromJson(json);
                string str = dictionary["purchaseData"].ToString();
                string str2 = dictionary["signature"].ToString();
                purchaseCompleteAwaitingVerificationEvent(str, str2);
            }
        }

        public void purchaseFailed(string json)
        {
            if (purchaseFailedEvent != null)
            {
                Dictionary<string, object> dictionary = Json.decode<Dictionary<string, object>>(json, null);
                purchaseFailedEvent(dictionary["result"].ToString(), int.Parse(dictionary["response"].ToString()));
            }
        }

        public void purchaseSucceeded(string json)
        {
            ActionExtensions.fire<GooglePurchase>(purchaseSucceededEvent, new GooglePurchase(JsonExtensions.dictionaryFromJson(json)));
        }

        public void queryInventoryFailed(string error)
        {
            ActionExtensions.fire<string>(queryInventoryFailedEvent, error);
        }

        public void queryInventorySucceeded(string json)
        {
            if (queryInventorySucceededEvent != null)
            {
                Dictionary<string, object> dictionary = JsonExtensions.dictionaryFromJson(json);
                queryInventorySucceededEvent(GooglePurchase.fromList(dictionary["purchases"] as List<object>), GoogleSkuInfo.fromList(dictionary["skus"] as List<object>));
            }
        }
    }
}

