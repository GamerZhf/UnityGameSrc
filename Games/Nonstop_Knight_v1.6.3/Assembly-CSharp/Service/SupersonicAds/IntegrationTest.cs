namespace Service.SupersonicAds
{
    using System;
    using System.Diagnostics;
    using UnityEngine;

    public class IntegrationTest
    {
        [Conditional("DEVELOPMENT")]
        public static void ExecuteIntegrationTest()
        {
            try
            {
                AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass class3 = new AndroidJavaClass("com.supersonic.mediationsdk.integration.IntegrationHelper");
                object[] args = new object[] { @static };
                class3.CallStatic("validateIntegration", args);
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.Log(" --- Error calling Supersonic integration test: " + exception);
            }
        }
    }
}

