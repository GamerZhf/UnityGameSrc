namespace Facebook.Unity.Mobile.IOS
{
    using System;
    using System.Runtime.InteropServices;

    internal interface IIOSWrapper
    {
        void AppInvite(int requestId, string appLinkUrl, string previewImageUrl);
        void AppRequest(int requestId, string message, string actionType, string objectId, [Optional, DefaultParameterValue(null)] string[] to, [Optional, DefaultParameterValue(0)] int toLength, [Optional, DefaultParameterValue("")] string filters, [Optional, DefaultParameterValue(null)] string[] excludeIds, [Optional, DefaultParameterValue(0)] int excludeIdsLength, [Optional, DefaultParameterValue(false)] bool hasMaxRecipients, [Optional, DefaultParameterValue(0)] int maxRecipients, [Optional, DefaultParameterValue("")] string data, [Optional, DefaultParameterValue("")] string title);
        void CreateGameGroup(int requestId, string name, string description, string privacy);
        void FBAppEventsSetLimitEventUsage(bool limitEventUsage);
        string FBSdkVersion();
        void FBSettingsActivateApp(string appId);
        void FeedShare(int requestId, string toId, string link, string linkName, string linkCaption, string linkDescription, string picture, string mediaSource);
        void FetchDeferredAppLink(int requestId);
        void GetAppLink(int requestId);
        void Init(string appId, bool frictionlessRequests, string urlSuffix, string unityUserAgentSuffix);
        void JoinGameGroup(int requestId, string groupId);
        void LogAppEvent(string logEvent, double valueToSum, int numParams, string[] paramKeys, string[] paramVals);
        void LogInWithPublishPermissions(int requestId, string scope);
        void LogInWithReadPermissions(int requestId, string scope);
        void LogOut();
        void LogPurchaseAppEvent(double logPurchase, string currency, int numParams, string[] paramKeys, string[] paramVals);
        void RefreshCurrentAccessToken(int requestId);
        void SetShareDialogMode(int mode);
        void ShareLink(int requestId, string contentURL, string contentTitle, string contentDescription, string photoURL);
    }
}

