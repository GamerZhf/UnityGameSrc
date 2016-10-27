using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;

public class IntDoubleDictKeyConverter : JsonConverter
{
    public override bool CanConvert(Type t)
    {
        return (t == typeof(Dictionary<int, double>));
    }

    public override object ReadJson(Type type, Dictionary<string, object> value)
    {
        Dictionary<int, double> dictionary = new Dictionary<int, double>();
        foreach (KeyValuePair<string, object> pair in value)
        {
            int key = int.Parse(pair.Key);
            dictionary.Add(key, double.Parse(pair.Value.ToString()));
        }
        return dictionary;
    }

    public override Dictionary<string, object> WriteJson(Type type, object value)
    {
        throw new NotSupportedException();
    }
}

