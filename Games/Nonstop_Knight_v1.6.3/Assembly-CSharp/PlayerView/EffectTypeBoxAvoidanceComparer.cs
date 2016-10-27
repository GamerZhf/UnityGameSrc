namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct EffectTypeBoxAvoidanceComparer : IEqualityComparer<EffectType>
    {
        public bool Equals(EffectType x, EffectType y)
        {
            return (x == y);
        }

        public int GetHashCode(EffectType obj)
        {
            return (int) obj;
        }
    }
}

