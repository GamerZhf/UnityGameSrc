namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct AudioSourceTypeBoxAvoidanceComparer : IEqualityComparer<AudioSourceType>
    {
        public bool Equals(AudioSourceType x, AudioSourceType y)
        {
            return (x == y);
        }

        public int GetHashCode(AudioSourceType obj)
        {
            return (int) obj;
        }
    }
}

