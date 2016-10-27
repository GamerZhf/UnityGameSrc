namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DungeonBoostPrefabTypeBoxAvoidanceComparer : IEqualityComparer<DungeonBoostPrefabType>
    {
        public bool Equals(DungeonBoostPrefabType x, DungeonBoostPrefabType y)
        {
            return (x == y);
        }

        public int GetHashCode(DungeonBoostPrefabType obj)
        {
            return (int) obj;
        }
    }
}

