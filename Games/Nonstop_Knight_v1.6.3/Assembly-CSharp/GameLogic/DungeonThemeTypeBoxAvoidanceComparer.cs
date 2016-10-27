namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DungeonThemeTypeBoxAvoidanceComparer : IEqualityComparer<DungeonThemeType>
    {
        public bool Equals(DungeonThemeType x, DungeonThemeType y)
        {
            return (x == y);
        }

        public int GetHashCode(DungeonThemeType obj)
        {
            return (int) obj;
        }
    }
}

