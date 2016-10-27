namespace Facebook.Unity.Editor
{
    using Facebook.MiniJSON;
    using Facebook.Unity;
    using Facebook.Unity.Canvas;
    using Facebook.Unity.Editor.Dialogs;
    using Facebook.Unity.Mobile;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class EditorFacebook : FacebookBase, ICanvasFacebook, ICanvasFacebookCallbackHandler, ICanvasFacebookImplementation, IFacebook, IFacebookCallbackHandler, IMobileFacebook, IMobileFacebookCallbackHandler, IMobileFacebookImplementation
    {
        [CompilerGenerated]
        private bool <LimitEventUsage>k__BackingField;
        [CompilerGenerated]
        private Facebook.Unity.ShareDialogMode <ShareDialogMode>k__BackingField;
        private const string AccessTokenKey = "com.facebook.unity.editor.accesstoken";
        private const string WarningMessage = "You are using the facebook SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.";

        public EditorFacebook() : base(new CallbackManager())
        {
        }

        public override void ActivateApp(string appId)
        {
            FacebookLogger.Info("This only needs to be called for iOS or Android.");
        }

        public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
        {
            FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
        }

        public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
        {
            FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
        }

        public void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
        {
            this.ShowEmptyMockDialog<IAppInviteResult>(new EditorFacebookMockDialog.OnComplete(this.OnAppInviteComplete), callback, "Mock App Invite");
        }

        public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
        {
            this.ShowEmptyMockDialog<IAppRequestResult>(new EditorFacebookMockDialog.OnComplete(this.OnAppRequestsComplete), callback, "Mock App Request");
        }

        public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
        {
            this.ShowMockShareDialog("FeedShare", callback);
        }

        public void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["url"] = "mockurl://testing.url";
            dictionary["ref"] = "mock ref";
            Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
            dictionary2.Add("mock extra key", "mock extra value");
            dictionary["extras"] = dictionary2;
            dictionary["target_url"] = "mocktargeturl://mocktarget.url";
            dictionary["callback_id"] = base.CallbackManager.AddFacebookDelegate<IAppLinkResult>(callback);
            this.OnFetchDeferredAppLinkComplete(Json.Serialize(dictionary));
        }

        public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
        {
            this.ShowEmptyMockDialog<IGroupCreateResult>(new EditorFacebookMockDialog.OnComplete(this.OnGroupCreateComplete), callback, "Mock Group Create");
        }

        public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
        {
            this.ShowEmptyMockDialog<IGroupJoinResult>(new EditorFacebookMockDialog.OnComplete(this.OnGroupJoinComplete), callback, "Mock Group Join");
        }

        public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["url"] = "mockurl://testing.url";
            dictionary["callback_id"] = base.CallbackManager.AddFacebookDelegate<IAppLinkResult>(callback);
            this.OnGetAppLinkComplete(Json.Serialize(dictionary));
        }

        public override void Init(HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
        {
            FacebookLogger.Warn("You are using the facebook SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.");
            base.Init(hideUnityDelegate, onInitComplete);
            this.EditorGameObject.OnInitComplete(string.Empty);
        }

        public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
        {
            MockLoginDialog component = ComponentFactory.GetComponent<MockLoginDialog>(ComponentFactory.IfNotExist.AddNew);
            IFacebookCallbackHandler editorGameObject = this.EditorGameObject;
            component.Callback = new EditorFacebookMockDialog.OnComplete(editorGameObject.OnLoginComplete);
            component.CallbackID = base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback);
        }

        public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
        {
            this.LogInWithPublishPermissions(permissions, callback);
        }

        public void OnAppInviteComplete(string message)
        {
            AppInviteResult result = new AppInviteResult(message);
            base.CallbackManager.OnFacebookResponse(result);
        }

        public override void OnAppRequestsComplete(string message)
        {
            AppRequestResult result = new AppRequestResult(message);
            base.CallbackManager.OnFacebookResponse(result);
        }

        public void OnFacebookAuthResponseChange(string message)
        {
            throw new NotSupportedException();
        }

        public void OnFetchDeferredAppLinkComplete(string message)
        {
            AppLinkResult result = new AppLinkResult(message);
            base.CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGetAppLinkComplete(string message)
        {
            AppLinkResult result = new AppLinkResult(message);
            base.CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGroupCreateComplete(string message)
        {
            GroupCreateResult result = new GroupCreateResult(message);
            base.CallbackManager.OnFacebookResponse(result);
        }

        public override void OnGroupJoinComplete(string message)
        {
            GroupJoinResult result = new GroupJoinResult(message);
            base.CallbackManager.OnFacebookResponse(result);
        }

        public override void OnLoginComplete(string message)
        {
            LoginResult result = new LoginResult(message);
            base.OnAuthResponse(result);
        }

        public void OnPayComplete(string message)
        {
            PayResult result = new PayResult(message);
            base.CallbackManager.OnFacebookResponse(result);
        }

        public void OnRefreshCurrentAccessTokenComplete(string message)
        {
            AccessTokenRefreshResult result = new AccessTokenRefreshResult(message);
            base.CallbackManager.OnFacebookResponse(result);
        }

        public override void OnShareLinkComplete(string message)
        {
            ShareResult result = new ShareResult(message);
            base.CallbackManager.OnFacebookResponse(result);
        }

        public void OnUrlResponse(string message)
        {
            throw new NotSupportedException();
        }

        public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
        {
            this.ShowEmptyMockDialog<IPayResult>(new EditorFacebookMockDialog.OnComplete(this.OnPayComplete), callback, "Mock Pay Dialog");
        }

        public void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
        {
            if (callback != null)
            {
                Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
                dictionary3.Add("callback_id", base.CallbackManager.AddFacebookDelegate<IAccessTokenRefreshResult>(callback));
                Dictionary<string, object> dest = dictionary3;
                if (AccessToken.CurrentAccessToken == null)
                {
                    dest["error"] = "No current access token";
                }
                else
                {
                    IDictionary<string, object> source = (IDictionary<string, object>) Json.Deserialize(AccessToken.CurrentAccessToken.ToJson());
                    Utilities.AddAllKVPFrom<string, object>(dest, source);
                }
                this.OnRefreshCurrentAccessTokenComplete(Utilities.ToJson(dest));
            }
        }

        public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
        {
            this.ShowMockShareDialog("ShareLink", callback);
        }

        private void ShowEmptyMockDialog<T>(EditorFacebookMockDialog.OnComplete callback, FacebookDelegate<T> userCallback, string title) where T: IResult
        {
            EmptyMockDialog component = ComponentFactory.GetComponent<EmptyMockDialog>(ComponentFactory.IfNotExist.AddNew);
            component.Callback = callback;
            component.CallbackID = base.CallbackManager.AddFacebookDelegate<T>(userCallback);
            component.EmptyDialogTitle = title;
        }

        private void ShowMockShareDialog(string subTitle, FacebookDelegate<IShareResult> userCallback)
        {
            MockShareDialog component = ComponentFactory.GetComponent<MockShareDialog>(ComponentFactory.IfNotExist.AddNew);
            component.SubTitle = subTitle;
            IFacebookCallbackHandler editorGameObject = this.EditorGameObject;
            component.Callback = new EditorFacebookMockDialog.OnComplete(editorGameObject.OnShareLinkComplete);
            component.CallbackID = base.CallbackManager.AddFacebookDelegate<IShareResult>(userCallback);
        }

        private IFacebookCallbackHandler EditorGameObject
        {
            get
            {
                return ComponentFactory.GetComponent<EditorFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
            }
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
                return "FBUnityEditorSDK";
            }
        }

        public override string SDKVersion
        {
            get
            {
                return FacebookSdkVersion.Build;
            }
        }

        public Facebook.Unity.ShareDialogMode ShareDialogMode
        {
            [CompilerGenerated]
            get
            {
                return this.<ShareDialogMode>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ShareDialogMode>k__BackingField = value;
            }
        }
    }
}

