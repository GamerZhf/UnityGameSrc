namespace GooglePlayGames.BasicApi
{
    using GooglePlayGames.BasicApi.Events;
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.BasicApi.Quests;
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.OurUtils;
    using System;
    using UnityEngine.SocialPlatforms;

    public class DummyClient : IPlayGamesClient
    {
        public void Authenticate(Action<bool> callback, bool silent)
        {
            LogUsage();
            if (callback != null)
            {
                callback(false);
            }
        }

        public string GetAccessToken()
        {
            LogUsage();
            return "DummyAccessToken";
        }

        public Achievement GetAchievement(string achId)
        {
            LogUsage();
            return null;
        }

        public IntPtr GetApiClient()
        {
            LogUsage();
            return IntPtr.Zero;
        }

        public IEventsClient GetEventsClient()
        {
            LogUsage();
            return null;
        }

        public IUserProfile[] GetFriends()
        {
            LogUsage();
            return new IUserProfile[0];
        }

        public void GetIdToken(Action<string> idTokenCallback)
        {
            LogUsage();
            if (idTokenCallback != null)
            {
                idTokenCallback("DummyIdToken");
            }
        }

        public Invitation GetInvitationFromNotification()
        {
            LogUsage();
            return null;
        }

        public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
        {
            LogUsage();
            callback(CommonStatusCodes.ApiNotConnected, new PlayerStats());
        }

        public IQuestsClient GetQuestsClient()
        {
            LogUsage();
            return null;
        }

        public IRealTimeMultiplayerClient GetRtmpClient()
        {
            LogUsage();
            return null;
        }

        public ISavedGameClient GetSavedGameClient()
        {
            LogUsage();
            return null;
        }

        public void GetServerAuthCode(string serverClientId, Action<CommonStatusCodes, string> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(CommonStatusCodes.ApiNotConnected, "DummyServerAuthCode");
            }
        }

        public ITurnBasedMultiplayerClient GetTbmpClient()
        {
            LogUsage();
            return null;
        }

        public string GetToken()
        {
            return "DummyToken";
        }

        public string GetUserDisplayName()
        {
            LogUsage();
            return "Player";
        }

        public string GetUserEmail()
        {
            return string.Empty;
        }

        public void GetUserEmail(Action<CommonStatusCodes, string> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(CommonStatusCodes.ApiNotConnected, null);
            }
        }

        public string GetUserId()
        {
            LogUsage();
            return "DummyID";
        }

        public string GetUserImageUrl()
        {
            LogUsage();
            return null;
        }

        public bool HasInvitationFromNotification()
        {
            LogUsage();
            return false;
        }

        public void IncrementAchievement(string achId, int steps, Action<bool> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(false);
            }
        }

        public bool IsAuthenticated()
        {
            LogUsage();
            return false;
        }

        public int LeaderboardMaxResults()
        {
            return 0x19;
        }

        public void LoadAchievements(Action<Achievement[]> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(null);
            }
        }

        public void LoadFriends(Action<bool> callback)
        {
            LogUsage();
            callback(false);
        }

        public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(new LeaderboardScoreData(token.LeaderboardId, ResponseStatus.LicenseCheckFailed));
            }
        }

        public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(new LeaderboardScoreData(leaderboardId, ResponseStatus.LicenseCheckFailed));
            }
        }

        public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(null);
            }
        }

        private static void LogUsage()
        {
            Logger.d("Received method call on DummyClient - using stub implementation.");
        }

        public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
        {
            LogUsage();
        }

        public void RevealAchievement(string achId, Action<bool> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(false);
            }
        }

        public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(false);
            }
        }

        public void ShowAchievementsUI(Action<UIStatus> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(UIStatus.VersionUpdateRequired);
            }
        }

        public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(UIStatus.VersionUpdateRequired);
            }
        }

        public void SignOut()
        {
            LogUsage();
        }

        public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(false);
            }
        }

        public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(false);
            }
        }

        public void UnlockAchievement(string achId, Action<bool> callback)
        {
            LogUsage();
            if (callback != null)
            {
                callback(false);
            }
        }
    }
}

