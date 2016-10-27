namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DungeonBoostTypeBoxAvoidanceComparer : IEqualityComparer<DungeonBoostType>
    {
        public bool Equals(DungeonBoostType x, DungeonBoostType y)
        {
            return (x == y);
        }

        public int GetHashCode(DungeonBoostType obj)
        {
            return (int) obj;
        }
    }
}

