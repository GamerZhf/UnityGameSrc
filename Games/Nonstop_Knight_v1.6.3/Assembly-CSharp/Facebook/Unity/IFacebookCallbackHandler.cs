namespace Facebook.Unity
{
    using System;

    internal interface IFacebookCallbackHandler
    {
        void OnAppRequestsComplete(string message);
        void OnGetAppLinkComplete(string message);
        void OnGroupCreateComplete(string message);
        void OnGroupJoinComplete(string message);
        void OnInitComplete(string message);
        void OnLoginComplete(string message);
        void OnLogoutComplete(string message);
        void OnShareLinkComplete(string message);
    }
}

