namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct BaseStatPropertyBoxAvoidanceComparer : IEqualityComparer<BaseStatProperty>
    {
        public bool Equals(BaseStatProperty x, BaseStatProperty y)
        {
            return (x == y);
        }

        public int GetHashCode(BaseStatProperty obj)
        {
            return (int) obj;
        }
    }
}

