namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Nearby;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeConnectionRequest : BaseReferenceHolder
    {
        internal NativeConnectionRequest(IntPtr pointer) : base(pointer)
        {
        }

        internal ConnectionRequest AsRequest()
        {
            return new ConnectionRequest(this.RemoteEndpointId(), this.RemoteDeviceId(), this.RemoteEndpointName(), NearbyConnectionsManager.ServiceId, this.Payload());
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            NearbyConnectionTypes.ConnectionRequest_Dispose(selfPointer);
        }

        internal static NativeConnectionRequest FromPointer(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero)
            {
                return null;
            }
            return new NativeConnectionRequest(pointer);
        }

        internal byte[] Payload()
        {
            return PInvokeUtilities.OutParamsToArray<byte>(delegate (byte[] out_arg, UIntPtr out_size) {
                return NearbyConnectionTypes.ConnectionRequest_GetPayload(base.SelfPtr(), out_arg, out_size);
            });
        }

        internal string RemoteDeviceId()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_arg, UIntPtr out_size) {
                return NearbyConnectionTypes.ConnectionRequest_GetRemoteDeviceId(base.SelfPtr(), out_arg, out_size);
            });
        }

        internal string RemoteEndpointId()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_arg, UIntPtr out_size) {
                return NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointId(base.SelfPtr(), out_arg, out_size);
            });
        }

        internal string RemoteEndpointName()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_arg, UIntPtr out_size) {
                return NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointName(base.SelfPtr(), out_arg, out_size);
            });
        }
    }
}

