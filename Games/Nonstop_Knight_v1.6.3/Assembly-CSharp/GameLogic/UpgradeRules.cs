namespace GameLogic
{
    using System;
    using System.Collections.Generic;

    public class UpgradeRules : CsvResources<string, UpgradeRules.LevelEntry>
    {
        private Dictionary<int, LevelEntry> m_resources = new Dictionary<int, LevelEntry>();

        public UpgradeRules()
        {
            string[,] strArray = CsvUtils.Deserialize(ResourceUtil.LoadSafe<TextAsset>("Rules/Rules-Upgrades", false).text);
            for (int i = 0; i < strArray.GetLength(1); i++)
            {
                if (strArray[0, i] != null)
                {
                    LevelEntry entry = new LevelEntry();
                    int num2 = 0;
                    entry.Level = base.parseInt(strArray[num2++, i]);
                    entry.Coins = base.parseInt(strArray[num2++, i]);
                    entry.Tokens = base.parseInt(strArray[num2++, i]);
                    entry.Material1 = base.parseInt(strArray[num2++, i]);
                    entry.Material2 = base.parseInt(strArray[num2++, i]);
                    entry.Material3 = base.parseInt(strArray[num2++, i]);
                    this.m_resources.Add(entry.Level, entry);
                }
            }
        }

        public LevelEntry getUpgradeCostForLevel(int level)
        {
            return this.m_resources[level];
        }

        public class LevelEntry
        {
            public int Coins;
            public int Level;
            public int Material1;
            public int Material2;
            public int Material3;
            public int Tokens;
        }
    }
}

