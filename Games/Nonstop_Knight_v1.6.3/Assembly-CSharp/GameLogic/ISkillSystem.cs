namespace GameLogic
{
    using System;
    using System.Runtime.InteropServices;

    public interface ISkillSystem
    {
        void activateSkill(CharacterInstance c, SkillType skillType, [Optional, DefaultParameterValue(-1f)] float overrideBuildupTime, [Optional, DefaultParameterValue(null)] CharacterInstance overrideTargetCharacter);
        void endSkillCooldownTimers();
        int getNumberOfUsedCharges(SkillType skillType);
        float getSkillCooldownNormalizedProgress(SkillType skillType);
        float getSkillCooldownTimeRemaining(SkillType skillType);
    }
}

