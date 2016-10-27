namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    internal class GameServices : BaseReferenceHolder
    {
        internal GameServices(IntPtr selfPointer) : base(selfPointer)
        {
        }

        public GooglePlayGames.Native.PInvoke.AchievementManager AchievementManager()
        {
            return new GooglePlayGames.Native.PInvoke.AchievementManager(this);
        }

        internal HandleRef AsHandle()
        {
            return base.SelfPtr();
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.GameServices.GameServices_Dispose(selfPointer);
        }

        internal void FetchServerAuthCode(string server_client_id, Action<FetchServerAuthCodeResponse> callback)
        {
            Misc.CheckNotNull<Action<FetchServerAuthCodeResponse>>(callback);
            Misc.CheckNotNull<string>(server_client_id);
            GooglePlayGames.Native.Cwrapper.GameServices.GameServices_FetchServerAuthCode(this.AsHandle(), server_client_id, new GooglePlayGames.Native.Cwrapper.GameServices.FetchServerAuthCodeCallback(GooglePlayGames.Native.PInvoke.GameServices.InternalFetchServerAuthCodeCallback), Callbacks.ToIntPtr<FetchServerAuthCodeResponse>(callback, new Func<IntPtr, FetchServerAuthCodeResponse>(FetchServerAuthCodeResponse.FromPointer)));
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.GameServices.FetchServerAuthCodeCallback))]
        private static void InternalFetchServerAuthCodeCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("GameServices#InternalFetchServerAuthCodeCallback", Callbacks.Type.Temporary, response, data);
        }

        internal bool IsAuthenticated()
        {
            return GooglePlayGames.Native.Cwrapper.GameServices.GameServices_IsAuthorized(base.SelfPtr());
        }

        public GooglePlayGames.Native.PInvoke.LeaderboardManager LeaderboardManager()
        {
            return new GooglePlayGames.Native.PInvoke.LeaderboardManager(this);
        }

        public GooglePlayGames.Native.PInvoke.PlayerManager PlayerManager()
        {
            return new GooglePlayGames.Native.PInvoke.PlayerManager(this);
        }

        internal void SignOut()
        {
            GooglePlayGames.Native.Cwrapper.GameServices.GameServices_SignOut(base.SelfPtr());
        }

        internal void StartAuthorizationUI()
        {
            GooglePlayGames.Native.Cwrapper.GameServices.GameServices_StartAuthorizationUI(base.SelfPtr());
        }

        public GooglePlayGames.Native.PInvoke.StatsManager StatsManager()
        {
            return new GooglePlayGames.Native.PInvoke.StatsManager(this);
        }

        internal class FetchServerAuthCodeResponse : BaseReferenceHolder
        {
            internal FetchServerAuthCodeResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.GameServices.GameServices_FetchServerAuthCodeResponse_Dispose(selfPointer);
            }

            internal string Code()
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return GooglePlayGames.Native.Cwrapper.GameServices.GameServices_FetchServerAuthCodeResponse_GetCode(base.SelfPtr(), out_string, out_size);
                });
            }

            internal static GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse(pointer);
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
            {
                return GooglePlayGames.Native.Cwrapper.GameServices.GameServices_FetchServerAuthCodeResponse_GetStatus(base.SelfPtr());
            }
        }
    }
}

