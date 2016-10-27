namespace Facebook.Unity
{
    using System;
    using System.Globalization;
    using UnityEngine;

    internal static class Constants
    {
        public const string AccessTokenKey = "access_token";
        public const string CallbackIdKey = "callback_id";
        public const string CancelledKey = "cancelled";
        private static FacebookUnityPlatform? currentPlatform;
        public const string EmailPermission = "email";
        public const string ErrorKey = "error";
        public const string ExtrasKey = "extras";
        public const string GraphApiVersion = "v2.5";
        public const string GraphUrlFormat = "https://graph.{0}/{1}/";
        public const string OnAppRequestsCompleteMethodName = "OnAppRequestsComplete";
        public const string OnGroupCreateCompleteMethodName = "OnGroupCreateComplete";
        public const string OnGroupJoinCompleteMethodName = "OnJoinGroupComplete";
        public const string OnPayCompleteMethodName = "OnPayComplete";
        public const string OnShareCompleteMethodName = "OnShareLinkComplete";
        public const string PublishActionsPermission = "publish_actions";
        public const string PublishPagesPermission = "publish_pages";
        public const string RefKey = "ref";
        public const string TargetUrlKey = "target_url";
        public const string UrlKey = "url";
        public const string UserLikesPermission = "user_likes";

        private static FacebookUnityPlatform GetCurrentPlatform()
        {
            RuntimePlatform platform = Application.platform;
            switch (platform)
            {
                case RuntimePlatform.OSXWebPlayer:
                case RuntimePlatform.WindowsWebPlayer:
                    return FacebookUnityPlatform.WebPlayer;

                case RuntimePlatform.IPhonePlayer:
                    return FacebookUnityPlatform.IOS;
            }
            if (platform != RuntimePlatform.Android)
            {
                if (platform == RuntimePlatform.WebGLPlayer)
                {
                    return FacebookUnityPlatform.WebGL;
                }
                return FacebookUnityPlatform.Unknown;
            }
            return FacebookUnityPlatform.Android;
        }

        public static FacebookUnityPlatform CurrentPlatform
        {
            get
            {
                if (!currentPlatform.HasValue)
                {
                    currentPlatform = new FacebookUnityPlatform?(GetCurrentPlatform());
                }
                return currentPlatform.Value;
            }
            set
            {
                currentPlatform = new FacebookUnityPlatform?(value);
            }
        }

        public static bool DebugMode
        {
            get
            {
                return Debug.isDebugBuild;
            }
        }

        public static string GraphApiUserAgent
        {
            get
            {
                object[] args = new object[] { FB.FacebookImpl.SDKUserAgent, UnitySDKUserAgent };
                return string.Format(CultureInfo.InvariantCulture, "{0} {1}", args);
            }
        }

        public static Uri GraphUrl
        {
            get
            {
                object[] args = new object[] { FB.FacebookDomain, FB.GraphApiVersion };
                return new Uri(string.Format(CultureInfo.InvariantCulture, "https://graph.{0}/{1}/", args));
            }
        }

        public static bool IsEditor
        {
            get
            {
                return false;
            }
        }

        public static bool IsMobile
        {
            get
            {
                return ((CurrentPlatform == FacebookUnityPlatform.Android) || (CurrentPlatform == FacebookUnityPlatform.IOS));
            }
        }

        public static bool IsWeb
        {
            get
            {
                return ((CurrentPlatform == FacebookUnityPlatform.WebGL) || (CurrentPlatform == FacebookUnityPlatform.WebPlayer));
            }
        }

        public static string UnitySDKUserAgent
        {
            get
            {
                return Utilities.GetUserAgent("FBUnitySDK", FacebookSdkVersion.Build);
            }
        }

        public static string UnitySDKUserAgentSuffixLegacy
        {
            get
            {
                object[] args = new object[] { FacebookSdkVersion.Build };
                return string.Format(CultureInfo.InvariantCulture, "Unity.{0}", args);
            }
        }
    }
}

