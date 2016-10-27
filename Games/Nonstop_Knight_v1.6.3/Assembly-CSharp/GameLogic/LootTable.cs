namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class LootTable
    {
        public List<LootTableItem> Items;
        private Dictionary<LootTableItem, int> m_originalWeights;
        public LootTableType Type;

        public LootTable()
        {
            this.Items = new List<LootTableItem>();
        }

        public LootTable(LootTable another)
        {
            this.Items = new List<LootTableItem>();
            this.Type = another.Type;
            for (int i = 0; i < another.Items.Count; i++)
            {
                this.Items.Add(new LootTableItem(another.Items[i]));
            }
            this.initializeOriginalWeights();
        }

        public void debugPrintContent([Optional, DefaultParameterValue(0)] int depth)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                LootTableItem item = this.Items[i];
                string str = string.Empty;
                for (int j = 0; j < depth; j++)
                {
                    str = str + "    ";
                }
                string str2 = str;
                Debug.Log(string.Concat(new object[] { str2, item.Id, ": ", item.Weight }));
                if ((item != null) && (item.SubTable != null))
                {
                    item.SubTable.debugPrintContent(++depth);
                }
            }
        }

        public LootTableItem getFirstMatchingItem(string id)
        {
            if (!this.originalWeightsInitialized())
            {
                this.initializeOriginalWeights();
            }
            for (int i = 0; i < this.Items.Count; i++)
            {
                LootTableItem item = this.Items[i];
                if (item != null)
                {
                    if (item.Id == id)
                    {
                        return item;
                    }
                    if (item.SubTable != null)
                    {
                        return item.SubTable.getFirstMatchingItem(id);
                    }
                }
            }
            return null;
        }

        public void initializeOriginalWeights()
        {
            if (this.m_originalWeights == null)
            {
                this.m_originalWeights = new Dictionary<LootTableItem, int>();
            }
            else
            {
                this.m_originalWeights.Clear();
            }
            for (int i = 0; i < this.Items.Count; i++)
            {
                LootTableItem key = this.Items[i];
                if (key != null)
                {
                    this.m_originalWeights.Add(key, key.Weight);
                    if (key.SubTable != null)
                    {
                        key.SubTable.initializeOriginalWeights();
                    }
                }
            }
        }

        private bool originalWeightsInitialized()
        {
            return (this.m_originalWeights != null);
        }

        public void resetWeightForId(string id, bool includeSubTables)
        {
            if (!this.originalWeightsInitialized())
            {
                this.initializeOriginalWeights();
            }
            for (int i = 0; i < this.Items.Count; i++)
            {
                LootTableItem item = this.Items[i];
                if (item != null)
                {
                    if (item.Id == id)
                    {
                        item.Weight = this.m_originalWeights[item];
                    }
                    if (includeSubTables && (item.SubTable != null))
                    {
                        item.SubTable.resetWeightForId(id, true);
                    }
                }
            }
        }

        public void resetWeights(bool includeSubTables)
        {
            if (!this.originalWeightsInitialized())
            {
                this.initializeOriginalWeights();
            }
            foreach (KeyValuePair<LootTableItem, int> pair in this.m_originalWeights)
            {
                pair.Key.Weight = pair.Value;
            }
            if (includeSubTables)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    LootTableItem item = this.Items[i];
                    if ((item != null) && (item.SubTable != null))
                    {
                        item.SubTable.resetWeights(true);
                    }
                }
            }
        }

        public LootTableItem roll()
        {
            if (!this.originalWeightsInitialized())
            {
                this.initializeOriginalWeights();
            }
            int num = 0;
            for (int i = 0; i < this.Items.Count; i++)
            {
                num += this.Items[i].Weight;
            }
            int num3 = UnityEngine.Random.Range(1, num + 1);
            int num4 = 0;
            LootTableItem item = null;
            for (int j = 0; j < this.Items.Count; j++)
            {
                LootTableItem item2 = this.Items[j];
                num4 += item2.Weight;
                if (num3 <= num4)
                {
                    item = item2;
                    break;
                }
            }
            if ((this.Type == LootTableType.NoReplacement) && (item != null))
            {
                item.Weight = 0;
            }
            if ((item != null) && (item.SubTable != null))
            {
                return item.SubTable.roll();
            }
            return item;
        }

        public void setAllWeightsTo(int weight, bool includeSubTables)
        {
            if (!this.originalWeightsInitialized())
            {
                this.initializeOriginalWeights();
            }
            for (int i = 0; i < this.Items.Count; i++)
            {
                LootTableItem item = this.Items[i];
                if (item != null)
                {
                    item.Weight = weight;
                    if (includeSubTables && (item.SubTable != null))
                    {
                        item.SubTable.setAllWeightsTo(weight, true);
                    }
                }
            }
        }

        public void setWeightForIds(string id, int weight, bool includeSubTables)
        {
            if (!this.originalWeightsInitialized())
            {
                this.initializeOriginalWeights();
            }
            for (int i = 0; i < this.Items.Count; i++)
            {
                LootTableItem item = this.Items[i];
                if (item != null)
                {
                    if (item.Id == id)
                    {
                        item.Weight = weight;
                    }
                    if (includeSubTables && (item.SubTable != null))
                    {
                        item.SubTable.setWeightForIds(id, weight, true);
                    }
                }
            }
        }

        public void setWeightForItem(LootTableItem lti, int weight)
        {
            if (!this.originalWeightsInitialized())
            {
                this.initializeOriginalWeights();
            }
            lti.Weight = weight;
        }
    }
}

