namespace Service
{
    using System;

    public interface ISessionData
    {
        string ClientVersion { get; }

        string FgUserHandle { get; }

        bool IsLoggedIn { get; }

        string ServerUrl { get; }

        string SessionId { get; }

        string TrackingUrl { get; }

        string UserId { get; }
    }
}

