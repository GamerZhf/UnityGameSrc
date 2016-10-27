namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct BoostTypeBoxAvoidanceComparer : IEqualityComparer<BoostType>
    {
        public bool Equals(BoostType x, BoostType y)
        {
            return (x == y);
        }

        public int GetHashCode(BoostType obj)
        {
            return (int) obj;
        }
    }
}

