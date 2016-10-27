namespace App
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ConfigDungeonModifiers
    {
        public static List<DungeonModifierType> ALL_MODIFIERS;
        public static Dictionary<DungeonModifierType, DungeonModifier> MODIFIERS;
        public static Dictionary<DungeonRulesetType, DungeonRuleset> RULESETS;

        static ConfigDungeonModifiers()
        {
            Dictionary<DungeonRulesetType, DungeonRuleset> dictionary = new Dictionary<DungeonRulesetType, DungeonRuleset>(new DungeonRulesetTypeBoxAvoidanceComparer());
            DungeonRuleset ruleset = new DungeonRuleset();
            List<DungeonModifierType> list = new List<DungeonModifierType>();
            list.Add(DungeonModifierType.StartingItemLegionnaire);
            list.Add(DungeonModifierType.NoItemTypeDropArmor);
            list.Add(DungeonModifierType.MonsterSkillResistance);
            list.Add(DungeonModifierType.MonsterWeaponDeflect);
            list.Add(DungeonModifierType.HordeMaxSize);
            ruleset.DungeonModifiers = list;
            dictionary.Add(DungeonRulesetType.Ruleset001, ruleset);
            ruleset = new DungeonRuleset();
            list = new List<DungeonModifierType>();
            list.Add(DungeonModifierType.StartingItemBeast);
            list.Add(DungeonModifierType.NoItemTypeDropArmor);
            list.Add(DungeonModifierType.MonsterWeaponResistance);
            list.Add(DungeonModifierType.MonsterSkillDeflect);
            list.Add(DungeonModifierType.MonsterFreeze);
            ruleset.DungeonModifiers = list;
            dictionary.Add(DungeonRulesetType.Ruleset002, ruleset);
            ruleset = new DungeonRuleset();
            list = new List<DungeonModifierType>();
            list.Add(DungeonModifierType.StartingItemSureshot);
            list.Add(DungeonModifierType.NoItemTypeDropWeapon);
            list.Add(DungeonModifierType.MonsterWeaponResistance);
            list.Add(DungeonModifierType.MonsterIncreasedAttackSpeed);
            list.Add(DungeonModifierType.MonsterCanCrit);
            list.Add(DungeonModifierType.GlobalCriticalHitDamageBonus);
            ruleset.DungeonModifiers = list;
            dictionary.Add(DungeonRulesetType.Ruleset003, ruleset);
            ruleset = new DungeonRuleset();
            list = new List<DungeonModifierType>();
            list.Add(DungeonModifierType.StartingItemBaron);
            list.Add(DungeonModifierType.NoItemTypeDropCloak);
            list.Add(DungeonModifierType.MonsterIncreasedDamage);
            list.Add(DungeonModifierType.MonsterExploding);
            list.Add(DungeonModifierType.HeroIncreasedCleaveDamagePct);
            ruleset.DungeonModifiers = list;
            dictionary.Add(DungeonRulesetType.Ruleset004, ruleset);
            ruleset = new DungeonRuleset();
            list = new List<DungeonModifierType>();
            list.Add(DungeonModifierType.StartingItemMojo);
            list.Add(DungeonModifierType.NoItemTypeDropCloak);
            list.Add(DungeonModifierType.MonsterHeroResistance);
            list.Add(DungeonModifierType.AllyIncreasedDamage);
            list.Add(DungeonModifierType.HeroDecreasedSkillCooldowns);
            ruleset.DungeonModifiers = list;
            dictionary.Add(DungeonRulesetType.Ruleset005, ruleset);
            ruleset = new DungeonRuleset();
            list = new List<DungeonModifierType>();
            list.Add(DungeonModifierType.StartingItemSnowflake);
            list.Add(DungeonModifierType.NoItemTypeDropCloak);
            list.Add(DungeonModifierType.MonsterIncreasedLife);
            list.Add(DungeonModifierType.HeroPostSkillIncreasedSkillDamage);
            list.Add(DungeonModifierType.HeroIncreasedSkillCooldowns);
            ruleset.DungeonModifiers = list;
            dictionary.Add(DungeonRulesetType.Ruleset006, ruleset);
            RULESETS = dictionary;
            Dictionary<DungeonModifierType, DungeonModifier> dictionary2 = new Dictionary<DungeonModifierType, DungeonModifier>(new DungeonModifierTypeBoxAvoidanceComparer());
            DungeonModifier modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_ALLY_INCREASED_DAMAGE;
            dictionary2.Add(DungeonModifierType.AllyIncreasedDamage, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_DUNGEON_BOOST_BOX_BONUS_COINS;
            dictionary2.Add(DungeonModifierType.DungeonBoostBoxBonusCoins, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_DUNGEON_BOOST_BOX_BONUS_SKILL_DAMAGE;
            dictionary2.Add(DungeonModifierType.DungeonBoostBoxBonusSkillDamage, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_DUNGEON_BOOST_BOX_BONUS_UNIVERSAL_DAMAGE;
            dictionary2.Add(DungeonModifierType.DungeonBoostBoxBonusUniversalDamage, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_DUNGEON_BOOST_BOX_BONUS_WEAPON_DAMAGE;
            dictionary2.Add(DungeonModifierType.DungeonBoostBoxBonusWeaponDamage, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_GLOBAL_CRITICAL_HIT_DAMAGE_BONUS;
            dictionary2.Add(DungeonModifierType.GlobalCriticalHitDamageBonus, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_HERO_DECREASED_SKILL_COOLDOWNS;
            dictionary2.Add(DungeonModifierType.HeroDecreasedSkillCooldowns, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_HERO_INCREASED_CLEAVE_DAMAGE_PCT;
            dictionary2.Add(DungeonModifierType.HeroIncreasedCleaveDamagePct, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_HERO_INCREASED_SKILL_COOLDOWNS;
            dictionary2.Add(DungeonModifierType.HeroIncreasedSkillCooldowns, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_HERO_MAX_SPEED;
            dictionary2.Add(DungeonModifierType.HeroMaxSpeed, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_HERO_SKILL_USE_INCREASED_SKILL_DAMAGE;
            dictionary2.Add(DungeonModifierType.HeroPostSkillIncreasedSkillDamage, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_HORDE_MAX_SIZE;
            dictionary2.Add(DungeonModifierType.HordeMaxSize, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_ItemType = ItemType.Weapon;
            dictionary2.Add(DungeonModifierType.NoItemTypeDropWeapon, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_ItemType = ItemType.Armor;
            dictionary2.Add(DungeonModifierType.NoItemTypeDropArmor, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_ItemType = ItemType.Cloak;
            dictionary2.Add(DungeonModifierType.NoItemTypeDropCloak, modifier);
            dictionary2.Add(DungeonModifierType.MonsterCanCrit, new DungeonModifier());
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_CRIT_BLOCK;
            dictionary2.Add(DungeonModifierType.MonsterCritBlock, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_EXPLODING;
            dictionary2.Add(DungeonModifierType.MonsterExploding, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_HERO_RESISTANCE;
            dictionary2.Add(DungeonModifierType.MonsterHeroResistance, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_INCREASED_ATTACK_SPEED;
            dictionary2.Add(DungeonModifierType.MonsterIncreasedAttackSpeed, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_INCREASED_DAMAGE;
            dictionary2.Add(DungeonModifierType.MonsterIncreasedDamage, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_INCREASED_LIFE;
            dictionary2.Add(DungeonModifierType.MonsterIncreasedLife, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_NO_COINS;
            dictionary2.Add(DungeonModifierType.MonsterNoCoins, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_SKILL_DEFLECT;
            dictionary2.Add(DungeonModifierType.MonsterSkillDeflect, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_SKILL_RESISTANCE;
            dictionary2.Add(DungeonModifierType.MonsterSkillResistance, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_WEAPON_DEFLECT;
            dictionary2.Add(DungeonModifierType.MonsterWeaponDeflect, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_WEAPON_RESISTANCE;
            dictionary2.Add(DungeonModifierType.MonsterWeaponResistance, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_FREEZE;
            dictionary2.Add(DungeonModifierType.MonsterFreeze, modifier);
            modifier = new DungeonModifier();
            modifier.Description = ConfigLoca.DMODIFIER_MONSTER_STUN;
            dictionary2.Add(DungeonModifierType.MonsterStun, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_String = "Armor011";
            modifier.Parameter_ItemType = ItemType.Armor;
            modifier.Parameter_Int = 5;
            dictionary2.Add(DungeonModifierType.StartingItemLegionnaire, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_String = "Armor022";
            modifier.Parameter_ItemType = ItemType.Armor;
            modifier.Parameter_Int = 5;
            dictionary2.Add(DungeonModifierType.StartingItemBeast, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_String = "Weapon013";
            modifier.Parameter_ItemType = ItemType.Weapon;
            modifier.Parameter_Int = 5;
            dictionary2.Add(DungeonModifierType.StartingItemGoldie, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_String = "Armor012";
            modifier.Parameter_ItemType = ItemType.Armor;
            modifier.Parameter_Int = 5;
            dictionary2.Add(DungeonModifierType.StartingItemSpike, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_String = "Weapon015";
            modifier.Parameter_ItemType = ItemType.Weapon;
            modifier.Parameter_Int = 5;
            dictionary2.Add(DungeonModifierType.StartingItemSureshot, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_String = "Weapon018";
            modifier.Parameter_ItemType = ItemType.Weapon;
            modifier.Parameter_Int = 5;
            dictionary2.Add(DungeonModifierType.StartingItemGrande, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_String = "Armor013";
            modifier.Parameter_ItemType = ItemType.Armor;
            modifier.Parameter_Int = 5;
            dictionary2.Add(DungeonModifierType.StartingItemNightmare, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_String = "Cloak011";
            modifier.Parameter_ItemType = ItemType.Cloak;
            modifier.Parameter_Int = 5;
            dictionary2.Add(DungeonModifierType.StartingItemBaron, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_String = "Cloak025";
            modifier.Parameter_ItemType = ItemType.Cloak;
            modifier.Parameter_Int = 5;
            dictionary2.Add(DungeonModifierType.StartingItemMojo, modifier);
            modifier = new DungeonModifier();
            modifier.Parameter_String = "Cloak021";
            modifier.Parameter_ItemType = ItemType.Cloak;
            modifier.Parameter_Int = 5;
            dictionary2.Add(DungeonModifierType.StartingItemSnowflake, modifier);
            MODIFIERS = dictionary2;
            ALL_MODIFIERS = LangUtil.GetEnumValuesWithException<DungeonModifierType>(DungeonModifierType.NONE);
        }

        public static DungeonRuleset GetRulesetForId(string rulesetId)
        {
            if (!string.IsNullOrEmpty(rulesetId))
            {
                try
                {
                    DungeonRulesetType type = (DungeonRulesetType) ((int) Enum.Parse(typeof(DungeonRulesetType), rulesetId));
                    return RULESETS[type];
                }
                catch (Exception)
                {
                    Debug.LogError("Unparseable tournament ruleset encountered: " + rulesetId);
                }
            }
            return null;
        }

        public static class AllyIncreasedDamage
        {
            public static double RawDamageMultiplier = 1.25;
        }

        public static class DungeonBoostBoxBonusCoins
        {
            public static double CoinMultiplier = 2.0;
        }

        public static class DungeonBoostBoxBonusSkillDamage
        {
            public static float BuffDurationSeconds = 10f;
            public static float SkillDamageModifier = 0.15f;
        }

        public static class DungeonBoostBoxBonusUniversalDamage
        {
            public static float BuffDurationSeconds = 20f;
            public static float Modifier = 0.1f;
        }

        public static class DungeonBoostBoxBonusWeaponDamage
        {
            public static float BuffDurationSeconds = 10f;
            public static float DamagePerHitModifier = 0.15f;
        }

        public static class GlobalCriticalHitDamageBonus
        {
            public static float MonsterMultiplier = 3f;
            public static float PlayerCharacterMultiplier = 2f;
        }

        public static class HeroDecreasedSkillCooldowns
        {
            public static float CooldownModifier = -0.33f;
        }

        public static class HeroIncreasedSkillCooldowns
        {
            public static float CooldownModifier = 0.33f;
        }

        public static class HeroPostSkillIncreasedSkillDamage
        {
            public static float BuffDurationSeconds = 10f;
            public static object BuffSource = new object();
            public static float Modifier = 0.33f;
        }

        public static class MonsterCanCrit
        {
            public static float BossCritChance = 0.33f;
            public static float MinionCritChance = 0.2f;
        }

        public static class MonsterExploding
        {
            public static float DamagePct = 0.18f;
            public static float ProcChance = 0.4f;
        }

        public static class MonsterFreeze
        {
            public static float BossBuffDurationSeconds = 3f;
            public static float BossProcChance = 0.33f;
            public static float MinionBuffDurationSeconds = 2f;
            public static float MinionProcChance = 0.15f;
        }

        public static class MonsterHeroResistance
        {
            public static float Modifier = 0.66f;
        }

        public static class MonsterIncreasedAttackSpeed
        {
            public static float RawAttacksPerSecondIncrease = 1f;
        }

        public static class MonsterIncreasedDamage
        {
            public static double RawDamageMultiplier = 1.5;
        }

        public static class MonsterIncreasedLife
        {
            public static double RawLifeMultiplier = 2.0;
        }

        public static class MonsterSkillDeflect
        {
            public static float DeflectionPct = 0.33f;
        }

        public static class MonsterSkillResistance
        {
            public static float Modifier = 0.5f;
        }

        public static class MonsterStun
        {
            public static float BossBuffDurationSeconds = 1f;
            public static float BossProcChance = 0.33f;
            public static float MinionBuffDurationSeconds = 0.33f;
            public static float MinionProcChance = 0.15f;
        }

        public static class MonsterWeaponDeflect
        {
            public static float DeflectionPct = 0.33f;
        }

        public static class MonsterWeaponResistance
        {
            public static float Modifier = 0.5f;
        }
    }
}

