namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;

    public class Inventory : IJsonData
    {
        public int BossPotions;
        public int FrenzyPotions;
        public List<ItemInstance> ItemInstances;
        [JsonIgnore]
        public Dictionary<string, int> ItemShards;
        public int RevivePotions;
        public int XpPotions;

        public Inventory()
        {
            this.ItemInstances = new List<ItemInstance>();
            this.ItemShards = new Dictionary<string, int>();
        }

        public Inventory(Inventory another)
        {
            this.ItemInstances = new List<ItemInstance>();
            this.ItemShards = new Dictionary<string, int>();
            this.copyFrom(another);
        }

        public void copyFrom(Inventory another)
        {
            this.ItemInstances.Clear();
            for (int i = 0; i < another.ItemInstances.Count; i++)
            {
                this.ItemInstances.Add(new ItemInstance(another.ItemInstances[i]));
            }
            this.ItemShards = new Dictionary<string, int>(another.ItemShards);
            this.RevivePotions = another.RevivePotions;
            this.FrenzyPotions = another.FrenzyPotions;
            this.XpPotions = another.XpPotions;
            this.BossPotions = another.BossPotions;
        }

        public void postDeserializeInitialization()
        {
            for (int i = this.ItemInstances.Count - 1; i >= 0; i--)
            {
                if ((this.ItemInstances[i] == null) || string.IsNullOrEmpty(this.ItemInstances[i].ItemId))
                {
                    this.ItemInstances.RemoveAt(i);
                }
            }
            for (int j = 0; j < this.ItemInstances.Count; j++)
            {
                this.ItemInstances[j].postDeserializeInitialization();
            }
        }
    }
}

