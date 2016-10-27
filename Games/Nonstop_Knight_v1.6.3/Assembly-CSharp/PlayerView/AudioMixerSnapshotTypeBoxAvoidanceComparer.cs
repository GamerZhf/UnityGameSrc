namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct AudioMixerSnapshotTypeBoxAvoidanceComparer : IEqualityComparer<AudioMixerSnapshotType>
    {
        public bool Equals(AudioMixerSnapshotType x, AudioMixerSnapshotType y)
        {
            return (x == y);
        }

        public int GetHashCode(AudioMixerSnapshotType obj)
        {
            return (int) obj;
        }
    }
}

