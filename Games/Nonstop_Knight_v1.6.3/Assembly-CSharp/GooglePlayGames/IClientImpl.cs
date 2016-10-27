namespace GooglePlayGames
{
    using GooglePlayGames.Native.PInvoke;
    using System;

    internal interface IClientImpl
    {
        PlatformConfiguration CreatePlatformConfiguration();
        TokenClient CreateTokenClient(string playerId, bool reset);
        void GetPlayerStats(IntPtr apiClientPtr, Action<CommonStatusCodes, PlayerStats> callback);
    }
}

