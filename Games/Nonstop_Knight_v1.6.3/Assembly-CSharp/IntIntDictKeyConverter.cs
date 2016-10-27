using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;

public class IntIntDictKeyConverter : JsonConverter
{
    public override bool CanConvert(Type t)
    {
        return (t == typeof(Dictionary<int, int>));
    }

    public override object ReadJson(Type type, Dictionary<string, object> value)
    {
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        foreach (KeyValuePair<string, object> pair in value)
        {
            int key = int.Parse(pair.Key);
            dictionary.Add(key, int.Parse(pair.Value.ToString()));
        }
        return dictionary;
    }

    public override Dictionary<string, object> WriteJson(Type type, object value)
    {
        throw new NotSupportedException();
    }
}

