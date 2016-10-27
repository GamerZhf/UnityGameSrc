namespace Area730.Notifications
{
    using System;
    using UnityEngine;

    public class NotificationTracker
    {
        private const string IDS_KEY = "area730_notification_ids";

        public static void CancelAll()
        {
            char[] separator = new char[] { ';' };
            string[] strArray = PlayerPrefs.GetString("area730_notification_ids").Split(separator);
            for (int i = 0; i < (strArray.Length - 1); i++)
            {
                AndroidNotifications.cancelNotification(Convert.ToInt32(strArray[i]));
            }
            PlayerPrefs.DeleteKey("area730_notification_ids");
        }

        public static void TrackId(int id)
        {
            string str = PlayerPrefs.GetString("area730_notification_ids") + id + ";";
            PlayerPrefs.SetString("area730_notification_ids", str);
        }
    }
}

