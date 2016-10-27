namespace GooglePlayGames.Native.Cwrapper
{
    using System;

    internal static class NearbyConnectionsStatus
    {
        internal enum InitializationStatus
        {
            ERROR_INTERNAL = -2,
            ERROR_VERSION_UPDATE_REQUIRED = -4,
            VALID = 1
        }
    }
}

