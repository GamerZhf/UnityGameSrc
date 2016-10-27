namespace Service
{
    using GameLogic;
    using System;

    public class LoginResponse
    {
        public string ConflictUserId;
        public string ConflictUserName;
        public int ConflictUserRank;
        public string FgUserHandle;
        public Service.LoginStatus LoginStatus;
        public string Password;
        public GameLogic.ServerStats ServerStats;
        public string SessionId;
        public string UserId;
    }
}

