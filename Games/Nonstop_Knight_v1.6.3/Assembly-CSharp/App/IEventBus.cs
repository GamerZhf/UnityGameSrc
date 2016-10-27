namespace App
{
    using GameLogic;
    using Service;
    using System;
    using UnityEngine.SocialPlatforms;

    public interface IEventBus
    {
        event App.Events.ApplicationQuitRequested OnApplicationQuitRequested;

        event App.Events.LeaderboardLoaded OnLeaderboardLoaded;

        event App.Events.PlatformConnectStateChanged OnPlatformConnectStateChanged;

        event App.Events.PlatformFriendsUpdated OnPlatformFriendsUpdated;

        event App.Events.PlatformProfileUpdated OnPlatformProfileUpdated;

        event App.Events.PlayerAuthenticatedToBackend OnPlayerAuthenticatedToBackend;

        event App.Events.PlayerScoreUpdated OnPlayerScoreUpdated;

        void ApplicationQuitRequested();
        void LeaderboardLoaded(LeaderboardType lbType);
        void PlatformConnectStateChanged(PlatformConnectType connectType);
        void PlatformFriendsUpdated(PlatformConnectType connectType);
        void PlatformProfileUpdated(PlatformConnectType connectType, IUserProfile profile);
        void PlayerAuthenticatedToBackend(Player player);
        void PlayerScoreUpdated(LeaderboardType lbType, int score);
    }
}

