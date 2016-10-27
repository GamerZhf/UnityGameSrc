namespace UTNotifications
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Settings : ScriptableObject
    {
        [SerializeField]
        private bool m_android4CompatibilityMode = true;
        [SerializeField]
        private NotificationsGroupingMode m_androidNotificationsGrouping;
        [SerializeField]
        private bool m_androidRestoreScheduledNotificationsAfterReboot = true;
        [SerializeField]
        private bool m_androidShowLatestNotificationOnly;
        [SerializeField]
        private ShowNotifications m_androidShowNotificationsMode;
        private const string m_assetName = "UTNotificationsSettings";
        [SerializeField]
        private string m_googlePlaySenderID = string.Empty;
        private static Settings m_instance;
        [SerializeField]
        private TokenEncoding m_iOSTokenEncoding = TokenEncoding.HEX;
        [SerializeField]
        private List<NotificationProfile> m_notificationProfiles = new List<NotificationProfile>();
        [SerializeField]
        private bool m_pushNotificationsEnabledAmazon;
        [SerializeField]
        private bool m_pushNotificationsEnabledGooglePlay;
        [SerializeField]
        private bool m_pushNotificationsEnabledIOS;
        [SerializeField]
        private bool m_pushNotificationsEnabledWindows;
        private const string m_settingsMenuItem = "Edit/Project Settings/UTNotifications";
        [SerializeField]
        private bool m_windowsDontShowWhenRunning = true;

        public bool Android4CompatibilityMode
        {
            get
            {
                return this.m_android4CompatibilityMode;
            }
        }

        public NotificationsGroupingMode AndroidNotificationsGrouping
        {
            get
            {
                return this.m_androidNotificationsGrouping;
            }
            set
            {
                if (this.m_androidNotificationsGrouping != value)
                {
                    this.m_androidNotificationsGrouping = value;
                }
            }
        }

        public bool AndroidRestoreScheduledNotificationsAfterReboot
        {
            get
            {
                return this.m_androidRestoreScheduledNotificationsAfterReboot;
            }
        }

        public bool AndroidShowLatestNotificationOnly
        {
            get
            {
                return this.m_androidShowLatestNotificationOnly;
            }
        }

        public ShowNotifications AndroidShowNotificationsMode
        {
            get
            {
                return this.m_androidShowNotificationsMode;
            }
            set
            {
                if (this.m_androidShowNotificationsMode != value)
                {
                    this.m_androidShowNotificationsMode = value;
                }
            }
        }

        public string GooglePlaySenderID
        {
            get
            {
                return this.m_googlePlaySenderID;
            }
        }

        public static Settings Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = Resources.Load("UTNotificationsSettings") as Settings;
                    if (m_instance == null)
                    {
                        m_instance = ScriptableObject.CreateInstance<Settings>();
                    }
                }
                return m_instance;
            }
        }

        public TokenEncoding IOSTokenEncoding
        {
            get
            {
                return this.m_iOSTokenEncoding;
            }
            set
            {
                if (this.m_iOSTokenEncoding != value)
                {
                    this.m_iOSTokenEncoding = value;
                }
            }
        }

        public List<NotificationProfile> NotificationProfiles
        {
            get
            {
                return this.m_notificationProfiles;
            }
        }

        public bool PushNotificationsEnabledAmazon
        {
            get
            {
                return this.m_pushNotificationsEnabledAmazon;
            }
            set
            {
                if (this.m_pushNotificationsEnabledAmazon != value)
                {
                    this.m_pushNotificationsEnabledAmazon = value;
                }
            }
        }

        public bool PushNotificationsEnabledGooglePlay
        {
            get
            {
                return this.m_pushNotificationsEnabledGooglePlay;
            }
            set
            {
                if (this.m_pushNotificationsEnabledGooglePlay != value)
                {
                    this.m_pushNotificationsEnabledGooglePlay = value;
                }
            }
        }

        public bool PushNotificationsEnabledIOS
        {
            get
            {
                return this.m_pushNotificationsEnabledIOS;
            }
            set
            {
                if (this.m_pushNotificationsEnabledIOS != value)
                {
                    this.m_pushNotificationsEnabledIOS = value;
                }
            }
        }

        public bool PushNotificationsEnabledWindows
        {
            get
            {
                return this.m_pushNotificationsEnabledWindows;
            }
            set
            {
                if (this.m_pushNotificationsEnabledWindows != value)
                {
                    this.m_pushNotificationsEnabledWindows = value;
                }
            }
        }

        public bool WindowsDontShowWhenRunning
        {
            get
            {
                return this.m_windowsDontShowWhenRunning;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct NotificationProfile
        {
            public string profileName;
            public string iosSound;
            public string androidIcon;
            public string androidLargeIcon;
            public string androidIcon5Plus;
            public string androidSound;
        }

        public enum NotificationsGroupingMode
        {
            NONE,
            BY_NOTIFICATION_PROFILES,
            FROM_USER_DATA,
            ALL_IN_A_SINGLE_GROUP
        }

        public enum ShowNotifications
        {
            WHEN_CLOSED_OR_IN_BACKGROUND,
            WHEN_CLOSED,
            ALWAYS
        }

        public enum TokenEncoding
        {
            Base64,
            HEX
        }
    }
}

