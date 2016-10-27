namespace Facebook.Unity
{
    using Facebook.Unity.Canvas;
    using Facebook.Unity.Editor;
    using Facebook.Unity.Mobile;
    using Facebook.Unity.Mobile.Android;
    using Facebook.Unity.Mobile.IOS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public sealed class FB : ScriptableObject
    {
        [CompilerGenerated]
        private static string <AppId>k__BackingField;
        [CompilerGenerated]
        private static OnDLLLoaded <OnDLLLoadedDelegate>k__BackingField;
        private const string DefaultJSSDKLocale = "en_US";
        private static IFacebook facebook;
        private static string facebookDomain = "facebook.com";
        private static string graphApiVersion = "v2.5";
        private static bool isInitCalled;

        public static void ActivateApp()
        {
            FacebookImpl.ActivateApp(AppId);
        }

        public static void API(string query, HttpMethod method, [Optional, DefaultParameterValue(null)] FacebookDelegate<IGraphResult> callback, [Optional, DefaultParameterValue(null)] IDictionary<string, string> formData)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query", "The query param cannot be null or empty");
            }
            FacebookImpl.API(query, method, formData, callback);
        }

        public static void API(string query, HttpMethod method, FacebookDelegate<IGraphResult> callback, WWWForm formData)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query", "The query param cannot be null or empty");
            }
            FacebookImpl.API(query, method, formData, callback);
        }

        public static void AppRequest(string message, OGActionType actionType, string objectId, IEnumerable<string> to, [Optional, DefaultParameterValue("")] string data, [Optional, DefaultParameterValue("")] string title, [Optional, DefaultParameterValue(null)] FacebookDelegate<IAppRequestResult> callback)
        {
            FacebookImpl.AppRequest(message, new OGActionType?(actionType), objectId, to, null, null, null, data, title, callback);
        }

        public static void AppRequest(string message, [Optional, DefaultParameterValue(null)] IEnumerable<string> to, [Optional, DefaultParameterValue(null)] IEnumerable<object> filters, [Optional, DefaultParameterValue(null)] IEnumerable<string> excludeIds, [Optional, DefaultParameterValue(null)] int? maxRecipients, [Optional, DefaultParameterValue("")] string data, [Optional, DefaultParameterValue("")] string title, [Optional, DefaultParameterValue(null)] FacebookDelegate<IAppRequestResult> callback)
        {
            FacebookImpl.AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
        }

        public static void AppRequest(string message, OGActionType actionType, string objectId, [Optional, DefaultParameterValue(null)] IEnumerable<object> filters, [Optional, DefaultParameterValue(null)] IEnumerable<string> excludeIds, [Optional, DefaultParameterValue(null)] int? maxRecipients, [Optional, DefaultParameterValue("")] string data, [Optional, DefaultParameterValue("")] string title, [Optional, DefaultParameterValue(null)] FacebookDelegate<IAppRequestResult> callback)
        {
            FacebookImpl.AppRequest(message, new OGActionType?(actionType), objectId, null, filters, excludeIds, maxRecipients, data, title, callback);
        }

        public static void FeedShare([Optional, DefaultParameterValue("")] string toId, [Optional, DefaultParameterValue(null)] Uri link, [Optional, DefaultParameterValue("")] string linkName, [Optional, DefaultParameterValue("")] string linkCaption, [Optional, DefaultParameterValue("")] string linkDescription, [Optional, DefaultParameterValue(null)] Uri picture, [Optional, DefaultParameterValue("")] string mediaSource, [Optional, DefaultParameterValue(null)] FacebookDelegate<IShareResult> callback)
        {
            FacebookImpl.FeedShare(toId, link, linkName, linkCaption, linkDescription, picture, mediaSource, callback);
        }

        public static void GameGroupCreate(string name, string description, [Optional, DefaultParameterValue("CLOSED")] string privacy, [Optional, DefaultParameterValue(null)] FacebookDelegate<IGroupCreateResult> callback)
        {
            FacebookImpl.GameGroupCreate(name, description, privacy, callback);
        }

        public static void GameGroupJoin(string id, [Optional, DefaultParameterValue(null)] FacebookDelegate<IGroupJoinResult> callback)
        {
            FacebookImpl.GameGroupJoin(id, callback);
        }

        public static void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            if (callback != null)
            {
                FacebookImpl.GetAppLink(callback);
            }
        }

        public static void Init([Optional, DefaultParameterValue(null)] InitDelegate onInitComplete, [Optional, DefaultParameterValue(null)] HideUnityDelegate onHideUnity, [Optional, DefaultParameterValue(null)] string authResponse)
        {
            Init(FacebookSettings.AppId, FacebookSettings.Cookie, FacebookSettings.Logging, FacebookSettings.Status, FacebookSettings.Xfbml, FacebookSettings.FrictionlessRequests, authResponse, "en_US", onHideUnity, onInitComplete);
        }

        public static void Init(string appId, [Optional, DefaultParameterValue(true)] bool cookie, [Optional, DefaultParameterValue(true)] bool logging, [Optional, DefaultParameterValue(true)] bool status, [Optional, DefaultParameterValue(false)] bool xfbml, [Optional, DefaultParameterValue(true)] bool frictionlessRequests, [Optional, DefaultParameterValue(null)] string authResponse, [Optional, DefaultParameterValue("en_US")] string jsSDKLocale, [Optional, DefaultParameterValue(null)] HideUnityDelegate onHideUnity, [Optional, DefaultParameterValue(null)] InitDelegate onInitComplete)
        {
            <Init>c__AnonStorey266 storey = new <Init>c__AnonStorey266();
            storey.onHideUnity = onHideUnity;
            storey.onInitComplete = onInitComplete;
            storey.appId = appId;
            storey.cookie = cookie;
            storey.logging = logging;
            storey.status = status;
            storey.xfbml = xfbml;
            storey.authResponse = authResponse;
            storey.frictionlessRequests = frictionlessRequests;
            storey.jsSDKLocale = jsSDKLocale;
            if (string.IsNullOrEmpty(storey.appId))
            {
                throw new ArgumentException("appId cannot be null or empty!");
            }
            AppId = storey.appId;
            if (!isInitCalled)
            {
                isInitCalled = true;
                if (!Constants.IsEditor)
                {
                    switch (Constants.CurrentPlatform)
                    {
                        case FacebookUnityPlatform.Android:
                            OnDLLLoadedDelegate = new OnDLLLoaded(storey.<>m__81);
                            ComponentFactory.GetComponent<AndroidFacebookLoader>(ComponentFactory.IfNotExist.AddNew);
                            return;

                        case FacebookUnityPlatform.IOS:
                            OnDLLLoadedDelegate = new OnDLLLoaded(storey.<>m__80);
                            ComponentFactory.GetComponent<IOSFacebookLoader>(ComponentFactory.IfNotExist.AddNew);
                            return;

                        case FacebookUnityPlatform.WebGL:
                        case FacebookUnityPlatform.WebPlayer:
                            OnDLLLoadedDelegate = new OnDLLLoaded(storey.<>m__7F);
                            ComponentFactory.GetComponent<CanvasFacebookLoader>(ComponentFactory.IfNotExist.AddNew);
                            return;
                    }
                    throw new NotImplementedException("Facebook API does not yet support this platform");
                }
                OnDLLLoadedDelegate = new OnDLLLoaded(storey.<>m__7E);
                ComponentFactory.GetComponent<EditorFacebookLoader>(ComponentFactory.IfNotExist.AddNew);
            }
            else
            {
                FacebookLogger.Warn("FB.Init() has already been called.  You only need to call this once and only once.");
            }
        }

        public static void LogAppEvent(string logEvent, [Optional, DefaultParameterValue(null)] float? valueToSum, [Optional, DefaultParameterValue(null)] Dictionary<string, object> parameters)
        {
            FacebookImpl.AppEventsLogEvent(logEvent, valueToSum, parameters);
        }

        public static void LogInWithPublishPermissions([Optional, DefaultParameterValue(null)] IEnumerable<string> permissions, [Optional, DefaultParameterValue(null)] FacebookDelegate<ILoginResult> callback)
        {
            FacebookImpl.LogInWithPublishPermissions(permissions, callback);
        }

        public static void LogInWithReadPermissions([Optional, DefaultParameterValue(null)] IEnumerable<string> permissions, [Optional, DefaultParameterValue(null)] FacebookDelegate<ILoginResult> callback)
        {
            FacebookImpl.LogInWithReadPermissions(permissions, callback);
        }

        public static void LogOut()
        {
            FacebookImpl.LogOut();
        }

        public static void LogPurchase(float logPurchase, [Optional, DefaultParameterValue(null)] string currency, [Optional, DefaultParameterValue(null)] Dictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(currency))
            {
                currency = "USD";
            }
            FacebookImpl.AppEventsLogPurchase(logPurchase, currency, parameters);
        }

        private static void LogVersion()
        {
            if (facebook != null)
            {
                FacebookLogger.Info(string.Format("Using Facebook Unity SDK v{0} with {1}", FacebookSdkVersion.Build, FacebookImpl.SDKUserAgent));
            }
            else
            {
                FacebookLogger.Info(string.Format("Using Facebook Unity SDK v{0}", FacebookSdkVersion.Build));
            }
        }

        public static void ShareLink([Optional, DefaultParameterValue(null)] Uri contentURL, [Optional, DefaultParameterValue("")] string contentTitle, [Optional, DefaultParameterValue("")] string contentDescription, [Optional, DefaultParameterValue(null)] Uri photoURL, [Optional, DefaultParameterValue(null)] FacebookDelegate<IShareResult> callback)
        {
            FacebookImpl.ShareLink(contentURL, contentTitle, contentDescription, photoURL, callback);
        }

        public static string AppId
        {
            [CompilerGenerated]
            get
            {
                return <AppId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                <AppId>k__BackingField = value;
            }
        }

        internal static string FacebookDomain
        {
            get
            {
                return facebookDomain;
            }
            set
            {
                facebookDomain = value;
            }
        }

        internal static IFacebook FacebookImpl
        {
            get
            {
                if (facebook == null)
                {
                    throw new NullReferenceException("Facebook object is not yet loaded.  Did you call FB.Init()?");
                }
                return facebook;
            }
            set
            {
                facebook = value;
            }
        }

        public static string GraphApiVersion
        {
            get
            {
                return graphApiVersion;
            }
            set
            {
                graphApiVersion = value;
            }
        }

        public static bool IsInitialized
        {
            get
            {
                return ((facebook != null) && facebook.Initialized);
            }
        }

        public static bool IsLoggedIn
        {
            get
            {
                return ((facebook != null) && FacebookImpl.LoggedIn);
            }
        }

        public static bool LimitAppEventUsage
        {
            get
            {
                return ((facebook != null) && facebook.LimitEventUsage);
            }
            set
            {
                if (facebook != null)
                {
                    facebook.LimitEventUsage = value;
                }
            }
        }

        private static OnDLLLoaded OnDLLLoadedDelegate
        {
            [CompilerGenerated]
            get
            {
                return <OnDLLLoadedDelegate>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <OnDLLLoadedDelegate>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <Init>c__AnonStorey266
        {
            internal string appId;
            internal string authResponse;
            internal bool cookie;
            internal bool frictionlessRequests;
            internal string jsSDKLocale;
            internal bool logging;
            internal HideUnityDelegate onHideUnity;
            internal InitDelegate onInitComplete;
            internal bool status;
            internal bool xfbml;

            internal void <>m__7E()
            {
                ((EditorFacebook) FB.facebook).Init(this.onHideUnity, this.onInitComplete);
            }

            internal void <>m__7F()
            {
                ((CanvasFacebook) FB.facebook).Init(this.appId, this.cookie, this.logging, this.status, this.xfbml, FacebookSettings.ChannelUrl, this.authResponse, this.frictionlessRequests, this.jsSDKLocale, this.onHideUnity, this.onInitComplete);
            }

            internal void <>m__80()
            {
                ((IOSFacebook) FB.facebook).Init(this.appId, this.frictionlessRequests, this.onHideUnity, this.onInitComplete);
            }

            internal void <>m__81()
            {
                ((AndroidFacebook) FB.facebook).Init(this.appId, this.onHideUnity, this.onInitComplete);
            }
        }

        public sealed class Android
        {
            public static string KeyHash
            {
                get
                {
                    AndroidFacebook facebookImpl = FB.FacebookImpl as AndroidFacebook;
                    return ((facebookImpl == null) ? string.Empty : facebookImpl.KeyHash);
                }
            }
        }

        public sealed class Canvas
        {
            public static void Pay(string product, [Optional, DefaultParameterValue("purchaseitem")] string action, [Optional, DefaultParameterValue(1)] int quantity, [Optional, DefaultParameterValue(null)] int? quantityMin, [Optional, DefaultParameterValue(null)] int? quantityMax, [Optional, DefaultParameterValue(null)] string requestId, [Optional, DefaultParameterValue(null)] string pricepointId, [Optional, DefaultParameterValue(null)] string testCurrency, [Optional, DefaultParameterValue(null)] FacebookDelegate<IPayResult> callback)
            {
                CanvasFacebookImpl.Pay(product, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, callback);
            }

            private static ICanvasFacebook CanvasFacebookImpl
            {
                get
                {
                    ICanvasFacebook facebookImpl = FB.FacebookImpl as ICanvasFacebook;
                    if (facebookImpl == null)
                    {
                        throw new InvalidOperationException("Attempt to call Canvas interface on non canvas platform");
                    }
                    return facebookImpl;
                }
            }
        }

        internal abstract class CompiledFacebookLoader : MonoBehaviour
        {
            protected CompiledFacebookLoader()
            {
            }

            public void Start()
            {
                FB.facebook = this.FBGameObject.Facebook;
                FB.OnDLLLoadedDelegate();
                FB.LogVersion();
                UnityEngine.Object.Destroy(this);
            }

            protected abstract FacebookGameObject FBGameObject { get; }
        }

        public sealed class Mobile
        {
            public static void AppInvite(Uri appLinkUrl, [Optional, DefaultParameterValue(null)] Uri previewImageUrl, [Optional, DefaultParameterValue(null)] FacebookDelegate<IAppInviteResult> callback)
            {
                MobileFacebookImpl.AppInvite(appLinkUrl, previewImageUrl, callback);
            }

            public static void FetchDeferredAppLinkData([Optional, DefaultParameterValue(null)] FacebookDelegate<IAppLinkResult> callback)
            {
                if (callback != null)
                {
                    MobileFacebookImpl.FetchDeferredAppLink(callback);
                }
            }

            public static void RefreshCurrentAccessToken([Optional, DefaultParameterValue(null)] FacebookDelegate<IAccessTokenRefreshResult> callback)
            {
                MobileFacebookImpl.RefreshCurrentAccessToken(callback);
            }

            private static IMobileFacebook MobileFacebookImpl
            {
                get
                {
                    IMobileFacebook facebookImpl = FB.FacebookImpl as IMobileFacebook;
                    if (facebookImpl == null)
                    {
                        throw new InvalidOperationException("Attempt to call Mobile interface on non mobile platform");
                    }
                    return facebookImpl;
                }
            }

            public static Facebook.Unity.ShareDialogMode ShareDialogMode
            {
                get
                {
                    return MobileFacebookImpl.ShareDialogMode;
                }
                set
                {
                    MobileFacebookImpl.ShareDialogMode = value;
                }
            }
        }

        private delegate void OnDLLLoaded();
    }
}

