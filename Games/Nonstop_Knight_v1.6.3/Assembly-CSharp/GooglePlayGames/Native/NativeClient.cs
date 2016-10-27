namespace GooglePlayGames.Native
{
    using GooglePlayGames;
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.Events;
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.BasicApi.Quests;
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class NativeClient : IPlayGamesClient
    {
        [CompilerGenerated]
        private static Predicate<GooglePlayGames.BasicApi.Achievement> <>f__am$cache18;
        [CompilerGenerated]
        private static Predicate<GooglePlayGames.BasicApi.Achievement> <>f__am$cache19;
        private readonly object AuthStateLock = new object();
        private readonly IClientImpl clientImpl;
        private volatile bool friendsLoading;
        private readonly object GameServicesLock = new object();
        private volatile Dictionary<string, GooglePlayGames.BasicApi.Achievement> mAchievements;
        private volatile uint mAuthGeneration;
        private volatile AuthState mAuthState;
        private readonly PlayGamesClientConfiguration mConfiguration;
        private volatile IEventsClient mEventsClient;
        private volatile List<GooglePlayGames.BasicApi.Multiplayer.Player> mFriends;
        private volatile Action<Invitation, bool> mInvitationDelegate;
        private volatile Action<bool> mPendingAuthCallbacks;
        private volatile IQuestsClient mQuestsClient;
        private volatile NativeRealtimeMultiplayerClient mRealTimeClient;
        private volatile ISavedGameClient mSavedGameClient;
        private GooglePlayGames.Native.PInvoke.GameServices mServices;
        private volatile Action<bool> mSilentAuthCallbacks;
        private volatile bool mSilentAuthFailed;
        private volatile TokenClient mTokenClient;
        private volatile NativeTurnBasedMultiplayerClient mTurnBasedClient;
        private volatile GooglePlayGames.BasicApi.Multiplayer.Player mUser;
        private int noWebClientIdWarningCount;
        private string rationale;
        private int webclientWarningFreq = 0x186a0;

        internal NativeClient(PlayGamesClientConfiguration configuration, IClientImpl clientImpl)
        {
            PlayGamesHelperObject.CreateObject();
            this.mConfiguration = Misc.CheckNotNull<PlayGamesClientConfiguration>(configuration);
            this.clientImpl = clientImpl;
            this.rationale = configuration.PermissionRationale;
            if (string.IsNullOrEmpty(this.rationale))
            {
                this.rationale = "Select email address to send to this game or hit cancel to not share.";
            }
        }

        [CompilerGenerated]
        private static void <AsOnGameThreadCallback`1>m__A1<T>(T)
        {
        }

        private static Action<T> AsOnGameThreadCallback<T>(Action<T> callback)
        {
            <AsOnGameThreadCallback>c__AnonStorey27A<T> storeya = new <AsOnGameThreadCallback>c__AnonStorey27A<T>();
            storeya.callback = callback;
            if (storeya.callback == null)
            {
                return new Action<T>(NativeClient.<AsOnGameThreadCallback`1>m__A1<T>);
            }
            return new Action<T>(storeya.<>m__A2);
        }

        public void Authenticate(Action<bool> callback, bool silent)
        {
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                if (this.mAuthState == 1)
                {
                    InvokeCallbackOnGameThread<bool>(callback, true);
                    return;
                }
                if (this.mSilentAuthFailed && silent)
                {
                    InvokeCallbackOnGameThread<bool>(callback, false);
                    return;
                }
                if (callback != null)
                {
                    if (silent)
                    {
                        this.mSilentAuthCallbacks = (Action<bool>) Delegate.Combine(this.mSilentAuthCallbacks, callback);
                    }
                    else
                    {
                        this.mPendingAuthCallbacks = (Action<bool>) Delegate.Combine(this.mPendingAuthCallbacks, callback);
                    }
                }
            }
            this.InitializeGameServices();
            this.friendsLoading = false;
            if (!silent)
            {
                this.GameServices().StartAuthorizationUI();
            }
        }

        private GooglePlayGames.Native.PInvoke.GameServices GameServices()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mServices;
            }
        }

        [Obsolete("Use GetServerAuthCode() then exchange it for a token")]
        public string GetAccessToken()
        {
            if (!this.IsAuthenticated())
            {
                Debug.Log("Cannot get API client - not authenticated");
                return null;
            }
            if (!GameInfo.WebClientIdInitialized())
            {
                if ((this.noWebClientIdWarningCount++ % this.webclientWarningFreq) == 0)
                {
                    Debug.LogError("Web client ID has not been set, cannot request access token.");
                    this.noWebClientIdWarningCount = (this.noWebClientIdWarningCount / this.webclientWarningFreq) + 1;
                }
                return null;
            }
            this.mTokenClient.SetRationale(this.rationale);
            return this.mTokenClient.GetAccessToken();
        }

        public GooglePlayGames.BasicApi.Achievement GetAchievement(string achId)
        {
            if ((this.mAchievements != null) && this.mAchievements.ContainsKey(achId))
            {
                return this.mAchievements[achId];
            }
            return null;
        }

        public IntPtr GetApiClient()
        {
            return InternalHooks.InternalHooks_GetApiClient(this.mServices.AsHandle());
        }

        public IEventsClient GetEventsClient()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mEventsClient;
            }
        }

        public IUserProfile[] GetFriends()
        {
            if ((this.mFriends == null) && !this.friendsLoading)
            {
                Logger.w("Getting friends before they are loaded!!!");
                this.friendsLoading = true;
                this.LoadFriends(delegate (bool ok) {
                    Logger.d(string.Concat(new object[] { "loading: ", ok, " mFriends = ", this.mFriends }));
                    if (!ok)
                    {
                        Logger.e("Friends list did not load successfully.  Disabling loading until re-authenticated");
                    }
                    this.friendsLoading = !ok;
                });
            }
            return ((this.mFriends != null) ? ((IUserProfile[]) this.mFriends.ToArray()) : new IUserProfile[0]);
        }

        [Obsolete("Use GetServerAuthCode() then exchange it for a token")]
        public void GetIdToken(Action<string> idTokenCallback)
        {
            <GetIdToken>c__AnonStorey27F storeyf = new <GetIdToken>c__AnonStorey27F();
            storeyf.idTokenCallback = idTokenCallback;
            if (!this.IsAuthenticated())
            {
                Debug.Log("Cannot get API client - not authenticated");
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyf.<>m__A8));
            }
            if (!GameInfo.WebClientIdInitialized())
            {
                if ((this.noWebClientIdWarningCount++ % this.webclientWarningFreq) == 0)
                {
                    Debug.LogError("Web client ID has not been set, cannot request id token.");
                    this.noWebClientIdWarningCount = (this.noWebClientIdWarningCount / this.webclientWarningFreq) + 1;
                }
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyf.<>m__A9));
            }
            this.mTokenClient.SetRationale(this.rationale);
            this.mTokenClient.GetIdToken("995406906094-gv9jtvln7nl7b44p7od2agqr983j3jdu.apps.googleusercontent.com", AsOnGameThreadCallback<string>(storeyf.idTokenCallback));
        }

        public void GetPlayerStats(Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback)
        {
            <GetPlayerStats>c__AnonStorey285 storey = new <GetPlayerStats>c__AnonStorey285();
            storey.callback = callback;
            storey.<>f__this = this;
            PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__B1));
        }

        public IQuestsClient GetQuestsClient()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mQuestsClient;
            }
        }

        public IRealTimeMultiplayerClient GetRtmpClient()
        {
            if (!this.IsAuthenticated())
            {
                return null;
            }
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mRealTimeClient;
            }
        }

        public ISavedGameClient GetSavedGameClient()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mSavedGameClient;
            }
        }

        public void GetServerAuthCode(string serverClientId, Action<CommonStatusCodes, string> callback)
        {
            <GetServerAuthCode>c__AnonStorey280 storey = new <GetServerAuthCode>c__AnonStorey280();
            storey.callback = callback;
            this.mServices.FetchServerAuthCode(serverClientId, new Action<GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse>(storey.<>m__AA));
        }

        public ITurnBasedMultiplayerClient GetTbmpClient()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                return this.mTurnBasedClient;
            }
        }

        public string GetToken()
        {
            if (this.mTokenClient != null)
            {
                return this.mTokenClient.GetAccessToken();
            }
            return null;
        }

        public string GetUserDisplayName()
        {
            if (this.mUser == null)
            {
                return null;
            }
            return this.mUser.userName;
        }

        public string GetUserEmail()
        {
            if (!this.IsAuthenticated())
            {
                Debug.Log("Cannot get API client - not authenticated");
                return null;
            }
            this.mTokenClient.SetRationale(this.rationale);
            return this.mTokenClient.GetEmail();
        }

        public void GetUserEmail(Action<CommonStatusCodes, string> callback)
        {
            <GetUserEmail>c__AnonStorey27D storeyd = new <GetUserEmail>c__AnonStorey27D();
            storeyd.callback = callback;
            if (!this.IsAuthenticated())
            {
                Debug.Log("Cannot get API client - not authenticated");
                if (storeyd.callback != null)
                {
                    PlayGamesHelperObject.RunOnGameThread(new Action(storeyd.<>m__A6));
                    return;
                }
            }
            this.mTokenClient.SetRationale(this.rationale);
            this.mTokenClient.GetEmail(new Action<CommonStatusCodes, string>(storeyd.<>m__A7));
        }

        public string GetUserId()
        {
            if (this.mUser == null)
            {
                return null;
            }
            return this.mUser.id;
        }

        public string GetUserImageUrl()
        {
            if (this.mUser == null)
            {
                return null;
            }
            return this.mUser.AvatarURL;
        }

        private void HandleAuthTransition(GooglePlayGames.Native.Cwrapper.Types.AuthOperation operation, GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus status)
        {
            Logger.d(string.Concat(new object[] { "Starting Auth Transition. Op: ", operation, " status: ", status }));
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                GooglePlayGames.Native.Cwrapper.Types.AuthOperation operation2 = operation;
                if (operation2 != GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_IN)
                {
                    if (operation2 == GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_OUT)
                    {
                        goto Label_019F;
                    }
                    goto Label_01AA;
                }
                if (status == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus.VALID)
                {
                    <HandleAuthTransition>c__AnonStorey284 storey = new <HandleAuthTransition>c__AnonStorey284();
                    storey.<>f__this = this;
                    if (this.mSilentAuthCallbacks != null)
                    {
                        this.mPendingAuthCallbacks = (Action<bool>) Delegate.Combine(this.mPendingAuthCallbacks, this.mSilentAuthCallbacks);
                        this.mSilentAuthCallbacks = null;
                    }
                    storey.currentAuthGeneration = this.mAuthGeneration;
                    this.mServices.AchievementManager().FetchAll(new Action<GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse>(storey.<>m__AF));
                    this.mServices.PlayerManager().FetchSelf(new Action<GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse>(storey.<>m__B0));
                }
                else if (this.mAuthState == 2)
                {
                    this.mSilentAuthFailed = true;
                    this.mAuthState = 0;
                    Action<bool> mSilentAuthCallbacks = this.mSilentAuthCallbacks;
                    this.mSilentAuthCallbacks = null;
                    Debug.Log("Invoking callbacks, AuthState changed from silentPending to Unauthenticated.");
                    InvokeCallbackOnGameThread<bool>(mSilentAuthCallbacks, false);
                    if (this.mPendingAuthCallbacks != null)
                    {
                        Debug.Log("there are pending auth callbacks - starting AuthUI");
                        this.GameServices().StartAuthorizationUI();
                    }
                }
                else
                {
                    Debug.Log("AuthState == " + ((AuthState) this.mAuthState) + " calling auth callbacks with failure");
                    this.UnpauseUnityPlayer();
                    Action<bool> mPendingAuthCallbacks = this.mPendingAuthCallbacks;
                    this.mPendingAuthCallbacks = null;
                    InvokeCallbackOnGameThread<bool>(mPendingAuthCallbacks, false);
                }
                goto Label_01D0;
            Label_019F:
                this.ToUnauthenticated();
                goto Label_01D0;
            Label_01AA:
                Logger.e("Unknown AuthOperation " + operation);
            Label_01D0:;
            }
        }

        internal void HandleInvitation(GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string invitationId, GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
        {
            <HandleInvitation>c__AnonStorey27C storeyc = new <HandleInvitation>c__AnonStorey27C();
            storeyc.currentHandler = this.mInvitationDelegate;
            if (storeyc.currentHandler == null)
            {
                Logger.d(string.Concat(new object[] { "Received ", eventType, " for invitation ", invitationId, " but no handler was registered." }));
            }
            else if (eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.REMOVED)
            {
                Logger.d("Ignoring REMOVED for invitation " + invitationId);
            }
            else
            {
                storeyc.shouldAutolaunch = eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
                storeyc.invite = invitation.AsInvitation();
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyc.<>m__A5));
            }
        }

        public void IncrementAchievement(string achId, int steps, Action<bool> callback)
        {
            <IncrementAchievement>c__AnonStorey28C storeyc = new <IncrementAchievement>c__AnonStorey28C();
            storeyc.achId = achId;
            storeyc.callback = callback;
            storeyc.<>f__this = this;
            Misc.CheckNotNull<string>(storeyc.achId);
            storeyc.callback = AsOnGameThreadCallback<bool>(storeyc.callback);
            this.InitializeGameServices();
            GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(storeyc.achId);
            if (achievement == null)
            {
                Logger.e("Could not increment, no achievement with ID " + storeyc.achId);
                storeyc.callback(false);
            }
            else if (!achievement.IsIncremental)
            {
                Logger.e("Could not increment, achievement with ID " + storeyc.achId + " was not incremental");
                storeyc.callback(false);
            }
            else if (steps < 0)
            {
                Logger.e("Attempted to increment by negative steps");
                storeyc.callback(false);
            }
            else
            {
                this.GameServices().AchievementManager().Increment(storeyc.achId, Convert.ToUInt32(steps));
                this.GameServices().AchievementManager().Fetch(storeyc.achId, new Action<GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse>(storeyc.<>m__B9));
            }
        }

        private void InitializeGameServices()
        {
            object gameServicesLock = this.GameServicesLock;
            lock (gameServicesLock)
            {
                if (this.mServices == null)
                {
                    using (GameServicesBuilder builder = GameServicesBuilder.Create())
                    {
                        using (PlatformConfiguration configuration = this.clientImpl.CreatePlatformConfiguration())
                        {
                            this.RegisterInvitationDelegate(this.mConfiguration.InvitationDelegate);
                            builder.SetOnAuthFinishedCallback(new GameServicesBuilder.AuthFinishedCallback(this.HandleAuthTransition));
                            builder.SetOnTurnBasedMatchEventCallback(delegate (GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string matchId, NativeTurnBasedMatch match) {
                                this.mTurnBasedClient.HandleMatchEvent(eventType, matchId, match);
                            });
                            builder.SetOnMultiplayerInvitationEventCallback(new Action<GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent, string, GooglePlayGames.Native.PInvoke.MultiplayerInvitation>(this.HandleInvitation));
                            if (this.mConfiguration.EnableSavedGames)
                            {
                                builder.EnableSnapshots();
                            }
                            if (this.mConfiguration.RequireGooglePlus)
                            {
                                builder.RequireGooglePlus();
                            }
                            Debug.Log("Building GPG services, implicitly attempts silent auth");
                            this.mAuthState = 2;
                            this.mServices = builder.Build(configuration);
                            this.mEventsClient = new NativeEventClient(new GooglePlayGames.Native.PInvoke.EventManager(this.mServices));
                            this.mQuestsClient = new NativeQuestClient(new GooglePlayGames.Native.PInvoke.QuestManager(this.mServices));
                            this.mTurnBasedClient = new NativeTurnBasedMultiplayerClient(this, new TurnBasedManager(this.mServices));
                            this.mTurnBasedClient.RegisterMatchDelegate(this.mConfiguration.MatchDelegate);
                            this.mRealTimeClient = new NativeRealtimeMultiplayerClient(this, new RealtimeManager(this.mServices));
                            if (this.mConfiguration.EnableSavedGames)
                            {
                                this.mSavedGameClient = new NativeSavedGameClient(new GooglePlayGames.Native.PInvoke.SnapshotManager(this.mServices));
                            }
                            else
                            {
                                this.mSavedGameClient = new UnsupportedSavedGamesClient("You must enable saved games before it can be used. See PlayGamesClientConfiguration.Builder.EnableSavedGames.");
                            }
                            this.mAuthState = 2;
                            this.mTokenClient = this.clientImpl.CreateTokenClient((this.mUser != null) ? this.mUser.id : null, false);
                        }
                    }
                }
            }
        }

        private static void InvokeCallbackOnGameThread<T>(Action<T> callback, T data)
        {
            <InvokeCallbackOnGameThread>c__AnonStorey27B<T> storeyb = new <InvokeCallbackOnGameThread>c__AnonStorey27B<T>();
            storeyb.callback = callback;
            storeyb.data = data;
            if (storeyb.callback != null)
            {
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyb.<>m__A3));
            }
        }

        public bool IsAuthenticated()
        {
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                return (this.mAuthState == 1);
            }
        }

        public int LeaderboardMaxResults()
        {
            return this.GameServices().LeaderboardManager().LeaderboardMaxResults;
        }

        public void LoadAchievements(Action<GooglePlayGames.BasicApi.Achievement[]> callback)
        {
            <LoadAchievements>c__AnonStorey288 storey = new <LoadAchievements>c__AnonStorey288();
            storey.callback = callback;
            storey.data = new GooglePlayGames.BasicApi.Achievement[this.mAchievements.Count];
            this.mAchievements.Values.CopyTo(storey.data, 0);
            PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__B3));
        }

        public void LoadFriends(Action<bool> callback)
        {
            <LoadFriends>c__AnonStorey283 storey = new <LoadFriends>c__AnonStorey283();
            storey.callback = callback;
            storey.<>f__this = this;
            if (!this.IsAuthenticated())
            {
                Logger.d("Cannot loadFriends when not authenticated");
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__AB));
            }
            else if (this.mFriends != null)
            {
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__AC));
            }
            else
            {
                this.mServices.PlayerManager().FetchFriends(new Action<GooglePlayGames.BasicApi.ResponseStatus, List<GooglePlayGames.BasicApi.Multiplayer.Player>>(storey.<>m__AD));
            }
        }

        public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
        {
            callback = AsOnGameThreadCallback<LeaderboardScoreData>(callback);
            this.GameServices().LeaderboardManager().LoadScorePage(null, rowCount, token, callback);
        }

        public void LoadScores(string leaderboardId, GooglePlayGames.BasicApi.LeaderboardStart start, int rowCount, GooglePlayGames.BasicApi.LeaderboardCollection collection, GooglePlayGames.BasicApi.LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
        {
            callback = AsOnGameThreadCallback<LeaderboardScoreData>(callback);
            this.GameServices().LeaderboardManager().LoadLeaderboardData(leaderboardId, start, rowCount, collection, timeSpan, this.mUser.id, callback);
        }

        public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
        {
            <LoadUsers>c__AnonStorey286 storey = new <LoadUsers>c__AnonStorey286();
            storey.callback = callback;
            this.mServices.PlayerManager().FetchList(userIds, new Action<NativePlayer[]>(storey.<>m__B2));
        }

        private void MaybeFinishAuthentication()
        {
            Action<bool> callback = null;
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                if ((this.mUser == null) || (this.mAchievements == null))
                {
                    Logger.d(string.Concat(new object[] { "Auth not finished. User=", this.mUser, " achievements=", this.mAchievements }));
                    return;
                }
                Logger.d("Auth finished. Proceeding.");
                callback = this.mPendingAuthCallbacks;
                this.mPendingAuthCallbacks = null;
                this.mAuthState = 1;
            }
            if (callback != null)
            {
                Logger.d("Invoking Callbacks: " + callback);
                InvokeCallbackOnGameThread<bool>(callback, true);
            }
        }

        private void PopulateAchievements(uint authGeneration, GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse response)
        {
            if (authGeneration != this.mAuthGeneration)
            {
                Logger.d("Received achievement callback after signout occurred, ignoring");
            }
            else
            {
                Logger.d("Populating Achievements, status = " + response.Status());
                object authStateLock = this.AuthStateLock;
                lock (authStateLock)
                {
                    if ((response.Status() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) && (response.Status() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
                    {
                        Logger.e("Error retrieving achievements - check the log for more information. Failing signin.");
                        Action<bool> mPendingAuthCallbacks = this.mPendingAuthCallbacks;
                        this.mPendingAuthCallbacks = null;
                        if (mPendingAuthCallbacks != null)
                        {
                            InvokeCallbackOnGameThread<bool>(mPendingAuthCallbacks, false);
                        }
                        this.SignOut();
                        return;
                    }
                    Dictionary<string, GooglePlayGames.BasicApi.Achievement> dictionary = new Dictionary<string, GooglePlayGames.BasicApi.Achievement>();
                    IEnumerator<NativeAchievement> enumerator = response.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            NativeAchievement current = enumerator.Current;
                            using (current)
                            {
                                dictionary[current.Id()] = current.AsAchievement();
                                continue;
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator == null)
                        {
                        }
                        enumerator.Dispose();
                    }
                    Logger.d("Found " + dictionary.Count + " Achievements");
                    this.mAchievements = dictionary;
                }
                Logger.d("Maybe finish for Achievements");
                this.MaybeFinishAuthentication();
            }
        }

        private void PopulateUser(uint authGeneration, GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse response)
        {
            Logger.d("Populating User");
            if (authGeneration != this.mAuthGeneration)
            {
                Logger.d("Received user callback after signout occurred, ignoring");
            }
            else
            {
                object authStateLock = this.AuthStateLock;
                lock (authStateLock)
                {
                    if ((response.Status() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) && (response.Status() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
                    {
                        Logger.e("Error retrieving user, signing out");
                        Action<bool> mPendingAuthCallbacks = this.mPendingAuthCallbacks;
                        this.mPendingAuthCallbacks = null;
                        if (mPendingAuthCallbacks != null)
                        {
                            InvokeCallbackOnGameThread<bool>(mPendingAuthCallbacks, false);
                        }
                        this.SignOut();
                        return;
                    }
                    this.mUser = response.Self().AsPlayer();
                    this.mFriends = null;
                    this.mTokenClient = this.clientImpl.CreateTokenClient(this.mUser.id, true);
                }
                Logger.d("Found User: " + this.mUser);
                Logger.d("Maybe finish for User");
                this.MaybeFinishAuthentication();
            }
        }

        public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
        {
            <RegisterInvitationDelegate>c__AnonStorey290 storey = new <RegisterInvitationDelegate>c__AnonStorey290();
            storey.invitationDelegate = invitationDelegate;
            if (storey.invitationDelegate == null)
            {
                this.mInvitationDelegate = null;
            }
            else
            {
                this.mInvitationDelegate = Callbacks.AsOnGameThreadCallback<Invitation, bool>(new Action<Invitation, bool>(storey.<>m__BD));
            }
        }

        public void RevealAchievement(string achId, Action<bool> callback)
        {
            <RevealAchievement>c__AnonStorey28A storeya = new <RevealAchievement>c__AnonStorey28A();
            storeya.achId = achId;
            storeya.<>f__this = this;
            if (<>f__am$cache19 == null)
            {
                <>f__am$cache19 = delegate (GooglePlayGames.BasicApi.Achievement a) {
                    return a.IsRevealed;
                };
            }
            this.UpdateAchievement("Reveal", storeya.achId, callback, <>f__am$cache19, new Action<GooglePlayGames.BasicApi.Achievement>(storeya.<>m__B7));
        }

        public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
        {
            <SetStepsAtLeast>c__AnonStorey28D storeyd = new <SetStepsAtLeast>c__AnonStorey28D();
            storeyd.achId = achId;
            storeyd.callback = callback;
            storeyd.<>f__this = this;
            Misc.CheckNotNull<string>(storeyd.achId);
            storeyd.callback = AsOnGameThreadCallback<bool>(storeyd.callback);
            this.InitializeGameServices();
            GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(storeyd.achId);
            if (achievement == null)
            {
                Logger.e("Could not increment, no achievement with ID " + storeyd.achId);
                storeyd.callback(false);
            }
            else if (!achievement.IsIncremental)
            {
                Logger.e("Could not increment, achievement with ID " + storeyd.achId + " is not incremental");
                storeyd.callback(false);
            }
            else if (steps < 0)
            {
                Logger.e("Attempted to increment by negative steps");
                storeyd.callback(false);
            }
            else
            {
                this.GameServices().AchievementManager().SetStepsAtLeast(storeyd.achId, Convert.ToUInt32(steps));
                this.GameServices().AchievementManager().Fetch(storeyd.achId, new Action<GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse>(storeyd.<>m__BA));
            }
        }

        public void ShowAchievementsUI(Action<GooglePlayGames.BasicApi.UIStatus> cb)
        {
            <ShowAchievementsUI>c__AnonStorey28E storeye = new <ShowAchievementsUI>c__AnonStorey28E();
            storeye.cb = cb;
            if (this.IsAuthenticated())
            {
                Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> noopUICallback = Callbacks.NoopUICallback;
                if (storeye.cb != null)
                {
                    noopUICallback = new Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>(storeye.<>m__BB);
                }
                noopUICallback = AsOnGameThreadCallback<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>(noopUICallback);
                this.GameServices().AchievementManager().ShowAllUI(noopUICallback);
            }
        }

        public void ShowLeaderboardUI(string leaderboardId, GooglePlayGames.BasicApi.LeaderboardTimeSpan span, Action<GooglePlayGames.BasicApi.UIStatus> cb)
        {
            <ShowLeaderboardUI>c__AnonStorey28F storeyf = new <ShowLeaderboardUI>c__AnonStorey28F();
            storeyf.cb = cb;
            if (this.IsAuthenticated())
            {
                Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> noopUICallback = Callbacks.NoopUICallback;
                if (storeyf.cb != null)
                {
                    noopUICallback = new Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>(storeyf.<>m__BC);
                }
                noopUICallback = AsOnGameThreadCallback<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>(noopUICallback);
                if (leaderboardId == null)
                {
                    this.GameServices().LeaderboardManager().ShowAllUI(noopUICallback);
                }
                else
                {
                    this.GameServices().LeaderboardManager().ShowUI(leaderboardId, span, noopUICallback);
                }
            }
        }

        public void SignOut()
        {
            this.ToUnauthenticated();
            if (this.GameServices() != null)
            {
                this.GameServices().SignOut();
            }
        }

        public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
        {
            callback = AsOnGameThreadCallback<bool>(callback);
            if (!this.IsAuthenticated())
            {
                callback(false);
            }
            this.InitializeGameServices();
            if (leaderboardId == null)
            {
                throw new ArgumentNullException("leaderboardId");
            }
            this.GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, null);
            callback(true);
        }

        public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
        {
            callback = AsOnGameThreadCallback<bool>(callback);
            if (!this.IsAuthenticated())
            {
                callback(false);
            }
            this.InitializeGameServices();
            if (leaderboardId == null)
            {
                throw new ArgumentNullException("leaderboardId");
            }
            this.GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, metadata);
            callback(true);
        }

        private void ToUnauthenticated()
        {
            object authStateLock = this.AuthStateLock;
            lock (authStateLock)
            {
                this.mUser = null;
                this.mFriends = null;
                this.mAchievements = null;
                this.mAuthState = 0;
                this.mTokenClient = this.clientImpl.CreateTokenClient(null, true);
                this.mAuthGeneration++;
            }
        }

        public void UnlockAchievement(string achId, Action<bool> callback)
        {
            <UnlockAchievement>c__AnonStorey289 storey = new <UnlockAchievement>c__AnonStorey289();
            storey.achId = achId;
            storey.<>f__this = this;
            if (<>f__am$cache18 == null)
            {
                <>f__am$cache18 = delegate (GooglePlayGames.BasicApi.Achievement a) {
                    return a.IsUnlocked;
                };
            }
            this.UpdateAchievement("Unlock", storey.achId, callback, <>f__am$cache18, new Action<GooglePlayGames.BasicApi.Achievement>(storey.<>m__B5));
        }

        private void UnpauseUnityPlayer()
        {
        }

        private void UpdateAchievement(string updateType, string achId, Action<bool> callback, Predicate<GooglePlayGames.BasicApi.Achievement> alreadyDone, Action<GooglePlayGames.BasicApi.Achievement> updateAchievment)
        {
            <UpdateAchievement>c__AnonStorey28B storeyb = new <UpdateAchievement>c__AnonStorey28B();
            storeyb.achId = achId;
            storeyb.callback = callback;
            storeyb.<>f__this = this;
            storeyb.callback = AsOnGameThreadCallback<bool>(storeyb.callback);
            Misc.CheckNotNull<string>(storeyb.achId);
            this.InitializeGameServices();
            GooglePlayGames.BasicApi.Achievement achievement = this.GetAchievement(storeyb.achId);
            if (achievement == null)
            {
                Logger.d("Could not " + updateType + ", no achievement with ID " + storeyb.achId);
                storeyb.callback(false);
            }
            else if (alreadyDone(achievement))
            {
                Logger.d("Did not need to perform " + updateType + ": on achievement " + storeyb.achId);
                storeyb.callback(true);
            }
            else
            {
                Logger.d("Performing " + updateType + " on " + storeyb.achId);
                updateAchievment(achievement);
                this.GameServices().AchievementManager().Fetch(storeyb.achId, new Action<GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse>(storeyb.<>m__B8));
            }
        }

        [CompilerGenerated]
        private sealed class <AsOnGameThreadCallback>c__AnonStorey27A<T>
        {
            internal Action<T> callback;

            internal void <>m__A2(T result)
            {
                NativeClient.InvokeCallbackOnGameThread<T>(this.callback, result);
            }
        }

        [CompilerGenerated]
        private sealed class <GetIdToken>c__AnonStorey27F
        {
            internal Action<string> idTokenCallback;

            internal void <>m__A8()
            {
                this.idTokenCallback(null);
            }

            internal void <>m__A9()
            {
                this.idTokenCallback(null);
            }
        }

        [CompilerGenerated]
        private sealed class <GetPlayerStats>c__AnonStorey285
        {
            internal NativeClient <>f__this;
            internal Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback;

            internal void <>m__B1()
            {
                this.<>f__this.clientImpl.GetPlayerStats(this.<>f__this.GetApiClient(), this.callback);
            }
        }

        [CompilerGenerated]
        private sealed class <GetServerAuthCode>c__AnonStorey280
        {
            internal Action<CommonStatusCodes, string> callback;

            internal void <>m__AA(GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse serverAuthCodeResponse)
            {
                <GetServerAuthCode>c__AnonStorey281 storey = new <GetServerAuthCode>c__AnonStorey281();
                storey.<>f__ref$640 = this;
                storey.responseCode = ConversionUtils.ConvertResponseStatusToCommonStatus(serverAuthCodeResponse.Status());
                if ((storey.responseCode != CommonStatusCodes.Success) && (storey.responseCode != CommonStatusCodes.SuccessCached))
                {
                    Logger.e("Error loading server auth code: " + serverAuthCodeResponse.Status().ToString());
                }
                if (this.callback != null)
                {
                    <GetServerAuthCode>c__AnonStorey282 storey2 = new <GetServerAuthCode>c__AnonStorey282();
                    storey2.<>f__ref$640 = this;
                    storey2.<>f__ref$641 = storey;
                    storey2.authCode = serverAuthCodeResponse.Code();
                    PlayGamesHelperObject.RunOnGameThread(new Action(storey2.<>m__BF));
                }
            }

            private sealed class <GetServerAuthCode>c__AnonStorey281
            {
                internal NativeClient.<GetServerAuthCode>c__AnonStorey280 <>f__ref$640;
                internal CommonStatusCodes responseCode;
            }

            private sealed class <GetServerAuthCode>c__AnonStorey282
            {
                internal NativeClient.<GetServerAuthCode>c__AnonStorey280 <>f__ref$640;
                internal NativeClient.<GetServerAuthCode>c__AnonStorey280.<GetServerAuthCode>c__AnonStorey281 <>f__ref$641;
                internal string authCode;

                internal void <>m__BF()
                {
                    this.<>f__ref$640.callback(this.<>f__ref$641.responseCode, this.authCode);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetUserEmail>c__AnonStorey27D
        {
            internal Action<CommonStatusCodes, string> callback;

            internal void <>m__A6()
            {
                this.callback(CommonStatusCodes.SignInRequired, null);
            }

            internal void <>m__A7(CommonStatusCodes status, string email)
            {
                <GetUserEmail>c__AnonStorey27E storeye = new <GetUserEmail>c__AnonStorey27E();
                storeye.<>f__ref$637 = this;
                storeye.status = status;
                storeye.email = email;
                PlayGamesHelperObject.RunOnGameThread(new Action(storeye.<>m__BE));
            }

            private sealed class <GetUserEmail>c__AnonStorey27E
            {
                internal NativeClient.<GetUserEmail>c__AnonStorey27D <>f__ref$637;
                internal string email;
                internal CommonStatusCodes status;

                internal void <>m__BE()
                {
                    this.<>f__ref$637.callback(this.status, this.email);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <HandleAuthTransition>c__AnonStorey284
        {
            internal NativeClient <>f__this;
            internal uint currentAuthGeneration;

            internal void <>m__AF(GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse results)
            {
                this.<>f__this.PopulateAchievements(this.currentAuthGeneration, results);
            }

            internal void <>m__B0(GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse results)
            {
                this.<>f__this.PopulateUser(this.currentAuthGeneration, results);
            }
        }

        [CompilerGenerated]
        private sealed class <HandleInvitation>c__AnonStorey27C
        {
            internal Action<Invitation, bool> currentHandler;
            internal Invitation invite;
            internal bool shouldAutolaunch;

            internal void <>m__A5()
            {
                this.currentHandler(this.invite, this.shouldAutolaunch);
            }
        }

        [CompilerGenerated]
        private sealed class <IncrementAchievement>c__AnonStorey28C
        {
            internal NativeClient <>f__this;
            internal string achId;
            internal Action<bool> callback;

            internal void <>m__B9(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
            {
                if (rsp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID)
                {
                    this.<>f__this.mAchievements.Remove(this.achId);
                    this.<>f__this.mAchievements.Add(this.achId, rsp.Achievement().AsAchievement());
                    this.callback(true);
                }
                else
                {
                    Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", this.achId, ": ", rsp.Status() }));
                    this.callback(false);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <InvokeCallbackOnGameThread>c__AnonStorey27B<T>
        {
            internal Action<T> callback;
            internal T data;

            internal void <>m__A3()
            {
                Logger.d("Invoking user callback on game thread");
                this.callback(this.data);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAchievements>c__AnonStorey288
        {
            internal Action<GooglePlayGames.BasicApi.Achievement[]> callback;
            internal GooglePlayGames.BasicApi.Achievement[] data;

            internal void <>m__B3()
            {
                this.callback(this.data);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadFriends>c__AnonStorey283
        {
            internal NativeClient <>f__this;
            internal Action<bool> callback;

            internal void <>m__AB()
            {
                this.callback(false);
            }

            internal void <>m__AC()
            {
                this.callback(true);
            }

            internal void <>m__AD(GooglePlayGames.BasicApi.ResponseStatus status, List<GooglePlayGames.BasicApi.Multiplayer.Player> players)
            {
                if ((status == GooglePlayGames.BasicApi.ResponseStatus.Success) || (status == GooglePlayGames.BasicApi.ResponseStatus.SuccessWithStale))
                {
                    this.<>f__this.mFriends = players;
                    PlayGamesHelperObject.RunOnGameThread(delegate {
                        this.callback(true);
                    });
                }
                else
                {
                    this.<>f__this.mFriends = new List<GooglePlayGames.BasicApi.Multiplayer.Player>();
                    Logger.e("Got " + status + " loading friends");
                    PlayGamesHelperObject.RunOnGameThread(delegate {
                        this.callback(false);
                    });
                }
            }

            internal void <>m__C0()
            {
                this.callback(true);
            }

            internal void <>m__C1()
            {
                this.callback(false);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadUsers>c__AnonStorey286
        {
            internal Action<IUserProfile[]> callback;

            internal void <>m__B2(NativePlayer[] nativeUsers)
            {
                <LoadUsers>c__AnonStorey287 storey = new <LoadUsers>c__AnonStorey287();
                storey.<>f__ref$646 = this;
                storey.users = new IUserProfile[nativeUsers.Length];
                for (int i = 0; i < storey.users.Length; i++)
                {
                    storey.users[i] = nativeUsers[i].AsPlayer();
                }
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__C2));
            }

            private sealed class <LoadUsers>c__AnonStorey287
            {
                internal NativeClient.<LoadUsers>c__AnonStorey286 <>f__ref$646;
                internal IUserProfile[] users;

                internal void <>m__C2()
                {
                    this.<>f__ref$646.callback(this.users);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterInvitationDelegate>c__AnonStorey290
        {
            internal InvitationReceivedDelegate invitationDelegate;

            internal void <>m__BD(Invitation invitation, bool autoAccept)
            {
                this.invitationDelegate(invitation, autoAccept);
            }
        }

        [CompilerGenerated]
        private sealed class <RevealAchievement>c__AnonStorey28A
        {
            internal NativeClient <>f__this;
            internal string achId;

            internal void <>m__B7(GooglePlayGames.BasicApi.Achievement a)
            {
                a.IsRevealed = true;
                this.<>f__this.GameServices().AchievementManager().Reveal(this.achId);
            }
        }

        [CompilerGenerated]
        private sealed class <SetStepsAtLeast>c__AnonStorey28D
        {
            internal NativeClient <>f__this;
            internal string achId;
            internal Action<bool> callback;

            internal void <>m__BA(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
            {
                if (rsp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID)
                {
                    this.<>f__this.mAchievements.Remove(this.achId);
                    this.<>f__this.mAchievements.Add(this.achId, rsp.Achievement().AsAchievement());
                    this.callback(true);
                }
                else
                {
                    Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", this.achId, ": ", rsp.Status() }));
                    this.callback(false);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ShowAchievementsUI>c__AnonStorey28E
        {
            internal Action<GooglePlayGames.BasicApi.UIStatus> cb;

            internal void <>m__BB(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus result)
            {
                this.cb((GooglePlayGames.BasicApi.UIStatus) result);
            }
        }

        [CompilerGenerated]
        private sealed class <ShowLeaderboardUI>c__AnonStorey28F
        {
            internal Action<GooglePlayGames.BasicApi.UIStatus> cb;

            internal void <>m__BC(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus result)
            {
                this.cb((GooglePlayGames.BasicApi.UIStatus) result);
            }
        }

        [CompilerGenerated]
        private sealed class <UnlockAchievement>c__AnonStorey289
        {
            internal NativeClient <>f__this;
            internal string achId;

            internal void <>m__B5(GooglePlayGames.BasicApi.Achievement a)
            {
                a.IsUnlocked = true;
                this.<>f__this.GameServices().AchievementManager().Unlock(this.achId);
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAchievement>c__AnonStorey28B
        {
            internal NativeClient <>f__this;
            internal string achId;
            internal Action<bool> callback;

            internal void <>m__B8(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
            {
                if (rsp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID)
                {
                    this.<>f__this.mAchievements.Remove(this.achId);
                    this.<>f__this.mAchievements.Add(this.achId, rsp.Achievement().AsAchievement());
                    this.callback(true);
                }
                else
                {
                    Logger.e(string.Concat(new object[] { "Cannot refresh achievement ", this.achId, ": ", rsp.Status() }));
                    this.callback(false);
                }
            }
        }

        private enum AuthState
        {
            Unauthenticated,
            Authenticated,
            SilentPending
        }
    }
}

