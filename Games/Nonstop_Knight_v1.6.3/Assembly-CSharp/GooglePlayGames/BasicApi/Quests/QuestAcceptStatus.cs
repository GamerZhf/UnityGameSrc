namespace GooglePlayGames.BasicApi.Quests
{
    using System;

    public enum QuestAcceptStatus
    {
        Success,
        BadInput,
        InternalError,
        NotAuthorized,
        Timeout,
        QuestNoLongerAvailable,
        QuestNotStarted
    }
}

