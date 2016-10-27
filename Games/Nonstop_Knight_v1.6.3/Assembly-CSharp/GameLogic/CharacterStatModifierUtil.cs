namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class CharacterStatModifierUtil
    {
        public static Dictionary<BaseStatProperty, bool> BASESTAT_MODIFIER_IS_ABSOLUTE;
        private static List<KeyValuePair<PerkInstance, BuffSource>> sm_perkInstances;

        static CharacterStatModifierUtil()
        {
            Dictionary<BaseStatProperty, bool> dictionary = new Dictionary<BaseStatProperty, bool>(new BaseStatPropertyBoxAvoidanceComparer());
            dictionary.Add(BaseStatProperty.UNSPECIFIED, false);
            dictionary.Add(BaseStatProperty.Life, false);
            dictionary.Add(BaseStatProperty.AttacksPerSecond, false);
            dictionary.Add(BaseStatProperty.AttackRange, false);
            dictionary.Add(BaseStatProperty.DamagePerHit, false);
            dictionary.Add(BaseStatProperty.CriticalHitChancePct, true);
            dictionary.Add(BaseStatProperty.CriticalHitMultiplier, false);
            dictionary.Add(BaseStatProperty.CleaveDamagePct, false);
            dictionary.Add(BaseStatProperty.CleaveRange, false);
            dictionary.Add(BaseStatProperty.MovementSpeed, false);
            dictionary.Add(BaseStatProperty.Threat, false);
            dictionary.Add(BaseStatProperty.UniversalArmorBonus, false);
            dictionary.Add(BaseStatProperty.SkillDamage, false);
            dictionary.Add(BaseStatProperty.UniversalXpBonus, false);
            BASESTAT_MODIFIER_IS_ABSOLUTE = dictionary;
            sm_perkInstances = new List<KeyValuePair<PerkInstance, BuffSource>>(8);
        }

        public static double ApplyBaseStatBonuses_Double(CharacterInstance source, BaseStatProperty prop, double baseAmount, [Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            if (prop == BaseStatProperty.DamagePerHit)
            {
                baseAmount += baseAmount * source.getGenericModifierForPerkType(PerkType.CoreBonusDamagePerHit);
            }
            else if (prop == BaseStatProperty.Life)
            {
                baseAmount += baseAmount * source.getGenericModifierForPerkType(PerkType.CoreBonusLife);
            }
            else if (prop == BaseStatProperty.SkillDamage)
            {
                baseAmount += baseAmount * source.getGenericModifierForPerkType(PerkType.CoreBonusSkillDamage);
            }
            float num = source.getBaseStatModifier(prop);
            if (includeBuffs)
            {
                num += GameLogic.Binder.BuffSystem.getBaseStatModifierSumFromActiveBuffs(source, prop);
            }
            BaseStatProperty property = prop;
            switch (property)
            {
                case BaseStatProperty.Life:
                    if (source.IsBoss)
                    {
                        num += source.getGenericModifierForPerkType(PerkType.BossBonusLife);
                    }
                    break;

                case BaseStatProperty.DamagePerHit:
                    if (source.IsBoss)
                    {
                        num += source.getGenericModifierForPerkType(PerkType.BossBonusDamage);
                        num += source.getGenericModifierForPerkType(PerkType.BossBonusWeaponDamage);
                    }
                    break;

                default:
                    if (property == BaseStatProperty.SkillDamage)
                    {
                        num += source.getGenericModifierForPerkType(PerkType.DamageBonusAll);
                        if (source.IsBoss)
                        {
                            num += source.getGenericModifierForPerkType(PerkType.BossBonusDamage);
                            num += source.getGenericModifierForPerkType(PerkType.BossBonusSkillDamage);
                        }
                    }
                    break;
            }
            if (source.getPerkInstanceCount(PerkType.Berserk) > 0)
            {
                if (prop == BaseStatProperty.Life)
                {
                    num += ConfigPerks.Berserk.ArmorModifier;
                }
                else if (prop == BaseStatProperty.DamagePerHit)
                {
                    num += ConfigPerks.Berserk.DamagePerHitModifier;
                }
            }
            if (((includeBuffs && GameLogic.Binder.BuffSystem.hasBuffFromPerk(source, PerkType.DungeonBoostEnrageEnemies)) || (source.getPerkInstanceCount(PerkType.DungeonBoostEnrageEnemies) > 0)) && (prop == BaseStatProperty.Life))
            {
                num += ConfigPerks.EnrageEnemies.LifeModifier;
            }
            return (!BASESTAT_MODIFIER_IS_ABSOLUTE[prop] ? ((baseAmount != 0.0) ? (baseAmount + (baseAmount * num)) : ((double) num)) : (baseAmount + num));
        }

        public static float ApplyBaseStatBonuses_Float(CharacterInstance source, BaseStatProperty prop, float baseAmount, [Optional, DefaultParameterValue(true)] bool includeBuffs)
        {
            float num = source.getBaseStatModifier(prop);
            if (includeBuffs)
            {
                num += GameLogic.Binder.BuffSystem.getBaseStatModifierSumFromActiveBuffs(source, prop);
            }
            switch (prop)
            {
                case BaseStatProperty.AttacksPerSecond:
                    if (source.getPerkInstanceCount(PerkType.BerserkersBlood) > 0)
                    {
                        num += (1f - source.CurrentHpNormalized) * source.getGenericModifierForPerkType(PerkType.BerserkersBlood);
                    }
                    if (source.IsBoss)
                    {
                        num += source.getGenericModifierForPerkType(PerkType.BossBonusSpeed);
                    }
                    break;

                case BaseStatProperty.MovementSpeed:
                    if (source.IsBoss)
                    {
                        num += source.getGenericModifierForPerkType(PerkType.BossBonusSpeed);
                    }
                    break;
            }
            if (((includeBuffs && GameLogic.Binder.BuffSystem.hasBuffFromPerk(source, PerkType.DungeonBoostEnrageEnemies)) || (source.getPerkInstanceCount(PerkType.DungeonBoostEnrageEnemies) > 0)) && (prop == BaseStatProperty.AttacksPerSecond))
            {
                num += ConfigPerks.EnrageEnemies.AttacksPerSecondModifier;
            }
            return (!BASESTAT_MODIFIER_IS_ABSOLUTE[prop] ? ((baseAmount != 0f) ? (baseAmount + (baseAmount * num)) : num) : (baseAmount + num));
        }

        public static double ApplyCoinBonuses(ICharacterStatModifier source, CharacterType characterType, double baseAmount, bool includePassiveBonuses)
        {
            baseAmount += baseAmount * source.getGenericModifierForPerkType(PerkType.CoreBonusCoins);
            float num = source.getCharacterTypeCoinModifier(characterType) + source.getGenericModifierForPerkType(PerkType.CoinBonusActive);
            if (includePassiveBonuses)
            {
                num += source.getGenericModifierForPerkType(PerkType.CoinBonusPassive);
            }
            return ((baseAmount + (baseAmount * num)) * App.Binder.ConfigMeta.COIN_GAIN_CONTROLLER);
        }

        public static double ApplyDustBonuses(ICharacterStatModifier source, double baseAmount)
        {
            float num = source.getGenericModifierForPerkType(PerkType.DustBonusUniversal);
            return (baseAmount + (baseAmount * num));
        }

        public static float ApplyFrenzyDurationBonuses(ICharacterStatModifier source, float baseDuration)
        {
            baseDuration += baseDuration * source.getGenericModifierForPerkType(PerkType.CoreBonusFrenzyDuration);
            return (baseDuration + (baseDuration * 0f));
        }

        public static double ApplySkillTypeDamageBonuses(ICharacterStatModifier source, SkillType skillType, double baseAmount)
        {
            float num = source.getSkillDamageModifier(skillType);
            return (baseAmount + (baseAmount * num));
        }

        public static double ApplyTokenBonuses(ICharacterStatModifier source, double baseAmount)
        {
            baseAmount += baseAmount * source.getGenericModifierForPerkType(PerkType.CoreBonusTokens);
            return ((baseAmount + (baseAmount * 0.0)) * App.Binder.ConfigMeta.TOKEN_REWARD_CONTROLLER);
        }

        public static List<KeyValuePair<PerkInstance, BuffSource>> GetPerkInstancesOfType(ICharacterStatModifier source, PerkType perkType)
        {
            sm_perkInstances.Clear();
            source.getPerkInstancesOfType(perkType, null, ref sm_perkInstances);
            return sm_perkInstances;
        }
    }
}

