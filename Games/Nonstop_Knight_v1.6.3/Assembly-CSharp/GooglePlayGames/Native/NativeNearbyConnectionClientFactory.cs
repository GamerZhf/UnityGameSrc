namespace GooglePlayGames.Native
{
    using GooglePlayGames.Android;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class NativeNearbyConnectionClientFactory
    {
        private static Action<INearbyConnectionClient> sCreationCallback;
        private static volatile NearbyConnectionsManager sManager;

        public static void Create(Action<INearbyConnectionClient> callback)
        {
            if (sManager == null)
            {
                sCreationCallback = callback;
                InitializeFactory();
            }
            else
            {
                callback(new NativeNearbyConnectionsClient(GetManager()));
            }
        }

        internal static NearbyConnectionsManager GetManager()
        {
            return sManager;
        }

        internal static void InitializeFactory()
        {
            PlayGamesHelperObject.CreateObject();
            NearbyConnectionsManager.ReadServiceId();
            NearbyConnectionsManagerBuilder builder = new NearbyConnectionsManagerBuilder();
            builder.SetOnInitializationFinished(new Action<NearbyConnectionsStatus.InitializationStatus>(NativeNearbyConnectionClientFactory.OnManagerInitialized));
            PlatformConfiguration configuration = new AndroidClient().CreatePlatformConfiguration();
            Debug.Log("Building manager Now");
            sManager = builder.Build(configuration);
        }

        internal static void OnManagerInitialized(NearbyConnectionsStatus.InitializationStatus status)
        {
            Debug.Log(string.Concat(new object[] { "Nearby Init Complete: ", status, " sManager = ", sManager }));
            if (status == NearbyConnectionsStatus.InitializationStatus.VALID)
            {
                if (sCreationCallback != null)
                {
                    sCreationCallback(new NativeNearbyConnectionsClient(GetManager()));
                    sCreationCallback = null;
                }
            }
            else
            {
                Debug.LogError("ERROR: NearbyConnectionManager not initialized: " + status);
            }
        }
    }
}

