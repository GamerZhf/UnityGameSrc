namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SkillTypeBoxAvoidanceComparer : IEqualityComparer<SkillType>
    {
        public bool Equals(SkillType x, SkillType y)
        {
            return (x == y);
        }

        public int GetHashCode(SkillType obj)
        {
            return (int) obj;
        }
    }
}

