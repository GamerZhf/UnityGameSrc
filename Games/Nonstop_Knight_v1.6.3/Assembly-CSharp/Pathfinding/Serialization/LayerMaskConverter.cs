namespace Pathfinding.Serialization
{
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class LayerMaskConverter : JsonConverter
    {
        public override bool CanConvert(System.Type type)
        {
            return object.Equals(type, typeof(LayerMask));
        }

        public override object ReadJson(System.Type type, Dictionary<string, object> values)
        {
            return (LayerMask) ((int) values["value"]);
        }

        public override Dictionary<string, object> WriteJson(System.Type type, object value)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            LayerMask mask = (LayerMask) value;
            dictionary.Add("value", mask.value);
            return dictionary;
        }
    }
}

