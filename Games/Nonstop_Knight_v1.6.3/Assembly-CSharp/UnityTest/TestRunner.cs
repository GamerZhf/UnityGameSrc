namespace UnityTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityTest.IntegrationTestRunner;

    [Serializable]
    public class TestRunner : MonoBehaviour
    {
        [CompilerGenerated]
        private static Func<TestResult, bool> <>f__am$cache10;
        [CompilerGenerated]
        private static Func<TestResult, bool> <>f__am$cache11;
        [CompilerGenerated]
        private static Func<TestResult, bool> <>f__am$cache12;
        [CompilerGenerated]
        private static Func<TestResult, string> <>f__am$cache13;
        [CompilerGenerated]
        private static Func<TestResult, bool> <>f__am$cache14;
        [CompilerGenerated]
        private static Func<TestResult, bool> <>f__am$cache15;
        [CompilerGenerated]
        private static Func<TestResult, bool> <>f__am$cache16;
        [CompilerGenerated]
        private static Func<TestResult, string> <>f__am$cache17;
        [CompilerGenerated]
        private static Func<System.Type, string> <>f__am$cacheC;
        [CompilerGenerated]
        private static Func<TestComponent, TestResult> <>f__am$cacheD;
        [CompilerGenerated]
        private static Func<TestResult, ITestComponent> <>f__am$cacheE;
        [CompilerGenerated]
        private static Func<TestResult, bool> <>f__am$cacheF;
        public TestComponent currentTest;
        private const string k_FailedExceptionMessage = "IntegrationTest Failed with exception";
        private const string k_FailedMessage = "IntegrationTest Failed";
        private const string k_FinishedMessage = "IntegrationTest Finished";
        private const string k_IgnoredMessage = "IntegrationTest Ignored";
        private const string k_InterruptedMessage = "IntegrationTest Run interrupted";
        private const string k_Prefix = "IntegrationTest";
        private static readonly TestResultRenderer k_ResultRenderer = new TestResultRenderer();
        private const string k_StartedMessage = "IntegrationTest Started";
        private const string k_TimeoutMessage = "IntegrationTest Timeout";
        private TestRunnerConfigurator m_Configurator;
        private bool m_ReadyToRun;
        private List<TestResult> m_ResultList = new List<TestResult>();
        private string m_Stacktrace;
        private double m_StartTime;
        private List<TestComponent> m_TestComponents;
        private string m_TestMessages;
        private IntegrationTestsProvider m_TestsProvider;
        private TestState m_TestState;
        public TestRunnerCallbackList TestRunnerCallback = new TestRunnerCallbackList();

        public void Awake()
        {
            this.m_Configurator = new TestRunnerConfigurator();
            if (!this.isInitializedByRunner)
            {
                TestComponent.DisableAllTests();
            }
        }

        private static GameObject Create()
        {
            GameObject obj2 = new GameObject("TestRunner");
            obj2.AddComponent<TestRunner>();
            UnityEngine.Debug.Log("Created Test Runner");
            return obj2;
        }

        private void FinishTest(TestResult.ResultType result)
        {
            this.m_TestsProvider.FinishTest(this.currentTest);
            TestResult test = Enumerable.Single<TestResult>(this.m_ResultList, delegate (TestResult t) {
                return t.GameObject == this.currentTest.gameObject;
            });
            test.resultType = result;
            test.duration = Time.time - this.m_StartTime;
            test.messages = this.m_TestMessages;
            test.stacktrace = this.m_Stacktrace;
            this.TestRunnerCallback.TestFinished(test);
            this.currentTest = null;
            if ((!test.IsSuccess && test.Executed) && !test.IsIgnored)
            {
                k_ResultRenderer.AddResults(Application.loadedLevelName, test);
            }
        }

        private void FinishTestRun()
        {
            this.PrintResultToLog();
            this.TestRunnerCallback.RunFinished(this.m_ResultList);
            this.LoadNextLevelOrQuit();
        }

        public static TestRunner GetTestRunner()
        {
            TestRunner runner = null;
            UnityEngine.Object[] source = Resources.FindObjectsOfTypeAll(typeof(TestRunner));
            if (Enumerable.Count<UnityEngine.Object>(source) > 1)
            {
                foreach (UnityEngine.Object obj2 in source)
                {
                    UnityEngine.Object.DestroyImmediate(((TestRunner) obj2).gameObject);
                }
                return runner;
            }
            if (!Enumerable.Any<UnityEngine.Object>(source))
            {
                return Create().GetComponent<TestRunner>();
            }
            return (Enumerable.Single<UnityEngine.Object>(source) as TestRunner);
        }

        public void InitRunner(List<TestComponent> tests, List<string> dynamicTestsToRun)
        {
            Application.logMessageReceived += new Application.LogCallback(this.LogHandler);
            foreach (string str in dynamicTestsToRun)
            {
                System.Type type = System.Type.GetType(str);
                if (type != null)
                {
                    MonoBehaviour[] source = Resources.FindObjectsOfTypeAll(type) as MonoBehaviour[];
                    if (source.Length == 0)
                    {
                        UnityEngine.Debug.LogWarning(type + " not found. Skipping.");
                    }
                    else
                    {
                        if (source.Length > 1)
                        {
                            UnityEngine.Debug.LogWarning("Multiple GameObjects refer to " + str);
                        }
                        tests.Add(Enumerable.First<MonoBehaviour>(source).GetComponent<TestComponent>());
                    }
                }
            }
            this.m_TestComponents = Enumerable.ToList<TestComponent>(ParseListForGroups(tests));
            if (<>f__am$cacheD == null)
            {
                <>f__am$cacheD = delegate (TestComponent component) {
                    return new TestResult(component);
                };
            }
            this.m_ResultList = Enumerable.ToList<TestResult>(Enumerable.Select<TestComponent, TestResult>(this.m_TestComponents, <>f__am$cacheD));
            if (<>f__am$cacheE == null)
            {
                <>f__am$cacheE = delegate (TestResult result) {
                    return result.TestComponent;
                };
            }
            this.m_TestsProvider = new IntegrationTestsProvider(Enumerable.Select<TestResult, ITestComponent>(this.m_ResultList, <>f__am$cacheE));
            this.m_ReadyToRun = true;
        }

        private static bool IsBatchMode()
        {
            System.Type type = System.Type.GetType("UnityEditorInternal.InternalEditorUtility, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", false);
            if (type == null)
            {
                return false;
            }
            return (bool) type.GetProperty("inBatchMode").GetValue(null, null);
        }

        private void LoadNextLevelOrQuit()
        {
            if (!this.isInitializedByRunner)
            {
                if (Application.loadedLevel < (Application.levelCount - 1))
                {
                    Application.LoadLevel((int) (Application.loadedLevel + 1));
                }
                else
                {
                    k_ResultRenderer.ShowResults();
                    if (this.m_Configurator.isBatchRun && this.m_Configurator.sendResultsOverNetwork)
                    {
                        Application.Quit();
                    }
                }
            }
        }

        private void LogHandler(string condition, string stacktrace, LogType type)
        {
            if (!condition.StartsWith("IntegrationTest Started") && !condition.StartsWith("IntegrationTest Finished"))
            {
                string str = condition;
                if (str.StartsWith("IntegrationTest"))
                {
                    str = str.Substring("IntegrationTest".Length + 1);
                }
                if ((this.currentTest != null) && str.EndsWith("(" + this.currentTest.name + ')'))
                {
                    str = str.Substring(0, str.LastIndexOf('('));
                }
                this.m_TestMessages = this.m_TestMessages + str + "\n";
            }
            switch (type)
            {
                case LogType.Error:
                case LogType.Assert:
                    this.m_TestState = TestState.Failure;
                    this.m_Stacktrace = stacktrace;
                    break;

                case LogType.Log:
                    if ((this.m_TestState == TestState.Running) && condition.StartsWith("IntegrationTest Pass"))
                    {
                        this.m_TestState = TestState.Success;
                    }
                    if (condition.StartsWith("IntegrationTest Fail"))
                    {
                        this.m_TestState = TestState.Failure;
                    }
                    break;

                case LogType.Exception:
                {
                    string exception = condition.Substring(0, condition.IndexOf(':'));
                    if ((this.currentTest == null) || !this.currentTest.IsExceptionExpected(exception))
                    {
                        this.m_TestState = TestState.Exception;
                        this.m_Stacktrace = stacktrace;
                        break;
                    }
                    this.m_TestMessages = this.m_TestMessages + exception + " was expected\n";
                    if (this.currentTest.ShouldSucceedOnException())
                    {
                        this.m_TestState = TestState.Success;
                    }
                    break;
                }
            }
        }

        private void LogMessage(string message)
        {
            if (this.currentTest != null)
            {
                UnityEngine.Debug.Log(message + " (" + this.currentTest.Name + ")", this.currentTest.gameObject);
            }
            else
            {
                UnityEngine.Debug.Log(message);
            }
        }

        public void OnDestroy()
        {
            if (this.currentTest != null)
            {
                TestResult local1 = Enumerable.Single<TestResult>(this.m_ResultList, delegate (TestResult result) {
                    return result.TestComponent == this.currentTest;
                });
                local1.messages = local1.messages + "Test run interrupted (crash?)";
                this.LogMessage("IntegrationTest Run interrupted");
                this.FinishTest(TestResult.ResultType.Failed);
            }
            if ((this.currentTest != null) || ((this.m_TestsProvider != null) && this.m_TestsProvider.AnyTestsLeft()))
            {
                List<ITestComponent> remainingTests = this.m_TestsProvider.GetRemainingTests();
                this.TestRunnerCallback.TestRunInterrupted(Enumerable.ToList<ITestComponent>(remainingTests));
            }
            Application.logMessageReceived -= new Application.LogCallback(this.LogHandler);
        }

        public void OnGUI()
        {
            k_ResultRenderer.Draw();
        }

        private static IEnumerable<TestComponent> ParseListForGroups(IEnumerable<TestComponent> tests)
        {
            HashSet<TestComponent> set = new HashSet<TestComponent>();
            <ParseListForGroups>c__AnonStorey265 storey = new <ParseListForGroups>c__AnonStorey265();
            IEnumerator<TestComponent> enumerator = tests.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    storey.testResult = enumerator.Current;
                    if (storey.testResult.IsTestGroup())
                    {
                        foreach (TestComponent component in Enumerable.ToArray<TestComponent>(Enumerable.Cast<TestComponent>(Enumerable.Where<Component>(storey.testResult.gameObject.GetComponentsInChildren(typeof(TestComponent), true), new Func<Component, bool>(storey.<>m__69)))))
                        {
                            if (!component.IsTestGroup())
                            {
                                set.Add(component);
                            }
                        }
                    }
                    else
                    {
                        set.Add(storey.testResult);
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
            return set;
        }

        private void PrintResultToLog()
        {
            if (<>f__am$cacheF == null)
            {
                <>f__am$cacheF = delegate (TestResult t) {
                    return t.IsSuccess;
                };
            }
            string message = string.Empty + "Passed: " + Enumerable.Count<TestResult>(this.m_ResultList, <>f__am$cacheF);
            if (<>f__am$cache10 == null)
            {
                <>f__am$cache10 = delegate (TestResult result) {
                    return result.IsFailure;
                };
            }
            if (Enumerable.Any<TestResult>(this.m_ResultList, <>f__am$cache10))
            {
                if (<>f__am$cache11 == null)
                {
                    <>f__am$cache11 = delegate (TestResult t) {
                        return t.IsFailure;
                    };
                }
                message = message + " Failed: " + Enumerable.Count<TestResult>(this.m_ResultList, <>f__am$cache11);
                if (<>f__am$cache12 == null)
                {
                    <>f__am$cache12 = delegate (TestResult t) {
                        return t.IsFailure;
                    };
                }
                if (<>f__am$cache13 == null)
                {
                    <>f__am$cache13 = delegate (TestResult result) {
                        return result.Name;
                    };
                }
                UnityEngine.Debug.Log("Failed tests: " + string.Join(", ", Enumerable.ToArray<string>(Enumerable.Select<TestResult, string>(Enumerable.Where<TestResult>(this.m_ResultList, <>f__am$cache12), <>f__am$cache13))));
            }
            if (<>f__am$cache14 == null)
            {
                <>f__am$cache14 = delegate (TestResult result) {
                    return result.IsIgnored;
                };
            }
            if (Enumerable.Any<TestResult>(this.m_ResultList, <>f__am$cache14))
            {
                if (<>f__am$cache15 == null)
                {
                    <>f__am$cache15 = delegate (TestResult t) {
                        return t.IsIgnored;
                    };
                }
                message = message + " Ignored: " + Enumerable.Count<TestResult>(this.m_ResultList, <>f__am$cache15);
                if (<>f__am$cache16 == null)
                {
                    <>f__am$cache16 = delegate (TestResult t) {
                        return t.IsIgnored;
                    };
                }
                if (<>f__am$cache17 == null)
                {
                    <>f__am$cache17 = delegate (TestResult result) {
                        return result.Name;
                    };
                }
                UnityEngine.Debug.Log("Ignored tests: " + string.Join(", ", Enumerable.ToArray<string>(Enumerable.Select<TestResult, string>(Enumerable.Where<TestResult>(this.m_ResultList, <>f__am$cache16), <>f__am$cache17))));
            }
            UnityEngine.Debug.Log(message);
        }

        public void Start()
        {
            if (!this.isInitializedByRunner)
            {
                if (this.m_Configurator.sendResultsOverNetwork)
                {
                    ITestRunnerCallback callback = this.m_Configurator.ResolveNetworkConnection();
                    if (callback != null)
                    {
                        this.TestRunnerCallback.Add(callback);
                    }
                }
                TestComponent.DestroyAllDynamicTests();
                IEnumerable<System.Type> typesWithHelpAttribute = TestComponent.GetTypesWithHelpAttribute(Application.loadedLevelName);
                IEnumerator<System.Type> enumerator = typesWithHelpAttribute.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        System.Type current = enumerator.Current;
                        TestComponent.CreateDynamicTest(current);
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                List<TestComponent> tests = TestComponent.FindAllTestsOnScene();
                if (<>f__am$cacheC == null)
                {
                    <>f__am$cacheC = delegate (System.Type type) {
                        return type.AssemblyQualifiedName;
                    };
                }
                this.InitRunner(tests, Enumerable.ToList<string>(Enumerable.Select<System.Type, string>(typesWithHelpAttribute, <>f__am$cacheC)));
            }
        }

        private void StartNewTest()
        {
            this.m_TestMessages = string.Empty;
            this.m_Stacktrace = string.Empty;
            this.m_TestState = TestState.Running;
            this.m_StartTime = Time.time;
            this.currentTest = this.m_TestsProvider.GetNextTest() as TestComponent;
            TestResult test = Enumerable.Single<TestResult>(this.m_ResultList, delegate (TestResult result) {
                return result.TestComponent == this.currentTest;
            });
            if ((this.currentTest != null) && this.currentTest.IsExludedOnThisPlatform())
            {
                this.m_TestState = TestState.Ignored;
                UnityEngine.Debug.Log(this.currentTest.gameObject.name + " is excluded on this platform");
            }
            if (((this.currentTest != null) && this.currentTest.IsIgnored()) && (!this.isInitializedByRunner || (this.m_ResultList.Count != 1)))
            {
                this.m_TestState = TestState.Ignored;
            }
            this.LogMessage("IntegrationTest Started");
            this.TestRunnerCallback.TestStarted(test);
        }

        [DebuggerHidden]
        public IEnumerator StateMachine()
        {
            <StateMachine>c__Iterator22 iterator = new <StateMachine>c__Iterator22();
            iterator.<>f__this = this;
            return iterator;
        }

        public void Update()
        {
            if (this.m_ReadyToRun && (Time.frameCount > 1))
            {
                this.m_ReadyToRun = false;
                base.StartCoroutine("StateMachine");
            }
        }

        public bool isInitializedByRunner
        {
            get
            {
                return (Application.isEditor && !IsBatchMode());
            }
        }

        [CompilerGenerated]
        private sealed class <ParseListForGroups>c__AnonStorey265
        {
            internal TestComponent testResult;

            internal bool <>m__69(Component t)
            {
                return (t != this.testResult);
            }
        }

        [CompilerGenerated]
        private sealed class <StateMachine>c__Iterator22 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            private static Func<AssertionComponent, bool> <>f__am$cache4;
            private static Func<AssertionComponent, bool> <>f__am$cache5;
            internal TestRunner <>f__this;
            internal AssertionComponent[] <assertionsToCheck>__0;

            private static bool <>m__76(AssertionComponent a)
            {
                return a.enabled;
            }

            private static bool <>m__77(AssertionComponent a)
            {
                return (a.checksPerformed > 0);
            }

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
                        this.<>f__this.TestRunnerCallback.RunStarted(Application.platform.ToString(), this.<>f__this.m_TestComponents);
                        break;

                    case 1:
                        break;
                        this.$PC = -1;
                        goto Label_02AC;

                    default:
                        goto Label_02AC;
                }
                if (!this.<>f__this.m_TestsProvider.AnyTestsLeft() && (this.<>f__this.currentTest == null))
                {
                    this.<>f__this.FinishTestRun();
                }
                else
                {
                    if (this.<>f__this.currentTest == null)
                    {
                        this.<>f__this.StartNewTest();
                    }
                    if (this.<>f__this.currentTest != null)
                    {
                        if (this.<>f__this.m_TestState == TestRunner.TestState.Running)
                        {
                            if (this.<>f__this.currentTest.ShouldSucceedOnAssertions())
                            {
                                if (<>f__am$cache4 == null)
                                {
                                    <>f__am$cache4 = new Func<AssertionComponent, bool>(TestRunner.<StateMachine>c__Iterator22.<>m__76);
                                }
                                this.<assertionsToCheck>__0 = Enumerable.ToArray<AssertionComponent>(Enumerable.Where<AssertionComponent>(this.<>f__this.currentTest.gameObject.GetComponentsInChildren<AssertionComponent>(), <>f__am$cache4));
                                if (<>f__am$cache5 == null)
                                {
                                    <>f__am$cache5 = new Func<AssertionComponent, bool>(TestRunner.<StateMachine>c__Iterator22.<>m__77);
                                }
                                if (Enumerable.All<AssertionComponent>(this.<assertionsToCheck>__0, <>f__am$cache5))
                                {
                                    IntegrationTest.Pass(this.<>f__this.currentTest.gameObject);
                                    this.<>f__this.m_TestState = TestRunner.TestState.Success;
                                }
                            }
                            if ((this.<>f__this.currentTest != null) && (Time.time > (this.<>f__this.m_StartTime + this.<>f__this.currentTest.GetTimeout())))
                            {
                                this.<>f__this.m_TestState = TestRunner.TestState.Timeout;
                            }
                        }
                        switch (this.<>f__this.m_TestState)
                        {
                            case TestRunner.TestState.Success:
                                this.<>f__this.LogMessage("IntegrationTest Finished");
                                this.<>f__this.FinishTest(TestResult.ResultType.Success);
                                break;

                            case TestRunner.TestState.Failure:
                                this.<>f__this.LogMessage("IntegrationTest Failed");
                                this.<>f__this.FinishTest(TestResult.ResultType.Failed);
                                break;

                            case TestRunner.TestState.Exception:
                                this.<>f__this.LogMessage("IntegrationTest Failed with exception");
                                this.<>f__this.FinishTest(TestResult.ResultType.FailedException);
                                break;

                            case TestRunner.TestState.Timeout:
                                this.<>f__this.LogMessage("IntegrationTest Timeout");
                                this.<>f__this.FinishTest(TestResult.ResultType.Timeout);
                                break;

                            case TestRunner.TestState.Ignored:
                                this.<>f__this.LogMessage("IntegrationTest Ignored");
                                this.<>f__this.FinishTest(TestResult.ResultType.Ignored);
                                break;
                        }
                    }
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_02AC:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
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

        private enum TestState
        {
            Running,
            Success,
            Failure,
            Exception,
            Timeout,
            Ignored
        }
    }
}

