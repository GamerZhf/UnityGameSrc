namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class PerkInstance : ICharacterStatModifier
    {
        public float Modifier;
        [JsonIgnore]
        public float Timer;
        public PerkType Type;

        public PerkInstance()
        {
        }

        public PerkInstance(PerkInstance another)
        {
            this.Type = another.Type;
            this.Modifier = another.Modifier;
        }

        public PerkInstance(PerkType perkType, float modifier)
        {
            this.Type = perkType;
            this.Modifier = modifier;
        }

        public float getBaseStatModifier(BaseStatProperty prop)
        {
            if (ConfigPerks.SHARED_DATA[this.Type].BaseStat == prop)
            {
                return this.Modifier;
            }
            return 0f;
        }

        public float getCharacterTypeArmorModifier(CharacterType characterType)
        {
            if ((characterType != CharacterType.UNSPECIFIED) && (ConfigPerks.SHARED_DATA[this.Type].CharacterTypeArmorBonus == characterType))
            {
                return this.Modifier;
            }
            return 0f;
        }

        public float getCharacterTypeCoinModifier([Optional, DefaultParameterValue(0)] CharacterType characterType)
        {
            if ((characterType != CharacterType.UNSPECIFIED) && (ConfigPerks.SHARED_DATA[this.Type].CharacterTypeCoinBonus == characterType))
            {
                return this.Modifier;
            }
            return 0f;
        }

        public float getCharacterTypeDamageModifier(CharacterType characterType)
        {
            if ((characterType != CharacterType.UNSPECIFIED) && (ConfigPerks.SHARED_DATA[this.Type].CharacterTypeDmgBonus == characterType))
            {
                return this.Modifier;
            }
            return 0f;
        }

        public float getCharacterTypeXpModifier(CharacterType characterType)
        {
            if ((characterType != CharacterType.UNSPECIFIED) && (ConfigPerks.SHARED_DATA[this.Type].CharacterTypeXpBonus == characterType))
            {
                return this.Modifier;
            }
            return 0f;
        }

        public float getGenericModifierForPerkType(PerkType perkType)
        {
            if (this.Type == perkType)
            {
                return this.Modifier;
            }
            return 0f;
        }

        public float getHighestThresholdForPerkType(PerkType perkType)
        {
            if (this.Type == perkType)
            {
                return ConfigPerks.SHARED_DATA[perkType].Threshold;
            }
            return 0f;
        }

        public float getLongestDurationForPerkType(PerkType perkType)
        {
            if (this.Type == perkType)
            {
                return ConfigPerks.SHARED_DATA[perkType].DurationSeconds;
            }
            return 0f;
        }

        public int getPerkInstanceCount(PerkType perkType)
        {
            if (this.Type == perkType)
            {
                return 1;
            }
            return 0;
        }

        public void getPerkInstancesOfType(PerkType perkType, IBuffIconProvider iconProvider, ref List<KeyValuePair<PerkInstance, BuffSource>> outPerkInstances)
        {
            if (this.Type == perkType)
            {
                BuffSource source2 = new BuffSource();
                source2.Object = this;
                source2.IconProvider = iconProvider;
                BuffSource source = source2;
                outPerkInstances.Add(new KeyValuePair<PerkInstance, BuffSource>(this, source));
            }
        }

        public float getSkillCooldownModifier(SkillType skillType)
        {
            if (skillType != SkillType.NONE)
            {
                ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[this.Type];
                if (data.AllSkillsCooldownBonus)
                {
                    return this.Modifier;
                }
                if (data.SkillCooldownBonus == skillType)
                {
                    return this.Modifier;
                }
            }
            return 0f;
        }

        public float getSkillDamageModifier(SkillType skillType)
        {
            if ((skillType != SkillType.NONE) && (ConfigPerks.SHARED_DATA[this.Type].SkillDamageBonus == skillType))
            {
                return this.Modifier;
            }
            return 0f;
        }

        public int getSkillExtraCharges(SkillType skillType)
        {
            if (ConfigPerks.SHARED_DATA[this.Type].ExtraSkillCharge == skillType)
            {
                return 1;
            }
            return 0;
        }

        public bool hasSkillInvulnerability(SkillType skillType)
        {
            return (ConfigPerks.SHARED_DATA[this.Type].SkillInvulnerability == skillType);
        }
    }
}

