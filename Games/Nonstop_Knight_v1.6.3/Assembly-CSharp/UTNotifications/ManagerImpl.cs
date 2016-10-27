namespace UTNotifications
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ManagerImpl : Manager
    {
        private const float m_timeBetweenCheckingForIncomingNotifications = 0.5f;
        private float m_timeToCheckForIncomingNotifications;
        private bool m_willHandleReceivedNotifications;

        public void _OnAndroidIdReceived(string providerAndId)
        {
            JSONNode node = JSON.Parse(providerAndId);
            if (base.OnSendRegistrationIdHasSubscribers())
            {
                base._OnSendRegistrationId((string) node[0], (string) node[1]);
            }
        }

        public override void CancelAllNotifications()
        {
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    class2.CallStatic("cancelAllNotifications", new object[0]);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
            }
        }

        public override void CancelNotification(int id)
        {
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    object[] args = new object[] { id };
                    class2.CallStatic("cancelNotification", args);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
            }
            this.HideNotification(id);
        }

        public override int GetBadge()
        {
            base.NotSupported("badges");
            return 0;
        }

        private void HandleClickedNotification(string receivedNotificationPacked)
        {
            if (!string.IsNullOrEmpty(receivedNotificationPacked))
            {
                base._OnNotificationClicked(this.ParseReceivedNotification(JSON.Parse(receivedNotificationPacked)));
            }
        }

        private void HandleReceivedNotifications(string receivedNotificationsPacked)
        {
            if (!string.IsNullOrEmpty(receivedNotificationsPacked) && (receivedNotificationsPacked != "[]"))
            {
                List<ReceivedNotification> receivedNotifications = new List<ReceivedNotification>();
                JSONNode node = JSON.Parse(receivedNotificationsPacked);
                for (int i = 0; i < node.Count; i++)
                {
                    JSONNode json = node[i];
                    ReceivedNotification item = this.ParseReceivedNotification(json);
                    bool flag = false;
                    for (int j = 0; j < receivedNotifications.Count; j++)
                    {
                        if (receivedNotifications[j].id == item.id)
                        {
                            receivedNotifications[j] = item;
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        receivedNotifications.Add(item);
                    }
                }
                base._OnNotificationsReceived(receivedNotifications);
            }
        }

        public override void HideAllNotifications()
        {
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    class2.CallStatic("hideAllNotifications", new object[0]);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
            }
        }

        public override void HideNotification(int id)
        {
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    object[] args = new object[] { id };
                    class2.CallStatic("hideNotification", args);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
            }
        }

        public override bool Initialize(bool willHandleReceivedNotifications, [Optional, DefaultParameterValue(0)] int startId, [Optional, DefaultParameterValue(false)] bool incrementalId)
        {
            bool flag;
            this.m_willHandleReceivedNotifications = willHandleReceivedNotifications;
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    object[] args = new object[] { Settings.Instance.PushNotificationsEnabledGooglePlay, Settings.Instance.PushNotificationsEnabledAmazon, Settings.Instance.GooglePlaySenderID, willHandleReceivedNotifications, startId, incrementalId, (int) Settings.Instance.AndroidShowNotificationsMode, Settings.Instance.AndroidRestoreScheduledNotificationsAfterReboot, (int) Settings.Instance.AndroidNotificationsGrouping, Settings.Instance.AndroidShowLatestNotificationOnly };
                    return class2.CallStatic<bool>("initialize", args);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
                flag = false;
            }
            return flag;
        }

        protected void LateUpdate()
        {
            this.m_timeToCheckForIncomingNotifications -= Time.deltaTime;
            if (this.m_timeToCheckForIncomingNotifications <= 0f)
            {
                this.m_timeToCheckForIncomingNotifications = 0.5f;
                if (base.OnNotificationClickedHasSubscribers())
                {
                    try
                    {
                        using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                        {
                            this.HandleClickedNotification(class2.CallStatic<string>("getClickedNotificationPacked", new object[0]));
                        }
                    }
                    catch (AndroidJavaException exception)
                    {
                        Debug.LogException(exception);
                    }
                }
                if (this.m_willHandleReceivedNotifications && base.OnNotificationsReceivedHasSubscribers())
                {
                    try
                    {
                        using (AndroidJavaClass class3 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                        {
                            this.HandleReceivedNotifications(class3.CallStatic<string>("getReceivedNotificationsPacked", new object[0]));
                        }
                    }
                    catch (AndroidJavaException exception2)
                    {
                        Debug.LogException(exception2);
                    }
                }
            }
        }

        public override bool NotificationsEnabled()
        {
            bool flag;
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    return class2.CallStatic<bool>("notificationsEnabled", new object[0]);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
                flag = false;
            }
            return flag;
        }

        protected void OnApplicationPause(bool paused)
        {
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    object[] args = new object[] { paused };
                    class2.CallStatic("setBackgroundMode", args);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
            }
        }

        private static string[] PackUserData(IDictionary<string, string> userData)
        {
            if ((userData == null) || (userData.Count == 0))
            {
                return null;
            }
            string[] strArray = new string[userData.Count * 2];
            int num = 0;
            IEnumerator<KeyValuePair<string, string>> enumerator = userData.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, string> current = enumerator.Current;
                    strArray[num++] = current.Key;
                    strArray[num++] = current.Value;
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            return strArray;
        }

        private ReceivedNotification ParseReceivedNotification(JSONNode json)
        {
            Dictionary<string, string> dictionary;
            string title = WWW.UnEscapeURL(json["title"].Value);
            string text = WWW.UnEscapeURL(json["text"].Value);
            int asInt = json["id"].AsInt;
            string notificationProfile = json["notification_profile"].Value;
            JSONNode node = json["user_data"];
            if ((node != null) && (node.Count > 0))
            {
                dictionary = new Dictionary<string, string>();
                IEnumerator enumerator = ((JSONClass) node).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<string, JSONNode> current = (KeyValuePair<string, JSONNode>) enumerator.Current;
                        string key = WWW.UnEscapeURL(current.Key);
                        dictionary.Add(key, WWW.UnEscapeURL(current.Value.Value));
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable == null)
                    {
                    }
                    disposable.Dispose();
                }
            }
            else
            {
                dictionary = null;
            }
            return new ReceivedNotification(title, text, asInt, dictionary, notificationProfile);
        }

        public override void PostLocalNotification(string title, string text, int id, IDictionary<string, string> userData, string notificationProfile)
        {
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    object[] args = new object[] { title, text, id, PackUserData(userData), notificationProfile };
                    class2.CallStatic("postNotification", args);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
            }
        }

        public override void ScheduleNotification(int triggerInSeconds, string title, string text, int id, IDictionary<string, string> userData, string notificationProfile)
        {
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    object[] args = new object[] { triggerInSeconds, title, text, id, PackUserData(userData), notificationProfile };
                    class2.CallStatic("scheduleNotification", args);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
            }
        }

        public override void ScheduleNotificationRepeating(int firstTriggerInSeconds, int intervalSeconds, string title, string text, int id, IDictionary<string, string> userData, string notificationProfile)
        {
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    object[] args = new object[] { firstTriggerInSeconds, intervalSeconds, title, text, id, PackUserData(userData), notificationProfile };
                    class2.CallStatic("scheduleNotificationRepeating", args);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
            }
        }

        public override void SetBadge(int bandgeNumber)
        {
            base.NotSupported("badges");
        }

        public override void SetNotificationsEnabled(bool enabled)
        {
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("universal.tools.notifications.Manager"))
                {
                    object[] args = new object[] { enabled };
                    class2.CallStatic("setNotificationsEnabled", args);
                }
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}

