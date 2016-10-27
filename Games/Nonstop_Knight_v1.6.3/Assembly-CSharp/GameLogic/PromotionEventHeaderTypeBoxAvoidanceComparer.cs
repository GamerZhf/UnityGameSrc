namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PromotionEventHeaderTypeBoxAvoidanceComparer : IEqualityComparer<PromotionEventHeaderType>
    {
        public bool Equals(PromotionEventHeaderType x, PromotionEventHeaderType y)
        {
            return (x == y);
        }

        public int GetHashCode(PromotionEventHeaderType obj)
        {
            return (int) obj;
        }
    }
}

