namespace Service
{
    using System;
    using UnityEngine;

    public class MockSdkWrapper : ISdkWrapper
    {
        private string m_sdkName;

        public MockSdkWrapper(string sdkName)
        {
            this.m_sdkName = sdkName;
        }

        public void Event(ESdkEvent eventName, object param)
        {
            if (param != null)
            {
                this.Print(string.Concat(new object[] { "Event ", eventName, " (", param, ")" }));
            }
            else
            {
                this.Print("Event " + eventName);
            }
        }

        private void Print(string value)
        {
            if (ConfigSdk.SDK_DEBUG)
            {
                Debug.Log("[Mock SDK " + this.m_sdkName + "] " + value);
            }
        }

        public void Purchase(PremiumProduct product)
        {
            this.Print("Purchase " + product.flareProductId);
        }

        public void SessionEnd()
        {
            this.Print("Session end");
        }

        public void SessionStart(string trUserId)
        {
            this.Print("Session start:" + trUserId);
        }
    }
}

