namespace PlayerView
{
    using System;
    using System.Threading;
    using UnityEngine;

    public class TextureScale
    {
        private static int finishCount;
        private static Mutex mutex;
        private static Color[] newColors;
        private static float ratioX;
        private static float ratioY;
        private static Color[] texColors;
        private static int w;
        private static int w2;

        public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
        {
            ThreadedScale(tex, newWidth, newHeight, true);
        }

        public static void BilinearScale(object obj)
        {
            ThreadData data = (ThreadData) obj;
            for (int i = data.start; i < data.end; i++)
            {
                int num2 = (int) Mathf.Floor(i * ratioY);
                int num3 = num2 * w;
                int num4 = (num2 + 1) * w;
                int num5 = i * w2;
                for (int j = 0; j < w2; j++)
                {
                    int num7 = (int) Mathf.Floor(j * ratioX);
                    float num8 = (j * ratioX) - num7;
                    newColors[num5 + j] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[num3 + num7], texColors[(num3 + num7) + 1], num8), ColorLerpUnclamped(texColors[num4 + num7], texColors[(num4 + num7) + 1], num8), (i * ratioY) - num2);
                }
            }
            mutex.WaitOne();
            finishCount++;
            mutex.ReleaseMutex();
        }

        private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
        {
            return new Color(c1.r + ((c2.r - c1.r) * value), c1.g + ((c2.g - c1.g) * value), c1.b + ((c2.b - c1.b) * value), c1.a + ((c2.a - c1.a) * value));
        }

        public static void Point(Texture2D tex, int newWidth, int newHeight)
        {
            ThreadedScale(tex, newWidth, newHeight, false);
        }

        public static void PointScale(object obj)
        {
            ThreadData data = (ThreadData) obj;
            for (int i = data.start; i < data.end; i++)
            {
                int num2 = ((int) (ratioY * i)) * w;
                int num3 = i * w2;
                for (int j = 0; j < w2; j++)
                {
                    newColors[num3 + j] = texColors[num2 + ((int) (ratioX * j))];
                }
            }
            mutex.WaitOne();
            finishCount++;
            mutex.ReleaseMutex();
        }

        private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
        {
            texColors = tex.GetPixels();
            newColors = new Color[newWidth * newHeight];
            if (useBilinear)
            {
                ratioX = 1f / (((float) newWidth) / ((float) (tex.width - 1)));
                ratioY = 1f / (((float) newHeight) / ((float) (tex.height - 1)));
            }
            else
            {
                ratioX = ((float) tex.width) / ((float) newWidth);
                ratioY = ((float) tex.height) / ((float) newHeight);
            }
            w = tex.width;
            w2 = newWidth;
            int num = Mathf.Min(SystemInfo.processorCount, newHeight);
            int num2 = newHeight / num;
            finishCount = 0;
            if (mutex == null)
            {
                mutex = new Mutex(false);
            }
            if (num > 1)
            {
                ThreadData data;
                int num3 = 0;
                num3 = 0;
                while (num3 < (num - 1))
                {
                    data = new ThreadData(num2 * num3, num2 * (num3 + 1));
                    ParameterizedThreadStart start = !useBilinear ? new ParameterizedThreadStart(TextureScale.PointScale) : new ParameterizedThreadStart(TextureScale.BilinearScale);
                    new Thread(start).Start(data);
                    num3++;
                }
                data = new ThreadData(num2 * num3, newHeight);
                if (useBilinear)
                {
                    BilinearScale(data);
                }
                else
                {
                    PointScale(data);
                }
                while (finishCount < num)
                {
                    Thread.Sleep(1);
                }
            }
            else
            {
                ThreadData data2 = new ThreadData(0, newHeight);
                if (useBilinear)
                {
                    BilinearScale(data2);
                }
                else
                {
                    PointScale(data2);
                }
            }
            tex.Resize(newWidth, newHeight);
            tex.SetPixels(newColors);
            tex.Apply();
        }

        public class ThreadData
        {
            public int end;
            public int start;

            public ThreadData(int s, int e)
            {
                this.start = s;
                this.end = e;
            }
        }
    }
}

