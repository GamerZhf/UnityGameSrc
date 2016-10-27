namespace GooglePlayGames
{
    using GooglePlayGames.BasicApi;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.SocialPlatforms;

    public class PlayGamesLocalUser : PlayGamesUserProfile, IUserProfile, ILocalUser
    {
        private string emailAddress;
        internal PlayGamesPlatform mPlatform;
        private PlayerStats mStats;

        internal PlayGamesLocalUser(PlayGamesPlatform plaf) : base("localUser", string.Empty, string.Empty)
        {
            this.mPlatform = plaf;
            this.emailAddress = null;
            this.mStats = null;
        }

        public void Authenticate(Action<bool> callback)
        {
            this.mPlatform.Authenticate(callback);
        }

        public void Authenticate(Action<bool> callback, bool silent)
        {
            this.mPlatform.Authenticate(callback, silent);
        }

        [Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
        public void GetIdToken(Action<string> idTokenCallback)
        {
            if (this.authenticated)
            {
                this.mPlatform.GetIdToken(idTokenCallback);
            }
            else
            {
                idTokenCallback(null);
            }
        }

        public void GetStats(Action<CommonStatusCodes, PlayerStats> callback)
        {
            <GetStats>c__AnonStorey268 storey = new <GetStats>c__AnonStorey268();
            storey.callback = callback;
            storey.<>f__this = this;
            if ((this.mStats == null) || !this.mStats.Valid)
            {
                this.mPlatform.GetPlayerStats(new Action<CommonStatusCodes, PlayerStats>(storey.<>m__88));
            }
            else
            {
                storey.callback(CommonStatusCodes.Success, this.mStats);
            }
        }

        public void LoadFriends(Action<bool> callback)
        {
            this.mPlatform.LoadFriends(this, callback);
        }

        [Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
        public string accessToken
        {
            get
            {
                return (!this.authenticated ? string.Empty : this.mPlatform.GetAccessToken());
            }
        }

        public bool authenticated
        {
            get
            {
                return this.mPlatform.IsAuthenticated();
            }
        }

        public string AvatarURL
        {
            get
            {
                string userImageUrl = string.Empty;
                if (this.authenticated)
                {
                    userImageUrl = this.mPlatform.GetUserImageUrl();
                    if (!base.id.Equals(userImageUrl))
                    {
                        base.ResetIdentity(this.mPlatform.GetUserDisplayName(), this.mPlatform.GetUserId(), userImageUrl);
                    }
                }
                return userImageUrl;
            }
        }

        public string Email
        {
            get
            {
                if (this.authenticated && string.IsNullOrEmpty(this.emailAddress))
                {
                    this.emailAddress = this.mPlatform.GetUserEmail();
                    if (this.emailAddress == null)
                    {
                    }
                    this.emailAddress = string.Empty;
                }
                return (!this.authenticated ? string.Empty : this.emailAddress);
            }
        }

        public IUserProfile[] friends
        {
            get
            {
                return this.mPlatform.GetFriends();
            }
        }

        public string id
        {
            get
            {
                string userId = string.Empty;
                if (this.authenticated)
                {
                    userId = this.mPlatform.GetUserId();
                    if (!base.id.Equals(userId))
                    {
                        base.ResetIdentity(this.mPlatform.GetUserDisplayName(), userId, this.mPlatform.GetUserImageUrl());
                    }
                }
                return userId;
            }
        }

        public bool isFriend
        {
            get
            {
                return true;
            }
        }

        public UserState state
        {
            get
            {
                return UserState.Online;
            }
        }

        public bool underage
        {
            get
            {
                return true;
            }
        }

        public string userName
        {
            get
            {
                string userDisplayName = string.Empty;
                if (this.authenticated)
                {
                    userDisplayName = this.mPlatform.GetUserDisplayName();
                    if (!base.userName.Equals(userDisplayName))
                    {
                        base.ResetIdentity(userDisplayName, this.mPlatform.GetUserId(), this.mPlatform.GetUserImageUrl());
                    }
                }
                return userDisplayName;
            }
        }

        [CompilerGenerated]
        private sealed class <GetStats>c__AnonStorey268
        {
            internal PlayGamesLocalUser <>f__this;
            internal Action<CommonStatusCodes, PlayerStats> callback;

            internal void <>m__88(CommonStatusCodes rc, PlayerStats stats)
            {
                this.<>f__this.mStats = stats;
                this.callback(rc, stats);
            }
        }
    }
}

