namespace Area730.Notifications
{
    using System;

    public class Notification
    {
        private bool _alertOnce;
        private bool _autoCancel;
        private string _body;
        private string _color;
        private int _defaults;
        private long _delay;
        private string _group;
        private int _groupId;
        private int _id;
        private long _interval;
        private bool _isRepeating;
        private string _largeIcon;
        private int _number;
        private string _smallIcon;
        private string _sortKey;
        private string _sound;
        private string _ticker;
        private string _title;
        private long[] _vibratePattern;
        private long _when;

        public Notification(int id, string smallIcon, string largeIcon, int defaults, bool autoCancel, string sound, string ticker, string title, string body, long[] vibroPattern, long when, long delay, bool repeating, long interval, int number, bool alertOnce, string color, string group, string sortKey, int groupId)
        {
            this._id = id;
            this._smallIcon = smallIcon;
            this._largeIcon = largeIcon;
            this._defaults = defaults;
            this._autoCancel = autoCancel;
            this._sound = sound;
            this._ticker = ticker;
            this._title = title;
            this._body = body;
            this._vibratePattern = vibroPattern;
            this._when = when;
            this._delay = delay;
            this._isRepeating = repeating;
            this._interval = interval;
            this._number = number;
            this._alertOnce = alertOnce;
            this._color = color;
            this._group = group;
            this._sortKey = sortKey;
            this._groupId = groupId;
        }

        public override string ToString()
        {
            string str2;
            string str = string.Concat(new object[] { 
                "Notification: \nID: ", this._id, "\n, number: ", this._number, ", SmallIcon: ", this._smallIcon, "\n, LargeIcon: ", this._largeIcon, "\n, defaults: ", this._defaults, "\n, AutoCancel: ", this._autoCancel, "\n, Sound: ", this._sound, "\n, Ticker: ", this._ticker, 
                "\n, Title: ", this._title, "\n, Body: ", this._body, "\n"
             }) + ", pattern: \n";
            if (this._vibratePattern != null)
            {
                foreach (long num in this._vibratePattern)
                {
                    str2 = str;
                    object[] objArray2 = new object[] { str2, "vibro: ", num, "\n" };
                    str = string.Concat(objArray2);
                }
            }
            str2 = str;
            object[] objArray3 = new object[] { 
                str2, ", When: ", this._when, "\n,delay: ", this._delay, "\n, isRepeating: ", this._isRepeating, "\n, alertOnce: ", this._alertOnce, "\n, interval: ", this._interval, "\n, color: ", this._color, "\n, group: ", this._group, "\n, sort key: ", 
                this._sortKey, "\n, group id: ", this._groupId, "\n"
             };
            return string.Concat(objArray3);
        }

        public bool AlertOnce
        {
            get
            {
                return this._alertOnce;
            }
        }

        public bool AutoCancel
        {
            get
            {
                return this._autoCancel;
            }
        }

        public string Body
        {
            get
            {
                return this._body;
            }
        }

        public string Color
        {
            get
            {
                return this._color;
            }
        }

        public int Defaults
        {
            get
            {
                return this._defaults;
            }
        }

        public long Delay
        {
            get
            {
                return this._delay;
            }
        }

        public string Group
        {
            get
            {
                return this._group;
            }
        }

        public int GroupId
        {
            get
            {
                return this._groupId;
            }
        }

        public int ID
        {
            get
            {
                return this._id;
            }
        }

        public long Interval
        {
            get
            {
                return this._interval;
            }
        }

        public bool IsRepeating
        {
            get
            {
                return this._isRepeating;
            }
        }

        public string LargeIcon
        {
            get
            {
                return this._largeIcon;
            }
        }

        public int Number
        {
            get
            {
                return this._number;
            }
        }

        public string SmallIcon
        {
            get
            {
                return this._smallIcon;
            }
        }

        public string SortKey
        {
            get
            {
                return this._sortKey;
            }
        }

        public string Sound
        {
            get
            {
                return this._sound;
            }
        }

        public string Ticker
        {
            get
            {
                return this._ticker;
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }
        }

        public long[] VibratePattern
        {
            get
            {
                return this._vibratePattern;
            }
        }

        public long When
        {
            get
            {
                return this._when;
            }
        }
    }
}

