namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;

    internal class AppInvites : MenuBase
    {
        protected override void GetGui()
        {
            FacebookDelegate<IAppInviteResult> delegate2;
            if (base.Button("Android Invite"))
            {
                base.Status = "Logged FB.AppEvent";
                delegate2 = new FacebookDelegate<IAppInviteResult>(this.HandleResult);
                FB.Mobile.AppInvite(new Uri("https://fb.me/892708710750483"), null, delegate2);
            }
            if (base.Button("Android Invite With Custom Image"))
            {
                base.Status = "Logged FB.AppEvent";
                FB.Mobile.AppInvite(new Uri("https://fb.me/892708710750483"), new Uri("http://i.imgur.com/zkYlB.jpg"), new FacebookDelegate<IAppInviteResult>(this.HandleResult));
            }
            if (base.Button("iOS Invite"))
            {
                base.Status = "Logged FB.AppEvent";
                delegate2 = new FacebookDelegate<IAppInviteResult>(this.HandleResult);
                FB.Mobile.AppInvite(new Uri("https://fb.me/810530068992919"), null, delegate2);
            }
            if (base.Button("iOS Invite With Custom Image"))
            {
                base.Status = "Logged FB.AppEvent";
                FB.Mobile.AppInvite(new Uri("https://fb.me/810530068992919"), new Uri("http://i.imgur.com/zkYlB.jpg"), new FacebookDelegate<IAppInviteResult>(this.HandleResult));
            }
        }
    }
}

