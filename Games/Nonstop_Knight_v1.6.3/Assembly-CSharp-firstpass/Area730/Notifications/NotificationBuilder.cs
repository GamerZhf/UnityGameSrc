namespace Area730.Notifications
{
    using System;

    public class NotificationBuilder
    {
        private bool _alertOnce;
        private bool _autoCancel;
        private string _body = "Notification body";
        private string _color = string.Empty;
        private int _defaults;
        private long _delay;
        private string _group = string.Empty;
        private int _groupId = -1;
        private int _id = 1;
        private long _interval;
        private bool _isRepeating;
        private string _largeIcon = string.Empty;
        private int _number;
        private string _smallIcon = "default_icon";
        private string _sortKey = string.Empty;
        private string _sound = string.Empty;
        private string _ticker = "Notification ticker";
        private string _title = "Notification title";
        private long[] _vibratePattern;
        private long _when;
        public const int DEFAULT_ALL = -1;
        public const string DEFAULT_ICON = "default_icon";
        public const int DEFAULT_SOUND = 1;
        public const int DEFAULT_VIBRATE = 2;

        public NotificationBuilder(int id, string title, string body)
        {
            this._id = id;
            this._title = title;
            this._body = body;
        }

        public Notification build()
        {
            string ticker = this._ticker;
            return new Notification(this._id, this._smallIcon, this._largeIcon, this._defaults, this._autoCancel, this._sound, ticker, this._title, this._body, this._vibratePattern, this._when, this._delay, this._isRepeating, this._interval, this._number, this._alertOnce, this._color, this._group, this._sortKey, this._groupId);
        }

        private long ConvertToMillis(DateTime value)
        {
            return ((value.Ticks - 0x89f7ff5f7b58000L) / 0x2710L);
        }

        public static NotificationBuilder FromInstance(NotificationInstance notif)
        {
            NotificationBuilder builder = new NotificationBuilder(notif.id, notif.title, notif.body);
            if (notif.smallIcon != null)
            {
                builder.setSmallIcon(notif.smallIcon.name);
            }
            if (notif.largeIcon != null)
            {
                builder.setLargeIcon(notif.largeIcon.name);
            }
            if ((notif.ticker != null) && (notif.ticker.Length > 0))
            {
                builder.setTicker(notif.ticker);
            }
            int defaultFlags = 0;
            if (notif.defaultSound)
            {
                defaultFlags |= 1;
            }
            else if (notif.soundFile != null)
            {
                builder.setSound(notif.soundFile.name);
            }
            if (notif.defaultVibrate)
            {
                defaultFlags |= 2;
            }
            else if (notif.vibroPattern != null)
            {
                long[] pattern = notif.vibroPattern.ToArray();
                builder.setVibrate(pattern);
            }
            if (defaultFlags > 0)
            {
                builder.setDefaults(defaultFlags);
            }
            if (notif.number > 0)
            {
                builder.setNumber(notif.number);
            }
            if ((notif.group != null) && (notif.group.Length > 0))
            {
                builder.setGroup(notif.group);
            }
            if ((notif.sortKey != null) && (notif.sortKey.Length > 0))
            {
                builder.setSortKey(notif.sortKey);
            }
            if (notif.hasColor)
            {
                builder.setColor("#" + ColorUtils.ToHtmlStringRGB(notif.color));
            }
            builder.setGroupId(notif.groupId);
            builder.setAutoCancel(notif.autoCancel);
            builder.setAlertOnlyOnce(notif.alertOnce);
            if (notif.isRepeating)
            {
                builder.setRepeating(true);
                long num2 = ((notif.intervalHours * 0xe10) + (notif.intervalMinutes * 60)) + notif.intervalSeconds;
                long interval = num2 * 0x3e8L;
                builder.setInterval(interval);
            }
            long num4 = ((notif.delayHours * 0xe10) + (notif.delayMinutes * 60)) + notif.delaySeconds;
            long delayMilliseconds = num4 * 0x3e8L;
            builder.setDelay(delayMilliseconds);
            return builder;
        }

        public NotificationBuilder setAlertOnlyOnce(bool state)
        {
            this._alertOnce = state;
            return this;
        }

        public NotificationBuilder setAutoCancel(bool state)
        {
            this._autoCancel = state;
            return this;
        }

        public NotificationBuilder setBody(string body)
        {
            this._body = body;
            return this;
        }

        public NotificationBuilder setColor(string color)
        {
            this._color = color;
            return this;
        }

        public NotificationBuilder setDefaults(int defaultFlags)
        {
            this._defaults = defaultFlags;
            return this;
        }

        public NotificationBuilder setDelay(long delayMilliseconds)
        {
            this._delay = delayMilliseconds;
            return this;
        }

        public NotificationBuilder setDelay(TimeSpan delayTimeSpan)
        {
            this._delay = (long) delayTimeSpan.TotalMilliseconds;
            return this;
        }

        public NotificationBuilder setGroup(string group)
        {
            this._group = group;
            return this;
        }

        public NotificationBuilder setGroupId(int groupId)
        {
            this._groupId = groupId;
            return this;
        }

        public NotificationBuilder setId(int id)
        {
            this._id = id;
            return this;
        }

        public NotificationBuilder setInterval(long interval)
        {
            this._interval = interval;
            return this;
        }

        public NotificationBuilder setInterval(TimeSpan intervalTimeSpan)
        {
            this._interval = (long) intervalTimeSpan.TotalMilliseconds;
            return this;
        }

        public NotificationBuilder setLargeIcon(string iconResourceName)
        {
            this._largeIcon = iconResourceName;
            return this;
        }

        public NotificationBuilder setNumber(int num)
        {
            this._number = num;
            return this;
        }

        public NotificationBuilder setRepeating(bool state)
        {
            this._isRepeating = state;
            return this;
        }

        public NotificationBuilder setSmallIcon(string iconResourceName)
        {
            this._smallIcon = iconResourceName;
            return this;
        }

        public NotificationBuilder setSortKey(string sortKey)
        {
            this._sortKey = sortKey;
            return this;
        }

        public NotificationBuilder setSound(string soundResourceName)
        {
            this._sound = soundResourceName;
            return this;
        }

        public NotificationBuilder setTicker(string ticker)
        {
            this._ticker = ticker;
            return this;
        }

        public NotificationBuilder setTitle(string title)
        {
            this._title = title;
            return this;
        }

        public NotificationBuilder setVibrate(long[] pattern)
        {
            this._vibratePattern = pattern;
            return this;
        }
    }
}

