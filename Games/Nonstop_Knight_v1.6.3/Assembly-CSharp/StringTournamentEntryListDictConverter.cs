using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;

public class StringTournamentEntryListDictConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Dictionary<string, List<TournamentEntry>>));
    }

    public override object ReadJson(Type type, Dictionary<string, object> value)
    {
        Dictionary<string, List<TournamentEntry>> dictionary = new Dictionary<string, List<TournamentEntry>>();
        foreach (KeyValuePair<string, object> pair in value)
        {
            dictionary.Add(pair.Key, JsonReader.CoerceType<List<TournamentEntry>>(pair.Value));
        }
        return dictionary;
    }

    public override Dictionary<string, object> WriteJson(Type type, object value)
    {
        throw new NotSupportedException();
    }
}

