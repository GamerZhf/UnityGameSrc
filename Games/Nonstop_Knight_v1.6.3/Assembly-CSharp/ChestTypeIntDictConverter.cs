using GameLogic;
using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;

public class ChestTypeIntDictConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Dictionary<ChestType, int>));
    }

    public override object ReadJson(Type type, Dictionary<string, object> value)
    {
        Dictionary<ChestType, int> dictionary = new Dictionary<ChestType, int>();
        foreach (KeyValuePair<string, object> pair in value)
        {
            ChestType key = (ChestType) ((int) Enum.Parse(typeof(ChestType), pair.Key));
            dictionary.Add(key, int.Parse(pair.Value.ToString()));
        }
        return dictionary;
    }

    public override Dictionary<string, object> WriteJson(Type type, object value)
    {
        throw new NotSupportedException();
    }
}

