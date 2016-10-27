namespace Appboy.Models
{
    using Appboy.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ApplePushNotification
    {
        [CompilerGenerated]
        private ApplePushNotificationAlert <Alert>k__BackingField;
        [CompilerGenerated]
        private int <Badge>k__BackingField;
        [CompilerGenerated]
        private int <ContentAvailable>k__BackingField;
        [CompilerGenerated]
        private IDictionary<string, object> <Extra>k__BackingField;
        [CompilerGenerated]
        private string <Sound>k__BackingField;

        public ApplePushNotification(JSONClass json)
        {
            this.Alert = new ApplePushNotificationAlert(json["alert"].AsObject);
            this.Badge = json["badge"].AsInt;
            this.Sound = (string) json["sound"];
            this.ContentAvailable = json["content-available"].AsInt;
            this.Extra = new Dictionary<string, object>();
            JSONNode node = json["extra"];
            if (node.GetType() == typeof(JSONClass))
            {
                IEnumerator enumerator = node.AsObject.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<string, JSONNode> current = (KeyValuePair<string, JSONNode>) enumerator.Current;
                        this.Extra.Add(current.Key, this.getJSONNodeValue(current.Value));
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
                Debug.Log("Value of key Extra isn't a dictionary. Stop parsing the Extra dictionary");
            }
        }

        private object getJSONNodeValue(JSONNode node)
        {
            int result = 0;
            if (int.TryParse(node.Value, out result))
            {
                return result;
            }
            float num2 = 0f;
            if (float.TryParse(node.Value, out num2))
            {
                return num2;
            }
            double num3 = 0.0;
            if (double.TryParse(node.Value, out num3))
            {
                return num3;
            }
            bool flag = false;
            if (bool.TryParse(node.Value, out flag))
            {
                return flag;
            }
            if (node.GetType() == typeof(JSONArray))
            {
                IList<object> list = new List<object>();
                IEnumerator enumerator = node.AsArray.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        JSONNode current = (JSONNode) enumerator.Current;
                        list.Add(this.getJSONNodeValue(current));
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
                return list;
            }
            if (node.GetType() != typeof(JSONClass))
            {
                return node.ToString();
            }
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            IEnumerator enumerator2 = node.AsObject.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    KeyValuePair<string, JSONNode> pair = (KeyValuePair<string, JSONNode>) enumerator2.Current;
                    dictionary.Add(pair.Key, this.getJSONNodeValue(pair.Value));
                }
            }
            finally
            {
                IDisposable disposable2 = enumerator2 as IDisposable;
                if (disposable2 == null)
                {
                }
                disposable2.Dispose();
            }
            return dictionary;
        }

        public override string ToString()
        {
            string str = "{";
            IEnumerator<KeyValuePair<string, object>> enumerator = this.Extra.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, object> current = enumerator.Current;
                    if (str.Length > 2)
                    {
                        str = str + ", ";
                    }
                    string str2 = str;
                    string[] textArray1 = new string[] { str2, "\"", current.Key, "\":", current.Value.ToString() };
                    str = string.Concat(textArray1);
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            str = str + "}";
            object[] args = new object[] { this.Alert.ToString(), this.Badge, this.Sound, this.ContentAvailable, str };
            return string.Format("PushNotification[Alert={0}, Badge={1}, Sound={2}, ContentAvailable={3}, Extra={4}]", args);
        }

        public ApplePushNotificationAlert Alert
        {
            [CompilerGenerated]
            get
            {
                return this.<Alert>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Alert>k__BackingField = value;
            }
        }

        public int Badge
        {
            [CompilerGenerated]
            get
            {
                return this.<Badge>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Badge>k__BackingField = value;
            }
        }

        public int ContentAvailable
        {
            [CompilerGenerated]
            get
            {
                return this.<ContentAvailable>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ContentAvailable>k__BackingField = value;
            }
        }

        public IDictionary<string, object> Extra
        {
            [CompilerGenerated]
            get
            {
                return this.<Extra>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Extra>k__BackingField = value;
            }
        }

        public string Sound
        {
            [CompilerGenerated]
            get
            {
                return this.<Sound>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Sound>k__BackingField = value;
            }
        }
    }
}

