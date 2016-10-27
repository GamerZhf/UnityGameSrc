namespace Service
{
    using System;

    public class TrackingUserAttr : TrackingEvent
    {
        public TrackingUserAttr()
        {
            base.Data.action = TrackingEventType.USER_ATTR;
        }

        public string Get(string _attr)
        {
            return base.Data.attr[_attr];
        }

        public void Set(string _attr, string _val)
        {
            base.Data.attr[_attr] = _val;
        }

        public override bool Validate()
        {
            return (base.Data.attr.Count > 0);
        }
    }
}

