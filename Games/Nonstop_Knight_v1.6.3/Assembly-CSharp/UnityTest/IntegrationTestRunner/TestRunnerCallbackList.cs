namespace UnityTest.IntegrationTestRunner
{
    using System;
    using System.Collections.Generic;
    using UnityTest;

    public class TestRunnerCallbackList : ITestRunnerCallback
    {
        private readonly List<ITestRunnerCallback> m_CallbackList = new List<ITestRunnerCallback>();

        public void Add(ITestRunnerCallback callback)
        {
            this.m_CallbackList.Add(callback);
        }

        public void Remove(ITestRunnerCallback callback)
        {
            this.m_CallbackList.Remove(callback);
        }

        public void RunFinished(List<TestResult> testResults)
        {
            foreach (ITestRunnerCallback callback in this.m_CallbackList)
            {
                callback.RunFinished(testResults);
            }
        }

        public void RunStarted(string platform, List<TestComponent> testsToRun)
        {
            foreach (ITestRunnerCallback callback in this.m_CallbackList)
            {
                callback.RunStarted(platform, testsToRun);
            }
        }

        public void TestFinished(TestResult test)
        {
            foreach (ITestRunnerCallback callback in this.m_CallbackList)
            {
                callback.TestFinished(test);
            }
        }

        public void TestRunInterrupted(List<ITestComponent> testsNotRun)
        {
            foreach (ITestRunnerCallback callback in this.m_CallbackList)
            {
                callback.TestRunInterrupted(testsNotRun);
            }
        }

        public void TestStarted(TestResult test)
        {
            foreach (ITestRunnerCallback callback in this.m_CallbackList)
            {
                callback.TestStarted(test);
            }
        }
    }
}

