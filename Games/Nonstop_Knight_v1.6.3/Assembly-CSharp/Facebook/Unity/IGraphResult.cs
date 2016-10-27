namespace Facebook.Unity
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IGraphResult : IResult
    {
        IList<object> ResultList { get; }

        Texture2D Texture { get; }
    }
}

