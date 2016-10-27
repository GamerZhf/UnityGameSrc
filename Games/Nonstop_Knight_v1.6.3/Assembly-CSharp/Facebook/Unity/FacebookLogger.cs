namespace Facebook.Unity
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal static class FacebookLogger
    {
        [CompilerGenerated]
        private static IFacebookLogger <Instance>k__BackingField;
        private const string UnityAndroidTag = "Facebook.Unity.FBDebug";

        static FacebookLogger()
        {
            Instance = new CustomLogger();
        }

        public static void Error(string msg)
        {
            Instance.Error(msg);
        }

        public static void Error(string format, params string[] args)
        {
            Error(string.Format(format, (object[]) args));
        }

        public static void Info(string msg)
        {
            Instance.Info(msg);
        }

        public static void Info(string format, params string[] args)
        {
            Info(string.Format(format, (object[]) args));
        }

        public static void Log(string msg)
        {
            Instance.Log(msg);
        }

        public static void Log(string format, params string[] args)
        {
            Log(string.Format(format, (object[]) args));
        }

        public static void Warn(string msg)
        {
            Instance.Warn(msg);
        }

        public static void Warn(string format, params string[] args)
        {
            Warn(string.Format(format, (object[]) args));
        }

        internal static IFacebookLogger Instance
        {
            [CompilerGenerated]
            private get
            {
                return <Instance>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <Instance>k__BackingField = value;
            }
        }

        private class AndroidLogger : IFacebookLogger
        {
            public void Error(string msg)
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("android.util.Log"))
                {
                    object[] args = new object[] { "Facebook.Unity.FBDebug", msg };
                    class2.CallStatic<int>("e", args);
                }
            }

            public void Info(string msg)
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("android.util.Log"))
                {
                    object[] args = new object[] { "Facebook.Unity.FBDebug", msg };
                    class2.CallStatic<int>("i", args);
                }
            }

            public void Log(string msg)
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("android.util.Log"))
                {
                    object[] args = new object[] { "Facebook.Unity.FBDebug", msg };
                    class2.CallStatic<int>("v", args);
                }
            }

            public void Warn(string msg)
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("android.util.Log"))
                {
                    object[] args = new object[] { "Facebook.Unity.FBDebug", msg };
                    class2.CallStatic<int>("w", args);
                }
            }
        }

        private class CustomLogger : IFacebookLogger
        {
            private IFacebookLogger logger = new FacebookLogger.AndroidLogger();

            public void Error(string msg)
            {
                Debug.LogError(msg);
                this.logger.Error(msg);
            }

            public void Info(string msg)
            {
                Debug.Log(msg);
                this.logger.Info(msg);
            }

            public void Log(string msg)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log(msg);
                    this.logger.Log(msg);
                }
            }

            public void Warn(string msg)
            {
                Debug.LogWarning(msg);
                this.logger.Warn(msg);
            }
        }
    }
}

