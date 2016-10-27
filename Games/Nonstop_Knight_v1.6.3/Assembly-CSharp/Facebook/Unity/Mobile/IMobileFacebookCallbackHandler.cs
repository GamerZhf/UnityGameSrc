namespace Facebook.Unity.Mobile
{
    using Facebook.Unity;
    using System;

    internal interface IMobileFacebookCallbackHandler : IFacebookCallbackHandler
    {
        void OnAppInviteComplete(string message);
        void OnFetchDeferredAppLinkComplete(string message);
        void OnRefreshCurrentAccessTokenComplete(string message);
    }
}

