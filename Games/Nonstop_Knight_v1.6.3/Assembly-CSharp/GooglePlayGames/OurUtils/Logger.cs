namespace GooglePlayGames.OurUtils
{
    using System;
    using UnityEngine;

    public class Logger
    {
        private static bool debugLogEnabled;
        private static bool warningLogEnabled = true;

        public static void d(string msg)
        {
            if (debugLogEnabled)
            {
                Debug.Log(ToLogMessage(string.Empty, "DEBUG", msg));
            }
        }

        public static string describe(byte[] b)
        {
            return ((b != null) ? ("byte[" + b.Length + "]") : "(null)");
        }

        public static void e(string msg)
        {
            if (warningLogEnabled)
            {
                Debug.LogWarning(ToLogMessage("***", "ERROR", msg));
            }
        }

        private static string ToLogMessage(string prefix, string logType, string msg)
        {
            object[] args = new object[] { prefix, DateTime.Now.ToString("MM/dd/yy H:mm:ss zzz"), logType, msg };
            return string.Format("{0} [Play Games Plugin DLL] {1} {2}: {3}", args);
        }

        public static void w(string msg)
        {
            if (warningLogEnabled)
            {
                Debug.LogWarning(ToLogMessage("!!!", "WARNING", msg));
            }
        }

        public static bool DebugLogEnabled
        {
            get
            {
                return debugLogEnabled;
            }
            set
            {
                debugLogEnabled = value;
            }
        }

        public static bool WarningLogEnabled
        {
            get
            {
                return warningLogEnabled;
            }
            set
            {
                warningLogEnabled = value;
            }
        }
    }
}

