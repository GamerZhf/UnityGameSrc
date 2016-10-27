namespace GooglePlayGames.Android
{
    using Com.Google.Android.Gms.Common.Api;
    using Google.Developers;
    using System;

    internal class TokenResult : JavaObjWrapper, Result
    {
        public TokenResult(IntPtr ptr) : base(ptr)
        {
        }

        public string getAccessToken()
        {
            return base.InvokeCall<string>("getAccessToken", "()Ljava/lang/String;", new object[0]);
        }

        public string getEmail()
        {
            return base.InvokeCall<string>("getEmail", "()Ljava/lang/String;", new object[0]);
        }

        public string getIdToken()
        {
            return base.InvokeCall<string>("getIdToken", "()Ljava/lang/String;", new object[0]);
        }

        public Status getStatus()
        {
            return new Status(base.InvokeCall<IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", new object[0]));
        }
    }
}

