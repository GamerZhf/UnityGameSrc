namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeScore : BaseReferenceHolder
    {
        private const ulong MinusOne = ulong.MaxValue;

        internal NativeScore(IntPtr selfPtr) : base(selfPtr)
        {
        }

        internal PlayGamesScore AsScore(string leaderboardId, string selfPlayerId)
        {
            DateTime time2 = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            ulong date = this.GetDate();
            if (date == ulong.MaxValue)
            {
                date = 0L;
            }
            return new PlayGamesScore(time2.AddMilliseconds((double) date), leaderboardId, this.GetRank(), selfPlayerId, this.GetValue(), this.GetMetadata());
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            Score.Score_Dispose(base.SelfPtr());
        }

        internal static NativeScore FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new NativeScore(pointer);
        }

        internal ulong GetDate()
        {
            return ulong.MaxValue;
        }

        internal string GetMetadata()
        {
            return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                return Score.Score_Metadata(base.SelfPtr(), out_string, out_size);
            });
        }

        internal ulong GetRank()
        {
            return Score.Score_Rank(base.SelfPtr());
        }

        internal ulong GetValue()
        {
            return Score.Score_Value(base.SelfPtr());
        }
    }
}

