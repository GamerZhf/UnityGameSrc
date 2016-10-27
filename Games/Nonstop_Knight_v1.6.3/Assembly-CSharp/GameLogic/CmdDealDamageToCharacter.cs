namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [ConsoleCommand("damage")]
    public class CmdDealDamageToCharacter : ICommand
    {
        private double m_baseAmount;
        private bool m_critted;
        private DamageType m_damageType;
        private SkillType m_fromSkill;
        private CharacterInstance m_sourceCharacter;
        private CharacterInstance m_targetCharacter;

        public CmdDealDamageToCharacter(string[] serialized)
        {
            this.m_sourceCharacter = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters[0];
            this.m_targetCharacter = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter;
            this.m_baseAmount = int.Parse(serialized[0]);
        }

        public CmdDealDamageToCharacter(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, double baseAmount, bool critted, DamageType damageType, [Optional, DefaultParameterValue(0)] SkillType fromSkill)
        {
            this.m_sourceCharacter = sourceCharacter;
            this.m_targetCharacter = targetCharacter;
            this.m_baseAmount = baseAmount;
            this.m_critted = critted;
            this.m_damageType = damageType;
            this.m_fromSkill = fromSkill;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator52 iterator = new <executeRoutine>c__Iterator52();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, double baseAmount, bool critted, DamageType damageType, [Optional, DefaultParameterValue(0)] SkillType fromSkill)
        {
            if (((sourceCharacter == null) || !sourceCharacter.IsDead) && ((targetCharacter != null) && !targetCharacter.IsDead))
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                if ((!ConfigApp.CHEAT_IGNORE_ALL_DAMAGE && !targetCharacter.isInvulnerable()) && (activeDungeon.ActiveRoom.numberOfCharactersAlive() != 1))
                {
                    targetCharacter.AttackTargetCounter++;
                    float num = 0f;
                    float num2 = 0f;
                    if (sourceCharacter != null)
                    {
                        num += sourceCharacter.getCharacterTypeDamageModifier(targetCharacter.Type);
                        num2 += targetCharacter.getCharacterTypeArmorModifier(sourceCharacter.Type);
                    }
                    if ((sourceCharacter != null) && sourceCharacter.IsBoss)
                    {
                        num2 += targetCharacter.getGenericModifierForPerkType(PerkType.ArmorBonusBoss);
                    }
                    if ((sourceCharacter != null) && targetCharacter.IsBoss)
                    {
                        num += sourceCharacter.getGenericModifierForPerkType(PerkType.ElementBonusBoss);
                    }
                    if (damageType == DamageType.Ranged)
                    {
                        num2 += targetCharacter.getGenericModifierForPerkType(PerkType.ArmorBonusRanged);
                    }
                    else if (damageType == DamageType.Melee)
                    {
                        num2 += targetCharacter.getGenericModifierForPerkType(PerkType.ArmorBonusMelee);
                        if (!targetCharacter.IsPlayerCharacter && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterWeaponResistance))
                        {
                            num2 += ConfigDungeonModifiers.MonsterWeaponResistance.Modifier;
                        }
                    }
                    else if (((damageType == DamageType.Magic) && !targetCharacter.IsPlayerCharacter) && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterSkillResistance))
                    {
                        num2 += ConfigDungeonModifiers.MonsterSkillResistance.Modifier;
                    }
                    if ((!targetCharacter.IsPlayerCharacter && (sourceCharacter != null)) && (sourceCharacter.IsPrimaryPlayerCharacter && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterHeroResistance)))
                    {
                        num2 += ConfigDungeonModifiers.MonsterHeroResistance.Modifier;
                    }
                    if (targetCharacter.IsBoss)
                    {
                        if (damageType == DamageType.Magic)
                        {
                            num2 += targetCharacter.getGenericModifierForPerkType(PerkType.BossResistSkillDamage);
                        }
                        else
                        {
                            num2 += targetCharacter.getGenericModifierForPerkType(PerkType.BossResistWeaponDamage);
                        }
                    }
                    num2 += targetCharacter.UniversalArmorBonus(true);
                    double num3 = MathUtil.Clamp(baseAmount + (baseAmount * num), 0.0, double.MaxValue);
                    if (((sourceCharacter != null) && !sourceCharacter.IsPlayerCharacter) && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterIncreasedDamage))
                    {
                        num3 *= ConfigDungeonModifiers.MonsterIncreasedDamage.RawDamageMultiplier;
                    }
                    if (((sourceCharacter != null) && sourceCharacter.IsSupport) && activeDungeon.hasDungeonModifier(DungeonModifierType.AllyIncreasedDamage))
                    {
                        num3 *= ConfigDungeonModifiers.AllyIncreasedDamage.RawDamageMultiplier;
                    }
                    double num4 = targetCharacter.MaxLife(true);
                    double num5 = num4 + (num4 * num2);
                    double num6 = MathUtil.Clamp(num3 / num5, 0.0, double.MaxValue);
                    double v = MathUtil.Clamp(num4 * num6, 0.0, double.MaxValue);
                    bool damageReduced = num2 > 0f;
                    bool flag3 = false;
                    if (!flag3 && (damageType == DamageType.Ranged))
                    {
                        float num8 = Mathf.Min(targetCharacter.getGenericModifierForPerkType(PerkType.ArmorBlockRanged), ConfigGameplay.GLOBAL_CAP_ARMOR_BLOCK_MODIFIER);
                        if ((num8 > 0f) && (UnityEngine.Random.Range((float) 0f, (float) 1f) <= num8))
                        {
                            flag3 = true;
                        }
                    }
                    if (!flag3 && (damageType == DamageType.Melee))
                    {
                        float num9 = Mathf.Min(targetCharacter.getGenericModifierForPerkType(PerkType.ArmorBlockMelee), ConfigGameplay.GLOBAL_CAP_ARMOR_BLOCK_MODIFIER);
                        if ((num9 > 0f) && (UnityEngine.Random.Range((float) 0f, (float) 1f) <= num9))
                        {
                            flag3 = true;
                        }
                    }
                    if (!flag3 && (damageType != DamageType.Magic))
                    {
                        float num10 = Mathf.Min(targetCharacter.getGenericModifierForPerkType(PerkType.ArmorBlockAll), ConfigGameplay.GLOBAL_CAP_ARMOR_BLOCK_MODIFIER);
                        if ((num10 > 0f) && (UnityEngine.Random.Range((float) 0f, (float) 1f) <= num10))
                        {
                            flag3 = true;
                        }
                        if ((targetCharacter.IsBoss && (targetCharacter.getPerkInstanceCount(PerkType.BossDodge) > 0)) && ((targetCharacter.AttackTargetCounter % Mathf.RoundToInt(targetCharacter.getGenericModifierForPerkType(PerkType.BossDodge))) == 0))
                        {
                            flag3 = true;
                        }
                        if ((targetCharacter.getPerkInstanceCount(PerkType.Evasion) > 0) && ((targetCharacter.AttackTargetCounter % 3) == 0))
                        {
                            flag3 = true;
                        }
                    }
                    if (!flag3 && GameLogic.Binder.BuffSystem.hasBuffFromPerk(targetCharacter, PerkType.AuraLowHpDodge))
                    {
                        float num11 = targetCharacter.getGenericModifierForPerkType(PerkType.AuraLowHpDodge);
                        if ((num11 > 0f) && (UnityEngine.Random.Range((float) 0f, (float) 1f) <= num11))
                        {
                            flag3 = true;
                        }
                    }
                    if ((!flag3 && critted) && (!targetCharacter.IsPlayerCharacter && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterCritBlock)))
                    {
                        flag3 = true;
                    }
                    if (flag3)
                    {
                        v = 0.0;
                        damageReduced = true;
                    }
                    if (targetCharacter.IsBoss && (targetCharacter.getPerkInstanceCount(PerkType.BossResistCriticalDamage) > 0))
                    {
                        critted = false;
                    }
                    if ((targetCharacter.IsPlayerCharacter && !activeDungeon.ActiveRoom.MainBossSummoned) && (sourceCharacter != null))
                    {
                        v = MathUtil.Clamp(v, 0.0, targetCharacter.MaxLife(true) * ConfigGameplay.HERO_HP_SAFEGUARD_MAX_HP_LOSS_PER_MINION_HIT_NORMALIZED);
                    }
                    if (double.IsNaN(v))
                    {
                        v = 0.0;
                    }
                    targetCharacter.CurrentHp = MathUtil.ClampMin(Math.Round((double) (targetCharacter.CurrentHp - v)), 0.0);
                    if (((targetCharacter == activeDungeon.PrimaryPlayerCharacter) && (targetCharacter.CurrentHp < 1.0)) && activeDungeon.isTutorialDungeon())
                    {
                        targetCharacter.CurrentHp = 1.0;
                    }
                    GameLogic.Binder.EventBus.CharacterDealtDamage(sourceCharacter, targetCharacter, targetCharacter.PhysicsBody.Transform.position, v, critted, damageReduced, damageType, fromSkill);
                }
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator52 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdDealDamageToCharacter <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    CmdDealDamageToCharacter.ExecuteStatic(this.<>f__this.m_sourceCharacter, this.<>f__this.m_targetCharacter, this.<>f__this.m_baseAmount, this.<>f__this.m_critted, this.<>f__this.m_damageType, this.<>f__this.m_fromSkill);
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

