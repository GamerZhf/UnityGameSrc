using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Vector2JsonConverter : JsonConverter
{
    public override bool CanConvert(System.Type t)
    {
        return (t == typeof(Vector2));
    }

    public override object ReadJson(System.Type type, Dictionary<string, object> value)
    {
        return new Vector2(base.CastFloat(value["x"]), base.CastFloat(value["y"]));
    }

    public override Dictionary<string, object> WriteJson(System.Type type, object value)
    {
        Vector2 vector = (Vector2) value;
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary.Add("x", vector.x);
        dictionary.Add("y", vector.y);
        return dictionary;
    }
}

