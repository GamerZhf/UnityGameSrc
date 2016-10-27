namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class AchievementManager
    {
        private readonly GooglePlayGames.Native.PInvoke.GameServices mServices;

        internal AchievementManager(GooglePlayGames.Native.PInvoke.GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
        }

        internal void Fetch(string achId, Action<FetchResponse> callback)
        {
            Misc.CheckNotNull<string>(achId);
            Misc.CheckNotNull<Action<FetchResponse>>(callback);
            GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_Fetch(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, achId, new GooglePlayGames.Native.Cwrapper.AchievementManager.FetchCallback(GooglePlayGames.Native.PInvoke.AchievementManager.InternalFetchCallback), Callbacks.ToIntPtr<FetchResponse>(callback, new Func<IntPtr, FetchResponse>(FetchResponse.FromPointer)));
        }

        internal void FetchAll(Action<FetchAllResponse> callback)
        {
            Misc.CheckNotNull<Action<FetchAllResponse>>(callback);
            GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAll(this.mServices.AsHandle(), Types.DataSource.CACHE_OR_NETWORK, new GooglePlayGames.Native.Cwrapper.AchievementManager.FetchAllCallback(GooglePlayGames.Native.PInvoke.AchievementManager.InternalFetchAllCallback), Callbacks.ToIntPtr<FetchAllResponse>(callback, new Func<IntPtr, FetchAllResponse>(FetchAllResponse.FromPointer)));
        }

        internal void Increment(string achievementId, uint numSteps)
        {
            Misc.CheckNotNull<string>(achievementId);
            GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_Increment(this.mServices.AsHandle(), achievementId, numSteps);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.AchievementManager.FetchAllCallback))]
        private static void InternalFetchAllCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("AchievementManager#InternalFetchAllCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.AchievementManager.FetchCallback))]
        private static void InternalFetchCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("AchievementManager#InternalFetchCallback", Callbacks.Type.Temporary, response, data);
        }

        internal void Reveal(string achievementId)
        {
            Misc.CheckNotNull<string>(achievementId);
            GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_Reveal(this.mServices.AsHandle(), achievementId);
        }

        internal void SetStepsAtLeast(string achivementId, uint numSteps)
        {
            Misc.CheckNotNull<string>(achivementId);
            GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_SetStepsAtLeast(this.mServices.AsHandle(), achivementId, numSteps);
        }

        internal void ShowAllUI(Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> callback)
        {
            Misc.CheckNotNull<Action<GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>>(callback);
            GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_ShowAllUI(this.mServices.AsHandle(), new GooglePlayGames.Native.Cwrapper.AchievementManager.ShowAllUICallback(Callbacks.InternalShowUICallback), Callbacks.ToIntPtr(callback));
        }

        internal void Unlock(string achievementId)
        {
            Misc.CheckNotNull<string>(achievementId);
            GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_Unlock(this.mServices.AsHandle(), achievementId);
        }

        internal class FetchAllResponse : BaseReferenceHolder, IEnumerable, IEnumerable<NativeAchievement>
        {
            internal FetchAllResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAllResponse_Dispose(selfPointer);
            }

            internal static GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse(pointer);
            }

            private NativeAchievement GetElement(UIntPtr index)
            {
                if (index.ToUInt64() >= this.Length().ToUInt64())
                {
                    throw new ArgumentOutOfRangeException();
                }
                return new NativeAchievement(GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAllResponse_GetData_GetElement(base.SelfPtr(), index));
            }

            public IEnumerator<NativeAchievement> GetEnumerator()
            {
                return PInvokeUtilities.ToEnumerator<NativeAchievement>(GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAllResponse_GetData_Length(base.SelfPtr()), delegate (UIntPtr index) {
                    return this.GetElement(index);
                });
            }

            private UIntPtr Length()
            {
                return GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAllResponse_GetData_Length(base.SelfPtr());
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
            {
                return GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchAllResponse_GetStatus(base.SelfPtr());
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

            internal NativeAchievement Achievement()
            {
                return new NativeAchievement(GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchResponse_GetData(base.SelfPtr()));
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchResponse_Dispose(selfPointer);
            }

            internal static GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse(pointer);
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
            {
                return GooglePlayGames.Native.Cwrapper.AchievementManager.AchievementManager_FetchResponse_GetStatus(base.SelfPtr());
            }
        }
    }
}

