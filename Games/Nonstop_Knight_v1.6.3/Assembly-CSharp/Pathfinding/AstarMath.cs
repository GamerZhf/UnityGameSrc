namespace Pathfinding
{
    using System;
    using UnityEngine;

    public static class AstarMath
    {
        [Obsolete("Use Mathf.Abs instead", true)]
        public static int Abs(int a)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Abs instead", true)]
        public static float Abs(float a)
        {
            throw new NotImplementedException("Obsolete");
        }

        private static int Bit(int a, int b)
        {
            return ((a >> b) & 1);
        }

        [Obsolete("Use Mathf.Clamp instead", true)]
        public static int Clamp(int a, int b, int c)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Clamp instead", true)]
        public static float Clamp(float a, float b, float c)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Clamp01 instead", true)]
        public static int Clamp01(int a)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Clamp01 instead", true)]
        public static float Clamp01(float a)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Int3.GetHashCode instead", true)]
        public static int ComputeVertexHash(int x, int y, int z)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use AstarSplines.CubicBezier instead")]
        public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            return AstarSplines.CubicBezier(p0, p1, p2, p3, t);
        }

        [Obsolete("Use VectorMath.SqrDistancePointSegmentApproximate instead")]
        public static float DistancePointSegment(Int3 a, Int3 b, Int3 p)
        {
            return VectorMath.SqrDistancePointSegmentApproximate(a, b, p);
        }

        [Obsolete("Use VectorMath.SqrDistancePointSegmentApproximate instead")]
        public static float DistancePointSegment(int x, int z, int px, int pz, int qx, int qz)
        {
            return VectorMath.SqrDistancePointSegmentApproximate(x, z, px, pz, qx, qz);
        }

        [Obsolete("Obsolete", true)]
        public static float DistancePointSegment2(Vector3 a, Vector3 b, Vector3 p)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Obsolete", true)]
        public static float DistancePointSegment2(int x, int z, int px, int pz, int qx, int qz)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use VectorMath.SqrDistancePointSegment instead")]
        public static float DistancePointSegmentStrict(Vector3 a, Vector3 b, Vector3 p)
        {
            return VectorMath.SqrDistancePointSegment(a, b, p);
        }

        [Obsolete("Obsolete", true)]
        public static string FormatBytes(int bytes)
        {
            throw new NotImplementedException("Obsolete");
        }

        public static string FormatBytesBinary(int bytes)
        {
            double num = (bytes < 0) ? -1.0 : 1.0;
            bytes = (bytes < 0) ? -bytes : bytes;
            if (bytes < 0x400)
            {
                return ((bytes * num) + " bytes");
            }
            if (bytes < 0x100000)
            {
                double num2 = (((double) bytes) / 1024.0) * num;
                return (num2.ToString("0.0") + " kb");
            }
            if (bytes < 0x40000000)
            {
                double num3 = (((double) bytes) / 1048576.0) * num;
                return (num3.ToString("0.0") + " mb");
            }
            double num4 = (((double) bytes) / 1073741824.0) * num;
            return (num4.ToString("0.0") + " gb");
        }

        [Obsolete("Obsolete", true)]
        public static float Hermite(float start, float end, float value)
        {
            throw new NotImplementedException("Obsolete");
        }

        public static Color HSVToRGB(float h, float s, float v)
        {
            float r = 0f;
            float g = 0f;
            float num7 = 0f;
            float num2 = s * v;
            float num3 = h / 60f;
            float num4 = num2 * (1f - Math.Abs((float) ((num3 % 2f) - 1f)));
            if (num3 < 1f)
            {
                r = num2;
                g = num4;
            }
            else if (num3 < 2f)
            {
                r = num4;
                g = num2;
            }
            else if (num3 < 3f)
            {
                g = num2;
                num7 = num4;
            }
            else if (num3 < 4f)
            {
                g = num4;
                num7 = num2;
            }
            else if (num3 < 5f)
            {
                r = num4;
                num7 = num2;
            }
            else if (num3 < 6f)
            {
                r = num2;
                num7 = num4;
            }
            float num = v - num2;
            r += num;
            g += num;
            return new Color(r, g, num7 + num);
        }

        public static Color IntToColor(int i, float a)
        {
            int num = (Bit(i, 1) + (Bit(i, 3) * 2)) + 1;
            int num2 = (Bit(i, 2) + (Bit(i, 4) * 2)) + 1;
            int num3 = (Bit(i, 0) + (Bit(i, 5) * 2)) + 1;
            return new Color(num * 0.25f, num2 * 0.25f, num3 * 0.25f, a);
        }

        [Obsolete("Use Mathf.Lerp instead", true)]
        public static float Lerp(float a, float b, float t)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Obsolete", true)]
        public static float MagnitudeXZ(Vector3 a, Vector3 b)
        {
            throw new NotImplementedException("Obsolete");
        }

        public static float MapTo(float startMin, float startMax, float value)
        {
            value -= startMin;
            value /= startMax - startMin;
            value = Mathf.Clamp01(value);
            return value;
        }

        public static float MapTo(float startMin, float startMax, float targetMin, float targetMax, float value)
        {
            value -= startMin;
            value /= startMax - startMin;
            value = Mathf.Clamp01(value);
            value *= targetMax - targetMin;
            value += targetMin;
            return value;
        }

        [Obsolete("Obsolete", true)]
        public static float MapToRange(float targetMin, float targetMax, float value)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Max instead", true)]
        public static int Max(int a, int b)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Max instead", true)]
        public static float Max(float a, float b)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Max instead", true)]
        public static ushort Max(ushort a, ushort b)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Max instead", true)]
        public static uint Max(uint a, uint b)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Min instead", true)]
        public static int Min(int a, int b)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Min instead", true)]
        public static float Min(float a, float b)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Min instead", true)]
        public static uint Min(uint a, uint b)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use VectorMath.ClosestPointOnLine instead")]
        public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            return VectorMath.ClosestPointOnLine(lineStart, lineEnd, point);
        }

        [Obsolete("Use VectorMath.ClosestPointOnLineFactor instead")]
        public static float NearestPointFactor(Int2 lineStart, Int2 lineEnd, Int2 point)
        {
            return VectorMath.ClosestPointOnLineFactor(lineStart, lineEnd, point);
        }

        [Obsolete("Use VectorMath.ClosestPointOnLineFactor instead")]
        public static float NearestPointFactor(Int3 lineStart, Int3 lineEnd, Int3 point)
        {
            return VectorMath.ClosestPointOnLineFactor(lineStart, lineEnd, point);
        }

        [Obsolete("Use VectorMath.ClosestPointOnLineFactor instead")]
        public static float NearestPointFactor(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            return VectorMath.ClosestPointOnLineFactor(lineStart, lineEnd, point);
        }

        [Obsolete("Use VectorMath.ClosestPointOnSegment instead")]
        public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            return VectorMath.ClosestPointOnSegment(lineStart, lineEnd, point);
        }

        [Obsolete("Use VectorMath.ClosestPointOnSegmentXZ instead")]
        public static Vector3 NearestPointStrictXZ(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            return VectorMath.ClosestPointOnSegmentXZ(lineStart, lineEnd, point);
        }

        [Obsolete("Obsolete", true)]
        public static int Repeat(int i, int n)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.RoundToInt instead", true)]
        public static int RoundToInt(double v)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.RoundToInt instead", true)]
        public static int RoundToInt(float v)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Sign instead", true)]
        public static int Sign(int a)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use Mathf.Sign instead", true)]
        public static float Sign(float a)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete("Use VectorMath.SqrDistanceXZ instead")]
        public static float SqrMagnitudeXZ(Vector3 a, Vector3 b)
        {
            return VectorMath.SqrDistanceXZ(a, b);
        }
    }
}

