namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class NativeScorePage : BaseReferenceHolder
    {
        internal NativeScorePage(IntPtr selfPtr) : base(selfPtr)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            ScorePage.ScorePage_Dispose(selfPointer);
        }

        internal static NativeScorePage FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new NativeScorePage(pointer);
        }

        internal Types.LeaderboardCollection GetCollection()
        {
            return ScorePage.ScorePage_Collection(base.SelfPtr());
        }

        private NativeScoreEntry GetElement(UIntPtr index)
        {
            if (index.ToUInt64() >= this.Length().ToUInt64())
            {
                throw new ArgumentOutOfRangeException();
            }
            return new NativeScoreEntry(ScorePage.ScorePage_Entries_GetElement(base.SelfPtr(), index));
        }

        public IEnumerator<NativeScoreEntry> GetEnumerator()
        {
            return PInvokeUtilities.ToEnumerator<NativeScoreEntry>(ScorePage.ScorePage_Entries_Length(base.SelfPtr()), delegate (UIntPtr index) {
                return this.GetElement(index);
            });
        }

        internal string GetLeaderboardId()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                return ScorePage.ScorePage_LeaderboardId(base.SelfPtr(), out_string, out_size);
            });
        }

        internal NativeScorePageToken GetNextScorePageToken()
        {
            return new NativeScorePageToken(ScorePage.ScorePage_NextScorePageToken(base.SelfPtr()));
        }

        internal NativeScorePageToken GetPreviousScorePageToken()
        {
            return new NativeScorePageToken(ScorePage.ScorePage_PreviousScorePageToken(base.SelfPtr()));
        }

        internal Types.LeaderboardStart GetStart()
        {
            return ScorePage.ScorePage_Start(base.SelfPtr());
        }

        internal Types.LeaderboardTimeSpan GetTimeSpan()
        {
            return ScorePage.ScorePage_TimeSpan(base.SelfPtr());
        }

        internal bool HasNextScorePage()
        {
            return ScorePage.ScorePage_HasNextScorePage(base.SelfPtr());
        }

        internal bool HasPrevScorePage()
        {
            return ScorePage.ScorePage_HasPreviousScorePage(base.SelfPtr());
        }

        private UIntPtr Length()
        {
            return ScorePage.ScorePage_Entries_Length(base.SelfPtr());
        }

        internal bool Valid()
        {
            return ScorePage.ScorePage_Valid(base.SelfPtr());
        }
    }
}

