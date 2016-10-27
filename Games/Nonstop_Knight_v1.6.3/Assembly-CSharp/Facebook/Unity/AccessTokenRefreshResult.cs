namespace Facebook.Unity
{
    using System;
    using System.Runtime.CompilerServices;

    internal class AccessTokenRefreshResult : ResultBase, IAccessTokenRefreshResult, IResult
    {
        [CompilerGenerated]
        private Facebook.Unity.AccessToken <AccessToken>k__BackingField;

        public AccessTokenRefreshResult(string result) : base(result)
        {
            if ((this.ResultDictionary != null) && this.ResultDictionary.ContainsKey(LoginResult.AccessTokenKey))
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

