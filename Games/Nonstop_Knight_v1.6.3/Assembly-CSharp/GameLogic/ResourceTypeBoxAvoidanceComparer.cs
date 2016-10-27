namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ResourceTypeBoxAvoidanceComparer : IEqualityComparer<ResourceType>
    {
        public bool Equals(ResourceType x, ResourceType y)
        {
            return (x == y);
        }

        public int GetHashCode(ResourceType obj)
        {
            return (int) obj;
        }
    }
}

