namespace Facebook.Unity.Mobile.IOS
{
    using Facebook.Unity;
    using Facebook.Unity.Mobile;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal class IOSFacebook : MobileFacebook
    {
        private const string CancelledResponse = "{\"cancelled\":true}";
        private IIOSWrapper iosWrapper;
        private bool limitEventUsage;

        public IOSFacebook() : this(new IOSWrapper(), new CallbackManager())
        {
        }

        public IOSFacebook(IIOSWrapper iosWrapper, CallbackManager callbackManager) : base(callbackManager)
        {
            this.iosWrapper = iosWrapper;
        }

        public override void ActivateApp(string appId)
        {
            this.iosWrapper.FBSettingsActivateApp(appId);
        }

        private int AddCallback<T>(FacebookDelegate<T> callback) where T: IResult
        {
            return Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<T>(callback));
        }

        public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
        {
            NativeDict dict = MarshallDict(parameters);
            if (valueToSum.HasValue)
            {
                this.iosWrapper.LogAppEvent(logEvent, (double) valueToSum.Value, dict.NumEntries, dict.Keys, dict.Values);
            }
            else
            {
                this.iosWrapper.LogAppEvent(logEvent, 0.0, dict.NumEntries, dict.Keys, dict.Values);
            }
        }

        public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
        {
            NativeDict dict = MarshallDict(parameters);
            this.iosWrapper.LogPurchaseAppEvent((double) logPurchase, currency, dict.NumEntries, dict.Keys, dict.Values);
        }

        public override void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
        {
            string absoluteUri = string.Empty;
            string str2 = string.Empty;
            if ((appLinkUrl != null) && !string.IsNullOrEmpty(appLinkUrl.AbsoluteUri))
            {
                absoluteUri = appLinkUrl.AbsoluteUri;
            }
            if ((previewImageUrl != null) && !string.IsNullOrEmpty(previewImageUrl.AbsoluteUri))
            {
                str2 = previewImageUrl.AbsoluteUri;
            }
            this.iosWrapper.AppInvite(this.AddCallback<IAppInviteResult>(callback), absoluteUri, str2);
        }

        public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
        {
            base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
            string str = null;
            if ((filters != null) && Enumerable.Any<object>(filters))
            {
                str = Enumerable.First<object>(filters) as string;
            }
            this.iosWrapper.AppRequest(this.AddCallback<IAppRequestResult>(callback), message, !actionType.HasValue ? string.Empty : actionType.ToString(), (objectId == null) ? string.Empty : objectId, (to == null) ? null : Enumerable.ToArray<string>(to), (to == null) ? 0 : Enumerable.Count<string>(to), (str == null) ? string.Empty : str, (excludeIds == null) ? null : Enumerable.ToArray<string>(excludeIds), (excludeIds == null) ? 0 : Enumerable.Count<string>(excludeIds), maxRecipients.HasValue, !maxRecipients.HasValue ? 0 : maxRecipients.Value, data, title);
        }

        public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
        {
            string str = (link == null) ? string.Empty : link.ToString();
            string str2 = (picture == null) ? string.Empty : picture.ToString();
            this.iosWrapper.FeedShare(this.AddCallback<IShareResult>(callback), toId, str, linkName, linkCaption, linkDescription, str2, mediaSource);
        }

        public override void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            this.iosWrapper.FetchDeferredAppLink(this.AddCallback<IAppLinkResult>(callback));
        }

        public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
        {
            this.iosWrapper.CreateGameGroup(this.AddCallback<IGroupCreateResult>(callback), name, description, privacy);
        }

        public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
        {
            this.iosWrapper.JoinGameGroup(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IGroupJoinResult>(callback)), id);
        }

        public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            this.iosWrapper.GetAppLink(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IAppLinkResult>(callback)));
        }

        public void Init(string appId, bool frictionlessRequests, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
        {
            base.Init(hideUnityDelegate, onInitComplete);
            this.iosWrapper.Init(appId, frictionlessRequests, FacebookSettings.IosURLSuffix, Constants.UnitySDKUserAgentSuffixLegacy);
        }

        public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
        {
            this.iosWrapper.LogInWithPublishPermissions(this.AddCallback<ILoginResult>(callback), Utilities.ToCommaSeparateList(permissions));
        }

        public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
        {
            this.iosWrapper.LogInWithReadPermissions(this.AddCallback<ILoginResult>(callback), Utilities.ToCommaSeparateList(permissions));
        }

        public override void LogOut()
        {
            base.LogOut();
            this.iosWrapper.LogOut();
        }

        private static NativeDict MarshallDict(Dictionary<string, object> dict)
        {
            NativeDict dict2 = new NativeDict();
            if ((dict != null) && (dict.Count > 0))
            {
                dict2.Keys = new string[dict.Count];
                dict2.Values = new string[dict.Count];
                dict2.NumEntries = 0;
                foreach (KeyValuePair<string, object> pair in dict)
                {
                    dict2.Keys[dict2.NumEntries] = pair.Key;
                    dict2.Values[dict2.NumEntries] = pair.Value.ToString();
                    dict2.NumEntries++;
                }
            }
            return dict2;
        }

        private static NativeDict MarshallDict(Dictionary<string, string> dict)
        {
            NativeDict dict2 = new NativeDict();
            if ((dict != null) && (dict.Count > 0))
            {
                dict2.Keys = new string[dict.Count];
                dict2.Values = new string[dict.Count];
                dict2.NumEntries = 0;
                foreach (KeyValuePair<string, string> pair in dict)
                {
                    dict2.Keys[dict2.NumEntries] = pair.Key;
                    dict2.Values[dict2.NumEntries] = pair.Value;
                    dict2.NumEntries++;
                }
            }
            return dict2;
        }

        public override void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
        {
            this.iosWrapper.RefreshCurrentAccessToken(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IAccessTokenRefreshResult>(callback)));
        }

        protected override void SetShareDialogMode(ShareDialogMode mode)
        {
            this.iosWrapper.SetShareDialogMode((int) mode);
        }

        public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
        {
            this.iosWrapper.ShareLink(this.AddCallback<IShareResult>(callback), Utilities.AbsoluteUrlOrEmptyString(contentURL), contentTitle, contentDescription, Utilities.AbsoluteUrlOrEmptyString(photoURL));
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
                this.iosWrapper.FBAppEventsSetLimitEventUsage(value);
            }
        }

        public override string SDKName
        {
            get
            {
                return "FBiOSSDK";
            }
        }

        public override string SDKVersion
        {
            get
            {
                return this.iosWrapper.FBSdkVersion();
            }
        }

        public enum FBInsightsFlushBehavior
        {
            FBInsightsFlushBehaviorAuto,
            FBInsightsFlushBehaviorExplicitOnly
        }

        private class NativeDict
        {
            [CompilerGenerated]
            private string[] <Keys>k__BackingField;
            [CompilerGenerated]
            private int <NumEntries>k__BackingField;
            [CompilerGenerated]
            private string[] <Values>k__BackingField;

            public NativeDict()
            {
                this.NumEntries = 0;
                this.Keys = null;
                this.Values = null;
            }

            public string[] Keys
            {
                [CompilerGenerated]
                get
                {
                    return this.<Keys>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<Keys>k__BackingField = value;
                }
            }

            public int NumEntries
            {
                [CompilerGenerated]
                get
                {
                    return this.<NumEntries>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<NumEntries>k__BackingField = value;
                }
            }

            public string[] Values
            {
                [CompilerGenerated]
                get
                {
                    return this.<Values>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<Values>k__BackingField = value;
                }
            }
        }
    }
}

