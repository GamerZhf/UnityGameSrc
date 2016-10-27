namespace Facebook.Unity
{
    using System;

    internal interface IInternalResult : IResult
    {
        string CallbackId { get; }
    }
}

