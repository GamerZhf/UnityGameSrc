namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct RewardGalleryCellTypeBoxAvoidanceComparer : IEqualityComparer<RewardGalleryCellType>
    {
        public bool Equals(RewardGalleryCellType x, RewardGalleryCellType y)
        {
            return (x == y);
        }

        public int GetHashCode(RewardGalleryCellType obj)
        {
            return (int) obj;
        }
    }
}

