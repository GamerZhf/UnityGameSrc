namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct AiBehaviourTypeBoxAvoidanceComparer : IEqualityComparer<AiBehaviourType>
    {
        public bool Equals(AiBehaviourType x, AiBehaviourType y)
        {
            return (x == y);
        }

        public int GetHashCode(AiBehaviourType obj)
        {
            return (int) obj;
        }
    }
}

