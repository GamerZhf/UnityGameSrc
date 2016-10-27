namespace Service
{
    using MATSDK;
    using System;

    public class SdkMAT : ISdkWrapper
    {
        private bool initialized;

        public void Event(ESdkEvent eventName, object param)
        {
            if (this.initialized)
            {
                if (eventName == ESdkEvent.Tuturial)
                {
                    MATEvent tuneEvent = new MATEvent("tutorial_complete");
                    MATBinding.MeasureEvent(tuneEvent);
                }
            }
        }

        private void Initialize(string trUserId)
        {
            if (!this.initialized)
            {
                MATBinding.Init(ConfigSdk.MAT_ADVERTISER_ID, ConfigSdk.MAT_CONVERSION_KEY);
                MATBinding.SetPackageName(ConfigSdk.ANDROID_PACKAGE_NAME);
                MATBinding.SetUserId(trUserId);
                MATBinding.AutomateIapEventMeasurement(true);
                MATBinding.SetDebugMode(ConfigSdk.SDK_DEBUG);
                this.initialized = true;
            }
        }

        public void Purchase(PremiumProduct product)
        {
            if (this.initialized)
            {
                MATEvent tuneEvent = new MATEvent();
                tuneEvent.name = "purchase";
                tuneEvent.revenue = new double?((double) product.price);
                tuneEvent.currencyCode = product.currencyCode;
                MATBinding.MeasureEvent(tuneEvent);
            }
        }

        public void SessionEnd()
        {
            if (this.initialized)
            {
                MATBinding.MeasureSession();
            }
        }

        public void SessionStart(string trUserId)
        {
            this.Initialize(trUserId);
            MATBinding.MeasureSession();
        }
    }
}

