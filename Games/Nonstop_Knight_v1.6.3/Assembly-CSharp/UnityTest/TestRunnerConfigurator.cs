namespace UnityTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;
    using UnityTest.IntegrationTestRunner;

    public class TestRunnerConfigurator
    {
        [CompilerGenerated]
        private static Func<UnicastIPAddressInformation, bool> <>f__am$cache5;
        [CompilerGenerated]
        private static Func<UnicastIPAddressInformation, string> <>f__am$cache6;
        [CompilerGenerated]
        private static Comparison<UnicastIPAddressInformation> <>f__am$cache7;
        [CompilerGenerated]
        private static Func<UnicastIPAddressInformation, string> <>f__am$cache8;
        [CompilerGenerated]
        private static Func<IPEndPoint, NetworkResultSender> <>f__am$cache9;
        [CompilerGenerated]
        private static Func<IPEndPoint, string> <>f__am$cacheA;
        [CompilerGenerated]
        private bool <isBatchRun>k__BackingField;
        [CompilerGenerated]
        private bool <sendResultsOverNetwork>k__BackingField;
        public static string batchRunFileMarker = "batchrun.txt";
        public static string integrationTestsNetwork = "networkconfig.txt";
        private readonly List<IPEndPoint> m_IPEndPointList = new List<IPEndPoint>();

        public TestRunnerConfigurator()
        {
            this.CheckForBatchMode();
            this.CheckForSendingResultsOverNetwork();
        }

        private void CheckForBatchMode()
        {
            if (GetTextFromTextAsset(batchRunFileMarker) != null)
            {
                this.isBatchRun = true;
            }
        }

        private void CheckForSendingResultsOverNetwork()
        {
            string textFromTempFile;
            if (Application.isEditor)
            {
                textFromTempFile = GetTextFromTempFile(integrationTestsNetwork);
            }
            else
            {
                textFromTempFile = GetTextFromTextAsset(integrationTestsNetwork);
            }
            if (textFromTempFile != null)
            {
                this.sendResultsOverNetwork = true;
                this.m_IPEndPointList.Clear();
                char[] separator = new char[] { '\n' };
                foreach (string str2 in textFromTempFile.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                {
                    int index = str2.IndexOf(':');
                    if (index == -1)
                    {
                        throw new Exception(str2);
                    }
                    string ipString = str2.Substring(0, index);
                    string s = str2.Substring(index + 1);
                    this.m_IPEndPointList.Add(new IPEndPoint(IPAddress.Parse(ipString), int.Parse(s)));
                }
            }
        }

        public static List<string> GetAvailableNetworkIPs()
        {
            List<string> list3;
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                list3 = new List<string>();
                list3.Add(IPAddress.Loopback.ToString());
                return list3;
            }
            List<UnicastIPAddressInformation> source = new List<UnicastIPAddressInformation>();
            List<UnicastIPAddressInformation> list2 = new List<UnicastIPAddressInformation>();
            foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
            {
                if ((interface2.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) || (interface2.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    if (<>f__am$cache5 == null)
                    {
                        <>f__am$cache5 = delegate (UnicastIPAddressInformation a) {
                            return a.Address.AddressFamily == AddressFamily.InterNetwork;
                        };
                    }
                    IEnumerable<UnicastIPAddressInformation> collection = Enumerable.Where<UnicastIPAddressInformation>(interface2.GetIPProperties().UnicastAddresses, <>f__am$cache5);
                    list2.AddRange(collection);
                    if (interface2.OperationalStatus == OperationalStatus.Up)
                    {
                        source.AddRange(collection);
                    }
                }
            }
            if (!Enumerable.Any<UnicastIPAddressInformation>(source))
            {
                if (<>f__am$cache6 == null)
                {
                    <>f__am$cache6 = delegate (UnicastIPAddressInformation i) {
                        return i.Address.ToString();
                    };
                }
                return Enumerable.ToList<string>(Enumerable.Select<UnicastIPAddressInformation, string>(list2, <>f__am$cache6));
            }
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = delegate (UnicastIPAddressInformation ip1, UnicastIPAddressInformation ip2) {
                    int num = BitConverter.ToInt32(Enumerable.ToArray<byte>(Enumerable.Reverse<byte>(ip1.IPv4Mask.GetAddressBytes())), 0);
                    return BitConverter.ToInt32(Enumerable.ToArray<byte>(Enumerable.Reverse<byte>(ip2.IPv4Mask.GetAddressBytes())), 0).CompareTo(num);
                };
            }
            source.Sort(<>f__am$cache7);
            if (source.Count == 0)
            {
                list3 = new List<string>();
                list3.Add(IPAddress.Loopback.ToString());
                return list3;
            }
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = delegate (UnicastIPAddressInformation i) {
                    return i.Address.ToString();
                };
            }
            return Enumerable.ToList<string>(Enumerable.Select<UnicastIPAddressInformation, string>(source, <>f__am$cache8));
        }

        private static string GetTextFromTempFile(string fileName)
        {
            try
            {
            }
            catch
            {
                return null;
            }
            return null;
        }

        private static string GetTextFromTextAsset(string fileName)
        {
            TextAsset asset = Resources.Load(fileName.Substring(0, fileName.LastIndexOf('.'))) as TextAsset;
            return ((asset == null) ? null : asset.text);
        }

        public ITestRunnerCallback ResolveNetworkConnection()
        {
            if (<>f__am$cache9 == null)
            {
                <>f__am$cache9 = delegate (IPEndPoint ipEndPoint) {
                    return new NetworkResultSender(ipEndPoint.Address.ToString(), ipEndPoint.Port);
                };
            }
            List<NetworkResultSender> list = Enumerable.ToList<NetworkResultSender>(Enumerable.Select<IPEndPoint, NetworkResultSender>(this.m_IPEndPointList, <>f__am$cache9));
            TimeSpan span = TimeSpan.FromSeconds(30.0);
            DateTime now = DateTime.Now;
            while ((DateTime.Now - now) < span)
            {
                foreach (NetworkResultSender sender in list)
                {
                    try
                    {
                        if (!sender.Ping())
                        {
                            continue;
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.LogException(exception);
                        this.sendResultsOverNetwork = false;
                        return null;
                    }
                    return sender;
                }
                Thread.Sleep(500);
            }
            if (<>f__am$cacheA == null)
            {
                <>f__am$cacheA = delegate (IPEndPoint ipep) {
                    return ipep.Address + ":" + ipep.Port;
                };
            }
            Debug.LogError("Couldn't connect to the server: " + string.Join(", ", Enumerable.ToArray<string>(Enumerable.Select<IPEndPoint, string>(this.m_IPEndPointList, <>f__am$cacheA))));
            this.sendResultsOverNetwork = false;
            return null;
        }

        public bool isBatchRun
        {
            [CompilerGenerated]
            get
            {
                return this.<isBatchRun>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<isBatchRun>k__BackingField = value;
            }
        }

        public bool sendResultsOverNetwork
        {
            [CompilerGenerated]
            get
            {
                return this.<sendResultsOverNetwork>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<sendResultsOverNetwork>k__BackingField = value;
            }
        }
    }
}

