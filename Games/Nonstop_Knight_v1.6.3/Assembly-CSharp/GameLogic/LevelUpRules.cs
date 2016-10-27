namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class LevelUpRules : CsvResources<int, LevelUpRules.LevelEntry>
    {
        private List<double> m_levelXpRequirements = new List<double>();

        public LevelUpRules()
        {
            string[,] strArray = CsvUtils.Deserialize(ResourceUtil.LoadSafe<TextAsset>("Rules/Rules-LevelUp", false).text);
            this.m_levelXpRequirements.Add(0.0);
            for (int i = 0; i < strArray.GetLength(1); i++)
            {
                if (strArray[0, i] != null)
                {
                    LevelEntry entry = new LevelEntry();
                    int num2 = 0;
                    entry.Level = base.parseInt(strArray[num2++, i]);
                    entry.XpReq = base.parseDouble(strArray[num2++, i]);
                    entry.MaxEnergy = base.parseInt(strArray[num2++, i]);
                    base.m_resources.Add(entry.Level, entry);
                    this.m_levelXpRequirements.Add(entry.XpReq);
                }
            }
        }

        public int getLevelForXp(double xp)
        {
            int num = -1;
            for (int i = 1; i < this.m_levelXpRequirements.Count; i++)
            {
                double num3 = this.m_levelXpRequirements[i];
                if (xp < num3)
                {
                    num = i - 1;
                    break;
                }
            }
            if (num == -1)
            {
                num = this.m_levelXpRequirements.Count - 1;
            }
            return num;
        }

        public int getMaxEnergyForLevel(int level)
        {
            return base.m_resources[level].MaxEnergy;
        }

        public double getNeededXpForTargetLevel(int fromLevel, int targetLevel)
        {
            double num = this.m_levelXpRequirements[fromLevel];
            double num2 = num;
            if (fromLevel < (this.m_levelXpRequirements.Count - 1))
            {
                num2 = this.m_levelXpRequirements[targetLevel];
            }
            return (num2 - num);
        }

        public float getNormalizedProgressTowardsNextLevel(int currentLevel, double currentXp)
        {
            double num = this.m_levelXpRequirements[currentLevel];
            double num2 = num;
            if (currentLevel < (this.m_levelXpRequirements.Count - 1))
            {
                num2 = this.m_levelXpRequirements[currentLevel + 1];
            }
            double num3 = num2 - num;
            double num4 = currentXp - num;
            return Mathf.Clamp01(((float) num4) / ((float) num3));
        }

        public double getProgressTowardsNextLevel(int currentLevel, double currentXp)
        {
            double num = this.m_levelXpRequirements[currentLevel];
            return (currentXp - num);
        }

        public double getXpRequirementForLevel(int level)
        {
            if (level < this.m_levelXpRequirements.Count)
            {
                return this.m_levelXpRequirements[level];
            }
            return 0.0;
        }

        public class LevelEntry
        {
            public int Level;
            public int MaxEnergy;
            public double XpReq;
        }
    }
}

