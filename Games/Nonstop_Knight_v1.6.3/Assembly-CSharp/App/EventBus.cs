namespace App
{
    using GameLogic;
    using Service;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.SocialPlatforms;

    public class EventBus : App.IEventBus
    {
        public event App.Events.ApplicationQuitRequested OnApplicationQuitRequested;

        public event App.Events.LeaderboardLoaded OnLeaderboardLoaded;

        public event App.Events.PlatformConnectStateChanged OnPlatformConnectStateChanged;

        public event App.Events.PlatformFriendsUpdated OnPlatformFriendsUpdated;

        public event App.Events.PlatformProfileUpdated OnPlatformProfileUpdated;

        public event App.Events.PlayerAuthenticatedToBackend OnPlayerAuthenticatedToBackend;

        public event App.Events.PlayerScoreUpdated OnPlayerScoreUpdated;

        public void ApplicationQuitRequested()
        {
            if (this.OnApplicationQuitRequested != null)
            {
                this.OnApplicationQuitRequested();
            }
        }

        public void LeaderboardLoaded(LeaderboardType lbType)
        {
            if (this.OnLeaderboardLoaded != null)
            {
                this.OnLeaderboardLoaded(lbType);
            }
        }

        public void PlatformConnectStateChanged(PlatformConnectType connectType)
        {
            if (this.OnPlatformConnectStateChanged != null)
            {
                this.OnPlatformConnectStateChanged(connectType);
            }
        }

        public void PlatformFriendsUpdated(PlatformConnectType connectType)
        {
            if (this.OnPlatformFriendsUpdated != null)
            {
                this.OnPlatformFriendsUpdated(connectType);
            }
        }

        public void PlatformProfileUpdated(PlatformConnectType connectType, IUserProfile profile)
        {
            if (this.OnPlatformProfileUpdated != null)
            {
                this.OnPlatformProfileUpdated(connectType, profile);
            }
        }

        public void PlayerAuthenticatedToBackend(Player player)
        {
            if (this.OnPlayerAuthenticatedToBackend != null)
            {
                this.OnPlayerAuthenticatedToBackend(player);
            }
        }

        public void PlayerScoreUpdated(LeaderboardType lbType, int score)
        {
            if (this.OnPlayerScoreUpdated != null)
            {
                this.OnPlayerScoreUpdated(lbType, score);
            }
        }
    }
}

