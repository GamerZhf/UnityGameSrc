namespace GooglePlayGames.BasicApi
{
    using System;

    public enum ResponseStatus
    {
        InternalError = -2,
        LicenseCheckFailed = -1,
        NotAuthorized = -3,
        Success = 1,
        SuccessWithStale = 2,
        Timeout = -5,
        VersionUpdateRequired = -4
    }
}

