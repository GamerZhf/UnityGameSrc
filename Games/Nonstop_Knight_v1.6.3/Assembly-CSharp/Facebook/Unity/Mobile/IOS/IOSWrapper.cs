namespace Facebook.Unity.Mobile.IOS
{
    using System;
    using System.Runtime.InteropServices;

    internal class IOSWrapper : IIOSWrapper
    {
        public void AppInvite(int requestId, string appLinkUrl, string previewImageUrl)
        {
            IOSAppInvite(requestId, appLinkUrl, previewImageUrl);
        }

        public void AppRequest(int requestId, string message, string actionType, string objectId, [Optional, DefaultParameterValue(null)] string[] to, [Optional, DefaultParameterValue(0)] int toLength, [Optional, DefaultParameterValue("")] string filters, [Optional, DefaultParameterValue(null)] string[] excludeIds, [Optional, DefaultParameterValue(0)] int excludeIdsLength, [Optional, DefaultParameterValue(false)] bool hasMaxRecipients, [Optional, DefaultParameterValue(0)] int maxRecipients, [Optional, DefaultParameterValue("")] string data, [Optional, DefaultParameterValue("")] string title)
        {
            IOSAppRequest(requestId, message, actionType, objectId, to, toLength, filters, excludeIds, excludeIdsLength, hasMaxRecipients, maxRecipients, data, title);
        }

        public void CreateGameGroup(int requestId, string name, string description, string privacy)
        {
            IOSCreateGameGroup(requestId, name, description, privacy);
        }

        public void FBAppEventsSetLimitEventUsage(bool limitEventUsage)
        {
            IOSFBAppEventsSetLimitEventUsage(limitEventUsage);
        }

        public string FBSdkVersion()
        {
            return IOSFBSdkVersion();
        }

        public void FBSettingsActivateApp(string appId)
        {
            IOSFBSettingsActivateApp(appId);
        }

        public void FeedShare(int requestId, string toId, string link, string linkName, string linkCaption, string linkDescription, string picture, string mediaSource)
        {
            IOSFeedShare(requestId, toId, link, linkName, linkCaption, linkDescription, picture, mediaSource);
        }

        public void FetchDeferredAppLink(int requestId)
        {
            IOSFetchDeferredAppLink(requestId);
        }

        public void GetAppLink(int requestId)
        {
            IOSGetAppLink(requestId);
        }

        public void Init(string appId, bool frictionlessRequests, string urlSuffix, string unityUserAgentSuffix)
        {
            IOSInit(appId, frictionlessRequests, urlSuffix, unityUserAgentSuffix);
        }

        private static void IOSAppInvite(int requestId, string appLinkUrl, string previewImageUrl)
        {
        }

        private static void IOSAppRequest(int requestId, string message, string actionType, string objectId, [Optional, DefaultParameterValue(null)] string[] to, [Optional, DefaultParameterValue(0)] int toLength, [Optional, DefaultParameterValue("")] string filters, [Optional, DefaultParameterValue(null)] string[] excludeIds, [Optional, DefaultParameterValue(0)] int excludeIdsLength, [Optional, DefaultParameterValue(false)] bool hasMaxRecipients, [Optional, DefaultParameterValue(0)] int maxRecipients, [Optional, DefaultParameterValue("")] string data, [Optional, DefaultParameterValue("")] string title)
        {
        }

        private static void IOSCreateGameGroup(int requestId, string name, string description, string privacy)
        {
        }

        private static void IOSFBAppEventsLogEvent(string logEvent, double valueToSum, int numParams, string[] paramKeys, string[] paramVals)
        {
        }

        private static void IOSFBAppEventsLogPurchase(double logPurchase, string currency, int numParams, string[] paramKeys, string[] paramVals)
        {
        }

        private static void IOSFBAppEventsSetLimitEventUsage(bool limitEventUsage)
        {
        }

        private static string IOSFBSdkVersion()
        {
            return "NONE";
        }

        private static void IOSFBSettingsActivateApp(string appId)
        {
        }

        private static void IOSFeedShare(int requestId, string toId, string link, string linkName, string linkCaption, string linkDescription, string picture, string mediaSource)
        {
        }

        private static void IOSFetchDeferredAppLink(int requestId)
        {
        }

        private static void IOSGetAppLink(int requestId)
        {
        }

        private static void IOSInit(string appId, bool frictionlessRequests, string urlSuffix, string unityUserAgentSuffix)
        {
        }

        private static void IOSJoinGameGroup(int requestId, string groupId)
        {
        }

        private static void IOSLogInWithPublishPermissions(int requestId, string scope)
        {
        }

        private static void IOSLogInWithReadPermissions(int requestId, string scope)
        {
        }

        private static void IOSLogOut()
        {
        }

        private static void IOSRefreshCurrentAccessToken(int requestId)
        {
        }

        private static void IOSSetShareDialogMode(int mode)
        {
        }

        private static void IOSShareLink(int requestId, string contentURL, string contentTitle, string contentDescription, string photoURL)
        {
        }

        public void JoinGameGroup(int requestId, string groupId)
        {
            IOSJoinGameGroup(requestId, groupId);
        }

        public void LogAppEvent(string logEvent, double valueToSum, int numParams, string[] paramKeys, string[] paramVals)
        {
            IOSFBAppEventsLogEvent(logEvent, valueToSum, numParams, paramKeys, paramVals);
        }

        public void LogInWithPublishPermissions(int requestId, string scope)
        {
            IOSLogInWithPublishPermissions(requestId, scope);
        }

        public void LogInWithReadPermissions(int requestId, string scope)
        {
            IOSLogInWithReadPermissions(requestId, scope);
        }

        public void LogOut()
        {
            IOSLogOut();
        }

        public void LogPurchaseAppEvent(double logPurchase, string currency, int numParams, string[] paramKeys, string[] paramVals)
        {
            IOSFBAppEventsLogPurchase(logPurchase, currency, numParams, paramKeys, paramVals);
        }

        public void RefreshCurrentAccessToken(int requestId)
        {
            IOSRefreshCurrentAccessToken(requestId);
        }

        public void SetShareDialogMode(int mode)
        {
            IOSSetShareDialogMode(mode);
        }

        public void ShareLink(int requestId, string contentURL, string contentTitle, string contentDescription, string photoURL)
        {
            IOSShareLink(requestId, contentURL, contentTitle, contentDescription, photoURL);
        }
    }
}

