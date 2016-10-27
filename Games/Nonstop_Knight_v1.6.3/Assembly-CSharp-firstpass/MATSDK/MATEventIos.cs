namespace MATSDK
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct MATEventIos
    {
        public string name;
        public string eventId;
        public string revenue;
        public string currencyCode;
        public string advertiserRefId;
        public string transactionState;
        public string contentType;
        public string contentId;
        public string level;
        public string quantity;
        public string searchString;
        public string rating;
        public string date1;
        public string date2;
        public string attribute1;
        public string attribute2;
        public string attribute3;
        public string attribute4;
        public string attribute5;
        private MATEventIos(int dummy1, int dummy2)
        {
            this.eventId = null;
            this.name = null;
            this.revenue = null;
            this.currencyCode = null;
            this.advertiserRefId = null;
            this.transactionState = null;
            this.contentType = null;
            this.contentId = null;
            this.level = null;
            this.quantity = null;
            this.searchString = null;
            this.rating = null;
            this.date1 = null;
            this.date2 = null;
            this.attribute1 = null;
            this.attribute2 = null;
            this.attribute3 = null;
            this.attribute4 = null;
            this.attribute5 = null;
        }

        public MATEventIos(string name) : this(0, 0)
        {
            this.name = name;
        }

        public MATEventIos(int id) : this(0, 0)
        {
            this.eventId = id.ToString();
        }

        public MATEventIos(MATEvent matEvent)
        {
            this.name = matEvent.name;
            this.eventId = (matEvent.name != null) ? null : matEvent.id.ToString();
            this.advertiserRefId = matEvent.advertiserRefId;
            this.attribute1 = matEvent.attribute1;
            this.attribute2 = matEvent.attribute2;
            this.attribute3 = matEvent.attribute3;
            this.attribute4 = matEvent.attribute4;
            this.attribute5 = matEvent.attribute5;
            this.contentId = (matEvent.contentId != null) ? matEvent.contentId.ToString() : null;
            this.contentType = matEvent.contentType;
            this.currencyCode = matEvent.currencyCode;
            this.level = matEvent.level.HasValue ? matEvent.level.ToString() : null;
            this.quantity = matEvent.quantity.HasValue ? matEvent.quantity.ToString() : null;
            this.rating = matEvent.rating.HasValue ? matEvent.rating.ToString() : null;
            this.revenue = matEvent.revenue.HasValue ? matEvent.revenue.ToString() : null;
            this.searchString = matEvent.searchString;
            this.transactionState = matEvent.transactionState.HasValue ? matEvent.transactionState.ToString() : null;
            this.date1 = null;
            this.date2 = null;
            DateTime time = new DateTime(0x7b2, 1, 1);
            if (matEvent.date1.HasValue)
            {
                TimeSpan span = new TimeSpan(matEvent.date1.Value.Ticks);
                TimeSpan span2 = new TimeSpan(time.Ticks);
                this.date1 = (span.TotalMilliseconds - span2.TotalMilliseconds).ToString();
            }
            if (matEvent.date2.HasValue)
            {
                TimeSpan span3 = new TimeSpan(matEvent.date2.Value.Ticks);
                TimeSpan span4 = new TimeSpan(time.Ticks);
                this.date2 = (span3.TotalMilliseconds - span4.TotalMilliseconds).ToString();
            }
        }
    }
}

