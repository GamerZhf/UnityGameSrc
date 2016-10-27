namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class GameServices
    {
        [DllImport("gpg")]
        internal static extern void GameServices_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern void GameServices_FetchServerAuthCode(HandleRef self, string server_client_id, FetchServerAuthCodeCallback callback, IntPtr callback_arg);
        [DllImport("gpg")]
        internal static extern void GameServices_FetchServerAuthCodeResponse_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern UIntPtr GameServices_FetchServerAuthCodeResponse_GetCode(HandleRef self, StringBuilder out_arg, UIntPtr out_size);
        [DllImport("gpg")]
        internal static extern GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus GameServices_FetchServerAuthCodeResponse_GetStatus(HandleRef self);
        [DllImport("gpg")]
        internal static extern void GameServices_Flush(HandleRef self, FlushCallback callback, IntPtr callback_arg);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool GameServices_IsAuthorized(HandleRef self);
        [DllImport("gpg")]
        internal static extern void GameServices_SignOut(HandleRef self);
        [DllImport("gpg")]
        internal static extern void GameServices_StartAuthorizationUI(HandleRef self);

        internal delegate void FetchServerAuthCodeCallback(IntPtr arg0, IntPtr arg1);

        internal delegate void FlushCallback(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.FlushStatus arg0, IntPtr arg1);
    }
}

