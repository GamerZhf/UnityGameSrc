namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct LeaderboardTypeBoxAvoidanceComparer : IEqualityComparer<LeaderboardType>
    {
        public bool Equals(LeaderboardType x, LeaderboardType y)
        {
            return (x == y);
        }

        public int GetHashCode(LeaderboardType obj)
        {
            return (int) obj;
        }
    }
}

