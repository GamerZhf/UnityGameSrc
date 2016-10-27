namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct MissionTypeBoxAvoidanceComparer : IEqualityComparer<MissionType>
    {
        public bool Equals(MissionType x, MissionType y)
        {
            return (x == y);
        }

        public int GetHashCode(MissionType obj)
        {
            return (int) obj;
        }
    }
}

