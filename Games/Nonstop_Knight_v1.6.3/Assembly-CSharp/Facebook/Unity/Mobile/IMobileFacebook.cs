namespace Facebook.Unity.Mobile
{
    using Facebook.Unity;
    using System;

    internal interface IMobileFacebook : IFacebook
    {
        void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback);
        void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback);
        void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback);

        Facebook.Unity.ShareDialogMode ShareDialogMode { get; set; }
    }
}

