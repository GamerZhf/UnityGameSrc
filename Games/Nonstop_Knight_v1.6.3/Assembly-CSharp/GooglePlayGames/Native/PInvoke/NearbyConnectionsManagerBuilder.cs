namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    internal class NearbyConnectionsManagerBuilder : BaseReferenceHolder
    {
        internal NearbyConnectionsManagerBuilder() : base(NearbyConnectionsBuilder.NearbyConnections_Builder_Construct())
        {
        }

        internal NearbyConnectionsManager Build(PlatformConfiguration configuration)
        {
            return new NearbyConnectionsManager(NearbyConnectionsBuilder.NearbyConnections_Builder_Create(base.SelfPtr(), configuration.AsPointer()));
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            NearbyConnectionsBuilder.NearbyConnections_Builder_Dispose(selfPointer);
        }

        [MonoPInvokeCallback(typeof(NearbyConnectionsBuilder.OnInitializationFinishedCallback))]
        private static void InternalOnInitializationFinishedCallback(NearbyConnectionsStatus.InitializationStatus status, IntPtr userData)
        {
            Action<NearbyConnectionsStatus.InitializationStatus> action = Callbacks.IntPtrToPermanentCallback<Action<NearbyConnectionsStatus.InitializationStatus>>(userData);
            if (action == null)
            {
                Logger.w("Callback for Initialization is null. Received status: " + status);
            }
            else
            {
                try
                {
                    action(status);
                }
                catch (Exception exception)
                {
                    Logger.e("Error encountered executing NearbyConnectionsManagerBuilder#InternalOnInitializationFinishedCallback. Smothering exception: " + exception);
                }
            }
        }

        internal NearbyConnectionsManagerBuilder SetDefaultLogLevel(Types.LogLevel minLevel)
        {
            NearbyConnectionsBuilder.NearbyConnections_Builder_SetDefaultOnLog(base.SelfPtr(), minLevel);
            return this;
        }

        internal NearbyConnectionsManagerBuilder SetLocalClientId(long localClientId)
        {
            NearbyConnectionsBuilder.NearbyConnections_Builder_SetClientId(base.SelfPtr(), localClientId);
            return this;
        }

        internal NearbyConnectionsManagerBuilder SetOnInitializationFinished(Action<NearbyConnectionsStatus.InitializationStatus> callback)
        {
            NearbyConnectionsBuilder.NearbyConnections_Builder_SetOnInitializationFinished(base.SelfPtr(), new NearbyConnectionsBuilder.OnInitializationFinishedCallback(NearbyConnectionsManagerBuilder.InternalOnInitializationFinishedCallback), Callbacks.ToIntPtr(callback));
            return this;
        }
    }
}

