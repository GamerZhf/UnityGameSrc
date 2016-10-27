namespace Service
{
    using System;

    public class ValueParser
    {
        public static object Parse(object value)
        {
            object parsed = null;
            if (ParseNumber(value, ref parsed))
            {
                return parsed;
            }
            if (ParseBoolean(value, ref parsed))
            {
                return parsed;
            }
            parsed = value as string;
            if (parsed != null)
            {
                return parsed;
            }
            return value;
        }

        private static bool ParseBoolean(object value, ref object parsed)
        {
            string str = value as string;
            if (!string.IsNullOrEmpty(str))
            {
                str = str.ToLower();
                if (str.Equals("true"))
                {
                    parsed = true;
                    return true;
                }
                if (str.Equals("false"))
                {
                    parsed = false;
                    return true;
                }
                parsed = null;
            }
            return false;
        }

        private static bool ParseNumber(object value, ref object parsed)
        {
            try
            {
                parsed = double.Parse(value as string);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}

