namespace Facebook.Unity
{
    using System;

    internal interface IFacebookLogger
    {
        void Error(string msg);
        void Info(string msg);
        void Log(string msg);
        void Warn(string msg);
    }
}

