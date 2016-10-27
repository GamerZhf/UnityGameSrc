namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeScoreEntry : BaseReferenceHolder
    {
        private const ulong MinusOne = ulong.MaxValue;

        internal NativeScoreEntry(IntPtr selfPtr) : base(selfPtr)
        {
        }

        internal PlayGamesScore AsScore(string leaderboardId)
        {
            DateTime time2 = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            ulong lastModifiedTime = this.GetLastModifiedTime();
            if (lastModifiedTime == ulong.MaxValue)
            {
                lastModifiedTime = 0L;
            }
            return new PlayGamesScore(time2.AddMilliseconds((double) lastModifiedTime), leaderboardId, this.GetScore().GetRank(), this.GetPlayerId(), this.GetScore().GetValue(), this.GetScore().GetMetadata());
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            ScorePage.ScorePage_Entry_Dispose(selfPointer);
        }

        internal ulong GetLastModifiedTime()
        {
            return ScorePage.ScorePage_Entry_LastModifiedTime(base.SelfPtr());
        }

        internal string GetPlayerId()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                return ScorePage.ScorePage_Entry_PlayerId(base.SelfPtr(), out_string, out_size);
            });
        }

        internal NativeScore GetScore()
        {
            return new NativeScore(ScorePage.ScorePage_Entry_Score(base.SelfPtr()));
        }
    }
}

