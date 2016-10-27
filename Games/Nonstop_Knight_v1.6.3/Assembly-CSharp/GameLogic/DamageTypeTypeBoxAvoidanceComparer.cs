namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DamageTypeTypeBoxAvoidanceComparer : IEqualityComparer<DamageType>
    {
        public bool Equals(DamageType x, DamageType y)
        {
            return (x == y);
        }

        public int GetHashCode(DamageType obj)
        {
            return (int) obj;
        }
    }
}

