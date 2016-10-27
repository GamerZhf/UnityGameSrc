namespace Facebook.Unity
{
    using System;

    internal class PayResult : ResultBase, IPayResult, IResult
    {
        internal PayResult(string result) : base(result)
        {
        }
    }
}

