namespace Pathfinding.Voxels
{
    using Pathfinding;
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Utility
    {
        public static void CopyVector(float[] a, int i, Vector3 v)
        {
            a[i] = v.x;
            a[i + 1] = v.y;
            a[i + 2] = v.z;
        }

        public static float Max(float a, float b, float c)
        {
            a = (a <= b) ? b : a;
            return ((a <= c) ? c : a);
        }

        public static int Max(int a, int b, int c, int d)
        {
            a = (a <= b) ? b : a;
            a = (a <= c) ? c : a;
            return ((a <= d) ? d : a);
        }

        public static float Max(float a, float b, float c, float d)
        {
            a = (a <= b) ? b : a;
            a = (a <= c) ? c : a;
            return ((a <= d) ? d : a);
        }

        public static float Min(float a, float b, float c)
        {
            a = (a >= b) ? b : a;
            return ((a >= c) ? c : a);
        }

        public static int Min(int a, int b, int c, int d)
        {
            a = (a >= b) ? b : a;
            a = (a >= c) ? c : a;
            return ((a >= d) ? d : a);
        }

        public static float Min(float a, float b, float c, float d)
        {
            a = (a >= b) ? b : a;
            a = (a >= c) ? c : a;
            return ((a >= d) ? d : a);
        }

        public static Int3[] RemoveDuplicateVertices(Int3[] vertices, int[] triangles)
        {
            Dictionary<Int3, int> dictionary = ObjectPoolSimple<Dictionary<Int3, int>>.Claim();
            dictionary.Clear();
            int[] numArray = new int[vertices.Length];
            int num = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (!dictionary.ContainsKey(vertices[i]))
                {
                    dictionary.Add(vertices[i], num);
                    numArray[i] = num;
                    vertices[num] = vertices[i];
                    num++;
                }
                else
                {
                    numArray[i] = dictionary[vertices[i]];
                }
            }
            dictionary.Clear();
            ObjectPoolSimple<Dictionary<Int3, int>>.Release(ref dictionary);
            for (int j = 0; j < triangles.Length; j++)
            {
                triangles[j] = numArray[triangles[j]];
            }
            Int3[] numArray2 = new Int3[num];
            for (int k = 0; k < num; k++)
            {
                numArray2[k] = vertices[k];
            }
            return numArray2;
        }

        public static void Swap(ref int a, ref int b)
        {
            int num = a;
            a = b;
            b = num;
        }
    }
}

