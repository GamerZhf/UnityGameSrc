namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterTypeBoxAvoidanceComparer : IEqualityComparer<CharacterType>
    {
        public bool Equals(CharacterType x, CharacterType y)
        {
            return (x == y);
        }

        public int GetHashCode(CharacterType obj)
        {
            return (int) obj;
        }
    }
}

