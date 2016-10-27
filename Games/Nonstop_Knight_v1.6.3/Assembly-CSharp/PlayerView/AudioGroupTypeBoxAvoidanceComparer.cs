namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct AudioGroupTypeBoxAvoidanceComparer : IEqualityComparer<AudioGroupType>
    {
        public bool Equals(AudioGroupType x, AudioGroupType y)
        {
            return (x == y);
        }

        public int GetHashCode(AudioGroupType obj)
        {
            return (int) obj;
        }
    }
}

