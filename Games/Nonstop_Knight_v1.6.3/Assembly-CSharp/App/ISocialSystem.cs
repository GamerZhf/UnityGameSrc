namespace App
{
    using Service;
    using System;
    using System.Collections;
    using UnityEngine.SocialPlatforms;

    public interface ISocialSystem
    {
        void Connect();
        void Disconnect();
        PlatformConnectType GetConnectType();
        IUserProfile GetIdentity();
        PlatformConnectState GetState();
        bool HasAuthCredentials();
        bool IsConnected();
        void ReConnect();
        bool ReportAchievement(string id, float percentage);
        bool ReportLeaderboardScore(long score);
        void SetupAuthRequest(AuthRequest authRequest, bool disconnected);
        void SetupDetachRequest(AuthRequest authRequest);
        void ShowAchievements();
        void ShowLeaderboards();
        bool SupportsDiconnect();
        IEnumerator WaitForAuthCredentials();
    }
}

