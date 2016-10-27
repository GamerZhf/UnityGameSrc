namespace Com.Google.Android.Gms.Games.Stats
{
    using Com.Google.Android.Gms.Common.Api;
    using Google.Developers;
    using System;

    public class Stats_LoadPlayerStatsResultObject : JavaObjWrapper, Result, Stats_LoadPlayerStatsResult
    {
        private const string CLASS_NAME = "com/google/android/gms/games/stats/Stats$LoadPlayerStatsResult";

        public Stats_LoadPlayerStatsResultObject(IntPtr ptr) : base(ptr)
        {
        }

        public PlayerStats getPlayerStats()
        {
            return new PlayerStatsObject(base.InvokeCall<IntPtr>("getPlayerStats", "()Lcom/google/android/gms/games/stats/PlayerStats;", new object[0]));
        }

        public Status getStatus()
        {
            return new Status(base.InvokeCall<IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", new object[0]));
        }
    }
}

