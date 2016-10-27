namespace GameLogic
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public interface IBlinkSystem
    {
        void blinkCharacter(CharacterInstance c, Vector3 targetWorldPt, [Optional, DefaultParameterValue(0f)] float waitBefore);
        void blinkCharacterQueued(CharacterInstance c, Vector3 targetWorldPt);
    }
}

