namespace App
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using UnityEngine;

    public static class DebugUtil
    {
        private static Stopwatch GlobalStopwatch = new Stopwatch();

        public static string DictionaryToOneLineString<K, V>(Dictionary<K, V> dict)
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            foreach (KeyValuePair<K, V> pair in dict)
            {
                builder.Append("{");
                builder.Append(pair.Key);
                builder.Append(":");
                builder.Append(pair.Value);
                builder.Append("}");
                num++;
                if (num < dict.Count)
                {
                    builder.Append(",");
                }
            }
            return builder.ToString();
        }

        [Conditional("DEVELOPMENT")]
        public static void DrawBounds(Transform transform, Bounds bounds, Color color, Space space)
        {
            Vector3 center = bounds.center;
            Vector3 extents = bounds.extents;
            Vector3 position = new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z);
            Vector3 vector2 = new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z);
            Vector3 vector3 = new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z);
            Vector3 vector4 = new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z);
            Vector3 vector5 = new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z);
            Vector3 vector6 = new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z);
            Vector3 vector7 = new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z);
            Vector3 vector8 = new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z);
            if (space == Space.Self)
            {
                position = transform.TransformPoint(position);
                vector2 = transform.TransformPoint(vector2);
                vector3 = transform.TransformPoint(vector3);
                vector4 = transform.TransformPoint(vector4);
                vector5 = transform.TransformPoint(vector5);
                vector6 = transform.TransformPoint(vector6);
                vector7 = transform.TransformPoint(vector7);
                vector8 = transform.TransformPoint(vector8);
            }
            UnityEngine.Debug.DrawLine(position, vector2, color);
            UnityEngine.Debug.DrawLine(vector2, vector4, color);
            UnityEngine.Debug.DrawLine(vector4, vector3, color);
            UnityEngine.Debug.DrawLine(vector3, position, color);
            UnityEngine.Debug.DrawLine(vector5, vector6, color);
            UnityEngine.Debug.DrawLine(vector6, vector8, color);
            UnityEngine.Debug.DrawLine(vector8, vector7, color);
            UnityEngine.Debug.DrawLine(vector7, vector5, color);
            UnityEngine.Debug.DrawLine(position, vector5, color);
            UnityEngine.Debug.DrawLine(vector2, vector6, color);
            UnityEngine.Debug.DrawLine(vector4, vector8, color);
            UnityEngine.Debug.DrawLine(vector3, vector7, color);
        }

        [Conditional("DEVELOPMENT")]
        public static void LapGlobalStopwatch(string msg)
        {
            UnityEngine.Debug.Log(string.Concat(new object[] { "Ms elapsed (", msg, "): ", GlobalStopwatch.ElapsedMilliseconds }));
            GlobalStopwatch.Reset();
            GlobalStopwatch.Start();
        }

        [Conditional("DEVELOPMENT")]
        public static void LapStopwatch(Stopwatch watch, string msg)
        {
            watch.Reset();
            watch.Start();
        }

        [Conditional("DEVELOPMENT")]
        public static void Log(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }

        [Conditional("DEVELOPMENT")]
        public static void PrintList<T>(List<T> list)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                builder.Append(list[i] + "\n");
            }
            UnityEngine.Debug.Log(builder.ToString());
        }

        [Conditional("DEVELOPMENT")]
        public static void StartGlobalStopwatch()
        {
            GlobalStopwatch.Reset();
            GlobalStopwatch.Start();
        }

        public static Stopwatch StartStopwatch()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }
    }
}

