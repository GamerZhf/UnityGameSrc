namespace GooglePlayGames
{
    using GooglePlayGames.BasicApi;
    using System;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    internal class PlayGamesAchievement : IAchievementDescription, IAchievement
    {
        private bool mCompleted;
        private int mCurrentSteps;
        private string mDescription;
        private bool mHidden;
        private string mId;
        private Texture2D mImage;
        private WWW mImageFetcher;
        private bool mIsIncremental;
        private DateTime mLastModifiedTime;
        private double mPercentComplete;
        private ulong mPoints;
        private readonly GooglePlayGames.ReportProgress mProgressCallback;
        private string mRevealedImageUrl;
        private string mTitle;
        private int mTotalSteps;
        private string mUnlockedImageUrl;

        internal PlayGamesAchievement() : this(new GooglePlayGames.ReportProgress(instance.ReportProgress))
        {
            PlayGamesPlatform instance = PlayGamesPlatform.Instance;
        }

        internal PlayGamesAchievement(Achievement ach) : this()
        {
            this.mId = ach.Id;
            this.mIsIncremental = ach.IsIncremental;
            this.mCurrentSteps = ach.CurrentSteps;
            this.mTotalSteps = ach.TotalSteps;
            if (ach.IsIncremental)
            {
                if (ach.TotalSteps > 0)
                {
                    this.mPercentComplete = (((double) ach.CurrentSteps) / ((double) ach.TotalSteps)) * 100.0;
                }
                else
                {
                    this.mPercentComplete = 0.0;
                }
            }
            else
            {
                this.mPercentComplete = !ach.IsUnlocked ? 0.0 : 100.0;
            }
            this.mCompleted = ach.IsUnlocked;
            this.mHidden = !ach.IsRevealed;
            this.mLastModifiedTime = ach.LastModifiedTime;
            this.mTitle = ach.Name;
            this.mDescription = ach.Description;
            this.mPoints = ach.Points;
            this.mRevealedImageUrl = ach.RevealedImageUrl;
            this.mUnlockedImageUrl = ach.UnlockedImageUrl;
        }

        internal PlayGamesAchievement(GooglePlayGames.ReportProgress progressCallback)
        {
            this.mId = string.Empty;
            this.mLastModifiedTime = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0);
            this.mTitle = string.Empty;
            this.mRevealedImageUrl = string.Empty;
            this.mUnlockedImageUrl = string.Empty;
            this.mDescription = string.Empty;
            this.mProgressCallback = progressCallback;
        }

        private Texture2D LoadImage()
        {
            if (!this.hidden)
            {
                string str = !this.completed ? this.mRevealedImageUrl : this.mUnlockedImageUrl;
                if (!string.IsNullOrEmpty(str))
                {
                    if ((this.mImageFetcher == null) || (this.mImageFetcher.url != str))
                    {
                        this.mImageFetcher = new WWW(str);
                        this.mImage = null;
                    }
                    if (this.mImage != null)
                    {
                        return this.mImage;
                    }
                    if (this.mImageFetcher.isDone)
                    {
                        this.mImage = this.mImageFetcher.texture;
                        return this.mImage;
                    }
                }
            }
            return null;
        }

        public void ReportProgress(Action<bool> callback)
        {
            this.mProgressCallback(this.mId, this.mPercentComplete, callback);
        }

        public string achievedDescription
        {
            get
            {
                return this.mDescription;
            }
        }

        public bool completed
        {
            get
            {
                return this.mCompleted;
            }
        }

        public int currentSteps
        {
            get
            {
                return this.mCurrentSteps;
            }
        }

        public bool hidden
        {
            get
            {
                return this.mHidden;
            }
        }

        public string id
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

        public Texture2D image
        {
            get
            {
                return this.LoadImage();
            }
        }

        public bool isIncremental
        {
            get
            {
                return this.mIsIncremental;
            }
        }

        public DateTime lastReportedDate
        {
            get
            {
                return this.mLastModifiedTime;
            }
        }

        public double percentCompleted
        {
            get
            {
                return this.mPercentComplete;
            }
            set
            {
                this.mPercentComplete = value;
            }
        }

        public int points
        {
            get
            {
                return (int) this.mPoints;
            }
        }

        public string title
        {
            get
            {
                return this.mTitle;
            }
        }

        public int totalSteps
        {
            get
            {
                return this.mTotalSteps;
            }
        }

        public string unachievedDescription
        {
            get
            {
                return this.mDescription;
            }
        }
    }
}

