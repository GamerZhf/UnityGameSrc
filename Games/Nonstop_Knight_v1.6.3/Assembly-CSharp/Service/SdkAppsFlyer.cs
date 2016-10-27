namespace Service
{
    using System;
    using System.Collections.Generic;

    public class SdkAppsFlyer : ISdkWrapper
    {
        private bool initialized;

        public void Event(ESdkEvent eventName, object value)
        {
            if (this.initialized)
            {
                Dictionary<string, string> eventValues = new Dictionary<string, string>();
                if (eventName == ESdkEvent.Tuturial)
                {
                    string str = "af_tutorial_completion";
                    eventValues["af_success"] = "true";
                    if (value != null)
                    {
                        eventValues["af_content_id"] = value.ToString();
                    }
                    AppsFlyer.trackRichEvent(str, eventValues);
                }
            }
        }

        private void Initialize(string trUserId)
        {
            if (!this.initialized)
            {
                AppsFlyer.setIsDebug(ConfigSdk.SDK_DEBUG);
                AppsFlyer.setAppsFlyerKey(ConfigSdk.APPSFLYER_DEV_KEY);
                AppsFlyer.setCustomerUserID(trUserId);
                AppsFlyer.setAppID(ConfigSdk.ANDROID_PACKAGE_NAME);
                AppsFlyer.setIsDebug(ConfigSdk.SDK_DEBUG);
                AppsFlyer.getConversionData();
                this.initialized = true;
            }
        }

        public void Purchase(PremiumProduct product)
        {
            if (this.initialized)
            {
                Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                dictionary2.Add("af_revenue", string.Empty + product.trackingAmount);
                dictionary2.Add("af_content_id", product.flareProductId);
                dictionary2.Add("af_currency", product.trackingIsoCurrencyCode);
                Dictionary<string, string> eventValues = dictionary2;
                AppsFlyer.trackRichEvent("af_purchase", eventValues);
            }
        }

        public void SessionEnd()
        {
        }

        public void SessionStart(string trUserId)
        {
            this.Initialize(trUserId);
            AppsFlyer.trackAppLaunch();
        }
    }
}

