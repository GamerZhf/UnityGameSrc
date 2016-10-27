namespace Service
{
    using App;
    using GameLogic;
    using PlayerView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class PlayerService : MonoBehaviour
    {
        [CompilerGenerated]
        private bool <HadRestoreError>k__BackingField;
        [CompilerGenerated]
        private string <PendingClientUpdateFromUrl>k__BackingField;
        private float lastSyncTime;
        private bool loggedIn;
        private LoginTask loginTask;
        private float nextSyncTime;
        private static float SESSION_TIMEOUT = 600f;

        [DebuggerHidden]
        public IEnumerator DetachGameState()
        {
            return new <DetachGameState>c__Iterator225();
        }

        public void FlagForSync()
        {
            this.nextSyncTime = Time.time + 3f;
        }

        private void Log(string msg)
        {
            Service.Binder.Logger.Log(msg);
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnPlayerRankUpped -= new GameLogic.Events.PlayerRankUpped(this.onSyncEvent<bool>);
            GameLogic.Binder.EventBus.OnRoomCompleted -= new GameLogic.Events.RoomCompleted(this.onRoomComplete);
            GameLogic.Binder.EventBus.OnPlayerRetired -= new GameLogic.Events.PlayerRetired(this.onSyncEvent<int>);
            PlayerView.Binder.EventBus.OnAdWatched -= new PlayerView.Events.AdWatched(this.onSyncEvent);
            Service.Binder.EventBus.OnIapShopPurchase -= new Service.Events.IapShopPurchase(this.onSyncEvent);
            App.Binder.EventBus.OnPlatformProfileUpdated -= new App.Events.PlatformProfileUpdated(this.OnSocialConnect);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnPlayerRankUpped += new GameLogic.Events.PlayerRankUpped(this.onSyncEvent<bool>);
            GameLogic.Binder.EventBus.OnRoomCompleted += new GameLogic.Events.RoomCompleted(this.onRoomComplete);
            GameLogic.Binder.EventBus.OnPlayerRetired += new GameLogic.Events.PlayerRetired(this.onSyncEvent<int>);
            PlayerView.Binder.EventBus.OnAdWatched += new PlayerView.Events.AdWatched(this.onSyncEvent);
            Service.Binder.EventBus.OnIapShopPurchase += new Service.Events.IapShopPurchase(this.onSyncEvent);
            App.Binder.EventBus.OnPlatformProfileUpdated += new App.Events.PlatformProfileUpdated(this.OnSocialConnect);
        }

        public void OnGameStateInitialized()
        {
            if (App.Binder.SocialSystem.IsConnected())
            {
                this.OnSocialConnect(App.Binder.SocialSystem.GetConnectType(), App.Binder.SocialSystem.GetIdentity());
            }
        }

        private void OnLoginSuccess()
        {
            this.loggedIn = true;
        }

        private void onRoomComplete(Room room)
        {
            if (room.MainBossSummoned)
            {
                this.FlagForSync();
            }
        }

        public void OnSocialConnect(PlatformConnectType connectType, IUserProfile user)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player != null)
            {
                if (connectType == PlatformConnectType.Facebook)
                {
                    player.SocialData.FacebookId = user.id;
                    player.SocialData.FacebookName = user.userName;
                }
                else if (connectType == PlatformConnectType.GameCenter)
                {
                    if (player.SocialData.GameCenterId != user.id)
                    {
                        this.loggedIn = false;
                        player.IsDisconnectedFromPlatform = false;
                        this.FlagForSync();
                        this.Log("retrigger login");
                    }
                    player.SocialData.GameCenterId = user.id;
                    player.SocialData.GameCenterName = user.userName;
                }
                else if (connectType == PlatformConnectType.GooglePlay)
                {
                    if (player.SocialData.GooglePlayId != user.id)
                    {
                        this.loggedIn = false;
                        player.IsDisconnectedFromPlatform = false;
                        this.FlagForSync();
                        this.Log("retrigger login");
                    }
                    player.SocialData.GooglePlayId = user.id;
                    player.SocialData.GooglePlayName = user.userName;
                }
                else if (connectType == PlatformConnectType.Mock)
                {
                    player.SocialData.GameCenterId = Environment.UserName;
                    player.SocialData.GameCenterName = Environment.UserName;
                }
                this.Log(string.Concat(new object[] { "on social connect ", connectType, " ", user.id, " ", user.userName }));
                this.FlagForSync();
            }
        }

        private void onSyncEvent(object o)
        {
            this.FlagForSync();
        }

        private void onSyncEvent<T>(object o, T u)
        {
            this.FlagForSync();
        }

        public void ResolveLoginConflict(bool loadRemote)
        {
            if (this.loginTask != null)
            {
                this.loginTask.ResolveLoginConflict(loadRemote);
            }
        }

        public void SendSocialInboxCommand(SocialInboxCommand inboxCommand)
        {
            Service.Binder.TaskManager.StartTask(this.SendSocialInboxCommandAsync(inboxCommand), null);
        }

        [DebuggerHidden]
        public IEnumerator SendSocialInboxCommandAsync(SocialInboxCommand inboxCommand)
        {
            <SendSocialInboxCommandAsync>c__Iterator227 iterator = new <SendSocialInboxCommandAsync>c__Iterator227();
            iterator.inboxCommand = inboxCommand;
            iterator.<$>inboxCommand = inboxCommand;
            iterator.<>f__this = this;
            return iterator;
        }

        public void StartPlayerService()
        {
            Service.Binder.TaskManager.StartTask(this.Sync(), null);
        }

        [DebuggerHidden]
        public IEnumerator Sync()
        {
            <Sync>c__Iterator226 iterator = new <Sync>c__Iterator226();
            iterator.<>f__this = this;
            return iterator;
        }

        public bool UpdateStatus(IRequest request)
        {
            if (request.Success)
            {
                return true;
            }
            ExceptionResponse exceptionResponse = request.ExceptionResponse;
            if ((exceptionResponse != null) && (exceptionResponse.code == ServerErrorCode.SessionExpired))
            {
                this.loggedIn = false;
                this.Log("Session expired");
            }
            return false;
        }

        public int ConflictingPlayerLevel
        {
            get
            {
                return ((this.loginTask == null) ? -1 : this.loginTask.ConflictingPlayerLevel);
            }
        }

        public LoginStatus ConflictStatus
        {
            get
            {
                return ((this.loginTask == null) ? LoginStatus.LOGIN : this.loginTask.ConflictStatus);
            }
        }

        public bool HadRestoreError
        {
            [CompilerGenerated]
            get
            {
                return this.<HadRestoreError>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<HadRestoreError>k__BackingField = value;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return this.loggedIn;
            }
        }

        public bool IsWaitingForConflictPopup
        {
            get
            {
                return ((this.loginTask != null) && this.loginTask.HasConflict);
            }
        }

        public string PendingClientUpdateFromUrl
        {
            [CompilerGenerated]
            get
            {
                return this.<PendingClientUpdateFromUrl>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<PendingClientUpdateFromUrl>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <DetachGameState>c__Iterator225 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AuthRequest <req>__0;
            internal Request<string> <resp>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    case 1:
                        if (App.Binder.SocialSystem == null)
                        {
                            this.$current = null;
                            this.$PC = 1;
                        }
                        else
                        {
                            if (GameLogic.Binder.GameState.Player == null)
                            {
                                break;
                            }
                            AuthRequest request = new AuthRequest();
                            request.FgUserHandle = GameLogic.Binder.GameState.Player.FgUserHandle;
                            this.<req>__0 = request;
                            App.Binder.SocialSystem.SetupDetachRequest(this.<req>__0);
                            this.<resp>__1 = Request<string>.Post("/auth/detach", this.<req>__0);
                            this.$current = this.<resp>__1.Task;
                            this.$PC = 2;
                        }
                        return true;

                    case 2:
                        break;

                    default:
                        break;
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <SendSocialInboxCommandAsync>c__Iterator227 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SocialInboxCommand <$>inboxCommand;
            internal PlayerService <>f__this;
            internal Request<SocialInboxCommand> <resp>__0;
            internal SocialInboxCommand inboxCommand;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    case 1:
                        if (!Service.Binder.PlayerService.IsLoggedIn || !Service.Binder.ServiceWatchdog.IsOnline)
                        {
                            this.$current = null;
                            this.$PC = 1;
                        }
                        else
                        {
                            this.<resp>__0 = Request<SocialInboxCommand>.Post("/player/{sessionId}/inboxcommand/social", this.inboxCommand);
                            this.$current = this.<resp>__0.Task;
                            this.$PC = 2;
                        }
                        return true;

                    case 2:
                        if (this.<resp>__0.Success)
                        {
                            this.<>f__this.Log("Succesfully sent a social inbox command.");
                        }
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Sync>c__Iterator226 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PlayerService <>f__this;
            internal GameState <gs>__0;
            internal Request<UpdateResponse> <req>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<>f__this.lastSyncTime = 0f;
                        this.<>f__this.nextSyncTime = Time.time;
                        this.<>f__this.loggedIn = false;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00E5;

                    case 3:
                        goto Label_0148;

                    case 4:
                        this.<>f__this.HadRestoreError = this.<>f__this.loginTask.HasRestoreError;
                        this.<>f__this.loginTask = null;
                        if (!this.<>f__this.loggedIn)
                        {
                            goto Label_024B;
                        }
                        this.$current = Service.Binder.ContentService.LoadRemoteContent();
                        this.$PC = 5;
                        goto Label_033D;

                    case 5:
                        this.$current = Service.Binder.PromotionService.LoadPromotions();
                        this.$PC = 6;
                        goto Label_033D;

                    case 6:
                        goto Label_024B;

                    case 7:
                        if (this.<>f__this.UpdateStatus(this.<req>__1))
                        {
                            this.<gs>__0.Player.ServerStats = this.<req>__1.Result.ServerStats;
                            Service.Binder.EventBus.InboxCommands(this.<req>__1.Result.InboxCommands);
                        }
                        goto Label_0304;

                    default:
                        goto Label_033B;
                }
                if (!Service.Binder.ServiceWatchdog.IsOnline)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_033D;
                }
            Label_00E5:
                while (!Service.Binder.ServiceWatchdog.IsOnline)
                {
                    if (this.<>f__this.nextSyncTime < Time.time)
                    {
                        this.<>f__this.nextSyncTime += UnityEngine.Random.Range((float) 0f, (float) (App.Binder.ConfigMeta.PLAYER_SYNC_TIMEOUT / 3f));
                    }
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_033D;
                }
                if ((Time.realtimeSinceStartup - this.<>f__this.lastSyncTime) > PlayerService.SESSION_TIMEOUT)
                {
                    this.<>f__this.loggedIn = false;
                    this.<>f__this.Log("Session too old, re-login");
                }
            Label_0148:
                while ((this.<>f__this.nextSyncTime > Time.time) && (this.<>f__this.lastSyncTime < (Time.realtimeSinceStartup + App.Binder.ConfigMeta.SYNC_CHECK_INTERVAL)))
                {
                    this.$current = 0;
                    this.$PC = 3;
                    goto Label_033D;
                }
                if (!this.<>f__this.loggedIn)
                {
                    this.<>f__this.HadRestoreError = false;
                    this.<>f__this.loginTask = new LoginTask(new System.Action(this.<>f__this.OnLoginSuccess));
                    this.$current = this.<>f__this.loginTask.Login();
                    this.$PC = 4;
                    goto Label_033D;
                }
            Label_024B:
                if (this.<>f__this.loggedIn)
                {
                    this.<gs>__0 = GameLogic.Binder.GameState;
                    this.<gs>__0.Player.RemoteNotificationPlayerData.HasLoggedInSinceLastNotification = true;
                    this.<req>__1 = Request<UpdateResponse>.PostEncrypted("/player/{sessionId}/update", this.<gs>__0.Player);
                    this.$current = this.<req>__1.Task;
                    this.$PC = 7;
                    goto Label_033D;
                }
            Label_0304:
                this.<>f__this.nextSyncTime = Time.time + App.Binder.ConfigMeta.PLAYER_SYNC_TIMEOUT;
                this.<>f__this.lastSyncTime = Time.realtimeSinceStartup;
                goto Label_00E5;
                this.$PC = -1;
            Label_033B:
                return false;
            Label_033D:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

