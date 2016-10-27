namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Nearby;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeEndpointDetails : BaseReferenceHolder
    {
        internal NativeEndpointDetails(IntPtr pointer) : base(pointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            NearbyConnectionTypes.EndpointDetails_Dispose(selfPointer);
        }

        internal string DeviceId()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_arg, UIntPtr out_size) {
                return NearbyConnectionTypes.EndpointDetails_GetDeviceId(base.SelfPtr(), out_arg, out_size);
            });
        }

        internal string EndpointId()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_arg, UIntPtr out_size) {
                return NearbyConnectionTypes.EndpointDetails_GetEndpointId(base.SelfPtr(), out_arg, out_size);
            });
        }

        internal static NativeEndpointDetails FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new NativeEndpointDetails(pointer);
        }

        internal string Name()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_arg, UIntPtr out_size) {
                return NearbyConnectionTypes.EndpointDetails_GetName(base.SelfPtr(), out_arg, out_size);
            });
        }

        internal string ServiceId()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_arg, UIntPtr out_size) {
                return NearbyConnectionTypes.EndpointDetails_GetServiceId(base.SelfPtr(), out_arg, out_size);
            });
        }

        internal EndpointDetails ToDetails()
        {
            return new EndpointDetails(this.EndpointId(), this.DeviceId(), this.Name(), this.ServiceId());
        }
    }
}

