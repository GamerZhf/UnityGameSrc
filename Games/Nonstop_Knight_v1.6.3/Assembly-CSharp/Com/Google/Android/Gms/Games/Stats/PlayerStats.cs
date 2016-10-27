namespace Com.Google.Android.Gms.Games.Stats
{
    using System;

    public interface PlayerStats
    {
        float getAverageSessionLength();
        float getChurnProbability();
        int getDaysSinceLastPlayed();
        int getNumberOfPurchases();
        int getNumberOfSessions();
        float getSessionPercentile();
        float getSpendPercentile();
        float getSpendProbability();
    }
}

