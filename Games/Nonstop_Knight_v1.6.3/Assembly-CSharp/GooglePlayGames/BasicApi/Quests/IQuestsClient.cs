namespace GooglePlayGames.BasicApi.Quests
{
    using GooglePlayGames.BasicApi;
    using System;

    public interface IQuestsClient
    {
        void Accept(IQuest quest, Action<QuestAcceptStatus, IQuest> callback);
        void ClaimMilestone(IQuestMilestone milestone, Action<QuestClaimMilestoneStatus, IQuest, IQuestMilestone> callback);
        void Fetch(DataSource source, string questId, Action<ResponseStatus, IQuest> callback);
        void FetchMatchingState(DataSource source, QuestFetchFlags flags, Action<ResponseStatus, List<IQuest>> callback);
        void ShowAllQuestsUI(Action<QuestUiResult, IQuest, IQuestMilestone> callback);
        void ShowSpecificQuestUI(IQuest quest, Action<QuestUiResult, IQuest, IQuestMilestone> callback);
    }
}

