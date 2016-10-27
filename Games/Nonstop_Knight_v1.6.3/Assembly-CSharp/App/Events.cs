namespace App
{
    using GameLogic;
    using Service;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.SocialPlatforms;

    public class Events
    {
        public delegate void ApplicationQuitRequested();

        public delegate void LeaderboardLoaded(LeaderboardType lbType);

        public delegate void PlatformConnectStateChanged(PlatformConnectType connectType);

        public delegate void PlatformFriendsUpdated(PlatformConnectType connectType);

        public delegate void PlatformProfileUpdated(PlatformConnectType connectType, IUserProfile profile);

        public delegate void PlayerAuthenticatedToBackend(Player player);

        public delegate void PlayerScoreUpdated(LeaderboardType lbType, int score);
    }
}

