namespace Appboy.Models
{
    using Appboy.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PushNotification
    {
        [CompilerGenerated]
        private int <AndroidNotificationId>k__BackingField;
        [CompilerGenerated]
        private string <CollapseKey>k__BackingField;
        [CompilerGenerated]
        private string <Content>k__BackingField;
        [CompilerGenerated]
        private Dictionary<string, string> <Extras>k__BackingField;
        [CompilerGenerated]
        private string <Title>k__BackingField;

        public PushNotification(string message)
        {
            JSONClass class2;
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", "message");
            }
            try
            {
                class2 = (JSONClass) JSON.Parse(message);
            }
            catch
            {
                throw new ArgumentException("Unable to parse json.");
            }
            if (class2["title"] != null)
            {
                this.Title = (string) class2["title"];
            }
            if (class2["content"] != null)
            {
                this.Content = (string) class2["content"];
            }
            if (class2["extras"] != null)
            {
                this.Extras = JsonUtils.JSONClassToDictionary(class2["extras"].AsObject);
            }
            else
            {
                this.Extras = new Dictionary<string, string>();
            }
            this.CollapseKey = (string) class2["collapse_key"];
            this.AndroidNotificationId = class2["notification_id"].AsInt;
        }

        public override string ToString()
        {
            object[] args = new object[] { this.Title, this.Content, CollectionUtils.DictionaryToString(this.Extras), this.CollapseKey, this.AndroidNotificationId };
            return string.Format("PushNotification[Title={0}, Content={1}, Extras={2}, CollapseKey={3}, AndroidNotificationId={4}]", args);
        }

        public int AndroidNotificationId
        {
            [CompilerGenerated]
            get
            {
                return this.<AndroidNotificationId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AndroidNotificationId>k__BackingField = value;
            }
        }

        public string CollapseKey
        {
            [CompilerGenerated]
            get
            {
                return this.<CollapseKey>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CollapseKey>k__BackingField = value;
            }
        }

        public string Content
        {
            [CompilerGenerated]
            get
            {
                return this.<Content>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Content>k__BackingField = value;
            }
        }

        public Dictionary<string, string> Extras
        {
            [CompilerGenerated]
            get
            {
                return this.<Extras>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Extras>k__BackingField = value;
            }
        }

        public string Title
        {
            [CompilerGenerated]
            get
            {
                return this.<Title>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Title>k__BackingField = value;
            }
        }
    }
}

