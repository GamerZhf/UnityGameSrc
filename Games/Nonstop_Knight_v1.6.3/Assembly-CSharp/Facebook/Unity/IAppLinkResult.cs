namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;

    public interface IAppLinkResult : IResult
    {
        IDictionary<string, object> Extras { get; }

        string Ref { get; }

        string TargetUrl { get; }

        string Url { get; }
    }
}

