namespace GameLogic
{
    using System;
    using System.Collections.Generic;

    public class GatedPerkContainer
    {
        public List<Entry> Entries = new List<Entry>();

        public bool containsPerkOfType(PerkType perkType)
        {
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (this.Entries[i].PerkInstance.Type == perkType)
                {
                    return true;
                }
            }
            return false;
        }

        public int count()
        {
            return this.Entries.Count;
        }

        public float getBaseStatModifier(int rank, BaseStatProperty prop)
        {
            float num = 0f;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    num += this.Entries[i].PerkInstance.getBaseStatModifier(prop);
                }
            }
            return num;
        }

        public float getCharacterTypeArmorModifier(int rank, CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    num += this.Entries[i].PerkInstance.getCharacterTypeArmorModifier(characterType);
                }
            }
            return num;
        }

        public float getCharacterTypeCoinModifier(int rank, CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    num += this.Entries[i].PerkInstance.getCharacterTypeCoinModifier(characterType);
                }
            }
            return num;
        }

        public float getCharacterTypeDamageModifier(int rank, CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    num += this.Entries[i].PerkInstance.getCharacterTypeDamageModifier(characterType);
                }
            }
            return num;
        }

        public float getCharacterTypeXpModifier(int rank, CharacterType characterType)
        {
            float num = 0f;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    num += this.Entries[i].PerkInstance.getCharacterTypeXpModifier(characterType);
                }
            }
            return num;
        }

        public float getGenericModifierForPerkType(int rank, PerkType perkType)
        {
            float num = 0f;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    num += this.Entries[i].PerkInstance.getGenericModifierForPerkType(perkType);
                }
            }
            return num;
        }

        public PerkInstance getPerkInstanceAtIndex(int idx)
        {
            return this.Entries[idx].PerkInstance;
        }

        public int getPerkInstanceCount(int rank, PerkType perkType)
        {
            int num = 0;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    num += this.Entries[i].PerkInstance.getPerkInstanceCount(perkType);
                }
            }
            return num;
        }

        public void getPerkInstancesOfType(int rank, PerkType perkType, IBuffIconProvider iconProvider, ref List<KeyValuePair<PerkInstance, BuffSource>> outPerkInstances)
        {
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    this.Entries[i].PerkInstance.getPerkInstancesOfType(perkType, iconProvider, ref outPerkInstances);
                }
            }
        }

        public float getSkillCooldownModifier(int rank, SkillType skillType)
        {
            float num = 0f;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    num += this.Entries[i].PerkInstance.getSkillCooldownModifier(skillType);
                }
            }
            return num;
        }

        public float getSkillDamageModifier(int rank, SkillType skillType)
        {
            float num = 0f;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    num += this.Entries[i].PerkInstance.getSkillDamageModifier(skillType);
                }
            }
            return num;
        }

        public int getSkillExtraCharges(int rank, SkillType skillType)
        {
            int num = 0;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (rank >= this.Entries[i].RankReq)
                {
                    num += this.Entries[i].PerkInstance.getSkillExtraCharges(skillType);
                }
            }
            return num;
        }

        public bool hasSkillInvulnerability(int rank, SkillType skillType)
        {
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if ((rank >= this.Entries[i].RankReq) && this.Entries[i].PerkInstance.hasSkillInvulnerability(skillType))
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasUnlockedPerkOfType(int rank, PerkType perkType)
        {
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if ((this.Entries[i].PerkInstance.Type == perkType) && (rank >= this.Entries[i].RankReq))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isPerkUnlocked(int rank, int index)
        {
            return (rank >= this.Entries[index].RankReq);
        }

        public class Entry
        {
            public GameLogic.PerkInstance PerkInstance;
            public int RankReq;
        }
    }
}

