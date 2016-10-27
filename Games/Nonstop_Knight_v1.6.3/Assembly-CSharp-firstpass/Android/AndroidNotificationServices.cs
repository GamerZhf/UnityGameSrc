namespace Android
{
    using Area730.Notifications;
    using System;
    using System.Collections.Generic;

    public class AndroidNotificationServices
    {
        public static AndroidNotificationType enabledNotificationTypes = AndroidNotificationType.None;
        private static readonly List<AndroidLocalNotification> scheduledLocalNotificationList = new List<AndroidLocalNotification>();

        public static void CancelAllLocalNotifications()
        {
            scheduledLocalNotificationList.Clear();
            AndroidNotifications.cancelAll();
        }

        public static void ClearLocalNotifications()
        {
            AndroidNotifications.clearAll();
        }

        public static void ClearRemoteNotifications()
        {
        }

        public static void RegisterForNotifications(AndroidNotificationType notificationTypes, bool registerForRemote)
        {
            enabledNotificationTypes = notificationTypes;
        }

        public static void ScheduleLocalNotification(AndroidLocalNotification notification)
        {
            if (notification.hasAction)
            {
                AndroidNotifications.scheduleNotification(notification.NotificationInstance);
                scheduledLocalNotificationList.Add(notification);
            }
        }

        public static AndroidLocalNotification[] scheduledLocalNotifications
        {
            get
            {
                return scheduledLocalNotificationList.ToArray();
            }
        }
    }
}

