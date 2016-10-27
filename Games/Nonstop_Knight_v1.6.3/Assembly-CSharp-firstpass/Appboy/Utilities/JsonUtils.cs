namespace Appboy.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class JsonUtils
    {
        public static Dictionary<string, string> JSONClassToDictionary(JSONClass json)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (json != null)
            {
                IEnumerator enumerator = json.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, JSONNode> current = (KeyValuePair<string, JSONNode>) enumerator.Current;
                    dictionary.Add(current.Key, current.Value);
                }
            }
            return dictionary;
        }
    }
}

