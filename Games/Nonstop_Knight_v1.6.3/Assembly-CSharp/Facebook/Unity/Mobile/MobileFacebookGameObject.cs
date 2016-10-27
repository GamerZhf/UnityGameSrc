namespace Facebook.Unity.Mobile
{
    using Facebook.Unity;
    using System;

    internal abstract class MobileFacebookGameObject : FacebookGameObject, IFacebookCallbackHandler, IMobileFacebookCallbackHandler
    {
        protected MobileFacebookGameObject()
        {
        }

        public void OnAppInviteComplete(string message)
        {
            this.MobileFacebook.OnAppInviteComplete(message);
        }

        public void OnFetchDeferredAppLinkComplete(string message)
        {
            this.MobileFacebook.OnFetchDeferredAppLinkComplete(message);
        }

        public void OnRefreshCurrentAccessTokenComplete(string message)
        {
            this.MobileFacebook.OnRefreshCurrentAccessTokenComplete(message);
        }

        private IMobileFacebookImplementation MobileFacebook
        {
            get
            {
                return (IMobileFacebookImplementation) base.Facebook;
            }
        }
    }
}

