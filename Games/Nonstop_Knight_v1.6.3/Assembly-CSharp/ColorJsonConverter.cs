using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorJsonConverter : JsonConverter
{
    public override bool CanConvert(System.Type t)
    {
        return (t == typeof(Color));
    }

    public override object ReadJson(System.Type type, Dictionary<string, object> value)
    {
        return new Color(base.CastFloat(value["r"]), base.CastFloat(value["g"]), base.CastFloat(value["b"]), base.CastFloat(value["a"]));
    }

    public override Dictionary<string, object> WriteJson(System.Type type, object value)
    {
        Color color = (Color) value;
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary.Add("r", color.r);
        dictionary.Add("g", color.g);
        dictionary.Add("b", color.b);
        dictionary.Add("a", color.a);
        return dictionary;
    }
}

