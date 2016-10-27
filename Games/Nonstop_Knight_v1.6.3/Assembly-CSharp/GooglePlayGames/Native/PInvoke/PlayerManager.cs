namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class PlayerManager
    {
        private readonly GooglePlayGames.Native.PInvoke.GameServices mGameServices;

        internal PlayerManager(GooglePlayGames.Native.PInvoke.GameServices services)
        {
            this.mGameServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
        }

        internal void FetchFriends(Action<GooglePlayGames.BasicApi.ResponseStatus, List<Player>> callback)
        {
            <FetchFriends>c__AnonStorey2E1 storeye = new <FetchFriends>c__AnonStorey2E1();
            storeye.callback = callback;
            storeye.<>f__this = this;
            GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchConnected(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, new GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback(GooglePlayGames.Native.PInvoke.PlayerManager.InternalFetchConnectedCallback), Callbacks.ToIntPtr<FetchListResponse>(new Action<FetchListResponse>(storeye.<>m__16D), new Func<IntPtr, FetchListResponse>(FetchListResponse.FromPointer)));
        }

        internal void FetchList(string[] userIds, Action<NativePlayer[]> callback)
        {
            <FetchList>c__AnonStorey2E0 storeye = new <FetchList>c__AnonStorey2E0();
            storeye.<>f__this = this;
            storeye.coll = new FetchResponseCollector();
            storeye.coll.pendingCount = userIds.Length;
            storeye.coll.callback = callback;
            foreach (string str in userIds)
            {
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_Fetch(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, str, new GooglePlayGames.Native.Cwrapper.PlayerManager.FetchCallback(GooglePlayGames.Native.PInvoke.PlayerManager.InternalFetchCallback), Callbacks.ToIntPtr<FetchResponse>(new Action<FetchResponse>(storeye.<>m__16C), new Func<IntPtr, FetchResponse>(FetchResponse.FromPointer)));
            }
        }

        internal void FetchSelf(Action<FetchSelfResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelf(this.mGameServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, new GooglePlayGames.Native.Cwrapper.PlayerManager.FetchSelfCallback(GooglePlayGames.Native.PInvoke.PlayerManager.InternalFetchSelfCallback), Callbacks.ToIntPtr<FetchSelfResponse>(callback, new Func<IntPtr, FetchSelfResponse>(FetchSelfResponse.FromPointer)));
        }

        internal void HandleFetchCollected(FetchListResponse rsp, Action<GooglePlayGames.BasicApi.ResponseStatus, List<Player>> callback)
        {
            List<Player> list = new List<Player>();
            if ((rsp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) || (rsp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
            {
                Logger.d("Got " + rsp.Length().ToUInt64() + " players");
                IEnumerator<NativePlayer> enumerator = rsp.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        list.Add(enumerator.Current.AsPlayer());
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
            }
            callback((GooglePlayGames.BasicApi.ResponseStatus) rsp.Status(), list);
        }

        internal void HandleFetchResponse(FetchResponseCollector collector, FetchResponse resp)
        {
            if ((resp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID) || (resp.Status() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE))
            {
                NativePlayer item = resp.GetPlayer();
                collector.results.Add(item);
            }
            collector.pendingCount--;
            if (collector.pendingCount == 0)
            {
                collector.callback(collector.results.ToArray());
            }
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.PlayerManager.FetchCallback))]
        private static void InternalFetchCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("PlayerManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback))]
        private static void InternalFetchConnectedCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("PlayerManager#InternalFetchConnectedCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.PlayerManager.FetchSelfCallback))]
        private static void InternalFetchSelfCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("PlayerManager#InternalFetchSelfCallback", Callbacks.Type.Temporary, response, data);
        }

        [CompilerGenerated]
        private sealed class <FetchFriends>c__AnonStorey2E1
        {
            internal GooglePlayGames.Native.PInvoke.PlayerManager <>f__this;
            internal Action<GooglePlayGames.BasicApi.ResponseStatus, List<Player>> callback;

            internal void <>m__16D(GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse rsp)
            {
                this.<>f__this.HandleFetchCollected(rsp, this.callback);
            }
        }

        [CompilerGenerated]
        private sealed class <FetchList>c__AnonStorey2E0
        {
            internal GooglePlayGames.Native.PInvoke.PlayerManager <>f__this;
            internal GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponseCollector coll;

            internal void <>m__16C(GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse rsp)
            {
                this.<>f__this.HandleFetchResponse(this.coll, rsp);
            }
        }

        internal class FetchListResponse : BaseReferenceHolder, IEnumerable, IEnumerable<NativePlayer>
        {
            internal FetchListResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_Dispose(base.SelfPtr());
            }

            internal static GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse FromPointer(IntPtr selfPointer)
            {
                if (PInvokeUtilities.IsNull(selfPointer))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse(selfPointer);
            }

            internal NativePlayer GetElement(UIntPtr index)
            {
                if (index.ToUInt64() >= this.Length().ToUInt64())
                {
                    throw new ArgumentOutOfRangeException();
                }
                return new NativePlayer(GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetData_GetElement(base.SelfPtr(), index));
            }

            public IEnumerator<NativePlayer> GetEnumerator()
            {
                return PInvokeUtilities.ToEnumerator<NativePlayer>(this.Length(), delegate (UIntPtr index) {
                    return this.GetElement(index);
                });
            }

            internal UIntPtr Length()
            {
                return GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetData_Length(base.SelfPtr());
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
            {
                return GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetStatus(base.SelfPtr());
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        internal class FetchResponse : BaseReferenceHolder
        {
            internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_Dispose(base.SelfPtr());
            }

            internal static GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse FromPointer(IntPtr selfPointer)
            {
                if (PInvokeUtilities.IsNull(selfPointer))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse(selfPointer);
            }

            internal NativePlayer GetPlayer()
            {
                return new NativePlayer(GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_GetData(base.SelfPtr()));
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
            {
                return GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_GetStatus(base.SelfPtr());
            }
        }

        internal class FetchResponseCollector
        {
            internal Action<NativePlayer[]> callback;
            internal int pendingCount;
            internal List<NativePlayer> results = new List<NativePlayer>();
        }

        internal class FetchSelfResponse : BaseReferenceHolder
        {
            internal FetchSelfResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_Dispose(base.SelfPtr());
            }

            internal static GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse FromPointer(IntPtr selfPointer)
            {
                if (PInvokeUtilities.IsNull(selfPointer))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse(selfPointer);
            }

            internal NativePlayer Self()
            {
                return new NativePlayer(GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_GetData(base.SelfPtr()));
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
            {
                return GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_GetStatus(base.SelfPtr());
            }
        }
    }
}

