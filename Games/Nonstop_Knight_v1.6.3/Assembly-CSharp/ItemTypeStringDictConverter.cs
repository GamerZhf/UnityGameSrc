using GameLogic;
using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;

public class ItemTypeStringDictConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Dictionary<ItemType, string>));
    }

    public override object ReadJson(Type type, Dictionary<string, object> value)
    {
        Dictionary<ItemType, string> dictionary = new Dictionary<ItemType, string>();
        foreach (KeyValuePair<string, object> pair in value)
        {
            ItemType key = (ItemType) ((int) Enum.Parse(typeof(ItemType), pair.Key));
            dictionary.Add(key, pair.Value.ToString());
        }
        return dictionary;
    }

    public override Dictionary<string, object> WriteJson(Type type, object value)
    {
        throw new NotSupportedException();
    }
}

