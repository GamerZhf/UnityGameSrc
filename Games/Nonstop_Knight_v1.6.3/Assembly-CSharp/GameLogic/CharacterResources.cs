namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class CharacterResources : CsvResources<string, Character>
    {
        private Dictionary<GameLogic.CharacterType, List<string>> m_meleeCharacterIds = new Dictionary<GameLogic.CharacterType, List<string>>(new CharacterTypeBoxAvoidanceComparer());
        private Dictionary<GameLogic.CharacterType, List<string>> m_rangedCharacterIds = new Dictionary<GameLogic.CharacterType, List<string>>(new CharacterTypeBoxAvoidanceComparer());

        public CharacterResources()
        {
            this.loadEnemyCharacterCsv("Rules/Characters-Enemies");
            this.loadPetCharacterCsv("Rules/Characters-Pets");
            foreach (UnityEngine.Object obj2 in ResourceUtil.LoadResourcesAtPath("Characters"))
            {
                if (obj2 is TextAsset)
                {
                    this.loadCharacterJson(((TextAsset) obj2).text);
                }
            }
        }

        public string getRandomCharacterId(DamageType damageType, GameLogic.CharacterType characterType, DungeonThemeType fallbackDungeonThemeType)
        {
            Dictionary<GameLogic.CharacterType, List<string>> dictionary = (damageType != DamageType.Melee) ? this.m_rangedCharacterIds : this.m_meleeCharacterIds;
            List<string> list = null;
            if (dictionary.ContainsKey(characterType))
            {
                list = dictionary[characterType];
            }
            else
            {
                List<GameLogic.CharacterType> list2 = GameLogic.Binder.DungeonResources.getMinionCharacterTypes(fallbackDungeonThemeType);
                for (int i = 0; i < list2.Count; i++)
                {
                    GameLogic.CharacterType key = list2[i];
                    if (dictionary.ContainsKey(key))
                    {
                        list = dictionary[key];
                        break;
                    }
                }
            }
            return ((list != null) ? LangUtil.GetRandomValueFromList<string>(list) : null);
        }

        public bool hasCharacter(string characterId)
        {
            return base.m_resources.ContainsKey(characterId);
        }

        public bool hasDamageTypeCharacterIds(GameLogic.CharacterType characterType, DamageType damageType)
        {
            Dictionary<GameLogic.CharacterType, List<string>> dictionary = (damageType != DamageType.Melee) ? this.m_rangedCharacterIds : this.m_meleeCharacterIds;
            return dictionary.ContainsKey(characterType);
        }

        private void loadCharacterJson(string json)
        {
            Character e = JsonUtils.Deserialize<Character>(json, true);
            e.Name = _.L(e.Name, null, false);
            this.postProcess(e);
        }

        private void loadEnemyCharacterCsv(string csvFilePath)
        {
            string[,] strArray = CsvUtils.Deserialize(ResourceUtil.LoadSafe<TextAsset>(csvFilePath, false).text);
            for (int i = 0; i < strArray.GetLength(1); i++)
            {
                if (strArray[0, i] != null)
                {
                    Character e = new Character();
                    int num2 = 0;
                    e.Id = strArray[num2++, i];
                    e.Name = _.L(strArray[num2++, i], null, false);
                    num2++;
                    e.Type = base.parseEnumType<GameLogic.CharacterType>(strArray[num2++, i]);
                    e.Prefab = base.parseEnumType<CharacterPrefab>(strArray[num2++, i]);
                    e.Rarity = base.parseInt(strArray[num2++, i]);
                    e.CoreAiBehaviour = base.parseEnumType<AiBehaviourType>(strArray[num2++, i]);
                    e.BossAiBehaviour = base.parseEnumType<AiBehaviourType>(strArray[num2++, i]);
                    e.BossAiParameters = new string[] { strArray[num2++, i], strArray[num2++, i], strArray[num2++, i] };
                    e.BossPerk = base.parseEnumType<PerkType>(strArray[num2++, i]);
                    e.RangedProjectileType = base.parseEnumType<ProjectileType>(strArray[num2++, i]);
                    e.AttackContactTimeNormalized = base.parseFloat(strArray[num2++, i]);
                    e.BaseStats = new Dictionary<string, double>();
                    e.BaseStats.Add(BaseStatProperty.Life.ToString(), base.parseDouble(strArray[num2++, i]));
                    e.BaseStats.Add(BaseStatProperty.AttacksPerSecond.ToString(), base.parseDouble(strArray[num2++, i]));
                    e.BaseStats.Add(BaseStatProperty.AttackRange.ToString(), base.parseDouble(strArray[num2++, i]));
                    e.BaseStats.Add(BaseStatProperty.DamagePerHit.ToString(), base.parseDouble(strArray[num2++, i]));
                    e.BaseStats.Add(BaseStatProperty.CriticalHitChancePct.ToString(), base.parseDouble(strArray[num2++, i]));
                    e.BaseStats.Add(BaseStatProperty.CriticalHitMultiplier.ToString(), base.parseDouble(strArray[num2++, i]));
                    e.BaseStats.Add(BaseStatProperty.CleaveDamagePct.ToString(), base.parseDouble(strArray[num2++, i]));
                    e.BaseStats.Add(BaseStatProperty.CleaveRange.ToString(), base.parseDouble(strArray[num2++, i]));
                    e.BaseStats.Add(BaseStatProperty.MovementSpeed.ToString(), base.parseDouble(strArray[num2++, i]));
                    e.BaseStats.Add(BaseStatProperty.Threat.ToString(), base.parseDouble(strArray[num2++, i]));
                    e.BaseStats.Add(BaseStatProperty.UniversalArmorBonus.ToString(), 0.0);
                    e.BaseStats.Add(BaseStatProperty.SkillDamage.ToString(), 0.0);
                    e.BaseStats.Add(BaseStatProperty.UniversalXpBonus.ToString(), 0.0);
                    this.postProcess(e);
                }
            }
        }

        private void loadPetCharacterCsv(string csvFilePath)
        {
            string[,] strArray = CsvUtils.Deserialize(ResourceUtil.LoadSafe<TextAsset>(csvFilePath, false).text);
            for (int i = 0; i < strArray.GetLength(1); i++)
            {
                if (strArray[0, i] == null)
                {
                    continue;
                }
                Character e = new Character();
                int num2 = 0;
                e.Id = strArray[num2++, i];
                e.Name = _.L(strArray[num2++, i], null, false);
                num2++;
                e.FlavorText = _.L(strArray[num2++, i], null, false);
                e.Type = GameLogic.CharacterType.Pet;
                e.Prefab = base.parseEnumType<CharacterPrefab>(strArray[num2++, i]);
                e.Rarity = base.parseInt(strArray[num2++, i]);
                e.AvatarSpriteId = strArray[num2++, i];
                e.CoreAiBehaviour = base.parseEnumType<AiBehaviourType>(strArray[num2++, i]);
                KeyValuePair<string, float> pair = base.parseStringFloatPair(strArray[num2++, i]);
                KeyValuePair<string, float> pair2 = base.parseStringFloatPair(strArray[num2++, i]);
                KeyValuePair<string, float> pair3 = base.parseStringFloatPair(strArray[num2++, i]);
                KeyValuePair<string, float> pair4 = base.parseStringFloatPair(strArray[num2++, i]);
                KeyValuePair<string, float>[] pairArray = new KeyValuePair<string, float>[] { pair, pair2, pair3, pair4 };
                e.FixedPerks = new GatedPerkContainer();
                for (int j = 0; j < pairArray.Length; j++)
                {
                    if (string.IsNullOrEmpty(pairArray[j].Key) || (j >= ConfigGameplay.PET_PERK_MILESTONE_LEVELS.Count))
                    {
                        break;
                    }
                    int num4 = ConfigGameplay.PET_PERK_MILESTONE_LEVELS[j];
                    PerkType perkType = base.parseEnumType<PerkType>(pairArray[j].Key);
                    ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[perkType];
                    float bestModifier = (data.LinkedToRunestone == null) ? pairArray[j].Value : ConfigRunestones.GetRunestoneData(data.LinkedToRunestone).PerkInstance.Modifier;
                    if (bestModifier == 0f)
                    {
                        bestModifier = ConfigPerks.GetBestModifier(perkType);
                    }
                    GatedPerkContainer.Entry item = new GatedPerkContainer.Entry();
                    item.RankReq = num4;
                    PerkInstance instance = new PerkInstance();
                    instance.Type = perkType;
                    instance.Modifier = bestModifier;
                    item.PerkInstance = instance;
                    e.FixedPerks.Entries.Add(item);
                }
                e.RangedProjectileType = base.parseEnumType<ProjectileType>(strArray[num2++, i]);
                e.MainHeroDamagePerHitPct = base.parseFloat(strArray[num2++, i]);
                e.AttackContactTimeNormalized = base.parseFloat(strArray[num2++, i]);
                e.BaseStats = new Dictionary<string, double>();
                e.BaseStats.Add(BaseStatProperty.AttacksPerSecond.ToString(), base.parseDouble(strArray[num2++, i]));
                e.BaseStats.Add(BaseStatProperty.AttackRange.ToString(), base.parseDouble(strArray[num2++, i]));
                e.BaseStats.Add(BaseStatProperty.CriticalHitChancePct.ToString(), base.parseDouble(strArray[num2++, i]));
                e.BaseStats.Add(BaseStatProperty.CriticalHitMultiplier.ToString(), base.parseDouble(strArray[num2++, i]));
                e.BaseStats.Add(BaseStatProperty.CleaveDamagePct.ToString(), base.parseDouble(strArray[num2++, i]));
                e.BaseStats.Add(BaseStatProperty.CleaveRange.ToString(), base.parseDouble(strArray[num2++, i]));
                e.BaseStats.Add(BaseStatProperty.MovementSpeed.ToString(), base.parseDouble(strArray[num2++, i]));
                e.BaseStats.Add(BaseStatProperty.Threat.ToString(), base.parseDouble(strArray[num2++, i]));
                e.BaseStats.Add(BaseStatProperty.Life.ToString(), 999.0);
                e.BaseStats.Add(BaseStatProperty.DamagePerHit.ToString(), 0.0);
                e.BaseStats.Add(BaseStatProperty.UniversalArmorBonus.ToString(), 0.0);
                e.BaseStats.Add(BaseStatProperty.SkillDamage.ToString(), 0.0);
                e.BaseStats.Add(BaseStatProperty.UniversalXpBonus.ToString(), 0.0);
                this.postProcess(e);
            }
        }

        private void postProcess(Character e)
        {
            e.Radius = 0.3f;
            e.postDeserializeInitialization();
            base.addResource(e.Id, e);
            Dictionary<GameLogic.CharacterType, List<string>> dictionary = (e.RangedProjectileType == ProjectileType.NONE) ? this.m_meleeCharacterIds : this.m_rangedCharacterIds;
            if (!dictionary.ContainsKey(e.Type))
            {
                dictionary.Add(e.Type, new List<string>());
            }
            dictionary[e.Type].Add(e.Id);
        }
    }
}

