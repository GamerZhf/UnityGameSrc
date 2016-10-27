namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;

    public interface IAppRequestResult : IResult
    {
        string RequestID { get; }

        IEnumerable<string> To { get; }
    }
}

