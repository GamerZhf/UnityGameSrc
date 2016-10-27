namespace UnityTest
{
    using System;

    [Serializable]
    internal class SerializableTestResult : ITestResult
    {
        public double duration;
        public bool executed;
        public string fullName;
        public string id;
        public bool isIgnored;
        public bool isSuccess;
        public string message;
        public string name;
        public TestResultState resultState;
        public string stackTrace;

        public double Duration
        {
            get
            {
                return this.duration;
            }
        }

        public bool Executed
        {
            get
            {
                return this.executed;
            }
        }

        public string FullName
        {
            get
            {
                return this.fullName;
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public bool IsIgnored
        {
            get
            {
                return this.isIgnored;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return this.isSuccess;
            }
        }

        public string Logs
        {
            get
            {
                return null;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public TestResultState ResultState
        {
            get
            {
                return this.resultState;
            }
        }

        public string StackTrace
        {
            get
            {
                return this.stackTrace;
            }
        }
    }
}

