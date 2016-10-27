namespace Facebook.Unity.Mobile
{
    using Facebook.MiniJSON;
    using Facebook.Unity;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal abstract class MobileFacebook : FacebookBase, IFacebook, IFacebookCallbackHandler, IMobileFacebook, IMobileFacebookCallbackHandler, IMobileFacebookImplementation
    {
        private const string CallbackIdKey = "callback_id";
        private Facebook.Unity.ShareDialogMode shareDialogMode;

        protected MobileFacebook(CallbackManager callbackManager) : base(callbackManager)
        {
        }

        public abstract void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback);
        private static IDictionary<string, object> DeserializeMessage(string message)
        {
            return (Dictionary<string, object>) Json.Deserialize(message);
        }

        public abstract void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback);
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

        public void OnRefreshCurrentAccessTokenComplete(string message)
        {
            AccessTokenRefreshResult result = new AccessTokenRefreshResult(message);
            if (result.AccessToken != null)
            {
                AccessToken.CurrentAccessToken = result.AccessToken;
            }
            base.CallbackManager.OnFacebookResponse(result);
        }

        public override void OnShareLinkComplete(string message)
        {
            ShareResult result = new ShareResult(message);
            base.CallbackManager.OnFacebookResponse(result);
        }

        public abstract void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback);
        private static string SerializeDictionary(IDictionary<string, object> dict)
        {
            return Json.Serialize(dict);
        }

        protected abstract void SetShareDialogMode(Facebook.Unity.ShareDialogMode mode);
        private static bool TryGetCallbackId(IDictionary<string, object> result, out string callbackId)
        {
            object obj2;
            callbackId = null;
            if (result.TryGetValue("callback_id", out obj2))
            {
                callbackId = obj2 as string;
                return true;
            }
            return false;
        }

        private static bool TryGetError(IDictionary<string, object> result, out string errorMessage)
        {
            object obj2;
            errorMessage = null;
            if (result.TryGetValue("error", out obj2))
            {
                errorMessage = obj2 as string;
                return true;
            }
            return false;
        }

        public Facebook.Unity.ShareDialogMode ShareDialogMode
        {
            get
            {
                return this.shareDialogMode;
            }
            set
            {
                this.shareDialogMode = value;
                this.SetShareDialogMode(this.shareDialogMode);
            }
        }
    }
}

