namespace OrbCreationExtensions
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Extension]
    public static class Vector3Extensions
    {
        [Extension]
        public static Vector3 Abs(Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        [Extension]
        public static Vector2 Barycentric(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 lhs = c - a;
            Vector3 rhs = b - a;
            Vector3 vector3 = p - a;
            float num = Vector3.Dot(lhs, lhs);
            float num2 = Vector3.Dot(lhs, rhs);
            float num3 = Vector3.Dot(lhs, vector3);
            float num4 = Vector3.Dot(rhs, rhs);
            float num5 = Vector3.Dot(rhs, vector3);
            float num6 = 1f / ((num * num4) - (num2 * num2));
            float x = ((num4 * num3) - (num2 * num5)) * num6;
            return new Vector2(x, ((num * num5) - (num2 * num3)) * num6);
        }

        [Extension]
        public static bool DiffBetween(Vector3 v1, Vector3 v2, float from, float to)
        {
            return ((((((v1.x - v2.x) < to) && ((v1.x - v2.x) > from)) && (((v1.y - v2.y) < to) && ((v1.y - v2.y) > from))) && ((v1.z - v2.z) < to)) && ((v1.z - v2.z) > from));
        }

        [Extension]
        public static float InProduct(Vector3 v1, Vector3 v2)
        {
            return (((v1.x * v2.x) + (v1.y * v2.y)) + (v1.z * v2.z));
        }

        [Extension]
        public static bool IsAllSmaller(Vector3 v1, Vector3 v2)
        {
            return (((v1.x < v2.x) && (v1.y < v2.y)) && (v1.z < v2.z));
        }

        [Extension]
        public static bool IsDiffSmallEnough(Vector3 v1, Vector3 v2, float maxDiff)
        {
            return ((((((v1.x - v2.x) < maxDiff) && ((v2.x - v1.x) < maxDiff)) && (((v1.y - v2.y) < maxDiff) && ((v2.y - v1.y) < maxDiff))) && ((v1.z - v2.z) < maxDiff)) && ((v2.z - v1.z) < maxDiff));
        }

        [Extension]
        public static bool IsEqual(Vector3 v1, Vector3 v2)
        {
            return (((v1.x == v2.x) && (v1.y == v2.y)) && (v1.z == v2.z));
        }

        [Extension]
        public static string MakeString(Vector3 v)
        {
            object[] objArray1 = new object[] { "<", v.x, ",", v.y, ",", v.z, ">" };
            return string.Concat(objArray1);
        }

        [Extension]
        public static string MakeString(Vector3 v, int decimals)
        {
            if (decimals <= 0)
            {
                object[] objArray1 = new object[] { "<", Mathf.RoundToInt(v.x), ",", Mathf.RoundToInt(v.y), ",", Mathf.RoundToInt(v.z), ">" };
                return string.Concat(objArray1);
            }
            string format = "{0:F" + decimals + "}";
            string[] textArray1 = new string[] { "<", string.Format(format, v.x), ",", string.Format(format, v.y), ",", string.Format(format, v.z), ">" };
            return string.Concat(textArray1);
        }

        [Extension]
        public static Vector3 Product(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        [Extension]
        public static float Sum(Vector3 v1)
        {
            return ((v1.x + v1.y) + v1.z);
        }

        [Extension]
        public static Vector3 To180Angle(Vector3 v)
        {
            v.x = FloatExtensions.To180Angle(v.x);
            v.y = FloatExtensions.To180Angle(v.y);
            v.z = FloatExtensions.To180Angle(v.z);
            return v;
        }

        [Extension]
        public static Vector3 To360Angle(Vector3 v)
        {
            v.x = FloatExtensions.To360Angle(v.x);
            v.y = FloatExtensions.To360Angle(v.y);
            v.z = FloatExtensions.To360Angle(v.z);
            return v;
        }

        [Extension]
        public static Vector3 VectorMax(Vector3 v1, Vector3 v2)
        {
            return new Vector3(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y), Mathf.Max(v1.z, v2.z));
        }
    }
}

