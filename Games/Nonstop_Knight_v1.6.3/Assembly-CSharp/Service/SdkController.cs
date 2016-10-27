namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SdkController
    {
        private IList<ISdkWrapper> m_trackingSdks = new List<ISdkWrapper>();

        public SdkController()
        {
            Binder.EventBus.OnPlayerLoggedIn += new Events.PlayerLoggedIn(this.onPlayerLoggedIn);
            Binder.EventBus.OnPlayerRegistered += new Events.PlayerRegistered(this.onPlayerLoggedIn);
        }

        private void Add(ISdkWrapper wrapper)
        {
            this.m_trackingSdks.Add(wrapper);
            if (ConfigSdk.SDK_DEBUG)
            {
                Debug.Log("Add SDK Wrapper: " + wrapper.GetType());
            }
        }

        public void Event(ESdkEvent eventName, [Optional, DefaultParameterValue(null)] object param)
        {
            IEnumerator<ISdkWrapper> enumerator = this.m_trackingSdks.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Event(eventName, param);
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
        }

        public void Initialize()
        {
            this.Add(new SdkAppsFlyer());
            this.Add(new SdkMAT());
            this.Add(new SdkAppboy());
            this.Add(new SdkFacebook());
        }

        private void onPlayerLoggedIn()
        {
            this.SessionStart(Binder.SessionData.FgUserHandle);
        }

        public void Purchase(PremiumProduct product)
        {
            if (product != null)
            {
                IEnumerator<ISdkWrapper> enumerator = this.m_trackingSdks.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.Purchase(product);
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
            }
        }

        public void SessionEnd()
        {
            IEnumerator<ISdkWrapper> enumerator = this.m_trackingSdks.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.SessionEnd();
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
        }

        public void SessionStart(string trackingUserId)
        {
            if (ConfigSdk.SDK_DEBUG)
            {
                Debug.Log("Init SDKs:" + trackingUserId);
            }
            IEnumerator<ISdkWrapper> enumerator = this.m_trackingSdks.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.SessionStart(trackingUserId);
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
        }
    }
}

