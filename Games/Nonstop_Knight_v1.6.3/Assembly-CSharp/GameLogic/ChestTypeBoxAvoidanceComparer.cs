namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ChestTypeBoxAvoidanceComparer : IEqualityComparer<ChestType>
    {
        public bool Equals(ChestType x, ChestType y)
        {
            return (x == y);
        }

        public int GetHashCode(ChestType obj)
        {
            return (int) obj;
        }
    }
}

