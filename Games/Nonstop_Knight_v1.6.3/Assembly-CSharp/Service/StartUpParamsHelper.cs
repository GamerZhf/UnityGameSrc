namespace Service
{
    using System;
    using UnityEngine;

    public class StartUpParamsHelper
    {
        private const char PARAM_SPLITTER = '|';

        public static string GetRaw()
        {
            return "x";
        }

        private static string[] GetStartupParamsAndroid()
        {
            string str = null;
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.koplagames.unityextensions.AppboyUnityPlayerNativeActivity"))
            {
                str = class2.CallStatic<string>("getCustomIntentParam", new object[0]);
            }
            if (str != null)
            {
                str = str;
            }
            else
            {
                str = string.Empty;
            }
            char[] separator = new char[] { '|' };
            return str.Split(separator);
        }

        public static string[] GetStartupParamsOrEmpty()
        {
            return GetStartupParamsAndroid();
        }

        public static string ToString(string[] input)
        {
            string str = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                str = str + input[i] + "|";
            }
            return str;
        }
    }
}

