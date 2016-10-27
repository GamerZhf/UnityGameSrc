namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    internal class NativeSavedGameClient : ISavedGameClient
    {
        private readonly GooglePlayGames.Native.PInvoke.SnapshotManager mSnapshotManager;
        private static readonly Regex ValidFilenameRegex = new Regex(@"\A[a-zA-Z0-9-._~]{1,100}\Z");

        internal NativeSavedGameClient(GooglePlayGames.Native.PInvoke.SnapshotManager manager)
        {
            this.mSnapshotManager = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.SnapshotManager>(manager);
        }

        private static Types.SnapshotConflictPolicy AsConflictPolicy(ConflictResolutionStrategy strategy)
        {
            switch (strategy)
            {
                case ConflictResolutionStrategy.UseLongestPlaytime:
                    return Types.SnapshotConflictPolicy.LONGEST_PLAYTIME;

                case ConflictResolutionStrategy.UseOriginal:
                    return Types.SnapshotConflictPolicy.LAST_KNOWN_GOOD;

                case ConflictResolutionStrategy.UseUnmerged:
                    return Types.SnapshotConflictPolicy.MOST_RECENTLY_MODIFIED;
            }
            throw new InvalidOperationException("Found unhandled strategy: " + strategy);
        }

        private static Types.DataSource AsDataSource(GooglePlayGames.BasicApi.DataSource source)
        {
            GooglePlayGames.BasicApi.DataSource source2 = source;
            if (source2 != GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork)
            {
                if (source2 != GooglePlayGames.BasicApi.DataSource.ReadNetworkOnly)
                {
                    throw new InvalidOperationException("Found unhandled DataSource: " + source);
                }
                return Types.DataSource.NETWORK_ONLY;
            }
            return Types.DataSource.CACHE_OR_NETWORK;
        }

        private static NativeSnapshotMetadataChange AsMetadataChange(SavedGameMetadataUpdate update)
        {
            NativeSnapshotMetadataChange.Builder builder = new NativeSnapshotMetadataChange.Builder();
            if (update.IsCoverImageUpdated)
            {
                builder.SetCoverImageFromPngData(update.UpdatedPngCoverImage);
            }
            if (update.IsDescriptionUpdated)
            {
                builder.SetDescription(update.UpdatedDescription);
            }
            if (update.IsPlayedTimeUpdated)
            {
                builder.SetPlayedTime((ulong) update.UpdatedPlayedTime.Value.TotalMilliseconds);
            }
            return builder.Build();
        }

        private static SavedGameRequestStatus AsRequestStatus(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status)
        {
            GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status2 = status;
            switch ((status2 + 5))
            {
                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
                    return SavedGameRequestStatus.TimeoutError;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
                    Logger.e("User was not authorized (they were probably not logged in).");
                    return SavedGameRequestStatus.AuthenticationError;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return SavedGameRequestStatus.InternalError;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
                    Logger.e("User attempted to use the game without a valid license.");
                    return SavedGameRequestStatus.AuthenticationError;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus) 6):
                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus) 7):
                    return SavedGameRequestStatus.Success;
            }
            Logger.e("Unknown status: " + status);
            return SavedGameRequestStatus.InternalError;
        }

        private static SavedGameRequestStatus AsRequestStatus(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus status)
        {
            GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus status2 = status;
            switch ((status2 + 5))
            {
                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus) 0):
                    return SavedGameRequestStatus.TimeoutError;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.ERROR_NOT_AUTHORIZED:
                    return SavedGameRequestStatus.AuthenticationError;
            }
            if (status2 == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID)
            {
                return SavedGameRequestStatus.Success;
            }
            Logger.e("Encountered unknown status: " + status);
            return SavedGameRequestStatus.InternalError;
        }

        private static SelectUIStatus AsUIStatus(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus uiStatus)
        {
            GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus status = uiStatus;
            switch ((status + 6))
            {
                case ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_INTERNAL):
                    return SelectUIStatus.UserClosedUI;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID:
                    return SelectUIStatus.TimeoutError;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return SelectUIStatus.AuthenticationError;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_TIMEOUT:
                    return SelectUIStatus.InternalError;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus) 7):
                    return SelectUIStatus.SavedGameSelected;
            }
            Logger.e("Encountered unknown UI Status: " + uiStatus);
            return SelectUIStatus.InternalError;
        }

        public void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
        {
            <CommitUpdate>c__AnonStorey2BF storeybf = new <CommitUpdate>c__AnonStorey2BF();
            storeybf.callback = callback;
            Misc.CheckNotNull<ISavedGameMetadata>(metadata);
            Misc.CheckNotNull<byte[]>(updatedBinaryData);
            Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(storeybf.callback);
            storeybf.callback = ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(storeybf.callback);
            NativeSnapshotMetadata metadata2 = metadata as NativeSnapshotMetadata;
            if (metadata2 == null)
            {
                Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
                storeybf.callback(SavedGameRequestStatus.BadInputError, null);
            }
            else if (!metadata2.IsOpen)
            {
                Logger.e("This method requires an open ISavedGameMetadata.");
                storeybf.callback(SavedGameRequestStatus.BadInputError, null);
            }
            else
            {
                this.mSnapshotManager.Commit(metadata2, AsMetadataChange(updateForMetadata), updatedBinaryData, new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse>(storeybf.<>m__100));
            }
        }

        public void Delete(ISavedGameMetadata metadata)
        {
            Misc.CheckNotNull<ISavedGameMetadata>(metadata);
            this.mSnapshotManager.Delete((NativeSnapshotMetadata) metadata);
        }

        public void FetchAllSavedGames(GooglePlayGames.BasicApi.DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback)
        {
            <FetchAllSavedGames>c__AnonStorey2C0 storeyc = new <FetchAllSavedGames>c__AnonStorey2C0();
            storeyc.callback = callback;
            Misc.CheckNotNull<Action<SavedGameRequestStatus, List<ISavedGameMetadata>>>(storeyc.callback);
            storeyc.callback = ToOnGameThread<SavedGameRequestStatus, List<ISavedGameMetadata>>(storeyc.callback);
            this.mSnapshotManager.FetchAll(AsDataSource(source), new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse>(storeyc.<>m__101));
        }

        private void InternalManualOpen(string filename, GooglePlayGames.BasicApi.DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
        {
            <InternalManualOpen>c__AnonStorey2BB storeybb = new <InternalManualOpen>c__AnonStorey2BB();
            storeybb.completedCallback = completedCallback;
            storeybb.filename = filename;
            storeybb.source = source;
            storeybb.prefetchDataOnConflict = prefetchDataOnConflict;
            storeybb.conflictCallback = conflictCallback;
            storeybb.<>f__this = this;
            this.mSnapshotManager.Open(storeybb.filename, AsDataSource(storeybb.source), Types.SnapshotConflictPolicy.MANUAL, new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse>(storeybb.<>m__FD));
        }

        internal static bool IsValidFilename(string filename)
        {
            if (filename == null)
            {
                return false;
            }
            return ValidFilenameRegex.IsMatch(filename);
        }

        public void OpenWithAutomaticConflictResolution(string filename, GooglePlayGames.BasicApi.DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
        {
            <OpenWithAutomaticConflictResolution>c__AnonStorey2B8 storeyb = new <OpenWithAutomaticConflictResolution>c__AnonStorey2B8();
            storeyb.resolutionStrategy = resolutionStrategy;
            storeyb.callback = callback;
            Misc.CheckNotNull<string>(filename);
            Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(storeyb.callback);
            storeyb.callback = ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(storeyb.callback);
            if (!IsValidFilename(filename))
            {
                Logger.e("Received invalid filename: " + filename);
                storeyb.callback(SavedGameRequestStatus.BadInputError, null);
            }
            else
            {
                this.OpenWithManualConflictResolution(filename, source, false, new ConflictCallback(storeyb.<>m__FB), storeyb.callback);
            }
        }

        public void OpenWithManualConflictResolution(string filename, GooglePlayGames.BasicApi.DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
        {
            Misc.CheckNotNull<string>(filename);
            Misc.CheckNotNull<ConflictCallback>(conflictCallback);
            Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completedCallback);
            conflictCallback = this.ToOnGameThread(conflictCallback);
            completedCallback = ToOnGameThread<SavedGameRequestStatus, ISavedGameMetadata>(completedCallback);
            if (!IsValidFilename(filename))
            {
                Logger.e("Received invalid filename: " + filename);
                completedCallback(SavedGameRequestStatus.BadInputError, null);
            }
            else
            {
                this.InternalManualOpen(filename, source, prefetchDataOnConflict, conflictCallback, completedCallback);
            }
        }

        public void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback)
        {
            <ReadBinaryData>c__AnonStorey2BD storeybd = new <ReadBinaryData>c__AnonStorey2BD();
            storeybd.completedCallback = completedCallback;
            Misc.CheckNotNull<ISavedGameMetadata>(metadata);
            Misc.CheckNotNull<Action<SavedGameRequestStatus, byte[]>>(storeybd.completedCallback);
            storeybd.completedCallback = ToOnGameThread<SavedGameRequestStatus, byte[]>(storeybd.completedCallback);
            NativeSnapshotMetadata metadata2 = metadata as NativeSnapshotMetadata;
            if (metadata2 == null)
            {
                Logger.e("Encountered metadata that was not generated by this ISavedGameClient");
                storeybd.completedCallback(SavedGameRequestStatus.BadInputError, null);
            }
            else if (!metadata2.IsOpen)
            {
                Logger.e("This method requires an open ISavedGameMetadata.");
                storeybd.completedCallback(SavedGameRequestStatus.BadInputError, null);
            }
            else
            {
                this.mSnapshotManager.Read(metadata2, new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse>(storeybd.<>m__FE));
            }
        }

        public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
        {
            <ShowSelectSavedGameUI>c__AnonStorey2BE storeybe = new <ShowSelectSavedGameUI>c__AnonStorey2BE();
            storeybe.callback = callback;
            Misc.CheckNotNull<string>(uiTitle);
            Misc.CheckNotNull<Action<SelectUIStatus, ISavedGameMetadata>>(storeybe.callback);
            storeybe.callback = ToOnGameThread<SelectUIStatus, ISavedGameMetadata>(storeybe.callback);
            if (maxDisplayedSavedGames <= 0)
            {
                Logger.e("maxDisplayedSavedGames must be greater than 0");
                storeybe.callback(SelectUIStatus.BadInputError, null);
            }
            else
            {
                this.mSnapshotManager.SnapshotSelectUI(showCreateSaveUI, showDeleteSaveUI, maxDisplayedSavedGames, uiTitle, new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse>(storeybe.<>m__FF));
            }
        }

        private ConflictCallback ToOnGameThread(ConflictCallback conflictCallback)
        {
            <ToOnGameThread>c__AnonStorey2B9 storeyb = new <ToOnGameThread>c__AnonStorey2B9();
            storeyb.conflictCallback = conflictCallback;
            return new ConflictCallback(storeyb.<>m__FC);
        }

        private static Action<T1, T2> ToOnGameThread<T1, T2>(Action<T1, T2> toConvert)
        {
            <ToOnGameThread>c__AnonStorey2C1<T1, T2> storeyc = new <ToOnGameThread>c__AnonStorey2C1<T1, T2>();
            storeyc.toConvert = toConvert;
            return new Action<T1, T2>(storeyc.<>m__102);
        }

        [CompilerGenerated]
        private sealed class <CommitUpdate>c__AnonStorey2BF
        {
            internal Action<SavedGameRequestStatus, ISavedGameMetadata> callback;

            internal void <>m__100(GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.callback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
                }
                else
                {
                    this.callback(SavedGameRequestStatus.Success, response.Data());
                }
            }
        }

        [CompilerGenerated]
        private sealed class <FetchAllSavedGames>c__AnonStorey2C0
        {
            internal Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback;

            internal void <>m__101(GooglePlayGames.Native.PInvoke.SnapshotManager.FetchAllResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.callback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), new List<ISavedGameMetadata>());
                }
                else
                {
                    this.callback(SavedGameRequestStatus.Success, Enumerable.ToList<ISavedGameMetadata>(Enumerable.Cast<ISavedGameMetadata>(response.Data())));
                }
            }
        }

        [CompilerGenerated]
        private sealed class <InternalManualOpen>c__AnonStorey2BB
        {
            internal NativeSavedGameClient <>f__this;
            internal Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback;
            internal ConflictCallback conflictCallback;
            internal string filename;
            internal bool prefetchDataOnConflict;
            internal GooglePlayGames.BasicApi.DataSource source;

            internal void <>m__FD(GooglePlayGames.Native.PInvoke.SnapshotManager.OpenResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.completedCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
                }
                else if (response.ResponseStatus() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID)
                {
                    this.completedCallback(SavedGameRequestStatus.Success, response.Data());
                }
                else if (response.ResponseStatus() == GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus.VALID_WITH_CONFLICT)
                {
                    <InternalManualOpen>c__AnonStorey2BC storeybc = new <InternalManualOpen>c__AnonStorey2BC();
                    storeybc.<>f__ref$699 = this;
                    storeybc.original = response.ConflictOriginal();
                    storeybc.unmerged = response.ConflictUnmerged();
                    storeybc.resolver = new NativeSavedGameClient.NativeConflictResolver(this.<>f__this.mSnapshotManager, response.ConflictId(), storeybc.original, storeybc.unmerged, this.completedCallback, new Action(storeybc.<>m__104));
                    if (!this.prefetchDataOnConflict)
                    {
                        this.conflictCallback(storeybc.resolver, storeybc.original, null, storeybc.unmerged, null);
                    }
                    else
                    {
                        NativeSavedGameClient.Prefetcher prefetcher = new NativeSavedGameClient.Prefetcher(new Action<byte[], byte[]>(storeybc.<>m__105), this.completedCallback);
                        this.<>f__this.mSnapshotManager.Read(storeybc.original, new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse>(prefetcher.OnOriginalDataRead));
                        this.<>f__this.mSnapshotManager.Read(storeybc.unmerged, new Action<GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse>(prefetcher.OnUnmergedDataRead));
                    }
                }
                else
                {
                    Logger.e("Unhandled response status");
                    this.completedCallback(SavedGameRequestStatus.InternalError, null);
                }
            }

            private sealed class <InternalManualOpen>c__AnonStorey2BC
            {
                internal NativeSavedGameClient.<InternalManualOpen>c__AnonStorey2BB <>f__ref$699;
                internal NativeSnapshotMetadata original;
                internal NativeSavedGameClient.NativeConflictResolver resolver;
                internal NativeSnapshotMetadata unmerged;

                internal void <>m__104()
                {
                    this.<>f__ref$699.<>f__this.InternalManualOpen(this.<>f__ref$699.filename, this.<>f__ref$699.source, this.<>f__ref$699.prefetchDataOnConflict, this.<>f__ref$699.conflictCallback, this.<>f__ref$699.completedCallback);
                }

                internal void <>m__105(byte[] originalData, byte[] unmergedData)
                {
                    this.<>f__ref$699.conflictCallback(this.resolver, this.original, originalData, this.unmerged, unmergedData);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <OpenWithAutomaticConflictResolution>c__AnonStorey2B8
        {
            internal Action<SavedGameRequestStatus, ISavedGameMetadata> callback;
            internal ConflictResolutionStrategy resolutionStrategy;

            internal void <>m__FB(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
            {
                switch (this.resolutionStrategy)
                {
                    case ConflictResolutionStrategy.UseLongestPlaytime:
                        if (original.TotalTimePlayed < unmerged.TotalTimePlayed)
                        {
                            resolver.ChooseMetadata(unmerged);
                            break;
                        }
                        resolver.ChooseMetadata(original);
                        break;

                    case ConflictResolutionStrategy.UseOriginal:
                        resolver.ChooseMetadata(original);
                        return;

                    case ConflictResolutionStrategy.UseUnmerged:
                        resolver.ChooseMetadata(unmerged);
                        return;

                    default:
                        Logger.e("Unhandled strategy " + this.resolutionStrategy);
                        this.callback(SavedGameRequestStatus.InternalError, null);
                        return;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ReadBinaryData>c__AnonStorey2BD
        {
            internal Action<SavedGameRequestStatus, byte[]> completedCallback;

            internal void <>m__FE(GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse response)
            {
                if (!response.RequestSucceeded())
                {
                    this.completedCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
                }
                else
                {
                    this.completedCallback(SavedGameRequestStatus.Success, response.Data());
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ShowSelectSavedGameUI>c__AnonStorey2BE
        {
            internal Action<SelectUIStatus, ISavedGameMetadata> callback;

            internal void <>m__FF(GooglePlayGames.Native.PInvoke.SnapshotManager.SnapshotSelectUIResponse response)
            {
                this.callback(NativeSavedGameClient.AsUIStatus(response.RequestStatus()), !response.RequestSucceeded() ? null : response.Data());
            }
        }

        [CompilerGenerated]
        private sealed class <ToOnGameThread>c__AnonStorey2B9
        {
            internal ConflictCallback conflictCallback;

            internal void <>m__FC(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
            {
                <ToOnGameThread>c__AnonStorey2BA storeyba = new <ToOnGameThread>c__AnonStorey2BA();
                storeyba.<>f__ref$697 = this;
                storeyba.resolver = resolver;
                storeyba.original = original;
                storeyba.originalData = originalData;
                storeyba.unmerged = unmerged;
                storeyba.unmergedData = unmergedData;
                Logger.d("Invoking conflict callback");
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyba.<>m__103));
            }

            private sealed class <ToOnGameThread>c__AnonStorey2BA
            {
                internal NativeSavedGameClient.<ToOnGameThread>c__AnonStorey2B9 <>f__ref$697;
                internal ISavedGameMetadata original;
                internal byte[] originalData;
                internal IConflictResolver resolver;
                internal ISavedGameMetadata unmerged;
                internal byte[] unmergedData;

                internal void <>m__103()
                {
                    this.<>f__ref$697.conflictCallback(this.resolver, this.original, this.originalData, this.unmerged, this.unmergedData);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ToOnGameThread>c__AnonStorey2C1<T1, T2>
        {
            internal Action<T1, T2> toConvert;

            internal void <>m__102(T1 val1, T2 val2)
            {
                <ToOnGameThread>c__AnonStorey2C2<T1, T2> storeyc = new <ToOnGameThread>c__AnonStorey2C2<T1, T2>();
                storeyc.<>f__ref$705 = (NativeSavedGameClient.<ToOnGameThread>c__AnonStorey2C1<T1, T2>) this;
                storeyc.val1 = val1;
                storeyc.val2 = val2;
                PlayGamesHelperObject.RunOnGameThread(new Action(storeyc.<>m__106));
            }

            private sealed class <ToOnGameThread>c__AnonStorey2C2
            {
                internal NativeSavedGameClient.<ToOnGameThread>c__AnonStorey2C1<T1, T2> <>f__ref$705;
                internal T1 val1;
                internal T2 val2;

                internal void <>m__106()
                {
                    this.<>f__ref$705.toConvert(this.val1, this.val2);
                }
            }
        }

        private class NativeConflictResolver : IConflictResolver
        {
            private readonly Action<SavedGameRequestStatus, ISavedGameMetadata> mCompleteCallback;
            private readonly string mConflictId;
            private readonly GooglePlayGames.Native.PInvoke.SnapshotManager mManager;
            private readonly NativeSnapshotMetadata mOriginal;
            private readonly Action mRetryFileOpen;
            private readonly NativeSnapshotMetadata mUnmerged;

            internal NativeConflictResolver(GooglePlayGames.Native.PInvoke.SnapshotManager manager, string conflictId, NativeSnapshotMetadata original, NativeSnapshotMetadata unmerged, Action<SavedGameRequestStatus, ISavedGameMetadata> completeCallback, Action retryOpen)
            {
                this.mManager = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.SnapshotManager>(manager);
                this.mConflictId = Misc.CheckNotNull<string>(conflictId);
                this.mOriginal = Misc.CheckNotNull<NativeSnapshotMetadata>(original);
                this.mUnmerged = Misc.CheckNotNull<NativeSnapshotMetadata>(unmerged);
                this.mCompleteCallback = Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completeCallback);
                this.mRetryFileOpen = Misc.CheckNotNull<Action>(retryOpen);
            }

            public void ChooseMetadata(ISavedGameMetadata chosenMetadata)
            {
                NativeSnapshotMetadata metadata = chosenMetadata as NativeSnapshotMetadata;
                if ((metadata != this.mOriginal) && (metadata != this.mUnmerged))
                {
                    Logger.e("Caller attempted to choose a version of the metadata that was not part of the conflict");
                    this.mCompleteCallback(SavedGameRequestStatus.BadInputError, null);
                }
                else
                {
                    this.mManager.Resolve(metadata, new NativeSnapshotMetadataChange.Builder().Build(), this.mConflictId, delegate (GooglePlayGames.Native.PInvoke.SnapshotManager.CommitResponse response) {
                        if (!response.RequestSucceeded())
                        {
                            this.mCompleteCallback(NativeSavedGameClient.AsRequestStatus(response.ResponseStatus()), null);
                        }
                        else
                        {
                            this.mRetryFileOpen();
                        }
                    });
                }
            }
        }

        private class Prefetcher
        {
            [CompilerGenerated]
            private static Action<SavedGameRequestStatus, ISavedGameMetadata> <>f__am$cache7;
            [CompilerGenerated]
            private static Action<SavedGameRequestStatus, ISavedGameMetadata> <>f__am$cache8;
            private Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback;
            private readonly Action<byte[], byte[]> mDataFetchedCallback;
            private readonly object mLock = new object();
            private byte[] mOriginalData;
            private bool mOriginalDataFetched;
            private byte[] mUnmergedData;
            private bool mUnmergedDataFetched;

            internal Prefetcher(Action<byte[], byte[]> dataFetchedCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
            {
                this.mDataFetchedCallback = Misc.CheckNotNull<Action<byte[], byte[]>>(dataFetchedCallback);
                this.completedCallback = Misc.CheckNotNull<Action<SavedGameRequestStatus, ISavedGameMetadata>>(completedCallback);
            }

            private void MaybeProceed()
            {
                if (this.mOriginalDataFetched && this.mUnmergedDataFetched)
                {
                    Logger.d("Fetched data for original and unmerged, proceeding");
                    this.mDataFetchedCallback(this.mOriginalData, this.mUnmergedData);
                }
                else
                {
                    Logger.d(string.Concat(new object[] { "Not all data fetched - original:", this.mOriginalDataFetched, " unmerged:", this.mUnmergedDataFetched }));
                }
            }

            internal void OnOriginalDataRead(GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse readResponse)
            {
                object mLock = this.mLock;
                lock (mLock)
                {
                    if (!readResponse.RequestSucceeded())
                    {
                        Logger.e("Encountered error while prefetching original data.");
                        this.completedCallback(NativeSavedGameClient.AsRequestStatus(readResponse.ResponseStatus()), null);
                        if (<>f__am$cache7 == null)
                        {
                            <>f__am$cache7 = delegate {
                            };
                        }
                        this.completedCallback = <>f__am$cache7;
                    }
                    else
                    {
                        Logger.d("Successfully fetched original data");
                        this.mOriginalDataFetched = true;
                        this.mOriginalData = readResponse.Data();
                        this.MaybeProceed();
                    }
                }
            }

            internal void OnUnmergedDataRead(GooglePlayGames.Native.PInvoke.SnapshotManager.ReadResponse readResponse)
            {
                object mLock = this.mLock;
                lock (mLock)
                {
                    if (!readResponse.RequestSucceeded())
                    {
                        Logger.e("Encountered error while prefetching unmerged data.");
                        this.completedCallback(NativeSavedGameClient.AsRequestStatus(readResponse.ResponseStatus()), null);
                        if (<>f__am$cache8 == null)
                        {
                            <>f__am$cache8 = delegate {
                            };
                        }
                        this.completedCallback = <>f__am$cache8;
                    }
                    else
                    {
                        Logger.d("Successfully fetched unmerged data");
                        this.mUnmergedDataFetched = true;
                        this.mUnmergedData = readResponse.Data();
                        this.MaybeProceed();
                    }
                }
            }
        }
    }
}

