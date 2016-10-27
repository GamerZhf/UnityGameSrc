namespace Facebook.Unity.Mobile.Android
{
    using Facebook.Unity;
    using Facebook.Unity.Mobile;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal sealed class AndroidFacebook : MobileFacebook
    {
        [CompilerGenerated]
        private string <KeyHash>k__BackingField;
        private IAndroidJavaClass facebookJava;
        private bool limitEventUsage;
        public const string LoginPermissionsKey = "scope";

        public AndroidFacebook() : this(new FBJavaClass(), new CallbackManager())
        {
        }

        public AndroidFacebook(IAndroidJavaClass facebookJavaClass, CallbackManager callbackManager) : base(callbackManager)
        {
            this.KeyHash = string.Empty;
            this.facebookJava = facebookJavaClass;
        }

        public override void ActivateApp(string appId)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("app_id", appId);
            new JavaMethodCall<IResult>(this, "ActivateApp").Call(args);
        }

        public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("logEvent", logEvent);
            args.AddNullablePrimitive<float>("valueToSum", valueToSum);
            args.AddDictionary("parameters", parameters);
            new JavaMethodCall<IResult>(this, "LogAppEvent").Call(args);
        }

        public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
        {
            MethodArguments args = new MethodArguments();
            args.AddPrimative<float>("logPurchase", logPurchase);
            args.AddString("currency", currency);
            args.AddDictionary("parameters", parameters);
            new JavaMethodCall<IResult>(this, "LogAppEvent").Call(args);
        }

        public override void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddUri("appLinkUrl", appLinkUrl);
            args.AddUri("previewImageUrl", previewImageUrl);
            JavaMethodCall<IAppInviteResult> call = new JavaMethodCall<IAppInviteResult>(this, "AppInvite");
            call.Callback = callback;
            call.Call(args);
        }

        public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
        {
            base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
            MethodArguments args = new MethodArguments();
            args.AddString("message", message);
            args.AddNullablePrimitive<OGActionType>("action_type", actionType);
            args.AddString("object_id", objectId);
            args.AddCommaSeparatedList("to", to);
            if ((filters != null) && Enumerable.Any<object>(filters))
            {
                string str = Enumerable.First<object>(filters) as string;
                if (str != null)
                {
                    args.AddString("filters", str);
                }
            }
            args.AddNullablePrimitive<int>("max_recipients", maxRecipients);
            args.AddString("data", data);
            args.AddString("title", title);
            JavaMethodCall<IAppRequestResult> call = new JavaMethodCall<IAppRequestResult>(this, "AppRequest");
            call.Callback = callback;
            call.Call(args);
        }

        private void CallFB(string method, string args)
        {
            object[] objArray1 = new object[] { args };
            this.facebookJava.CallStatic(method, objArray1);
        }

        public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("toId", toId);
            args.AddUri("link", link);
            args.AddString("linkName", linkName);
            args.AddString("linkCaption", linkCaption);
            args.AddString("linkDescription", linkDescription);
            args.AddUri("picture", picture);
            args.AddString("mediaSource", mediaSource);
            JavaMethodCall<IShareResult> call = new JavaMethodCall<IShareResult>(this, "FeedShare");
            call.Callback = callback;
            call.Call(args);
        }

        public override void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            MethodArguments args = new MethodArguments();
            JavaMethodCall<IAppLinkResult> call = new JavaMethodCall<IAppLinkResult>(this, "FetchDeferredAppLinkData");
            call.Callback = callback;
            call.Call(args);
        }

        public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("name", name);
            args.AddString("description", description);
            args.AddString("privacy", privacy);
            JavaMethodCall<IGroupCreateResult> call = new JavaMethodCall<IGroupCreateResult>(this, "GameGroupCreate");
            call.Callback = callback;
            call.Call(args);
        }

        public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("id", id);
            JavaMethodCall<IGroupJoinResult> call = new JavaMethodCall<IGroupJoinResult>(this, "GameGroupJoin");
            call.Callback = callback;
            call.Call(args);
        }

        public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            JavaMethodCall<IAppLinkResult> call = new JavaMethodCall<IAppLinkResult>(this, "GetAppLink");
            call.Callback = callback;
            call.Call(null);
        }

        public void Init(string appId, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
        {
            this.CallFB("SetUserAgentSuffix", string.Format("Unity.{0}", Constants.UnitySDKUserAgentSuffixLegacy));
            base.Init(hideUnityDelegate, onInitComplete);
            MethodArguments args = new MethodArguments();
            args.AddString("appId", appId);
            new JavaMethodCall<IResult>(this, "Init").Call(args);
        }

        public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddCommaSeparatedList("scope", permissions);
            JavaMethodCall<ILoginResult> call = new JavaMethodCall<ILoginResult>(this, "LoginWithPublishPermissions");
            call.Callback = callback;
            call.Call(args);
        }

        public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddCommaSeparatedList("scope", permissions);
            JavaMethodCall<ILoginResult> call = new JavaMethodCall<ILoginResult>(this, "LoginWithReadPermissions");
            call.Callback = callback;
            call.Call(args);
        }

        public override void LogOut()
        {
            new JavaMethodCall<IResult>(this, "Logout").Call(null);
        }

        public override void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
        {
            JavaMethodCall<IAccessTokenRefreshResult> call = new JavaMethodCall<IAccessTokenRefreshResult>(this, "RefreshCurrentAccessToken");
            call.Callback = callback;
            call.Call(null);
        }

        protected override void SetShareDialogMode(ShareDialogMode mode)
        {
            this.CallFB("SetShareDialogMode", mode.ToString());
        }

        public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddUri("content_url", contentURL);
            args.AddString("content_title", contentTitle);
            args.AddString("content_description", contentDescription);
            args.AddUri("photo_url", photoURL);
            JavaMethodCall<IShareResult> call = new JavaMethodCall<IShareResult>(this, "ShareLink");
            call.Callback = callback;
            call.Call(args);
        }

        public string KeyHash
        {
            [CompilerGenerated]
            get
            {
                return this.<KeyHash>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<KeyHash>k__BackingField = value;
            }
        }

        public override bool LimitEventUsage
        {
            get
            {
                return this.limitEventUsage;
            }
            set
            {
                this.limitEventUsage = value;
                this.CallFB("SetLimitEventUsage", value.ToString());
            }
        }

        public override string SDKName
        {
            get
            {
                return "FBAndroidSDK";
            }
        }

        public override string SDKVersion
        {
            get
            {
                return this.facebookJava.CallStatic<string>("GetSdkVersion");
            }
        }

        private class JavaMethodCall<T> : MethodCall<T> where T: IResult
        {
            private AndroidFacebook androidImpl;

            public JavaMethodCall(AndroidFacebook androidImpl, string methodName) : base(androidImpl, methodName)
            {
                this.androidImpl = androidImpl;
            }

            public override void Call([Optional, DefaultParameterValue(null)] MethodArguments args)
            {
                MethodArguments arguments;
                if (args == null)
                {
                    arguments = new MethodArguments();
                }
                else
                {
                    arguments = new MethodArguments(args);
                }
                if (base.Callback != null)
                {
                    arguments.AddString("callback_id", this.androidImpl.CallbackManager.AddFacebookDelegate<T>(base.Callback));
                }
                this.androidImpl.CallFB(base.MethodName, arguments.ToJsonString());
            }
        }
    }
}

