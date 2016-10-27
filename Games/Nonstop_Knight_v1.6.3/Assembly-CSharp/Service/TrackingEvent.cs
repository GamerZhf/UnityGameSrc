namespace Service
{
    using System;

    public class TrackingEvent : TrackingEventBase
    {
        public TrackingEvent()
        {
        }

        public TrackingEvent(string _eventName)
        {
            base.Data.action = TrackingEventType.EVENT;
            this.EventName = _eventName;
        }

        public TrackingEvent(string _eventName, params object[] _payload) : this(_eventName)
        {
            for (int i = 1; i < _payload.Length; i += 2)
            {
                if (_payload[i - 1] != null)
                {
                    object obj1 = _payload[i];
                    if (obj1 == null)
                    {
                    }
                    base.Payload[_payload[i - 1].ToString()] = string.Empty;
                }
            }
        }

        public virtual bool Validate()
        {
            return ((base.Data.action != TrackingEventType.EVENT) || (this.EventName != null));
        }

        public string EventName
        {
            get
            {
                return (!base.Data.attr.ContainsKey("eventName") ? null : base.Data.attr["eventName"]);
            }
            set
            {
                base.Data.attr["eventName"] = value;
            }
        }
    }
}

