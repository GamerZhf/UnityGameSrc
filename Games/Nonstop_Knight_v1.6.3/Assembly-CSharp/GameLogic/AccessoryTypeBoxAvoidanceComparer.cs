namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct AccessoryTypeBoxAvoidanceComparer : IEqualityComparer<AccessoryType>
    {
        public bool Equals(AccessoryType x, AccessoryType y)
        {
            return (x == y);
        }

        public int GetHashCode(AccessoryType obj)
        {
            return (int) obj;
        }
    }
}

