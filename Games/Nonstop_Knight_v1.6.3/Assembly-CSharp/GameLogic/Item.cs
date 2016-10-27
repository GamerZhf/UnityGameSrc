namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;

    public class Item : IJsonData, IBuffIconProvider, IComparable<Item>
    {
        public OrderedDict<AccessoryType, string> Accessories;
        public List<string> BodyMaterials;
        public string Description;
        public PerkContainer FixedPerks;
        public ChestType FromChest;
        public string Id;
        public string Name;
        public int Rarity;
        public string SpriteId;
        public ItemType Type;

        public Item()
        {
            this.Id = string.Empty;
            this.Name = string.Empty;
            this.Accessories = new OrderedDict<AccessoryType, string>();
            this.SpriteId = string.Empty;
            this.BodyMaterials = new List<string>();
            this.Description = string.Empty;
            this.FixedPerks = new PerkContainer();
        }

        public Item(Item another)
        {
            this.Id = string.Empty;
            this.Name = string.Empty;
            this.Accessories = new OrderedDict<AccessoryType, string>();
            this.SpriteId = string.Empty;
            this.BodyMaterials = new List<string>();
            this.Description = string.Empty;
            this.FixedPerks = new PerkContainer();
            this.copyFrom(another);
        }

        public int CompareTo(Item other)
        {
            if (this.Rarity > other.Rarity)
            {
                return 1;
            }
            if (this.Rarity < other.Rarity)
            {
                return -1;
            }
            int num = GameLogic.Binder.ItemResources.getItemOrderedIndex(this.Id);
            int num2 = GameLogic.Binder.ItemResources.getItemOrderedIndex(other.Id);
            if (num < num2)
            {
                return -1;
            }
            if (num > num2)
            {
                return 1;
            }
            return 0;
        }

        public void copyFrom(Item another)
        {
            this.Id = another.Id;
            this.Name = another.Name;
            this.Type = another.Type;
            this.Accessories = another.Accessories;
            this.Rarity = another.Rarity;
            this.SpriteId = another.SpriteId;
            this.BodyMaterials = another.BodyMaterials;
            this.Description = another.Description;
            this.FixedPerks = new PerkContainer(another.FixedPerks);
            this.FromChest = another.FromChest;
        }

        public string getSpriteId()
        {
            return this.SpriteId;
        }

        public void postDeserializeInitialization()
        {
        }
    }
}

