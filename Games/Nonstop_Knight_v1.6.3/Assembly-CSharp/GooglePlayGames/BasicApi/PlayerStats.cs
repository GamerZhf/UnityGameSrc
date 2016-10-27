namespace GooglePlayGames.BasicApi
{
    using System;
    using System.Runtime.CompilerServices;

    public class PlayerStats
    {
        [CompilerGenerated]
        private float <AvgSessonLength>k__BackingField;
        [CompilerGenerated]
        private float <ChurnProbability>k__BackingField;
        [CompilerGenerated]
        private int <DaysSinceLastPlayed>k__BackingField;
        [CompilerGenerated]
        private int <NumberOfPurchases>k__BackingField;
        [CompilerGenerated]
        private int <NumberOfSessions>k__BackingField;
        [CompilerGenerated]
        private float <SessPercentile>k__BackingField;
        [CompilerGenerated]
        private float <SpendPercentile>k__BackingField;
        [CompilerGenerated]
        private float <SpendProbability>k__BackingField;
        [CompilerGenerated]
        private bool <Valid>k__BackingField;
        private static float UNSET_VALUE = -1f;

        public PlayerStats()
        {
            this.Valid = false;
        }

        public bool HasAvgSessonLength()
        {
            return (this.AvgSessonLength != UNSET_VALUE);
        }

        public bool HasChurnProbability()
        {
            return (this.ChurnProbability != UNSET_VALUE);
        }

        public bool HasDaysSinceLastPlayed()
        {
            return (this.DaysSinceLastPlayed != ((int) UNSET_VALUE));
        }

        public bool HasNumberOfPurchases()
        {
            return (this.NumberOfPurchases != ((int) UNSET_VALUE));
        }

        public bool HasNumberOfSessions()
        {
            return (this.NumberOfSessions != ((int) UNSET_VALUE));
        }

        public bool HasSessPercentile()
        {
            return (this.SessPercentile != UNSET_VALUE);
        }

        public bool HasSpendPercentile()
        {
            return (this.SpendPercentile != UNSET_VALUE);
        }

        public float AvgSessonLength
        {
            [CompilerGenerated]
            get
            {
                return this.<AvgSessonLength>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<AvgSessonLength>k__BackingField = value;
            }
        }

        public float ChurnProbability
        {
            [CompilerGenerated]
            get
            {
                return this.<ChurnProbability>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ChurnProbability>k__BackingField = value;
            }
        }

        public int DaysSinceLastPlayed
        {
            [CompilerGenerated]
            get
            {
                return this.<DaysSinceLastPlayed>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<DaysSinceLastPlayed>k__BackingField = value;
            }
        }

        public int NumberOfPurchases
        {
            [CompilerGenerated]
            get
            {
                return this.<NumberOfPurchases>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<NumberOfPurchases>k__BackingField = value;
            }
        }

        public int NumberOfSessions
        {
            [CompilerGenerated]
            get
            {
                return this.<NumberOfSessions>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<NumberOfSessions>k__BackingField = value;
            }
        }

        public float SessPercentile
        {
            [CompilerGenerated]
            get
            {
                return this.<SessPercentile>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<SessPercentile>k__BackingField = value;
            }
        }

        public float SpendPercentile
        {
            [CompilerGenerated]
            get
            {
                return this.<SpendPercentile>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<SpendPercentile>k__BackingField = value;
            }
        }

        public float SpendProbability
        {
            [CompilerGenerated]
            get
            {
                return this.<SpendProbability>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<SpendProbability>k__BackingField = value;
            }
        }

        public bool Valid
        {
            [CompilerGenerated]
            get
            {
                return this.<Valid>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Valid>k__BackingField = value;
            }
        }
    }
}

