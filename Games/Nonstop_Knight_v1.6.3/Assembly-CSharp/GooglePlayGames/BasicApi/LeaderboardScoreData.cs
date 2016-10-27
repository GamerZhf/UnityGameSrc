namespace GooglePlayGames.BasicApi
{
    using GooglePlayGames;
    using System;
    using System.Collections.Generic;
    using UnityEngine.SocialPlatforms;

    public class LeaderboardScoreData
    {
        private ulong mApproxCount;
        private string mId;
        private ScorePageToken mNextPage;
        private IScore mPlayerScore;
        private ScorePageToken mPrevPage;
        private List<PlayGamesScore> mScores;
        private ResponseStatus mStatus;
        private string mTitle;

        internal LeaderboardScoreData(string leaderboardId)
        {
            this.mScores = new List<PlayGamesScore>();
            this.mId = leaderboardId;
        }

        internal LeaderboardScoreData(string leaderboardId, ResponseStatus status)
        {
            this.mScores = new List<PlayGamesScore>();
            this.mId = leaderboardId;
            this.mStatus = status;
        }

        internal int AddScore(PlayGamesScore score)
        {
            this.mScores.Add(score);
            return this.mScores.Count;
        }

        public override string ToString()
        {
            object[] args = new object[] { this.mId, this.mStatus, this.mApproxCount, this.mTitle };
            return string.Format("[LeaderboardScoreData: mId={0},  mStatus={1}, mApproxCount={2}, mTitle={3}]", args);
        }

        public ulong ApproximateCount
        {
            get
            {
                return this.mApproxCount;
            }
            internal set
            {
                this.mApproxCount = value;
            }
        }

        public string Id
        {
            get
            {
                return this.mId;
            }
            internal set
            {
                this.mId = value;
            }
        }

        public ScorePageToken NextPageToken
        {
            get
            {
                return this.mNextPage;
            }
            internal set
            {
                this.mNextPage = value;
            }
        }

        public IScore PlayerScore
        {
            get
            {
                return this.mPlayerScore;
            }
            internal set
            {
                this.mPlayerScore = value;
            }
        }

        public ScorePageToken PrevPageToken
        {
            get
            {
                return this.mPrevPage;
            }
            internal set
            {
                this.mPrevPage = value;
            }
        }

        public IScore[] Scores
        {
            get
            {
                return this.mScores.ToArray();
            }
        }

        public ResponseStatus Status
        {
            get
            {
                return this.mStatus;
            }
            internal set
            {
                this.mStatus = value;
            }
        }

        public string Title
        {
            get
            {
                return this.mTitle;
            }
            internal set
            {
                this.mTitle = value;
            }
        }

        public bool Valid
        {
            get
            {
                return ((this.mStatus == ResponseStatus.Success) || (this.mStatus == ResponseStatus.SuccessWithStale));
            }
        }
    }
}

