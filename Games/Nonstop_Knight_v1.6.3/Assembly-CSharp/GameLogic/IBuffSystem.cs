namespace GameLogic
{
    using System;
    using System.Runtime.InteropServices;

    public interface IBuffSystem
    {
        void endBuff(Buff buff);
        void endBuffsForCharacter(CharacterInstance c);
        void endBuffsFromCharacter(CharacterInstance c);
        float getBaseStatModifierSumFromActiveBuffs(CharacterInstance c, BaseStatProperty baseStat);
        Buff getBuffFromBoost(CharacterInstance c, BoostType boostType);
        Buff getBuffFromPerk(CharacterInstance c, PerkType perkType);
        Buff getBuffFromSource(CharacterInstance c, BuffSource source);
        int getNumberOfBuffsFromSource(CharacterInstance c, BuffSource source);
        int getNumberOfBuffsWithId(CharacterInstance c, string id);
        float getSkillCooldownModifierSumFromActiveBuffs(CharacterInstance c, SkillType skillType);
        float getTargetViewScaleForCharacter(CharacterInstance c);
        float getTotalBuffModifierFromSource(CharacterInstance c, BuffSource source);
        void grantSpurtBuff(CharacterInstance c);
        bool hasBuffFromBoost(CharacterInstance c, BoostType boostType);
        bool hasBuffFromPerk(CharacterInstance c, PerkType perkType);
        void refreshBuff(Buff buff, float duration);
        void refreshBuff(Buff buff, double modifier, float duration);
        void startBuff(CharacterInstance c, Buff buff);
        void startBuffFromPerk(CharacterInstance c, PerkType perkType, float durationSeconds, double modifier, BuffSource source, [Optional, DefaultParameterValue(null)] CharacterInstance sourceCharacter);
        void startOrRefreshBuff(Buff buff, float duration);
        void startOrRefreshBuffFromPerk(CharacterInstance c, PerkType perkType, float durationSeconds, double modifier, BuffSource source, [Optional, DefaultParameterValue(null)] CharacterInstance sourceCharacter);
    }
}

