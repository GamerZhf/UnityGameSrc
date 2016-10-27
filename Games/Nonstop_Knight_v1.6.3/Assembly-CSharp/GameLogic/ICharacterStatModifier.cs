namespace GameLogic
{
    using System;
    using System.Collections.Generic;

    public interface ICharacterStatModifier
    {
        float getBaseStatModifier(BaseStatProperty prop);
        float getCharacterTypeArmorModifier(CharacterType characterType);
        float getCharacterTypeCoinModifier(CharacterType characterType);
        float getCharacterTypeDamageModifier(CharacterType characterType);
        float getCharacterTypeXpModifier(CharacterType characterType);
        float getGenericModifierForPerkType(PerkType perkType);
        int getPerkInstanceCount(PerkType perkType);
        void getPerkInstancesOfType(PerkType perkType, IBuffIconProvider iconProvider, ref List<KeyValuePair<PerkInstance, BuffSource>> outPerkInstances);
        float getSkillCooldownModifier(SkillType skillType);
        float getSkillDamageModifier(SkillType skillType);
        int getSkillExtraCharges(SkillType skillType);
        bool hasSkillInvulnerability(SkillType skillType);
    }
}

