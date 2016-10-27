namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct MissionGroupBoxAvoidanceComparer : IEqualityComparer<MissionGroup>
    {
        public bool Equals(MissionGroup x, MissionGroup y)
        {
            return (x == y);
        }

        public int GetHashCode(MissionGroup obj)
        {
            return (int) obj;
        }
    }
}

