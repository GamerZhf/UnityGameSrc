using System;
using UnityTest;

public interface ITestResult
{
    double Duration { get; }

    bool Executed { get; }

    string FullName { get; }

    string Id { get; }

    bool IsIgnored { get; }

    bool IsSuccess { get; }

    string Logs { get; }

    string Message { get; }

    string Name { get; }

    TestResultState ResultState { get; }

    string StackTrace { get; }
}

