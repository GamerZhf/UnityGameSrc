namespace Facebook.Unity
{
    using System;
    using System.Runtime.CompilerServices;

    internal class LoginResult : ResultBase, ILoginResult, IResult
    {
        [CompilerGenerated]
        private Facebook.Unity.AccessToken <AccessToken>k__BackingField;
        public static readonly string AccessTokenKey = (!Constants.IsWeb ? "access_token" : "accessToken");
        public static readonly string ExpirationTimestampKey = (!Constants.IsWeb ? "expiration_timestamp" : "expiresIn");
        public const string LastRefreshKey = "last_refresh";
        public static readonly string PermissionsKey = (!Constants.IsWeb ? "permissions" : "grantedScopes");
        public static readonly string UserIdKey = (!Constants.IsWeb ? "user_id" : "userID");

        internal LoginResult(string response) : base(response)
        {
            if ((this.ResultDictionary != null) && this.ResultDictionary.ContainsKey(AccessTokenKey))
            {
                this.AccessToken = Utilities.ParseAccessTokenFromResult(this.ResultDictionary);
            }
        }

        public Facebook.Unity.AccessToken AccessToken
        {
            [CompilerGenerated]
            get
            {
                return this.<AccessToken>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AccessToken>k__BackingField = value;
            }
        }
    }
}

