namespace Facebook.Unity
{
    using Facebook.MiniJSON;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class AccessToken
    {
        [CompilerGenerated]
        private static AccessToken <CurrentAccessToken>k__BackingField;
        [CompilerGenerated]
        private DateTime <ExpirationTime>k__BackingField;
        [CompilerGenerated]
        private DateTime? <LastRefresh>k__BackingField;
        [CompilerGenerated]
        private IEnumerable<string> <Permissions>k__BackingField;
        [CompilerGenerated]
        private string <TokenString>k__BackingField;
        [CompilerGenerated]
        private string <UserId>k__BackingField;

        internal AccessToken(string tokenString, string userId, DateTime expirationTime, IEnumerable<string> permissions, DateTime? lastRefresh)
        {
            if (string.IsNullOrEmpty(tokenString))
            {
                throw new ArgumentNullException("tokenString");
            }
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }
            if (expirationTime == DateTime.MinValue)
            {
                throw new ArgumentException("Expiration time is unassigned");
            }
            if (permissions == null)
            {
                throw new ArgumentNullException("permissions");
            }
            this.TokenString = tokenString;
            this.ExpirationTime = expirationTime;
            this.Permissions = permissions;
            this.UserId = userId;
            this.LastRefresh = lastRefresh;
        }

        internal string ToJson()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary[LoginResult.PermissionsKey] = string.Join(",", Enumerable.ToArray<string>(this.Permissions));
            dictionary[LoginResult.ExpirationTimestampKey] = Utilities.TotalSeconds(this.ExpirationTime).ToString();
            dictionary[LoginResult.AccessTokenKey] = this.TokenString;
            dictionary[LoginResult.UserIdKey] = this.UserId;
            if (this.LastRefresh.HasValue)
            {
                dictionary["last_refresh"] = Utilities.TotalSeconds(this.LastRefresh.Value).ToString();
            }
            return Json.Serialize(dictionary);
        }

        public static AccessToken CurrentAccessToken
        {
            [CompilerGenerated]
            get
            {
                return <CurrentAccessToken>k__BackingField;
            }
            [CompilerGenerated]
            internal set
            {
                <CurrentAccessToken>k__BackingField = value;
            }
        }

        public DateTime ExpirationTime
        {
            [CompilerGenerated]
            get
            {
                return this.<ExpirationTime>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ExpirationTime>k__BackingField = value;
            }
        }

        public DateTime? LastRefresh
        {
            [CompilerGenerated]
            get
            {
                return this.<LastRefresh>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LastRefresh>k__BackingField = value;
            }
        }

        public IEnumerable<string> Permissions
        {
            [CompilerGenerated]
            get
            {
                return this.<Permissions>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Permissions>k__BackingField = value;
            }
        }

        public string TokenString
        {
            [CompilerGenerated]
            get
            {
                return this.<TokenString>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TokenString>k__BackingField = value;
            }
        }

        public string UserId
        {
            [CompilerGenerated]
            get
            {
                return this.<UserId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<UserId>k__BackingField = value;
            }
        }
    }
}

