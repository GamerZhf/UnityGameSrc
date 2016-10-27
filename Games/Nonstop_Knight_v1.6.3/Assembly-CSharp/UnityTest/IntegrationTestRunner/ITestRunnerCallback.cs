namespace UnityTest.IntegrationTestRunner
{
    using System;
    using System.Collections.Generic;
    using UnityTest;

    public interface ITestRunnerCallback
    {
        void RunFinished(List<TestResult> testResults);
        void RunStarted(string platform, List<TestComponent> testsToRun);
        void TestFinished(TestResult test);
        void TestRunInterrupted(List<ITestComponent> testsNotRun);
        void TestStarted(TestResult test);
    }
}

