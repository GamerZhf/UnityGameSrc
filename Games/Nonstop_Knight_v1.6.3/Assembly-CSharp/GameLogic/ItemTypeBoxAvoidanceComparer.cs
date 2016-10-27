namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ItemTypeBoxAvoidanceComparer : IEqualityComparer<ItemType>
    {
        public bool Equals(ItemType x, ItemType y)
        {
            return (x == y);
        }

        public int GetHashCode(ItemType obj)
        {
            return (int) obj;
        }
    }
}

