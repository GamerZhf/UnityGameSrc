namespace GooglePlayGames.BasicApi
{
    using GooglePlayGames.BasicApi.Events;
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.BasicApi.Quests;
    using GooglePlayGames.BasicApi.SavedGame;
    using System;
    using UnityEngine.SocialPlatforms;

    public interface IPlayGamesClient
    {
        void Authenticate(Action<bool> callback, bool silent);
        string GetAccessToken();
        Achievement GetAchievement(string achievementId);
        IntPtr GetApiClient();
        IEventsClient GetEventsClient();
        IUserProfile[] GetFriends();
        void GetIdToken(Action<string> idTokenCallback);
        void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback);
        IQuestsClient GetQuestsClient();
        IRealTimeMultiplayerClient GetRtmpClient();
        ISavedGameClient GetSavedGameClient();
        void GetServerAuthCode(string serverClientId, Action<CommonStatusCodes, string> callback);
        ITurnBasedMultiplayerClient GetTbmpClient();
        string GetToken();
        string GetUserDisplayName();
        string GetUserEmail();
        void GetUserEmail(Action<CommonStatusCodes, string> callback);
        string GetUserId();
        string GetUserImageUrl();
        void IncrementAchievement(string achievementId, int steps, Action<bool> successOrFailureCalllback);
        bool IsAuthenticated();
        int LeaderboardMaxResults();
        void LoadAchievements(Action<Achievement[]> callback);
        void LoadFriends(Action<bool> callback);
        void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback);
        void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback);
        void LoadUsers(string[] userIds, Action<IUserProfile[]> callback);
        void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate);
        void RevealAchievement(string achievementId, Action<bool> successOrFailureCalllback);
        void SetStepsAtLeast(string achId, int steps, Action<bool> callback);
        void ShowAchievementsUI(Action<UIStatus> callback);
        void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback);
        void SignOut();
        void SubmitScore(string leaderboardId, long score, Action<bool> successOrFailureCalllback);
        void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> successOrFailureCalllback);
        void UnlockAchievement(string achievementId, Action<bool> successOrFailureCalllback);
    }
}

