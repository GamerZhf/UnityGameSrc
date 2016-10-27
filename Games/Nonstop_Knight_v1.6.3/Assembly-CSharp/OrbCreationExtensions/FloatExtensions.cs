namespace OrbCreationExtensions
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Extension]
    public static class FloatExtensions
    {
        [Extension]
        public static float DegreesToCompassAngle(float angle)
        {
            angle = 90f - angle;
            return To360Angle(angle);
        }

        [Extension]
        public static float Distance(float f1, float f2)
        {
            return Mathf.Abs((float) (f1 - f2));
        }

        [Extension]
        public static string MakeString(float aFloat)
        {
            return (string.Empty + aFloat);
        }

        [Extension]
        public static string MakeString(float aFloat, int decimals)
        {
            if (decimals <= 0)
            {
                return (string.Empty + Mathf.RoundToInt(aFloat));
            }
            return string.Format("{0:F" + decimals + "}", aFloat);
        }

        [Extension]
        public static float RadToCompassAngle(float rad)
        {
            return DegreesToCompassAngle(rad * 57.29578f);
        }

        [Extension]
        public static float RelativePositionBetweenAngles(float angle, float from, float to)
        {
            from = To360Angle(from);
            to = To360Angle(to);
            if ((from - to) > 180f)
            {
                from -= 360f;
            }
            if ((to - from) > 180f)
            {
                to -= 360f;
            }
            angle = To360Angle(angle);
            if (from < to)
            {
                if ((angle >= from) && (angle < to))
                {
                    return ((angle - from) / (to - from));
                }
                if (((angle - 360f) >= from) && ((angle - 360f) < to))
                {
                    return (((angle - 360f) - from) / (to - from));
                }
            }
            if (from > to)
            {
                if ((angle < from) && (angle >= to))
                {
                    return ((angle - to) / (from - to));
                }
                if (((angle - 360f) < from) && ((angle - 360f) >= to))
                {
                    return (((angle - 360f) - to) / (from - to));
                }
            }
            return -1f;
        }

        [Extension]
        public static float Round(float f, int decimals)
        {
            float num = Mathf.Pow(10f, (float) decimals);
            f = Mathf.Round(f * num);
            return (f / num);
        }

        [Extension]
        public static float To180Angle(float f)
        {
            while (f <= -180f)
            {
                f += 360f;
            }
            while (f > 180f)
            {
                f -= 360f;
            }
            return f;
        }

        [Extension]
        public static float To360Angle(float f)
        {
            while (f < 0f)
            {
                f += 360f;
            }
            while (f >= 360f)
            {
                f -= 360f;
            }
            return f;
        }
    }
}

