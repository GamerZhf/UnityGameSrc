namespace Facebook.Unity
{
    using Facebook.MiniJSON;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Extension]
    internal static class Utilities
    {
        [CompilerGenerated]
        private static Func<object, string> <>f__am$cache0;
        private const string WarningMissingParameter = "Did not find expected value '{0}' in dictionary";

        [Extension]
        public static string AbsoluteUrlOrEmptyString(Uri uri)
        {
            if (uri == null)
            {
                return string.Empty;
            }
            return uri.AbsoluteUri;
        }

        [Extension]
        public static void AddAllKVPFrom<T1, T2>(IDictionary<T1, T2> dest, IDictionary<T1, T2> source)
        {
            IEnumerator<T1> enumerator = source.get_Keys().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    T1 current = enumerator.Current;
                    dest[current] = source[current];
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
        }

        private static DateTime FromTimestamp(int timestamp)
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0);
            return time.AddSeconds((double) timestamp);
        }

        public static string GetUserAgent(string productName, string productVersion)
        {
            object[] args = new object[] { productName, productVersion };
            return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", args);
        }

        [Extension]
        public static T GetValueOrDefault<T>(IDictionary<string, object> dictionary, string key, [Optional, DefaultParameterValue(true)] bool logWarning)
        {
            T local;
            if (!TryGetValue<T>(dictionary, key, out local))
            {
                string[] args = new string[] { key };
                FacebookLogger.Warn("Did not find expected value '{0}' in dictionary", args);
            }
            return local;
        }

        public static AccessToken ParseAccessTokenFromResult(IDictionary<string, object> resultDictionary)
        {
            string userId = GetValueOrDefault<string>(resultDictionary, LoginResult.UserIdKey, true);
            string tokenString = GetValueOrDefault<string>(resultDictionary, LoginResult.AccessTokenKey, true);
            DateTime expirationTime = ParseExpirationDateFromResult(resultDictionary);
            ICollection<string> permissions = ParsePermissionFromResult(resultDictionary);
            return new AccessToken(tokenString, userId, expirationTime, permissions, ParseLastRefreshFromResult(resultDictionary));
        }

        private static DateTime ParseExpirationDateFromResult(IDictionary<string, object> resultDictionary)
        {
            int num;
            if (Constants.IsWeb)
            {
                return DateTime.Now.AddSeconds((double) GetValueOrDefault<long>(resultDictionary, LoginResult.ExpirationTimestampKey, true));
            }
            if (int.TryParse(GetValueOrDefault<string>(resultDictionary, LoginResult.ExpirationTimestampKey, true), out num) && (num > 0))
            {
                return FromTimestamp(num);
            }
            return DateTime.MaxValue;
        }

        private static DateTime? ParseLastRefreshFromResult(IDictionary<string, object> resultDictionary)
        {
            int num;
            if (int.TryParse(GetValueOrDefault<string>(resultDictionary, LoginResult.ExpirationTimestampKey, true), out num) && (num > 0))
            {
                return new DateTime?(FromTimestamp(num));
            }
            return null;
        }

        private static ICollection<string> ParsePermissionFromResult(IDictionary<string, object> resultDictionary)
        {
            string str;
            IEnumerable<object> enumerable;
            if (TryGetValue<string>(resultDictionary, LoginResult.PermissionsKey, out str))
            {
                char[] separator = new char[] { ',' };
                enumerable = str.Split(separator);
            }
            else if (!TryGetValue<IEnumerable<object>>(resultDictionary, LoginResult.PermissionsKey, out enumerable))
            {
                enumerable = new string[0];
                string[] args = new string[] { LoginResult.PermissionsKey };
                FacebookLogger.Warn("Failed to find parameter '{0}' in login result", args);
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (object permission) {
                    return permission.ToString();
                };
            }
            return Enumerable.ToList<string>(Enumerable.Select<object, string>(enumerable, <>f__am$cache0));
        }

        [Extension]
        public static string ToCommaSeparateList(IEnumerable<string> list)
        {
            if (list == null)
            {
                return string.Empty;
            }
            return string.Join(",", Enumerable.ToArray<string>(list));
        }

        [Extension]
        public static string ToJson(IDictionary<string, object> dictionary)
        {
            return Json.Serialize(dictionary);
        }

        [Extension]
        public static long TotalSeconds(DateTime dateTime)
        {
            TimeSpan span = (TimeSpan) (dateTime - new DateTime(0x7b2, 1, 1));
            return (long) span.TotalSeconds;
        }

        [Extension]
        public static bool TryGetValue<T>(IDictionary<string, object> dictionary, string key, out T value)
        {
            object obj2;
            if (dictionary.TryGetValue(key, out obj2) && (obj2 is T))
            {
                value = (T) obj2;
                return true;
            }
            value = default(T);
            return false;
        }
    }
}

