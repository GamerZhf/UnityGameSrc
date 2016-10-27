namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DungeonModifierTypeBoxAvoidanceComparer : IEqualityComparer<DungeonModifierType>
    {
        public bool Equals(DungeonModifierType x, DungeonModifierType y)
        {
            return (x == y);
        }

        public int GetHashCode(DungeonModifierType obj)
        {
            return (int) obj;
        }
    }
}

