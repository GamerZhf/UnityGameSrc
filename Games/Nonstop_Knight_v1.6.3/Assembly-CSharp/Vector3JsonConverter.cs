using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Vector3JsonConverter : JsonConverter
{
    public override bool CanConvert(System.Type t)
    {
        return (t == typeof(Vector3));
    }

    public override object ReadJson(System.Type type, Dictionary<string, object> value)
    {
        return new Vector3(base.CastFloat(value["x"]), base.CastFloat(value["y"]), base.CastFloat(value["z"]));
    }

    public override Dictionary<string, object> WriteJson(System.Type type, object value)
    {
        Vector3 vector = (Vector3) value;
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary.Add("x", vector.x);
        dictionary.Add("y", vector.y);
        dictionary.Add("z", vector.z);
        return dictionary;
    }
}

