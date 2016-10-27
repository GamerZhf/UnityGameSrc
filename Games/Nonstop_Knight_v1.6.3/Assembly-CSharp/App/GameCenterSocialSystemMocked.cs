namespace App
{
    using GameLogic;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class GameCenterSocialSystemMocked : MonoBehaviour, ISocialSystem
    {
        private GameCenterAuth currentAccount;
        private string currentName;
        private static GameCenterAuth gameCenter1;
        private static GameCenterAuth gameCenter2;
        private PlatformPlayerIdentity m_identity;
        private PlatformConnectState m_state;

        static GameCenterSocialSystemMocked()
        {
            GameCenterAuth auth = new GameCenterAuth();
            auth.playerId = "G:10184998643";
            auth.bundleId = "com.koplagames.kopla01";
            auth.publicKeyUrl = "https://static.gc.apple.com/public-key/gc-prod-2.cer";
            auth.signature = "mNV/IjG6wZmXcnKEXrF7bMxnOuMHWlylNKMFaUstip4pRVF5HBAmPJDI0Rrf9V8GWI1oCZEYPqq/skUPdvtks/Y8qLGRnF7BDN4mZehf6DggZkTD91Hs54TRMkurEmQMTNUVqSLzokc8O93v1yW+Dc8VC0DcW6cVphohbEZn054398gE/KivDzVqjJ0IPQzXuYgrpGQFrxbm2rTBtbYBgc5Zr+I5ZDSHRz8kZBE7qoyPu2PYb9NWporiFW47ALYkj/sf64c477y5fD+uvC6/G0v7ZlU+taXkmXcfeF2YOCAse9OoXVUwTHRcD59KefI7cDaJJUQfQweiWXA/te7t3Q==";
            auth.salt = "uISUpw==";
            auth.timestamp = 0x153a7fe90b5L;
            gameCenter1 = auth;
            auth = new GameCenterAuth();
            auth.playerId = "G:1864360753";
            auth.bundleId = "com.koplagames.kopla01";
            auth.publicKeyUrl = "https://static.gc.apple.com/public-key/gc-prod-2.cer";
            auth.signature = "m4419Q0SpCey/IP6C9MRAysBSZrmgC/DQNq6LH/Sm9krz8OQBu+SKzHfBq3NPFtNta3m04YWl0cICkHyIR/zIRHdOzZDMsuZ+7FJxCfiuRj7z4O3Xkx5etFWe1Tj0gkXoDBN1E78z5yewo4bnprq8aa1itGlZc/HMLwbt2eqjU42XYSZBYC45LMCj7aynXhZTnMc6zlPWj1eBHXVebgb+owurPbll94kD8p/3g0ygSCwS7n95HDfzfFhPFDWrp8E4RBTeDPE3hsldE10jTmB3DWOHcudKrpCUGmDtEDyA2tuQMIhAmQehPva4b7QX71CGs27BzYLUJM1IJ1MBjEvHw==";
            auth.salt = "nwwwjA==";
            auth.timestamp = 0x153a861bfbfL;
            gameCenter2 = auth;
        }

        private void Awake()
        {
            this.m_state = PlatformConnectState.Initializing;
        }

        public void Connect()
        {
        }

        public void Disconnect()
        {
            this.m_identity = null;
            this.m_state = PlatformConnectState.Unconnected;
        }

        private GameCenterAuth GetAccount()
        {
            switch (PlayerPrefs.GetString("gamecenter_account"))
            {
                case "account1":
                    return gameCenter1;

                case "account2":
                    return gameCenter2;
            }
            return null;
        }

        public PlatformConnectType GetConnectType()
        {
            return PlatformConnectType.GameCenter;
        }

        public IUserProfile GetIdentity()
        {
            return this.m_identity;
        }

        private string GetName()
        {
            switch (PlayerPrefs.GetString("gamecenter_account"))
            {
                case "account1":
                    return "\ud83d\ude34\ud83c\udfb6\ud83d\ude1c\ud83d\ude11\ud83d\ude0e\ud83d\ude0c\ud83d\udc95❤️\ud83d\ude01\ud83d\ude04";

                case "account2":
                    return "ACCOUNT_2";
            }
            return null;
        }

        public PlatformConnectState GetState()
        {
            return this.m_state;
        }

        public bool HasAuthCredentials()
        {
            return (this.currentAccount != null);
        }

        public bool IsConnected()
        {
            return (this.m_state == PlatformConnectState.Connected);
        }

        private void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
        }

        private void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
        }

        private void onGameStateInitialized()
        {
            this.ReConnect();
        }

        public void ReConnect()
        {
            this.m_state = PlatformConnectState.Initializing;
            this.currentAccount = this.GetAccount();
            this.currentName = this.GetName();
            if (this.currentAccount != null)
            {
                PlatformPlayerIdentity identity = new PlatformPlayerIdentity();
                identity.id = this.currentAccount.playerId;
                identity.userName = this.currentName;
                this.m_identity = identity;
                this.m_state = PlatformConnectState.Connected;
                App.Binder.EventBus.PlatformProfileUpdated(PlatformConnectType.GameCenter, this.m_identity);
            }
            else
            {
                this.m_state = PlatformConnectState.Unconnected;
            }
        }

        public bool ReportAchievement(string id, float percentage)
        {
            UnityEngine.Debug.Log(string.Concat(new object[] { "Report Achievement ", id, " percentage ", percentage }));
            return true;
        }

        public bool ReportLeaderboardScore(long score)
        {
            UnityEngine.Debug.Log("Report score " + score);
            return true;
        }

        public void SetupAuthRequest(AuthRequest authRequest, bool disconnected)
        {
            if (this.HasAuthCredentials() && !disconnected)
            {
                authRequest.GameCenter = this.currentAccount;
            }
            authRequest.platformName = Regex.Replace(SystemInfo.operatingSystem, @"[0-9\.]", string.Empty).Trim();
            authRequest.platformVersion = Regex.Replace(SystemInfo.operatingSystem, "[a-zA-Z]", string.Empty).Trim();
            authRequest.model = Regex.Replace(SystemInfo.deviceModel, "[0-9,]", string.Empty);
            authRequest.installSource = "com.flaregames.mockstore";
        }

        public void SetupDetachRequest(AuthRequest authRequest)
        {
            authRequest.GameCenter = new GameCenterAuth();
        }

        public void ShowAchievements()
        {
            UnityEngine.Debug.Log("ShowAchievements");
        }

        public void ShowLeaderboards()
        {
            UnityEngine.Debug.Log("ShowLeaderboards");
        }

        public bool SupportsDiconnect()
        {
            return true;
        }

        [DebuggerHidden]
        public IEnumerator WaitForAuthCredentials()
        {
            <WaitForAuthCredentials>c__Iterator32 iterator = new <WaitForAuthCredentials>c__Iterator32();
            iterator.<>f__this = this;
            return iterator;
        }

        [CompilerGenerated]
        private sealed class <WaitForAuthCredentials>c__Iterator32 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal GameCenterSocialSystemMocked <>f__this;

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
                        if (this.<>f__this.m_state == PlatformConnectState.Initializing)
                        {
                            this.$current = null;
                            this.$PC = 1;
                            return true;
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
    }
}

