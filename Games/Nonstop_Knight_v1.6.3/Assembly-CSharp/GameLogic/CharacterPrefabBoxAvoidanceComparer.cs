namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct CharacterPrefabBoxAvoidanceComparer : IEqualityComparer<CharacterPrefab>
    {
        public bool Equals(CharacterPrefab x, CharacterPrefab y)
        {
            return (x == y);
        }

        public int GetHashCode(CharacterPrefab obj)
        {
            return (int) obj;
        }
    }
}

