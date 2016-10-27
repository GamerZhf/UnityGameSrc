namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeSnapshotMetadata : BaseReferenceHolder, ISavedGameMetadata
    {
        internal NativeSnapshotMetadata(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            SnapshotMetadata.SnapshotMetadata_Dispose(base.SelfPtr());
        }

        public override string ToString()
        {
            if (base.IsDisposed())
            {
                return "[NativeSnapshotMetadata: DELETED]";
            }
            object[] args = new object[] { this.IsOpen, this.Filename, this.Description, this.CoverImageURL, this.TotalTimePlayed, this.LastModifiedTimestamp };
            return string.Format("[NativeSnapshotMetadata: IsOpen={0}, Filename={1}, Description={2}, CoverImageUrl={3}, TotalTimePlayed={4}, LastModifiedTimestamp={5}]", args);
        }

        public string CoverImageURL
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return SnapshotMetadata.SnapshotMetadata_CoverImageURL(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public string Description
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return SnapshotMetadata.SnapshotMetadata_Description(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public string Filename
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return SnapshotMetadata.SnapshotMetadata_FileName(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public bool IsOpen
        {
            get
            {
                return SnapshotMetadata.SnapshotMetadata_IsOpen(base.SelfPtr());
            }
        }

        public DateTime LastModifiedTimestamp
        {
            get
            {
                return PInvokeUtilities.FromMillisSinceUnixEpoch(SnapshotMetadata.SnapshotMetadata_LastModifiedTime(base.SelfPtr()));
            }
        }

        public TimeSpan TotalTimePlayed
        {
            get
            {
                long num = SnapshotMetadata.SnapshotMetadata_PlayedTime(base.SelfPtr());
                if (num < 0L)
                {
                    return TimeSpan.FromMilliseconds(0.0);
                }
                return TimeSpan.FromMilliseconds((double) num);
            }
        }
    }
}

