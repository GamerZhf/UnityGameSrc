namespace GooglePlayGames
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.Events;
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.BasicApi.Nearby;
    using GooglePlayGames.BasicApi.Quests;
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class PlayGamesPlatform : ISocialPlatform
    {
        private IPlayGamesClient mClient;
        private readonly PlayGamesClientConfiguration mConfiguration;
        private string mDefaultLbUi;
        private Dictionary<string, string> mIdMap;
        private PlayGamesLocalUser mLocalUser;
        private static volatile PlayGamesPlatform sInstance;
        private static volatile INearbyConnectionClient sNearbyConnectionClient;
        private static volatile bool sNearbyInitializePending;

        internal PlayGamesPlatform(IPlayGamesClient client)
        {
            this.mIdMap = new Dictionary<string, string>();
            this.mClient = Misc.CheckNotNull<IPlayGamesClient>(client);
            this.mLocalUser = new PlayGamesLocalUser(this);
            this.mConfiguration = PlayGamesClientConfiguration.DefaultConfiguration;
        }

        private PlayGamesPlatform(PlayGamesClientConfiguration configuration)
        {
            this.mIdMap = new Dictionary<string, string>();
            this.mLocalUser = new PlayGamesLocalUser(this);
            this.mConfiguration = configuration;
        }

        public static PlayGamesPlatform Activate()
        {
            Logger.d("Activating PlayGamesPlatform.");
            Social.Active = Instance;
            Logger.d("PlayGamesPlatform activated: " + Social.Active);
            return Instance;
        }

        public void AddIdMapping(string fromId, string toId)
        {
            this.mIdMap[fromId] = toId;
        }

        public void Authenticate(Action<bool> callback)
        {
            this.Authenticate(callback, false);
        }

        public void Authenticate(Action<bool> callback, bool silent)
        {
            if (this.mClient == null)
            {
                Logger.d("Creating platform-specific Play Games client.");
                this.mClient = PlayGamesClientFactory.GetPlatformPlayGamesClient(this.mConfiguration);
            }
            this.mClient.Authenticate(callback, silent);
        }

        public void Authenticate(ILocalUser unused, Action<bool> callback)
        {
            this.Authenticate(callback, false);
        }

        public IAchievement CreateAchievement()
        {
            return new PlayGamesAchievement();
        }

        public ILeaderboard CreateLeaderboard()
        {
            return new PlayGamesLeaderboard(this.mDefaultLbUi);
        }

        public string GetAccessToken()
        {
            if (this.mClient != null)
            {
                return this.mClient.GetAccessToken();
            }
            return null;
        }

        public Achievement GetAchievement(string achievementId)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("GetAchievement can only be called after authentication.");
                return null;
            }
            return this.mClient.GetAchievement(achievementId);
        }

        public IntPtr GetApiClient()
        {
            return this.mClient.GetApiClient();
        }

        internal IUserProfile[] GetFriends()
        {
            if (!this.IsAuthenticated())
            {
                Logger.d("Cannot get friends when not authenticated!");
                return new IUserProfile[0];
            }
            return this.mClient.GetFriends();
        }

        public void GetIdToken(Action<string> idTokenCallback)
        {
            if (this.mClient != null)
            {
                this.mClient.GetIdToken(idTokenCallback);
            }
            else
            {
                Logger.e("No client available, calling back with null.");
                idTokenCallback(null);
            }
        }

        public bool GetLoading(ILeaderboard board)
        {
            return ((board != null) && board.loading);
        }

        public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
        {
            if ((this.mClient != null) && this.mClient.IsAuthenticated())
            {
                this.mClient.GetPlayerStats(callback);
            }
            else
            {
                Logger.e("GetPlayerStats can only be called after authentication.");
                callback(CommonStatusCodes.SignInRequired, new PlayerStats());
            }
        }

        public void GetServerAuthCode(Action<CommonStatusCodes, string> callback)
        {
            if ((this.mClient != null) && this.mClient.IsAuthenticated())
            {
                if (GameInfo.WebClientIdInitialized())
                {
                    this.mClient.GetServerAuthCode("995406906094-gv9jtvln7nl7b44p7od2agqr983j3jdu.apps.googleusercontent.com", callback);
                }
                else
                {
                    Logger.e("GetServerAuthCode requires a webClientId.");
                    callback(CommonStatusCodes.DeveloperError, string.Empty);
                }
            }
            else
            {
                Logger.e("GetServerAuthCode can only be called after authentication.");
                callback(CommonStatusCodes.SignInRequired, string.Empty);
            }
        }

        public string GetToken()
        {
            return this.mClient.GetToken();
        }

        public string GetUserDisplayName()
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("GetUserDisplayName can only be called after authentication.");
                return string.Empty;
            }
            return this.mClient.GetUserDisplayName();
        }

        public string GetUserEmail()
        {
            if (this.mClient != null)
            {
                return this.mClient.GetUserEmail();
            }
            return null;
        }

        public void GetUserEmail(Action<CommonStatusCodes, string> callback)
        {
            this.mClient.GetUserEmail(callback);
        }

        public string GetUserId()
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("GetUserId() can only be called after authentication.");
                return "0";
            }
            return this.mClient.GetUserId();
        }

        public string GetUserImageUrl()
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("GetUserImageUrl can only be called after authentication.");
                return null;
            }
            return this.mClient.GetUserImageUrl();
        }

        internal void HandleLoadingScores(PlayGamesLeaderboard board, LeaderboardScoreData scoreData, Action<bool> callback)
        {
            <HandleLoadingScores>c__AnonStorey26E storeye = new <HandleLoadingScores>c__AnonStorey26E();
            storeye.board = board;
            storeye.callback = callback;
            storeye.<>f__this = this;
            bool flag = storeye.board.SetFromData(scoreData);
            if ((flag && !storeye.board.HasAllScores()) && (scoreData.NextPageToken != null))
            {
                int rowCount = storeye.board.range.count - storeye.board.ScoreCount;
                this.mClient.LoadMoreScores(scoreData.NextPageToken, rowCount, new Action<LeaderboardScoreData>(storeye.<>m__8E));
            }
            else
            {
                storeye.callback(flag);
            }
        }

        public void IncrementAchievement(string achievementID, int steps, Action<bool> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("IncrementAchievement can only be called after authentication.");
                if (callback != null)
                {
                    callback(false);
                }
            }
            else
            {
                Logger.d(string.Concat(new object[] { "IncrementAchievement: ", achievementID, ", steps ", steps }));
                achievementID = this.MapId(achievementID);
                this.mClient.IncrementAchievement(achievementID, steps, callback);
            }
        }

        public static void InitializeInstance(PlayGamesClientConfiguration configuration)
        {
            if (sInstance != null)
            {
                Logger.w("PlayGamesPlatform already initialized. Ignoring this call.");
            }
            else
            {
                sInstance = new PlayGamesPlatform(configuration);
            }
        }

        public static void InitializeNearby(Action<INearbyConnectionClient> callback)
        {
            <InitializeNearby>c__AnonStorey269 storey = new <InitializeNearby>c__AnonStorey269();
            storey.callback = callback;
            Debug.Log("Calling InitializeNearby!");
            if (sNearbyConnectionClient == null)
            {
                NearbyConnectionClientFactory.Create(new Action<INearbyConnectionClient>(storey.<>m__89));
            }
            else if (storey.callback != null)
            {
                Debug.Log("Nearby Already initialized: calling callback directly");
                storey.callback(sNearbyConnectionClient);
            }
            else
            {
                Debug.Log("Nearby Already initialized");
            }
        }

        public bool IsAuthenticated()
        {
            return ((this.mClient != null) && this.mClient.IsAuthenticated());
        }

        public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
        {
            <LoadAchievementDescriptions>c__AnonStorey26A storeya = new <LoadAchievementDescriptions>c__AnonStorey26A();
            storeya.callback = callback;
            if (!this.IsAuthenticated())
            {
                Logger.e("LoadAchievementDescriptions can only be called after authentication.");
                if (storeya.callback != null)
                {
                    storeya.callback(null);
                }
            }
            else
            {
                this.mClient.LoadAchievements(new Action<Achievement[]>(storeya.<>m__8A));
            }
        }

        public void LoadAchievements(Action<IAchievement[]> callback)
        {
            <LoadAchievements>c__AnonStorey26B storeyb = new <LoadAchievements>c__AnonStorey26B();
            storeyb.callback = callback;
            if (!this.IsAuthenticated())
            {
                Logger.e("LoadAchievements can only be called after authentication.");
                storeyb.callback(null);
            }
            this.mClient.LoadAchievements(new Action<Achievement[]>(storeyb.<>m__8B));
        }

        public void LoadFriends(ILocalUser user, Action<bool> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("LoadScores can only be called after authentication.");
                if (callback != null)
                {
                    callback(false);
                }
            }
            this.mClient.LoadFriends(callback);
        }

        public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("LoadMoreScores can only be called after authentication.");
                callback(new LeaderboardScoreData(token.LeaderboardId, ResponseStatus.NotAuthorized));
            }
            else
            {
                this.mClient.LoadMoreScores(token, rowCount, callback);
            }
        }

        public void LoadScores(string leaderboardId, Action<IScore[]> callback)
        {
            <LoadScores>c__AnonStorey26C storeyc = new <LoadScores>c__AnonStorey26C();
            storeyc.callback = callback;
            this.LoadScores(leaderboardId, LeaderboardStart.PlayerCentered, this.mClient.LeaderboardMaxResults(), LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, new Action<LeaderboardScoreData>(storeyc.<>m__8C));
        }

        public void LoadScores(ILeaderboard board, Action<bool> callback)
        {
            LeaderboardTimeSpan daily;
            <LoadScores>c__AnonStorey26D storeyd = new <LoadScores>c__AnonStorey26D();
            storeyd.board = board;
            storeyd.callback = callback;
            storeyd.<>f__this = this;
            if (!this.IsAuthenticated())
            {
                Logger.e("LoadScores can only be called after authentication.");
                if (storeyd.callback != null)
                {
                    storeyd.callback(false);
                }
            }
            switch (storeyd.board.timeScope)
            {
                case TimeScope.Today:
                    daily = LeaderboardTimeSpan.Daily;
                    break;

                case TimeScope.Week:
                    daily = LeaderboardTimeSpan.Weekly;
                    break;

                case TimeScope.AllTime:
                    daily = LeaderboardTimeSpan.AllTime;
                    break;

                default:
                    daily = LeaderboardTimeSpan.AllTime;
                    break;
            }
            ((PlayGamesLeaderboard) storeyd.board).loading = true;
            Logger.d(string.Concat(new object[] { "LoadScores, board=", storeyd.board, " callback is ", storeyd.callback }));
            this.mClient.LoadScores(storeyd.board.id, LeaderboardStart.PlayerCentered, (storeyd.board.range.count <= 0) ? this.mClient.LeaderboardMaxResults() : storeyd.board.range.count, (storeyd.board.userScope != UserScope.FriendsOnly) ? LeaderboardCollection.Public : LeaderboardCollection.Social, daily, new Action<LeaderboardScoreData>(storeyd.<>m__8D));
        }

        public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("LoadScores can only be called after authentication.");
                callback(new LeaderboardScoreData(leaderboardId, ResponseStatus.NotAuthorized));
            }
            else
            {
                this.mClient.LoadScores(leaderboardId, start, rowCount, collection, timeSpan, callback);
            }
        }

        public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("GetUserId() can only be called after authentication.");
                callback(new IUserProfile[0]);
            }
            this.mClient.LoadUsers(userIds, callback);
        }

        private string MapId(string id)
        {
            if (id == null)
            {
                return null;
            }
            if (this.mIdMap.ContainsKey(id))
            {
                string str = this.mIdMap[id];
                Logger.d("Mapping alias " + id + " to ID " + str);
                return str;
            }
            return id;
        }

        public void RegisterInvitationDelegate(InvitationReceivedDelegate deleg)
        {
            this.mClient.RegisterInvitationDelegate(deleg);
        }

        public void ReportProgress(string achievementID, double progress, Action<bool> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("ReportProgress can only be called after authentication.");
                if (callback != null)
                {
                    callback(false);
                }
            }
            else
            {
                Logger.d(string.Concat(new object[] { "ReportProgress, ", achievementID, ", ", progress }));
                achievementID = this.MapId(achievementID);
                if (progress < 1E-06)
                {
                    Logger.d("Progress 0.00 interpreted as request to reveal.");
                    this.mClient.RevealAchievement(achievementID, callback);
                }
                else
                {
                    bool isIncremental = false;
                    int currentSteps = 0;
                    int totalSteps = 0;
                    Achievement achievement = this.mClient.GetAchievement(achievementID);
                    if (achievement == null)
                    {
                        Logger.w("Unable to locate achievement " + achievementID);
                        Logger.w("As a quick fix, assuming it's standard.");
                        isIncremental = false;
                    }
                    else
                    {
                        isIncremental = achievement.IsIncremental;
                        currentSteps = achievement.CurrentSteps;
                        totalSteps = achievement.TotalSteps;
                        Logger.d("Achievement is " + (!isIncremental ? "STANDARD" : "INCREMENTAL"));
                        if (isIncremental)
                        {
                            Logger.d(string.Concat(new object[] { "Current steps: ", currentSteps, "/", totalSteps }));
                        }
                    }
                    if (isIncremental)
                    {
                        Logger.d("Progress " + progress + " interpreted as incremental target (approximate).");
                        if ((progress >= 0.0) && (progress <= 1.0))
                        {
                            Logger.w("Progress " + progress + " is less than or equal to 1. You might be trying to use values in the range of [0,1], while values are expected to be within the range [0,100]. If you are using the latter, you can safely ignore this message.");
                        }
                        int num3 = (int) ((progress / 100.0) * totalSteps);
                        int steps = num3 - currentSteps;
                        Logger.d(string.Concat(new object[] { "Target steps: ", num3, ", cur steps:", currentSteps }));
                        Logger.d("Steps to increment: " + steps);
                        if (steps >= 0)
                        {
                            this.mClient.IncrementAchievement(achievementID, steps, callback);
                        }
                    }
                    else if (progress >= 100.0)
                    {
                        Logger.d("Progress " + progress + " interpreted as UNLOCK.");
                        this.mClient.UnlockAchievement(achievementID, callback);
                    }
                    else
                    {
                        Logger.d("Progress " + progress + " not enough to unlock non-incremental achievement.");
                    }
                }
            }
        }

        public void ReportScore(long score, string board, Action<bool> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("ReportScore can only be called after authentication.");
                if (callback != null)
                {
                    callback(false);
                }
            }
            else
            {
                Logger.d(string.Concat(new object[] { "ReportScore: score=", score, ", board=", board }));
                string leaderboardId = this.MapId(board);
                this.mClient.SubmitScore(leaderboardId, score, callback);
            }
        }

        public void ReportScore(long score, string board, string metadata, Action<bool> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("ReportScore can only be called after authentication.");
                if (callback != null)
                {
                    callback(false);
                }
            }
            else
            {
                Logger.d(string.Concat(new object[] { "ReportScore: score=", score, ", board=", board, " metadata=", metadata }));
                string leaderboardId = this.MapId(board);
                this.mClient.SubmitScore(leaderboardId, score, metadata, callback);
            }
        }

        public void SetDefaultLeaderboardForUI(string lbid)
        {
            Logger.d("SetDefaultLeaderboardForUI: " + lbid);
            if (lbid != null)
            {
                lbid = this.MapId(lbid);
            }
            this.mDefaultLbUi = lbid;
        }

        public void SetStepsAtLeast(string achievementID, int steps, Action<bool> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("SetStepsAtLeast can only be called after authentication.");
                if (callback != null)
                {
                    callback(false);
                }
            }
            else
            {
                Logger.d(string.Concat(new object[] { "SetStepsAtLeast: ", achievementID, ", steps ", steps }));
                achievementID = this.MapId(achievementID);
                this.mClient.SetStepsAtLeast(achievementID, steps, callback);
            }
        }

        public void ShowAchievementsUI()
        {
            this.ShowAchievementsUI(null);
        }

        public void ShowAchievementsUI(Action<UIStatus> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("ShowAchievementsUI can only be called after authentication.");
            }
            else
            {
                Logger.d("ShowAchievementsUI callback is " + callback);
                this.mClient.ShowAchievementsUI(callback);
            }
        }

        public void ShowLeaderboardUI()
        {
            Logger.d("ShowLeaderboardUI with default ID");
            this.ShowLeaderboardUI(this.MapId(this.mDefaultLbUi), null);
        }

        public void ShowLeaderboardUI(string leaderboardId)
        {
            if (leaderboardId != null)
            {
                leaderboardId = this.MapId(leaderboardId);
            }
            this.mClient.ShowLeaderboardUI(leaderboardId, LeaderboardTimeSpan.AllTime, null);
        }

        public void ShowLeaderboardUI(string leaderboardId, Action<UIStatus> callback)
        {
            this.ShowLeaderboardUI(leaderboardId, LeaderboardTimeSpan.AllTime, callback);
        }

        public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback)
        {
            if (!this.IsAuthenticated())
            {
                Logger.e("ShowLeaderboardUI can only be called after authentication.");
                if (callback != null)
                {
                    callback(UIStatus.NotAuthorized);
                }
            }
            else
            {
                Logger.d(string.Concat(new object[] { "ShowLeaderboardUI, lbId=", leaderboardId, " callback is ", callback }));
                this.mClient.ShowLeaderboardUI(leaderboardId, span, callback);
            }
        }

        public void SignOut()
        {
            if (this.mClient != null)
            {
                this.mClient.SignOut();
            }
            this.mLocalUser = new PlayGamesLocalUser(this);
        }

        public static bool DebugLogEnabled
        {
            get
            {
                return Logger.DebugLogEnabled;
            }
            set
            {
                Logger.DebugLogEnabled = value;
            }
        }

        public IEventsClient Events
        {
            get
            {
                return this.mClient.GetEventsClient();
            }
        }

        public static PlayGamesPlatform Instance
        {
            get
            {
                if (sInstance == null)
                {
                    Logger.d("Instance was not initialized, using default configuration.");
                    InitializeInstance(PlayGamesClientConfiguration.DefaultConfiguration);
                }
                return sInstance;
            }
        }

        public ILocalUser localUser
        {
            get
            {
                return this.mLocalUser;
            }
        }

        public static INearbyConnectionClient Nearby
        {
            get
            {
                if ((sNearbyConnectionClient == null) && !sNearbyInitializePending)
                {
                    sNearbyInitializePending = true;
                    InitializeNearby(null);
                }
                return sNearbyConnectionClient;
            }
        }

        public IQuestsClient Quests
        {
            get
            {
                return this.mClient.GetQuestsClient();
            }
        }

        public IRealTimeMultiplayerClient RealTime
        {
            get
            {
                return this.mClient.GetRtmpClient();
            }
        }

        public ISavedGameClient SavedGame
        {
            get
            {
                return this.mClient.GetSavedGameClient();
            }
        }

        public ITurnBasedMultiplayerClient TurnBased
        {
            get
            {
                return this.mClient.GetTbmpClient();
            }
        }

        [CompilerGenerated]
        private sealed class <HandleLoadingScores>c__AnonStorey26E
        {
            internal PlayGamesPlatform <>f__this;
            internal PlayGamesLeaderboard board;
            internal Action<bool> callback;

            internal void <>m__8E(LeaderboardScoreData nextScoreData)
            {
                this.<>f__this.HandleLoadingScores(this.board, nextScoreData, this.callback);
            }
        }

        [CompilerGenerated]
        private sealed class <InitializeNearby>c__AnonStorey269
        {
            internal Action<INearbyConnectionClient> callback;

            internal void <>m__89(INearbyConnectionClient client)
            {
                Debug.Log("Nearby Client Created!!");
                PlayGamesPlatform.sNearbyConnectionClient = client;
                if (this.callback != null)
                {
                    this.callback(client);
                }
                else
                {
                    Debug.Log("Initialize Nearby callback is null");
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAchievementDescriptions>c__AnonStorey26A
        {
            internal Action<IAchievementDescription[]> callback;

            internal void <>m__8A(Achievement[] ach)
            {
                IAchievementDescription[] descriptionArray = new IAchievementDescription[ach.Length];
                for (int i = 0; i < descriptionArray.Length; i++)
                {
                    descriptionArray[i] = new PlayGamesAchievement(ach[i]);
                }
                this.callback(descriptionArray);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAchievements>c__AnonStorey26B
        {
            internal Action<IAchievement[]> callback;

            internal void <>m__8B(Achievement[] ach)
            {
                IAchievement[] achievementArray = new IAchievement[ach.Length];
                for (int i = 0; i < achievementArray.Length; i++)
                {
                    achievementArray[i] = new PlayGamesAchievement(ach[i]);
                }
                this.callback(achievementArray);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadScores>c__AnonStorey26C
        {
            internal Action<IScore[]> callback;

            internal void <>m__8C(LeaderboardScoreData scoreData)
            {
                this.callback(scoreData.Scores);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadScores>c__AnonStorey26D
        {
            internal PlayGamesPlatform <>f__this;
            internal ILeaderboard board;
            internal Action<bool> callback;

            internal void <>m__8D(LeaderboardScoreData scoreData)
            {
                this.<>f__this.HandleLoadingScores((PlayGamesLeaderboard) this.board, scoreData, this.callback);
            }
        }
    }
}

