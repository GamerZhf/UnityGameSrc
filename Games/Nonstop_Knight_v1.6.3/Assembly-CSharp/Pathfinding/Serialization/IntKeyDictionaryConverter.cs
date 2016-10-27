namespace Pathfinding.Serialization
{
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;

    public class IntKeyDictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type type)
        {
            return (object.Equals(type, typeof(Dictionary<int, int>)) || object.Equals(type, typeof(SortedDictionary<int, int>)));
        }

        public override object ReadJson(Type type, Dictionary<string, object> values)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            foreach (KeyValuePair<string, object> pair in values)
            {
                int key = Convert.ToInt32(pair.Key);
                dictionary.Add(key, Convert.ToInt32(pair.Value));
            }
            return dictionary;
        }

        public override Dictionary<string, object> WriteJson(Type type, object value)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            Dictionary<int, int> dictionary2 = (Dictionary<int, int>) value;
            foreach (KeyValuePair<int, int> pair in dictionary2)
            {
                string key = pair.Key.ToString();
                dictionary.Add(key, pair.Value);
            }
            return dictionary;
        }
    }
}

