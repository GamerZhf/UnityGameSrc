namespace Area730.Notifications
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class NotificationInstance
    {
        public bool alertOnce;
        public bool autoCancel;
        public string body;
        public Color color;
        public bool defaultSound;
        public bool defaultVibrate;
        public int delayHours;
        public int delayMinutes;
        public int delaySeconds;
        public string group;
        public int groupId;
        public bool hasColor;
        public int id;
        public int intervalHours;
        public int intervalMinutes;
        public int intervalSeconds;
        public bool isRepeating;
        public Texture2D largeIcon;
        public string name;
        public int number;
        public Texture2D smallIcon;
        public string sortKey;
        public AudioClip soundFile;
        public string ticker;
        public string title;
        public List<long> vibroPattern = new List<long>();

        public void Print()
        {
            Debug.Log((((((((((((((((((((("Notification: " + "title: " + this.title) + ", body: " + this.body) + ", ticker: " + this.ticker) + ", autoCancel: " + this.autoCancel) + ", isRepeating: " + this.isRepeating) + ", intervalHours: " + this.intervalHours) + ", intervalMinutes: " + this.intervalMinutes) + ", intervalSeconds: " + this.intervalSeconds) + ", alertOnce: " + this.alertOnce) + ", number: " + this.number) + ", delayHours: " + this.delayHours) + ", delayMinutes: " + this.delayMinutes) + ", delaySeconds: " + this.delaySeconds) + ", defaultSound: " + this.defaultSound) + ", defaultVibrate: " + this.defaultVibrate) + ", group: " + this.group) + ", sortKey: " + this.sortKey) + ", hasColor: " + this.hasColor) + ", color: " + ColorUtils.ToHtmlStringRGB(this.color)) + ", body: " + this.body) + ", groupId: " + this.groupId);
        }
    }
}

