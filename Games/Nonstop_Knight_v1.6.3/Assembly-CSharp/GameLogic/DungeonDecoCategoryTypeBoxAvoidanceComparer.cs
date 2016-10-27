namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DungeonDecoCategoryTypeBoxAvoidanceComparer : IEqualityComparer<DungeonDecoCategoryType>
    {
        public bool Equals(DungeonDecoCategoryType x, DungeonDecoCategoryType y)
        {
            return (x == y);
        }

        public int GetHashCode(DungeonDecoCategoryType obj)
        {
            return (int) obj;
        }
    }
}

