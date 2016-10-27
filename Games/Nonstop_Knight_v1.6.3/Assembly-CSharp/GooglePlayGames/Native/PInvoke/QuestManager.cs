namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class QuestManager
    {
        private readonly GooglePlayGames.Native.PInvoke.GameServices mServices;

        internal QuestManager(GooglePlayGames.Native.PInvoke.GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
        }

        internal void Accept(NativeQuest quest, Action<AcceptResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_Accept(this.mServices.AsHandle(), quest.AsPointer(), new GooglePlayGames.Native.Cwrapper.QuestManager.AcceptCallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalAcceptCallback), Callbacks.ToIntPtr<AcceptResponse>(callback, new Func<IntPtr, AcceptResponse>(AcceptResponse.FromPointer)));
        }

        internal void ClaimMilestone(NativeQuestMilestone milestone, Action<ClaimMilestoneResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestone(this.mServices.AsHandle(), milestone.AsPointer(), new GooglePlayGames.Native.Cwrapper.QuestManager.ClaimMilestoneCallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalClaimMilestoneCallback), Callbacks.ToIntPtr<ClaimMilestoneResponse>(callback, new Func<IntPtr, ClaimMilestoneResponse>(ClaimMilestoneResponse.FromPointer)));
        }

        internal void Fetch(Types.DataSource source, string questId, Action<FetchResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_Fetch(this.mServices.AsHandle(), source, questId, new GooglePlayGames.Native.Cwrapper.QuestManager.FetchCallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalFetchCallback), Callbacks.ToIntPtr<FetchResponse>(callback, new Func<IntPtr, FetchResponse>(FetchResponse.FromPointer)));
        }

        internal void FetchList(Types.DataSource source, int fetchFlags, Action<FetchListResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchList(this.mServices.AsHandle(), source, fetchFlags, new GooglePlayGames.Native.Cwrapper.QuestManager.FetchListCallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalFetchListCallback), Callbacks.ToIntPtr<FetchListResponse>(callback, new Func<IntPtr, FetchListResponse>(FetchListResponse.FromPointer)));
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.QuestManager.AcceptCallback))]
        internal static void InternalAcceptCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("QuestManager#AcceptCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.QuestManager.ClaimMilestoneCallback))]
        internal static void InternalClaimMilestoneCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("QuestManager#ClaimMilestoneCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.QuestManager.FetchCallback))]
        internal static void InternalFetchCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("QuestManager#FetchCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.QuestManager.FetchListCallback))]
        internal static void InternalFetchListCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("QuestManager#FetchListCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.QuestManager.QuestUICallback))]
        internal static void InternalQuestUICallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("QuestManager#QuestUICallback", Callbacks.Type.Temporary, response, data);
        }

        internal void ShowAllQuestUI(Action<QuestUIResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ShowAllUI(this.mServices.AsHandle(), new GooglePlayGames.Native.Cwrapper.QuestManager.QuestUICallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalQuestUICallback), Callbacks.ToIntPtr<QuestUIResponse>(callback, new Func<IntPtr, QuestUIResponse>(QuestUIResponse.FromPointer)));
        }

        internal void ShowQuestUI(NativeQuest quest, Action<QuestUIResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ShowUI(this.mServices.AsHandle(), quest.AsPointer(), new GooglePlayGames.Native.Cwrapper.QuestManager.QuestUICallback(GooglePlayGames.Native.PInvoke.QuestManager.InternalQuestUICallback), Callbacks.ToIntPtr<QuestUIResponse>(callback, new Func<IntPtr, QuestUIResponse>(QuestUIResponse.FromPointer)));
        }

        internal class AcceptResponse : BaseReferenceHolder
        {
            internal AcceptResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            internal NativeQuest AcceptedQuest()
            {
                if (!this.RequestSucceeded())
                {
                    return null;
                }
                return new NativeQuest(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_AcceptResponse_GetAcceptedQuest(base.SelfPtr()));
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_AcceptResponse_Dispose(selfPointer);
            }

            internal static GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.QuestManager.AcceptResponse(pointer);
            }

            internal bool RequestSucceeded()
            {
                return (this.ResponseStatus() > ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestAcceptStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestAcceptStatus.ERROR_INTERNAL));
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestAcceptStatus ResponseStatus()
            {
                return GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_AcceptResponse_GetStatus(base.SelfPtr());
            }
        }

        internal class ClaimMilestoneResponse : BaseReferenceHolder
        {
            internal ClaimMilestoneResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_Dispose(selfPointer);
            }

            internal NativeQuestMilestone ClaimedMilestone()
            {
                if (this.RequestSucceeded())
                {
                    NativeQuestMilestone milestone = new NativeQuestMilestone(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_GetClaimedMilestone(base.SelfPtr()));
                    if (milestone.Valid())
                    {
                        return milestone;
                    }
                    milestone.Dispose();
                }
                return null;
            }

            internal static GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.QuestManager.ClaimMilestoneResponse(pointer);
            }

            internal NativeQuest Quest()
            {
                if (this.RequestSucceeded())
                {
                    NativeQuest quest = new NativeQuest(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_GetQuest(base.SelfPtr()));
                    if (quest.Valid())
                    {
                        return quest;
                    }
                    quest.Dispose();
                }
                return null;
            }

            internal bool RequestSucceeded()
            {
                return (this.ResponseStatus() > ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestClaimMilestoneStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestClaimMilestoneStatus.ERROR_INTERNAL));
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestClaimMilestoneStatus ResponseStatus()
            {
                return GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_ClaimMilestoneResponse_GetStatus(base.SelfPtr());
            }
        }

        internal class FetchListResponse : BaseReferenceHolder
        {
            internal FetchListResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_Dispose(selfPointer);
            }

            internal IEnumerable<NativeQuest> Data()
            {
                return PInvokeUtilities.ToEnumerable<NativeQuest>(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_GetData_Length(base.SelfPtr()), delegate (UIntPtr index) {
                    return new NativeQuest(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_GetData_GetElement(base.SelfPtr(), index));
                });
            }

            internal static GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.QuestManager.FetchListResponse(pointer);
            }

            internal bool RequestSucceeded()
            {
                return (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
            {
                return GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchListResponse_GetStatus(base.SelfPtr());
            }
        }

        internal class FetchResponse : BaseReferenceHolder
        {
            internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchResponse_Dispose(selfPointer);
            }

            internal NativeQuest Data()
            {
                if (!this.RequestSucceeded())
                {
                    return null;
                }
                return new NativeQuest(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchResponse_GetData(base.SelfPtr()));
            }

            internal static GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.QuestManager.FetchResponse(pointer);
            }

            internal bool RequestSucceeded()
            {
                return (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
            {
                return GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_FetchResponse_GetStatus(base.SelfPtr());
            }
        }

        internal class QuestUIResponse : BaseReferenceHolder
        {
            internal QuestUIResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            internal NativeQuest AcceptedQuest()
            {
                if (this.RequestSucceeded())
                {
                    NativeQuest quest = new NativeQuest(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_GetAcceptedQuest(base.SelfPtr()));
                    if (quest.Valid())
                    {
                        return quest;
                    }
                    quest.Dispose();
                }
                return null;
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_Dispose(selfPointer);
            }

            internal static GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.QuestManager.QuestUIResponse(pointer);
            }

            internal NativeQuestMilestone MilestoneToClaim()
            {
                if (this.RequestSucceeded())
                {
                    NativeQuestMilestone milestone = new NativeQuestMilestone(GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_GetMilestoneToClaim(base.SelfPtr()));
                    if (milestone.Valid())
                    {
                        return milestone;
                    }
                    milestone.Dispose();
                }
                return null;
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus RequestStatus()
            {
                return GooglePlayGames.Native.Cwrapper.QuestManager.QuestManager_QuestUIResponse_GetStatus(base.SelfPtr());
            }

            internal bool RequestSucceeded()
            {
                return (this.RequestStatus() > ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_INTERNAL));
            }
        }
    }
}

