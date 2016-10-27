namespace Com.Google.Android.Gms.Games.Stats
{
    using Google.Developers;
    using System;

    public class PlayerStatsObject : JavaObjWrapper, PlayerStats
    {
        private const string CLASS_NAME = "com/google/android/gms/games/stats/PlayerStats";

        public PlayerStatsObject(IntPtr ptr) : base(ptr)
        {
        }

        public float getAverageSessionLength()
        {
            return base.InvokeCall<float>("getAverageSessionLength", "()F", new object[0]);
        }

        public float getChurnProbability()
        {
            return base.InvokeCall<float>("getChurnProbability", "()F", new object[0]);
        }

        public int getDaysSinceLastPlayed()
        {
            return base.InvokeCall<int>("getDaysSinceLastPlayed", "()I", new object[0]);
        }

        public int getNumberOfPurchases()
        {
            return base.InvokeCall<int>("getNumberOfPurchases", "()I", new object[0]);
        }

        public int getNumberOfSessions()
        {
            return base.InvokeCall<int>("getNumberOfSessions", "()I", new object[0]);
        }

        public float getSessionPercentile()
        {
            return base.InvokeCall<float>("getSessionPercentile", "()F", new object[0]);
        }

        public float getSpendPercentile()
        {
            return base.InvokeCall<float>("getSpendPercentile", "()F", new object[0]);
        }

        public float getSpendProbability()
        {
            return base.InvokeCall<float>("getSpendProbability", "()F", new object[0]);
        }

        public static int CONTENTS_FILE_DESCRIPTOR
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/games/stats/PlayerStats", "CONTENTS_FILE_DESCRIPTOR");
            }
        }

        public static int PARCELABLE_WRITE_RETURN_VALUE
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/games/stats/PlayerStats", "PARCELABLE_WRITE_RETURN_VALUE");
            }
        }

        public static float UNSET_VALUE
        {
            get
            {
                return JavaObjWrapper.GetStaticFloatField("com/google/android/gms/games/stats/PlayerStats", "UNSET_VALUE");
            }
        }
    }
}

