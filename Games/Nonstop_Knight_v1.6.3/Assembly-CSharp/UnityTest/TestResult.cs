namespace UnityTest
{
    using System;
    using UnityEngine;

    [Serializable]
    public class TestResult : ITestResult, IComparable<TestResult>
    {
        public double duration;
        public bool dynamicTest;
        public string id;
        private readonly UnityEngine.GameObject m_Go;
        private string m_Name;
        public string messages;
        public ResultType resultType = ResultType.NotRun;
        public string stacktrace;
        public UnityTest.TestComponent TestComponent;

        public TestResult(UnityTest.TestComponent testComponent)
        {
            this.TestComponent = testComponent;
            this.m_Go = testComponent.gameObject;
            this.id = testComponent.gameObject.GetInstanceID().ToString();
            this.dynamicTest = testComponent.dynamic;
            if (this.m_Go != null)
            {
                this.m_Name = this.m_Go.name;
            }
            if (this.dynamicTest)
            {
                this.id = testComponent.dynamicTypeName;
            }
        }

        public int CompareTo(TestResult other)
        {
            int num = this.Name.CompareTo(other.Name);
            if (num == 0)
            {
                num = this.m_Go.GetInstanceID().CompareTo(other.m_Go.GetInstanceID());
            }
            return num;
        }

        public override bool Equals(object obj)
        {
            if (obj is TestResult)
            {
                return (this.GetHashCode() == obj.GetHashCode());
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public void Reset()
        {
            this.resultType = ResultType.NotRun;
            this.duration = 0.0;
            this.messages = string.Empty;
            this.stacktrace = string.Empty;
        }

        public void Update(TestResult oldResult)
        {
            this.resultType = oldResult.resultType;
            this.duration = oldResult.duration;
            this.messages = oldResult.messages;
            this.stacktrace = oldResult.stacktrace;
        }

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
                return (this.resultType != ResultType.NotRun);
            }
        }

        public string FullName
        {
            get
            {
                string name = this.Name;
                if (this.m_Go != null)
                {
                    for (Transform transform = this.m_Go.transform.parent; transform != null; transform = transform.transform.parent)
                    {
                        name = transform.name + "." + name;
                    }
                }
                return name;
            }
        }

        public UnityEngine.GameObject GameObject
        {
            get
            {
                return this.m_Go;
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public bool IsFailure
        {
            get
            {
                return (((this.resultType == ResultType.Failed) || (this.resultType == ResultType.FailedException)) || (this.resultType == ResultType.Timeout));
            }
        }

        public bool IsIgnored
        {
            get
            {
                return (this.resultType == ResultType.Ignored);
            }
        }

        public bool IsSuccess
        {
            get
            {
                return (this.resultType == ResultType.Success);
            }
        }

        public bool IsTimeout
        {
            get
            {
                return (this.resultType == ResultType.Timeout);
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
                return this.messages;
            }
        }

        public string Name
        {
            get
            {
                if (this.m_Go != null)
                {
                    this.m_Name = this.m_Go.name;
                }
                return this.m_Name;
            }
        }

        public TestResultState ResultState
        {
            get
            {
                switch (this.resultType)
                {
                    case ResultType.Success:
                        return TestResultState.Success;

                    case ResultType.Failed:
                        return TestResultState.Failure;

                    case ResultType.Timeout:
                        return TestResultState.Cancelled;

                    case ResultType.NotRun:
                        return TestResultState.Skipped;

                    case ResultType.FailedException:
                        return TestResultState.Error;

                    case ResultType.Ignored:
                        return TestResultState.Ignored;
                }
                throw new Exception();
            }
        }

        public string StackTrace
        {
            get
            {
                return this.stacktrace;
            }
        }

        public enum ResultType
        {
            Success,
            Failed,
            Timeout,
            NotRun,
            FailedException,
            Ignored
        }
    }
}

