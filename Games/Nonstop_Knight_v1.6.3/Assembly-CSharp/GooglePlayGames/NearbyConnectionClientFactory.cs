namespace GooglePlayGames
{
    using GooglePlayGames.BasicApi.Nearby;
    using GooglePlayGames.Native;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using UnityEngine;

    public static class NearbyConnectionClientFactory
    {
        public static void Create(Action<INearbyConnectionClient> callback)
        {
            if (Application.isEditor)
            {
                Logger.d("Creating INearbyConnection in editor, using DummyClient.");
                callback(new DummyNearbyConnectionClient());
            }
            Logger.d("Creating real INearbyConnectionClient");
            NativeNearbyConnectionClientFactory.Create(callback);
        }

        private static GooglePlayGames.BasicApi.Nearby.InitializationStatus ToStatus(NearbyConnectionsStatus.InitializationStatus status)
        {
            NearbyConnectionsStatus.InitializationStatus status2 = status;
            switch ((status2 + 4))
            {
                case ~(NearbyConnectionsStatus.InitializationStatus.VALID | NearbyConnectionsStatus.InitializationStatus.ERROR_INTERNAL):
                    return GooglePlayGames.BasicApi.Nearby.InitializationStatus.VersionUpdateRequired;

                case ~(NearbyConnectionsStatus.InitializationStatus.VALID | NearbyConnectionsStatus.InitializationStatus.ERROR_VERSION_UPDATE_REQUIRED):
                    return GooglePlayGames.BasicApi.Nearby.InitializationStatus.InternalError;

                case ((NearbyConnectionsStatus.InitializationStatus) 5):
                    return GooglePlayGames.BasicApi.Nearby.InitializationStatus.Success;
            }
            Logger.w("Unknown initialization status: " + status);
            return GooglePlayGames.BasicApi.Nearby.InitializationStatus.InternalError;
        }
    }
}

