namespace GooglePlayGames.BasicApi
{
    using System;

    public class Achievement
    {
        private int mCurrentSteps;
        private string mDescription = string.Empty;
        private string mId = string.Empty;
        private bool mIsIncremental;
        private bool mIsRevealed;
        private bool mIsUnlocked;
        private long mLastModifiedTime;
        private string mName = string.Empty;
        private ulong mPoints;
        private string mRevealedImageUrl;
        private int mTotalSteps;
        private string mUnlockedImageUrl;
        private static readonly DateTime UnixEpoch = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public override string ToString()
        {
            object[] args = new object[] { this.mId, this.mName, this.mDescription, !this.mIsIncremental ? "STANDARD" : "INCREMENTAL", this.mIsRevealed, this.mIsUnlocked, this.mCurrentSteps, this.mTotalSteps };
            return string.Format("[Achievement] id={0}, name={1}, desc={2}, type={3}, revealed={4}, unlocked={5}, steps={6}/{7}", args);
        }

        public int CurrentSteps
        {
            get
            {
                return this.mCurrentSteps;
            }
            set
            {
                this.mCurrentSteps = value;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
            set
            {
                this.mDescription = value;
            }
        }

        public string Id
        {
            get
            {
                return this.mId;
            }
            set
            {
                this.mId = value;
            }
        }

        public bool IsIncremental
        {
            get
            {
                return this.mIsIncremental;
            }
            set
            {
                this.mIsIncremental = value;
            }
        }

        public bool IsRevealed
        {
            get
            {
                return this.mIsRevealed;
            }
            set
            {
                this.mIsRevealed = value;
            }
        }

        public bool IsUnlocked
        {
            get
            {
                return this.mIsUnlocked;
            }
            set
            {
                this.mIsUnlocked = value;
            }
        }

        public DateTime LastModifiedTime
        {
            get
            {
                return UnixEpoch.AddMilliseconds((double) this.mLastModifiedTime);
            }
            set
            {
                TimeSpan span = (TimeSpan) (value - UnixEpoch);
                this.mLastModifiedTime = (long) span.TotalMilliseconds;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        public ulong Points
        {
            get
            {
                return this.mPoints;
            }
            set
            {
                this.mPoints = value;
            }
        }

        public string RevealedImageUrl
        {
            get
            {
                return this.mRevealedImageUrl;
            }
            set
            {
                this.mRevealedImageUrl = value;
            }
        }

        public int TotalSteps
        {
            get
            {
                return this.mTotalSteps;
            }
            set
            {
                this.mTotalSteps = value;
            }
        }

        public string UnlockedImageUrl
        {
            get
            {
                return this.mUnlockedImageUrl;
            }
            set
            {
                this.mUnlockedImageUrl = value;
            }
        }
    }
}

