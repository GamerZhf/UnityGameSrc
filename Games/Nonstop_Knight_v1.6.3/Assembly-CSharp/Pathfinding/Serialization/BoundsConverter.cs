namespace Pathfinding.Serialization
{
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class BoundsConverter : JsonConverter
    {
        public override bool CanConvert(System.Type type)
        {
            return object.Equals(type, typeof(Bounds));
        }

        public override object ReadJson(System.Type objectType, Dictionary<string, object> values)
        {
            Bounds bounds = new Bounds();
            bounds.center = new Vector3(base.CastFloat(values["cx"]), base.CastFloat(values["cy"]), base.CastFloat(values["cz"]));
            bounds.extents = new Vector3(base.CastFloat(values["ex"]), base.CastFloat(values["ey"]), base.CastFloat(values["ez"]));
            return bounds;
        }

        public override Dictionary<string, object> WriteJson(System.Type type, object value)
        {
            Bounds bounds = (Bounds) value;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("cx", bounds.center.x);
            dictionary.Add("cy", bounds.center.y);
            dictionary.Add("cz", bounds.center.z);
            dictionary.Add("ex", bounds.extents.x);
            dictionary.Add("ey", bounds.extents.y);
            dictionary.Add("ez", bounds.extents.z);
            return dictionary;
        }
    }
}

