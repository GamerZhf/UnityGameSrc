namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeEndpointDiscoveryListenerHelper : BaseReferenceHolder
    {
        internal NativeEndpointDiscoveryListenerHelper() : base(EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_Construct())
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_Dispose(selfPointer);
        }

        [MonoPInvokeCallback(typeof(EndpointDiscoveryListenerHelper.OnEndpointFoundCallback))]
        private static void InternalOnEndpointFoundCallback(long id, IntPtr data, IntPtr userData)
        {
            Callbacks.PerformInternalCallback<long>("NativeEndpointDiscoveryListenerHelper#InternalOnEndpointFoundCallback", Callbacks.Type.Permanent, id, data, userData);
        }

        [MonoPInvokeCallback(typeof(EndpointDiscoveryListenerHelper.OnEndpointLostCallback))]
        private static void InternalOnEndpointLostCallback(long id, string lostEndpointId, IntPtr userData)
        {
            Action<long, string> action = Callbacks.IntPtrToPermanentCallback<Action<long, string>>(userData);
            if (action != null)
            {
                try
                {
                    action(id, lostEndpointId);
                }
                catch (Exception exception)
                {
                    Logger.e("Error encountered executing NativeEndpointDiscoveryListenerHelper#InternalOnEndpointLostCallback. Smothering to avoid passing exception into Native: " + exception);
                }
            }
        }

        internal void SetOnEndpointFound(Action<long, NativeEndpointDetails> callback)
        {
            EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_SetOnEndpointFoundCallback(base.SelfPtr(), new EndpointDiscoveryListenerHelper.OnEndpointFoundCallback(NativeEndpointDiscoveryListenerHelper.InternalOnEndpointFoundCallback), Callbacks.ToIntPtr<long, NativeEndpointDetails>(callback, new Func<IntPtr, NativeEndpointDetails>(NativeEndpointDetails.FromPointer)));
        }

        internal void SetOnEndpointLostCallback(Action<long, string> callback)
        {
            EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_SetOnEndpointLostCallback(base.SelfPtr(), new EndpointDiscoveryListenerHelper.OnEndpointLostCallback(NativeEndpointDiscoveryListenerHelper.InternalOnEndpointLostCallback), Callbacks.ToIntPtr(callback));
        }
    }
}

