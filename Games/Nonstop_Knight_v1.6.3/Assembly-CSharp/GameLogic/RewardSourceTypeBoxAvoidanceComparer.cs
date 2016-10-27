namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct RewardSourceTypeBoxAvoidanceComparer : IEqualityComparer<RewardSourceType>
    {
        public bool Equals(RewardSourceType x, RewardSourceType y)
        {
            return (x == y);
        }

        public int GetHashCode(RewardSourceType obj)
        {
            return (int) obj;
        }
    }
}

