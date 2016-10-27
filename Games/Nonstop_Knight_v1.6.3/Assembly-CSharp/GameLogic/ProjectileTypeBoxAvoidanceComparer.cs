namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ProjectileTypeBoxAvoidanceComparer : IEqualityComparer<ProjectileType>
    {
        public bool Equals(ProjectileType x, ProjectileType y)
        {
            return (x == y);
        }

        public int GetHashCode(ProjectileType obj)
        {
            return (int) obj;
        }
    }
}

