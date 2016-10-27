namespace GooglePlayGames
{
    using System;

    public static class GameInfo
    {
        public const string ApplicationId = "995406906094";
        public const string IosClientId = "";
        public const string NearbyConnectionServiceId = "";
        private const string UnescapedApplicationId = "APP_ID";
        private const string UnescapedIosClientId = "IOS_CLIENTID";
        private const string UnescapedNearbyServiceId = "NEARBY_SERVICE_ID";
        private const string UnescapedRequireGooglePlus = "REQUIRE_GOOGLE_PLUS";
        private const string UnescapedWebClientId = "WEB_CLIENTID";
        public const string WebClientId = "995406906094-gv9jtvln7nl7b44p7od2agqr983j3jdu.apps.googleusercontent.com";

        public static bool ApplicationIdInitialized()
        {
            return (!string.IsNullOrEmpty("995406906094") && !"995406906094".Equals(ToEscapedToken("APP_ID")));
        }

        public static bool IosClientIdInitialized()
        {
            return (!string.IsNullOrEmpty(string.Empty) && !string.Empty.Equals(ToEscapedToken("IOS_CLIENTID")));
        }

        public static bool NearbyConnectionsInitialized()
        {
            return (!string.IsNullOrEmpty(string.Empty) && !string.Empty.Equals(ToEscapedToken("NEARBY_SERVICE_ID")));
        }

        public static bool RequireGooglePlus()
        {
            return false;
        }

        private static string ToEscapedToken(string token)
        {
            return string.Format("__{0}__", token);
        }

        public static bool WebClientIdInitialized()
        {
            return (!string.IsNullOrEmpty("995406906094-gv9jtvln7nl7b44p7od2agqr983j3jdu.apps.googleusercontent.com") && !"995406906094-gv9jtvln7nl7b44p7od2agqr983j3jdu.apps.googleusercontent.com".Equals(ToEscapedToken("WEB_CLIENTID")));
        }
    }
}

