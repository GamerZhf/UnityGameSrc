namespace Facebook.Unity
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void FacebookDelegate<T>(T result) where T: IResult;
}

