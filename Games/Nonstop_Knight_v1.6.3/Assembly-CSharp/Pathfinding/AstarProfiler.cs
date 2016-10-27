namespace Pathfinding
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using UnityEngine;

    public class AstarProfiler
    {
        public static string[] fastProfileNames;
        public static ProfilePoint[] fastProfiles;
        private static readonly Dictionary<string, ProfilePoint> profiles = new Dictionary<string, ProfilePoint>();
        private static DateTime startTime = DateTime.UtcNow;

        private AstarProfiler()
        {
        }

        [Conditional("ProfileAstar")]
        public static void EndFastProfile(int tag)
        {
            ProfilePoint point = fastProfiles[tag];
            point.totalCalls++;
            point.watch.Stop();
        }

        [Conditional("ASTAR_UNITY_PRO_PROFILER")]
        public static void EndProfile()
        {
        }

        [Conditional("ProfileAstar")]
        public static void EndProfile(string tag)
        {
            if (!profiles.ContainsKey(tag))
            {
                UnityEngine.Debug.LogError("Can only end profiling for a tag which has already been started (tag was " + tag + ")");
            }
            else
            {
                ProfilePoint point = profiles[tag];
                point.totalCalls++;
                point.watch.Stop();
                point.totalBytes += GC.GetTotalMemory(false) - point.tmpBytes;
            }
        }

        [Conditional("ProfileAstar")]
        public static void InitializeFastProfile(string[] profileNames)
        {
            fastProfileNames = new string[profileNames.Length + 2];
            Array.Copy(profileNames, fastProfileNames, profileNames.Length);
            fastProfileNames[fastProfileNames.Length - 2] = "__Control1__";
            fastProfileNames[fastProfileNames.Length - 1] = "__Control2__";
            fastProfiles = new ProfilePoint[fastProfileNames.Length];
            for (int i = 0; i < fastProfiles.Length; i++)
            {
                fastProfiles[i] = new ProfilePoint();
            }
        }

        [Conditional("ProfileAstar")]
        public static void PrintFastResults()
        {
            if (fastProfiles != null)
            {
                for (int i = 0; i < 0x3e8; i++)
                {
                }
                double num2 = fastProfiles[fastProfiles.Length - 2].watch.Elapsed.TotalMilliseconds / 1000.0;
                TimeSpan span = (TimeSpan) (DateTime.UtcNow - startTime);
                StringBuilder builder = new StringBuilder();
                builder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
                builder.Append("Name\t\t|\tTotal Time\t|\tTotal Calls\t|\tAvg/Call\t|\tBytes");
                for (int j = 0; j < fastProfiles.Length; j++)
                {
                    string str = fastProfileNames[j];
                    ProfilePoint point = fastProfiles[j];
                    int totalCalls = point.totalCalls;
                    double num5 = point.watch.Elapsed.TotalMilliseconds - (num2 * totalCalls);
                    if (totalCalls >= 1)
                    {
                        builder.Append("\n").Append(str.PadLeft(10)).Append("|   ");
                        builder.Append(num5.ToString("0.0 ").PadLeft(10)).Append(point.watch.Elapsed.TotalMilliseconds.ToString("(0.0)").PadLeft(10)).Append("|   ");
                        builder.Append(totalCalls.ToString().PadLeft(10)).Append("|   ");
                        double num7 = num5 / ((double) totalCalls);
                        builder.Append(num7.ToString("0.000").PadLeft(10));
                    }
                }
                builder.Append("\n\n============================\n\t\tTotal runtime: ");
                builder.Append(span.TotalSeconds.ToString("F3"));
                builder.Append(" seconds\n============================");
                UnityEngine.Debug.Log(builder.ToString());
            }
        }

        [Conditional("ProfileAstar")]
        public static void PrintResults()
        {
            TimeSpan span = (TimeSpan) (DateTime.UtcNow - startTime);
            StringBuilder builder = new StringBuilder();
            builder.Append("============================\n\t\t\t\tProfile results:\n============================\n");
            int num = 5;
            foreach (KeyValuePair<string, ProfilePoint> pair in profiles)
            {
                num = Math.Max(pair.Key.Length, num);
            }
            builder.Append(" Name ".PadRight(num)).Append("|").Append(" Total Time\t".PadRight(20)).Append("|").Append(" Total Calls ".PadRight(20)).Append("|").Append(" Avg/Call ".PadRight(20));
            foreach (KeyValuePair<string, ProfilePoint> pair2 in profiles)
            {
                double totalMilliseconds = pair2.Value.watch.Elapsed.TotalMilliseconds;
                int totalCalls = pair2.Value.totalCalls;
                if (totalCalls >= 1)
                {
                    string key = pair2.Key;
                    builder.Append("\n").Append(key.PadRight(num)).Append("| ");
                    builder.Append(totalMilliseconds.ToString("0.0").PadRight(20)).Append("| ");
                    builder.Append(totalCalls.ToString().PadRight(20)).Append("| ");
                    double num4 = totalMilliseconds / ((double) totalCalls);
                    builder.Append(num4.ToString("0.000").PadRight(20));
                    builder.Append(AstarMath.FormatBytesBinary((int) pair2.Value.totalBytes).PadLeft(10));
                }
            }
            builder.Append("\n\n============================\n\t\tTotal runtime: ");
            builder.Append(span.TotalSeconds.ToString("F3"));
            builder.Append(" seconds\n============================");
            UnityEngine.Debug.Log(builder.ToString());
        }

        [Conditional("ProfileAstar")]
        public static void Reset()
        {
            profiles.Clear();
            startTime = DateTime.UtcNow;
            if (fastProfiles != null)
            {
                for (int i = 0; i < fastProfiles.Length; i++)
                {
                    fastProfiles[i] = new ProfilePoint();
                }
            }
        }

        [Conditional("ProfileAstar")]
        public static void StartFastProfile(int tag)
        {
            fastProfiles[tag].watch.Start();
        }

        [Conditional("ProfileAstar")]
        public static void StartProfile(string tag)
        {
            ProfilePoint point;
            profiles.TryGetValue(tag, out point);
            if (point == null)
            {
                point = new ProfilePoint();
                profiles[tag] = point;
            }
            point.tmpBytes = GC.GetTotalMemory(false);
            point.watch.Start();
        }

        public class ProfilePoint
        {
            public long tmpBytes;
            public long totalBytes;
            public int totalCalls;
            public Stopwatch watch = new Stopwatch();
        }
    }
}

