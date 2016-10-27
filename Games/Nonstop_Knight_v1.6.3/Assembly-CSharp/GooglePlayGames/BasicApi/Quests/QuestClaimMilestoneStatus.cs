namespace GooglePlayGames.BasicApi.Quests
{
    using System;

    public enum QuestClaimMilestoneStatus
    {
        Success,
        BadInput,
        InternalError,
        NotAuthorized,
        Timeout,
        MilestoneAlreadyClaimed,
        MilestoneClaimFailed
    }
}

