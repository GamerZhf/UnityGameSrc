namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_radius_modifier.php"), AddComponentMenu("Pathfinding/Modifiers/Radius Offset")]
    public class RadiusModifier : MonoModifier
    {
        private float[] a1 = new float[10];
        private float[] a2 = new float[10];
        public float detail = 10f;
        private bool[] dir = new bool[10];
        private float[] radi = new float[10];
        public float radius = 1f;

        public override void Apply(Path p)
        {
            List<Vector3> vectorPath = p.vectorPath;
            List<Vector3> list2 = this.Apply(vectorPath);
            if (list2 != vectorPath)
            {
                ListPool<Vector3>.Release(p.vectorPath);
                p.vectorPath = list2;
            }
        }

        public List<Vector3> Apply(List<Vector3> vs)
        {
            if ((vs == null) || (vs.Count < 3))
            {
                return vs;
            }
            if (this.radi.Length < vs.Count)
            {
                this.radi = new float[vs.Count];
                this.a1 = new float[vs.Count];
                this.a2 = new float[vs.Count];
                this.dir = new bool[vs.Count];
            }
            for (int i = 0; i < vs.Count; i++)
            {
                this.radi[i] = this.radius;
            }
            this.radi[0] = 0f;
            this.radi[vs.Count - 1] = 0f;
            int num2 = 0;
            for (int j = 0; j < (vs.Count - 1); j++)
            {
                TangentType type;
                num2++;
                if (num2 > (2 * vs.Count))
                {
                    Debug.LogWarning("Could not resolve radiuses, the path is too complex. Try reducing the base radius");
                    break;
                }
                if (j == 0)
                {
                    type = this.CalculateTangentTypeSimple(vs[j], vs[j + 1], vs[j + 2]);
                }
                else if (j == (vs.Count - 2))
                {
                    type = this.CalculateTangentTypeSimple(vs[j - 1], vs[j], vs[j + 1]);
                }
                else
                {
                    type = this.CalculateTangentType(vs[j - 1], vs[j], vs[j + 1], vs[j + 2]);
                }
                if ((type & TangentType.Inner) != 0)
                {
                    float num4;
                    float num5;
                    if (!this.CalculateCircleInner(vs[j], vs[j + 1], this.radi[j], this.radi[j + 1], out num4, out num5))
                    {
                        Vector3 vector = vs[j + 1] - vs[j];
                        float magnitude = vector.magnitude;
                        this.radi[j] = magnitude * (this.radi[j] / (this.radi[j] + this.radi[j + 1]));
                        this.radi[j + 1] = magnitude - this.radi[j];
                        this.radi[j] *= 0.99f;
                        this.radi[j + 1] *= 0.99f;
                        j -= 2;
                    }
                    else if (type == TangentType.InnerRightLeft)
                    {
                        this.a2[j] = num5 - num4;
                        this.a1[j + 1] = (num5 - num4) + 3.141593f;
                        this.dir[j] = true;
                    }
                    else
                    {
                        this.a2[j] = num5 + num4;
                        this.a1[j + 1] = (num5 + num4) + 3.141593f;
                        this.dir[j] = false;
                    }
                }
                else
                {
                    float num7;
                    float num8;
                    if (!this.CalculateCircleOuter(vs[j], vs[j + 1], this.radi[j], this.radi[j + 1], out num8, out num7))
                    {
                        if (j == (vs.Count - 2))
                        {
                            Vector3 vector2 = vs[j + 1] - vs[j];
                            this.radi[j] = vector2.magnitude;
                            this.radi[j] *= 0.99f;
                            j--;
                        }
                        else
                        {
                            if (this.radi[j] > this.radi[j + 1])
                            {
                                Vector3 vector3 = vs[j + 1] - vs[j];
                                this.radi[j + 1] = this.radi[j] - vector3.magnitude;
                            }
                            else
                            {
                                Vector3 vector4 = vs[j + 1] - vs[j];
                                this.radi[j + 1] = this.radi[j] + vector4.magnitude;
                            }
                            this.radi[j + 1] *= 0.99f;
                        }
                        j--;
                    }
                    else if (type == TangentType.OuterRight)
                    {
                        this.a2[j] = num7 - num8;
                        this.a1[j + 1] = num7 - num8;
                        this.dir[j] = true;
                    }
                    else
                    {
                        this.a2[j] = num7 + num8;
                        this.a1[j + 1] = num7 + num8;
                        this.dir[j] = false;
                    }
                }
            }
            List<Vector3> list = ListPool<Vector3>.Claim();
            list.Add(vs[0]);
            if (this.detail < 1f)
            {
                this.detail = 1f;
            }
            float num9 = 6.283185f / this.detail;
            for (int k = 1; k < (vs.Count - 1); k++)
            {
                float num11 = this.a1[k];
                float num12 = this.a2[k];
                float num13 = this.radi[k];
                if (this.dir[k])
                {
                    if (num12 < num11)
                    {
                        num12 += 6.283185f;
                    }
                    for (float m = num11; m < num12; m += num9)
                    {
                        list.Add(((Vector3) (new Vector3((float) Math.Cos((double) m), 0f, (float) Math.Sin((double) m)) * num13)) + vs[k]);
                    }
                }
                else
                {
                    if (num11 < num12)
                    {
                        num11 += 6.283185f;
                    }
                    for (float n = num11; n > num12; n -= num9)
                    {
                        list.Add(((Vector3) (new Vector3((float) Math.Cos((double) n), 0f, (float) Math.Sin((double) n)) * num13)) + vs[k]);
                    }
                }
            }
            list.Add(vs[vs.Count - 1]);
            return list;
        }

        private bool CalculateCircleInner(Vector3 p1, Vector3 p2, float r1, float r2, out float a, out float sigma)
        {
            Vector3 vector = p1 - p2;
            float magnitude = vector.magnitude;
            if ((r1 + r2) > magnitude)
            {
                a = 0f;
                sigma = 0f;
                return false;
            }
            a = (float) Math.Acos((double) ((r1 + r2) / magnitude));
            sigma = (float) Math.Atan2((double) (p2.z - p1.z), (double) (p2.x - p1.x));
            return true;
        }

        private bool CalculateCircleOuter(Vector3 p1, Vector3 p2, float r1, float r2, out float a, out float sigma)
        {
            Vector3 vector = p1 - p2;
            float magnitude = vector.magnitude;
            if (Math.Abs((float) (r1 - r2)) > magnitude)
            {
                a = 0f;
                sigma = 0f;
                return false;
            }
            a = (float) Math.Acos((double) ((r1 - r2) / magnitude));
            sigma = (float) Math.Atan2((double) (p2.z - p1.z), (double) (p2.x - p1.x));
            return true;
        }

        private TangentType CalculateTangentType(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            bool flag = VectorMath.RightOrColinearXZ(p1, p2, p3);
            bool flag2 = VectorMath.RightOrColinearXZ(p2, p3, p4);
            return (TangentType) (((int) 1) << (((flag == null) ? 0 : 2) + ((flag2 == null) ? 0 : 1)));
        }

        private TangentType CalculateTangentTypeSimple(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            bool flag = VectorMath.RightOrColinearXZ(p1, p2, p3);
            bool flag2 = flag;
            return (TangentType) (((int) 1) << (((flag2 == null) ? 0 : 2) + ((flag == null) ? 0 : 1)));
        }

        private void DrawCircleSegment(Vector3 p1, float rad, Color col, [Optional, DefaultParameterValue(0)] float start, [Optional, DefaultParameterValue(6.283185f)] float end)
        {
            Vector3 vector = ((Vector3) (new Vector3((float) Math.Cos((double) start), 0f, (float) Math.Sin((double) start)) * rad)) + p1;
            for (float i = start; i < end; i += 0.03141593f)
            {
                Vector3 vector2 = ((Vector3) (new Vector3((float) Math.Cos((double) i), 0f, (float) Math.Sin((double) i)) * rad)) + p1;
                Debug.DrawLine(vector, vector2, col);
                vector = vector2;
            }
            if (end == 6.2831853071795862)
            {
                Vector3 vector3 = ((Vector3) (new Vector3((float) Math.Cos((double) start), 0f, (float) Math.Sin((double) start)) * rad)) + p1;
                Debug.DrawLine(vector, vector3, col);
            }
        }

        public override int Order
        {
            get
            {
                return 0x29;
            }
        }

        [Flags]
        private enum TangentType
        {
            Inner = 6,
            InnerLeftRight = 4,
            InnerRightLeft = 2,
            Outer = 9,
            OuterLeft = 8,
            OuterRight = 1
        }
    }
}

