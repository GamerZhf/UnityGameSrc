namespace GooglePlayGames.BasicApi
{
    using System;

    public enum UIStatus
    {
        InternalError = -2,
        LeftRoom = -18,
        NotAuthorized = -3,
        Timeout = -5,
        UiBusy = -12,
        UserClosedUI = -6,
        Valid = 1,
        VersionUpdateRequired = -4
    }
}

