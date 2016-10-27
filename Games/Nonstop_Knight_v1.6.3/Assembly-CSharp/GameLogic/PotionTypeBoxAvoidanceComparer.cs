namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PotionTypeBoxAvoidanceComparer : IEqualityComparer<PotionType>
    {
        public bool Equals(PotionType x, PotionType y)
        {
            return (x == y);
        }

        public int GetHashCode(PotionType obj)
        {
            return (int) obj;
        }
    }
}

