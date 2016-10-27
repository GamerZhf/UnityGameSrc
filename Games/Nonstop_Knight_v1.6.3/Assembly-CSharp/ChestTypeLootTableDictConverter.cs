using GameLogic;
using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;

public class ChestTypeLootTableDictConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Dictionary<ChestType, LootTable>));
    }

    public override object ReadJson(Type type, Dictionary<string, object> value)
    {
        Dictionary<ChestType, LootTable> dictionary = new Dictionary<ChestType, LootTable>();
        foreach (KeyValuePair<string, object> pair in value)
        {
            ChestType key = (ChestType) ((int) Enum.Parse(typeof(ChestType), pair.Key));
            dictionary.Add(key, JsonReader.CoerceType<LootTable>(pair.Value));
        }
        return dictionary;
    }

    public override Dictionary<string, object> WriteJson(Type type, object value)
    {
        throw new NotSupportedException();
    }
}

