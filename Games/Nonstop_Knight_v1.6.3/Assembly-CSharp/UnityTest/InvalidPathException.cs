namespace UnityTest
{
    using System;

    public class InvalidPathException : Exception
    {
        public InvalidPathException(string path) : base("Invalid path part " + path)
        {
        }
    }
}

