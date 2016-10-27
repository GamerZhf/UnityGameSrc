namespace App
{
    using GameLogic;
    using GooglePlayGames;
    using GooglePlayGames.BasicApi;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class GooglePlaySocialSystem : MonoBehaviour, ISocialSystem
    {
        private bool? m_authSign;
        private PlatformPlayerIdentity m_identity;
        private PlatformConnectState m_state;
        private string m_verificationSignature;

        protected void Awake()
        {
            this.m_authSign = null;
            this.m_identity = null;
            this.m_verificationSignature = null;
            this.ChangeState(PlatformConnectState.Initializing);
            PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
            PlayGamesPlatform.Activate();
        }

        private void ChangeState(PlatformConnectState newState)
        {
            this.m_state = newState;
            App.Binder.EventBus.PlatformConnectStateChanged(PlatformConnectType.GooglePlay);
        }

        public void Connect()
        {
            Log("Connect()");
            GameLogic.Binder.GameState.Player.IsDisconnectedFromPlatform = false;
            Social.localUser.Authenticate(new Action<bool>(this.onAuthenticated));
        }

        public void Disconnect()
        {
            Log("Disconnect()");
            if (Social.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.SignOut();
            }
            GameLogic.Binder.GameState.Player.IsDisconnectedFromPlatform = true;
            this.m_authSign = false;
            this.m_verificationSignature = null;
            this.m_identity = null;
            this.ChangeState(PlatformConnectState.Unconnected);
        }

        private string GetAndroidInstallerPackageName()
        {
            try
            {
                AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                object[] args = new object[] { Application.bundleIdentifier };
                return class2.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getPackageManager", new object[0]).Call<string>("getInstallerPackageName", args);
            }
            catch (Exception)
            {
            }
            return null;
        }

        public PlatformConnectType GetConnectType()
        {
            return PlatformConnectType.GooglePlay;
        }

        public IUserProfile GetIdentity()
        {
            return this.m_identity;
        }

        public PlatformConnectState GetState()
        {
            return this.m_state;
        }

        public bool HasAuthCredentials()
        {
            return ((this.m_verificationSignature != null) && (this.m_identity != null));
        }

        public bool IsConnected()
        {
            return (this.m_state == PlatformConnectState.Connected);
        }

        private static void Log(string str)
        {
            if (Service.Binder.Logger != null)
            {
                Service.Binder.Logger.Log(str);
            }
            else
            {
                UnityEngine.Debug.Log(str);
            }
        }

        private void onAuthenticated(bool success)
        {
            if (success)
            {
                PlatformPlayerIdentity identity = new PlatformPlayerIdentity();
                identity.id = Social.localUser.id;
                identity.userName = Social.localUser.userName;
                identity.isFriend = false;
                this.m_identity = identity;
                Log("Locally connected " + this.m_identity.id);
                this.ChangeState(PlatformConnectState.Connected);
                this.m_authSign = true;
                App.Binder.EventBus.PlatformProfileUpdated(PlatformConnectType.GooglePlay, this.m_identity);
            }
            else
            {
                GameLogic.Binder.GameState.Player.IsDisconnectedFromPlatform = true;
                this.ChangeState(PlatformConnectState.Unconnected);
                this.m_identity = null;
                this.m_authSign = false;
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
        }

        private void onGameStateInitialized()
        {
            Log("onGameStateInitialized, IsDisconnectedFromPlatform" + GameLogic.Binder.GameState.Player.IsDisconnectedFromPlatform);
            if (!GameLogic.Binder.GameState.Player.IsDisconnectedFromPlatform)
            {
                this.Connect();
            }
            else
            {
                this.m_authSign = false;
                this.ChangeState(PlatformConnectState.Unconnected);
            }
        }

        public void ReConnect()
        {
            this.Disconnect();
            this.Connect();
        }

        public bool ReportAchievement(string id, float percentage)
        {
            <ReportAchievement>c__AnonStorey2E5 storeye = new <ReportAchievement>c__AnonStorey2E5();
            storeye.id = id;
            storeye.percentage = percentage;
            if (this.IsConnected())
            {
                Social.ReportProgress(storeye.id, (double) (storeye.percentage * 100f), new Action<bool>(storeye.<>m__17D));
                return true;
            }
            return false;
        }

        public bool ReportLeaderboardScore(long score)
        {
            <ReportLeaderboardScore>c__AnonStorey2E6 storeye = new <ReportLeaderboardScore>c__AnonStorey2E6();
            storeye.score = score;
            if (this.IsConnected())
            {
                Social.ReportScore(storeye.score, ConfigApp.GooglePlayLeaderboardId, new Action<bool>(storeye.<>m__17E));
                return true;
            }
            return false;
        }

        public void SetupAuthRequest(AuthRequest authRequest, bool disconnected)
        {
            Log(string.Concat(new object[] { "SetupAuthRequest: ", this.m_identity, ", ", this.m_verificationSignature }));
            if (this.HasAuthCredentials() && !disconnected)
            {
                Log("HasAuthCredentials: " + this.m_identity.id);
                GooglePlayAuth auth = new GooglePlayAuth();
                auth.authCode = this.m_verificationSignature;
                auth.playerId = this.m_identity.id;
                authRequest.GooglePlay = auth;
            }
            authRequest.platformName = "Android OS";
            authRequest.platformVersion = !Regex.IsMatch(SystemInfo.operatingSystem, @"API\-[0-9]+") ? SystemInfo.operatingSystem : Regex.Match(SystemInfo.operatingSystem, @"API\-[0-9]+").Value;
            authRequest.model = SystemInfo.deviceModel;
            authRequest.androidDeviceId = SystemInfo.deviceUniqueIdentifier;
            authRequest.installSource = this.GetAndroidInstallerPackageName();
            Log("bundleIdentifier: " + Application.bundleIdentifier);
            Log("installSource: " + authRequest.installSource);
        }

        public void SetupDetachRequest(AuthRequest authRequest)
        {
            authRequest.GooglePlay = new GooglePlayAuth();
        }

        public void ShowAchievements()
        {
            if (this.IsConnected())
            {
                Social.ShowAchievementsUI();
            }
        }

        public void ShowLeaderboards()
        {
            if (this.IsConnected())
            {
                Social.ShowLeaderboardUI();
            }
        }

        public bool SupportsDiconnect()
        {
            return true;
        }

        [DebuggerHidden]
        public IEnumerator WaitForAuthCredentials()
        {
            <WaitForAuthCredentials>c__Iterator33 iterator = new <WaitForAuthCredentials>c__Iterator33();
            iterator.<>f__this = this;
            return iterator;
        }

        [CompilerGenerated]
        private sealed class <ReportAchievement>c__AnonStorey2E5
        {
            internal string id;
            internal float percentage;

            internal void <>m__17D(bool _result)
            {
                GooglePlaySocialSystem.Log(string.Concat(new object[] { "Report achievement ", this.id, " percentage ", this.percentage, ", result: ", _result }));
            }
        }

        [CompilerGenerated]
        private sealed class <ReportLeaderboardScore>c__AnonStorey2E6
        {
            internal long score;

            internal void <>m__17E(bool _result)
            {
                GooglePlaySocialSystem.Log(string.Concat(new object[] { "Report score ", this.score, ", result: ", _result }));
            }
        }

        [CompilerGenerated]
        private sealed class <WaitForAuthCredentials>c__Iterator33 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal GooglePlaySocialSystem <>f__this;
            internal bool <done>__0;

            internal void <>m__17F(CommonStatusCodes _codes, string _s)
            {
                if ((_codes == CommonStatusCodes.Success) || (_codes == CommonStatusCodes.SuccessCached))
                {
                    GooglePlaySocialSystem.Log("GetServerAuthCode, authCode: " + _s);
                    this.<>f__this.m_verificationSignature = _s;
                }
                else
                {
                    UnityEngine.Debug.LogError("GetServerAuthCode failed: " + _codes);
                }
                this.<done>__0 = true;
            }

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
                        if (!this.<>f__this.m_authSign.HasValue)
                        {
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_00C7;
                        }
                        if (!this.<>f__this.m_authSign.Value)
                        {
                            goto Label_00C5;
                        }
                        this.<>f__this.m_verificationSignature = null;
                        this.<done>__0 = false;
                        PlayGamesPlatform.Instance.GetServerAuthCode(new Action<CommonStatusCodes, string>(this.<>m__17F));
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_00C5;
                }
                while (!this.<done>__0)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_00C7;
                }
                this.$PC = -1;
            Label_00C5:
                return false;
            Label_00C7:
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

