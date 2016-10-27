using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class IntegrationTest
{
    public const string failMessage = "IntegrationTest Fail";
    public const string passMessage = "IntegrationTest Pass";

    public static void Assert(bool condition)
    {
        Assert(condition, string.Empty);
    }

    public static void Assert(bool condition, string message)
    {
        if (condition)
        {
            Pass();
        }
        else
        {
            Fail(message);
        }
    }

    public static void Assert(GameObject go, bool condition)
    {
        Assert(go, condition, string.Empty);
    }

    public static void Assert(GameObject go, bool condition, string message)
    {
        if (condition)
        {
            Pass(go);
        }
        else
        {
            Fail(go, message);
        }
    }

    public static void Fail()
    {
        LogResult("IntegrationTest Fail");
    }

    public static void Fail(string reason)
    {
        Fail();
        if (!string.IsNullOrEmpty(reason))
        {
            Debug.Log(reason);
        }
    }

    public static void Fail(GameObject go)
    {
        LogResult(go, "IntegrationTest Fail");
    }

    public static void Fail(GameObject go, string reason)
    {
        Fail(go);
        if (!string.IsNullOrEmpty(reason))
        {
            Debug.Log(reason);
        }
    }

    private static GameObject FindTestObject(GameObject go)
    {
        for (GameObject obj2 = go; obj2.transform.parent != null; obj2 = obj2.transform.parent.gameObject)
        {
            if (obj2.GetComponent("TestComponent") != null)
            {
                return obj2;
            }
        }
        return go;
    }

    private static void LogResult(string message)
    {
        Debug.Log(message);
    }

    private static void LogResult(GameObject go, string message)
    {
        Debug.Log(message + " (" + FindTestObject(go).name + ")", go);
    }

    public static void Pass()
    {
        LogResult("IntegrationTest Pass");
    }

    public static void Pass(GameObject go)
    {
        LogResult(go, "IntegrationTest Pass");
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class DynamicTestAttribute : Attribute
    {
        private readonly string m_SceneName;

        public DynamicTestAttribute(string sceneName)
        {
            if (sceneName.EndsWith(".unity"))
            {
                sceneName = sceneName.Substring(0, sceneName.Length - ".unity".Length);
            }
            this.m_SceneName = sceneName;
        }

        public bool IncludeOnScene(string sceneName)
        {
            return (Path.GetFileNameWithoutExtension(sceneName) == this.m_SceneName);
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class ExcludePlatformAttribute : Attribute
    {
        [CompilerGenerated]
        private static Func<RuntimePlatform, string> <>f__am$cache1;
        public string[] platformsToExclude;

        public ExcludePlatformAttribute(params RuntimePlatform[] platformsToExclude)
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = new Func<RuntimePlatform, string>(IntegrationTest.ExcludePlatformAttribute.<ExcludePlatformAttribute>m__5A);
            }
            this.platformsToExclude = Enumerable.ToArray<string>(Enumerable.Select<RuntimePlatform, string>(platformsToExclude, <>f__am$cache1));
        }

        [CompilerGenerated]
        private static string <ExcludePlatformAttribute>m__5A(RuntimePlatform platform)
        {
            return platform.ToString();
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class ExpectExceptions : Attribute
    {
        [CompilerGenerated]
        private static Func<System.Type, string> <>f__am$cache2;
        public string[] exceptionTypeNames;
        public bool succeedOnException;

        public ExpectExceptions() : this(false)
        {
        }

        public ExpectExceptions(bool succeedOnException) : this(succeedOnException, new string[0])
        {
        }

        public ExpectExceptions(params string[] exceptionTypeNames) : this(false, exceptionTypeNames)
        {
        }

        public ExpectExceptions(params System.Type[] exceptionTypes) : this(false, exceptionTypes)
        {
        }

        public ExpectExceptions(bool succeedOnException, params string[] exceptionTypeNames)
        {
            this.succeedOnException = succeedOnException;
            this.exceptionTypeNames = exceptionTypeNames;
        }

        public ExpectExceptions(bool succeedOnException, params System.Type[] exceptionTypes) : this(succeedOnException, Enumerable.ToArray<string>(Enumerable.Select<System.Type, string>(exceptionTypes, <>f__am$cache2)))
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = new Func<System.Type, string>(IntegrationTest.ExpectExceptions.<ExpectExceptions>m__5B);
            }
        }

        [CompilerGenerated]
        private static string <ExpectExceptions>m__5B(System.Type type)
        {
            return type.FullName;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class IgnoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class SucceedWithAssertions : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class TimeoutAttribute : Attribute
    {
        public float timeout;

        public TimeoutAttribute(float seconds)
        {
            this.timeout = seconds;
        }
    }
}

