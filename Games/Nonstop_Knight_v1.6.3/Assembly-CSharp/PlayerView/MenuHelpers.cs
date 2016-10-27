namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UnityEngine.UI;

    public static class MenuHelpers
    {
        private static BigNumberFormatter BigNumberFormatter = new BigNumberFormatter();

        public static string BigModifierToString(float m, [Optional, DefaultParameterValue(true)] bool doAddPrefixSign)
        {
            string str = BigValueToString(m * 100.0);
            if ((m > 0f) && doAddPrefixSign)
            {
                return ("+" + str + "%");
            }
            return (str + "%");
        }

        public static string BigValueToString(double v)
        {
            object[] args = new object[] { v };
            return string.Format(BigNumberFormatter, "{0:####.##}", args);
        }

        public static int CalculateNumBurstsForDiamondPurchase(double amount)
        {
            if (amount > 6000.0)
            {
                return 0x10;
            }
            if (amount > 3000.0)
            {
                return 12;
            }
            if (amount > 1000.0)
            {
                return 8;
            }
            if (amount > 500.0)
            {
                return 6;
            }
            if (amount > 200.0)
            {
                return 4;
            }
            return 2;
        }

        public static string ColoredText(object text)
        {
            object[] objArray1 = new object[] { "<color=#", ConfigUi.TEXT_HIGHLIGHT_COLOR_HEX, ">", text, "</color>" };
            return string.Concat(objArray1);
        }

        private static bool DoUsePerkType(PerkType perkType, CharacterInstance targetCharacter, PerkType targetPerkType)
        {
            return ((targetPerkType == perkType) || ((targetPerkType == PerkType.NONE) && (targetCharacter.getPerkInstanceCount(perkType) > 0)));
        }

        public static string GetFormattedDescription(string description, string key, int value)
        {
            return description.Replace(key, value.ToString());
        }

        public static string GetFormattedDescription(string description, string key, string value)
        {
            return description.Replace(key, value);
        }

        public static string GetFormattedDescriptionColored(string description, string key, int value)
        {
            object[] objArray1 = new object[] { "<color=#", ConfigUi.TEXT_HIGHLIGHT_COLOR_HEX, ">", value, "</color>" };
            return description.Replace(key, string.Concat(objArray1));
        }

        public static string GetFormattedDescriptionColored(string description, string key, string value)
        {
            string[] textArray1 = new string[] { "<color=#", ConfigUi.TEXT_HIGHLIGHT_COLOR_HEX, ">", value, "</color>" };
            return description.Replace(key, string.Concat(textArray1));
        }

        public static string GetFormattedItemPerkDescription(ItemInstance ii, PerkInstance pi)
        {
            ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[pi.Type];
            string input = _.L(data.Description, null, false);
            Match match = Regex.Match(input, @"(\$.*\$)");
            if ((match.Groups.Count <= 1) || !match.Groups[1].Success)
            {
                return "CHANGE ME";
            }
            int evolveRank = ii.EvolveRank;
            string baseString = match.Groups[1].ToString();
            string str3 = GetFormattedPerkString(pi.Type, baseString, pi.Modifier, data.DurationSeconds, data.Threshold, ii.getEvolveBonusForPerk(pi.Type, 1), false, false, false);
            string str4 = GetFormattedPerkString(pi.Type, baseString, pi.Modifier, data.DurationSeconds, data.Threshold, ii.getEvolveBonusForPerk(pi.Type, evolveRank), false, true, false);
            if (1 < evolveRank)
            {
                string[] textArray1 = new string[] { "<color=#", ConfigUi.PERK_DESCRIPTION_VARIABLE_TEXT_COLOR_HEX, ">", str3, "</color>", "+", "<color=#", ConfigUi.PERK_DESCRIPTION_BONUS_VARIABLE_TEXT_COLOR_HEX, ">", str4, "</color>" };
                return input.Replace(baseString, string.Concat(textArray1));
            }
            string[] textArray2 = new string[] { "<color=#", ConfigUi.PERK_DESCRIPTION_VARIABLE_TEXT_COLOR_HEX, ">", str4, "</color>" };
            return input.Replace(baseString, string.Concat(textArray2));
        }

        public static string GetFormattedPerkDescription(PerkType perkType, float baseModifier, float baseDuration, float baseThreshold, float bonus, bool colors)
        {
            ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[perkType];
            return GetFormattedPerkString(perkType, _.L(data.Description, null, false), baseModifier, baseDuration, baseThreshold, bonus, !data.HideDescriptionModifierPrefix, true, colors);
        }

        public static string GetFormattedPerkString(PerkType perkType, string baseString, float baseModifier, float baseDuration, float baseThreshold, float bonus, bool showPrefix, bool showPostfix, bool colors)
        {
            StringBuilder builder = new StringBuilder(baseString);
            string str = string.Empty;
            float num = 0f;
            string format = string.Empty;
            string str3 = string.Empty;
            if (baseString.Contains("$ModifierPct$"))
            {
                str = "$ModifierPct$";
                num = (baseModifier + bonus) * 100f;
                format = !showPrefix ? "0;-0" : "+0;-0";
                str3 = "%";
            }
            else if (baseString.Contains("$ModifierInt$"))
            {
                str = "$ModifierInt$";
                num = baseModifier + bonus;
                format = "0;-0";
                str3 = string.Empty;
            }
            else if (baseString.Contains("$DurationSec$"))
            {
                str = "$DurationSec$";
                num = baseDuration + bonus;
                format = "0.0;-0.0";
                str3 = !showPostfix ? string.Empty : (" " + _.L(ConfigLoca.UNIT_SECONDS_LONG, null, false));
            }
            else if (baseString.Contains("$ThresholdPct$"))
            {
                str = "$ThresholdPct$";
                num = (baseThreshold + bonus) * 100f;
                format = "0;-0";
                str3 = "%";
            }
            if (!string.IsNullOrEmpty(str))
            {
                if (colors)
                {
                    builder.Replace(str, "<color=#" + ConfigUi.PERK_DESCRIPTION_VARIABLE_TEXT_COLOR_HEX + ">" + num.ToString(format) + str3 + "</color>");
                }
                else
                {
                    builder.Replace(str, num.ToString(format) + str3);
                }
            }
            return builder.ToString();
        }

        public static string GetFormattedPlayerUpgradePerkDescription(PerkInstance pi, float evolveBonus, bool colors)
        {
            ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[pi.Type];
            string input = _.L(data.Description, null, false);
            Match match = Regex.Match(input, @"(\$.*\$)");
            if ((match.Groups.Count <= 1) || !match.Groups[1].Success)
            {
                return input;
            }
            string baseString = match.Groups[1].ToString();
            string newValue = GetFormattedPerkString(pi.Type, baseString, pi.Modifier, data.DurationSeconds, data.Threshold, evolveBonus, false, true, false);
            if (colors)
            {
                string[] textArray1 = new string[] { "<color=#", ConfigUi.PERK_DESCRIPTION_VARIABLE_TEXT_COLOR_HEX, ">", newValue, "</color>" };
                newValue = string.Concat(textArray1);
            }
            return input.Replace(baseString, newValue);
        }

        public static string GetFormattedSkillDescription(string baseText, CharacterInstance character, SkillType skillType, PerkType perkType, bool bonusDamageSeparately, bool colors)
        {
            int bestModifier = 0;
            float num2 = 0f;
            double baseAmount = 0.0;
            double v = 0.0;
            switch (skillType)
            {
                case SkillType.Omnislash:
                    int num5;
                    int num6;
                    float num7;
                    float num8;
                    double num9;
                    double num10;
                    OmnislashSkill.CalculateTotalDamage(character, out num7, out num8, out num6, out num5, out num9, out num10);
                    bestModifier = num5;
                    baseAmount = (num9 + num10) / ((double) num5);
                    v = 0.0;
                    break;

                case SkillType.Slam:
                    baseAmount = character.SkillDamage(true) * ConfigSkills.Slam.DamagePct;
                    v = CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(character, SkillType.Slam, baseAmount) - baseAmount;
                    break;

                case SkillType.Implosion:
                    if (perkType == PerkType.SkillUpgradeImplosion1)
                    {
                        bestModifier = (int) ConfigPerks.GetBestModifier(PerkType.SkillUpgradeImplosion1);
                    }
                    baseAmount = character.SkillDamage(true) * ConfigSkills.Implosion.DamagePct;
                    v = CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(character, SkillType.Implosion, baseAmount) - baseAmount;
                    break;

                case SkillType.Clone:
                {
                    baseAmount = character.SkillDamage(true) * ConfigSkills.Clone.DphMultiplier;
                    v = CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(character, SkillType.Clone, baseAmount) - baseAmount;
                    Character summonedCharacterPrototype = GameLogic.Binder.CharacterResources.getResource(CloneSkill.CLONE_CHARACTER_ID);
                    num2 = character.getLimitedLifetimeForSummon(summonedCharacterPrototype);
                    break;
                }
                case SkillType.Leap:
                    baseAmount = character.SkillDamage(true) * ConfigSkills.Leap.DamagePct;
                    v = CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(character, SkillType.Leap, baseAmount) - baseAmount;
                    break;

                case SkillType.Whirlwind:
                {
                    bool flag = perkType == PerkType.SkillUpgradeWhirlwind1;
                    bool flag2 = perkType == PerkType.SkillUpgradeWhirlwind4;
                    if (flag2)
                    {
                        baseAmount = character.SkillDamage(true) * ConfigSkills.Whirlwind.HugeRuneTotalDamagePct;
                    }
                    else
                    {
                        baseAmount = character.SkillDamage(true) * ConfigSkills.Whirlwind.TotalDamagePct;
                    }
                    if (flag)
                    {
                        bestModifier = ConfigSkills.Whirlwind.ShieldRuneTotalSpinCount;
                    }
                    else if (flag2)
                    {
                        bestModifier = ConfigSkills.Whirlwind.HugeRuneTotalSpinCount;
                    }
                    else
                    {
                        bestModifier = ConfigSkills.Whirlwind.TotalSpinCount;
                    }
                    v = CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(character, SkillType.Whirlwind, baseAmount) - baseAmount;
                    break;
                }
            }
            StringBuilder builder = new StringBuilder(baseText);
            if (v >= 1.0)
            {
                if (bonusDamageSeparately)
                {
                    if (colors)
                    {
                        builder.Replace("$Damage$", "<color=#" + ConfigUi.SKILL_DESCRIPTION_VARIABLE_TEXT_COLOR_HEX + ">" + BigValueToString(baseAmount) + "</color>" + "<color=#" + ConfigUi.SKILL_DESCRIPTION_BONUS_VARIABLE_TEXT_COLOR_HEX + ">+" + BigValueToString(v) + "</color>");
                    }
                    else
                    {
                        builder.Replace("$Damage$", BigValueToString(baseAmount) + "+" + BigValueToString(v));
                    }
                }
                else if (colors)
                {
                    builder.Replace("$Damage$", "<color=#" + ConfigUi.SKILL_DESCRIPTION_VARIABLE_TEXT_COLOR_HEX + ">" + BigValueToString(baseAmount + v) + "</color>");
                }
                else
                {
                    builder.Replace("$Damage$", BigValueToString(baseAmount + v));
                }
            }
            else if (colors)
            {
                builder.Replace("$Damage$", "<color=#" + ConfigUi.SKILL_DESCRIPTION_VARIABLE_TEXT_COLOR_HEX + ">" + BigValueToString(baseAmount) + "</color>");
            }
            else
            {
                builder.Replace("$Damage$", BigValueToString(baseAmount));
            }
            if (colors)
            {
                builder.Replace("$Count$", string.Concat(new object[] { "<color=#", ConfigUi.SKILL_DESCRIPTION_VARIABLE_TEXT_COLOR_HEX, ">", bestModifier, "</color>" }));
                builder.Replace("$Duration$", "<color=#" + ConfigUi.SKILL_DESCRIPTION_VARIABLE_TEXT_COLOR_HEX + ">" + num2.ToString("0.0") + "</color>");
            }
            else
            {
                builder.Replace("$Count$", bestModifier.ToString());
                builder.Replace("$Duration$", num2.ToString("0.0"));
            }
            return builder.ToString();
        }

        public static string GetFormattedTip(string rawTipString, HeroStats heroStats)
        {
            SkillType key = heroStats.getMostUsedSkill();
            SkillType nONE = heroStats.getMostDeadlySkill();
            GameLogic.CharacterType uNSPECIFIED = heroStats.getMostKilledEnemy();
            if ((uNSPECIFIED != GameLogic.CharacterType.UNSPECIFIED) && !ConfigUi.CHARACTER_TYPE_NAMES.ContainsKey(uNSPECIFIED))
            {
                Debug.LogWarning("Character type name not found for " + uNSPECIFIED);
                uNSPECIFIED = GameLogic.CharacterType.UNSPECIFIED;
            }
            if ((key != SkillType.NONE) && !ConfigSkills.SHARED_DATA.ContainsKey(key))
            {
                Debug.LogWarning("Skill name not found for " + key);
                key = SkillType.NONE;
            }
            if ((nONE != SkillType.NONE) && !ConfigSkills.SHARED_DATA.ContainsKey(nONE))
            {
                Debug.LogWarning("Skill name not found for " + nONE);
                nONE = SkillType.NONE;
            }
            return _.L(rawTipString, null, false).Replace("$CoinsEarned$", ColoredText(_.L(ConfigLoca.HEROSTATS_COINS_EARNED, null, false))).Replace("$CoinsEarned_Value$", ColoredText(BigValueToString(heroStats.CoinsEarned))).Replace("$HighestMultikill$", ColoredText(_.L(ConfigLoca.HEROSTATS_HIGHEST_MULTIKILL, null, false))).Replace("$HighestMultikill_Value$", ColoredText(heroStats.HighestMultikill.ToString())).Replace("$DamageDealt$", ColoredText(_.L(ConfigLoca.HEROSTATS_DAMAGE_DEALT, null, false))).Replace("$DamageDealt_Value$", ColoredText(BigValueToString(heroStats.DamageDealt))).Replace("$MostUsedSkill$", ColoredText(_.L(ConfigLoca.HEROSTATS_MOST_USED_SKILL, null, false))).Replace("$MostUsedSkill_Value$", ColoredText((key == SkillType.NONE) ? "-" : _.L(ConfigSkills.SHARED_DATA[key].Name, null, false))).Replace("$MostKilledEnemy$", ColoredText(_.L(ConfigLoca.HEROSTATS_MOST_KILLED_ENEMY, null, false))).Replace("$MostKilledEnemy_Value$", ColoredText((uNSPECIFIED == GameLogic.CharacterType.UNSPECIFIED) ? "-" : _.L(ConfigUi.CHARACTER_TYPE_NAMES[uNSPECIFIED], null, false))).Replace("$HighestCriticalHit$", ColoredText(_.L(ConfigLoca.HEROSTATS_HIGHEST_CRITICAL_HIT, null, false))).Replace("$HighestCriticalHit_Value$", ColoredText(BigValueToString(heroStats.HighestCriticalHit))).Replace("$MostDeadlySkill$", ColoredText(_.L(ConfigLoca.HEROSTATS_MOST_DEADLY_SKILL, null, false))).Replace("$MostDeadlySkill_Value$", ColoredText((nONE == SkillType.NONE) ? "-" : _.L(ConfigSkills.SHARED_DATA[nONE].Name, null, false))).Replace("$FloorsCompleted$", ColoredText(_.L(ConfigLoca.HEROSTATS_FLOORS_COMPLETED, null, false))).Replace("$FloorsCompleted_Value$", ColoredText(heroStats.FloorsCompleted.ToString())).Replace("$ItemsUnlocked$", ColoredText(_.L(ConfigLoca.HEROSTATS_ITEMS_UNLOCKED, null, false))).Replace("$ItemsUnlocked_Value$", ColoredText(BigValueToString(heroStats.ItemsUnlocked))).Replace("$EnemiesKilled$", ColoredText(_.L(ConfigLoca.HEROSTATS_ENEMIES_KILLED, null, false))).Replace("$EnemiesKilled_Value$", ColoredText(BigValueToString(heroStats.MonstersKilled)));
        }

        public static string GetPetAttackTypeDescription(Character petCharacter)
        {
            switch (petCharacter.CoreAiBehaviour)
            {
                case AiBehaviourType.SupportMeleeAtk:
                    return _.L(ConfigLoca.PET_ATTACK_TYPE_MELEE, null, false);

                case AiBehaviourType.SupportRangedAtk:
                    return _.L(ConfigLoca.PET_ATTACK_TYPE_RANGED, null, false);

                case AiBehaviourType.SupportDasher:
                    return _.L(ConfigLoca.PET_ATTACK_TYPE_DASH, null, false);

                case AiBehaviourType.SupportWhirler:
                    return _.L(ConfigLoca.PET_ATTACK_TYPE_WHIRL, null, false);

                case AiBehaviourType.SupportLeaper:
                    return _.L(ConfigLoca.PET_ATTACK_TYPE_LEAP, null, false);

                case AiBehaviourType.SupportCharger:
                    return _.L(ConfigLoca.PET_ATTACK_TYPE_CHARGE, null, false);

                case AiBehaviourType.SupportSplashSlammer:
                    return _.L(ConfigLoca.PET_ATTACK_TYPE_SLAM, null, false);
            }
            return _.L(ConfigLoca.PET_ATTACK_TYPE_RANGED, null, false);
        }

        public static string GetRarityColoredText(int rarity, string str, [Optional, DefaultParameterValue(1f)] float alpha)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<color=#");
            Color color = ConfigUi.RARITY_TEXT_COLORS[rarity];
            color.a = alpha;
            builder.Append(ColorUtil.ColorToHex(color));
            builder.Append(">");
            builder.Append(str);
            builder.Append("</color>");
            return builder.ToString();
        }

        public static void RefreshItemCompareIndicator(Image targetIndicator, float a, float b, [Optional, DefaultParameterValue(true)] bool useDownArrow)
        {
            if (a < b)
            {
                if (useDownArrow)
                {
                    targetIndicator.enabled = true;
                    targetIndicator.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_ui_arrow_down");
                }
                else
                {
                    targetIndicator.enabled = false;
                }
            }
            else if (a > b)
            {
                targetIndicator.enabled = true;
                targetIndicator.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_ui_arrow_up");
            }
            else
            {
                targetIndicator.enabled = false;
            }
        }

        public static void RefreshStarContainer(List<Image> stars, int rarity)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                if (i < rarity)
                {
                    stars[i].gameObject.SetActive(true);
                }
                else
                {
                    stars[i].gameObject.SetActive(false);
                }
            }
        }

        public static void RefreshStarContainer(List<Image> stars, List<Vector3> originalLocalPositions, int starRank, [Optional, DefaultParameterValue(false)] bool grayscale)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].transform.localPosition = originalLocalPositions[i];
                stars[i].material = !grayscale ? null : PlayerView.Binder.DisabledUiMaterial;
                stars[i].color = !grayscale ? Color.white : ConfigUi.STAR_GRAYSCALE_HIGHLIGHTED;
            }
            Vector3 vector = originalLocalPositions[1] - originalLocalPositions[0];
            switch (starRank)
            {
                case 0:
                    stars[0].enabled = false;
                    stars[1].enabled = false;
                    stars[2].enabled = false;
                    stars[3].enabled = false;
                    stars[4].enabled = false;
                    break;

                case 1:
                    stars[0].enabled = true;
                    stars[1].enabled = false;
                    stars[2].enabled = false;
                    stars[3].enabled = false;
                    stars[4].enabled = false;
                    stars[0].transform.localPosition = originalLocalPositions[2];
                    break;

                case 2:
                    stars[0].enabled = true;
                    stars[1].enabled = true;
                    stars[2].enabled = false;
                    stars[3].enabled = false;
                    stars[4].enabled = false;
                    stars[0].transform.localPosition = originalLocalPositions[2] - ((Vector3) (vector * 0.5f));
                    stars[1].transform.localPosition = originalLocalPositions[2] + ((Vector3) (vector * 0.5f));
                    break;

                case 3:
                    stars[0].enabled = true;
                    stars[1].enabled = true;
                    stars[2].enabled = true;
                    stars[3].enabled = false;
                    stars[4].enabled = false;
                    stars[0].transform.localPosition = originalLocalPositions[1];
                    stars[1].transform.localPosition = originalLocalPositions[2];
                    stars[2].transform.localPosition = originalLocalPositions[3];
                    break;

                case 4:
                    stars[0].enabled = true;
                    stars[1].enabled = true;
                    stars[2].enabled = true;
                    stars[3].enabled = true;
                    stars[4].enabled = false;
                    stars[0].transform.localPosition = originalLocalPositions[2] - (vector + ((Vector3) (vector * 0.5f)));
                    stars[1].transform.localPosition = originalLocalPositions[2] - ((Vector3) (vector * 0.5f));
                    stars[2].transform.localPosition = originalLocalPositions[2] + ((Vector3) (vector * 0.5f));
                    stars[3].transform.localPosition = originalLocalPositions[2] + (vector + ((Vector3) (vector * 0.5f)));
                    break;

                case 5:
                    stars[0].enabled = true;
                    stars[1].enabled = true;
                    stars[2].enabled = true;
                    stars[3].enabled = true;
                    stars[4].enabled = true;
                    break;

                default:
                    stars[0].enabled = false;
                    stars[1].enabled = false;
                    stars[2].enabled = false;
                    stars[3].enabled = false;
                    stars[4].enabled = false;
                    break;
            }
        }

        public static void RefreshStarContainerWithBackground(List<Image> stars, int starRank, bool highlighted)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].enabled = true;
                if (starRank <= i)
                {
                    stars[i].material = PlayerView.Binder.DisabledUiMaterial;
                    stars[i].color = !highlighted ? ConfigUi.STAR_GRAYSCALE_DEHIGHLIGHTED : ConfigUi.STAR_GRAYSCALE_HIGHLIGHTED;
                }
                else
                {
                    stars[i].material = null;
                    stars[i].color = !highlighted ? ConfigUi.COLOR_DEHIGHLIGHTED_UNLOCKED : ConfigUi.COLOR_HIGHLIGHTED;
                }
            }
        }

        public static string SecondsToStringDaysHours(long totalSeconds)
        {
            long num = ((totalSeconds / 60L) / 60L) / 0x18L;
            long num2 = ((totalSeconds - (((num * 60L) * 60L) * 0x18L)) / 60L) / 60L;
            string[] textArray1 = new string[] { num.ToString("0"), _.L(ConfigLoca.UNIT_DAYS_SHORT, null, false), " ", num2.ToString("0"), _.L(ConfigLoca.UNIT_HOURS_SHORT, null, false) };
            return string.Concat(textArray1);
        }

        public static string SecondsToStringDaysHoursMinutes(long totalSeconds, [Optional, DefaultParameterValue(false)] bool isDynamic)
        {
            long num = ((totalSeconds / 60L) / 60L) / 0x18L;
            long num2 = ((totalSeconds - (((num * 60L) * 60L) * 0x18L)) / 60L) / 60L;
            long num3 = ((totalSeconds - (((num * 60L) * 60L) * 0x18L)) - ((num2 * 60L) * 60L)) / 60L;
            string str = string.Empty;
            bool flag = !isDynamic || (num > 0L);
            bool flag2 = (!isDynamic || flag) || (num2 > 0L);
            bool flag3 = !isDynamic || !flag;
            if (flag)
            {
                str = str + (num.ToString("0") + _.L(ConfigLoca.UNIT_DAYS_SHORT, null, false) + " ");
            }
            if (flag2)
            {
                str = str + (num2.ToString("0") + _.L(ConfigLoca.UNIT_HOURS_SHORT, null, false) + " ");
            }
            if (flag3)
            {
                str = str + (num3.ToString("0") + _.L(ConfigLoca.UNIT_MINUTES_SHORT, null, false));
            }
            return str;
        }

        public static string SecondsToStringDaysHoursMinutesSeconds(long totalSeconds, [Optional, DefaultParameterValue(false)] bool isDynamic)
        {
            long num = ((totalSeconds / 60L) / 60L) / 0x18L;
            long num2 = ((totalSeconds - (((num * 60L) * 60L) * 0x18L)) / 60L) / 60L;
            long num3 = ((totalSeconds - (((num * 60L) * 60L) * 0x18L)) - ((num2 * 60L) * 60L)) / 60L;
            long num4 = totalSeconds - (num3 * 60L);
            string str = string.Empty;
            bool flag = !isDynamic || (num > 0L);
            bool flag2 = (!isDynamic || flag) || (num2 > 0L);
            bool flag3 = !isDynamic || (!flag && (num3 > 0L));
            bool flag4 = !isDynamic || !flag2;
            if (flag)
            {
                str = str + (num.ToString("0") + _.L(ConfigLoca.UNIT_DAYS_SHORT, null, false) + " ");
            }
            if (flag2)
            {
                str = str + (num2.ToString("0") + _.L(ConfigLoca.UNIT_HOURS_SHORT, null, false) + " ");
            }
            if (flag3)
            {
                str = str + (num3.ToString("0") + _.L(ConfigLoca.UNIT_MINUTES_SHORT, null, false) + " ");
            }
            if (flag4)
            {
                str = str + (num4.ToString("0") + _.L(ConfigLoca.UNIT_SECONDS_SHORT, null, false));
            }
            return str;
        }

        public static string SecondsToStringHoursMinutes(long totalSeconds)
        {
            long num = (totalSeconds / 60L) / 60L;
            long num2 = (totalSeconds - ((num * 60L) * 60L)) / 60L;
            string[] textArray1 = new string[] { num.ToString("0"), _.L(ConfigLoca.UNIT_HOURS_SHORT, null, false), " ", num2.ToString("0"), _.L(ConfigLoca.UNIT_MINUTES_SHORT, null, false) };
            return string.Concat(textArray1);
        }

        public static string SecondsToStringMinutesSeconds(long totalSeconds)
        {
            long num = (totalSeconds / 60L) / 60L;
            long num2 = (totalSeconds - ((num * 60L) * 60L)) / 60L;
            long num3 = totalSeconds - (num2 * 60L);
            return (num2.ToString("00") + ":" + num3.ToString("00"));
        }

        public static string SuperhighlightText(object text)
        {
            object[] objArray1 = new object[] { "<color=#", ConfigUi.TEXT_SUPERHIGHLIGHT_COLOR_HEX, ">", text, "</color>" };
            return string.Concat(objArray1);
        }

        public static string TicksToStringDaysHours(long ticks)
        {
            return SecondsToStringDaysHours(TimeUtil.TicksToSeconds(ticks));
        }

        public static string TicksToStringDaysHoursMinutes(long ticks)
        {
            return SecondsToStringDaysHoursMinutes(TimeUtil.TicksToSeconds(ticks), false);
        }
    }
}

