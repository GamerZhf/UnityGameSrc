namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class SnapshotManager
    {
        private readonly GooglePlayGames.Native.PInvoke.GameServices mServices;

        internal SnapshotManager(GooglePlayGames.Native.PInvoke.GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
        }

        internal void Commit(NativeSnapshotMetadata metadata, NativeSnapshotMetadataChange metadataChange, byte[] updatedData, Action<CommitResponse> callback)
        {
            Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
            Misc.CheckNotNull<NativeSnapshotMetadataChange>(metadataChange);
            GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_Commit(this.mServices.AsHandle(), metadata.AsPointer(), metadataChange.AsPointer(), updatedData, new UIntPtr((ulong) updatedData.Length), new GooglePlayGames.Native.Cwrapper.SnapshotManager.CommitCallback(GooglePlayGames.Native.PInvoke.SnapshotManager.InternalCommitCallback), Callbacks.ToIntPtr<CommitResponse>(callback, new Func<IntPtr, CommitResponse>(CommitResponse.FromPointer)));
        }

        internal void Delete(NativeSnapshotMetadata metadata)
        {
            Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
            GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_Delete(this.mServices.AsHandle(), metadata.AsPointer());
        }

        internal void FetchAll(Types.DataSource source, Action<FetchAllResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_FetchAll(this.mServices.AsHandle(), source, new GooglePlayGames.Native.Cwrapper.SnapshotManager.FetchAllCallback(GooglePlayGames.Native.PInvoke.SnapshotManager.InternalFetchAllCallback), Callbacks.ToIntPtr<FetchAllResponse>(callback, new Func<IntPtr, FetchAllResponse>(FetchAllResponse.FromPointer)));
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.SnapshotManager.CommitCallback))]
        internal static void InternalCommitCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("SnapshotManager#CommitCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.SnapshotManager.FetchAllCallback))]
        internal static void InternalFetchAllCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("SnapshotManager#FetchAllCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.SnapshotManager.OpenCallback))]
        internal static void InternalOpenCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("SnapshotManager#OpenCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.SnapshotManager.ReadCallback))]
        internal static void InternalReadCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("SnapshotManager#ReadCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotSelectUICallback))]
        internal static void InternalSnapshotSelectUICallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("SnapshotManager#SnapshotSelectUICallback", Callbacks.Type.Temporary, response, data);
        }

        internal void Open(string fileName, Types.DataSource source, Types.SnapshotConflictPolicy conflictPolicy, Action<OpenResponse> callback)
        {
            Misc.CheckNotNull<string>(fileName);
            Misc.CheckNotNull<Action<OpenResponse>>(callback);
            GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_Open(this.mServices.AsHandle(), source, fileName, conflictPolicy, new GooglePlayGames.Native.Cwrapper.SnapshotManager.OpenCallback(GooglePlayGames.Native.PInvoke.SnapshotManager.InternalOpenCallback), Callbacks.ToIntPtr<OpenResponse>(callback, new Func<IntPtr, OpenResponse>(OpenResponse.FromPointer)));
        }

        internal void Read(NativeSnapshotMetadata metadata, Action<ReadResponse> callback)
        {
            Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
            Misc.CheckNotNull<Action<ReadResponse>>(callback);
            GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_Read(this.mServices.AsHandle(), metadata.AsPointer(), new GooglePlayGames.Native.Cwrapper.SnapshotManager.ReadCallback(GooglePlayGames.Native.PInvoke.SnapshotManager.InternalReadCallback), Callbacks.ToIntPtr<ReadResponse>(callback, new Func<IntPtr, ReadResponse>(ReadResponse.FromPointer)));
        }

        internal void Resolve(NativeSnapshotMetadata metadata, NativeSnapshotMetadataChange metadataChange, string conflictId, Action<CommitResponse> callback)
        {
            Misc.CheckNotNull<NativeSnapshotMetadata>(metadata);
            Misc.CheckNotNull<NativeSnapshotMetadataChange>(metadataChange);
            Misc.CheckNotNull<string>(conflictId);
            GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_ResolveConflict(this.mServices.AsHandle(), metadata.AsPointer(), metadataChange.AsPointer(), conflictId, new GooglePlayGames.Native.Cwrapper.SnapshotManager.CommitCallback(GooglePlayGames.Native.PInvoke.SnapshotManager.InternalCommitCallback), Callbacks.ToIntPtr<CommitResponse>(callback, new Func<IntPtr, CommitResponse>(CommitResponse.FromPointer)));
        }

        internal void SnapshotSelectUI(bool allowCreate, bool allowDelete, uint maxSnapshots, string uiTitle, Action<SnapshotSelectUIResponse> callback)
        {
            GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_ShowSelectUIOperation(this.mServices.AsHandle(), allowCreate, allowDelete, maxSnapshots, uiTitle, new GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotSelectUICallback(GooglePlayGames.Native.PInvoke.SnapshotManager.InternalSnapshotSelectUICallback), Callbacks.ToIntPtr<SnapshotSelectUIResponse>(callback, new Func<IntPtr, SnapshotSelectUIResponse>(SnapshotSelectUIResponse.FromPointer)));
        }

        internal class CommitResponse : BaseReferenceHolder
        {
            internal CommitResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_CommitResponse_Dispose(selfPointer);
            }

            internal NativeSnapshotMetadata Data()
            {
                if (!this.RequestSucceeded())
                {
                    throw new InvalidOperationException("Request did not succeed");
                }
                return new NativeSnapshotMetadata(GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_CommitResponse_GetData(base.SelfPtr()));
            }

            internal static GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse(pointer);
            }

            internal bool RequestSucceeded()
            {
                return (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
            {
                return GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_CommitResponse_GetStatus(base.SelfPtr());
            }
        }

        internal class FetchAllResponse : BaseReferenceHolder
        {
            internal FetchAllResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_FetchAllResponse_Dispose(selfPointer);
            }

            internal IEnumerable<NativeSnapshotMetadata> Data()
            {
                return PInvokeUtilities.ToEnumerable<NativeSnapshotMetadata>(GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_FetchAllResponse_GetData_Length(base.SelfPtr()), delegate (UIntPtr index) {
                    return new NativeSnapshotMetadata(GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_FetchAllResponse_GetData_GetElement(base.SelfPtr(), index));
                });
            }

            internal static GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse(pointer);
            }

            internal bool RequestSucceeded()
            {
                return (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
            {
                return GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_FetchAllResponse_GetStatus(base.SelfPtr());
            }
        }

        internal class OpenResponse : BaseReferenceHolder
        {
            internal OpenResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_Dispose(selfPointer);
            }

            internal string ConflictId()
            {
                if (this.ResponseStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
                {
                    throw new InvalidOperationException("OpenResponse did not have a conflict");
                }
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_GetConflictId(base.SelfPtr(), out_string, out_size);
                });
            }

            internal NativeSnapshotMetadata ConflictOriginal()
            {
                if (this.ResponseStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
                {
                    throw new InvalidOperationException("OpenResponse did not have a conflict");
                }
                return new NativeSnapshotMetadata(GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_GetConflictOriginal(base.SelfPtr()));
            }

            internal NativeSnapshotMetadata ConflictUnmerged()
            {
                if (this.ResponseStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
                {
                    throw new InvalidOperationException("OpenResponse did not have a conflict");
                }
                return new NativeSnapshotMetadata(GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_GetConflictUnmerged(base.SelfPtr()));
            }

            internal NativeSnapshotMetadata Data()
            {
                if (this.ResponseStatus() != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID)
                {
                    throw new InvalidOperationException("OpenResponse had a conflict");
                }
                return new NativeSnapshotMetadata(GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_GetData(base.SelfPtr()));
            }

            internal static GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse(pointer);
            }

            internal bool RequestSucceeded()
            {
                return (this.ResponseStatus() > ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus) 0));
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus ResponseStatus()
            {
                return GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_OpenResponse_GetStatus(base.SelfPtr());
            }
        }

        internal class ReadResponse : BaseReferenceHolder
        {
            internal ReadResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_ReadResponse_Dispose(selfPointer);
            }

            internal byte[] Data()
            {
                if (!this.RequestSucceeded())
                {
                    throw new InvalidOperationException("Request did not succeed");
                }
                return PInvokeUtilities.OutParamsToArray<byte>(delegate (byte[] out_bytes, UIntPtr out_size) {
                    return GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_ReadResponse_GetData(base.SelfPtr(), out_bytes, out_size);
                });
            }

            internal static GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse(pointer);
            }

            internal bool RequestSucceeded()
            {
                return (this.ResponseStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
            {
                return GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_CommitResponse_GetStatus(base.SelfPtr());
            }
        }

        internal class SnapshotSelectUIResponse : BaseReferenceHolder
        {
            internal SnapshotSelectUIResponse(IntPtr selfPointer) : base(selfPointer)
            {
            }

            protected override void CallDispose(HandleRef selfPointer)
            {
                GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_Dispose(selfPointer);
            }

            internal NativeSnapshotMetadata Data()
            {
                if (!this.RequestSucceeded())
                {
                    throw new InvalidOperationException("Request did not succeed");
                }
                return new NativeSnapshotMetadata(GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_GetData(base.SelfPtr()));
            }

            internal static GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse FromPointer(IntPtr pointer)
            {
                if (pointer.Equals(IntPtr.Zero))
                {
                    return null;
                }
                return new GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse(pointer);
            }

            internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus RequestStatus()
            {
                return GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotManager_SnapshotSelectUIResponse_GetStatus(base.SelfPtr());
            }

            internal bool RequestSucceeded()
            {
                return (this.RequestStatus() > ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_INTERNAL));
            }
        }
    }
}

