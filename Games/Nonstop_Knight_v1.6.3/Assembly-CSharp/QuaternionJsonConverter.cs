using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionJsonConverter : JsonConverter
{
    public override bool CanConvert(System.Type t)
    {
        return (t == typeof(Quaternion));
    }

    public override object ReadJson(System.Type type, Dictionary<string, object> value)
    {
        return new Quaternion(base.CastFloat(value["x"]), base.CastFloat(value["y"]), base.CastFloat(value["z"]), base.CastFloat(value["w"]));
    }

    public override Dictionary<string, object> WriteJson(System.Type type, object value)
    {
        Quaternion quaternion = (Quaternion) value;
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary.Add("x", quaternion.x);
        dictionary.Add("y", quaternion.y);
        dictionary.Add("z", quaternion.z);
        dictionary.Add("w", quaternion.w);
        return dictionary;
    }
}

