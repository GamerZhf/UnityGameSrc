namespace Service
{
    using System;
    using System.Collections.Generic;

    public class LeaderboardResponse
    {
        public List<RemoteLeaderboardEntry> entries;

        public LeaderboardResponse()
        {
            this.entries = new List<RemoteLeaderboardEntry>();
        }

        public LeaderboardResponse(List<RemoteLeaderboardEntry> _entries)
        {
            this.entries = _entries;
        }
    }
}

