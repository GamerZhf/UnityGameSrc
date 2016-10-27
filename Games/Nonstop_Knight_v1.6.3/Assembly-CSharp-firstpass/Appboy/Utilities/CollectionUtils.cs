namespace Appboy.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CollectionUtils
    {
        public static string DictionaryToString(Dictionary<string, string> dictionary)
        {
            if ((dictionary == null) || (dictionary.Count <= 0))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                builder.Append(string.Format("{0}={1},", pair.Key, pair.Value));
            }
            if (builder.Length > 1)
            {
                builder.Length--;
            }
            builder.Append("}");
            return builder.ToString();
        }

        public static string ListToString<T>(List<T> list)
        {
            if ((list == null) || (list.Count <= 0))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (T local in list)
            {
                builder.Append(string.Format("{0},", local.ToString()));
            }
            if (builder.Length > 1)
            {
                builder.Length--;
            }
            builder.Append("]");
            return builder.ToString();
        }
    }
}

