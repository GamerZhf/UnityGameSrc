namespace Appboy.Utilities
{
    using System;
    using UnityEngine;

    public class ColorUtils
    {
        public static Color HexToColor(int hex)
        {
            Debug.Log(string.Format("The Hex color int is {0}.", hex));
            byte num = (byte) ((hex >> 0x18) & 0xff);
            byte num2 = (byte) ((hex >> 0x10) & 0xff);
            byte num3 = (byte) ((hex >> 8) & 0xff);
            byte num4 = (byte) (hex & 0xff);
            object[] args = new object[] { num, num2, num3, num4 };
            Debug.Log(string.Format("The Hex color is a:{0} r:{1} g:{2} b:{3}.", args));
            return new Color((float) num2, (float) num3, (float) num4, (float) num);
        }

        public static Color? HexToColor(string hex)
        {
            if (hex != null)
            {
                int num;
                uint num2;
                if (int.TryParse(hex, out num))
                {
                    return new Color?(HexToColor(Convert.ToInt32(num)));
                }
                if (uint.TryParse(hex, out num2))
                {
                    return new Color?(HexToColor((int) num2));
                }
                Debug.Log(string.Format("The Hex color string {0} cannot be parsed to int. Return clear color.", hex));
            }
            return null;
        }
    }
}

