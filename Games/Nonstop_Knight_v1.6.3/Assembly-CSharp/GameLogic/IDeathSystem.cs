namespace GameLogic
{
    using System;
    using System.Runtime.InteropServices;

    public interface IDeathSystem
    {
        bool allQueuedCharactersDestroyed();
        void killAllNonPersistentCharacters(bool includeSupportCharacters, bool instantDestruction);
        void killCharacter(CharacterInstance target, CharacterInstance killer, bool critted, bool instantDestruction, [Optional, DefaultParameterValue(0)] SkillType fromSkill);
    }
}

