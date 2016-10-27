namespace Pathfinding.Serialization
{
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class VectorConverter : JsonConverter
    {
        public override bool CanConvert(System.Type type)
        {
            return ((object.Equals(type, typeof(Vector2)) || object.Equals(type, typeof(Vector3))) || object.Equals(type, typeof(Vector4)));
        }

        public override object ReadJson(System.Type type, Dictionary<string, object> values)
        {
            if (object.Equals(type, typeof(Vector2)))
            {
                return new Vector2(base.CastFloat(values["x"]), base.CastFloat(values["y"]));
            }
            if (object.Equals(type, typeof(Vector3)))
            {
                return new Vector3(base.CastFloat(values["x"]), base.CastFloat(values["y"]), base.CastFloat(values["z"]));
            }
            if (!object.Equals(type, typeof(Vector4)))
            {
                throw new NotImplementedException("Can only read Vector2,3,4. Not objects of type " + type);
            }
            return new Vector4(base.CastFloat(values["x"]), base.CastFloat(values["y"]), base.CastFloat(values["z"]), base.CastFloat(values["w"]));
        }

        public override Dictionary<string, object> WriteJson(System.Type type, object value)
        {
            Dictionary<string, object> dictionary;
            if (object.Equals(type, typeof(Vector2)))
            {
                Vector2 vector = (Vector2) value;
                dictionary = new Dictionary<string, object>();
                dictionary.Add("x", vector.x);
                dictionary.Add("y", vector.y);
                return dictionary;
            }
            if (object.Equals(type, typeof(Vector3)))
            {
                Vector3 vector2 = (Vector3) value;
                dictionary = new Dictionary<string, object>();
                dictionary.Add("x", vector2.x);
                dictionary.Add("y", vector2.y);
                dictionary.Add("z", vector2.z);
                return dictionary;
            }
            if (!object.Equals(type, typeof(Vector4)))
            {
                throw new NotImplementedException("Can only write Vector2,3,4. Not objects of type " + type);
            }
            Vector4 vector3 = (Vector4) value;
            dictionary = new Dictionary<string, object>();
            dictionary.Add("x", vector3.x);
            dictionary.Add("y", vector3.y);
            dictionary.Add("z", vector3.z);
            dictionary.Add("w", vector3.w);
            return dictionary;
        }
    }
}

