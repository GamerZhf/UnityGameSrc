namespace Prime31
{
    using System;
    using UnityEngine;

    public class GoogleIAB
    {
        private static AndroidJavaObject _plugin;

        static GoogleIAB()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("com.prime31.GoogleIABPlugin"))
                {
                    _plugin = class2.CallStatic<AndroidJavaObject>("instance", new object[0]);
                }
            }
        }

        public static bool areSubscriptionsSupported()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                return false;
            }
            return _plugin.Call<bool>("areSubscriptionsSupported", new object[0]);
        }

        public static void consumeProduct(string sku)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                object[] args = new object[] { sku };
                _plugin.Call("consumeProduct", args);
            }
        }

        public static void consumeProducts(string[] skus)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                object[] args = new object[] { skus };
                _plugin.Call("consumeProducts", args);
            }
        }

        public static void enableLogging(bool shouldEnable)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (shouldEnable)
                {
                    Debug.LogWarning("YOU HAVE ENABLED HIGH DETAIL LOGS. DO NOT DISTRIBUTE THE GENERATED APK PUBLICLY. IT WILL DUMP SENSITIVE INFORMATION TO THE CONSOLE!");
                }
                object[] args = new object[] { shouldEnable };
                _plugin.Call("enableLogging", args);
            }
        }

        public static void init(string publicKey)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                object[] args = new object[] { publicKey };
                _plugin.Call("init", args);
            }
        }

        public static void purchaseProduct(string sku)
        {
            purchaseProduct(sku, string.Empty);
        }

        public static void purchaseProduct(string sku, string developerPayload)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                object[] args = new object[] { sku, developerPayload };
                _plugin.Call("purchaseProduct", args);
            }
        }

        public static void queryInventory(string[] skus)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                object[] args = new object[] { skus };
                _plugin.Call("queryInventory", args);
            }
        }

        public static void setAutoVerifySignatures(bool shouldVerify)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                object[] args = new object[] { shouldVerify };
                _plugin.Call("setAutoVerifySignatures", args);
            }
        }

        public static void unbindService()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                _plugin.Call("unbindService", new object[0]);
            }
        }
    }
}

