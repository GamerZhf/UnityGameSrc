namespace GooglePlayGames.Android
{
    using Com.Google.Android.Gms.Common.Api;
    using GooglePlayGames;
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AndroidTokenClient : TokenClient
    {
        private string accessToken;
        private string accountName;
        private bool apiAccessDenied;
        private int apiWarningCount;
        private int apiWarningFreq = 0x186a0;
        private bool fetchingAccessToken;
        private bool fetchingEmail;
        private bool fetchingIdToken;
        private const string FetchTokenMethod = "fetchToken";
        private const string FetchTokenSignature = "(Landroid/app/Activity;Ljava/lang/String;Ljava/lang/String;ZZZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;";
        private string idToken;
        private Action<string> idTokenCb;
        private string idTokenScope;
        private string playerId;
        private string rationale;
        private const string TokenFragmentClass = "com.google.games.bridge.TokenFragment";
        private int webClientWarningCount;
        private int webClientWarningFreq = 0x186a0;

        public AndroidTokenClient(string playerId)
        {
            this.playerId = playerId;
        }

        internal void Fetch(string scope, bool fetchEmail, bool fetchAccessToken, bool fetchIdToken, Action<CommonStatusCodes> doneCallback)
        {
            <Fetch>c__AnonStorey272 storey = new <Fetch>c__AnonStorey272();
            storey.scope = scope;
            storey.fetchEmail = fetchEmail;
            storey.fetchAccessToken = fetchAccessToken;
            storey.fetchIdToken = fetchIdToken;
            storey.doneCallback = doneCallback;
            storey.<>f__this = this;
            if (this.apiAccessDenied)
            {
                if ((this.apiWarningCount++ % this.apiWarningFreq) == 0)
                {
                    Logger.w("Access to API denied");
                    this.apiWarningCount = (this.apiWarningCount / this.apiWarningFreq) + 1;
                }
                storey.doneCallback(CommonStatusCodes.AuthApiAccessForbidden);
            }
            else
            {
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__93));
            }
        }

        internal static void FetchToken(string scope, string playerId, string rationale, bool fetchEmail, bool fetchAccessToken, bool fetchIdToken, Action<int, string, string, string> callback)
        {
            object[] args = new object[7];
            jvalue[] jvalueArray = AndroidJNIHelper.CreateJNIArgArray(args);
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("com.google.games.bridge.TokenFragment"))
                {
                    using (AndroidJavaObject obj2 = GetActivity())
                    {
                        IntPtr methodID = AndroidJNI.GetStaticMethodID(class2.GetRawClass(), "fetchToken", "(Landroid/app/Activity;Ljava/lang/String;Ljava/lang/String;ZZZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;");
                        jvalueArray[0].l = obj2.GetRawObject();
                        jvalueArray[1].l = AndroidJNI.NewStringUTF(playerId);
                        jvalueArray[2].l = AndroidJNI.NewStringUTF(rationale);
                        jvalueArray[3].z = fetchEmail;
                        jvalueArray[4].z = fetchAccessToken;
                        jvalueArray[5].z = fetchIdToken;
                        jvalueArray[6].l = AndroidJNI.NewStringUTF(scope);
                        new PendingResult<TokenResult>(AndroidJNI.CallStaticObjectMethod(class2.GetRawClass(), methodID, jvalueArray)).setResultCallback(new TokenResultCallback(callback));
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.e("Exception launching token request: " + exception.Message);
                Logger.e(exception.ToString());
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
        }

        [Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
        public string GetAccessToken()
        {
            if (string.IsNullOrEmpty(this.accessToken) && !this.fetchingAccessToken)
            {
                this.fetchingAccessToken = true;
                this.Fetch(this.idTokenScope, false, true, false, delegate (CommonStatusCodes rc) {
                    this.fetchingAccessToken = false;
                });
            }
            return this.accessToken;
        }

        private string GetAccountName(Action<CommonStatusCodes, string> callback)
        {
            <GetAccountName>c__AnonStorey273 storey = new <GetAccountName>c__AnonStorey273();
            storey.callback = callback;
            storey.<>f__this = this;
            if (string.IsNullOrEmpty(this.accountName))
            {
                if (!this.fetchingEmail)
                {
                    this.fetchingEmail = true;
                    this.Fetch(this.idTokenScope, true, false, false, new Action<CommonStatusCodes>(storey.<>m__94));
                }
            }
            else if (storey.callback != null)
            {
                storey.callback(CommonStatusCodes.Success, this.accountName);
            }
            return this.accountName;
        }

        public static AndroidJavaObject GetActivity()
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                return class2.GetStatic<AndroidJavaObject>("currentActivity");
            }
        }

        public string GetEmail()
        {
            return this.GetAccountName(null);
        }

        public void GetEmail(Action<CommonStatusCodes, string> callback)
        {
            this.GetAccountName(callback);
        }

        [Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
        public void GetIdToken(string serverClientId, Action<string> idTokenCallback)
        {
            if (string.IsNullOrEmpty(serverClientId))
            {
                if ((this.webClientWarningCount++ % this.webClientWarningFreq) == 0)
                {
                    Logger.w("serverClientId is empty, cannot get Id Token");
                    this.webClientWarningCount = (this.webClientWarningCount / this.webClientWarningFreq) + 1;
                }
                idTokenCallback(null);
            }
            else
            {
                string str = "audience:server:client_id:" + serverClientId;
                if (string.IsNullOrEmpty(this.idToken) || (str != this.idTokenScope))
                {
                    if (!this.fetchingIdToken)
                    {
                        this.fetchingIdToken = true;
                        this.idTokenScope = str;
                        this.idTokenCb = idTokenCallback;
                        this.Fetch(this.idTokenScope, false, false, true, delegate (CommonStatusCodes status) {
                            this.fetchingIdToken = false;
                            if (status == CommonStatusCodes.Success)
                            {
                                this.idTokenCb(null);
                            }
                            else
                            {
                                this.idTokenCb(this.idToken);
                            }
                        });
                    }
                }
                else
                {
                    idTokenCallback(this.idToken);
                }
            }
        }

        public void SetRationale(string rationale)
        {
            this.rationale = rationale;
        }

        [CompilerGenerated]
        private sealed class <Fetch>c__AnonStorey272
        {
            internal AndroidTokenClient <>f__this;
            internal Action<CommonStatusCodes> doneCallback;
            internal bool fetchAccessToken;
            internal bool fetchEmail;
            internal bool fetchIdToken;
            internal string scope;

            internal void <>m__93()
            {
                AndroidTokenClient.FetchToken(this.scope, this.<>f__this.playerId, this.<>f__this.rationale, this.fetchEmail, this.fetchAccessToken, this.fetchIdToken, delegate (int rc, string access, string id, string email) {
                    if (rc != 0)
                    {
                        this.<>f__this.apiAccessDenied = (rc == 0xbb9) || (rc == 0x10);
                        Logger.w("Non-success returned from fetch: " + rc);
                        this.doneCallback(CommonStatusCodes.AuthApiAccessForbidden);
                    }
                    else
                    {
                        if (this.fetchAccessToken)
                        {
                            Logger.d("a = " + access);
                        }
                        if (this.fetchEmail)
                        {
                            Logger.d("email = " + email);
                        }
                        if (this.fetchIdToken)
                        {
                            Logger.d("idt = " + id);
                        }
                        if (this.fetchAccessToken && !string.IsNullOrEmpty(access))
                        {
                            this.<>f__this.accessToken = access;
                        }
                        if (this.fetchIdToken && !string.IsNullOrEmpty(id))
                        {
                            this.<>f__this.idToken = id;
                            this.<>f__this.idTokenCb(this.<>f__this.idToken);
                        }
                        if (this.fetchEmail && !string.IsNullOrEmpty(email))
                        {
                            this.<>f__this.accountName = email;
                        }
                        this.doneCallback(CommonStatusCodes.Success);
                    }
                });
            }

            internal void <>m__97(int rc, string access, string id, string email)
            {
                if (rc != 0)
                {
                    this.<>f__this.apiAccessDenied = (rc == 0xbb9) || (rc == 0x10);
                    Logger.w("Non-success returned from fetch: " + rc);
                    this.doneCallback(CommonStatusCodes.AuthApiAccessForbidden);
                }
                else
                {
                    if (this.fetchAccessToken)
                    {
                        Logger.d("a = " + access);
                    }
                    if (this.fetchEmail)
                    {
                        Logger.d("email = " + email);
                    }
                    if (this.fetchIdToken)
                    {
                        Logger.d("idt = " + id);
                    }
                    if (this.fetchAccessToken && !string.IsNullOrEmpty(access))
                    {
                        this.<>f__this.accessToken = access;
                    }
                    if (this.fetchIdToken && !string.IsNullOrEmpty(id))
                    {
                        this.<>f__this.idToken = id;
                        this.<>f__this.idTokenCb(this.<>f__this.idToken);
                    }
                    if (this.fetchEmail && !string.IsNullOrEmpty(email))
                    {
                        this.<>f__this.accountName = email;
                    }
                    this.doneCallback(CommonStatusCodes.Success);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetAccountName>c__AnonStorey273
        {
            internal AndroidTokenClient <>f__this;
            internal Action<CommonStatusCodes, string> callback;

            internal void <>m__94(CommonStatusCodes status)
            {
                this.<>f__this.fetchingEmail = false;
                if (this.callback != null)
                {
                    this.callback(status, this.<>f__this.accountName);
                }
            }
        }
    }
}

