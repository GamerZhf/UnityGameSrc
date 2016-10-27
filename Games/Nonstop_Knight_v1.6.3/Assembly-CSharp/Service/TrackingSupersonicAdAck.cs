namespace Service
{
    using System;

    public class TrackingSupersonicAdAck : TrackingEvent
    {
        public TrackingSupersonicAdAck()
        {
            base.Data.action = TrackingEventType.SUPERSONIC_AD_ACKNOWLEDGEMENT;
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

