﻿namespace Pathfinding
{
    using System;
    using UnityEngine;

    internal static class AstarSplines
    {
        public static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime)
        {
            float num = elapsedTime;
            float num2 = num * num;
            float num3 = num2 * num;
            return (Vector3) ((((previous * (((-0.5f * num3) + num2) - (0.5f * num))) + (start * (((1.5f * num3) + (-2.5f * num2)) + 1f))) + (end * (((-1.5f * num3) + (2f * num2)) + (0.5f * num)))) + (next * ((0.5f * num3) - (0.5f * num2))));
        }

        [Obsolete("Use CatmullRom")]
        public static Vector3 CatmullRomOLD(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime)
        {
            return CatmullRom(previous, start, end, next, elapsedTime);
        }

        public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float num = 1f - t;
            return (Vector3) ((((((num * num) * num) * p0) + ((((3f * num) * num) * t) * p1)) + ((((3f * num) * t) * t) * p2)) + (((t * t) * t) * p3));
        }
    }
}

