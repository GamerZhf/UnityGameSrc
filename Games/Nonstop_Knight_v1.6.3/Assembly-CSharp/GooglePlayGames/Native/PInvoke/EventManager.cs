namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class EventManager
    {
        private readonly GooglePlayGames.Native.PInvoke.GameServices mServices;

        internal EventManager(GooglePlayGames.Native.PInvoke.GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
        }

        internal void Fetch(Types.DataSource source, string eventId, Action<FetchResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.EventManager.EventManager_Fetch(this.mServices.AsHandle(), source, eventId, new GooglePlayGames.Native.Cwrapper.EventManager.FetchCallback(GooglePlayGames.Native.PInvoke.EventManager.InternalFetchCallback), Callbacks.ToIntPtr<FetchResponse>(callback, new Func<IntPtr, FetchResponse>(FetchResponse.FromPointer)));
        }

        internal void FetchAll(Types.DataSource source, Action<FetchAllResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAll(this.mServices.AsHandle(), source, new GooglePlayGames.Native.Cwrapper.EventManager.FetchAllCallback(GooglePlayGames.Native.PInvoke.EventManager.InternalFetchAllCallback), Callbacks.ToIntPtr<FetchAllResponse>(callback, new Func<IntPtr, FetchAllResponse>(FetchAllResponse.FromPointer)));
        }

        internal void Increment(string eventId, uint steps)
        {
            GooglePlayGames.Native.Cwrapper.EventManager.EventManager_Increment(this.mServices.AsHandle(), eventId, steps);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.EventManager.FetchAllCallback))]
        internal static void InternalFetchAllCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("EventManager#FetchAllCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.EventManager.FetchCallback))]
        internal static void InternalFetchCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("EventManager#FetchCallback", Callbacks.Type.Temporary, response, data);
        }

        internal class FetchAllResponse : BaseReferenceHolder
        {
            [CompilerGenerated]
            private static Func<IntPtr, NativeEvent> <>f__am$cache0;

            internal FetchAllResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_Dispose(selfPointer);
            }

            internal List<NativeEvent> Data()
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate (IntPtr ptr) {
                        return new NativeEvent(ptr);
                    };
                }
                return Enumerable.ToList<NativeEvent>(Enumerable.Select<IntPtr, NativeEvent>(PInvokeUtilities.OutParamsToArray<IntPtr>(delegate (IntPtr[] out_arg, UIntPtr out_size) {
                    return GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_GetData(base.SelfPtr(), out_arg, out_size);
                }), <>f__am$cache0));
            }

            internal static GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse(pointer);
            }

            internal bool RequestSucceeded()
            {
                return (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
            {
                return GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_GetStatus(base.SelfPtr());
            }
        }

        internal class FetchResponse : BaseReferenceHolder
        {
            internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_Dispose(selfPointer);
            }

            internal NativeEvent Data()
            {
                if (!this.RequestSucceeded())
                {
                    return null;
                }
                return new NativeEvent(GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_GetData(base.SelfPtr()));
            }

            internal static GooglePlayGames.Native.PInvoke.EventManager.FetchResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.EventManager.FetchResponse(pointer);
            }

            internal bool RequestSucceeded()
            {
                return (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
            {
                return GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_GetStatus(base.SelfPtr());
            }
        }
    }
}

