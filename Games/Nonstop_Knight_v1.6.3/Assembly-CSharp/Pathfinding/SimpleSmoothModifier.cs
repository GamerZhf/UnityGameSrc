namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable, AddComponentMenu("Pathfinding/Modifiers/Simple Smooth"), RequireComponent(typeof(Seeker)), HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_simple_smooth_modifier.php")]
    public class SimpleSmoothModifier : MonoModifier
    {
        [Tooltip("Length factor of the bezier curves' tangents")]
        public float bezierTangentLength = 0.4f;
        [Tooltip("How much to smooth the path. A higher value will give a smoother path, but might take the character far off the optimal path.")]
        public float factor = 0.1f;
        [Tooltip("Number of times to apply smoothing")]
        public int iterations = 2;
        [Tooltip("The length of each segment in the smoothed path. A high value yields rough paths and low value yields very smooth paths, but is slower")]
        public float maxSegmentLength = 2f;
        [Tooltip("Offset to apply in each smoothing iteration when using Offset Simple")]
        public float offset = 0.2f;
        public SmoothType smoothType;
        [Tooltip("Determines how much smoothing to apply in each smooth iteration. 0.5 usually produces the nicest looking curves")]
        public float strength = 0.5f;
        [Tooltip("The number of times to subdivide (divide in half) the path segments. [0...inf] (recommended [1...10])")]
        public int subdivisions = 2;
        [Tooltip("Toggle to divide all lines in equal length segments")]
        public bool uniformLength = true;

        public override void Apply(Path p)
        {
            if (p.vectorPath == null)
            {
                Debug.LogWarning("Can't process NULL path (has another modifier logged an error?)");
            }
            else
            {
                List<Vector3> list = null;
                switch (this.smoothType)
                {
                    case SmoothType.Simple:
                        list = this.SmoothSimple(p.vectorPath);
                        break;

                    case SmoothType.Bezier:
                        list = this.SmoothBezier(p.vectorPath);
                        break;

                    case SmoothType.OffsetSimple:
                        list = this.SmoothOffsetSimple(p.vectorPath);
                        break;

                    case SmoothType.CurvedNonuniform:
                        list = this.CurvedNonuniform(p.vectorPath);
                        break;
                }
                if (list != p.vectorPath)
                {
                    ListPool<Vector3>.Release(p.vectorPath);
                    p.vectorPath = list;
                }
            }
        }

        public List<Vector3> CurvedNonuniform(List<Vector3> path)
        {
            if (this.maxSegmentLength <= 0f)
            {
                Debug.LogWarning("Max Segment Length is <= 0 which would cause DivByZero-exception or other nasty errors (avoid this)");
                return path;
            }
            int capacity = 0;
            for (int i = 0; i < (path.Count - 1); i++)
            {
                Vector3 vector8 = path[i] - path[i + 1];
                float magnitude = vector8.magnitude;
                for (float k = 0f; k <= magnitude; k += this.maxSegmentLength)
                {
                    capacity++;
                }
            }
            List<Vector3> list = ListPool<Vector3>.Claim(capacity);
            Vector3 vector9 = path[1] - path[0];
            Vector3 normalized = vector9.normalized;
            for (int j = 0; j < (path.Count - 1); j++)
            {
                Vector3 vector10 = path[j] - path[j + 1];
                float num6 = vector10.magnitude;
                Vector3 vector2 = normalized;
                Vector3 vector3 = (j >= (path.Count - 2)) ? (path[j + 1] - path[j]).normalized : ((path[j + 2] - path[j + 1]).normalized - (path[j] - path[j + 1]).normalized).normalized;
                Vector3 vector4 = (Vector3) ((vector2 * num6) * this.factor);
                Vector3 vector5 = (Vector3) ((vector3 * num6) * this.factor);
                Vector3 a = path[j];
                Vector3 b = path[j + 1];
                float num7 = 1f / num6;
                for (float m = 0f; m <= num6; m += this.maxSegmentLength)
                {
                    float t = m * num7;
                    list.Add(GetPointOnCubic(a, b, vector4, vector5, t));
                }
                normalized = vector3;
            }
            list[list.Count - 1] = path[path.Count - 1];
            return list;
        }

        public static Vector3 GetPointOnCubic(Vector3 a, Vector3 b, Vector3 tan1, Vector3 tan2, float t)
        {
            float num = t * t;
            float num2 = num * t;
            float num3 = ((2f * num2) - (3f * num)) + 1f;
            float num4 = (-2f * num2) + (3f * num);
            float num5 = (num2 - (2f * num)) + t;
            float num6 = num2 - num;
            return (Vector3) ((((num3 * a) + (num4 * b)) + (num5 * tan1)) + (num6 * tan2));
        }

        public List<Vector3> SmoothBezier(List<Vector3> path)
        {
            if (this.subdivisions < 0)
            {
                this.subdivisions = 0;
            }
            int num = ((int) 1) << this.subdivisions;
            List<Vector3> list = ListPool<Vector3>.Claim();
            for (int i = 0; i < (path.Count - 1); i++)
            {
                Vector3 vector;
                Vector3 vector2;
                if (i == 0)
                {
                    vector = path[i + 1] - path[i];
                }
                else
                {
                    vector = path[i + 1] - path[i - 1];
                }
                if (i == (path.Count - 2))
                {
                    vector2 = path[i] - path[i + 1];
                }
                else
                {
                    vector2 = path[i] - path[i + 2];
                }
                vector = (Vector3) (vector * this.bezierTangentLength);
                vector2 = (Vector3) (vector2 * this.bezierTangentLength);
                Vector3 vector3 = path[i];
                Vector3 vector4 = vector3 + vector;
                Vector3 vector5 = path[i + 1];
                Vector3 vector6 = vector5 + vector2;
                for (int j = 0; j < num; j++)
                {
                    list.Add(AstarSplines.CubicBezier(vector3, vector4, vector6, vector5, ((float) j) / ((float) num)));
                }
            }
            list.Add(path[path.Count - 1]);
            return list;
        }

        public List<Vector3> SmoothOffsetSimple(List<Vector3> path)
        {
            if ((path.Count <= 2) || (this.iterations <= 0))
            {
                return path;
            }
            if (this.iterations > 12)
            {
                Debug.LogWarning("A very high iteration count was passed, won't let this one through");
                return path;
            }
            int capacity = ((path.Count - 2) * ((int) Mathf.Pow(2f, (float) this.iterations))) + 2;
            List<Vector3> list = ListPool<Vector3>.Claim(capacity);
            List<Vector3> list2 = ListPool<Vector3>.Claim(capacity);
            for (int i = 0; i < capacity; i++)
            {
                list.Add(Vector3.zero);
                list2.Add(Vector3.zero);
            }
            for (int j = 0; j < path.Count; j++)
            {
                list[j] = path[j];
            }
            for (int k = 0; k < this.iterations; k++)
            {
                int num5 = ((path.Count - 2) * ((int) Mathf.Pow(2f, (float) k))) + 2;
                List<Vector3> list3 = list;
                list = list2;
                list2 = list3;
                for (int m = 0; m < (num5 - 1); m++)
                {
                    Vector3 a = list2[m];
                    Vector3 b = list2[m + 1];
                    Vector3 normalized = Vector3.Cross(b - a, Vector3.up).normalized;
                    bool flag = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    bool flag4 = false;
                    if ((m != 0) && !VectorMath.IsColinearXZ(a, b, list2[m - 1]))
                    {
                        flag3 = true;
                        flag = VectorMath.RightOrColinearXZ(a, b, list2[m - 1]);
                    }
                    if ((m < (num5 - 1)) && !VectorMath.IsColinearXZ(a, b, list2[m + 2]))
                    {
                        flag4 = true;
                        flag2 = VectorMath.RightOrColinearXZ(a, b, list2[m + 2]);
                    }
                    if (flag3)
                    {
                        list[m * 2] = a + (!flag ? ((Vector3) ((-normalized * this.offset) * 1f)) : ((Vector3) ((normalized * this.offset) * 1f)));
                    }
                    else
                    {
                        list[m * 2] = a;
                    }
                    if (flag4)
                    {
                        list[(m * 2) + 1] = b + (!flag2 ? ((Vector3) ((-normalized * this.offset) * 1f)) : ((Vector3) ((normalized * this.offset) * 1f)));
                    }
                    else
                    {
                        list[(m * 2) + 1] = b;
                    }
                }
                list[(((path.Count - 2) * ((int) Mathf.Pow(2f, (float) (k + 1)))) + 2) - 1] = list2[num5 - 1];
            }
            ListPool<Vector3>.Release(list2);
            return list;
        }

        public List<Vector3> SmoothSimple(List<Vector3> path)
        {
            List<Vector3> list;
            if (path.Count < 2)
            {
                return path;
            }
            if (this.uniformLength)
            {
                this.maxSegmentLength = Mathf.Max(this.maxSegmentLength, 0.005f);
                float num = 0f;
                for (int i = 0; i < (path.Count - 1); i++)
                {
                    num += Vector3.Distance(path[i], path[i + 1]);
                }
                list = ListPool<Vector3>.Claim(Mathf.FloorToInt(num / this.maxSegmentLength) + 2);
                float num4 = 0f;
                for (int j = 0; j < (path.Count - 1); j++)
                {
                    Vector3 a = path[j];
                    Vector3 b = path[j + 1];
                    float num6 = Vector3.Distance(a, b);
                    while (num4 < num6)
                    {
                        list.Add(Vector3.Lerp(a, b, num4 / num6));
                        num4 += this.maxSegmentLength;
                    }
                    num4 -= num6;
                }
            }
            else
            {
                this.subdivisions = Mathf.Max(this.subdivisions, 0);
                if (this.subdivisions > 10)
                {
                    Debug.LogWarning("Very large number of subdivisions. Cowardly refusing to subdivide every segment into more than " + (((int) 1) << this.subdivisions) + " subsegments");
                    this.subdivisions = 10;
                }
                int num7 = ((int) 1) << this.subdivisions;
                list = ListPool<Vector3>.Claim(((path.Count - 1) * num7) + 1);
                for (int k = 0; k < (path.Count - 1); k++)
                {
                    for (int m = 0; m < num7; m++)
                    {
                        list.Add(Vector3.Lerp(path[k], path[k + 1], ((float) m) / ((float) num7)));
                    }
                }
            }
            list.Add(path[path.Count - 1]);
            if (this.strength > 0f)
            {
                for (int n = 0; n < this.iterations; n++)
                {
                    Vector3 vector3 = list[0];
                    for (int num11 = 1; num11 < (list.Count - 1); num11++)
                    {
                        Vector3 from = list[num11];
                        list[num11] = Vector3.Lerp(from, (Vector3) ((vector3 + list[num11 + 1]) / 2f), this.strength);
                        vector3 = from;
                    }
                }
            }
            return list;
        }

        public override int Order
        {
            get
            {
                return 50;
            }
        }

        public enum SmoothType
        {
            Simple,
            Bezier,
            OffsetSimple,
            CurvedNonuniform
        }
    }
}

