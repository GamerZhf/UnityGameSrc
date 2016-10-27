namespace GooglePlayGames
{
    using GooglePlayGames.BasicApi;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class PlayGamesLeaderboard : ILeaderboard
    {
        private string[] mFilteredUserIds;
        private string mId;
        private bool mLoading;
        private IScore mLocalUserScore;
        private uint mMaxRange;
        private Range mRange;
        private List<PlayGamesScore> mScoreList = new List<PlayGamesScore>();
        private TimeScope mTimeScope;
        private string mTitle;
        private UserScope mUserScope;

        public PlayGamesLeaderboard(string id)
        {
            this.mId = id;
        }

        internal int AddScore(PlayGamesScore score)
        {
            if ((this.mFilteredUserIds == null) || (this.mFilteredUserIds.Length == 0))
            {
                this.mScoreList.Add(score);
            }
            else
            {
                foreach (string str in this.mFilteredUserIds)
                {
                    if (str.Equals(score.userID))
                    {
                        return this.mScoreList.Count;
                    }
                }
                this.mScoreList.Add(score);
            }
            return this.mScoreList.Count;
        }

        internal bool HasAllScores()
        {
            return ((this.mScoreList.Count >= this.mRange.count) || (this.mScoreList.Count >= this.maxRange));
        }

        public void LoadScores(Action<bool> callback)
        {
            PlayGamesPlatform.Instance.LoadScores(this, callback);
        }

        internal bool SetFromData(LeaderboardScoreData data)
        {
            if (data.Valid)
            {
                Debug.Log("Setting leaderboard from: " + data);
                this.SetMaxRange(data.ApproximateCount);
                this.SetTitle(data.Title);
                this.SetLocalUserScore((PlayGamesScore) data.PlayerScore);
                foreach (IScore score in data.Scores)
                {
                    this.AddScore((PlayGamesScore) score);
                }
                this.mLoading = (data.Scores.Length == 0) || this.HasAllScores();
            }
            return data.Valid;
        }

        internal void SetLocalUserScore(PlayGamesScore score)
        {
            this.mLocalUserScore = score;
        }

        internal void SetMaxRange(ulong val)
        {
            this.mMaxRange = (uint) val;
        }

        internal void SetTitle(string value)
        {
            this.mTitle = value;
        }

        public void SetUserFilter(string[] userIDs)
        {
            this.mFilteredUserIds = userIDs;
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

        public bool loading
        {
            get
            {
                return this.mLoading;
            }
            internal set
            {
                this.mLoading = value;
            }
        }

        public IScore localUserScore
        {
            get
            {
                return this.mLocalUserScore;
            }
        }

        public uint maxRange
        {
            get
            {
                return this.mMaxRange;
            }
        }

        public Range range
        {
            get
            {
                return this.mRange;
            }
            set
            {
                this.mRange = value;
            }
        }

        public int ScoreCount
        {
            get
            {
                return this.mScoreList.Count;
            }
        }

        public IScore[] scores
        {
            get
            {
                PlayGamesScore[] array = new PlayGamesScore[this.mScoreList.Count];
                this.mScoreList.CopyTo(array);
                return array;
            }
        }

        public TimeScope timeScope
        {
            get
            {
                return this.mTimeScope;
            }
            set
            {
                this.mTimeScope = value;
            }
        }

        public string title
        {
            get
            {
                return this.mTitle;
            }
        }

        public UserScope userScope
        {
            get
            {
                return this.mUserScope;
            }
            set
            {
                this.mUserScope = value;
            }
        }
    }
}

