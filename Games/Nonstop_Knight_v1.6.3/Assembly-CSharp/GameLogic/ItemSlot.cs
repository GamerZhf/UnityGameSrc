namespace GameLogic
{
    using App;
    using System;

    public class ItemSlot : IJsonData
    {
        public ItemType CompatibleItemType;
        public GameLogic.ItemInstance ItemInstance;

        public ItemSlot()
        {
        }

        public ItemSlot(ItemSlot prototype)
        {
            this.copyFrom(prototype);
        }

        public void copyFrom(ItemSlot prototype)
        {
            this.CompatibleItemType = prototype.CompatibleItemType;
            if (prototype.ItemInstance != null)
            {
                this.ItemInstance = new GameLogic.ItemInstance(prototype.ItemInstance);
            }
            else
            {
                this.ItemInstance = null;
            }
        }

        public void postDeserializeInitialization()
        {
            if (this.ItemInstance != null)
            {
                this.ItemInstance.postDeserializeInitialization();
            }
        }
    }
}

