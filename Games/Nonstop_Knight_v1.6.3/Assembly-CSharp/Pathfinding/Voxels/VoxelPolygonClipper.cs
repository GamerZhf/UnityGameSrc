namespace Pathfinding.Voxels
{
    using Pathfinding;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct VoxelPolygonClipper
    {
        private float[] clipPolygonCache;
        private int[] clipPolygonIntCache;
        private void Init()
        {
            if (this.clipPolygonCache == null)
            {
                this.clipPolygonCache = new float[0x15];
                this.clipPolygonIntCache = new int[0x15];
            }
        }

        public int ClipPolygon(float[] vIn, int n, float[] vOut, float multi, float offset, int axis)
        {
            this.Init();
            float[] clipPolygonCache = this.clipPolygonCache;
            for (int i = 0; i < n; i++)
            {
                clipPolygonCache[i] = (multi * vIn[(i * 3) + axis]) + offset;
            }
            int num2 = 0;
            int index = 0;
            int num4 = n - 1;
            while (index < n)
            {
                bool flag = clipPolygonCache[num4] >= 0f;
                bool flag2 = clipPolygonCache[index] >= 0f;
                if (flag != flag2)
                {
                    int num5 = num2 * 3;
                    int num6 = index * 3;
                    int num7 = num4 * 3;
                    float num8 = clipPolygonCache[num4] / (clipPolygonCache[num4] - clipPolygonCache[index]);
                    vOut[num5] = vIn[num7] + ((vIn[num6] - vIn[num7]) * num8);
                    vOut[num5 + 1] = vIn[num7 + 1] + ((vIn[num6 + 1] - vIn[num7 + 1]) * num8);
                    vOut[num5 + 2] = vIn[num7 + 2] + ((vIn[num6 + 2] - vIn[num7 + 2]) * num8);
                    num2++;
                }
                if (flag2)
                {
                    int num9 = num2 * 3;
                    int num10 = index * 3;
                    vOut[num9] = vIn[num10];
                    vOut[num9 + 1] = vIn[num10 + 1];
                    vOut[num9 + 2] = vIn[num10 + 2];
                    num2++;
                }
                num4 = index;
                index++;
            }
            return num2;
        }

        public int ClipPolygonY(float[] vIn, int n, float[] vOut, float multi, float offset, int axis)
        {
            this.Init();
            float[] clipPolygonCache = this.clipPolygonCache;
            for (int i = 0; i < n; i++)
            {
                clipPolygonCache[i] = (multi * vIn[(i * 3) + axis]) + offset;
            }
            int num2 = 0;
            int index = 0;
            int num4 = n - 1;
            while (index < n)
            {
                bool flag = clipPolygonCache[num4] >= 0f;
                bool flag2 = clipPolygonCache[index] >= 0f;
                if (flag != flag2)
                {
                    vOut[(num2 * 3) + 1] = vIn[(num4 * 3) + 1] + ((vIn[(index * 3) + 1] - vIn[(num4 * 3) + 1]) * (clipPolygonCache[num4] / (clipPolygonCache[num4] - clipPolygonCache[index])));
                    num2++;
                }
                if (flag2)
                {
                    vOut[(num2 * 3) + 1] = vIn[(index * 3) + 1];
                    num2++;
                }
                num4 = index;
                index++;
            }
            return num2;
        }

        public int ClipPolygon(Int3[] vIn, int n, Int3[] vOut, int multi, int offset, int axis)
        {
            this.Init();
            int[] clipPolygonIntCache = this.clipPolygonIntCache;
            for (int i = 0; i < n; i++)
            {
                clipPolygonIntCache[i] = (multi * vIn[i][axis]) + offset;
            }
            int index = 0;
            int num3 = 0;
            int num4 = n - 1;
            while (num3 < n)
            {
                bool flag = clipPolygonIntCache[num4] >= 0;
                bool flag2 = clipPolygonIntCache[num3] >= 0;
                if (flag != flag2)
                {
                    double num5 = ((double) clipPolygonIntCache[num4]) / ((double) (clipPolygonIntCache[num4] - clipPolygonIntCache[num3]));
                    vOut[index] = vIn[num4] + ((Int3) ((vIn[num3] - vIn[num4]) * num5));
                    index++;
                }
                if (flag2)
                {
                    vOut[index] = vIn[num3];
                    index++;
                }
                num4 = num3;
                num3++;
            }
            return index;
        }
    }
}

