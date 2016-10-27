namespace Facebook.Unity.Canvas
{
    using Facebook.MiniJSON;
    using Facebook.Unity;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal sealed class CanvasFacebook : FacebookBase, ICanvasFacebook, ICanvasFacebookCallbackHandler, ICanvasFacebookImplementation, IFacebook, IFacebookCallbackHandler
    {
        [CompilerGenerated]
        private bool <LimitEventUsage>k__BackingField;
        private string appId;
        private string appLinkUrl;
        private const string AuthResponseKey = "authResponse";
        internal const string CancelledResponse = "{\"cancelled\":true}";
        private ICanvasJSWrapper canvasJSWrapper;
        internal const string FacebookConnectURL = "https://connect.facebook.net";
        internal const string MethodAppRequests = "apprequests";
        internal const string MethodFeed = "feed";
        internal const string MethodGameGroupCreate = "game_group_create";
        internal const string MethodGameGroupJoin = "game_group_join";
        internal const string MethodPay = "pay";
        private const string ResponseKey = "response";

        public CanvasFacebook() : this(new CanvasJSWrapper(), new CallbackManager())
        {
        }

        public CanvasFacebook(ICanvasJSWrapper canvasJSWrapper, CallbackManager callbackManager) : base(callbackManager)
        {
            this.canvasJSWrapper = canvasJSWrapper;
        }

        public override void ActivateApp(string appId)
        {
            this.canvasJSWrapper.ExternalCall("FBUnity.activateApp", new object[0]);
        }

        public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
        {
            object[] args = new object[] { logEvent, valueToSum, Json.Serialize(parameters) };
            this.canvasJSWrapper.ExternalCall("FBUnity.logAppEvent", args);
        }

        public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
        {
            object[] args = new object[] { logPurchase, currency, Json.Serialize(parameters) };
            this.canvasJSWrapper.ExternalCall("FBUnity.logPurchase", args);
        }

        public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
        {
            base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
            MethodArguments args = new MethodArguments();
            args.AddString("message", message);
            args.AddCommaSeparatedList("to", to);
            args.AddString("action_type", !actionType.HasValue ? null : actionType.ToString());
            args.AddString("object_id", objectId);
            args.AddList<object>("filters", filters);
            args.AddList<string>("exclude_ids", excludeIds);
            args.AddNullablePrimitive<int>("max_recipients", maxRecipients);
            args.AddString("data", data);
            args.AddString("title", title);
            CanvasUIMethodCall<IAppRequestResult> call = new CanvasUIMethodCall<IAppRequestResult>(this, "apprequests", "OnAppRequestsComplete");
            call.Callback = callback;
            call.Call(args);
        }

        public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("to", toId);
            args.AddUri("link", link);
            args.AddString("name", linkName);
            args.AddString("caption", linkCaption);
            args.AddString("description", linkDescription);
            args.AddUri("picture", picture);
            args.AddString("source", mediaSource);
            CanvasUIMethodCall<IShareResult> call = new CanvasUIMethodCall<IShareResult>(this, "feed", "OnShareLinkComplete");
            call.Callback = callback;
            call.Call(args);
        }

        private static string FormatAuthResponse(string result)
        {
            IDictionary<string, object> dictionary2;
            if (string.IsNullOrEmpty(result))
            {
                return result;
            }
            IDictionary<string, object> formattedResponseDictionary = GetFormattedResponseDictionary(result);
            if (Utilities.TryGetValue<IDictionary<string, object>>(formattedResponseDictionary, "authResponse", out dictionary2))
            {
                formattedResponseDictionary.Remove("authResponse");
                IEnumerator<KeyValuePair<string, object>> enumerator = dictionary2.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<string, object> current = enumerator.Current;
                        formattedResponseDictionary[current.Key] = current.Value;
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
            }
            return Json.Serialize(formattedResponseDictionary);
        }

        private static string FormatResult(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return result;
            }
            return Json.Serialize(GetFormattedResponseDictionary(result));
        }

        public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("name", name);
            args.AddString("description", description);
            args.AddString("privacy", privacy);
            args.AddString("display", "async");
            CanvasUIMethodCall<IGroupCreateResult> call = new CanvasUIMethodCall<IGroupCreateResult>(this, "game_group_create", "OnGroupCreateComplete");
            call.Callback = callback;
            call.Call(args);
        }

        public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("id", id);
            args.AddString("display", "async");
            CanvasUIMethodCall<IGroupJoinResult> call = new CanvasUIMethodCall<IGroupJoinResult>(this, "game_group_join", "OnJoinGroupComplete");
            call.Callback = callback;
            call.Call(args);
        }

        public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
            dictionary2.Add("url", this.appLinkUrl);
            Dictionary<string, object> dictionary = dictionary2;
            callback(new AppLinkResult(Json.Serialize(dictionary)));
            this.appLinkUrl = string.Empty;
        }

        private static IDictionary<string, object> GetFormattedResponseDictionary(string result)
        {
            IDictionary<string, object> dictionary2;
            object obj2;
            IDictionary<string, object> dictionary = (IDictionary<string, object>) Json.Deserialize(result);
            if (!Utilities.TryGetValue<IDictionary<string, object>>(dictionary, "response", out dictionary2))
            {
                return dictionary;
            }
            if (dictionary.TryGetValue("callback_id", out obj2))
            {
                dictionary2["callback_id"] = obj2;
            }
            return dictionary2;
        }

        public void Init(string appId, bool cookie, bool logging, bool status, bool xfbml, string channelUrl, string authResponse, bool frictionlessRequests, string jsSDKLocale, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
        {
            if (this.canvasJSWrapper.IntegrationMethodJs == null)
            {
                throw new Exception("Cannot initialize facebook javascript");
            }
            base.Init(hideUnityDelegate, onInitComplete);
            this.canvasJSWrapper.ExternalEval(this.canvasJSWrapper.IntegrationMethodJs);
            this.appId = appId;
            bool flag = true;
            MethodArguments arguments = new MethodArguments();
            arguments.AddString("appId", appId);
            arguments.AddPrimative<bool>("cookie", cookie);
            arguments.AddPrimative<bool>("logging", logging);
            arguments.AddPrimative<bool>("status", status);
            arguments.AddPrimative<bool>("xfbml", xfbml);
            arguments.AddString("channelUrl", channelUrl);
            arguments.AddString("authResponse", authResponse);
            arguments.AddPrimative<bool>("frictionlessRequests", frictionlessRequests);
            arguments.AddString("version", FB.GraphApiVersion);
            object[] args = new object[] { !flag ? 0 : 1, "https://connect.facebook.net", jsSDKLocale, !Constants.DebugMode ? 0 : 1, arguments.ToJsonString(), !status ? 0 : 1 };
            this.canvasJSWrapper.ExternalCall("FBUnity.init", args);
        }

        public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
        {
            this.canvasJSWrapper.DisableFullScreen();
            object[] args = new object[] { permissions, base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback) };
            this.canvasJSWrapper.ExternalCall("FBUnity.login", args);
        }

        public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
        {
            this.canvasJSWrapper.DisableFullScreen();
            object[] args = new object[] { permissions, base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback) };
            this.canvasJSWrapper.ExternalCall("FBUnity.login", args);
        }

        public override void LogOut()
        {
            base.LogOut();
            this.canvasJSWrapper.ExternalCall("FBUnity.logout", new object[0]);
        }

        public override void OnAppRequestsComplete(string responseJsonData)
        {
            AppRequestResult result = new AppRequestResult(FormatResult(responseJsonData));
            base.CallbackManager.OnFacebookResponse(result);
        }

        public void OnFacebookAuthResponseChange(string responseJsonData)
        {
            LoginResult result = new LoginResult(FormatAuthResponse(responseJsonData));
            AccessToken.CurrentAccessToken = result.AccessToken;
        }

        public override void OnGetAppLinkComplete(string message)
        {
            throw new NotImplementedException();
        }

        public override void OnGroupCreateComplete(string responseJsonData)
        {
            GroupCreateResult result = new GroupCreateResult(FormatResult(responseJsonData));
            base.CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGroupJoinComplete(string responseJsonData)
        {
            GroupJoinResult result = new GroupJoinResult(FormatResult(responseJsonData));
            base.CallbackManager.OnFacebookResponse(result);
        }

        public override void OnLoginComplete(string responseJsonData)
        {
            string response = FormatAuthResponse(responseJsonData);
            base.OnAuthResponse(new LoginResult(response));
        }

        public void OnPayComplete(string responseJsonData)
        {
            PayResult result = new PayResult(FormatResult(responseJsonData));
            base.CallbackManager.OnFacebookResponse(result);
        }

        public override void OnShareLinkComplete(string responseJsonData)
        {
            ShareResult result = new ShareResult(FormatResult(responseJsonData));
            base.CallbackManager.OnFacebookResponse(result);
        }

        public void OnUrlResponse(string url)
        {
            this.appLinkUrl = url;
        }

        public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddString("product", product);
            args.AddString("action", action);
            args.AddPrimative<int>("quantity", quantity);
            args.AddNullablePrimitive<int>("quantity_min", quantityMin);
            args.AddNullablePrimitive<int>("quantity_max", quantityMax);
            args.AddString("request_id", requestId);
            args.AddString("pricepoint_id", pricepointId);
            args.AddString("test_currency", testCurrency);
            CanvasUIMethodCall<IPayResult> call = new CanvasUIMethodCall<IPayResult>(this, "pay", "OnPayComplete");
            call.Callback = callback;
            call.Call(args);
        }

        public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
        {
            MethodArguments args = new MethodArguments();
            args.AddUri("link", contentURL);
            args.AddString("name", contentTitle);
            args.AddString("description", contentDescription);
            args.AddUri("picture", photoURL);
            CanvasUIMethodCall<IShareResult> call = new CanvasUIMethodCall<IShareResult>(this, "feed", "OnShareLinkComplete");
            call.Callback = callback;
            call.Call(args);
        }

        public override bool LimitEventUsage
        {
            [CompilerGenerated]
            get
            {
                return this.<LimitEventUsage>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LimitEventUsage>k__BackingField = value;
            }
        }

        public override string SDKName
        {
            get
            {
                return "FBJSSDK";
            }
        }

        public override string SDKUserAgent
        {
            get
            {
                string str;
                switch (Constants.CurrentPlatform)
                {
                    case FacebookUnityPlatform.WebGL:
                    case FacebookUnityPlatform.WebPlayer:
                    {
                        object[] objArray1 = new object[] { Constants.CurrentPlatform.ToString() };
                        str = string.Format(CultureInfo.InvariantCulture, "FBUnity{0}", objArray1);
                        break;
                    }
                    default:
                        FacebookLogger.Warn("Currently running on uknown web platform");
                        str = "FBUnityWebUnknown";
                        break;
                }
                object[] args = new object[] { base.SDKUserAgent, Utilities.GetUserAgent(str, FacebookSdkVersion.Build) };
                return string.Format(CultureInfo.InvariantCulture, "{0} {1}", args);
            }
        }

        public override string SDKVersion
        {
            get
            {
                return this.canvasJSWrapper.GetSDKVersion();
            }
        }

        private class CanvasUIMethodCall<T> : MethodCall<T> where T: IResult
        {
            private string callbackMethod;
            private CanvasFacebook canvasImpl;

            public CanvasUIMethodCall(CanvasFacebook canvasImpl, string methodName, string callbackMethod) : base(canvasImpl, methodName)
            {
                this.canvasImpl = canvasImpl;
                this.callbackMethod = callbackMethod;
            }

            public override void Call(MethodArguments args)
            {
                this.UI(base.MethodName, args, base.Callback);
            }

            private void UI(string method, MethodArguments args, [Optional, DefaultParameterValue(null)] FacebookDelegate<T> callback)
            {
                this.canvasImpl.canvasJSWrapper.DisableFullScreen();
                MethodArguments arguments = new MethodArguments(args);
                arguments.AddString("app_id", this.canvasImpl.appId);
                arguments.AddString("method", method);
                string str = this.canvasImpl.CallbackManager.AddFacebookDelegate<T>(callback);
                object[] objArray1 = new object[] { arguments.ToJsonString(), str, this.callbackMethod };
                this.canvasImpl.canvasJSWrapper.ExternalCall("FBUnity.ui", objArray1);
            }
        }
    }
}

