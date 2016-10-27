namespace OrbCreationExtensions
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Extension]
    public static class Vector2Extensions
    {
        [Extension]
        public static Vector2 Abs(Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        [Extension]
        public static bool DiffBetween(Vector2 v1, Vector2 v2, float from, float to)
        {
            return (((((v1.x - v2.x) < to) && ((v1.x - v2.x) > from)) && ((v1.y - v2.y) < to)) && ((v1.y - v2.y) > from));
        }

        [Extension]
        public static float InProduct(Vector2 v1, Vector2 v2)
        {
            return ((v1.x * v2.x) + (v1.y * v2.y));
        }

        [Extension]
        public static bool IsAllSmaller(Vector2 v1, Vector2 v2)
        {
            return ((v1.x < v2.x) && (v1.y < v2.y));
        }

        [Extension]
        public static bool IsBarycentricInTriangle(Vector2 v)
        {
            return (((v.x >= 0f) && (v.y >= 0f)) && ((v.x + v.y) <= 1.01f));
        }

        [Extension]
        public static bool IsDiffSmallEnough(Vector2 v1, Vector2 v2, float maxDiff)
        {
            return (((((v1.x - v2.x) < maxDiff) && ((v2.x - v1.x) < maxDiff)) && ((v1.y - v2.y) < maxDiff)) && ((v2.y - v1.y) < maxDiff));
        }

        [Extension]
        public static bool IsEqual(Vector2 v1, Vector2 v2)
        {
            return ((v1.x == v2.x) && (v1.y == v2.y));
        }

        [Extension]
        public static string MakeString(Vector2 v)
        {
            object[] objArray1 = new object[] { "<", v.x, ",", v.y, ">" };
            return string.Concat(objArray1);
        }

        [Extension]
        public static string MakeString(Vector2 v, int decimals)
        {
            if (decimals <= 0)
            {
                object[] objArray1 = new object[] { "<", Mathf.RoundToInt(v.x), ",", Mathf.RoundToInt(v.y), ">" };
                return string.Concat(objArray1);
            }
            string format = "{0:F" + decimals + "}";
            string[] textArray1 = new string[] { "<", string.Format(format, v.x), ",", string.Format(format, v.y), ">" };
            return string.Concat(textArray1);
        }

        [Extension]
        public static Vector2 Product(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }
    }
}

