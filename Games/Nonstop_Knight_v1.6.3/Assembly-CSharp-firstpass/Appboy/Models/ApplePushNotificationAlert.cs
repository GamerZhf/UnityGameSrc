namespace Appboy.Models
{
    using Appboy.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ApplePushNotificationAlert
    {
        [CompilerGenerated]
        private string <ActionLocationKey>k__BackingField;
        [CompilerGenerated]
        private string <Body>k__BackingField;
        [CompilerGenerated]
        private string <LaunchImage>k__BackingField;
        [CompilerGenerated]
        private IList<string> <LocationArguments>k__BackingField;
        [CompilerGenerated]
        private string <LocationKey>k__BackingField;

        public ApplePushNotificationAlert(JSONClass json)
        {
            this.LocationArguments = new List<string>();
            IEnumerator enumerator = json["loc-args"].AsArray.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    this.LocationArguments.Add(current.ToString());
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
            this.Body = (string) json["body"];
            this.ActionLocationKey = (string) json["action-loc-key"];
            this.LocationKey = (string) json["loc-key"];
            this.LaunchImage = (string) json["launch-image"];
        }

        public override string ToString()
        {
            string str = "[ ";
            IEnumerator<string> enumerator = this.LocationArguments.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    if (str.Length > 2)
                    {
                        str = str + ", ";
                    }
                    str = str + current.ToString();
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            str = str + " ]";
            object[] args = new object[] { str, this.Body, this.ActionLocationKey, this.LocationKey, this.LaunchImage };
            return string.Format("PushNotificationAlert[LocationArguments={0}, Body={1}, ActionLocationKey={2}, LocationKey={3}, LaunchImage={4}]", args);
        }

        public string ActionLocationKey
        {
            [CompilerGenerated]
            get
            {
                return this.<ActionLocationKey>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ActionLocationKey>k__BackingField = value;
            }
        }

        public string Body
        {
            [CompilerGenerated]
            get
            {
                return this.<Body>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Body>k__BackingField = value;
            }
        }

        public string LaunchImage
        {
            [CompilerGenerated]
            get
            {
                return this.<LaunchImage>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LaunchImage>k__BackingField = value;
            }
        }

        public IList<string> LocationArguments
        {
            [CompilerGenerated]
            get
            {
                return this.<LocationArguments>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LocationArguments>k__BackingField = value;
            }
        }

        public string LocationKey
        {
            [CompilerGenerated]
            get
            {
                return this.<LocationKey>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LocationKey>k__BackingField = value;
            }
        }
    }
}

