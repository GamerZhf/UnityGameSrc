namespace Pathfinding.Serialization
{
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MatrixConverter : JsonConverter
    {
        private readonly float[] values = new float[0x10];

        public override bool CanConvert(System.Type type)
        {
            return object.Equals(type, typeof(Matrix4x4));
        }

        public override object ReadJson(System.Type objectType, Dictionary<string, object> values)
        {
            Matrix4x4 matrixx = new Matrix4x4();
            Array array = (Array) values["values"];
            if (array.Length != 0x10)
            {
                Debug.LogError("Number of elements in matrix was not 16 (got " + array.Length + ")");
                return matrixx;
            }
            for (int i = 0; i < 0x10; i++)
            {
                int[] indices = new int[] { i };
                matrixx[i] = Convert.ToSingle(array.GetValue(indices));
            }
            return matrixx;
        }

        public override Dictionary<string, object> WriteJson(System.Type type, object value)
        {
            Matrix4x4 matrixx = (Matrix4x4) value;
            for (int i = 0; i < this.values.Length; i++)
            {
                this.values[i] = matrixx[i];
            }
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("values", this.values);
            return dictionary;
        }
    }
}

