namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PerkTypeBoxAvoidanceComparer : IEqualityComparer<PerkType>
    {
        public bool Equals(PerkType x, PerkType y)
        {
            return (x == y);
        }

        public int GetHashCode(PerkType obj)
        {
            return (int) obj;
        }
    }
}

