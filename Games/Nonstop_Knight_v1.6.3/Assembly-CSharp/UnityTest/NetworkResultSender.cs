namespace UnityTest
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using UnityEngine;
    using UnityTest.IntegrationTestRunner;

    public class NetworkResultSender : ITestRunnerCallback
    {
        private readonly TimeSpan m_ConnectionTimeout = TimeSpan.FromSeconds(5.0);
        private readonly string m_Ip;
        private bool m_LostConnection;
        private readonly int m_Port;

        public NetworkResultSender(string ip, int port)
        {
            this.m_Ip = ip;
            this.m_Port = port;
        }

        public bool Ping()
        {
            bool flag = this.SendDTO(ResultDTO.CreatePing());
            this.m_LostConnection = false;
            return flag;
        }

        public void RunFinished(List<TestResult> testResults)
        {
            this.SendDTO(ResultDTO.CreateRunFinished(testResults));
        }

        public void RunStarted(string platform, List<TestComponent> testsToRun)
        {
            this.SendDTO(ResultDTO.CreateRunStarted());
        }

        private bool SendDTO(ResultDTO dto)
        {
            if (this.m_LostConnection)
            {
                return false;
            }
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    IAsyncResult asyncResult = client.BeginConnect(this.m_Ip, this.m_Port, null, null);
                    if (!asyncResult.AsyncWaitHandle.WaitOne(this.m_ConnectionTimeout))
                    {
                        return false;
                    }
                    try
                    {
                        client.EndConnect(asyncResult);
                    }
                    catch (SocketException)
                    {
                        this.m_LostConnection = true;
                        return false;
                    }
                    new DTOFormatter().Serialize(client.GetStream(), dto);
                    client.GetStream().Close();
                    client.Close();
                    Debug.Log("Sent " + dto.messageType);
                }
            }
            catch (SocketException exception)
            {
                Debug.LogException(exception);
                this.m_LostConnection = true;
                return false;
            }
            return true;
        }

        public void TestFinished(TestResult test)
        {
            this.SendDTO(ResultDTO.CreateTestFinished(test));
        }

        public void TestRunInterrupted(List<ITestComponent> testsNotRun)
        {
            this.RunFinished(new List<TestResult>());
        }

        public void TestStarted(TestResult test)
        {
            this.SendDTO(ResultDTO.CreateTestStarted(test));
        }
    }
}

