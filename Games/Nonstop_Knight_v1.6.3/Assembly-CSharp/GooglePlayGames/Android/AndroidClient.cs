namespace GooglePlayGames.Android
{
    using Com.Google.Android.Gms.Common.Api;
    using Com.Google.Android.Gms.Games;
    using Com.Google.Android.Gms.Games.Stats;
    using GooglePlayGames;
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AndroidClient : IClientImpl
    {
        [CompilerGenerated]
        private static Action<IntPtr> <>f__am$cache1;
        internal const string BridgeActivityClass = "com.google.games.bridge.NativeBridgeActivity";
        private const string LaunchBridgeMethod = "launchBridgeIntent";
        private const string LaunchBridgeSignature = "(Landroid/app/Activity;Landroid/content/Intent;)V";
        private TokenClient tokenClient;

        public PlatformConfiguration CreatePlatformConfiguration()
        {
            AndroidPlatformConfiguration configuration = AndroidPlatformConfiguration.Create();
            using (AndroidJavaObject obj2 = AndroidTokenClient.GetActivity())
            {
                configuration.SetActivity(obj2.GetRawObject());
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate (IntPtr intent) {
                        <CreatePlatformConfiguration>c__AnonStorey270 storey = new <CreatePlatformConfiguration>c__AnonStorey270 {
                            intentRef = AndroidJNI.NewGlobalRef(intent)
                        };
                        PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__92));
                    };
                }
                configuration.SetOptionalIntentHandlerForUI(<>f__am$cache1);
            }
            return configuration;
        }

        public TokenClient CreateTokenClient(string playerId, bool reset)
        {
            if ((this.tokenClient == null) || reset)
            {
                this.tokenClient = new AndroidTokenClient(playerId);
            }
            return this.tokenClient;
        }

        public void GetPlayerStats(IntPtr apiClient, Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback)
        {
            StatsResultCallback callback2;
            <GetPlayerStats>c__AnonStorey271 storey = new <GetPlayerStats>c__AnonStorey271();
            storey.callback = callback;
            GoogleApiClient client = new GoogleApiClient(apiClient);
            try
            {
                callback2 = new StatsResultCallback(new Action<int, Com.Google.Android.Gms.Games.Stats.PlayerStats>(storey.<>m__91));
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                storey.callback(CommonStatusCodes.DeveloperError, null);
                return;
            }
            Com.Google.Android.Gms.Games.Games.Stats.loadPlayerStats(client, true).setResultCallback(callback2);
        }

        private static void LaunchBridgeIntent(IntPtr bridgedIntent)
        {
            object[] args = new object[2];
            jvalue[] jvalueArray = AndroidJNIHelper.CreateJNIArgArray(args);
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("com.google.games.bridge.NativeBridgeActivity"))
                {
                    using (AndroidJavaObject obj2 = AndroidTokenClient.GetActivity())
                    {
                        IntPtr methodID = AndroidJNI.GetStaticMethodID(class2.GetRawClass(), "launchBridgeIntent", "(Landroid/app/Activity;Landroid/content/Intent;)V");
                        jvalueArray[0].l = obj2.GetRawObject();
                        jvalueArray[1].l = bridgedIntent;
                        AndroidJNI.CallStaticVoidMethod(class2.GetRawClass(), methodID, jvalueArray);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.e("Exception launching bridge intent: " + exception.Message);
                Logger.e(exception.ToString());
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
        }

        [CompilerGenerated]
        private sealed class <CreatePlatformConfiguration>c__AnonStorey270
        {
            internal IntPtr intentRef;

            internal void <>m__92()
            {
                try
                {
                    AndroidClient.LaunchBridgeIntent(this.intentRef);
                }
                finally
                {
                    AndroidJNI.DeleteGlobalRef(this.intentRef);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetPlayerStats>c__AnonStorey271
        {
            internal Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback;

            internal void <>m__91(int result, Com.Google.Android.Gms.Games.Stats.PlayerStats stats)
            {
                Debug.Log("Result for getStats: " + result);
                GooglePlayGames.BasicApi.PlayerStats stats2 = null;
                if (stats != null)
                {
                    stats2 = new GooglePlayGames.BasicApi.PlayerStats();
                    stats2.AvgSessonLength = stats.getAverageSessionLength();
                    stats2.DaysSinceLastPlayed = stats.getDaysSinceLastPlayed();
                    stats2.NumberOfPurchases = stats.getNumberOfPurchases();
                    stats2.NumberOfSessions = stats.getNumberOfSessions();
                    stats2.SessPercentile = stats.getSessionPercentile();
                    stats2.SpendPercentile = stats.getSpendPercentile();
                    stats2.ChurnProbability = stats.getChurnProbability();
                    stats2.SpendProbability = stats.getSpendProbability();
                }
                this.callback((CommonStatusCodes) result, stats2);
            }
        }

        private class StatsResultCallback : ResultCallbackProxy<Stats_LoadPlayerStatsResultObject>
        {
            private Action<int, Com.Google.Android.Gms.Games.Stats.PlayerStats> callback;

            public StatsResultCallback(Action<int, Com.Google.Android.Gms.Games.Stats.PlayerStats> callback)
            {
                this.callback = callback;
            }

            public override void OnResult(Stats_LoadPlayerStatsResultObject arg_Result_1)
            {
                this.callback(arg_Result_1.getStatus().getStatusCode(), arg_Result_1.getPlayerStats());
            }
        }
    }
}

