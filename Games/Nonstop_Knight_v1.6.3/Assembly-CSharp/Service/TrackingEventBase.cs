namespace Service
{
    using System;
    using System.Collections.Generic;

    public class TrackingEventBase
    {
        public TrackingEventData Data = new TrackingEventData();

        protected TrackingEventBase()
        {
        }

        public TrackingEventData GetData()
        {
            return this.Data;
        }

        public void SetSession(string sid, string clientVersion)
        {
            this.Data.sid = sid;
            this.Data.appversion = clientVersion;
        }

        public IDictionary<string, string> Attr
        {
            get
            {
                return this.Data.attr;
            }
        }

        public IDictionary<string, object> Payload
        {
            get
            {
                return this.Data.payload;
            }
        }
    }
}

