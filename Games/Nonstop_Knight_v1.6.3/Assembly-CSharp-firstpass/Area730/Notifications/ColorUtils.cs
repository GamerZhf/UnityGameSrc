namespace Area730.Notifications
{
    using System;
    using UnityEngine;

    public class ColorUtils
    {
        private static string GetHex(int num)
        {
            string str = "0123456789ABCDEF";
            return (string.Empty + str[num]);
        }

        public static string ToHtmlStringRGB(Color color)
        {
            float num = color.r * 255f;
            float num2 = color.g * 255f;
            float num3 = color.b * 255f;
            string hex = GetHex((int) Mathf.Floor((float) (((int) num) / 0x10)));
            string str2 = GetHex((int) Mathf.Round((float) (((int) num) % 0x10)));
            string str3 = GetHex((int) Mathf.Floor((float) (((int) num2) / 0x10)));
            string str4 = GetHex((int) Mathf.Round((float) (((int) num2) % 0x10)));
            string str5 = GetHex((int) Mathf.Floor((float) (((int) num3) / 0x10)));
            string str6 = GetHex((int) Mathf.Round((float) (((int) num3) % 0x10)));
            string[] textArray1 = new string[] { hex, str2, str3, str4, str5, str6 };
            return string.Concat(textArray1);
        }
    }
}

