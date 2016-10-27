namespace Service
{
    using System;
    using System.Collections.Generic;

    public class TrackingEventData
    {
        public TrackingEventType action;
        public string appversion;
        public Dictionary<string, string> attr = new Dictionary<string, string>();
        public Dictionary<string, object> payload = new Dictionary<string, object>();
        public string sid;
    }
}

