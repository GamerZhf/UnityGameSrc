namespace UnityTest
{
    using System;
    using UnityEngine;

    public interface ITestComponent : IComparable<ITestComponent>
    {
        void EnableTest(bool enable);
        ITestComponent GetTestGroup();
        double GetTimeout();
        bool IsExceptionExpected(string exceptionType);
        bool IsExludedOnThisPlatform();
        bool IsIgnored();
        bool IsTestGroup();
        bool ShouldSucceedOnAssertions();
        bool ShouldSucceedOnException();

        GameObject gameObject { get; }

        string Name { get; }
    }
}

