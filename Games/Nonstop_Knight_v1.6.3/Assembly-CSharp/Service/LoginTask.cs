namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class LoginTask
    {
        [CompilerGenerated]
        private LoginStatus <ConflictStatus>k__BackingField;
        [CompilerGenerated]
        private bool <HasConflict>k__BackingField;
        [CompilerGenerated]
        private bool <HasRestoreError>k__BackingField;
        private LoginResponse lastResponse;
        private bool? loadRemote;
        private bool loggedIn;
        private Action onLoginSuccess;
        private bool rebindAccount;

        public LoginTask(Action onLoginSuccessCb)
        {
            this.onLoginSuccess = onLoginSuccessCb;
        }

        private void CommitLogin(LoginResponse resp, Player player)
        {
            player._id = resp.UserId;
            if (resp.Password == null)
            {
            }
            player.Password = player.Password;
            player.FgUserHandle = resp.FgUserHandle;
            player.SessionId = resp.SessionId;
            player.ServerStats = resp.ServerStats;
            this.Log("commited login with:" + player.FgUserHandle + " " + player.Password);
            this.loggedIn = true;
            this.onLoginSuccess();
        }

        private AuthRequest CreateAuthRequest(Player player, [Optional, DefaultParameterValue(true)] bool withLocalCredentials)
        {
            AuthRequest request2 = new AuthRequest();
            request2.FgUserHandle = !withLocalCredentials ? null : player.FgUserHandle;
            request2.Password = !withLocalCredentials ? null : player.Password;
            request2.LastUpdateStamp = player.ServerStats.UpdateStamp;
            request2.displayLanguage = App.Binder.LocaSystem.DisplayLanguage;
            request2.arch = SystemInfo.deviceModel;
            request2.utcOffset = (int) TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes;
            request2.appversion = ConfigApp.BundleVersion;
            AuthRequest authRequest = request2;
            App.Binder.SocialSystem.SetupAuthRequest(authRequest, player.IsDisconnectedFromPlatform);
            this.Log("platformName: " + authRequest.platformName);
            this.Log("platformVersion: " + authRequest.platformVersion);
            this.Log("model: " + authRequest.model);
            this.Log("arch: " + authRequest.arch);
            this.Log(string.Concat(new object[] { "login with credentials:", authRequest.FgUserHandle, " ", authRequest.Password, " gamecenter:", authRequest.GameCenter != null }));
            return authRequest;
        }

        private string GeneratePassword()
        {
            string str = "abcdefghklmnopqrstuvwxyz0123456789";
            byte[] buffer = Guid.NewGuid().ToByteArray();
            string str2 = string.Empty;
            foreach (byte num in buffer)
            {
                str2 = str2 + str[num % str.Length];
            }
            return str2;
        }

        private string GetConnectPath()
        {
            return (!this.rebindAccount ? "/auth/connect" : "/auth/forceconnect");
        }

        private void Log(string msg)
        {
            Service.Binder.Logger.Log(msg);
        }

        [DebuggerHidden]
        public IEnumerator Login()
        {
            <Login>c__Iterator21B iteratorb = new <Login>c__Iterator21B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        [DebuggerHidden]
        private IEnumerator LoginDisconnected(Player player)
        {
            <LoginDisconnected>c__Iterator21A iteratora = new <LoginDisconnected>c__Iterator21A();
            iteratora.player = player;
            iteratora.<$>player = player;
            iteratora.<>f__this = this;
            return iteratora;
        }

        private Player migratePlayer(string playerJson)
        {
            PlayerLoader.MigrationResult result;
            Player player = PlayerLoader.CreateAndMigrate(playerJson, out result);
            if ((result != PlayerLoader.MigrationResult.Error) && (result != PlayerLoader.MigrationResult.Repaired))
            {
                return player;
            }
            this.HasRestoreError = true;
            return null;
        }

        private void ResetWithPlayer(Player remotePlayer)
        {
            if (remotePlayer == null)
            {
                remotePlayer = PlayerLoader.CreateNew();
            }
            App.Binder.AppContext.hardReset(remotePlayer);
        }

        public void ResolveLoginConflict(bool loadRemote)
        {
            this.rebindAccount = !loadRemote;
            this.loadRemote = new bool?(loadRemote);
        }

        public bool UpdateStatus(IRequest request)
        {
            if (request.Success)
            {
                return true;
            }
            ExceptionResponse exceptionResponse = request.ExceptionResponse;
            if (exceptionResponse != null)
            {
                if (exceptionResponse.code == ServerErrorCode.SessionExpired)
                {
                    this.loggedIn = false;
                    this.Log("Session expired");
                }
                else if (exceptionResponse.code == ServerErrorCode.NotFound)
                {
                    this.Log("Player not found, re-register");
                    Player player = GameLogic.Binder.GameState.Player;
                    player._id = null;
                    player.FgUserHandle = null;
                    player.SessionId = null;
                    player.Password = null;
                    this.loggedIn = false;
                }
            }
            return false;
        }

        public int ConflictingPlayerLevel
        {
            get
            {
                return ((this.lastResponse == null) ? -1 : this.lastResponse.ConflictUserRank);
            }
        }

        public LoginStatus ConflictStatus
        {
            [CompilerGenerated]
            get
            {
                return this.<ConflictStatus>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ConflictStatus>k__BackingField = value;
            }
        }

        public bool HasConflict
        {
            [CompilerGenerated]
            get
            {
                return this.<HasConflict>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<HasConflict>k__BackingField = value;
            }
        }

        public bool HasRestoreError
        {
            [CompilerGenerated]
            get
            {
                return this.<HasRestoreError>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<HasRestoreError>k__BackingField = value;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return this.loggedIn;
            }
        }

        [CompilerGenerated]
        private sealed class <Login>c__Iterator21B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal LoginTask <>f__this;
            internal GameState <gs>__0;
            internal LoginResponse <loginResponse>__3;
            internal Player <newPlayer>__5;
            internal Player <newPlayer>__8;
            internal Player <player>__1;
            internal Request<LoginResponse> <req>__2;
            internal Player <switchPlayer>__7;
            internal Request<string> <switchReq>__4;
            internal Request<string> <switchReq>__6;

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
                        this.<gs>__0 = GameLogic.Binder.GameState;
                        this.<player>__1 = this.<gs>__0.Player;
                        this.$current = App.Binder.SocialSystem.WaitForAuthCredentials();
                        this.$PC = 1;
                        goto Label_04D5;

                    case 1:
                        if (!App.Binder.SocialSystem.IsConnected() && !App.Binder.SocialSystem.SupportsDiconnect())
                        {
                            this.<player>__1.IsDisconnectedFromPlatform = false;
                        }
                        this.<req>__2 = Request<LoginResponse>.Post(this.<>f__this.GetConnectPath(), this.<>f__this.CreateAuthRequest(this.<player>__1, true));
                        this.$current = this.<req>__2.Task;
                        this.$PC = 2;
                        goto Label_04D5;

                    case 2:
                        this.<>f__this.lastResponse = this.<req>__2.Result;
                        if (this.<>f__this.UpdateStatus(this.<req>__2))
                        {
                            this.<loginResponse>__3 = this.<req>__2.Result;
                            if (this.<loginResponse>__3 != null)
                            {
                                if (this.<loginResponse>__3.LoginStatus == LoginStatus.REGISTER)
                                {
                                    this.<>f__this.CommitLogin(this.<loginResponse>__3, this.<player>__1);
                                    Service.Binder.EventBus.PlayerRegistered();
                                }
                                else if (this.<loginResponse>__3.LoginStatus == LoginStatus.LOGIN)
                                {
                                    this.<>f__this.CommitLogin(this.<loginResponse>__3, this.<player>__1);
                                    Service.Binder.EventBus.PlayerLoggedIn();
                                }
                                else
                                {
                                    if (this.<loginResponse>__3.LoginStatus != LoginStatus.CONFLICT_CHOOSE)
                                    {
                                        goto Label_02DD;
                                    }
                                    this.<>f__this.HasConflict = true;
                                    this.<>f__this.ConflictStatus = LoginStatus.CONFLICT_CHOOSE;
                                    break;
                                }
                            }
                        }
                        goto Label_04D3;

                    case 3:
                        break;

                    case 4:
                        this.<switchReq>__4 = Request<string>.Post("/auth/switch", this.<>f__this.CreateAuthRequest(this.<player>__1, false));
                        this.$current = this.<switchReq>__4.Task;
                        this.$PC = 5;
                        goto Label_04D5;

                    case 5:
                        if (!this.<>f__this.UpdateStatus(this.<switchReq>__4))
                        {
                            this.<>f__this.HasRestoreError = true;
                            goto Label_04D3;
                        }
                        this.<newPlayer>__5 = this.<>f__this.migratePlayer(this.<switchReq>__4.Result);
                        if (this.<newPlayer>__5 != null)
                        {
                            this.<>f__this.ResetWithPlayer(this.<newPlayer>__5);
                            goto Label_04D3;
                        }
                        goto Label_02DD;

                    case 6:
                        goto Label_031E;

                    case 7:
                        if (!this.<>f__this.UpdateStatus(this.<switchReq>__6))
                        {
                            this.<>f__this.HasRestoreError = true;
                        }
                        else
                        {
                            this.<switchPlayer>__7 = this.<>f__this.migratePlayer(this.<switchReq>__6.Result);
                            if (this.<switchPlayer>__7 == null)
                            {
                                goto Label_0410;
                            }
                            this.<>f__this.ResetWithPlayer(this.<switchPlayer>__7);
                        }
                        goto Label_04D3;

                    case 8:
                        goto Label_0451;

                    case 9:
                        this.$PC = -1;
                        goto Label_04D3;

                    default:
                        goto Label_04D3;
                }
                while (!this.<>f__this.loadRemote.HasValue)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_04D5;
                }
                this.<>f__this.HasConflict = false;
                if (this.<>f__this.loadRemote.Value)
                {
                    this.$current = App.Binder.SocialSystem.WaitForAuthCredentials();
                    this.$PC = 4;
                    goto Label_04D5;
                }
            Label_02DD:
                if (this.<loginResponse>__3.LoginStatus != LoginStatus.CONFLICT_UPDATE)
                {
                    goto Label_0410;
                }
                this.<>f__this.HasConflict = true;
                this.<>f__this.ConflictStatus = LoginStatus.CONFLICT_UPDATE;
            Label_031E:
                while (!this.<>f__this.loadRemote.HasValue)
                {
                    this.$current = null;
                    this.$PC = 6;
                    goto Label_04D5;
                }
                this.<>f__this.HasConflict = false;
                this.<>f__this.CommitLogin(this.<loginResponse>__3, this.<player>__1);
                if (this.<>f__this.loadRemote.Value)
                {
                    this.<switchReq>__6 = Request<string>.Get("/player/{sessionId}");
                    this.$current = this.<switchReq>__6.Task;
                    this.$PC = 7;
                    goto Label_04D5;
                }
                Service.Binder.EventBus.PlayerLoggedIn();
                goto Label_04D3;
            Label_0410:
                if (this.<loginResponse>__3.LoginStatus != LoginStatus.CONFLICT_NEW)
                {
                    goto Label_04A8;
                }
                this.<>f__this.HasConflict = true;
                this.<>f__this.ConflictStatus = LoginStatus.CONFLICT_NEW;
            Label_0451:
                while (!this.<>f__this.loadRemote.HasValue)
                {
                    this.$current = null;
                    this.$PC = 8;
                    goto Label_04D5;
                }
                this.<>f__this.HasConflict = false;
                if (this.<>f__this.loadRemote.Value)
                {
                    this.<newPlayer>__8 = PlayerLoader.CreateNew();
                    this.<>f__this.ResetWithPlayer(this.<newPlayer>__8);
                    goto Label_04D3;
                }
            Label_04A8:
                this.$current = this.<>f__this.LoginDisconnected(this.<player>__1);
                this.$PC = 9;
                goto Label_04D5;
            Label_04D3:
                return false;
            Label_04D5:
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

        [CompilerGenerated]
        private sealed class <LoginDisconnected>c__Iterator21A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Player <$>player;
            internal LoginTask <>f__this;
            internal Request<LoginResponse> <loginReq>__0;
            internal Player player;

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
                        if (this.<>f__this.rebindAccount)
                        {
                            this.$current = App.Binder.SocialSystem.WaitForAuthCredentials();
                            this.$PC = 1;
                            goto Label_00FD;
                        }
                        this.player.IsDisconnectedFromPlatform = true;
                        break;

                    case 1:
                        break;

                    case 2:
                        this.<>f__this.lastResponse = this.<loginReq>__0.Result;
                        if (this.<>f__this.UpdateStatus(this.<loginReq>__0))
                        {
                            this.<>f__this.CommitLogin(this.<loginReq>__0.Result, this.player);
                            this.$PC = -1;
                        }
                        goto Label_00FB;

                    default:
                        goto Label_00FB;
                }
                this.<loginReq>__0 = Request<LoginResponse>.Post(this.<>f__this.GetConnectPath(), this.<>f__this.CreateAuthRequest(this.player, true));
                this.$current = this.<loginReq>__0.Task;
                this.$PC = 2;
                goto Label_00FD;
            Label_00FB:
                return false;
            Label_00FD:
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

