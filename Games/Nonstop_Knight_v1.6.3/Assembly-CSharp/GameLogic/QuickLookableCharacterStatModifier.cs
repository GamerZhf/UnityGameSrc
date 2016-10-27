namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public abstract class QuickLookableCharacterStatModifier : ICharacterStatModifier
    {
        private Dictionary<BaseStatProperty, float> m_quickLookup_baseStatModifier = new Dictionary<BaseStatProperty, float>(new BaseStatPropertyBoxAvoidanceComparer());
        private Dictionary<CharacterType, CharacterTypeQuickLookup> m_quickLookup_characterType = new Dictionary<CharacterType, CharacterTypeQuickLookup>(new CharacterTypeBoxAvoidanceComparer());
        private Dictionary<PerkType, PerkTypeQuickLookup> m_quickLookup_perkType = new Dictionary<PerkType, PerkTypeQuickLookup>(new PerkTypeBoxAvoidanceComparer());
        private Dictionary<SkillType, SkillTypeQuickLookup> m_quickLookup_skillType = new Dictionary<SkillType, SkillTypeQuickLookup>(new SkillTypeBoxAvoidanceComparer());
        [JsonIgnore]
        protected List<PerkInstance> QuickLookupPerkInstances = new List<PerkInstance>();

        protected QuickLookableCharacterStatModifier()
        {
        }

        protected void addQuickLookupPerkInstance(PerkInstance pi, [Optional, DefaultParameterValue(true)] bool triggerRefresh)
        {
            this.QuickLookupPerkInstances.Add(pi);
            if (triggerRefresh)
            {
                this.refreshQuickLookup();
            }
        }

        protected void clearQuickLookup()
        {
            this.QuickLookupPerkInstances.Clear();
            this.m_quickLookup_perkType.Clear();
            this.m_quickLookup_baseStatModifier.Clear();
            this.m_quickLookup_skillType.Clear();
            this.m_quickLookup_characterType.Clear();
        }

        public float getBaseStatModifier(BaseStatProperty baseStat)
        {
            return this.m_quickLookup_baseStatModifier[baseStat];
        }

        protected abstract IBuffIconProvider getBuffIconProvideForPerkInstance(PerkInstance perkInstance);
        public float getCharacterTypeArmorModifier(CharacterType characterType)
        {
            return this.m_quickLookup_characterType[characterType].CharacterTypeArmorModifier;
        }

        public float getCharacterTypeCoinModifier([Optional, DefaultParameterValue(0)] CharacterType characterType)
        {
            return this.m_quickLookup_characterType[characterType].CoinModifier;
        }

        public float getCharacterTypeDamageModifier(CharacterType characterType)
        {
            return this.m_quickLookup_characterType[characterType].CharacterTypeDamageModifier;
        }

        public float getCharacterTypeXpModifier(CharacterType characterType)
        {
            return this.m_quickLookup_characterType[characterType].CharacterTypeXpModifier;
        }

        public float getGenericModifierForPerkType(PerkType perkType)
        {
            return this.m_quickLookup_perkType[perkType].GenericModifierForPerkType;
        }

        public int getPerkInstanceCount(PerkType perkType)
        {
            return this.m_quickLookup_perkType[perkType].PerkInstanceCount;
        }

        public void getPerkInstancesOfType(PerkType perkType, IBuffIconProvider iconProvider, ref List<KeyValuePair<PerkInstance, BuffSource>> outPerkInstances)
        {
            for (int i = 0; i < this.QuickLookupPerkInstances.Count; i++)
            {
                PerkInstance perkInstance = this.QuickLookupPerkInstances[i];
                perkInstance.getPerkInstancesOfType(perkType, this.getBuffIconProvideForPerkInstance(perkInstance), ref outPerkInstances);
            }
        }

        public float getSkillCooldownModifier(SkillType skillType)
        {
            return this.m_quickLookup_skillType[skillType].SkillCooldownModifier;
        }

        public float getSkillDamageModifier(SkillType skillType)
        {
            return this.m_quickLookup_skillType[skillType].SkillDamageModifier;
        }

        public int getSkillExtraCharges(SkillType skillType)
        {
            return this.m_quickLookup_skillType[skillType].SkillExtraCharges;
        }

        public bool hasSkillInvulnerability(SkillType skillType)
        {
            return this.m_quickLookup_skillType[skillType].SkillInvulnerability;
        }

        protected void refreshQuickLookup()
        {
            this.m_quickLookup_perkType.Clear();
            for (int i = 0; i < ConfigPerks.ALL_PERKS.Count; i++)
            {
                PerkType perkType = ConfigPerks.ALL_PERKS[i];
                PerkTypeQuickLookup lookup = new PerkTypeQuickLookup();
                int num2 = 0;
                for (int n = 0; n < this.QuickLookupPerkInstances.Count; n++)
                {
                    PerkInstance instance = this.QuickLookupPerkInstances[n];
                    num2 += instance.getPerkInstanceCount(perkType);
                }
                lookup.PerkInstanceCount = num2;
                float num4 = 0f;
                for (int num5 = 0; num5 < this.QuickLookupPerkInstances.Count; num5++)
                {
                    float num6 = this.QuickLookupPerkInstances[num5].getGenericModifierForPerkType(perkType);
                    num4 += num6;
                }
                lookup.GenericModifierForPerkType = num4;
                this.m_quickLookup_perkType.Add(perkType, lookup);
            }
            this.m_quickLookup_baseStatModifier.Clear();
            for (int j = 0; j < ConfigGameplay.ALL_BASE_STAT_PROPERTY_TYPES.Count; j++)
            {
                BaseStatProperty prop = ConfigGameplay.ALL_BASE_STAT_PROPERTY_TYPES[j];
                float num8 = 0f;
                for (int num9 = 0; num9 < this.QuickLookupPerkInstances.Count; num9++)
                {
                    float num10 = this.QuickLookupPerkInstances[num9].getBaseStatModifier(prop);
                    num8 += num10;
                }
                this.m_quickLookup_baseStatModifier.Add(prop, num8);
            }
            this.m_quickLookup_skillType.Clear();
            for (int k = 0; k < ConfigSkills.ALL_HERO_SKILLS.Count; k++)
            {
                SkillType skillType = ConfigSkills.ALL_HERO_SKILLS[k];
                SkillTypeQuickLookup lookup2 = new SkillTypeQuickLookup();
                float num12 = 0f;
                for (int num13 = 0; num13 < this.QuickLookupPerkInstances.Count; num13++)
                {
                    float num14 = this.QuickLookupPerkInstances[num13].getSkillDamageModifier(skillType);
                    num12 += num14;
                }
                lookup2.SkillDamageModifier = num12;
                int num15 = 0;
                for (int num16 = 0; num16 < this.QuickLookupPerkInstances.Count; num16++)
                {
                    PerkInstance instance5 = this.QuickLookupPerkInstances[num16];
                    num15 += instance5.getSkillExtraCharges(skillType);
                }
                lookup2.SkillExtraCharges = num15;
                bool flag = false;
                for (int num17 = 0; num17 < this.QuickLookupPerkInstances.Count; num17++)
                {
                    PerkInstance instance6 = this.QuickLookupPerkInstances[num17];
                    if (instance6.hasSkillInvulnerability(skillType))
                    {
                        flag = true;
                        break;
                    }
                }
                lookup2.SkillInvulnerability = flag;
                float num18 = 0f;
                for (int num19 = 0; num19 < this.QuickLookupPerkInstances.Count; num19++)
                {
                    float num20 = this.QuickLookupPerkInstances[num19].getSkillCooldownModifier(skillType);
                    num18 += num20;
                }
                lookup2.SkillCooldownModifier = num18;
                this.m_quickLookup_skillType.Add(skillType, lookup2);
            }
            this.m_quickLookup_characterType.Clear();
            for (int m = 0; m < ConfigGameplay.ALL_CHARACTER_TYPES_INCLUDING_UNSPECIFIED.Count; m++)
            {
                CharacterType characterType = ConfigGameplay.ALL_CHARACTER_TYPES_INCLUDING_UNSPECIFIED[m];
                CharacterTypeQuickLookup lookup3 = new CharacterTypeQuickLookup();
                float num22 = 0f;
                for (int num23 = 0; num23 < this.QuickLookupPerkInstances.Count; num23++)
                {
                    float num24 = this.QuickLookupPerkInstances[num23].getCharacterTypeDamageModifier(characterType);
                    num22 += num24;
                }
                lookup3.CharacterTypeDamageModifier = num22;
                float num25 = 0f;
                for (int num26 = 0; num26 < this.QuickLookupPerkInstances.Count; num26++)
                {
                    float num27 = this.QuickLookupPerkInstances[num26].getCharacterTypeArmorModifier(characterType);
                    num25 += num27;
                }
                lookup3.CharacterTypeArmorModifier = num25;
                float num28 = 0f;
                for (int num29 = 0; num29 < this.QuickLookupPerkInstances.Count; num29++)
                {
                    float num30 = this.QuickLookupPerkInstances[num29].getCharacterTypeXpModifier(characterType);
                    num28 += num30;
                }
                lookup3.CharacterTypeXpModifier = num28;
                float num31 = 0f;
                for (int num32 = 0; num32 < this.QuickLookupPerkInstances.Count; num32++)
                {
                    float num33 = this.QuickLookupPerkInstances[num32].getCharacterTypeCoinModifier(characterType);
                    num31 += num33;
                }
                lookup3.CoinModifier = num31;
                this.m_quickLookup_characterType.Add(characterType, lookup3);
            }
        }

        private class CharacterTypeQuickLookup
        {
            public float CharacterTypeArmorModifier;
            public float CharacterTypeDamageModifier;
            public float CharacterTypeXpModifier;
            public float CoinModifier;
        }

        private class PerkTypeQuickLookup
        {
            public float GenericModifierForPerkType;
            public int PerkInstanceCount;
        }

        private class SkillTypeQuickLookup
        {
            public float SkillCooldownModifier;
            public float SkillDamageModifier;
            public int SkillExtraCharges;
            public bool SkillInvulnerability;
        }
    }
}

