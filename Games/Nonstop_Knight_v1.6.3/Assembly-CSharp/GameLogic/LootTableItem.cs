namespace GameLogic
{
    using System;

    public class LootTableItem
    {
        public string Id;
        public LootTable SubTable;
        public int Weight;

        public LootTableItem()
        {
            this.Id = string.Empty;
        }

        public LootTableItem(LootTableItem another)
        {
            this.Id = string.Empty;
            this.Id = another.Id;
            this.Weight = another.Weight;
            if (another.SubTable != null)
            {
                this.SubTable = new LootTable(another.SubTable);
            }
        }
    }
}

