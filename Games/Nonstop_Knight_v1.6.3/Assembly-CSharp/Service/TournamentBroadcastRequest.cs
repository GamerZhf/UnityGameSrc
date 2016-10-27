namespace Service
{
    using System;
    using System.Collections.Generic;

    public class TournamentBroadcastRequest
    {
        public Dictionary<string, object> CustomParams;
        public BroadcastKey Key;
        public TournamentLogEvent LogEvent;
    }
}

