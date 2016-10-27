namespace Service
{
    using System;

    public enum ServerErrorCode
    {
        Unspecified,
        Internal,
        SessionExpired,
        NotFound,
        InvalidPassword,
        AuthenticationError,
        ShopUnavailable,
        ShopInvalidResponse,
        ShopInvalidRequest,
        DisallowedAction,
        TournamentNotStarted,
        TournamentJoinTimeOver,
        TournamentEnded
    }
}

