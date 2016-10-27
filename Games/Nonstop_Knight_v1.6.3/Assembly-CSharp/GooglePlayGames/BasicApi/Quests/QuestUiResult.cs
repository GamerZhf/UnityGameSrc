namespace GooglePlayGames.BasicApi.Quests
{
    using System;

    public enum QuestUiResult
    {
        UserRequestsQuestAcceptance,
        UserRequestsMilestoneClaiming,
        BadInput,
        InternalError,
        UserCanceled,
        NotAuthorized,
        VersionUpdateRequired,
        Timeout,
        UiBusy
    }
}

