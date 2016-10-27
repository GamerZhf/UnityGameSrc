namespace Service
{
    using App;
    using GameLogic;
    using System;

    public class SessionData : ISessionData
    {
        public string ClientVersion
        {
            get
            {
                return ConfigApp.BundleVersion;
            }
        }

        public string FgUserHandle
        {
            get
            {
                return GameLogic.Binder.GameState.Player.FgUserHandle;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return Service.Binder.PlayerService.IsLoggedIn;
            }
        }

        public string ServerUrl
        {
            get
            {
                return Service.Binder.ServerSelection.SelectedServer.Url;
            }
        }

        public string SessionId
        {
            get
            {
                return GameLogic.Binder.GameState.Player.SessionId;
            }
        }

        public string TrackingUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(App.Binder.ConfigMeta.TRACKING_URL))
                {
                    return App.Binder.ConfigMeta.TRACKING_URL;
                }
                return (this.ServerUrl + "/tracking/events");
            }
        }

        public string UserId
        {
            get
            {
                return GameLogic.Binder.GameState.Player._id;
            }
        }
    }
}

