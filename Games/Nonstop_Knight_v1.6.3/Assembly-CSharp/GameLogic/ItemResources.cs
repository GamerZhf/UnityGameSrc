namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ItemResources : CsvResources<string, Item>
    {
        private Dictionary<ChestType, LootTable> m_dynamicChestLootTables = new Dictionary<ChestType, LootTable>(new ChestTypeBoxAvoidanceComparer());
        private List<string> m_orderedItemIds = new List<string>();
        private List<Item> m_sortedItems = new List<Item>(0x80);
        private List<Item> m_tempCandidateList = new List<Item>(0x80);

        public ItemResources()
        {
            for (int i = 0; i < ConfigMeta.BOSS_CHESTS.Count; i++)
            {
                this.m_dynamicChestLootTables.Add(ConfigMeta.BOSS_CHESTS[i], new LootTable());
            }
            this.loadItemCsv("Items/Items-Weapons");
            this.loadItemCsv("Items/Items-Armors");
            this.loadItemCsv("Items/Items-Cloaks");
        }

        public LootTable getDynamicChestLootTable(ChestType chestType)
        {
            return this.m_dynamicChestLootTables[chestType];
        }

        public Material getItemBodyMaterial(Item item, int materialIdx)
        {
            if (materialIdx < item.BodyMaterials.Count)
            {
                return ResourceUtil.Instantiate<Material>("Materials/" + item.BodyMaterials[materialIdx]);
            }
            return null;
        }

        public Item getItemForLootTableRollId(string lootTableRollId, [Optional, DefaultParameterValue(0)] ItemType ofType)
        {
            if (base.m_resources.ContainsKey(lootTableRollId))
            {
                return base.m_resources[lootTableRollId];
            }
            return null;
        }

        public int getItemOrderedIndex(string id)
        {
            return this.m_orderedItemIds.IndexOf(id);
        }

        public List<Item> getItemsOfRarity_Fast(int rarity)
        {
            this.m_tempCandidateList.Clear();
            for (int i = 0; i < this.m_sortedItems.Count; i++)
            {
                Item item = this.m_sortedItems[i];
                if (item.Rarity == rarity)
                {
                    this.m_tempCandidateList.Add(item);
                }
            }
            return this.m_tempCandidateList;
        }

        public int getNumberOfItemsInBasicChests()
        {
            int num = 0;
            for (int i = 0; i < ConfigMeta.BOSS_CHESTS_BASIC.Length; i++)
            {
                ChestType type = ConfigMeta.BOSS_CHESTS_BASIC[i];
                num += this.m_dynamicChestLootTables[type].Items.Count;
            }
            return num;
        }

        public int getNumberOfItemsInBossChests()
        {
            return ((this.getNumberOfItemsInBasicChests() + this.getNumberOfItemsInSpecialChests()) + this.getNumberOfItemsInEventChests());
        }

        public int getNumberOfItemsInEventChests()
        {
            int num = 0;
            for (int i = 0; i < ConfigMeta.BOSS_CHESTS_EVENT.Length; i++)
            {
                ChestType type = ConfigMeta.BOSS_CHESTS_EVENT[i];
                num += this.m_dynamicChestLootTables[type].Items.Count;
            }
            return num;
        }

        public int getNumberOfItemsInSpecialChests()
        {
            int num = 0;
            for (int i = 0; i < ConfigMeta.BOSS_CHESTS_SPECIAL.Length; i++)
            {
                ChestType type = ConfigMeta.BOSS_CHESTS_SPECIAL[i];
                num += this.m_dynamicChestLootTables[type].Items.Count;
            }
            return num;
        }

        public List<string> getOrderedItemIdList()
        {
            return this.m_orderedItemIds;
        }

        public Item getRandomItem([Optional, DefaultParameterValue(0)] ItemType type)
        {
            this.m_tempCandidateList.Clear();
            for (int i = 0; i < this.m_sortedItems.Count; i++)
            {
                Item item = this.m_sortedItems[i];
                if ((type == ItemType.UNSPECIFIED) || (type == item.Type))
                {
                    this.m_tempCandidateList.Add(item);
                }
            }
            return LangUtil.GetRandomValueFromList<Item>(this.m_tempCandidateList);
        }

        public Item getRandomItemOfRarity(int rarity, [Optional, DefaultParameterValue(0)] ItemType type)
        {
            this.m_tempCandidateList.Clear();
            for (int i = 0; i < this.m_sortedItems.Count; i++)
            {
                Item item = this.m_sortedItems[i];
                if ((item.Rarity == rarity) && ((type == ItemType.UNSPECIFIED) || (type == item.Type)))
                {
                    this.m_tempCandidateList.Add(item);
                }
            }
            if (this.m_tempCandidateList.Count > 0)
            {
                return LangUtil.GetRandomValueFromList<Item>(this.m_tempCandidateList);
            }
            return null;
        }

        public List<Item> getSortedItemIdList()
        {
            return this.m_sortedItems;
        }

        public void initialize()
        {
            this.m_sortedItems.Sort();
        }

        public bool itemExists(string id)
        {
            return base.m_resources.ContainsKey(id);
        }

        private void loadItemCsv(string csvFilePath)
        {
            string[,] strArray = CsvUtils.Deserialize(ResourceUtil.LoadSafe<TextAsset>(csvFilePath, false).text);
            for (int i = 0; i < strArray.GetLength(1); i++)
            {
                if (strArray[0, i] != null)
                {
                    Item res = new Item();
                    int num2 = 0;
                    res.Id = strArray[num2++, i];
                    res.Name = _.L(strArray[num2++, i], null, false);
                    res.Type = base.parseEnumType<ItemType>(strArray[num2++, i]);
                    res.Accessories = base.parseOrderedEnumStringDict<AccessoryType>(strArray[num2++, i]);
                    res.SpriteId = strArray[num2++, i];
                    res.BodyMaterials = base.parseStringList(strArray[num2++, i]);
                    res.Rarity = base.parseInt(strArray[num2++, i]);
                    List<PerkType> list = base.parseEnumList<PerkType>(strArray[num2++, i]);
                    for (int j = 0; j < list.Count; j++)
                    {
                        PerkType perkType = list[j];
                        float bestModifier = ConfigPerks.GetBestModifier(perkType);
                        PerkInstance item = new PerkInstance();
                        item.Type = perkType;
                        item.Modifier = bestModifier;
                        res.FixedPerks.PerkInstances.Add(item);
                    }
                    res.FromChest = base.parseEnumType<ChestType>(strArray[num2++, i]);
                    res.postDeserializeInitialization();
                    base.addResource(res.Id, res);
                    this.m_orderedItemIds.Add(res.Id);
                    this.m_sortedItems.Add(res);
                    if (res.FromChest != ChestType.NONE)
                    {
                        LootTableItem item3 = new LootTableItem();
                        item3.Id = res.Id;
                        LootTableItem item2 = item3;
                        this.m_dynamicChestLootTables[res.FromChest].Items.Add(item2);
                    }
                }
            }
        }

        public void releaseItemBodyMaterial(Material mat)
        {
            UnityEngine.Object.Destroy(mat);
        }
    }
}

