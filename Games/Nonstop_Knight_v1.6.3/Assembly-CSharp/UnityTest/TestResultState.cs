namespace UnityTest
{
    using System;

    public enum TestResultState : byte
    {
        Cancelled = 7,
        Error = 6,
        Failure = 5,
        Ignored = 3,
        Inconclusive = 0,
        NotRunnable = 1,
        Skipped = 2,
        Success = 4
    }
}

