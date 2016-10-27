namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativePlayerStats : BaseReferenceHolder
    {
        internal NativePlayerStats(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal GooglePlayGames.BasicApi.PlayerStats AsPlayerStats()
        {
            GooglePlayGames.BasicApi.PlayerStats stats = new GooglePlayGames.BasicApi.PlayerStats();
            stats.Valid = this.Valid();
            if (this.Valid())
            {
                stats.AvgSessonLength = this.AverageSessionLength();
                stats.ChurnProbability = this.ChurnProbability();
                stats.DaysSinceLastPlayed = this.DaysSinceLastPlayed();
                stats.NumberOfPurchases = this.NumberOfPurchases();
                stats.NumberOfSessions = this.NumberOfSessions();
                stats.SessPercentile = this.SessionPercentile();
                stats.SpendPercentile = this.SpendPercentile();
                stats.SpendProbability = -1f;
            }
            return stats;
        }

        internal float AverageSessionLength()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_AverageSessionLength(base.SelfPtr());
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_Dispose(selfPointer);
        }

        internal float ChurnProbability()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_ChurnProbability(base.SelfPtr());
        }

        internal int DaysSinceLastPlayed()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_DaysSinceLastPlayed(base.SelfPtr());
        }

        internal bool HasAverageSessionLength()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasAverageSessionLength(base.SelfPtr());
        }

        internal bool HasChurnProbability()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasChurnProbability(base.SelfPtr());
        }

        internal bool HasDaysSinceLastPlayed()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasDaysSinceLastPlayed(base.SelfPtr());
        }

        internal bool HasNumberOfPurchases()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasNumberOfPurchases(base.SelfPtr());
        }

        internal bool HasNumberOfSessions()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasNumberOfSessions(base.SelfPtr());
        }

        internal bool HasSessionPercentile()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasSessionPercentile(base.SelfPtr());
        }

        internal bool HasSpendPercentile()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasSpendPercentile(base.SelfPtr());
        }

        internal int NumberOfPurchases()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_NumberOfPurchases(base.SelfPtr());
        }

        internal int NumberOfSessions()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_NumberOfSessions(base.SelfPtr());
        }

        internal float SessionPercentile()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_SessionPercentile(base.SelfPtr());
        }

        internal float SpendPercentile()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_SpendPercentile(base.SelfPtr());
        }

        internal bool Valid()
        {
            return GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_Valid(base.SelfPtr());
        }
    }
}

