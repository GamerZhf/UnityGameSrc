namespace GameLogic
{
    using System;
    using System.Collections.Generic;

    public class PlayerSkillUpgradeResources : CsvResources<string, PlayerSkillUpgrade>
    {
        private List<PlayerSkillUpgrade> m_orderedList = new List<PlayerSkillUpgrade>();
        private List<PerkType> m_usedPerkTypesList = new List<PerkType>();

        public PlayerSkillUpgradeResources()
        {
            this.loadCsv("PlayerSkillUpgrades");
        }

        public List<PlayerSkillUpgrade> getOrderedList()
        {
            return this.m_orderedList;
        }

        public List<PerkType> getUsedPerkTypes()
        {
            return this.m_usedPerkTypesList;
        }

        private void loadCsv(string csvFilePath)
        {
            string[,] strArray = CsvUtils.Deserialize(ResourceUtil.LoadSafe<TextAsset>(csvFilePath, false).text);
            for (int i = 0; i < strArray.GetLength(1); i++)
            {
                if (strArray[0, i] != null)
                {
                    PlayerSkillUpgrade res = new PlayerSkillUpgrade();
                    int num2 = 0;
                    res.Id = strArray[num2++, i];
                    res.Price = base.parseDouble(strArray[num2++, i]);
                    PerkType item = base.parseEnumType<PerkType>(strArray[num2++, i]);
                    float num3 = base.parseFloat(strArray[num2++, i]);
                    if (item != PerkType.NONE)
                    {
                        PerkInstance instance = new PerkInstance();
                        instance.Type = item;
                        instance.Modifier = num3;
                        res.PerkInstance = instance;
                        if (!this.m_usedPerkTypesList.Contains(item))
                        {
                            this.m_usedPerkTypesList.Add(item);
                        }
                    }
                    base.addResource(res.Id, res);
                    this.m_orderedList.Add(res);
                }
            }
        }
    }
}

