namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal sealed class AndroidPlatformConfiguration : PlatformConfiguration
    {
        private AndroidPlatformConfiguration(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_Dispose(selfPointer);
        }

        internal static GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration Create()
        {
            return new GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration(GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_Construct());
        }

        [MonoPInvokeCallback(typeof(IntentHandlerInternal))]
        private static void InternalIntentHandler(IntPtr intent, IntPtr userData)
        {
            Callbacks.PerformInternalCallback("AndroidPlatformConfiguration#InternalIntentHandler", Callbacks.Type.Permanent, intent, userData);
        }

        internal void SetActivity(IntPtr activity)
        {
            GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetActivity(base.SelfPtr(), activity);
        }

        internal void SetOptionalIntentHandlerForUI(Action<IntPtr> intentHandler)
        {
            Misc.CheckNotNull<Action<IntPtr>>(intentHandler);
            GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(base.SelfPtr(), new GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.IntentHandler(GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration.InternalIntentHandler), Callbacks.ToIntPtr(intentHandler));
        }

        private delegate void IntentHandlerInternal(IntPtr intent, IntPtr userData);
    }
}

