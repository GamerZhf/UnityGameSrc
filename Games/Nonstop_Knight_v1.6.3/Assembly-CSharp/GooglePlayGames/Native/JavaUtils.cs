namespace GooglePlayGames.Native
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Extension]
    internal static class JavaUtils
    {
        private static ConstructorInfo IntPtrConstructor;

        static JavaUtils()
        {
            System.Type[] types = new System.Type[] { typeof(IntPtr) };
            IntPtrConstructor = typeof(AndroidJavaObject).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, types, null);
        }

        internal static AndroidJavaObject JavaObjectFromPointer(IntPtr jobject)
        {
            if (jobject == IntPtr.Zero)
            {
                return null;
            }
            object[] parameters = new object[] { jobject };
            return (AndroidJavaObject) IntPtrConstructor.Invoke(parameters);
        }

        [Extension]
        internal static AndroidJavaObject NullSafeCall(AndroidJavaObject target, string methodName, params object[] args)
        {
            try
            {
                return target.Call<AndroidJavaObject>(methodName, args);
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("null"))
                {
                    Logger.w("CallObjectMethod exception: " + exception);
                }
                return null;
            }
        }
    }
}

