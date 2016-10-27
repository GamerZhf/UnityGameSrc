namespace UnityTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public class TestComponent : MonoBehaviour, ITestComponent, IComparable<ITestComponent>
    {
        [CompilerGenerated]
        private static Func<string, bool> <>f__am$cacheB;
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cacheC;
        [CompilerGenerated]
        private static Func<Assembly, IEnumerable<System.Type>> <>f__am$cacheD;
        [CompilerGenerated]
        private static Func<TestComponent, bool> <>f__am$cacheE;
        [CompilerGenerated]
        private static Func<TestComponent, bool> <>f__am$cacheF;
        public bool dynamic;
        public string dynamicTypeName;
        public string expectedExceptionList = string.Empty;
        public bool expectException;
        public bool ignored;
        public IncludedPlatforms includedPlatforms = -1;
        public static ITestComponent NullTestComponent = new NullTestComponentImpl();
        public string[] platformsToIgnore;
        public bool succeedAfterAllAssertionsAreExecuted;
        public bool succeedWhenExceptionIsThrown;
        public float timeout = 5f;

        public static bool AnyTestsOnScene()
        {
            return Enumerable.Any<TestComponent>(FindAllTestsOnScene());
        }

        public int CompareTo(ITestComponent obj)
        {
            if (obj == NullTestComponent)
            {
                return 1;
            }
            int num = base.gameObject.name.CompareTo(obj.gameObject.name);
            if (num == 0)
            {
                num = base.gameObject.GetInstanceID().CompareTo(obj.gameObject.GetInstanceID());
            }
            return num;
        }

        public static TestComponent CreateDynamicTest(System.Type type)
        {
            GameObject obj2 = CreateTest(type.Name);
            obj2.hideFlags |= HideFlags.DontSave;
            obj2.SetActive(false);
            TestComponent component = obj2.GetComponent<TestComponent>();
            component.dynamic = true;
            component.dynamicTypeName = type.AssemblyQualifiedName;
            foreach (object obj3 in type.GetCustomAttributes(false))
            {
                if (obj3 is IntegrationTest.TimeoutAttribute)
                {
                    component.timeout = (obj3 as IntegrationTest.TimeoutAttribute).timeout;
                }
                else if (obj3 is IntegrationTest.IgnoreAttribute)
                {
                    component.ignored = true;
                }
                else if (obj3 is IntegrationTest.SucceedWithAssertions)
                {
                    component.succeedAfterAllAssertionsAreExecuted = true;
                }
                else if (obj3 is IntegrationTest.ExcludePlatformAttribute)
                {
                    component.platformsToIgnore = (obj3 as IntegrationTest.ExcludePlatformAttribute).platformsToExclude;
                }
                else if (obj3 is IntegrationTest.ExpectExceptions)
                {
                    IntegrationTest.ExpectExceptions exceptions = obj3 as IntegrationTest.ExpectExceptions;
                    component.expectException = true;
                    component.expectedExceptionList = string.Join(",", exceptions.exceptionTypeNames);
                    component.succeedWhenExceptionIsThrown = exceptions.succeedOnException;
                }
            }
            obj2.AddComponent(type);
            return component;
        }

        public static GameObject CreateTest()
        {
            return CreateTest("New Test");
        }

        private static GameObject CreateTest(string name)
        {
            GameObject obj2 = new GameObject(name);
            obj2.AddComponent<TestComponent>();
            return obj2;
        }

        public static void DestroyAllDynamicTests()
        {
            foreach (TestComponent component in FindAllDynamicTestsOnScene())
            {
                UnityEngine.Object.DestroyImmediate(component.gameObject);
            }
        }

        public static void DisableAllTests()
        {
            foreach (TestComponent component in FindAllTestsOnScene())
            {
                component.EnableTest(false);
            }
        }

        public void EnableTest(bool enable)
        {
            if (enable && this.dynamic)
            {
                System.Type type = System.Type.GetType(this.dynamicTypeName);
                MonoBehaviour component = base.gameObject.GetComponent(type) as MonoBehaviour;
                if (component != null)
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
                base.gameObject.AddComponent(type);
            }
            if (base.gameObject.activeSelf != enable)
            {
                base.gameObject.SetActive(enable);
            }
        }

        public override bool Equals(object o)
        {
            return ((o is TestComponent) && (this == (o as TestComponent)));
        }

        public static List<TestComponent> FindAllDynamicTestsOnScene()
        {
            if (<>f__am$cacheF == null)
            {
                <>f__am$cacheF = delegate (TestComponent t) {
                    return t.dynamic;
                };
            }
            return Enumerable.ToList<TestComponent>(Enumerable.Where<TestComponent>(FindAllTestsOnScene(), <>f__am$cacheF));
        }

        public static List<TestComponent> FindAllTestsOnScene()
        {
            return Enumerable.ToList<TestComponent>(Enumerable.Cast<TestComponent>(Resources.FindObjectsOfTypeAll(typeof(TestComponent))));
        }

        public static List<TestComponent> FindAllTopTestsOnScene()
        {
            if (<>f__am$cacheE == null)
            {
                <>f__am$cacheE = delegate (TestComponent component) {
                    return component.gameObject.transform.parent == null;
                };
            }
            return Enumerable.ToList<TestComponent>(Enumerable.Where<TestComponent>(FindAllTestsOnScene(), <>f__am$cacheE));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public ITestComponent GetTestGroup()
        {
            Transform parent = base.gameObject.transform.parent;
            if (parent == null)
            {
                return NullTestComponent;
            }
            return parent.GetComponent<TestComponent>();
        }

        public double GetTimeout()
        {
            return (double) this.timeout;
        }

        private static System.Type GetTypeByName(string className)
        {
            <GetTypeByName>c__AnonStorey264 storey = new <GetTypeByName>c__AnonStorey264();
            storey.className = className;
            if (<>f__am$cacheD == null)
            {
                <>f__am$cacheD = delegate (Assembly a) {
                    return a.GetTypes();
                };
            }
            return Enumerable.FirstOrDefault<System.Type>(Enumerable.SelectMany<Assembly, System.Type>(AppDomain.CurrentDomain.GetAssemblies(), <>f__am$cacheD), new Func<System.Type, bool>(storey.<>m__60));
        }

        [DebuggerHidden]
        public static IEnumerable<System.Type> GetTypesWithHelpAttribute(string sceneName)
        {
            <GetTypesWithHelpAttribute>c__Iterator21 iterator = new <GetTypesWithHelpAttribute>c__Iterator21();
            iterator.sceneName = sceneName;
            iterator.<$>sceneName = sceneName;
            iterator.$PC = -2;
            return iterator;
        }

        private static bool IsAssignableFrom(System.Type a, System.Type b)
        {
            return a.IsAssignableFrom(b);
        }

        public bool IsExceptionExpected(string exception)
        {
            if (this.expectException)
            {
                exception = exception.Trim();
                char[] separator = new char[] { ',' };
                if (<>f__am$cacheC == null)
                {
                    <>f__am$cacheC = delegate (string e) {
                        return e.Trim();
                    };
                }
                IEnumerator<string> enumerator = Enumerable.Select<string, string>(this.expectedExceptionList.Split(separator), <>f__am$cacheC).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        System.Type typeByName;
                        System.Type type2;
                        string current = enumerator.Current;
                        if (exception == current)
                        {
                            return true;
                        }
                        System.Type type1 = System.Type.GetType(exception);
                        if (type1 != null)
                        {
                            typeByName = type1;
                        }
                        else
                        {
                            typeByName = GetTypeByName(exception);
                        }
                        System.Type type3 = System.Type.GetType(current);
                        if (type3 != null)
                        {
                            type2 = type3;
                        }
                        else
                        {
                            type2 = GetTypeByName(current);
                        }
                        if (((typeByName != null) && (type2 != null)) && IsAssignableFrom(type2, typeByName))
                        {
                            return true;
                        }
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
            return false;
        }

        public bool IsExludedOnThisPlatform()
        {
            if (this.platformsToIgnore != null)
            {
            }
            return ((<>f__am$cacheB == null) && Enumerable.Any<string>(this.platformsToIgnore, <>f__am$cacheB));
        }

        public bool IsIgnored()
        {
            return this.ignored;
        }

        public bool IsTestGroup()
        {
            for (int i = 0; i < base.gameObject.transform.childCount; i++)
            {
                if (base.gameObject.transform.GetChild(i).GetComponent(typeof(TestComponent)) != null)
                {
                    return true;
                }
            }
            return false;
        }

        public void OnValidate()
        {
            if (this.timeout < 0.01f)
            {
                this.timeout = 0.01f;
            }
        }

        public static bool operator ==(TestComponent a, TestComponent b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }
            if ((a == null) || (b == null))
            {
                return false;
            }
            if (a.dynamic && b.dynamic)
            {
                return (a.dynamicTypeName == b.dynamicTypeName);
            }
            return ((!a.dynamic && !b.dynamic) && (a.gameObject == b.gameObject));
        }

        public static bool operator !=(TestComponent a, TestComponent b)
        {
            return !(a == b);
        }

        public bool ShouldSucceedOnAssertions()
        {
            return this.succeedAfterAllAssertionsAreExecuted;
        }

        public bool ShouldSucceedOnException()
        {
            return this.succeedWhenExceptionIsThrown;
        }

        GameObject ITestComponent.get_gameObject()
        {
            return base.gameObject;
        }

        public string Name
        {
            get
            {
                return ((base.gameObject != null) ? base.gameObject.name : string.Empty);
            }
        }

        [CompilerGenerated]
        private sealed class <GetTypeByName>c__AnonStorey264
        {
            internal string className;

            internal bool <>m__60(System.Type type)
            {
                return (type.Name == this.className);
            }
        }

        [CompilerGenerated]
        private sealed class <GetTypesWithHelpAttribute>c__Iterator21 : IEnumerator, IDisposable, IEnumerable, IEnumerable<System.Type>, IEnumerator<System.Type>
        {
            internal System.Type $current;
            internal int $PC;
            internal string <$>sceneName;
            internal Assembly[] <$s_120>__0;
            internal int <$s_121>__1;
            internal System.Type[] <$s_122>__3;
            internal int <$s_123>__4;
            internal IntegrationTest.DynamicTestAttribute <a>__7;
            internal Assembly <assembly>__2;
            internal object[] <attributes>__6;
            internal System.Type <type>__5;
            internal string sceneName;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<$s_120>__0 = AppDomain.CurrentDomain.GetAssemblies();
                        this.<$s_121>__1 = 0;
                        while (this.<$s_121>__1 < this.<$s_120>__0.Length)
                        {
                            this.<assembly>__2 = this.<$s_120>__0[this.<$s_121>__1];
                            this.<$s_122>__3 = this.<assembly>__2.GetTypes();
                            this.<$s_123>__4 = 0;
                            while (this.<$s_123>__4 < this.<$s_122>__3.Length)
                            {
                                this.<type>__5 = this.<$s_122>__3[this.<$s_123>__4];
                                this.<attributes>__6 = this.<type>__5.GetCustomAttributes(typeof(IntegrationTest.DynamicTestAttribute), true);
                                if (this.<attributes>__6.Length == 1)
                                {
                                    this.<a>__7 = Enumerable.Single<object>(this.<attributes>__6) as IntegrationTest.DynamicTestAttribute;
                                    if (this.<a>__7.IncludeOnScene(this.sceneName))
                                    {
                                        this.$current = this.<type>__5;
                                        this.$PC = 1;
                                        return true;
                                    }
                                }
                            Label_00EE:
                                this.<$s_123>__4++;
                            }
                            this.<$s_121>__1++;
                        }
                        this.$PC = -1;
                        break;

                    case 1:
                        goto Label_00EE;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<System.Type> IEnumerable<System.Type>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                TestComponent.<GetTypesWithHelpAttribute>c__Iterator21 iterator = new TestComponent.<GetTypesWithHelpAttribute>c__Iterator21();
                iterator.sceneName = this.<$>sceneName;
                return iterator;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Type>.GetEnumerator();
            }

            System.Type IEnumerator<System.Type>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [Flags]
        public enum IncludedPlatforms
        {
            Android = 0x400,
            BB10Player = 0x4000,
            IPhonePlayer = 0x800,
            LinuxPlayer = 0x10,
            MetroPlayerARM = 0x80,
            MetroPlayerX64 = 0x40,
            MetroPlayerX86 = 0x20,
            NaCl = 0x8000,
            OSXEditor = 2,
            OSXPlayer = 8,
            OSXWebPlayer = 0x200,
            PS3 = 0x10000,
            PS4 = 0x100000,
            PSMPlayer = 0x200000,
            PSP2 = 0x80000,
            TizenPlayer = 0x1000,
            WiiPlayer = 0x40000,
            WindowsEditor = 1,
            WindowsPlayer = 4,
            WindowsWebPlayer = 0x100,
            WP8Player = 0x2000,
            XBOX360 = 0x20000,
            XboxOne = 0x400000
        }

        private sealed class NullTestComponentImpl : ITestComponent, IComparable<ITestComponent>
        {
            [CompilerGenerated]
            private GameObject <gameObject>k__BackingField;

            public int CompareTo(ITestComponent other)
            {
                if (other == this)
                {
                    return 0;
                }
                return -1;
            }

            public void EnableTest(bool enable)
            {
            }

            public ITestComponent GetTestGroup()
            {
                return null;
            }

            public double GetTimeout()
            {
                throw new NotImplementedException();
            }

            public bool IsExceptionExpected(string exceptionType)
            {
                throw new NotImplementedException();
            }

            public bool IsExludedOnThisPlatform()
            {
                throw new NotImplementedException();
            }

            public bool IsIgnored()
            {
                throw new NotImplementedException();
            }

            public bool IsTestGroup()
            {
                throw new NotImplementedException();
            }

            public bool ShouldSucceedOnAssertions()
            {
                throw new NotImplementedException();
            }

            public bool ShouldSucceedOnException()
            {
                throw new NotImplementedException();
            }

            public GameObject gameObject
            {
                [CompilerGenerated]
                get
                {
                    return this.<gameObject>k__BackingField;
                }
                [CompilerGenerated]
                private set
                {
                    this.<gameObject>k__BackingField = value;
                }
            }

            public string Name
            {
                get
                {
                    return string.Empty;
                }
            }
        }
    }
}

