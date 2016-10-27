using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

public static class ColorUtil
{
    public static string ColorToHex(Color32 color)
    {
        return (color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2"));
    }

    public static Color Combine(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static Color Desaturate(Color color)
    {
        float grayscale = color.grayscale;
        color.r = grayscale;
        color.g = grayscale;
        color.b = grayscale;
        return color;
    }

    public static Color GetRandomColor_OneOrTwoChannels()
    {
        Color color = new Color(0f, 0f, 0f, 1f);
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            switch (UnityEngine.Random.Range(0, 3))
            {
                case 0:
                    color.r = UnityEngine.Random.Range((float) 0f, (float) 1f);
                    color.g = UnityEngine.Random.Range((float) 0f, (float) 1f);
                    color.b = 0f;
                    return color;

                case 1:
                    color.r = 0f;
                    color.g = UnityEngine.Random.Range((float) 0f, (float) 1f);
                    color.b = UnityEngine.Random.Range((float) 0f, (float) 1f);
                    return color;

                case 2:
                    color.r = UnityEngine.Random.Range((float) 0f, (float) 1f);
                    color.g = 0f;
                    color.b = UnityEngine.Random.Range((float) 0f, (float) 1f);
                    return color;
            }
            return color;
        }
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                color.r = UnityEngine.Random.Range((float) 0f, (float) 1f);
                color.g = 0f;
                color.b = 0f;
                return color;

            case 1:
                color.r = 0f;
                color.g = UnityEngine.Random.Range((float) 0f, (float) 1f);
                color.b = 0f;
                return color;

            case 2:
                color.r = 0f;
                color.g = 0f;
                color.b = UnityEngine.Random.Range((float) 0f, (float) 1f);
                return color;
        }
        return color;
    }

    public static Color[] HexToColor(string[] hex)
    {
        Color[] colorArray = new Color[hex.Length];
        for (int i = 0; i < hex.Length; i++)
        {
            colorArray[i] = HexToColor(hex[i], 1f);
        }
        return colorArray;
    }

    public static Color HexToColor(string hex, [Optional, DefaultParameterValue(1f)] float opacity)
    {
        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1, hex.Length - 1);
        }
        byte num = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte num2 = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        byte num3 = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        return new Color(((float) num) / 255f, ((float) num2) / 255f, ((float) num3) / 255f, opacity);
    }

    public static Color32[] HexToColor32(string[] hex)
    {
        Color32[] colorArray = new Color32[hex.Length];
        for (int i = 0; i < hex.Length; i++)
        {
            colorArray[i] = HexToColor32(hex[i], 0xff);
        }
        return colorArray;
    }

    public static Color32 HexToColor32(string hex, [Optional, DefaultParameterValue(0xff)] byte opacity)
    {
        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1, hex.Length - 1);
        }
        byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        return new Color32(r, g, byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber), opacity);
    }

    public static bool IsValidColorHex(string hex)
    {
        if (string.IsNullOrEmpty(hex))
        {
            return false;
        }
        foreach (char ch in hex)
        {
            if ((((ch < '0') || (ch > '9')) && ((ch < 'a') || (ch > 'f'))) && ((ch < 'A') || (ch > 'F')))
            {
                return false;
            }
        }
        return true;
    }

    public static Color MultiplyAlpha(Color color, float multiplier)
    {
        color.a *= multiplier;
        return color;
    }

    public static Color MultiplyRGB(Color color, float multiplier)
    {
        color.r *= multiplier;
        color.g *= multiplier;
        color.b *= multiplier;
        return color;
    }
}

