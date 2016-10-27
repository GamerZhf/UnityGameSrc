namespace GooglePlayGames.BasicApi.SavedGame
{
    using System;

    public enum SelectUIStatus
    {
        AuthenticationError = -3,
        BadInputError = -4,
        InternalError = -1,
        SavedGameSelected = 1,
        TimeoutError = -2,
        UserClosedUI = 2
    }
}

