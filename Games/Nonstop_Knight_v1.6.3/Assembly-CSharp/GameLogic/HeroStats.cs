namespace GameLogic
{
    using App;
    using PlayerView;
    using Service;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class HeroStats
    {
        public int BossesBeat;
        public int BossesBeatDuringFrenzy;
        public Dictionary<string, int> CharacterTypeKillCounts;
        public double CoinsEarned;
        public int CompletedBossTickets;
        public long CreationTimestamp;
        public double DamageDealt;
        public double DungeonBoostBoxesDestroyed;
        public int EliteBossesBeat;
        public List<GameLogic.CharacterType> EncounteredCharacterTypes;
        public List<string> EncounteredChestTypes;
        public List<string> EncounteredItemsIds;
        public int EnemiesFrozen;
        public int EnemiesPoisoned;
        public int EnemiesStunned;
        public int FirstBossSummonCount;
        public int FloorsCompleted;
        public int FrenzyActivations;
        public int GoldChestsOpened;
        public int HeroesRetired;
        public double HighestCriticalHit;
        public int HighestFloor;
        public int HighestMultikill;
        public double HighestTokenGainWithRetirement;
        public double HighestTokenMultiplier;
        public Dictionary<int, int> ItemsGainedByRarity;
        public double ItemsUnlocked;
        public double ItemUpgrades;
        public double KnightUpgrades;
        public int MinionsKilledDuringFrenzy;
        public int MissionBigPrizesOpened;
        public int MissionsStarted;
        public double MonstersKilled;
        public double MultikilledMonsters;
        public Dictionary<int, int> Multikills;
        public int RankUps;
        public long SecondsPlayedActive;
        public int SilverChestsOpened;
        public Dictionary<string, int> SkillActivationCounts;
        public Dictionary<string, double> SkillDamageCounts;
        public Dictionary<string, double> SkillDungeonBoostBoxDestructionCounts;
        public Dictionary<string, int> SkillMinionKills;
        public static List<string> STAT_HEADERS;
        public double TokensEarned;
        public int UsedBossTickets;

        static HeroStats()
        {
            List<string> list = new List<string>();
            list.Add(ConfigLoca.HEROSTATS_HIGHEST_FLOOR);
            list.Add(ConfigLoca.HEROSTATS_FLOORS_COMPLETED);
            list.Add(ConfigLoca.HEROSTATS_TIME_PLAYED);
            list.Add(ConfigLoca.HEROSTATS_TIME_SINCE_INSTALL);
            list.Add(ConfigLoca.HEROSTATS_DAMAGE_DEALT);
            list.Add(ConfigLoca.HEROSTATS_ENEMIES_KILLED);
            list.Add(ConfigLoca.HEROSTATS_HIGHEST_MULTIKILL);
            list.Add(ConfigLoca.HEROSTATS_HIGHEST_CRITICAL_HIT);
            list.Add(ConfigLoca.HEROSTATS_MOST_KILLED_ENEMY);
            list.Add(ConfigLoca.HEROSTATS_MOST_USED_SKILL);
            list.Add(ConfigLoca.HEROSTATS_MOST_DEADLY_SKILL);
            list.Add(ConfigLoca.HEROSTATS_COINS_EARNED);
            list.Add(ConfigLoca.HEROSTATS_ITEMS_UNLOCKED);
            list.Add(ConfigLoca.HEROSTATS_ITEMS_UPGRADED);
            list.Add(ConfigLoca.HEROSTATS_KNIGHT_UPGRADED);
            list.Add(ConfigLoca.HEROSTATS_CHESTS_OPENED);
            list.Add(ConfigLoca.HEROSTATS_TIMES_ASCENDED);
            list.Add(ConfigLoca.HEROSTATS_TOKENS_EARNED);
            list.Add(ConfigLoca.HEROSTATS_HIGHEST_TOKEN_MULTIPLIER);
            list.Add(ConfigLoca.HEROSTATS_DUNGEON_BOOST_BOXES_DESTROYED);
            STAT_HEADERS = list;
        }

        public HeroStats()
        {
            this.EncounteredChestTypes = new List<string>();
            this.EncounteredItemsIds = new List<string>();
            this.EncounteredCharacterTypes = new List<GameLogic.CharacterType>();
            this.SkillActivationCounts = new Dictionary<string, int>();
            this.SkillDamageCounts = new Dictionary<string, double>();
            this.CharacterTypeKillCounts = new Dictionary<string, int>();
            this.SkillMinionKills = new Dictionary<string, int>();
            this.Multikills = new Dictionary<int, int>();
            this.ItemsGainedByRarity = new Dictionary<int, int>();
            this.SkillDungeonBoostBoxDestructionCounts = new Dictionary<string, double>();
            this.CreationTimestamp = Service.Binder.ServerTime.GameTime;
        }

        public HeroStats(HeroStats another)
        {
            this.EncounteredChestTypes = new List<string>();
            this.EncounteredItemsIds = new List<string>();
            this.EncounteredCharacterTypes = new List<GameLogic.CharacterType>();
            this.SkillActivationCounts = new Dictionary<string, int>();
            this.SkillDamageCounts = new Dictionary<string, double>();
            this.CharacterTypeKillCounts = new Dictionary<string, int>();
            this.SkillMinionKills = new Dictionary<string, int>();
            this.Multikills = new Dictionary<int, int>();
            this.ItemsGainedByRarity = new Dictionary<int, int>();
            this.SkillDungeonBoostBoxDestructionCounts = new Dictionary<string, double>();
            this.copyFrom(another);
        }

        public void add(HeroStats another)
        {
            string str;
            int num4;
            double num5;
            this.FloorsCompleted += another.FloorsCompleted;
            this.SecondsPlayedActive += another.SecondsPlayedActive;
            this.MonstersKilled += another.MonstersKilled;
            this.MultikilledMonsters += another.MultikilledMonsters;
            this.CoinsEarned += another.CoinsEarned;
            this.TokensEarned += another.TokensEarned;
            this.ItemsUnlocked += another.ItemsUnlocked;
            this.ItemUpgrades += another.ItemUpgrades;
            this.KnightUpgrades += another.KnightUpgrades;
            this.GoldChestsOpened += another.GoldChestsOpened;
            this.SilverChestsOpened += another.SilverChestsOpened;
            this.HeroesRetired += another.HeroesRetired;
            this.HighestFloor = Mathf.Max(this.HighestFloor, another.HighestFloor);
            this.RankUps += another.RankUps;
            for (int i = 0; i < another.EncounteredChestTypes.Count; i++)
            {
                if (!this.EncounteredChestTypes.Contains(another.EncounteredChestTypes[i]))
                {
                    this.EncounteredChestTypes.Add(another.EncounteredChestTypes[i]);
                }
            }
            for (int j = 0; j < another.EncounteredItemsIds.Count; j++)
            {
                if (!this.EncounteredItemsIds.Contains(another.EncounteredItemsIds[j]))
                {
                    this.EncounteredItemsIds.Add(another.EncounteredItemsIds[j]);
                }
            }
            for (int k = 0; k < another.EncounteredCharacterTypes.Count; k++)
            {
                if (!this.EncounteredCharacterTypes.Contains(another.EncounteredCharacterTypes[k]))
                {
                    this.EncounteredCharacterTypes.Add(another.EncounteredCharacterTypes[k]);
                }
            }
            this.HighestTokenMultiplier = Math.Max(this.HighestTokenMultiplier, another.HighestTokenMultiplier);
            this.FirstBossSummonCount = Mathf.Max(this.FirstBossSummonCount, another.FirstBossSummonCount);
            this.HighestTokenGainWithRetirement = Math.Max(this.HighestTokenGainWithRetirement, another.HighestTokenGainWithRetirement);
            this.HighestMultikill = Math.Max(this.HighestMultikill, another.HighestMultikill);
            this.HighestCriticalHit = Math.Max(this.HighestCriticalHit, another.HighestCriticalHit);
            foreach (KeyValuePair<string, int> pair in another.SkillActivationCounts)
            {
                if (this.SkillActivationCounts.ContainsKey(pair.Key))
                {
                    Dictionary<string, int> dictionary;
                    num4 = dictionary[str];
                    (dictionary = this.SkillActivationCounts)[str = pair.Key] = num4 + pair.Value;
                }
                else
                {
                    this.SkillActivationCounts.Add(pair.Key, pair.Value);
                }
            }
            foreach (KeyValuePair<string, double> pair2 in another.SkillDamageCounts)
            {
                if (this.SkillDamageCounts.ContainsKey(pair2.Key))
                {
                    Dictionary<string, double> dictionary2;
                    num5 = dictionary2[str];
                    (dictionary2 = this.SkillDamageCounts)[str = pair2.Key] = num5 + pair2.Value;
                }
                else
                {
                    this.SkillDamageCounts.Add(pair2.Key, pair2.Value);
                }
            }
            foreach (KeyValuePair<string, int> pair3 in another.CharacterTypeKillCounts)
            {
                if (this.CharacterTypeKillCounts.ContainsKey(pair3.Key))
                {
                    Dictionary<string, int> dictionary3;
                    num4 = dictionary3[str];
                    (dictionary3 = this.CharacterTypeKillCounts)[str = pair3.Key] = num4 + pair3.Value;
                }
                else
                {
                    this.CharacterTypeKillCounts.Add(pair3.Key, pair3.Value);
                }
            }
            this.DamageDealt += another.DamageDealt;
            this.BossesBeat += another.BossesBeat;
            this.BossesBeatDuringFrenzy += another.BossesBeatDuringFrenzy;
            this.MinionsKilledDuringFrenzy += another.MinionsKilledDuringFrenzy;
            foreach (KeyValuePair<string, int> pair4 in another.SkillMinionKills)
            {
                if (this.SkillMinionKills.ContainsKey(pair4.Key))
                {
                    Dictionary<string, int> dictionary4;
                    num4 = dictionary4[str];
                    (dictionary4 = this.SkillMinionKills)[str = pair4.Key] = num4 + pair4.Value;
                }
                else
                {
                    this.SkillMinionKills.Add(pair4.Key, pair4.Value);
                }
            }
            this.EnemiesFrozen += another.EnemiesFrozen;
            this.EnemiesPoisoned += another.EnemiesPoisoned;
            this.EnemiesStunned += another.EnemiesStunned;
            foreach (KeyValuePair<int, int> pair5 in another.Multikills)
            {
                if (this.Multikills.ContainsKey(pair5.Key))
                {
                    Dictionary<int, int> dictionary5;
                    num4 = dictionary5[num4];
                    (dictionary5 = this.Multikills)[num4 = pair5.Key] = num4 + pair5.Value;
                }
                else
                {
                    this.Multikills.Add(pair5.Key, pair5.Value);
                }
            }
            foreach (KeyValuePair<int, int> pair6 in another.ItemsGainedByRarity)
            {
                if (this.ItemsGainedByRarity.ContainsKey(pair6.Key))
                {
                    Dictionary<int, int> dictionary6;
                    num4 = dictionary6[num4];
                    (dictionary6 = this.ItemsGainedByRarity)[num4 = pair6.Key] = num4 + pair6.Value;
                }
                else
                {
                    this.ItemsGainedByRarity.Add(pair6.Key, pair6.Value);
                }
            }
            this.UsedBossTickets += another.UsedBossTickets;
            this.CompletedBossTickets += another.CompletedBossTickets;
            this.MissionsStarted += another.MissionsStarted;
            this.MissionBigPrizesOpened += another.MissionBigPrizesOpened;
            this.EliteBossesBeat += another.EliteBossesBeat;
            this.FrenzyActivations += another.FrenzyActivations;
            this.DungeonBoostBoxesDestroyed += another.DungeonBoostBoxesDestroyed;
            foreach (KeyValuePair<string, double> pair7 in another.SkillDungeonBoostBoxDestructionCounts)
            {
                if (this.SkillDungeonBoostBoxDestructionCounts.ContainsKey(pair7.Key))
                {
                    Dictionary<string, double> dictionary7;
                    num5 = dictionary7[str];
                    (dictionary7 = this.SkillDungeonBoostBoxDestructionCounts)[str = pair7.Key] = num5 + pair7.Value;
                }
                else
                {
                    this.SkillDungeonBoostBoxDestructionCounts.Add(pair7.Key, pair7.Value);
                }
            }
        }

        public void copyFrom(HeroStats another)
        {
            this.FloorsCompleted = another.FloorsCompleted;
            this.CreationTimestamp = another.CreationTimestamp;
            this.SecondsPlayedActive = another.SecondsPlayedActive;
            this.MonstersKilled = another.MonstersKilled;
            this.MultikilledMonsters = another.MultikilledMonsters;
            this.CoinsEarned = another.CoinsEarned;
            this.TokensEarned = another.TokensEarned;
            this.ItemsUnlocked = another.ItemsUnlocked;
            this.ItemUpgrades = another.ItemUpgrades;
            this.KnightUpgrades = another.KnightUpgrades;
            this.GoldChestsOpened = another.GoldChestsOpened;
            this.SilverChestsOpened = another.SilverChestsOpened;
            this.HeroesRetired = another.HeroesRetired;
            this.HighestFloor = another.HighestFloor;
            this.RankUps = another.RankUps;
            this.EncounteredChestTypes = new List<string>(another.EncounteredChestTypes);
            this.EncounteredItemsIds = new List<string>(another.EncounteredItemsIds);
            this.EncounteredCharacterTypes = new List<GameLogic.CharacterType>(another.EncounteredCharacterTypes);
            this.HighestTokenMultiplier = another.HighestTokenMultiplier;
            this.FirstBossSummonCount = another.FirstBossSummonCount;
            this.HighestTokenGainWithRetirement = another.HighestTokenGainWithRetirement;
            this.HighestMultikill = another.HighestMultikill;
            this.HighestCriticalHit = another.HighestCriticalHit;
            this.SkillActivationCounts = new Dictionary<string, int>(another.SkillActivationCounts);
            this.SkillDamageCounts = new Dictionary<string, double>(another.SkillDamageCounts);
            this.CharacterTypeKillCounts = new Dictionary<string, int>(another.CharacterTypeKillCounts);
            this.DamageDealt = another.DamageDealt;
            this.BossesBeat = another.BossesBeat;
            this.BossesBeatDuringFrenzy = another.BossesBeatDuringFrenzy;
            this.MinionsKilledDuringFrenzy = another.MinionsKilledDuringFrenzy;
            this.SkillMinionKills = new Dictionary<string, int>(another.SkillMinionKills);
            this.EnemiesFrozen = another.EnemiesFrozen;
            this.EnemiesPoisoned = another.EnemiesPoisoned;
            this.EnemiesStunned = another.EnemiesStunned;
            this.Multikills = new Dictionary<int, int>(another.Multikills);
            this.UsedBossTickets = another.UsedBossTickets;
            this.CompletedBossTickets = another.CompletedBossTickets;
            this.MissionsStarted = another.MissionsStarted;
            this.MissionBigPrizesOpened = another.MissionBigPrizesOpened;
            this.ItemsGainedByRarity = new Dictionary<int, int>(another.ItemsGainedByRarity);
            this.EliteBossesBeat = another.EliteBossesBeat;
            this.FrenzyActivations = another.FrenzyActivations;
            this.DungeonBoostBoxesDestroyed = another.DungeonBoostBoxesDestroyed;
            this.SkillDungeonBoostBoxDestructionCounts = new Dictionary<string, double>(another.SkillDungeonBoostBoxDestructionCounts);
        }

        private string getActiveTimePlayedString()
        {
            long num = (this.SecondsPlayedActive / 60L) / 60L;
            long num2 = (this.SecondsPlayedActive - ((num * 60L) * 60L)) / 60L;
            string[] textArray1 = new string[] { num.ToString("0"), _.L(ConfigLoca.UNIT_HOURS_SHORT, null, false), " ", num2.ToString("0"), _.L(ConfigLoca.UNIT_MINUTES_SHORT, null, false) };
            return string.Concat(textArray1);
        }

        public SkillType getMostDeadlySkill()
        {
            try
            {
                if (this.SkillDamageCounts.Count > 0)
                {
                    return (SkillType) ((int) Enum.Parse(typeof(SkillType), LangUtil.GetKeyWithHighestDoubleValueFromDictionary<string>(this.SkillDamageCounts)));
                }
            }
            catch (Exception)
            {
                Debug.LogError("Unidentified key in HeroStats.SkillDamageCounts");
            }
            return SkillType.NONE;
        }

        public GameLogic.CharacterType getMostKilledEnemy()
        {
            try
            {
                if (this.CharacterTypeKillCounts.Count > 0)
                {
                    return (GameLogic.CharacterType) ((int) Enum.Parse(typeof(GameLogic.CharacterType), LangUtil.GetKeyWithHighestIntValueFromDictionary<string>(this.CharacterTypeKillCounts)));
                }
            }
            catch (Exception)
            {
                Debug.LogError("Unidentified key in HeroStats.CharacterTypeKillCounts");
            }
            return GameLogic.CharacterType.UNSPECIFIED;
        }

        public SkillType getMostUsedSkill()
        {
            try
            {
                if (this.SkillActivationCounts.Count > 0)
                {
                    return (SkillType) ((int) Enum.Parse(typeof(SkillType), LangUtil.GetKeyWithHighestIntValueFromDictionary<string>(this.SkillActivationCounts)));
                }
            }
            catch (Exception)
            {
                Debug.LogError("Unidentified key in HeroStats.SkillActivationCounts");
            }
            return SkillType.NONE;
        }

        private string getTotalTimePlayedString()
        {
            long num2 = Service.Binder.ServerTime.GameTime - this.CreationTimestamp;
            long num3 = (num2 / 60L) / 60L;
            long num4 = (num2 - ((num3 * 60L) * 60L)) / 60L;
            string[] textArray1 = new string[] { num3.ToString("0"), _.L(ConfigLoca.UNIT_HOURS_SHORT, null, false), " ", num4.ToString("0"), _.L(ConfigLoca.UNIT_MINUTES_SHORT, null, false) };
            return string.Concat(textArray1);
        }

        public string toRichTextFormattedString(string title, bool alltime)
        {
            List<string> list = this.toRichTextFormattedStringList(title, alltime);
            string str = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                str = str + list[i];
                if (i < (list.Count - 1))
                {
                    str = str + "\n";
                }
            }
            return str;
        }

        public List<string> toRichTextFormattedStringList(string title, bool alltime)
        {
            GameLogic.CharacterType key = this.getMostKilledEnemy();
            SkillType nONE = this.getMostUsedSkill();
            SkillType type3 = this.getMostDeadlySkill();
            if ((key != GameLogic.CharacterType.UNSPECIFIED) && !ConfigUi.CHARACTER_TYPE_NAMES.ContainsKey(key))
            {
                Debug.LogWarning("Character type name not found for " + key);
                key = GameLogic.CharacterType.UNSPECIFIED;
            }
            if ((nONE != SkillType.NONE) && !ConfigSkills.SHARED_DATA.ContainsKey(nONE))
            {
                Debug.LogWarning("Skill name not found for " + nONE);
                nONE = SkillType.NONE;
            }
            if ((type3 != SkillType.NONE) && !ConfigSkills.SHARED_DATA.ContainsKey(type3))
            {
                Debug.LogWarning("Skill name not found for " + type3);
                type3 = SkillType.NONE;
            }
            List<string> list2 = new List<string>();
            list2.Add(this.HighestFloor.ToString());
            list2.Add(this.FloorsCompleted.ToString());
            list2.Add(this.getActiveTimePlayedString());
            list2.Add(this.getTotalTimePlayedString());
            list2.Add(MenuHelpers.BigValueToString(this.DamageDealt));
            list2.Add(MenuHelpers.BigValueToString(this.MonstersKilled));
            list2.Add(this.HighestMultikill.ToString());
            list2.Add(MenuHelpers.BigValueToString(this.HighestCriticalHit));
            list2.Add((key == GameLogic.CharacterType.UNSPECIFIED) ? "-" : _.L(ConfigUi.CHARACTER_TYPE_NAMES[key], null, false));
            list2.Add((nONE == SkillType.NONE) ? "-" : _.L(ConfigSkills.SHARED_DATA[nONE].Name, null, false));
            list2.Add((type3 == SkillType.NONE) ? "-" : _.L(ConfigSkills.SHARED_DATA[type3].Name, null, false));
            list2.Add(MenuHelpers.BigValueToString(this.CoinsEarned));
            list2.Add(MenuHelpers.BigValueToString(this.ItemsUnlocked));
            list2.Add(MenuHelpers.BigValueToString(this.ItemUpgrades));
            list2.Add(MenuHelpers.BigValueToString(this.KnightUpgrades));
            list2.Add((this.GoldChestsOpened + this.SilverChestsOpened).ToString());
            list2.Add(!alltime ? "-" : this.HeroesRetired.ToString());
            list2.Add(MenuHelpers.BigValueToString(this.TokensEarned));
            list2.Add("x" + MathUtil.RoundToNumDecimals((float) this.HighestTokenMultiplier, 2).ToString());
            list2.Add(MenuHelpers.BigValueToString(this.DungeonBoostBoxesDestroyed));
            return list2;
        }

        public override string ToString()
        {
            return this.toRichTextFormattedString("TITLE", true);
        }
    }
}

