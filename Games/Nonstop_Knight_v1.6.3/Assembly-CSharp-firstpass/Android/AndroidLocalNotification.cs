namespace Android
{
    using Area730.Notifications;
    using System;
    using UnityEngine;

    public class AndroidLocalNotification
    {
        public int applicationIconBadgeNumber;
        public bool hasAction = true;
        private static int nextNotificationId = 1;
        private readonly NotificationBuilder notificationBuilder = new NotificationBuilder(nextNotificationId++, string.Empty, string.Empty);

        public AndroidLocalNotification()
        {
            this.notificationBuilder.setDefaults(-1);
            this.notificationBuilder.setSmallIcon("icon_notification");
            this.notificationBuilder.setTitle(Application.productName);
            this.notificationBuilder.setGroupId(0x29a);
        }

        public override string ToString()
        {
            object[] args = new object[] { this.alertAction, this.alertBody, this.fireDate, this.applicationIconBadgeNumber };
            return string.Format("AndroidLocalNotification: {{ alertAction=\"{0}\", alertBody=\"{1}\", fireDate=\"{2}\", applicationIconBadgeNumber=\"{3}\" }} ", args);
        }

        public string alertAction
        {
            get
            {
                return this.notificationBuilder.build().Ticker;
            }
            set
            {
                this.notificationBuilder.setTicker(value);
            }
        }

        public string alertBody
        {
            get
            {
                return this.notificationBuilder.build().Body;
            }
            set
            {
                this.notificationBuilder.setBody(value);
            }
        }

        public string alertLaunchImage
        {
            get
            {
                return this.notificationBuilder.build().SmallIcon;
            }
            set
            {
                this.notificationBuilder.setSmallIcon(value);
            }
        }

        public DateTime fireDate
        {
            get
            {
                return (DateTime.Now + TimeSpan.FromMilliseconds((double) this.notificationBuilder.build().Delay));
            }
            set
            {
                this.notificationBuilder.setDelay((long) value.Subtract(DateTime.Now).TotalMilliseconds);
            }
        }

        internal Notification NotificationInstance
        {
            get
            {
                return this.notificationBuilder.build();
            }
        }
    }
}

