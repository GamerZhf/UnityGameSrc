namespace GooglePlayGames.BasicApi
{
    using System;

    public class ScorePageToken
    {
        private LeaderboardCollection mCollection;
        private string mId;
        private object mInternalObject;
        private LeaderboardTimeSpan mTimespan;

        internal ScorePageToken(object internalObject, string id, LeaderboardCollection collection, LeaderboardTimeSpan timespan)
        {
            this.mInternalObject = internalObject;
            this.mId = id;
            this.mCollection = collection;
            this.mTimespan = timespan;
        }

        public LeaderboardCollection Collection
        {
            get
            {
                return this.mCollection;
            }
        }

        internal object InternalObject
        {
            get
            {
                return this.mInternalObject;
            }
        }

        public string LeaderboardId
        {
            get
            {
                return this.mId;
            }
        }

        public LeaderboardTimeSpan TimeSpan
        {
            get
            {
                return this.mTimespan;
            }
        }
    }
}

