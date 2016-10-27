namespace PlayerView
{
    using System;
    using System.Text.RegularExpressions;

    public static class FontUtil
    {
        public static bool StringHasNonAsciiCharacters(string str)
        {
            return Regex.IsMatch(str, @"[^\u0000-\u007F]");
        }
    }
}

