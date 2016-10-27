namespace OrbCreationExtensions
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Extension]
    public static class Texture2DExtensions
    {
        [Extension]
        public static void CopyFrom(Texture2D tex, Texture2D fromTex, int toX, int toY, int fromX, int fromY, int width, int height)
        {
            MakeFormatWritable(tex);
            int num = tex.width;
            Color[] pixels = tex.GetPixels(0);
            Color[] colorArray2 = fromTex.GetPixels(fromX, fromY, width, height, 0);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pixels[(((i + toY) * num) + j) + toX] = colorArray2[(i * width) + j];
                }
            }
            tex.SetPixels(pixels, 0);
            tex.Apply(tex.mipmapCount > 1, false);
        }

        [Extension]
        public static void Fill(Texture2D tex, Color aColor)
        {
            MakeFormatWritable(tex);
            Color[] pixels = tex.GetPixels(0);
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = aColor;
            }
            tex.SetPixels(pixels, 0);
            tex.Apply(tex.mipmapCount > 1, false);
        }

        [Extension]
        public static Texture2D FromUnityNormalMap(Texture2D tex)
        {
            Texture2D textured = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, tex.mipmapCount > 1);
            Color[] pixels = tex.GetPixels(0);
            Color[] colors = new Color[pixels.Length];
            for (int i = 0; i < tex.height; i++)
            {
                for (int j = 0; j < tex.width; j++)
                {
                    Color color = pixels[(i * tex.width) + j];
                    Color color2 = new Color(0f, 0f, 0f, 0f);
                    color2.g = color.r;
                    color2.r = color.a;
                    color2.b = 1f;
                    colors[(i * tex.width) + j] = color2;
                }
            }
            textured.SetPixels(colors, 0);
            textured.Apply(tex.mipmapCount > 1, false);
            return textured;
        }

        [Extension]
        public static Color GetAverageColor(Texture2D tex)
        {
            Vector4 zero = Vector4.zero;
            float num = 0f;
            Color[] pixels = tex.GetPixels(0);
            for (int i = 0; i < pixels.Length; i++)
            {
                zero += (Vector4) (pixels[i] * pixels[i].a);
                num += pixels[i].a;
            }
            if (num < 1f)
            {
                num = 1f;
            }
            zero.w = num;
            return (Color) (zero / num);
        }

        [Extension]
        public static Color GetAverageColor(Texture2D tex, Color useThisColorForAlpha)
        {
            Vector4 zero = Vector4.zero;
            float num = 0f;
            Color[] pixels = tex.GetPixels(0);
            for (int i = 0; i < pixels.Length; i++)
            {
                zero += (Vector4) (pixels[i] * pixels[i].a);
                zero += (Vector4) (useThisColorForAlpha * (1f - pixels[i].a));
                num++;
            }
            if (num < 1f)
            {
                num = 1f;
            }
            zero.w = num;
            return (Color) (zero / num);
        }

        [Extension]
        public static Texture2D GetCopy(Texture2D tex)
        {
            return GetCopy(tex, 0, 0, tex.width, tex.height, tex.mipmapCount > 1);
        }

        [Extension]
        public static Texture2D GetCopy(Texture2D tex, int x, int y, int w, int h)
        {
            return GetCopy(tex, x, y, w, h, tex.mipmapCount > 1);
        }

        [Extension]
        public static Texture2D GetCopy(Texture2D tex, int x, int y, int w, int h, bool mipMaps)
        {
            Texture2D textured = new Texture2D(w, h, GetWritableFormat(tex.format), mipMaps);
            textured.SetPixels(tex.GetPixels(x, y, w, h, 0), 0);
            textured.Apply(mipMaps, false);
            return textured;
        }

        [Extension]
        public static Texture2D GetSection(Texture2D tex, int x, int y, int w, int h)
        {
            return GetCopy(tex, x, y, w, h, tex.mipmapCount > 1);
        }

        [Extension]
        public static Texture2D GetUnityNormalMap(Texture2D tex)
        {
            Texture2D textured = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, tex.mipmapCount > 1);
            Color[] pixels = tex.GetPixels(0);
            Color[] colors = new Color[pixels.Length];
            for (int i = 0; i < tex.height; i++)
            {
                for (int j = 0; j < tex.width; j++)
                {
                    Color color = pixels[(i * tex.width) + j];
                    Color color2 = new Color(0f, 0f, 0f, 0f);
                    color2.r = color.g;
                    color2.g = color.g;
                    color2.b = color.g;
                    color2.a = color.r;
                    colors[(i * tex.width) + j] = color2;
                }
            }
            textured.SetPixels(colors, 0);
            textured.Apply(tex.mipmapCount > 1, false);
            return textured;
        }

        public static TextureFormat GetWritableFormat(TextureFormat format)
        {
            if (((format != TextureFormat.Alpha8) && (format != TextureFormat.RGB24)) && ((format != TextureFormat.ARGB32) && (format != TextureFormat.RGBA32)))
            {
                if (((((format == TextureFormat.RGB24) || (format == TextureFormat.DXT1)) || ((format == TextureFormat.PVRTC_RGB2) || (format == TextureFormat.PVRTC_RGB4))) || (((format == TextureFormat.ETC_RGB4) || (format == TextureFormat.ATC_RGB4)) || ((format == TextureFormat.ATC_RGBA8) || (format == TextureFormat.ETC2_RGB)))) || ((((format == TextureFormat.ASTC_RGB_4x4) || (format == TextureFormat.ASTC_RGB_5x5)) || ((format == TextureFormat.ASTC_RGB_5x5) || (format == TextureFormat.ASTC_RGB_5x5))) || (((format == TextureFormat.ASTC_RGB_6x6) || (format == TextureFormat.ASTC_RGB_10x10)) || (format == TextureFormat.ASTC_RGB_12x12))))
                {
                    format = TextureFormat.RGB24;
                    return format;
                }
                format = TextureFormat.RGBA32;
            }
            return format;
        }

        [Extension]
        public static bool HasTransparency(Texture2D tex)
        {
            Color[] pixels;
            try
            {
                pixels = tex.GetPixels(0);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                return false;
            }
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a < 1f)
                {
                    return true;
                }
            }
            return false;
        }

        [Extension]
        public static bool IsReadable(Texture2D tex)
        {
            try
            {
                tex.GetPixels(0, 0, 1, 1, 0);
            }
            catch (Exception exception)
            {
                return (exception == null);
            }
            return true;
        }

        [Extension]
        public static void MakeFormatWritable(Texture2D tex)
        {
            TextureFormat format = tex.format;
            TextureFormat writableFormat = GetWritableFormat(tex.format);
            if (writableFormat != format)
            {
                Color[] pixels = tex.GetPixels(0);
                tex.Resize(tex.width, tex.height, writableFormat, tex.mipmapCount > 1);
                tex.SetPixels(pixels, 0);
                tex.Apply(tex.mipmapCount > 1, false);
            }
        }

        [Extension]
        public static void Scale(Texture2D tex, int width, int height)
        {
            if (((width > 0) && (height > 0)) && ((width != tex.width) || (height != tex.height)))
            {
                MakeFormatWritable(tex);
                Color[] colors = ScaledPixels(tex.GetPixels(0), tex.width, tex.height, width, height);
                if (tex.Resize(width, height, tex.format, tex.mipmapCount > 1))
                {
                    tex.SetPixels(colors, 0);
                    tex.Apply(tex.mipmapCount > 1, false);
                }
            }
        }

        [Extension]
        public static Texture2D ScaledCopy(Texture2D tex, int width, int height, bool mipMaps)
        {
            if ((width <= 0) || (height <= 0))
            {
                return null;
            }
            if ((width == tex.width) && (height == tex.height))
            {
                return GetCopy(tex, 0, 0, tex.width, tex.height, mipMaps);
            }
            Color[] colors = ScaledPixels(tex.GetPixels(0), tex.width, tex.height, width, height);
            Texture2D textured = new Texture2D(width, height, GetWritableFormat(tex.format), mipMaps);
            textured.SetPixels(colors, 0);
            textured.Apply(mipMaps, false);
            return textured;
        }

        private static Color[] ScaledPixels(Color[] originalPixels, int oldWidth, int oldHeight, int width, int height)
        {
            if (((width <= 0) || (height <= 0)) || ((width == oldWidth) && (height == oldHeight)))
            {
                return originalPixels;
            }
            float num = ((float) width) / ((float) oldWidth);
            float num2 = ((float) height) / ((float) oldHeight);
            Color[] colorArray = new Color[width * height];
            for (int i = 0; i < height; i++)
            {
                float f = ((float) i) / num2;
                int num5 = Mathf.FloorToInt(f);
                int num6 = Mathf.CeilToInt(f);
                for (int j = 0; j < width; j++)
                {
                    float num8 = ((float) j) / num;
                    int num9 = Mathf.FloorToInt(num8);
                    int num10 = Mathf.CeilToInt(num8);
                    Color color = (Color) ((originalPixels[(num5 * oldWidth) + num9] * (1f - (f - num5))) * (1f - (num8 - num9)));
                    if ((num9 < num10) && (num10 < oldWidth))
                    {
                        color += (Color) ((originalPixels[(num5 * oldWidth) + num10] * (1f - (f - num5))) * (1f - (num10 - num8)));
                    }
                    if ((num5 < num6) && (num6 < oldHeight))
                    {
                        color += (Color) ((originalPixels[(num6 * oldWidth) + num9] * (1f - (num6 - f))) * (1f - (num8 - num9)));
                        if ((num9 < num10) && (num10 < oldWidth))
                        {
                            color += (Color) ((originalPixels[(num6 * oldWidth) + num10] * (1f - (num6 - f))) * (1f - (num10 - num8)));
                        }
                    }
                    colorArray[(i * width) + j] = color;
                }
            }
            return colorArray;
        }
    }
}

