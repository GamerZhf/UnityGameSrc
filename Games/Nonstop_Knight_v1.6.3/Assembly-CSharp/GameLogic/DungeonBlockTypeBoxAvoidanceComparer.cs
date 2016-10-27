namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DungeonBlockTypeBoxAvoidanceComparer : IEqualityComparer<DungeonBlockType>
    {
        public bool Equals(DungeonBlockType x, DungeonBlockType y)
        {
            return (x == y);
        }

        public int GetHashCode(DungeonBlockType obj)
        {
            return (int) obj;
        }
    }
}

