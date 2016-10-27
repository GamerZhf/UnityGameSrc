namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Runtime.CompilerServices;

    public class ShopEntryInstance : IJsonData
    {
        [CompilerGenerated]
        private GameLogic.ShopEntry <ShopEntry>k__BackingField;
        public int NumTimesPurchased;
        public ChestType PrerolledChestType;
        public string PrerolledItemId;
        public string ShopEntryId;
        public bool Sold;

        public ShopEntryInstance()
        {
        }

        public ShopEntryInstance(ShopEntryInstance another)
        {
            this.ShopEntryId = another.ShopEntryId;
            this.Sold = another.Sold;
            this.postDeserializeInitialization();
        }

        public ShopEntryInstance(string shopEntryId, ChestType prerolledChestType)
        {
            this.ShopEntryId = shopEntryId;
            this.PrerolledChestType = prerolledChestType;
            this.postDeserializeInitialization();
        }

        public void postDeserializeInitialization()
        {
            this.ShopEntry = ConfigShops.GetShopEntry(this.ShopEntryId);
        }

        [JsonIgnore]
        public GameLogic.ShopEntry ShopEntry
        {
            [CompilerGenerated]
            get
            {
                return this.<ShopEntry>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ShopEntry>k__BackingField = value;
            }
        }
    }
}

