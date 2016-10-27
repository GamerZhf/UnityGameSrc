namespace GameLogic
{
    using System;
    using System.Collections.Generic;

    public class PerkContainer : ICharacterStatModifier
    {
        public List<PerkInstance> PerkInstances;

        public PerkContainer()
        {
            this.PerkInstances = new List<PerkInstance>();
        }

        public PerkContainer(PerkContainer another)
        {
            this.PerkInstances = new List<PerkInstance>();
            this.PerkInstances.Clear();
            for (int i = 0; i < another.PerkInstances.Count; i++)
            {
                this.PerkInstances.Add(new PerkInstance(another.PerkInstances[i]));
            }
        }

        public bool containsPerkOfType(PerkType perkType)
        {
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                if (this.PerkInstances[i].Type == perkType)
                {
                    return true;
                }
            }
            return false;
        }

        public int count()
        {
            return this.PerkInstances.Count;
        }

        public float getBaseStatModifier(BaseStatProperty prop)
        {
            float num = 0f;
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                num += this.PerkInstances[i].getBaseStatModifier(prop);
            }
            return num;
        }

        public float getCharacterTypeArmorModifier(CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                num += this.PerkInstances[i].getCharacterTypeArmorModifier(characterType);
            }
            return num;
        }

        public float getCharacterTypeCoinModifier(CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                num += this.PerkInstances[i].getCharacterTypeCoinModifier(characterType);
            }
            return num;
        }

        public float getCharacterTypeDamageModifier(CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                num += this.PerkInstances[i].getCharacterTypeDamageModifier(characterType);
            }
            return num;
        }

        public float getCharacterTypeXpModifier(CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                num += this.PerkInstances[i].getCharacterTypeXpModifier(characterType);
            }
            return num;
        }

        public float getGenericModifierForPerkType(PerkType perkType)
        {
            float num = 0f;
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                num += this.PerkInstances[i].getGenericModifierForPerkType(perkType);
            }
            return num;
        }

        public int getPerkInstanceCount(PerkType perkType)
        {
            int num = 0;
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                num += this.PerkInstances[i].getPerkInstanceCount(perkType);
            }
            return num;
        }

        public void getPerkInstancesOfType(PerkType perkType, IBuffIconProvider iconProvider, ref List<KeyValuePair<PerkInstance, BuffSource>> outPerkInstances)
        {
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                this.PerkInstances[i].getPerkInstancesOfType(perkType, iconProvider, ref outPerkInstances);
            }
        }

        public float getSkillCooldownModifier(SkillType skillType)
        {
            float num = 0f;
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                num += this.PerkInstances[i].getSkillCooldownModifier(skillType);
            }
            return num;
        }

        public float getSkillDamageModifier(SkillType skillType)
        {
            float num = 0f;
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                num += this.PerkInstances[i].getSkillDamageModifier(skillType);
            }
            return num;
        }

        public int getSkillExtraCharges(SkillType skillType)
        {
            int num = 0;
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                num += this.PerkInstances[i].getSkillExtraCharges(skillType);
            }
            return num;
        }

        public bool hasSkillInvulnerability(SkillType skillType)
        {
            for (int i = 0; i < this.PerkInstances.Count; i++)
            {
                if (this.PerkInstances[i].hasSkillInvulnerability(skillType))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

