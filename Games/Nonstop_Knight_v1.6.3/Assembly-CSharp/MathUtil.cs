using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MathUtil
{
    public static float Average<T>(IEnumerable<T> list, Converter<T, int> converter)
    {
        if ((list == null) || !Enumerable.Any<T>(list))
        {
            return 0f;
        }
        int num = 0;
        int num2 = 0;
        IEnumerator<T> enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;
                num += converter(current);
                num2++;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        return (((float) num) / ((float) num2));
    }

    public static float Average<T>(IEnumerable<T> list, Converter<T, float> converter)
    {
        if ((list == null) || !Enumerable.Any<T>(list))
        {
            return 0f;
        }
        float num = 0f;
        int num2 = 0;
        IEnumerator<T> enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;
                num += converter(current);
                num2++;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        return (num / ((float) num2));
    }

    public static uint BinomialCoefficient(uint n, uint k)
    {
        uint num = 1;
        if (k > n)
        {
            return 0;
        }
        for (uint i = 1; i <= k; i++)
        {
            num *= n;
            n--;
            num /= i;
        }
        return num;
    }

    public static Vector2 CatmullRom(float v, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        Vector2 at = CatmullRomTangent(p0, p2);
        Vector2 bt = CatmullRomTangent(p1, p3);
        return Hermite(v, p1, at, p2, bt);
    }

    public static Vector3 CatmullRom(float v, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 at = CatmullRomTangent(p0, p2);
        Vector3 bt = CatmullRomTangent(p1, p3);
        return Hermite(v, p1, at, p2, bt);
    }

    public static Vector2 CatmullRomTangent(Vector2 a, Vector2 b)
    {
        return (Vector2) ((b - a) * 0.5f);
    }

    public static Vector3 CatmullRomTangent(Vector3 a, Vector3 b)
    {
        return (Vector3) ((b - a) * 0.5f);
    }

    public static bool CircleLineIntersections(Vector2 p, Vector2 n, Vector2 q, float r, ref float dist1, ref float dist2)
    {
        float sqrMagnitude = n.sqrMagnitude;
        float b = 2f * Vector3.Dot((Vector3) (p - q), (Vector3) n);
        float c = ((p.sqrMagnitude + q.sqrMagnitude) - (2f * Vector3.Dot((Vector3) q, (Vector3) p))) - (r * r);
        return QuadraticRoots(sqrMagnitude, b, c, ref dist1, ref dist2);
    }

    public static double Clamp(double v, double min, double max)
    {
        if (v < min)
        {
            return min;
        }
        if (v > max)
        {
            return max;
        }
        return v;
    }

    public static long Clamp(long v, long min, long max)
    {
        if (v < min)
        {
            return min;
        }
        if (v > max)
        {
            return max;
        }
        return v;
    }

    public static void Clamp(ref Vector2 source, float min, float max)
    {
        float magnitude = source.magnitude;
        source = (Vector2) (source.normalized * Mathf.Clamp(magnitude, min, max));
    }

    public static void Clamp(ref Vector3 source, float min, float max)
    {
        float magnitude = source.magnitude;
        source = (Vector3) (source.normalized * Mathf.Clamp(magnitude, min, max));
    }

    public static Vector2 Clamp(Vector2 source, float min, float max)
    {
        float magnitude = source.magnitude;
        return (Vector2) (source.normalized * Mathf.Clamp(magnitude, min, max));
    }

    public static Vector3 Clamp(Vector3 source, float min, float max)
    {
        float magnitude = source.magnitude;
        return (Vector3) (source.normalized * Mathf.Clamp(magnitude, min, max));
    }

    public static float Clamp01(double v)
    {
        return (float) Clamp(v, 0.0, 1.0);
    }

    public static void ClampComponents(ref Vector2 source, float min, float max)
    {
        source.x = Mathf.Clamp(source.x, min, max);
        source.y = Mathf.Clamp(source.y, min, max);
    }

    public static void ClampComponents(ref Vector3 source, float min, float max)
    {
        source.x = Mathf.Clamp(source.x, min, max);
        source.y = Mathf.Clamp(source.y, min, max);
        source.z = Mathf.Clamp(source.z, min, max);
    }

    public static Vector2 ClampComponents(Vector2 source, float min, float max)
    {
        source.x = Mathf.Clamp(source.x, min, max);
        source.y = Mathf.Clamp(source.y, min, max);
        return source;
    }

    public static Vector3 ClampComponents(Vector3 source, float min, float max)
    {
        source.x = Mathf.Clamp(source.x, min, max);
        source.y = Mathf.Clamp(source.y, min, max);
        source.z = Mathf.Clamp(source.z, min, max);
        return source;
    }

    public static void ClampComponents01(ref Vector2 source)
    {
        source.x = Mathf.Clamp(source.x, 0f, 1f);
        source.y = Mathf.Clamp(source.y, 0f, 1f);
    }

    public static void ClampComponents01(ref Vector3 source)
    {
        source.x = Mathf.Clamp(source.x, 0f, 1f);
        source.y = Mathf.Clamp(source.y, 0f, 1f);
        source.z = Mathf.Clamp(source.z, 0f, 1f);
    }

    public static Vector2 ClampComponents01(Vector2 source)
    {
        source.x = Mathf.Clamp(source.x, 0f, 1f);
        source.y = Mathf.Clamp(source.y, 0f, 1f);
        return source;
    }

    public static Vector3 ClampComponents01(Vector3 source)
    {
        source.x = Mathf.Clamp(source.x, 0f, 1f);
        source.y = Mathf.Clamp(source.y, 0f, 1f);
        source.z = Mathf.Clamp(source.z, 0f, 1f);
        return source;
    }

    public static double ClampMin(double v, double min)
    {
        if (v < min)
        {
            return min;
        }
        return v;
    }

    public static Vector2 CubicBezier(float v, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float num = 1f - v;
        float num2 = num * num;
        float num3 = v * v;
        return (Vector2) (((((num2 * num) * p0) + (((3f * num2) * v) * p1)) + (((3f * num) * num3) * p2)) + ((num3 * v) * p3));
    }

    public static Vector3 CubicBezier(float v, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float num = 1f - v;
        float num2 = num * num;
        float num3 = v * v;
        return (Vector3) (((((num2 * num) * p0) + (((3f * num2) * v) * p1)) + (((3f * num) * num3) * p2)) + ((num3 * v) * p3));
    }

    public static void DistributeValuesIntoChunksDouble(double totalAmount, int numChunks, ref List<double> targetList)
    {
        if (numChunks > 0)
        {
            List<double> list;
            int num4;
            targetList.Clear();
            double item = Math.Floor(totalAmount / ((double) numChunks));
            double d = Math.Floor(totalAmount - (item * numChunks));
            for (int i = 0; i < numChunks; i++)
            {
                targetList.Add(item);
            }
            double num5 = list[num4];
            (list = targetList)[num4 = targetList.Count - 1] = num5 + (double.IsNaN(d) ? 0.0 : d);
        }
    }

    public static void DistributeValuesIntoChunksInt(int totalAmount, int numChunks, ref List<double> targetList)
    {
        if (numChunks > 0)
        {
            List<double> list;
            int num4;
            targetList.Clear();
            int num = totalAmount / numChunks;
            int num2 = totalAmount - (num * numChunks);
            for (int i = 0; i < numChunks; i++)
            {
                targetList.Add((double) num);
            }
            double num5 = list[num4];
            (list = targetList)[num4 = targetList.Count - 1] = num5 + num2;
        }
    }

    public static int FindMax<T>(IEnumerable<T> list, Converter<T, int> converter)
    {
        if ((list == null) || !Enumerable.Any<T>(list))
        {
            return 0;
        }
        int num = -2147483648;
        IEnumerator<T> enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;
                int num2 = converter(current);
                if (num2 > num)
                {
                    num = num2;
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        return num;
    }

    public static float FindMax<T>(IEnumerable<T> list, Converter<T, float> converter)
    {
        if ((list == null) || !Enumerable.Any<T>(list))
        {
            return float.NaN;
        }
        float minValue = float.MinValue;
        IEnumerator<T> enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;
                float num2 = converter(current);
                if (num2 > minValue)
                {
                    minValue = num2;
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        return minValue;
    }

    public static int FindMin<T>(IEnumerable<T> list, Converter<T, int> converter)
    {
        if ((list == null) || !Enumerable.Any<T>(list))
        {
            return 0;
        }
        int num = 0x7fffffff;
        IEnumerator<T> enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;
                int num2 = converter(current);
                if (num2 < num)
                {
                    num = num2;
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        return num;
    }

    public static float FindMin<T>(IEnumerable<T> list, Converter<T, float> converter)
    {
        if ((list == null) || !Enumerable.Any<T>(list))
        {
            return float.NaN;
        }
        float maxValue = float.MaxValue;
        IEnumerator<T> enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;
                float num2 = converter(current);
                if (num2 < maxValue)
                {
                    maxValue = num2;
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        return maxValue;
    }

    public static IEnumerable<T> GetRandomSubSet<T>(IEnumerable<T> list, int max)
    {
        IEnumerable<T> enumerable;
        IList<T> list1 = list as IList<T>;
        if (list1 != null)
        {
            enumerable = list1;
        }
        else
        {
            enumerable = Enumerable.ToList<T>(list);
        }
        if (Enumerable.Count<T>(enumerable) <= max)
        {
            return enumerable;
        }
        IList<T> list2 = new List<T>();
        int num = max;
        for (int i = Enumerable.Count<T>(enumerable) - 1; i >= 0; i--)
        {
            if (((i + 1) == num) || ((((float) num) / ((float) (i + 1))) > UnityEngine.Random.Range((float) 0f, (float) 1f)))
            {
                list2.Add(Enumerable.ElementAt<T>(enumerable, i));
                num--;
            }
        }
        return list2;
    }

    public static Vector2 Hermite(float v, Vector2 a, Vector2 at, Vector2 b, Vector2 bt)
    {
        float num = v * v;
        float num2 = v * num;
        float num3 = (2f * num2) - (3f * num);
        float num4 = num3 + 1f;
        float num5 = -num3;
        float num6 = (num2 - (2f * num)) + v;
        float num7 = num2 - num;
        return (Vector2) ((((a * num4) + (b * num5)) + (at * num6)) + (bt * num7));
    }

    public static Vector3 Hermite(float v, Vector3 a, Vector3 at, Vector3 b, Vector3 bt)
    {
        float num = v * v;
        float num2 = v * num;
        float num3 = (2f * num2) - (3f * num);
        float num4 = num3 + 1f;
        float num5 = -num3;
        float num6 = (num2 - (2f * num)) + v;
        float num7 = num2 - num;
        return (Vector3) ((((a * num4) + (b * num5)) + (at * num6)) + (bt * num7));
    }

    public static float InverseClamp(float value, float min, float max)
    {
        float f = min - value;
        float num2 = max - value;
        if ((f >= 0f) || (num2 <= 0f))
        {
            return value;
        }
        return ((Mathf.Abs(f) >= num2) ? max : min);
    }

    public static bool IsInRange<T>(T value, T min, T max) where T: IComparable
    {
        return ((value.CompareTo(min) >= 0) && (value.CompareTo(max) <= 0));
    }

    public static T Loop<T>(T value, T min, T max) where T: IComparable
    {
        if (value.CompareTo(max) > 0)
        {
            return min;
        }
        if (value.CompareTo(min) < 0)
        {
            return max;
        }
        return value;
    }

    public static uint LowerPowerOfTwo(uint v)
    {
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 0x10;
        return (v - (v >> 1));
    }

    public static Matrix4x4 MatrixFromFunction(Func<Vector3, Vector3> linearFunction)
    {
        Matrix4x4 matrixx = new Matrix4x4();
        Vector3 vector = linearFunction(new Vector3(1f, 0f, 0f));
        matrixx.SetColumn(0, new Vector4(vector[0], vector[1], vector[2], 0f));
        Vector3 vector2 = linearFunction(new Vector3(0f, 1f, 0f));
        matrixx.SetColumn(1, new Vector4(vector2[0], vector2[1], vector2[2], 0f));
        Vector3 vector3 = linearFunction(new Vector3(0f, 0f, 1f));
        matrixx.SetColumn(2, new Vector4(vector3[0], vector3[1], vector3[2], 0f));
        matrixx.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
        return matrixx;
    }

    public static Matrix4x4 MatrixFromFunction(Func<Vector4, Vector4> linearFunction)
    {
        Matrix4x4 matrixx = new Matrix4x4();
        matrixx.SetColumn(0, linearFunction(new Vector4(1f, 0f, 0f, 0f)));
        matrixx.SetColumn(1, linearFunction(new Vector4(0f, 1f, 0f, 0f)));
        matrixx.SetColumn(2, linearFunction(new Vector4(0f, 0f, 1f, 0f)));
        matrixx.SetColumn(3, linearFunction(new Vector4(0f, 0f, 0f, 1f)));
        return matrixx;
    }

    public static void MultiplyComponents(ref Vector2 vector, Vector2 multiplier)
    {
        vector.x *= multiplier.x;
        vector.y *= multiplier.y;
    }

    public static Vector2 MultiplyComponents(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x * b.x, a.y * b.y);
    }

    public static void MultiplyComponents(ref Vector3 vector, Vector3 multiplier)
    {
        vector.x *= multiplier.x;
        vector.y *= multiplier.y;
        vector.z *= multiplier.z;
    }

    public static Vector3 MultiplyComponents(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public static void Normalize(ref Quaternion q)
    {
        float f = (((q.w * q.w) + (q.x * q.x)) + (q.y * q.y)) + (q.z * q.z);
        if ((0.9999f >= f) || (f >= 1.0001f))
        {
            float num2 = Mathf.Sqrt(f);
            q.w /= num2;
            q.x /= num2;
            q.y /= num2;
            q.z /= num2;
        }
    }

    public static Vector2 QuadraticBezier(float v, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float num = 1f - v;
        return (Vector2) ((num * ((num * p0) + (v * p1))) + (v * ((num * p1) + (v * p2))));
    }

    public static Vector3 QuadraticBezier(float v, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float num = 1f - v;
        return (Vector3) ((num * ((num * p0) + (v * p1))) + (v * ((num * p1) + (v * p2))));
    }

    public static bool QuadraticRoots(float a, float b, float c, ref float root1, ref float root2)
    {
        float f = (b * b) - ((4f * a) * c);
        if (f < 0f)
        {
            return false;
        }
        root1 = (-b - Mathf.Sqrt(f)) / (2f * a);
        root2 = (-b + Mathf.Sqrt(f)) / (2f * a);
        return true;
    }

    public static Vector2 QuarticBezier(float v, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        float num = v * v;
        float num2 = num * v;
        float num3 = num2 * v;
        float num4 = num3 * v;
        float num5 = 1f - v;
        float num6 = num5 * num5;
        float num7 = num6 * num5;
        float num8 = num7 * num5;
        float num9 = num8 * num5;
        return (Vector2) ((((((num9 * p0) + (((5f * v) * num8) * p1)) + (((10f * num) * num7) * p2)) + (((10f * num2) * num6) * p3)) + (((5f * num3) * num5) * p4)) + (num4 * p4));
    }

    public static Vector3 QuarticBezier(float v, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float num = v * v;
        float num2 = num * v;
        float num3 = num2 * v;
        float num4 = num3 * v;
        float num5 = 1f - v;
        float num6 = num5 * num5;
        float num7 = num6 * num5;
        float num8 = num7 * num5;
        float num9 = num8 * num5;
        return (Vector3) ((((((num9 * p0) + (((5f * v) * num8) * p1)) + (((10f * num) * num7) * p2)) + (((10f * num2) * num6) * p3)) + (((5f * num3) * num5) * p4)) + (num4 * p4));
    }

    public static int RandomFromRange(int min, int max, uint random_value)
    {
        return (min + ((int) (((ulong) random_value) % ((long) ((max - min) + 1)))));
    }

    public static float RandomSign()
    {
        return ((UnityEngine.Random.Range(0, 2) != 0) ? -1f : 1f);
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 vector = point - pivot;
        vector = (Vector3) (Quaternion.Euler(angles) * vector);
        point = vector + pivot;
        return point;
    }

    public static float RoundToNumDecimals(float value, int numDecimals)
    {
        float num = Mathf.Pow(10f, (float) numDecimals);
        return (Mathf.Round(value * num) / num);
    }

    public static double RoundToSignificantDigits(double d, int digits)
    {
        if (d == 0.0)
        {
            return 0.0;
        }
        double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(d))) + 1.0);
        return (num * Math.Round(d / num, digits));
    }

    public static int SampleDiscreteDistribution(List<int> distribution)
    {
        int num = 0;
        foreach (int num2 in distribution)
        {
            num += num2;
        }
        int num3 = UnityEngine.Random.Range(0, num - 1);
        int num4 = 0;
        int num5 = 0;
        while (true)
        {
            num4 += distribution[num5];
            if (num4 > num3)
            {
                return num5;
            }
            num5++;
        }
    }

    public static float SampleNormalDistributionBoxMuller(float mean, float stdDev)
    {
        float f = UnityEngine.Random.Range((float) 0f, (float) 1f);
        float num2 = UnityEngine.Random.Range((float) 0f, (float) 1f);
        float num3 = Mathf.Sqrt(-2f * Mathf.Log(f)) * Mathf.Sin(6.283185f * num2);
        return (mean + (stdDev * num3));
    }

    public static uint UpperPowerOfTwo(uint v)
    {
        v--;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 0x10;
        v++;
        return v;
    }

    public static int Wrap(int value, int min, int max)
    {
        int num = max - min;
        if (num == 0)
        {
            return min;
        }
        if (max < min)
        {
            throw new InvalidOperationException("Wrap MAX needs to be greater that MIN.");
        }
        while (value < min)
        {
            value += num;
        }
        while (value > max)
        {
            value -= num;
        }
        return value;
    }
}

