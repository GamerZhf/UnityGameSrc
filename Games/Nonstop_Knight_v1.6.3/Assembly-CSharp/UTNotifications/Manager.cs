namespace UTNotifications
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public abstract class Manager : MonoBehaviour
    {
        private static Manager m_instance;

        public event OnNotificationClickedHandler OnNotificationClicked;

        public event OnNotificationsReceivedHandler OnNotificationsReceived;

        public event OnSendRegistrationIdHandler OnSendRegistrationId;

        protected Manager()
        {
        }

        protected void _OnNotificationClicked(ReceivedNotification notification)
        {
            this.OnNotificationClicked(notification);
        }

        protected void _OnNotificationsReceived(IList<ReceivedNotification> receivedNotifications)
        {
            this.OnNotificationsReceived(receivedNotifications);
        }

        protected void _OnSendRegistrationId(string providerName, string registrationId)
        {
            this.OnSendRegistrationId(providerName, registrationId);
        }

        public abstract void CancelAllNotifications();
        public abstract void CancelNotification(int id);
        public abstract int GetBadge();
        public abstract void HideAllNotifications();
        public abstract void HideNotification(int id);
        public abstract bool Initialize(bool willHandleReceivedNotifications, [Optional, DefaultParameterValue(0)] int startId, [Optional, DefaultParameterValue(false)] bool incrementalId);
        private static void InstanceRequired()
        {
            if (m_instance == null)
            {
                GameObject target = new GameObject("UTNotificationsManager");
                m_instance = target.AddComponent<ManagerImpl>();
                UnityEngine.Object.DontDestroyOnLoad(target);
            }
        }

        public abstract bool NotificationsEnabled();
        protected void NotSupported([Optional, DefaultParameterValue(null)] string feature)
        {
            if (feature == null)
            {
                Debug.LogWarning("UTNotifications: not supported on this platform");
            }
            else
            {
                Debug.LogWarning("UTNotifications: " + feature + " feature is not supported on this platform");
            }
        }

        protected virtual void OnDestroy()
        {
            m_instance = null;
        }

        protected bool OnNotificationClickedHasSubscribers()
        {
            return (this.OnNotificationClicked != null);
        }

        protected bool OnNotificationsReceivedHasSubscribers()
        {
            return (this.OnNotificationsReceived != null);
        }

        protected bool OnSendRegistrationIdHasSubscribers()
        {
            return (this.OnSendRegistrationId != null);
        }

        public abstract void PostLocalNotification(string title, string text, int id, [Optional, DefaultParameterValue(null)] IDictionary<string, string> userData, [Optional, DefaultParameterValue(null)] string notificationProfile);
        public void ScheduleNotification(DateTime triggerDateTime, string title, string text, int id, [Optional, DefaultParameterValue(null)] IDictionary<string, string> userData, [Optional, DefaultParameterValue(null)] string notificationProfile)
        {
            this.ScheduleNotification(TimeUtils.ToSecondsFromNow(triggerDateTime), title, text, id, userData, notificationProfile);
        }

        public abstract void ScheduleNotification(int triggerInSeconds, string title, string text, int id, [Optional, DefaultParameterValue(null)] IDictionary<string, string> userData, [Optional, DefaultParameterValue(null)] string notificationProfile);
        public void ScheduleNotificationRepeating(DateTime firstTriggerDateTime, int intervalSeconds, string title, string text, int id, [Optional, DefaultParameterValue(null)] IDictionary<string, string> userData, [Optional, DefaultParameterValue(null)] string notificationProfile)
        {
            this.ScheduleNotificationRepeating(TimeUtils.ToSecondsFromNow(firstTriggerDateTime), intervalSeconds, title, text, id, userData, notificationProfile);
        }

        public abstract void ScheduleNotificationRepeating(int firstTriggerInSeconds, int intervalSeconds, string title, string text, int id, [Optional, DefaultParameterValue(null)] IDictionary<string, string> userData, [Optional, DefaultParameterValue(null)] string notificationProfile);
        public abstract void SetBadge(int bandgeNumber);
        public abstract void SetNotificationsEnabled(bool enabled);

        public static Manager Instance
        {
            get
            {
                InstanceRequired();
                return m_instance;
            }
        }

        public delegate void OnNotificationClickedHandler(ReceivedNotification notification);

        public delegate void OnNotificationsReceivedHandler(IList<ReceivedNotification> receivedNotifications);

        public delegate void OnSendRegistrationIdHandler(string providerName, string registrationId);
    }
}

