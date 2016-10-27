namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.Nearby;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeStartAdvertisingResult : BaseReferenceHolder
    {
        internal NativeStartAdvertisingResult(IntPtr pointer) : base(pointer)
        {
        }

        internal AdvertisingResult AsResult()
        {
            return new AdvertisingResult((GooglePlayGames.BasicApi.ResponseStatus) ((int) Enum.ToObject(typeof(GooglePlayGames.BasicApi.ResponseStatus), this.GetStatus())), this.LocalEndpointName());
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            NearbyConnectionTypes.StartAdvertisingResult_Dispose(selfPointer);
        }

        internal static NativeStartAdvertisingResult FromPointer(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero)
            {
                return null;
            }
            return new NativeStartAdvertisingResult(pointer);
        }

        internal int GetStatus()
        {
            return NearbyConnectionTypes.StartAdvertisingResult_GetStatus(base.SelfPtr());
        }

        internal string LocalEndpointName()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_arg, UIntPtr out_size) {
                return NearbyConnectionTypes.StartAdvertisingResult_GetLocalEndpointName(base.SelfPtr(), out_arg, out_size);
            });
        }
    }
}

