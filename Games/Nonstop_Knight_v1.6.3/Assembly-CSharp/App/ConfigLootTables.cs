namespace App
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class ConfigLootTables
    {
        public LootTable BossAdditionalDropLootTable;
        public LootTable BossDropLootTable;
        public static int CHEST_NUM_ROLLS_DEFAULT;
        public static Dictionary<ChestType, int> CHEST_TYPE_NUM_ROLLS;
        public Dictionary<ChestType, LootTable> ChestLootTables;
        public LootTable DungeonBoostBoxTable = new LootTable();
        public LootTable DungeonBoostResourceBoxLootTable = new LootTable();
        public LootTable MegaboxSlot1LootTable;
        public LootTable MegaboxSlot2LootTable;
        public LootTable MegaboxSlot3LootTable;
        public LootTable MinionDropLootTable;
        public LootTable MissionRewardLootTable;
        public Dictionary<string, LootTable> PetBundleLootTables;
        public Dictionary<int, int> UniversalItemRarityWeights;
        public LootTable VendorSlot1LootTable;
        public LootTable VendorSlot2LootTable;
        public LootTable VendorSlot3LootTable;

        static ConfigLootTables()
        {
            Dictionary<ChestType, int> dictionary = new Dictionary<ChestType, int>(new ChestTypeBoxAvoidanceComparer());
            dictionary.Add(ChestType.RewardBoxMulti, 3);
            dictionary.Add(ChestType.LootBoxBossHunt, 1);
            CHEST_TYPE_NUM_ROLLS = dictionary;
            CHEST_NUM_ROLLS_DEFAULT = 1;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v10()
        {
            return new Dictionary<string, System.Type>();
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v2()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("BossDropLootTable", typeof(LootTable));
            dictionary.Add("BossAdditionalDropLootTable", typeof(LootTable));
            dictionary.Add("MinionDropLootTable", typeof(LootTable));
            dictionary.Add("ChestLootTables", typeof(Dictionary<ChestType, LootTable>));
            dictionary.Add("VendorSlot1LootTable", typeof(LootTable));
            dictionary.Add("VendorSlot2LootTable", typeof(LootTable));
            dictionary.Add("VendorSlot3LootTable", typeof(LootTable));
            return dictionary;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v4()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("UniversalItemRarityWeights", typeof(Dictionary<int, int>));
            return dictionary;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v7()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("MegaboxSlot1LootTable", typeof(LootTable));
            dictionary.Add("MegaboxSlot2LootTable", typeof(LootTable));
            dictionary.Add("MegaboxSlot3LootTable", typeof(LootTable));
            dictionary.Add("PetBundleLootTables", typeof(Dictionary<string, LootTable>));
            dictionary.Add("MissionRewardLootTable", typeof(LootTable));
            return dictionary;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v9()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("DungeonBoostBoxTable", typeof(LootTable));
            dictionary.Add("DungeonBoostResourceBoxLootTable", typeof(LootTable));
            return dictionary;
        }

        public static int GetChestTypeNumRolls(ChestType chestType)
        {
            return (!CHEST_TYPE_NUM_ROLLS.ContainsKey(chestType) ? CHEST_NUM_ROLLS_DEFAULT : CHEST_TYPE_NUM_ROLLS[chestType]);
        }

        public LootTable getLootTableForChestType(ChestType chestType, [Optional, DefaultParameterValue(0)] int rollNo)
        {
            if (chestType == ChestType.RewardBoxMulti)
            {
                switch (rollNo)
                {
                    case 0:
                        return this.MegaboxSlot1LootTable;

                    case 1:
                        return this.MegaboxSlot2LootTable;

                    case 2:
                        return this.MegaboxSlot3LootTable;
                }
                return this.MegaboxSlot1LootTable;
            }
            if (chestType == ChestType.MysterySpecialOffer)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                if ((activeDungeon != null) && (activeDungeon.ActiveTournament != null))
                {
                    return this.ChestLootTables[ChestType.MysterySpecialOffer_BossHunt];
                }
            }
            if (ConfigMeta.IsBossChest(chestType))
            {
                return this.prepareDynamicChestTable(chestType);
            }
            return this.ChestLootTables[chestType];
        }

        public LootTable getLootTableForVendorSlot(int vendorSlot)
        {
            switch (vendorSlot)
            {
                case 1:
                    return this.VendorSlot1LootTable;

                case 2:
                    return this.VendorSlot2LootTable;

                case 3:
                    return this.VendorSlot3LootTable;
            }
            return null;
        }

        private LootTable prepareDynamicChestTable(ChestType chestType)
        {
            LootTable table = GameLogic.Binder.ItemResources.getDynamicChestLootTable(chestType);
            for (int i = 0; i < table.Items.Count; i++)
            {
                LootTableItem lti = table.Items[i];
                Item item2 = GameLogic.Binder.ItemResources.getItemForLootTableRollId(lti.Id, ItemType.UNSPECIFIED);
                if (item2 != null)
                {
                    int weight = App.Binder.ConfigLootTables.UniversalItemRarityWeights[item2.Rarity];
                    table.setWeightForItem(lti, weight);
                }
            }
            return table;
        }
    }
}

