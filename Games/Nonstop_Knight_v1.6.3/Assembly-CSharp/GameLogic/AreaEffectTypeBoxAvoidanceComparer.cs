namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct AreaEffectTypeBoxAvoidanceComparer : IEqualityComparer<AreaEffectType>
    {
        public bool Equals(AreaEffectType x, AreaEffectType y)
        {
            return (x == y);
        }

        public int GetHashCode(AreaEffectType obj)
        {
            return (int) obj;
        }
    }
}

