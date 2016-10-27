﻿namespace Facebook.Unity.Mobile.Android
{
    using System;

    internal interface IAndroidJavaClass
    {
        T CallStatic<T>(string methodName);
        void CallStatic(string methodName, params object[] args);
    }
}

