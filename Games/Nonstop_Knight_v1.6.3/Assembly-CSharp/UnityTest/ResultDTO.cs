namespace UnityTest
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class ResultDTO
    {
        public int levelCount;
        public int loadedLevel;
        public string loadedLevelName;
        public MessageType messageType;
        public string testName;
        public ITestResult testResult;
        public float testTimeout;

        private ResultDTO(MessageType messageType)
        {
            this.messageType = messageType;
            this.levelCount = Application.levelCount;
            this.loadedLevel = Application.loadedLevel;
            this.loadedLevelName = Application.loadedLevelName;
        }

        public static ResultDTO CreatePing()
        {
            return new ResultDTO(MessageType.Ping);
        }

        public static ResultDTO CreateRunFinished(List<TestResult> testResults)
        {
            return new ResultDTO(MessageType.RunFinished);
        }

        public static ResultDTO CreateRunStarted()
        {
            return new ResultDTO(MessageType.RunStarted);
        }

        public static ResultDTO CreateTestFinished(TestResult test)
        {
            ResultDTO tdto = new ResultDTO(MessageType.TestFinished);
            tdto.testName = test.FullName;
            tdto.testResult = GetSerializableTestResult(test);
            return tdto;
        }

        public static ResultDTO CreateTestStarted(TestResult test)
        {
            ResultDTO tdto = new ResultDTO(MessageType.TestStarted);
            tdto.testName = test.FullName;
            tdto.testTimeout = test.TestComponent.timeout;
            return tdto;
        }

        private static ITestResult GetSerializableTestResult(TestResult test)
        {
            SerializableTestResult result = new SerializableTestResult();
            result.resultState = test.ResultState;
            result.message = test.messages;
            result.executed = test.Executed;
            result.name = test.Name;
            result.fullName = test.FullName;
            result.id = test.id;
            result.isSuccess = test.IsSuccess;
            result.duration = test.duration;
            result.stackTrace = test.stacktrace;
            result.isIgnored = test.IsIgnored;
            return result;
        }

        public enum MessageType : byte
        {
            Ping = 0,
            RunFinished = 2,
            RunInterrupted = 5,
            RunStarted = 1,
            TestFinished = 4,
            TestStarted = 3
        }
    }
}

