using System;
using System.Diagnostics;
using UnityEngine;

public static class Assert
{
    [Conditional("DEVELOPMENT")]
    public static void IsNull(object obj)
    {
        if (obj != null)
        {
            UnityEngine.Debug.LogError("Assertion failed. " + StackTraceUtility.ExtractStackTrace());
        }
    }

    [Conditional("DEVELOPMENT")]
    public static void IsTrue(bool condition)
    {
        if (!condition)
        {
            UnityEngine.Debug.LogError("Assertion failed. " + StackTraceUtility.ExtractStackTrace());
        }
    }

    [Conditional("DEVELOPMENT")]
    public static void IsTrue(bool condition, string errorMessage)
    {
        if (!condition)
        {
            UnityEngine.Debug.LogError("Assertion failed: " + errorMessage + " | " + StackTraceUtility.ExtractStackTrace());
        }
    }

    public static void IsTrue_Release(bool condition)
    {
        if (!condition)
        {
            UnityEngine.Debug.LogError("Assertion failed. " + StackTraceUtility.ExtractStackTrace());
        }
    }

    public static void IsTrue_Release(bool condition, string errorMessage)
    {
        if (!condition)
        {
            UnityEngine.Debug.LogError("Assertion failed: " + errorMessage + " | " + StackTraceUtility.ExtractStackTrace());
        }
    }

    [Conditional("DEVELOPMENT")]
    public static void NotNull(object obj)
    {
        if (obj == null)
        {
            UnityEngine.Debug.LogError("Assertion failed. " + StackTraceUtility.ExtractStackTrace());
        }
    }
}

