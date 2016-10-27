namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class VectorMath
    {
        public static Vector3 ClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 rhs = Vector3.Normalize(lineEnd - lineStart);
            float num = Vector3.Dot(point - lineStart, rhs);
            return (lineStart + ((Vector3) (num * rhs)));
        }

        public static float ClosestPointOnLineFactor(Int2 lineStart, Int2 lineEnd, Int2 point)
        {
            Int2 b = lineEnd - lineStart;
            double sqrMagnitudeLong = b.sqrMagnitudeLong;
            double num3 = Int2.DotLong(point - lineStart, b);
            if (sqrMagnitudeLong != 0.0)
            {
                num3 /= sqrMagnitudeLong;
            }
            return (float) num3;
        }

        public static float ClosestPointOnLineFactor(Int3 lineStart, Int3 lineEnd, Int3 point)
        {
            Int3 rhs = lineEnd - lineStart;
            float sqrMagnitude = rhs.sqrMagnitude;
            float num3 = Int3.Dot(point - lineStart, rhs);
            if (sqrMagnitude != 0f)
            {
                num3 /= sqrMagnitude;
            }
            return num3;
        }

        public static float ClosestPointOnLineFactor(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 rhs = lineEnd - lineStart;
            float sqrMagnitude = rhs.sqrMagnitude;
            if (sqrMagnitude <= 1E-06)
            {
                return 0f;
            }
            return (Vector3.Dot(point - lineStart, rhs) / sqrMagnitude);
        }

        public static Vector3 ClosestPointOnSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 rhs = lineEnd - lineStart;
            float sqrMagnitude = rhs.sqrMagnitude;
            if (sqrMagnitude <= 1E-06)
            {
                return lineStart;
            }
            float num2 = Vector3.Dot(point - lineStart, rhs) / sqrMagnitude;
            return (lineStart + ((Vector3) (Mathf.Clamp01(num2) * rhs)));
        }

        public static Vector3 ClosestPointOnSegmentXZ(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            lineStart.y = point.y;
            lineEnd.y = point.y;
            Vector3 vector2 = lineEnd - lineStart;
            vector2.y = 0f;
            float magnitude = vector2.magnitude;
            Vector3 rhs = (magnitude <= float.Epsilon) ? Vector3.zero : ((Vector3) (vector2 / magnitude));
            float num2 = Vector3.Dot(point - lineStart, rhs);
            return (lineStart + ((Vector3) (Mathf.Clamp(num2, 0f, vector2.magnitude) * rhs)));
        }

        public static bool IsClockwiseMarginXZ(Vector3 a, Vector3 b, Vector3 c)
        {
            return ((((b.x - a.x) * (c.z - a.z)) - ((c.x - a.x) * (b.z - a.z))) <= float.Epsilon);
        }

        public static bool IsClockwiseOrColinear(Int2 a, Int2 b, Int2 c)
        {
            return RightOrColinear(a, b, c);
        }

        public static bool IsClockwiseOrColinearXZ(Int3 a, Int3 b, Int3 c)
        {
            return RightOrColinearXZ(a, b, c);
        }

        public static bool IsClockwiseXZ(Int3 a, Int3 b, Int3 c)
        {
            return RightXZ(a, b, c);
        }

        public static bool IsClockwiseXZ(Vector3 a, Vector3 b, Vector3 c)
        {
            return ((((b.x - a.x) * (c.z - a.z)) - ((c.x - a.x) * (b.z - a.z))) < 0f);
        }

        public static bool IsColinearAlmostXZ(Int3 a, Int3 b, Int3 c)
        {
            long num = ((b.x - a.x) * (c.z - a.z)) - ((c.x - a.x) * (b.z - a.z));
            return ((num > -1L) && (num < 1L));
        }

        public static bool IsColinearXZ(Int3 a, Int3 b, Int3 c)
        {
            return ((((b.x - a.x) * (c.z - a.z)) - ((c.x - a.x) * (b.z - a.z))) == 0L);
        }

        public static bool IsColinearXZ(Vector3 a, Vector3 b, Vector3 c)
        {
            float num = ((b.x - a.x) * (c.z - a.z)) - ((c.x - a.x) * (b.z - a.z));
            return ((num <= 1E-07f) && (num >= -1E-07f));
        }

        public static Vector3 LineDirIntersectionPointXZ(Vector3 start1, Vector3 dir1, Vector3 start2, Vector3 dir2)
        {
            float num = (dir2.z * dir1.x) - (dir2.x * dir1.z);
            if (num == 0f)
            {
                return start1;
            }
            float num2 = (dir2.x * (start1.z - start2.z)) - (dir2.z * (start1.x - start2.x));
            float num3 = num2 / num;
            return (start1 + ((Vector3) (dir1 * num3)));
        }

        public static Vector3 LineDirIntersectionPointXZ(Vector3 start1, Vector3 dir1, Vector3 start2, Vector3 dir2, out bool intersects)
        {
            float num = (dir2.z * dir1.x) - (dir2.x * dir1.z);
            if (num == 0f)
            {
                intersects = false;
                return start1;
            }
            float num2 = (dir2.x * (start1.z - start2.z)) - (dir2.z * (start1.x - start2.x));
            float num3 = num2 / num;
            intersects = true;
            return (start1 + ((Vector3) (dir1 * num3)));
        }

        public static float LineIntersectionFactorXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
        {
            Vector3 vector = end1 - start1;
            Vector3 vector2 = end2 - start2;
            float num = (vector2.z * vector.x) - (vector2.x * vector.z);
            if (num == 0f)
            {
                return -1f;
            }
            float num2 = (vector2.x * (start1.z - start2.z)) - (vector2.z * (start1.x - start2.x));
            return (num2 / num);
        }

        public static bool LineIntersectionFactorXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2, out float factor1, out float factor2)
        {
            Int3 num = end1 - start1;
            Int3 num2 = end2 - start2;
            long num3 = (num2.z * num.x) - (num2.x * num.z);
            if (num3 == 0)
            {
                factor1 = 0f;
                factor2 = 0f;
                return false;
            }
            long num4 = (num2.x * (start1.z - start2.z)) - (num2.z * (start1.x - start2.x));
            long num5 = (num.x * (start1.z - start2.z)) - (num.z * (start1.x - start2.x));
            factor1 = ((float) num4) / ((float) num3);
            factor2 = ((float) num5) / ((float) num3);
            return true;
        }

        public static bool LineIntersectionFactorXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out float factor1, out float factor2)
        {
            Vector3 vector = end1 - start1;
            Vector3 vector2 = end2 - start2;
            float num = (vector2.z * vector.x) - (vector2.x * vector.z);
            if ((num <= 1E-05f) && (num >= -1E-05f))
            {
                factor1 = 0f;
                factor2 = 0f;
                return false;
            }
            float num2 = (vector2.x * (start1.z - start2.z)) - (vector2.z * (start1.x - start2.x));
            float num3 = (vector.x * (start1.z - start2.z)) - (vector.z * (start1.x - start2.x));
            float num4 = num2 / num;
            float num5 = num3 / num;
            factor1 = num4;
            factor2 = num5;
            return true;
        }

        public static Vector2 LineIntersectionPoint(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
        {
            bool flag;
            return LineIntersectionPoint(start1, end1, start2, end2, out flag);
        }

        public static Vector2 LineIntersectionPoint(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out bool intersects)
        {
            Vector2 vector = end1 - start1;
            Vector2 vector2 = end2 - start2;
            float num = (vector2.y * vector.x) - (vector2.x * vector.y);
            if (num == 0f)
            {
                intersects = false;
                return start1;
            }
            float num2 = (vector2.x * (start1.y - start2.y)) - (vector2.y * (start1.x - start2.x));
            float num3 = num2 / num;
            intersects = true;
            return (start1 + ((Vector2) (vector * num3)));
        }

        public static Vector3 LineIntersectionPointXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
        {
            bool flag;
            return LineIntersectionPointXZ(start1, end1, start2, end2, out flag);
        }

        public static Vector3 LineIntersectionPointXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out bool intersects)
        {
            Vector3 vector = end1 - start1;
            Vector3 vector2 = end2 - start2;
            float num = (vector2.z * vector.x) - (vector2.x * vector.z);
            if (num == 0f)
            {
                intersects = false;
                return start1;
            }
            float num2 = (vector2.x * (start1.z - start2.z)) - (vector2.z * (start1.x - start2.x));
            float num3 = num2 / num;
            intersects = true;
            return (start1 + ((Vector3) (vector * num3)));
        }

        public static float LineRayIntersectionFactorXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
        {
            Int3 num = end1 - start1;
            Int3 num2 = end2 - start2;
            int num3 = (num2.z * num.x) - (num2.x * num.z);
            if (num3 == 0)
            {
                return float.NaN;
            }
            int num4 = (num2.x * (start1.z - start2.z)) - (num2.z * (start1.x - start2.x));
            int num5 = (num.x * (start1.z - start2.z)) - (num.z * (start1.x - start2.x));
            if ((((float) num5) / ((float) num3)) < 0f)
            {
                return float.NaN;
            }
            return (((float) num4) / ((float) num3));
        }

        public static bool RaySegmentIntersectXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
        {
            Int3 num = end1 - start1;
            Int3 num2 = end2 - start2;
            long num3 = (num2.z * num.x) - (num2.x * num.z);
            if (num3 == 0)
            {
                return false;
            }
            long num4 = (num2.x * (start1.z - start2.z)) - (num2.z * (start1.x - start2.x));
            long num5 = (num.x * (start1.z - start2.z)) - (num.z * (start1.x - start2.x));
            if (!((num4 < 0L) ^ (num3 < 0L)))
            {
                return false;
            }
            if (!((num5 < 0L) ^ (num3 < 0L)))
            {
                return false;
            }
            return (((num3 < 0L) || (num5 <= num3)) && ((num3 >= 0L) || (num5 > num3)));
        }

        public static bool ReversesFaceOrientations(Matrix4x4 matrix)
        {
            Vector3 lhs = matrix.MultiplyVector(new Vector3(1f, 0f, 0f));
            Vector3 rhs = matrix.MultiplyVector(new Vector3(0f, 1f, 0f));
            Vector3 vector3 = matrix.MultiplyVector(new Vector3(0f, 0f, 1f));
            return (Vector3.Dot(Vector3.Cross(lhs, rhs), vector3) < 0f);
        }

        public static bool ReversesFaceOrientationsXZ(Matrix4x4 matrix)
        {
            Vector3 vector = matrix.MultiplyVector(new Vector3(1f, 0f, 0f));
            Vector3 vector2 = matrix.MultiplyVector(new Vector3(0f, 0f, 1f));
            float num = (vector.x * vector2.z) - (vector2.x * vector.z);
            return (num < 0f);
        }

        public static bool RightOrColinear(Int2 a, Int2 b, Int2 p)
        {
            return ((((b.x - a.x) * (p.y - a.y)) - ((p.x - a.x) * (b.y - a.y))) <= 0L);
        }

        public static bool RightOrColinear(Vector2 a, Vector2 b, Vector2 p)
        {
            return ((((b.x - a.x) * (p.y - a.y)) - ((p.x - a.x) * (b.y - a.y))) <= 0f);
        }

        public static bool RightOrColinearXZ(Int3 a, Int3 b, Int3 p)
        {
            return ((((b.x - a.x) * (p.z - a.z)) - ((p.x - a.x) * (b.z - a.z))) <= 0L);
        }

        public static bool RightOrColinearXZ(Vector3 a, Vector3 b, Vector3 p)
        {
            return ((((b.x - a.x) * (p.z - a.z)) - ((p.x - a.x) * (b.z - a.z))) <= 0f);
        }

        public static bool RightXZ(Int3 a, Int3 b, Int3 p)
        {
            return ((((b.x - a.x) * (p.z - a.z)) - ((p.x - a.x) * (b.z - a.z))) < 0L);
        }

        public static bool RightXZ(Vector3 a, Vector3 b, Vector3 p)
        {
            return ((((b.x - a.x) * (p.z - a.z)) - ((p.x - a.x) * (b.z - a.z))) < -1.401298E-45f);
        }

        public static Vector3 SegmentIntersectionPointXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out bool intersects)
        {
            Vector3 vector = end1 - start1;
            Vector3 vector2 = end2 - start2;
            float num = (vector2.z * vector.x) - (vector2.x * vector.z);
            if (num == 0f)
            {
                intersects = false;
                return start1;
            }
            float num2 = (vector2.x * (start1.z - start2.z)) - (vector2.z * (start1.x - start2.x));
            float num3 = (vector.x * (start1.z - start2.z)) - (vector.z * (start1.x - start2.x));
            float num4 = num2 / num;
            float num5 = num3 / num;
            if (((num4 < 0f) || (num4 > 1f)) || ((num5 < 0f) || (num5 > 1f)))
            {
                intersects = false;
                return start1;
            }
            intersects = true;
            return (start1 + ((Vector3) (vector * num4)));
        }

        public static bool SegmentIntersectsBounds(Bounds bounds, Vector3 a, Vector3 b)
        {
            a -= bounds.center;
            b -= bounds.center;
            Vector3 vector = (Vector3) ((a + b) * 0.5f);
            Vector3 vector2 = a - vector;
            float x = Math.Abs(vector2.x);
            float y = Math.Abs(vector2.y);
            Vector3 vector3 = new Vector3(x, y, Math.Abs(vector2.z));
            Vector3 extents = bounds.extents;
            if (Math.Abs(vector.x) > (extents.x + vector3.x))
            {
                return false;
            }
            if (Math.Abs(vector.y) > (extents.y + vector3.y))
            {
                return false;
            }
            if (Math.Abs(vector.z) > (extents.z + vector3.z))
            {
                return false;
            }
            if (Math.Abs((float) ((vector.y * vector2.z) - (vector.z * vector2.y))) > ((extents.y * vector3.z) + (extents.z * vector3.y)))
            {
                return false;
            }
            if (Math.Abs((float) ((vector.x * vector2.z) - (vector.z * vector2.x))) > ((extents.x * vector3.z) + (extents.z * vector3.x)))
            {
                return false;
            }
            if (Math.Abs((float) ((vector.x * vector2.y) - (vector.y * vector2.x))) > ((extents.x * vector3.y) + (extents.y * vector3.x)))
            {
                return false;
            }
            return true;
        }

        public static bool SegmentsIntersect(Int2 start1, Int2 end1, Int2 start2, Int2 end2)
        {
            return ((RightOrColinear(start1, end1, start2) != RightOrColinear(start1, end1, end2)) && (RightOrColinear(start2, end2, start1) != RightOrColinear(start2, end2, end1)));
        }

        public static bool SegmentsIntersectXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
        {
            return ((RightOrColinearXZ(start1, end1, start2) != RightOrColinearXZ(start1, end1, end2)) && (RightOrColinearXZ(start2, end2, start1) != RightOrColinearXZ(start2, end2, end1)));
        }

        public static bool SegmentsIntersectXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
        {
            Vector3 vector = end1 - start1;
            Vector3 vector2 = end2 - start2;
            float num = (vector2.z * vector.x) - (vector2.x * vector.z);
            if (num == 0f)
            {
                return false;
            }
            float num2 = (vector2.x * (start1.z - start2.z)) - (vector2.z * (start1.x - start2.x));
            float num3 = (vector.x * (start1.z - start2.z)) - (vector.z * (start1.x - start2.x));
            float num4 = num2 / num;
            float num5 = num3 / num;
            return (((num4 >= 0f) && (num4 <= 1f)) && ((num5 >= 0f) && (num5 <= 1f)));
        }

        public static long SignedTriangleAreaTimes2XZ(Int3 a, Int3 b, Int3 c)
        {
            return (((b.x - a.x) * (c.z - a.z)) - ((c.x - a.x) * (b.z - a.z)));
        }

        public static float SignedTriangleAreaTimes2XZ(Vector3 a, Vector3 b, Vector3 c)
        {
            return (((b.x - a.x) * (c.z - a.z)) - ((c.x - a.x) * (b.z - a.z)));
        }

        public static float SqrDistancePointSegment(Vector3 a, Vector3 b, Vector3 p)
        {
            Vector3 vector2 = ClosestPointOnSegment(a, b, p) - p;
            return vector2.sqrMagnitude;
        }

        public static float SqrDistancePointSegmentApproximate(Int3 a, Int3 b, Int3 p)
        {
            float num = b.x - a.x;
            float num2 = b.z - a.z;
            float num3 = p.x - a.x;
            float num4 = p.z - a.z;
            float num5 = (num * num) + (num2 * num2);
            float num6 = (num * num3) + (num2 * num4);
            if (num5 > 0f)
            {
                num6 /= num5;
            }
            if (num6 < 0f)
            {
                num6 = 0f;
            }
            else if (num6 > 1f)
            {
                num6 = 1f;
            }
            num3 = (a.x + (num6 * num)) - p.x;
            num4 = (a.z + (num6 * num2)) - p.z;
            return ((num3 * num3) + (num4 * num4));
        }

        public static float SqrDistancePointSegmentApproximate(int x, int z, int px, int pz, int qx, int qz)
        {
            float num = qx - px;
            float num2 = qz - pz;
            float num3 = x - px;
            float num4 = z - pz;
            float num5 = (num * num) + (num2 * num2);
            float num6 = (num * num3) + (num2 * num4);
            if (num5 > 0f)
            {
                num6 /= num5;
            }
            if (num6 < 0f)
            {
                num6 = 0f;
            }
            else if (num6 > 1f)
            {
                num6 = 1f;
            }
            num3 = (px + (num6 * num)) - x;
            num4 = (pz + (num6 * num2)) - z;
            return ((num3 * num3) + (num4 * num4));
        }

        public static float SqrDistanceSegmentSegment(Vector3 s1, Vector3 e1, Vector3 s2, Vector3 e2)
        {
            float num8;
            float num11;
            Vector3 lhs = e1 - s1;
            Vector3 rhs = e2 - s2;
            Vector3 vector3 = s1 - s2;
            float num = Vector3.Dot(lhs, lhs);
            float num2 = Vector3.Dot(lhs, rhs);
            float num3 = Vector3.Dot(rhs, rhs);
            float num4 = Vector3.Dot(lhs, vector3);
            float num5 = Vector3.Dot(rhs, vector3);
            float num6 = (num * num3) - (num2 * num2);
            float num9 = num6;
            float num12 = num6;
            if (num6 < 1E-06f)
            {
                num8 = 0f;
                num9 = 1f;
                num11 = num5;
                num12 = num3;
            }
            else
            {
                num8 = (num2 * num5) - (num3 * num4);
                num11 = (num * num5) - (num2 * num4);
                if (num8 < 0f)
                {
                    num8 = 0f;
                    num11 = num5;
                    num12 = num3;
                }
                else if (num8 > num9)
                {
                    num8 = num9;
                    num11 = num5 + num2;
                    num12 = num3;
                }
            }
            if (num11 < 0f)
            {
                num11 = 0f;
                if (-num4 < 0f)
                {
                    num8 = 0f;
                }
                else if (-num4 > num)
                {
                    num8 = num9;
                }
                else
                {
                    num8 = -num4;
                    num9 = num;
                }
            }
            else if (num11 > num12)
            {
                num11 = num12;
                if ((-num4 + num2) < 0f)
                {
                    num8 = 0f;
                }
                else if ((-num4 + num2) > num)
                {
                    num8 = num9;
                }
                else
                {
                    num8 = -num4 + num2;
                    num9 = num;
                }
            }
            float num7 = (Math.Abs(num8) >= 1E-06f) ? (num8 / num9) : 0f;
            float num10 = (Math.Abs(num11) >= 1E-06f) ? (num11 / num12) : 0f;
            Vector3 vector4 = (Vector3) ((vector3 + (num7 * lhs)) - (num10 * rhs));
            return vector4.sqrMagnitude;
        }

        public static float SqrDistanceXZ(Vector3 a, Vector3 b)
        {
            Vector3 vector = a - b;
            return ((vector.x * vector.x) + (vector.z * vector.z));
        }
    }
}

