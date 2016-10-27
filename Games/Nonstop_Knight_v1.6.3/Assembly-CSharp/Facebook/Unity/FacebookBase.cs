namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    internal abstract class FacebookBase : IFacebook, IFacebookCallbackHandler, IFacebookImplementation
    {
        [CompilerGenerated]
        private Facebook.Unity.CallbackManager <CallbackManager>k__BackingField;
        [CompilerGenerated]
        private bool <Initialized>k__BackingField;
        private HideUnityDelegate onHideUnityDelegate;
        private InitDelegate onInitCompleteDelegate;

        protected FacebookBase(Facebook.Unity.CallbackManager callbackManager)
        {
            this.CallbackManager = callbackManager;
        }

        public abstract void ActivateApp([Optional, DefaultParameterValue(null)] string appId);
        public void API(string query, HttpMethod method, IDictionary<string, string> formData, FacebookDelegate<IGraphResult> callback)
        {
            IDictionary<string, string> dictionary = (formData == null) ? new Dictionary<string, string>() : this.CopyByValue(formData);
            if (!dictionary.ContainsKey("access_token") && !query.Contains("access_token="))
            {
                dictionary["access_token"] = !FB.IsLoggedIn ? string.Empty : AccessToken.CurrentAccessToken.TokenString;
            }
            AsyncRequestString.Request(this.GetGraphUrl(query), method, dictionary, callback);
        }

        public void API(string query, HttpMethod method, WWWForm formData, FacebookDelegate<IGraphResult> callback)
        {
            if (formData == null)
            {
                formData = new WWWForm();
            }
            string str = (AccessToken.CurrentAccessToken == null) ? string.Empty : AccessToken.CurrentAccessToken.TokenString;
            formData.AddField("access_token", str);
            AsyncRequestString.Request(this.GetGraphUrl(query), method, formData, callback);
        }

        public abstract void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters);
        public abstract void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters);
        public void AppRequest(string message, [Optional, DefaultParameterValue(null)] IEnumerable<string> to, [Optional, DefaultParameterValue(null)] IEnumerable<object> filters, [Optional, DefaultParameterValue(null)] IEnumerable<string> excludeIds, [Optional, DefaultParameterValue(null)] int? maxRecipients, [Optional, DefaultParameterValue("")] string data, [Optional, DefaultParameterValue("")] string title, [Optional, DefaultParameterValue(null)] FacebookDelegate<IAppRequestResult> callback)
        {
            this.AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
        }

        public abstract void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback);
        private IDictionary<string, string> CopyByValue(IDictionary<string, string> data)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>(data.Count);
            IEnumerator<KeyValuePair<string, string>> enumerator = data.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, string> current = enumerator.Current;
                    dictionary[current.Key] = (current.Value == null) ? null : new string(current.Value.ToCharArray());
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            return dictionary;
        }

        public abstract void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback);
        public abstract void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback);
        public abstract void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback);
        public abstract void GetAppLink(FacebookDelegate<IAppLinkResult> callback);
        private Uri GetGraphUrl(string query)
        {
            if (!string.IsNullOrEmpty(query) && query.StartsWith("/"))
            {
                query = query.Substring(1);
            }
            return new Uri(Constants.GraphUrl, query);
        }

        public virtual void Init(HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
        {
            this.onHideUnityDelegate = hideUnityDelegate;
            this.onInitCompleteDelegate = onInitComplete;
        }

        public abstract void LogInWithPublishPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback);
        public abstract void LogInWithReadPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback);
        public virtual void LogOut()
        {
            AccessToken.CurrentAccessToken = null;
        }

        public abstract void OnAppRequestsComplete(string message);
        protected void OnAuthResponse(LoginResult result)
        {
            if (result.AccessToken != null)
            {
                AccessToken.CurrentAccessToken = result.AccessToken;
            }
            this.CallbackManager.OnFacebookResponse(result);
        }

        public abstract void OnGetAppLinkComplete(string message);
        public abstract void OnGroupCreateComplete(string message);
        public abstract void OnGroupJoinComplete(string message);
        public virtual void OnHideUnity(bool isGameShown)
        {
            if (this.onHideUnityDelegate != null)
            {
                this.onHideUnityDelegate(isGameShown);
            }
        }

        public virtual void OnInitComplete(string message)
        {
            this.Initialized = true;
            this.OnLoginComplete(message);
            if (this.onInitCompleteDelegate != null)
            {
                this.onInitCompleteDelegate();
            }
        }

        public abstract void OnLoginComplete(string message);
        public void OnLogoutComplete(string message)
        {
            AccessToken.CurrentAccessToken = null;
        }

        public abstract void OnShareLinkComplete(string message);
        public abstract void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback);
        protected void ValidateAppRequestArgs(string message, OGActionType? actionType, string objectId, [Optional, DefaultParameterValue(null)] IEnumerable<string> to, [Optional, DefaultParameterValue(null)] IEnumerable<object> filters, [Optional, DefaultParameterValue(null)] IEnumerable<string> excludeIds, [Optional, DefaultParameterValue(null)] int? maxRecipients, [Optional, DefaultParameterValue("")] string data, [Optional, DefaultParameterValue("")] string title, [Optional, DefaultParameterValue(null)] FacebookDelegate<IAppRequestResult> callback)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message", "message cannot be null or empty!");
            }
            if ((!string.IsNullOrEmpty(objectId) && !((((OGActionType) actionType.GetValueOrDefault()) == OGActionType.ASKFOR) && actionType.HasValue)) && !((((OGActionType) actionType.GetValueOrDefault()) == OGActionType.SEND) && actionType.HasValue))
            {
                throw new ArgumentNullException("objectId", "Object ID must be set if and only if action type is SEND or ASKFOR");
            }
            if (!actionType.HasValue && !string.IsNullOrEmpty(objectId))
            {
                throw new ArgumentNullException("actionType", "You cannot provide an objectId without an actionType");
            }
        }

        protected Facebook.Unity.CallbackManager CallbackManager
        {
            [CompilerGenerated]
            get
            {
                return this.<CallbackManager>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CallbackManager>k__BackingField = value;
            }
        }

        public bool Initialized
        {
            [CompilerGenerated]
            get
            {
                return this.<Initialized>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Initialized>k__BackingField = value;
            }
        }

        public abstract bool LimitEventUsage { get; set; }

        public bool LoggedIn
        {
            get
            {
                return (AccessToken.CurrentAccessToken != null);
            }
        }

        public abstract string SDKName { get; }

        public virtual string SDKUserAgent
        {
            get
            {
                return Utilities.GetUserAgent(this.SDKName, this.SDKVersion);
            }
        }

        public abstract string SDKVersion { get; }
    }
}

