namespace Service
{
    using System;

    public class DummyLogger
    {
        public void Log(string _msg)
        {
            ServiceLogger.Log(_msg);
        }
    }
}

