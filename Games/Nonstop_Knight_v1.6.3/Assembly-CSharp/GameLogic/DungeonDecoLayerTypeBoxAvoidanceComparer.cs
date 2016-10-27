namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DungeonDecoLayerTypeBoxAvoidanceComparer : IEqualityComparer<DungeonDecoLayerType>
    {
        public bool Equals(DungeonDecoLayerType x, DungeonDecoLayerType y)
        {
            return (x == y);
        }

        public int GetHashCode(DungeonDecoLayerType obj)
        {
            return (int) obj;
        }
    }
}

