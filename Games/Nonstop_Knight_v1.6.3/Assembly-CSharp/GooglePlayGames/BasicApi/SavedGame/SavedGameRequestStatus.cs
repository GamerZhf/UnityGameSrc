namespace GooglePlayGames.BasicApi.SavedGame
{
    using System;

    public enum SavedGameRequestStatus
    {
        AuthenticationError = -3,
        BadInputError = -4,
        InternalError = -2,
        Success = 1,
        TimeoutError = -1
    }
}

