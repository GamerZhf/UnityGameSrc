namespace Service
{
    using System;
    using System.Collections.Generic;

    public class TournamentViewRemote
    {
        public string BucketId;
        public long BucketStartTime = -1L;
        public List<TournamentEntry> Entries = new List<TournamentEntry>();
        public TournamentLog Log = new TournamentLog();
        public Status status;
        public Service.TournamentInfo TournamentInfo;

        public enum Status
        {
            OK,
            TooEarlyForJoin,
            TooLateForJoin,
            BucketTimeEnded,
            Expired
        }
    }
}

