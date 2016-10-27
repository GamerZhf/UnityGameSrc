namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DungeonEventTypeBoxAvoidanceComparer : IEqualityComparer<DungeonEventType>
    {
        public bool Equals(DungeonEventType x, DungeonEventType y)
        {
            return (x == y);
        }

        public int GetHashCode(DungeonEventType obj)
        {
            return (int) obj;
        }
    }
}

