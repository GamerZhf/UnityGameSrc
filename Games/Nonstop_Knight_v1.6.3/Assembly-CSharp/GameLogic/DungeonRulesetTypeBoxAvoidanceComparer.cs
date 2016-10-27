namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DungeonRulesetTypeBoxAvoidanceComparer : IEqualityComparer<DungeonRulesetType>
    {
        public bool Equals(DungeonRulesetType x, DungeonRulesetType y)
        {
            return (x == y);
        }

        public int GetHashCode(DungeonRulesetType obj)
        {
            return (int) obj;
        }
    }
}

