namespace Appboy.Utilities
{
    using System;

    public class EnumUtils
    {
        public static object TryParse(Type enumType, string value, bool ignoreCase, object defaultValue)
        {
            try
            {
                return Enum.Parse(enumType, value, ignoreCase);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}

