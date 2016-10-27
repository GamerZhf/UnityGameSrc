namespace UTNotifications
{
    using System;
    using System.Collections.Generic;

    public class ReceivedNotification
    {
        public readonly int id;
        public readonly string notificationProfile;
        public readonly string text;
        public readonly string title;
        public readonly IDictionary<string, string> userData;

        public ReceivedNotification(string title, string text, int id, IDictionary<string, string> userData, string notificationProfile)
        {
            this.title = title;
            this.text = text;
            this.id = id;
            this.userData = userData;
            this.notificationProfile = notificationProfile;
        }
    }
}

