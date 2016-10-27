namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class Polygon
    {
        [Obsolete("Scheduled for removal since it is not used by any part of the A* Pathfinding Project")]
        public static Vector3 ClosestPointOnTriangle(Vector3[] triangle, Vector3 point)
        {
            return ClosestPointOnTriangle(triangle[0], triangle[1], triangle[2], point);
        }

        public static Vector2 ClosestPointOnTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
        {
            Vector2 lhs = b - a;
            Vector2 vector2 = c - a;
            Vector2 rhs = p - a;
            float num = Vector2.Dot(lhs, rhs);
            float num2 = Vector2.Dot(vector2, rhs);
            if ((num <= 0f) && (num2 <= 0f))
            {
                return a;
            }
            Vector2 vector4 = p - b;
            float num3 = Vector2.Dot(lhs, vector4);
            float num4 = Vector2.Dot(vector2, vector4);
            if ((num3 >= 0f) && (num4 <= num3))
            {
                return b;
            }
            if ((num >= 0f) && (num3 <= 0f))
            {
                float num5 = (num * num4) - (num3 * num2);
                if (num5 <= 0f)
                {
                    float num6 = num / (num - num3);
                    return (a + ((Vector2) (lhs * num6)));
                }
            }
            Vector2 vector5 = p - c;
            float num7 = Vector2.Dot(lhs, vector5);
            float num8 = Vector2.Dot(vector2, vector5);
            if ((num8 >= 0f) && (num7 <= num8))
            {
                return c;
            }
            if ((num2 >= 0f) && (num8 <= 0f))
            {
                float num9 = (num7 * num2) - (num * num8);
                if (num9 <= 0f)
                {
                    float num10 = num2 / (num2 - num8);
                    return (a + ((Vector2) (vector2 * num10)));
                }
            }
            if (((num4 - num3) >= 0f) && ((num7 - num8) >= 0f))
            {
                float num11 = (num3 * num8) - (num7 * num4);
                if (num11 <= 0f)
                {
                    float num12 = (num4 - num3) / ((num4 - num3) + (num7 - num8));
                    return (b + ((Vector2) ((c - b) * num12)));
                }
            }
            return p;
        }

        public static Vector3 ClosestPointOnTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
        {
            Vector3 lhs = b - a;
            Vector3 vector2 = c - a;
            Vector3 rhs = p - a;
            float num = Vector3.Dot(lhs, rhs);
            float num2 = Vector3.Dot(vector2, rhs);
            if ((num <= 0f) && (num2 <= 0f))
            {
                return a;
            }
            Vector3 vector4 = p - b;
            float num3 = Vector3.Dot(lhs, vector4);
            float num4 = Vector3.Dot(vector2, vector4);
            if ((num3 >= 0f) && (num4 <= num3))
            {
                return b;
            }
            float num5 = (num * num4) - (num3 * num2);
            if (((num >= 0f) && (num3 <= 0f)) && (num5 <= 0f))
            {
                float num6 = num / (num - num3);
                return (a + ((Vector3) (lhs * num6)));
            }
            Vector3 vector5 = p - c;
            float num7 = Vector3.Dot(lhs, vector5);
            float num8 = Vector3.Dot(vector2, vector5);
            if ((num8 >= 0f) && (num7 <= num8))
            {
                return c;
            }
            float num9 = (num7 * num2) - (num * num8);
            if (((num2 >= 0f) && (num8 <= 0f)) && (num9 <= 0f))
            {
                float num10 = num2 / (num2 - num8);
                return (a + ((Vector3) (vector2 * num10)));
            }
            float num11 = (num3 * num8) - (num7 * num4);
            if ((((num4 - num3) >= 0f) && ((num7 - num8) >= 0f)) && (num11 <= 0f))
            {
                float num12 = (num4 - num3) / ((num4 - num3) + (num7 - num8));
                return (b + ((Vector3) ((c - b) * num12)));
            }
            float num13 = 1f / ((num11 + num9) + num5);
            float num14 = num9 * num13;
            float num15 = num5 * num13;
            return (Vector3) ((a + (lhs * num14)) + (vector2 * num15));
        }

        public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
        {
            int index = polyPoints.Length - 1;
            bool flag = false;
            int num2 = 0;
            while (num2 < polyPoints.Length)
            {
                if ((((polyPoints[num2].y <= p.y) && (p.y < polyPoints[index].y)) || ((polyPoints[index].y <= p.y) && (p.y < polyPoints[num2].y))) && (p.x < ((((polyPoints[index].x - polyPoints[num2].x) * (p.y - polyPoints[num2].y)) / (polyPoints[index].y - polyPoints[num2].y)) + polyPoints[num2].x)))
                {
                    flag = !flag;
                }
                index = num2++;
            }
            return flag;
        }

        [Obsolete("Use ContainsPointXZ instead")]
        public static bool ContainsPoint(Vector3[] polyPoints, Vector3 p)
        {
            return ContainsPointXZ(polyPoints, p);
        }

        public static bool ContainsPoint(Int2 a, Int2 b, Int2 c, Int2 p)
        {
            return ((VectorMath.IsClockwiseOrColinear(a, b, p) && VectorMath.IsClockwiseOrColinear(b, c, p)) && VectorMath.IsClockwiseOrColinear(c, a, p));
        }

        [Obsolete("Use ContainsPointXZ instead")]
        public static bool ContainsPoint(Int3 a, Int3 b, Int3 c, Int3 p)
        {
            return ContainsPointXZ(a, b, c, p);
        }

        [Obsolete("Use ContainsPointXZ instead")]
        public static bool ContainsPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
        {
            return ContainsPointXZ(a, b, c, p);
        }

        public static bool ContainsPointXZ(Vector3[] polyPoints, Vector3 p)
        {
            int index = polyPoints.Length - 1;
            bool flag = false;
            int num2 = 0;
            while (num2 < polyPoints.Length)
            {
                if ((((polyPoints[num2].z <= p.z) && (p.z < polyPoints[index].z)) || ((polyPoints[index].z <= p.z) && (p.z < polyPoints[num2].z))) && (p.x < ((((polyPoints[index].x - polyPoints[num2].x) * (p.z - polyPoints[num2].z)) / (polyPoints[index].z - polyPoints[num2].z)) + polyPoints[num2].x)))
                {
                    flag = !flag;
                }
                index = num2++;
            }
            return flag;
        }

        public static bool ContainsPointXZ(Int3 a, Int3 b, Int3 c, Int3 p)
        {
            return ((VectorMath.IsClockwiseOrColinearXZ(a, b, p) && VectorMath.IsClockwiseOrColinearXZ(b, c, p)) && VectorMath.IsClockwiseOrColinearXZ(c, a, p));
        }

        public static bool ContainsPointXZ(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
        {
            return ((VectorMath.IsClockwiseMarginXZ(a, b, p) && VectorMath.IsClockwiseMarginXZ(b, c, p)) && VectorMath.IsClockwiseMarginXZ(c, a, p));
        }

        [Obsolete("Use ConvexHullXZ instead")]
        public static Vector3[] ConvexHull(Vector3[] points)
        {
            return ConvexHullXZ(points);
        }

        public static Vector3[] ConvexHullXZ(Vector3[] points)
        {
            if (points.Length == 0)
            {
                return new Vector3[0];
            }
            List<Vector3> list = ListPool<Vector3>.Claim();
            int index = 0;
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].x < points[index].x)
                {
                    index = i;
                }
            }
            int num3 = index;
            int num4 = 0;
            do
            {
                list.Add(points[index]);
                int num5 = 0;
                for (int j = 0; j < points.Length; j++)
                {
                    if ((num5 == index) || !VectorMath.RightOrColinearXZ(points[index], points[num5], points[j]))
                    {
                        num5 = j;
                    }
                }
                index = num5;
                num4++;
                if (num4 > 0x2710)
                {
                    Debug.LogWarning("Infinite Loop in Convex Hull Calculation");
                    break;
                }
            }
            while (index != num3);
            Vector3[] vectorArray = list.ToArray();
            ListPool<Vector3>.Release(list);
            return vectorArray;
        }

        [Obsolete("Use VectorMath.SqrDistanceSegmentSegment instead")]
        public static float DistanceSegmentSegment3D(Vector3 s1, Vector3 e1, Vector3 s2, Vector3 e2)
        {
            return VectorMath.SqrDistanceSegmentSegment(s1, e1, s2, e2);
        }

        [Obsolete("Use VectorMath.LineIntersectionFactorXZ instead")]
        public static float IntersectionFactor(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
        {
            return VectorMath.LineIntersectionFactorXZ(start1, end1, start2, end2);
        }

        [Obsolete("Use VectorMath.LineIntersectionFactorXZ instead")]
        public static bool IntersectionFactor(Int3 start1, Int3 end1, Int3 start2, Int3 end2, out float factor1, out float factor2)
        {
            return VectorMath.LineIntersectionFactorXZ(start1, end1, start2, end2, out factor1, out factor2);
        }

        [Obsolete("Use VectorMath.LineIntersectionFactorXZ instead")]
        public static bool IntersectionFactor(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out float factor1, out float factor2)
        {
            return VectorMath.LineIntersectionFactorXZ(start1, end1, start2, end2, out factor1, out factor2);
        }

        [Obsolete("Use VectorMath.LineRayIntersectionFactorXZ instead")]
        public static float IntersectionFactorRay(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
        {
            return VectorMath.LineRayIntersectionFactorXZ(start1, end1, start2, end2);
        }

        [Obsolete("Use VectorMath.RaySegmentIntersectXZ instead")]
        public static bool IntersectionFactorRaySegment(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
        {
            return VectorMath.RaySegmentIntersectXZ(start1, end1, start2, end2);
        }

        [Obsolete("Use VectorMath.LineIntersectionPoint instead")]
        public static Vector2 IntersectionPoint(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
        {
            return VectorMath.LineIntersectionPoint(start1, end1, start2, end2);
        }

        [Obsolete("Use VectorMath.LineIntersectionPointXZ instead")]
        public static Vector3 IntersectionPoint(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
        {
            return VectorMath.LineIntersectionPointXZ(start1, end1, start2, end2);
        }

        [Obsolete("Use VectorMath.LineIntersectionPoint instead")]
        public static Vector2 IntersectionPoint(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out bool intersects)
        {
            return VectorMath.LineIntersectionPoint(start1, end1, start2, end2, out intersects);
        }

        [Obsolete("Use VectorMath.LineIntersectionPointXZ instead")]
        public static Vector3 IntersectionPoint(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out bool intersects)
        {
            return VectorMath.LineIntersectionPointXZ(start1, end1, start2, end2, out intersects);
        }

        [Obsolete("Use VectorMath.LineDirIntersectionPointXZ instead")]
        public static Vector3 IntersectionPointOptimized(Vector3 start1, Vector3 dir1, Vector3 start2, Vector3 dir2)
        {
            return VectorMath.LineDirIntersectionPointXZ(start1, dir1, start2, dir2);
        }

        [Obsolete("Use VectorMath.LineDirIntersectionPointXZ instead")]
        public static Vector3 IntersectionPointOptimized(Vector3 start1, Vector3 dir1, Vector3 start2, Vector3 dir2, out bool intersects)
        {
            return VectorMath.LineDirIntersectionPointXZ(start1, dir1, start2, dir2, out intersects);
        }

        [Obsolete("Use VectorMath.SegmentsIntersect instead")]
        public static bool Intersects(Int2 start1, Int2 end1, Int2 start2, Int2 end2)
        {
            return VectorMath.SegmentsIntersect(start1, end1, start2, end2);
        }

        [Obsolete("Use VectorMath.SegmentsIntersectXZ instead")]
        public static bool Intersects(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
        {
            return VectorMath.SegmentsIntersectXZ(start1, end1, start2, end2);
        }

        [Obsolete("Use VectorMath.SegmentsIntersectXZ instead")]
        public static bool Intersects(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
        {
            return VectorMath.SegmentsIntersectXZ(start1, end1, start2, end2);
        }

        [Obsolete("Marked for removal since it is not used by any part of the A* Pathfinding Project")]
        public static bool IntersectsUnclamped(Vector3 a, Vector3 b, Vector3 a2, Vector3 b2)
        {
            return (VectorMath.RightOrColinearXZ(a, b, a2) != VectorMath.RightOrColinearXZ(a, b, b2));
        }

        [Obsolete("Use VectorMath.IsClockwiseXZ instead")]
        public static bool IsClockwise(Int3 a, Int3 b, Int3 c)
        {
            return VectorMath.IsClockwiseXZ(a, b, c);
        }

        [Obsolete("Use VectorMath.IsClockwiseXZ instead")]
        public static bool IsClockwise(Vector3 a, Vector3 b, Vector3 c)
        {
            return VectorMath.IsClockwiseXZ(a, b, c);
        }

        [Obsolete("Use VectorMath.IsClockwiseOrColinear instead")]
        public static bool IsClockwiseMargin(Int2 a, Int2 b, Int2 c)
        {
            return VectorMath.IsClockwiseOrColinear(a, b, c);
        }

        [Obsolete("Use VectorMath.IsClockwiseOrColinearXZ instead")]
        public static bool IsClockwiseMargin(Int3 a, Int3 b, Int3 c)
        {
            return VectorMath.IsClockwiseOrColinearXZ(a, b, c);
        }

        [Obsolete("Use VectorMath.IsClockwiseMarginXZ instead")]
        public static bool IsClockwiseMargin(Vector3 a, Vector3 b, Vector3 c)
        {
            return VectorMath.IsClockwiseMarginXZ(a, b, c);
        }

        [Obsolete("Use VectorMath.IsColinearXZ instead")]
        public static bool IsColinear(Int3 a, Int3 b, Int3 c)
        {
            return VectorMath.IsColinearXZ(a, b, c);
        }

        [Obsolete("Use VectorMath.IsColinearXZ instead")]
        public static bool IsColinear(Vector3 a, Vector3 b, Vector3 c)
        {
            return VectorMath.IsColinearXZ(a, b, c);
        }

        [Obsolete("Use VectorMath.IsColinearAlmostXZ instead")]
        public static bool IsColinearAlmost(Int3 a, Int3 b, Int3 c)
        {
            return VectorMath.IsColinearAlmostXZ(a, b, c);
        }

        [Obsolete("Use VectorMath.RightOrColinear instead. Note that it now uses a left handed coordinate system (same as Unity)")]
        public static bool Left(Int2 a, Int2 b, Int2 p)
        {
            return VectorMath.RightOrColinear(a, b, p);
        }

        [Obsolete("Use VectorMath.RightOrColinearXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
        public static bool Left(Int3 a, Int3 b, Int3 p)
        {
            return VectorMath.RightOrColinearXZ(a, b, p);
        }

        [Obsolete("Use VectorMath.RightOrColinear instead. Note that it now uses a left handed coordinate system (same as Unity)")]
        public static bool Left(Vector2 a, Vector2 b, Vector2 p)
        {
            return VectorMath.RightOrColinear(a, b, p);
        }

        [Obsolete("Use VectorMath.RightOrColinearXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
        public static bool Left(Vector3 a, Vector3 b, Vector3 p)
        {
            return VectorMath.RightOrColinearXZ(a, b, p);
        }

        [Obsolete("Use VectorMath.RightXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
        public static bool LeftNotColinear(Int3 a, Int3 b, Int3 p)
        {
            return VectorMath.RightXZ(a, b, p);
        }

        [Obsolete("Use VectorMath.RightXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
        public static bool LeftNotColinear(Vector3 a, Vector3 b, Vector3 p)
        {
            return VectorMath.RightXZ(a, b, p);
        }

        [Obsolete("Use VectorMath.SegmentIntersectsBounds instead")]
        public static bool LineIntersectsBounds(Bounds bounds, Vector3 a, Vector3 b)
        {
            return VectorMath.SegmentIntersectsBounds(bounds, a, b);
        }

        [Obsolete("Use VectorMath.SegmentIntersectionPointXZ instead")]
        public static Vector3 SegmentIntersectionPoint(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out bool intersects)
        {
            return VectorMath.SegmentIntersectionPointXZ(start1, end1, start2, end2, out intersects);
        }

        public static Vector3[] Subdivide(Vector3[] path, int subdivisions)
        {
            subdivisions = (subdivisions >= 0) ? subdivisions : 0;
            if (subdivisions == 0)
            {
                return path;
            }
            Vector3[] vectorArray = new Vector3[((path.Length - 1) * ((int) Mathf.Pow(2f, (float) subdivisions))) + 1];
            int index = 0;
            for (int i = 0; i < (path.Length - 1); i++)
            {
                float num3 = 1f / Mathf.Pow(2f, (float) subdivisions);
                for (float j = 0f; j < 1f; j += num3)
                {
                    vectorArray[index] = Vector3.Lerp(path[i], path[i + 1], Mathf.SmoothStep(0f, 1f, j));
                    index++;
                }
            }
            vectorArray[index] = path[path.Length - 1];
            return vectorArray;
        }

        [Obsolete("Use TriangleArea2 instead to avoid confusion regarding the factor 2")]
        public static long TriangleArea(Int3 a, Int3 b, Int3 c)
        {
            return TriangleArea2(a, b, c);
        }

        [Obsolete("Use TriangleArea2 instead to avoid confusion regarding the factor 2")]
        public static float TriangleArea(Vector3 a, Vector3 b, Vector3 c)
        {
            return TriangleArea2(a, b, c);
        }

        [Obsolete("Use VectorMath.SignedTriangleAreaTimes2XZ instead")]
        public static long TriangleArea2(Int3 a, Int3 b, Int3 c)
        {
            return VectorMath.SignedTriangleAreaTimes2XZ(a, b, c);
        }

        [Obsolete("Use VectorMath.SignedTriangleAreaTimes2XZ instead")]
        public static float TriangleArea2(Vector3 a, Vector3 b, Vector3 c)
        {
            return VectorMath.SignedTriangleAreaTimes2XZ(a, b, c);
        }
    }
}

