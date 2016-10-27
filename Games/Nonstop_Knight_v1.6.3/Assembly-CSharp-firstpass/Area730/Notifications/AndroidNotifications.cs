namespace Area730.Notifications
{
    using System;
    using UnityEngine;

    public class AndroidNotifications
    {
        private static DataHolder _dataHolder;
        private static AndroidJavaClass notifHandlerClass = new AndroidJavaClass("com.area730.localnotif.NotificationHandler");

        static AndroidNotifications()
        {
            if (notifHandlerClass == null)
            {
                Debug.LogError("Class com.area730.localnotif.NotificationHandler not found");
            }
            else
            {
                Debug.Log("Android notifications plugin loaded. Version: " + getVersion());
                _dataHolder = (DataHolder) Resources.Load("NotificationData");
            }
        }

        public static void cancelAll()
        {
            NotificationTracker.CancelAll();
        }

        public static void cancelNotification(Notification notif)
        {
            cancelNotification(notif.ID);
        }

        public static void cancelNotification(int id)
        {
            object[] args = new object[] { id };
            notifHandlerClass.CallStatic("cancelNotifications", args);
        }

        public static void clear(int id)
        {
            object[] args = new object[] { id };
            notifHandlerClass.CallStatic("clear", args);
        }

        public static void clearAll()
        {
            notifHandlerClass.CallStatic("clearAll", new object[0]);
        }

        public static NotificationBuilder GetNotificationBuilderByIndex(int pos)
        {
            NotificationInstance notif = _dataHolder.notifications[pos];
            return NotificationBuilder.FromInstance(notif);
        }

        public static NotificationBuilder GetNotificationBuilderByName(string name)
        {
            foreach (NotificationInstance instance in _dataHolder.notifications)
            {
                if (instance.name.Equals(name))
                {
                    return NotificationBuilder.FromInstance(instance);
                }
            }
            return null;
        }

        public static float getVersion()
        {
            return notifHandlerClass.CallStatic<float>("getVersion", new object[0]);
        }

        public static void scheduleNotification(Notification notif)
        {
            NotificationTracker.TrackId(notif.ID);
            object[] args = new object[] { 
                notif.Delay, notif.ID, notif.Title, notif.Body, notif.Ticker, notif.SmallIcon, notif.LargeIcon, notif.Defaults, notif.AutoCancel, notif.Sound, notif.VibratePattern, notif.When, notif.IsRepeating, notif.Interval, notif.Number, notif.AlertOnce, 
                notif.Color, _dataHolder.unityClass, notif.Group, notif.SortKey, notif.GroupId
             };
            notifHandlerClass.CallStatic("scheduleNotification", args);
        }

        public static void showToast(string text)
        {
            object[] args = new object[] { text };
            notifHandlerClass.CallStatic("showToast", args);
        }
    }
}

