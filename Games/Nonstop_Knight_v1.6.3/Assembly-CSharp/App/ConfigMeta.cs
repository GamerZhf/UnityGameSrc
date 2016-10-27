namespace App
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ConfigMeta
    {
        public Dictionary<int, double> ACHIEVEMENT_TIER_DIAMOND_REWARDS;
        public static List<AccessoryType> ACTIVE_ACCESSORY_TYPES;
        public static List<ItemType> ACTIVE_ITEM_TYPES;
        public bool ALLOW_BOSS_AUTO_SUMMON_IN_NEW_FLOORS = true;
        public bool ALLOW_VENDOR_TASKPANEL_NOTIFIER = true;
        public long ANDROID_VIDEO_AD_SESSION_TIMEOUT_SECONDS = 600L;
        public int BASIC001_CHEST_DROP_MAX_FLOOR = 50;
        public static bool BOSS_ADDITIONAL_DROPS_ENABLED = true;
        public bool BOSS_AUTO_SUMMON_ENABLED = true;
        public static List<ChestType> BOSS_CHESTS;
        public static ChestType[] BOSS_CHESTS_BASIC = new ChestType[] { ChestType.Basic001, ChestType.Basic002, ChestType.Basic003 };
        public static ChestType[] BOSS_CHESTS_EVENT = new ChestType[] { ChestType.EventHalloween };
        public static ChestType[] BOSS_CHESTS_SPECIAL = new ChestType[] { ChestType.Special003, ChestType.Special002, ChestType.Special007, ChestType.Special001, ChestType.Special004, ChestType.Special008, ChestType.Special005, ChestType.Special009, ChestType.Special006 };
        public double BOSS_COINDROP_MULTIPLIER;
        public double BOSS_COINDROP_MULTIPLIER_BOSSTRAIN;
        public double BOSS_DAMAGE_MULTIPLIER;
        public double BOSS_HEALTH_MULTIPLIER;
        public int BOSS_POTION_NUM_BOSSES = 3;
        public bool BOSS_POTIONS_ENABLED = true;
        public int BOSS_SUMMON_NOTIFICATION_TIMER_MIN_SECONDS = 0x1c20;
        public static bool CAN_SELL_HIGHEST_LEVEL_ITEM;
        public static bool CAN_SELL_HIGHEST_RARITY_ITEM;
        public Dictionary<ChestType, int> CHEST_COIN_MULTIPLIERS;
        public Dictionary<ChestType, int> CHEST_DIAMOND_DROPS;
        public Dictionary<ChestType, int> CHEST_DROP_MAX_FLOOR = new Dictionary<ChestType, int>();
        public Dictionary<ChestType, int> CHEST_TOKEN_DROPS;
        public Dictionary<ChestType, int> CHEST_UNLOCK_FLOOR;
        public double COIN_BUNDLE_MINIMUM_BASE_VALUE = 150.0;
        public double COIN_GAIN_CONTROLLER;
        public bool COIN_UPGRADES_AFFECT_MYSTERY_CHESTS;
        public bool COIN_UPGRADES_AFFECT_VENDOR_COIN_BUNDLES;
        public bool COINBUNDLES_USE_COINBALANCE = true;
        public bool COINBUNDLES_USE_PREASCENSIONITEMS = true;
        public bool COMBAT_STATS_ENABLED = true;
        public bool CTUT003_ENABLED = true;
        public int CTUT003_REQUIRED_MONSTER_KILLS = 0x2d;
        public int CTUT003_REQUIRED_UPGRADE_COUNT = 3;
        public bool CTUT004_ENABLED = true;
        public int CTUT004_REQUIRED_MONSTER_KILLS = 0x41;
        public bool CTUT005_ENABLED = true;
        public int DAILY_ADS_LIMIT;
        public int DAILY_ADS_LIMIT_ADVENTURE_MYSTERY;
        public int DAILY_ADS_LIMIT_TOURNAMENT_CARDS;
        public int DAILY_ADS_LIMIT_TOURNAMENT_MYSTERY;
        public int DAILY_ADS_LIMIT_VENDOR;
        public int DAILY_DIAMOND_LIMIT;
        public double DIFFICULTY_AVERAGE_SKILL_DAMAGE_MULTIPLIER = 3.0;
        public double DIMINISHINGCOINS_MULTIPLIER = 0.5;
        public bool DISABLE_VENDOR_ADS_CONFIRMATION_POPUP = true;
        public int DUNGEON_BOOST_BOX_FIRST_RUN_MAX_SPAWN_COUNT_FLOOR = 20;
        public int DUNGEON_BOOST_BOX_FIRST_RUN_MIN_SPAWN_COUNT_FLOOR = 10;
        public bool DUNGEON_BOOST_BOX_SPAWN_ENABLED_DURING_FRENZY = true;
        public float DUNGEON_BOOST_EMPTY_BOX_COIN_GAIN_CONTROLLER = 0.25f;
        public DungeonEventType DUNGEON_EVENT_TYPE;
        public bool ELITE_BOSSES_ENABLED = true;
        public bool ENABLE_POST_SECURE = true;
        public string FACEBOOK_URL = "https://www.facebook.com/Nonstop-Knight-965906406766140/";
        public string FORUMS_URL = "http://forums.flaregames.com/forum/78-nonstop-knight/";
        public int FRENZY_BONUS_POTION_EVERY_X_FLOOR = 60;
        public int FRENZY_BONUS_POTION_FIRST_FLOOR = 0x3b;
        public float FRENZY_HERO_BUFF_ATK_SPEED_MODIFIER = 0.75f;
        public float FRENZY_HERO_BUFF_DPH_MODIFIER = 1f;
        public float FRENZY_HERO_BUFF_LIFE_MODIFIER = 1f;
        public float FRENZY_HERO_BUFF_SKILLDMG_MODIFIER = 1f;
        public int FRENZY_MOB_SPAWNCOUNT_PER_SPAWNPOINT_MAX = 8;
        public int FRENZY_MOB_SPAWNCOUNT_PER_SPAWNPOINT_MIN = 6;
        public float FRENZY_TIMER_ADD_SECONDS_PER_MINION_KILL = 0.15f;
        public float FRENZY_TIMER_ADD_SECONDS_PER_MULTIKILL = 0.5f;
        public float FRENZY_TIMER_MAX_SECONDS = 60f;
        public int GLOBAL_LEVEL_CAP;
        public bool HIGH_FREQUENT_PROMOTION_UPDATES = true;
        public double HIGHEST_FLOOR_COINS_MULTIPLIER = 4.0;
        public double INSTANT_ITEM_UPGRADE_PRICE;
        public double ITEM_BASECOST_MULTIPLIER;
        public static Dictionary<ItemType, double> ITEM_COST_MULTIPLIERS;
        public double ITEM_DAMAGE_MULTIPLIER;
        public double ITEM_HEALTH_MULTIPLIER;
        public static int ITEM_HIGHEST_RARITY;
        public static int ITEM_INSTANT_UPGRADE_LEVEL_THRESHOLD;
        public Dictionary<int, int> ITEM_RARITY_BOSS_DROP_MAX_FLOOR;
        public Dictionary<int, int> ITEM_RARITY_BOSS_DROP_MIN_FLOOR;
        public Dictionary<int, int> ITEM_RARITY_UNLOCK_FLOOR;
        public Dictionary<int, double> ITEM_SELL_COIN_GAIN_OFFSET_BY_RARITY;
        public double ITEM_SKILLDAMAGE_MULTIPLIER;
        public static int ITEM_START_RANK_OFFSET_MAX;
        public static int ITEM_START_RANK_OFFSET_MIN;
        public int ITEM_TOKEN_VALUE;
        public static int ITEM_TYPE_ROLL_HISTORY;
        public static int ITEM_UNLOCK_LEVEL_OFFSET_MAX;
        public static int ITEM_UNLOCK_LEVEL_OFFSET_MIN;
        public double LEADERBOARD_COINS_MULTIPLIER = 4.0;
        public double LEADERBOARD_RANK_REWARD_DIAMONDS = 5.0;
        public int LEVEL_TO_FLOOR_MULTIPLIER;
        public const int MAX_NUM_VISIBLE_AUGMENTATIONS_IN_SHOP = 6;
        public double MINION_COINDROP_MULTIPLIER;
        public double MINION_DAMAGE_MULTIPLIER;
        public double MINION_HEALTH_MULTIPLIER;
        public long MISSION_BASE_COOLDOWN_SECONDS = 0x5460L;
        public ChestType MISSION_BIG_PRIZE_CHEST_TYPE = ChestType.RewardBoxMulti;
        public bool MISSION_BIG_PRIZE_INSTANT_SHOP_AVAILABILITY;
        public bool MISSION_NOTIFICATIONS_ENABLED = true;
        public Dictionary<string, MissionConfig> MISSIONS = new Dictionary<string, MissionConfig>();
        public List<MissionConfig> MISSIONS_FTUE = new List<MissionConfig>();
        public static int MULTIKILL_REWARD_MIN_KILL_COUNT;
        public long MYSTERY_CHEST_COOLDOWN_SECONDS;
        public int MYSTERY_CHEST_DROP_UNLOCK_FLOOR;
        public static ChestType[] MYSTERY_CHESTS = new ChestType[] { ChestType.MysteryStandard, ChestType.MysterySpecialOffer };
        public double MYSTERYCHEST_COINBUNDLE_MINIMUM = 1.5;
        public bool MYSTERYCHEST_DIMINISHINGCOINS = true;
        public bool NOTIFY_NEW_MISSIONS_DURING_WELCOME_BACK = true;
        public int NUM_COMPLETED_MISSIONS_REQUIRED_FOR_BIG_PRIZE = 10;
        public int NUM_COMPLETED_MISSIONS_REQUIRED_FOR_BIG_PRIZE_FTUE = 3;
        public int NUM_VISIBLE_MENU_MILESTONES = 1;
        public float PASSIVE_BOSS_KILL_FREQUENCY_SECONDS = 1800f;
        public double PASSIVE_COIN_EARNING_RATE_MULTIPLIER;
        public long PASSIVE_COIN_GAIN_CEREMONY_COOLDOWN_SECONDS;
        public double PASSIVE_COIN_GAIN_MAX_TIMEOFF_SECONDS;
        public int PASSIVE_ITEM_UPGRADE_COUNT_AFTER_BOSS_FIGHT = 6;
        public int PASSIVE_MAX_BOSS_ENCOUNTER_COUNT = 6;
        public int PASSIVE_MAX_BOSS_ITEM_DROP_COUNT = 6;
        public int PASSIVE_MAX_MINION_COIN_DROP_COUNT = 0x3e8;
        public double PASSIVE_MINION_KILL_FREQUENCY_SECONDS;
        public List<int> PET_LEVELUP_DUPLICATE_REQUIREMENTS = new List<int>();
        public double PET_POWER_INCREASE_PER_LEVEL = 0.15000000596046448;
        public List<PetConfig> PETS = new List<PetConfig>();
        public float PLAYER_SYNC_TIMEOUT = 120f;
        public static int PLAYER_UPGRADES_MAX_RANK;
        public double PREASCENSION_LEVELMULTIPLIER = 0.5;
        public string PRIVACY_URL = "http://www.flaregames.com/privacy-policy";
        public double PROGRESS_COST_EXPONENT;
        public double PROGRESS_DIFFICULTY_EXPONENT;
        public int PROGRESS_MINIONS_PER_FLOOR;
        public double PROGRESS_VALUE_EXPONENT;
        public Dictionary<int, double> REROLL_PRICE_BASE_PER_RARITY;
        public Dictionary<int, double> REROLL_PRICE_INCREMENT_PER_RARITY;
        public static Dictionary<ResourceType, string> RESOURCE_TYPE_TO_STRING_MAPPING;
        public static ChestType[] RETIREMENT_CHESTS = new ChestType[] { ChestType.RetirementTrigger, ChestType.RewardBoxCommon, ChestType.RewardBoxRare, ChestType.RewardBoxEpic, ChestType.RewardBoxMulti, ChestType.PetBoxSmall };
        public bool RETIREMENT_FORCED_DURING_FTUE;
        public int RETIREMENT_GATE_FLOOR_OFFSET;
        public int RETIREMENT_MIN_FLOOR;
        public bool RETIREMENT_NOTIFICATION_ENABLED;
        public double RETIREMENT_REWARD_BASE;
        public double RETIREMENT_REWARD_INCREMENT;
        public int REVIVE_PRICE_BASE;
        public int REVIVE_PRICE_INCREMENT;
        public int REVIVE_UNLOCK_FLOOR;
        public int REWARD_BOX_BONUS_EVERY_X_FLOOR = 60;
        public int REWARD_BOX_BONUS_FIRST_FLOOR = 0x1d;
        public List<int> REWARD_BOX_GUARANTEED_DROP_FLOORS;
        public bool REWARD_ON_VIDEO_AD_TIMOUT = true;
        public static bool RUNESTONE_DUPLICATES_AUTO_CONVERTED_INTO_TOKENS;
        public static int RUNESTONE_MAX_LEVEL;
        public bool SELL_AUGMENTATIONS_IN_ADVENTURE_PANEL;
        public bool SHOW_SHOP_BUTTON_IN_TOP = true;
        public bool SHOW_SHOP_IN_SLIDING_MENU;
        private static List<string> sm_tempCandidateList;
        private static Dictionary<string, int> sm_tempWeightDict;
        public string SNAPCHAT_URL = "https://www.snapchat.com/add/nonstopknight";
        public int SPECIAL_CHEST_DROP_MIN_FLOOR;
        public double STARTER_BUNDLE_DIAMOND_COUNT = 275.0;
        public bool STARTER_BUNDLE_SELLING_ENABLED = true;
        public int STARTER_BUNDLE_TASKPANEL_OFFER_FLOOR = 15;
        public string STORE_URL_ANDROID = "market://details?id=com.koplagames.kopla01";
        public string STORE_URL_IOS = "https://itunes.apple.com/app/nonstop-knight/id979151411?mt=8";
        public string SUPPORT_URL = "https://flaregames.zendesk.com/hc/categories/200855975-Nonstop-Knight";
        public float SYNC_CHECK_INTERVAL = 20f;
        public int THRESHHOLD_DONTOFFER_BOSSTICKET = 15;
        public int THRESHHOLD_DONTOFFER_FRENZY = 15;
        public int THRESHHOLD_DONTOFFER_REVIVEPOTION = 15;
        public double TOKEN_REWARD_CONTROLLER;
        public List<Tuple<int, double>> TOKEN_REWARD_FLOOR_MULTIPLIERS;
        public int TOURNAMENT_CARD_PRICE_EPIC;
        public int TOURNAMENT_CARD_PRICE_NORMAL;
        public bool TOURNAMENT_DIRECT_CARD_PURCHASING_ENABLED;
        public double TOURNAMENT_DONATION_PRICE;
        public int TOURNAMENT_MAX_DONATIONS_PER_PLAYER_PER_TOUNAMENT;
        public float TOURNAMENT_UPGRADE_EPIC_PROBABILITY;
        public Dictionary<string, TournamentUpgrade> TOURNAMENT_UPGRADES;
        public int TOURNAMENT_WILD_BOSS_MIN_SUMMON_FLOOR;
        public string TRACKING_URL = string.Empty;
        public string TWITTER_URL = "https://twitter.com/nonstopknight";
        public int VENDOR_AND_SHOP_UNLOCK_FLOOR;
        public double VENDOR_AUGMENTATION_SALE_CONTROLLER = 1.0;
        public Dictionary<string, double> VENDOR_BOSS_BUNDLES;
        public Dictionary<string, double> VENDOR_COIN_BUNDLES;
        public bool VENDOR_COIN_BUNDLES_ALWAYS_APPEAR;
        public List<string> VENDOR_COIN_BUNDLES_USING_ALTERNATIVE_SIZE = new List<string>();
        public Dictionary<string, double> VENDOR_DIAMOND_BUNDLES;
        public Dictionary<string, double> VENDOR_DUST_BUNDLES;
        public string VENDOR_FIXED_SLOT1_ENTRY;
        public string VENDOR_FIXED_SLOT2_ENTRY;
        public string VENDOR_FIXED_SLOT3_ENTRY;
        public bool VENDOR_FIXED_SLOTS_ENABLED;
        public Dictionary<string, double> VENDOR_FRENZY_BUNDLES;
        public int VENDOR_INVENTORY_SIZE;
        public Dictionary<string, double> VENDOR_MEGA_BOX_BUNDLES;
        public Dictionary<string, MinMaxInt> VENDOR_PET_BUNDLES;
        public Dictionary<string, VendorPriceData> VENDOR_PRICE_DATA;
        public int VENDOR_REFRESH_INTERVAL_SECONDS;
        public Dictionary<string, double> VENDOR_REVIVE_BUNDLES;
        public bool VENDOR_SLOT1_ADS;
        public int VENDOR_SLOT1_NUM_ALLOWED_PURCHASES = 1;
        public int VENDOR_SLOT1_PRICE;
        public int VENDOR_SLOT1_PRICE_INCREMENT;
        public int VENDOR_SLOT2_NUM_ALLOWED_PURCHASES = 10;
        public int VENDOR_SLOT2_PRICE;
        public int VENDOR_SLOT2_PRICE_INCREMENT;
        public int VENDOR_SLOT3_NUM_ALLOWED_PURCHASES = 10;
        public int VENDOR_SLOT3_PRICE;
        public int VENDOR_SLOT3_PRICE_INCREMENT;
        public Dictionary<string, double> VENDOR_TOKEN_BUNDLE_MINIMUM_SIZE;
        public Dictionary<string, double> VENDOR_TOKEN_BUNDLES;
        public Dictionary<string, double> VENDOR_XP_BUNDLES;
        public bool VENDORSTACKSIZE_ALWAYS_USE_MAX;
        public double WILD_BOSS_DAMAGE_MULTIPLIER;
        public double WILD_BOSS_HEALTH_MULTIPLIER;
        public bool WINTER_THEME_ENABLED = true;
        public double XP_BOSS_FLOORBONUS;
        public double XP_BOSS_OLDMULTIPLIER;
        public float XP_EXP = 1f;
        public int XP_GAIN_BOSS;
        public float XP_GAIN_CONTROLLER;
        public int XP_GAIN_MINION;
        public double XP_GAIN_PER_POTION = 0.25;
        public int XP_LEVEL_CAP;
        public int XP_RANK_BASE;
        public int XP_RANK_INCREMENT;
        public int XP_RANK_REWARD_GEMS;

        static ConfigMeta()
        {
            ChestType[][] arrays = new ChestType[][] { BOSS_CHESTS_BASIC, BOSS_CHESTS_SPECIAL, BOSS_CHESTS_EVENT };
            BOSS_CHESTS = LangUtil.MergeArrays<ChestType>(arrays);
            CAN_SELL_HIGHEST_LEVEL_ITEM = true;
            CAN_SELL_HIGHEST_RARITY_ITEM = true;
            ITEM_HIGHEST_RARITY = 3;
            ITEM_INSTANT_UPGRADE_LEVEL_THRESHOLD = 5;
            ITEM_START_RANK_OFFSET_MIN = -2;
            ITEM_START_RANK_OFFSET_MAX = 4;
            ITEM_UNLOCK_LEVEL_OFFSET_MIN = 0;
            ITEM_UNLOCK_LEVEL_OFFSET_MAX = 0;
            ITEM_TYPE_ROLL_HISTORY = 2;
            List<ItemType> list = new List<ItemType>();
            list.Add(ItemType.Weapon);
            list.Add(ItemType.Armor);
            list.Add(ItemType.Cloak);
            ACTIVE_ITEM_TYPES = list;
            ACTIVE_ACCESSORY_TYPES = LangUtil.GetEnumValuesWithException<AccessoryType>(AccessoryType.NONE);
            Dictionary<ItemType, double> dictionary = new Dictionary<ItemType, double>(new ItemTypeBoxAvoidanceComparer());
            dictionary.Add(ItemType.UNSPECIFIED, 1.0);
            dictionary.Add(ItemType.Weapon, 0.8);
            dictionary.Add(ItemType.Armor, 1.0);
            dictionary.Add(ItemType.Cloak, 1.2);
            ITEM_COST_MULTIPLIERS = dictionary;
            MULTIKILL_REWARD_MIN_KILL_COUNT = 3;
            Dictionary<ResourceType, string> dictionary2 = new Dictionary<ResourceType, string>(new ResourceTypeBoxAvoidanceComparer());
            dictionary2.Add(ResourceType.Energy, ResourceType.Energy.ToString());
            dictionary2.Add(ResourceType.Coin, ResourceType.Coin.ToString());
            dictionary2.Add(ResourceType.Diamond, ResourceType.Diamond.ToString());
            dictionary2.Add(ResourceType.Xp, ResourceType.Xp.ToString());
            dictionary2.Add(ResourceType.Token, ResourceType.Token.ToString());
            dictionary2.Add(ResourceType.Crown, ResourceType.Crown.ToString());
            dictionary2.Add(ResourceType.Dust, ResourceType.Dust.ToString());
            RESOURCE_TYPE_TO_STRING_MAPPING = dictionary2;
            RUNESTONE_DUPLICATES_AUTO_CONVERTED_INTO_TOKENS = true;
            PLAYER_UPGRADES_MAX_RANK = 9;
            RUNESTONE_MAX_LEVEL = 10;
            sm_tempCandidateList = new List<string>(0x10);
            sm_tempWeightDict = new Dictionary<string, int>(0x10);
        }

        public double BossCoinDropCurve(int floor, bool bossTrainActive)
        {
            if (bossTrainActive)
            {
                return Math.Floor(this.ProgressValueCurveFloor((double) floor) * this.BOSS_COINDROP_MULTIPLIER_BOSSTRAIN);
            }
            return Math.Floor(this.ProgressValueCurveFloor((double) floor) * this.BOSS_COINDROP_MULTIPLIER);
        }

        public double BossDamagePerHitCurve(int floor, double progressDifficultyExponent, bool isWildBoss)
        {
            double num;
            if (isWildBoss)
            {
                num = this.ProgressDifficultyCurveFloor((double) floor, progressDifficultyExponent) * this.WILD_BOSS_DAMAGE_MULTIPLIER;
            }
            else
            {
                num = this.ProgressDifficultyCurveFloor((double) floor, progressDifficultyExponent) * this.BOSS_DAMAGE_MULTIPLIER;
            }
            return Math.Floor(num);
        }

        public double BossLifeCurve(int floor, double progressDifficultyExponent, bool isWildBoss)
        {
            double num;
            if (isWildBoss)
            {
                num = this.ProgressDifficultyCurveFloor((double) floor, progressDifficultyExponent) * this.WILD_BOSS_HEALTH_MULTIPLIER;
            }
            else
            {
                num = this.ProgressDifficultyCurveFloor((double) floor, progressDifficultyExponent) * this.BOSS_HEALTH_MULTIPLIER;
            }
            return Math.Floor(num);
        }

        public bool BossShouldDropFrenzyPotionAtFloor(int floor)
        {
            return (this.GetNextGuaranteedFrenzyPotionDropFloor(floor) == floor);
        }

        public bool BossShouldDropRewardBoxAtFloor(int floor)
        {
            return (this.GetNextGuaranteedRewardBoxDropFloor(floor) == floor);
        }

        public int BossSummonRequiredMinionKills(int floor, bool lastBossEncounterFailed)
        {
            if (lastBossEncounterFailed)
            {
                return 0;
            }
            return this.PROGRESS_MINIONS_PER_FLOOR;
        }

        public double CoinBundleSize(Player player, string shopEntryId, [Optional, DefaultParameterValue(0)] double coinAmountNotAddedToPlayer)
        {
            CharacterInstance activeCharacter = player.ActiveCharacter;
            return this.CoinBundleSize(activeCharacter.HighestLevelItemOwnedAtFloorStart, player, shopEntryId, coinAmountNotAddedToPlayer);
        }

        public double CoinBundleSize(CharacterInstance.HighestLevelItemInfo info, Player player, string shopEntryId, [Optional, DefaultParameterValue(0)] double coinAmountNotAddedToPlayer)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            double numUpgrades = this.VENDOR_COIN_BUNDLES[shopEntryId];
            return (!this.VENDOR_COIN_BUNDLES_USING_ALTERNATIVE_SIZE.Contains(shopEntryId) ? this.CoinBundleSize(info, player, shopEntryId, numUpgrades, coinAmountNotAddedToPlayer) : this.CoinBundleSizeAlternative(activeDungeon, player, shopEntryId, numUpgrades, coinAmountNotAddedToPlayer));
        }

        public double CoinBundleSize(CharacterInstance.HighestLevelItemInfo info, Player player, string shopEntryId, double numUpgrades, double coinAmountNotAddedToPlayer)
        {
            int num = 0;
            if (this.COINBUNDLES_USE_COINBALANCE)
            {
                string str = RESOURCE_TYPE_TO_STRING_MAPPING[ResourceType.Coin];
                num = player.getPossibleUpgradeAmountForCost(info.ItemType, info.Level, info.Rank, player.getResources(0)[str] + coinAmountNotAddedToPlayer);
            }
            return Math.Floor(Math.Max(player.itemCumulativeUpgradeCost(info.ItemType, info.Level, info.Rank + num, numUpgrades), this.COIN_BUNDLE_MINIMUM_BASE_VALUE * numUpgrades));
        }

        public double CoinBundleSizeAlternative(ActiveDungeon ad, Player player, string shopEntryId, double numMinionsKills, double coinAmountNotAddedToPlayer)
        {
            double baseCoinReward = this.MinionCoinDropCurve(ad.Floor);
            double v = player.calculateStandardCoinRoll(baseCoinReward, GameLogic.CharacterType.UNSPECIFIED, 1) * numMinionsKills;
            return MathUtil.Clamp(v, 1.0, double.MaxValue);
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v10()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("BOSS_SUMMON_NOTIFICATION_TIMER_MIN_SECONDS", typeof(int));
            dictionary.Add("COINBUNDLES_USE_COINBALANCE", typeof(bool));
            dictionary.Add("COINBUNDLES_USE_PREASCENSIONITEMS", typeof(bool));
            dictionary.Add("PREASCENSION_LEVELMULTIPLIER", typeof(double));
            dictionary.Add("MYSTERYCHEST_DIMINISHINGCOINS", typeof(bool));
            dictionary.Add("DIMINISHINGCOINS_MULTIPLIER", typeof(double));
            dictionary.Add("MYSTERYCHEST_COINBUNDLE_MINIMUM", typeof(double));
            dictionary.Add("THRESHHOLD_DONTOFFER_FRENZY", typeof(int));
            dictionary.Add("THRESHHOLD_DONTOFFER_REVIVEPOTION", typeof(int));
            dictionary.Add("THRESHHOLD_DONTOFFER_BOSSTICKET", typeof(int));
            dictionary.Add("VENDORSTACKSIZE_ALWAYS_USE_MAX", typeof(bool));
            dictionary.Add("TOURNAMENT_UPGRADES", typeof(Dictionary<string, TournamentUpgrade>));
            dictionary.Add("TOURNAMENT_UPGRADE_EPIC_PROBABILITY", typeof(float));
            dictionary.Add("TOURNAMENT_WILD_BOSS_MIN_SUMMON_FLOOR", typeof(int));
            dictionary.Add("TOURNAMENT_DONATION_PRICE", typeof(double));
            dictionary.Add("TOURNAMENT_MAX_DONATIONS_PER_PLAYER_PER_TOUNAMENT", typeof(int));
            dictionary.Add("VENDOR_AUGMENTATION_SALE_CONTROLLER", typeof(double));
            dictionary.Add("DUNGEON_EVENT_TYPE", typeof(DungeonEventType));
            dictionary.Add("WILD_BOSS_HEALTH_MULTIPLIER", typeof(double));
            dictionary.Add("WILD_BOSS_DAMAGE_MULTIPLIER", typeof(double));
            dictionary.Add("RETIREMENT_NOTIFICATION_ENABLED", typeof(bool));
            dictionary.Add("DAILY_ADS_LIMIT_VENDOR", typeof(int));
            dictionary.Add("DAILY_ADS_LIMIT_ADVENTURE_MYSTERY", typeof(int));
            dictionary.Add("DAILY_ADS_LIMIT_TOURNAMENT_MYSTERY", typeof(int));
            dictionary.Add("DAILY_ADS_LIMIT_TOURNAMENT_CARDS", typeof(int));
            dictionary.Add("TOURNAMENT_CARD_PRICE_NORMAL", typeof(int));
            dictionary.Add("TOURNAMENT_CARD_PRICE_EPIC", typeof(int));
            dictionary.Add("SELL_AUGMENTATIONS_IN_ADVENTURE_PANEL", typeof(bool));
            dictionary.Add("DUNGEON_BOOST_BOX_SPAWN_ENABLED_DURING_FRENZY", typeof(bool));
            dictionary.Add("VENDOR_COIN_BUNDLES_USING_ALTERNATIVE_SIZE", typeof(List<string>));
            dictionary.Add("TOURNAMENT_DIRECT_CARD_PURCHASING_ENABLED", typeof(bool));
            dictionary.Add("NOTIFY_NEW_MISSIONS_DURING_WELCOME_BACK", typeof(bool));
            return dictionary;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v2()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("COIN_GAIN_CONTROLLER", typeof(double));
            dictionary.Add("XP_GAIN_CONTROLLER", typeof(float));
            dictionary.Add("TOKEN_REWARD_CONTROLLER", typeof(double));
            dictionary.Add("LEVEL_TO_FLOOR_MULTIPLIER", typeof(int));
            dictionary.Add("PROGRESS_VALUE_EXPONENT", typeof(double));
            dictionary.Add("PROGRESS_DIFFICULTY_EXPONENT", typeof(double));
            dictionary.Add("PROGRESS_COST_EXPONENT", typeof(double));
            dictionary.Add("PROGRESS_MINIONS_PER_FLOOR", typeof(int));
            dictionary.Add("GLOBAL_LEVEL_CAP", typeof(int));
            dictionary.Add("ITEM_HEALTH_MULTIPLIER", typeof(double));
            dictionary.Add("ITEM_DAMAGE_MULTIPLIER", typeof(double));
            dictionary.Add("ITEM_SKILLDAMAGE_MULTIPLIER", typeof(double));
            dictionary.Add("ITEM_BASECOST_MULTIPLIER", typeof(double));
            dictionary.Add("MINION_HEALTH_MULTIPLIER", typeof(double));
            dictionary.Add("MINION_DAMAGE_MULTIPLIER", typeof(double));
            dictionary.Add("BOSS_HEALTH_MULTIPLIER", typeof(double));
            dictionary.Add("BOSS_DAMAGE_MULTIPLIER", typeof(double));
            dictionary.Add("PASSIVE_COIN_GAIN_CEREMONY_COOLDOWN_SECONDS", typeof(long));
            dictionary.Add("PASSIVE_MINION_KILL_FREQUENCY_SECONDS", typeof(double));
            dictionary.Add("PASSIVE_COIN_EARNING_RATE_MULTIPLIER", typeof(double));
            dictionary.Add("PASSIVE_COIN_GAIN_MAX_TIMEOFF_SECONDS", typeof(double));
            dictionary.Add("VENDOR_INVENTORY_SIZE", typeof(int));
            dictionary.Add("VENDOR_REFRESH_INTERVAL_SECONDS", typeof(int));
            dictionary.Add("VENDOR_SLOT1_ADS", typeof(bool));
            dictionary.Add("VENDOR_SLOT1_PRICE", typeof(int));
            dictionary.Add("VENDOR_SLOT2_PRICE", typeof(int));
            dictionary.Add("VENDOR_SLOT3_PRICE", typeof(int));
            dictionary.Add("REVIVE_PRICE_BASE", typeof(int));
            dictionary.Add("REVIVE_PRICE_INCREMENT", typeof(int));
            dictionary.Add("INSTANT_ITEM_UPGRADE_PRICE", typeof(double));
            dictionary.Add("DAILY_ADS_LIMIT", typeof(int));
            dictionary.Add("DAILY_DIAMOND_LIMIT", typeof(int));
            dictionary.Add("MYSTERY_CHEST_COOLDOWN_SECONDS", typeof(long));
            dictionary.Add("REROLL_PRICE_BASE_PER_RARITY", typeof(Dictionary<int, double>));
            dictionary.Add("REROLL_PRICE_INCREMENT_PER_RARITY", typeof(Dictionary<int, double>));
            dictionary.Add("XP_GAIN_MINION", typeof(int));
            dictionary.Add("XP_GAIN_BOSS", typeof(int));
            dictionary.Add("XP_RANK_BASE", typeof(int));
            dictionary.Add("XP_RANK_INCREMENT", typeof(int));
            dictionary.Add("XP_LEVEL_CAP", typeof(int));
            dictionary.Add("VENDOR_AND_SHOP_UNLOCK_FLOOR", typeof(int));
            dictionary.Add("MYSTERY_CHEST_DROP_UNLOCK_FLOOR", typeof(int));
            dictionary.Add("REVIVE_UNLOCK_FLOOR", typeof(int));
            dictionary.Add("ITEM_RARITY_UNLOCK_FLOOR", typeof(Dictionary<int, int>));
            dictionary.Add("CHEST_UNLOCK_FLOOR", typeof(Dictionary<ChestType, int>));
            dictionary.Add("RETIREMENT_MIN_FLOOR", typeof(int));
            dictionary.Add("RETIREMENT_FORCED_DURING_FTUE", typeof(bool));
            dictionary.Add("RETIREMENT_REWARD_BASE", typeof(double));
            dictionary.Add("RETIREMENT_REWARD_INCREMENT", typeof(double));
            dictionary.Add("ITEM_TOKEN_VALUE", typeof(int));
            dictionary.Add("CHEST_TOKEN_DROPS", typeof(Dictionary<ChestType, int>));
            dictionary.Add("VENDOR_TOKEN_BUNDLES", typeof(Dictionary<string, double>));
            dictionary.Add("TOKEN_REWARD_FLOOR_MULTIPLIERS", typeof(List<Tuple<int, double>>));
            dictionary.Add("MINION_COINDROP_MULTIPLIER", typeof(double));
            dictionary.Add("BOSS_COINDROP_MULTIPLIER", typeof(double));
            dictionary.Add("CHEST_COIN_MULTIPLIERS", typeof(Dictionary<ChestType, int>));
            dictionary.Add("VENDOR_COIN_BUNDLES", typeof(Dictionary<string, double>));
            dictionary.Add("XP_RANK_REWARD_GEMS", typeof(int));
            dictionary.Add("CHEST_DIAMOND_DROPS", typeof(Dictionary<ChestType, int>));
            dictionary.Add("ACHIEVEMENT_TIER_DIAMOND_REWARDS", typeof(Dictionary<int, double>));
            dictionary.Add("VENDOR_REVIVE_BUNDLES", typeof(Dictionary<string, double>));
            dictionary.Add("REWARD_BOX_GUARANTEED_DROP_FLOORS", typeof(List<int>));
            dictionary.Add("VENDOR_SLOT1_PRICE_INCREMENT", typeof(int));
            dictionary.Add("VENDOR_SLOT2_PRICE_INCREMENT", typeof(int));
            dictionary.Add("VENDOR_SLOT3_PRICE_INCREMENT", typeof(int));
            dictionary.Add("VENDOR_SLOT1_NUM_ALLOWED_PURCHASES", typeof(int));
            dictionary.Add("VENDOR_SLOT2_NUM_ALLOWED_PURCHASES", typeof(int));
            dictionary.Add("VENDOR_SLOT3_NUM_ALLOWED_PURCHASES", typeof(int));
            dictionary.Add("RETIREMENT_GATE_FLOOR_OFFSET", typeof(int));
            return dictionary;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v3()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("STARTER_BUNDLE_SELLING_ENABLED", typeof(bool));
            dictionary.Add("STARTER_BUNDLE_TASKPANEL_OFFER_FLOOR", typeof(int));
            dictionary.Add("FRENZY_BONUS_POTION_EVERY_X_FLOOR", typeof(int));
            dictionary.Add("FRENZY_TIMER_MAX_SECONDS", typeof(float));
            dictionary.Add("FRENZY_TIMER_ADD_SECONDS_PER_MINION_KILL", typeof(float));
            dictionary.Add("FRENZY_HERO_BUFF_ATK_SPEED_MODIFIER", typeof(float));
            dictionary.Add("FRENZY_HERO_BUFF_DPH_MODIFIER", typeof(float));
            dictionary.Add("FRENZY_HERO_BUFF_LIFE_MODIFIER", typeof(float));
            dictionary.Add("FRENZY_HERO_BUFF_SKILLDMG_MODIFIER", typeof(float));
            dictionary.Add("FRENZY_MOB_SPAWNCOUNT_PER_SPAWNPOINT_MIN", typeof(int));
            dictionary.Add("FRENZY_MOB_SPAWNCOUNT_PER_SPAWNPOINT_MAX", typeof(int));
            dictionary.Add("CTUT003_ENABLED", typeof(bool));
            dictionary.Add("CTUT004_ENABLED", typeof(bool));
            dictionary.Add("CTUT003_REQUIRED_MONSTER_KILLS", typeof(int));
            dictionary.Add("CTUT003_REQUIRED_UPGRADE_COUNT", typeof(int));
            dictionary.Add("CTUT004_REQUIRED_MONSTER_KILLS", typeof(int));
            dictionary.Add("PASSIVE_BOSS_KILL_FREQUENCY_SECONDS", typeof(float));
            dictionary.Add("PASSIVE_MAX_MINION_COIN_DROP_COUNT", typeof(int));
            dictionary.Add("PASSIVE_MAX_BOSS_ENCOUNTER_COUNT", typeof(int));
            dictionary.Add("PASSIVE_MAX_BOSS_ITEM_DROP_COUNT", typeof(int));
            dictionary.Add("PASSIVE_ITEM_UPGRADE_COUNT_AFTER_BOSS_FIGHT", typeof(int));
            dictionary.Add("VENDOR_FRENZY_BUNDLES", typeof(Dictionary<string, double>));
            dictionary.Add("VENDOR_DUST_BUNDLES", typeof(Dictionary<string, double>));
            return dictionary;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v4()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("COIN_UPGRADES_AFFECT_VENDOR_COIN_BUNDLES", typeof(bool));
            dictionary.Add("COIN_UPGRADES_AFFECT_MYSTERY_CHESTS", typeof(bool));
            dictionary.Add("FACEBOOK_URL", typeof(string));
            dictionary.Add("FORUMS_URL", typeof(string));
            dictionary.Add("SNAPCHAT_URL", typeof(string));
            dictionary.Add("SUPPORT_URL", typeof(string));
            dictionary.Add("STORE_URL_ANDROID", typeof(string));
            dictionary.Add("STORE_URL_IOS", typeof(string));
            dictionary.Add("TWITTER_URL", typeof(string));
            dictionary.Add("VENDOR_COIN_BUNDLES_ALWAYS_APPEAR", typeof(bool));
            dictionary.Add("VENDOR_PRICE_DATA", typeof(Dictionary<string, VendorPriceData>));
            dictionary.Add("SPECIAL_CHEST_DROP_MIN_FLOOR", typeof(int));
            dictionary.Add("TRACKING_URL", typeof(string));
            dictionary.Add("PLAYER_SYNC_TIMEOUT", typeof(float));
            dictionary.Add("SYNC_CHECK_INTERVAL", typeof(float));
            dictionary.Add("COIN_BUNDLE_MINIMUM_BASE_VALUE", typeof(double));
            dictionary.Add("VENDOR_TOKEN_BUNDLE_MINIMUM_SIZE", typeof(Dictionary<string, double>));
            dictionary.Add("LEADERBOARD_RANK_REWARD_DIAMONDS", typeof(double));
            dictionary.Add("XP_EXP", typeof(float));
            dictionary.Add("XP_BOSS_FLOORBONUS", typeof(double));
            dictionary.Add("XP_BOSS_OLDMULTIPLIER", typeof(double));
            dictionary.Add("LEADERBOARD_COINS_MULTIPLIER", typeof(double));
            dictionary.Add("STARTER_BUNDLE_DIAMOND_COUNT", typeof(double));
            dictionary.Add("VENDOR_DIAMOND_BUNDLES", typeof(Dictionary<string, double>));
            dictionary.Add("BASIC001_CHEST_DROP_MAX_FLOOR", typeof(int));
            dictionary.Add("FRENZY_TIMER_ADD_SECONDS_PER_MULTIKILL", typeof(float));
            dictionary.Add("VENDOR_XP_BUNDLES", typeof(Dictionary<string, double>));
            dictionary.Add("BOSS_POTIONS_ENABLED", typeof(bool));
            dictionary.Add("BOSS_POTION_NUM_BOSSES", typeof(int));
            dictionary.Add("BOSS_COINDROP_MULTIPLIER_BOSSTRAIN", typeof(double));
            dictionary.Add("DIFFICULTY_AVERAGE_SKILL_DAMAGE_MULTIPLIER", typeof(double));
            return dictionary;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v6()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("FRENZY_BONUS_POTION_FIRST_FLOOR", typeof(int));
            dictionary.Add("REWARD_BOX_BONUS_FIRST_FLOOR", typeof(int));
            dictionary.Add("REWARD_BOX_BONUS_EVERY_X_FLOOR", typeof(int));
            dictionary.Add("ENABLE_POST_SECURE", typeof(bool));
            dictionary.Add("ELITE_BOSSES_ENABLED", typeof(bool));
            return dictionary;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v7()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("WINTER_THEME_ENABLED", typeof(bool));
            dictionary.Add("VENDOR_FIXED_SLOTS_ENABLED", typeof(bool));
            dictionary.Add("VENDOR_FIXED_SLOT1_ENTRY", typeof(string));
            dictionary.Add("VENDOR_FIXED_SLOT2_ENTRY", typeof(string));
            dictionary.Add("VENDOR_FIXED_SLOT3_ENTRY", typeof(string));
            dictionary.Add("VENDOR_MEGA_BOX_BUNDLES", typeof(Dictionary<string, double>));
            dictionary.Add("VENDOR_PET_BUNDLES", typeof(Dictionary<string, MinMaxInt>));
            dictionary.Add("PETS", typeof(List<PetConfig>));
            dictionary.Add("PET_LEVELUP_DUPLICATE_REQUIREMENTS", typeof(List<int>));
            dictionary.Add("PET_POWER_INCREASE_PER_LEVEL", typeof(double));
            dictionary.Add("MISSIONS", typeof(Dictionary<string, MissionConfig>));
            dictionary.Add("MISSION_BASE_COOLDOWN_SECONDS", typeof(long));
            dictionary.Add("MISSION_BIG_PRIZE_CHEST_TYPE", typeof(ChestType));
            dictionary.Add("NUM_COMPLETED_MISSIONS_REQUIRED_FOR_BIG_PRIZE_FTUE", typeof(int));
            dictionary.Add("MISSION_NOTIFICATIONS_ENABLED", typeof(bool));
            dictionary.Add("MISSION_BIG_PRIZE_INSTANT_SHOP_AVAILABILITY", typeof(bool));
            dictionary.Add("MISSIONS_FTUE", typeof(List<MissionConfig>));
            return dictionary;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v8()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("HIGH_FREQUENT_PROMOTION_UPDATES", typeof(bool));
            dictionary.Add("ANDROID_VIDEO_AD_SESSION_TIMEOUT_SECONDS", typeof(long));
            return dictionary;
        }

        public static Dictionary<string, System.Type> FieldsRemoteContentVersion_v9()
        {
            Dictionary<string, System.Type> dictionary = new Dictionary<string, System.Type>();
            dictionary.Add("CHEST_DROP_MAX_FLOOR", typeof(Dictionary<ChestType, int>));
            dictionary.Add("HIGHEST_FLOOR_COINS_MULTIPLIER", typeof(double));
            dictionary.Add("CTUT005_ENABLED", typeof(bool));
            dictionary.Add("ITEM_SELL_COIN_GAIN_OFFSET_BY_RARITY", typeof(Dictionary<int, double>));
            dictionary.Add("BOSS_AUTO_SUMMON_ENABLED", typeof(bool));
            dictionary.Add("ALLOW_BOSS_AUTO_SUMMON_IN_NEW_FLOORS", typeof(bool));
            dictionary.Add("DUNGEON_BOOST_BOX_FIRST_RUN_MIN_SPAWN_COUNT_FLOOR", typeof(int));
            dictionary.Add("DUNGEON_BOOST_BOX_FIRST_RUN_MAX_SPAWN_COUNT_FLOOR", typeof(int));
            dictionary.Add("DUNGEON_BOOST_EMPTY_BOX_COIN_GAIN_CONTROLLER", typeof(float));
            dictionary.Add("SHOW_SHOP_IN_SLIDING_MENU", typeof(bool));
            dictionary.Add("COMBAT_STATS_ENABLED", typeof(bool));
            dictionary.Add("NUM_VISIBLE_MENU_MILESTONES", typeof(int));
            dictionary.Add("ALLOW_VENDOR_TASKPANEL_NOTIFIER", typeof(bool));
            dictionary.Add("SHOW_SHOP_BUTTON_IN_TOP", typeof(bool));
            dictionary.Add("DISABLE_VENDOR_ADS_CONFIRMATION_POPUP", typeof(bool));
            return dictionary;
        }

        public void GetActiveMissionsIds(ref List<string> output)
        {
            output.Clear();
            foreach (KeyValuePair<string, MissionConfig> pair in this.MISSIONS)
            {
                if (pair.Value.Enabled)
                {
                    output.Add(pair.Key);
                }
            }
        }

        public double GetAugmentationPrice(string id)
        {
            return MathUtil.ClampMin(Math.Floor(GameLogic.Binder.PlayerAugmentationResources.getResource(id).Price * App.Binder.ConfigMeta.VENDOR_AUGMENTATION_SALE_CONTROLLER), 1.0);
        }

        public double GetItemPowerMultiplier(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.UNSPECIFIED:
                    return 1.0;

                case ItemType.Weapon:
                    return this.ITEM_DAMAGE_MULTIPLIER;

                case ItemType.Armor:
                    return this.ITEM_HEALTH_MULTIPLIER;

                case ItemType.Cloak:
                    return this.ITEM_SKILLDAMAGE_MULTIPLIER;
            }
            return 1.0;
        }

        public double GetMissionRequirement(string missionId, int difficultyIndex)
        {
            if (!this.MISSIONS.ContainsKey(missionId))
            {
                return 1.0;
            }
            List<double> difficulty = this.MISSIONS[missionId].Difficulty;
            return difficulty[difficultyIndex % difficulty.Count];
        }

        public int GetNextGuaranteedFrenzyPotionDropFloor(int floor)
        {
            if (floor <= this.RETIREMENT_MIN_FLOOR)
            {
                return this.RETIREMENT_MIN_FLOOR;
            }
            if (this.FRENZY_BONUS_POTION_EVERY_X_FLOOR <= 0)
            {
                return 0x7fffffff;
            }
            int nextFloorWithBoss = ConfigDungeons.GetNextFloorWithBoss(this.FRENZY_BONUS_POTION_FIRST_FLOOR);
            while (floor > nextFloorWithBoss)
            {
                nextFloorWithBoss = ConfigDungeons.GetNextFloorWithBoss(nextFloorWithBoss + this.FRENZY_BONUS_POTION_EVERY_X_FLOOR);
            }
            return nextFloorWithBoss;
        }

        public int GetNextGuaranteedRewardBoxDropFloor(int floor)
        {
            if (this.REWARD_BOX_BONUS_EVERY_X_FLOOR <= 0)
            {
                return 0x7fffffff;
            }
            int nextFloorWithBoss = ConfigDungeons.GetNextFloorWithBoss(this.REWARD_BOX_BONUS_FIRST_FLOOR);
            while (floor > nextFloorWithBoss)
            {
                nextFloorWithBoss = ConfigDungeons.GetNextFloorWithBoss(nextFloorWithBoss + this.REWARD_BOX_BONUS_EVERY_X_FLOOR);
            }
            return nextFloorWithBoss;
        }

        public int GetNumCompletedMissionsRequiredForBigPrize(Player player)
        {
            return (((player.CumulativeRetiredHeroStats.MissionBigPrizesOpened + player.ActiveCharacter.HeroStats.MissionBigPrizesOpened) != 0) ? this.NUM_COMPLETED_MISSIONS_REQUIRED_FOR_BIG_PRIZE : this.NUM_COMPLETED_MISSIONS_REQUIRED_FOR_BIG_PRIZE_FTUE);
        }

        public PetConfig GetPetConfig(string petId)
        {
            for (int i = 0; i < this.PETS.Count; i++)
            {
                if (this.PETS[i].Id == petId)
                {
                    return this.PETS[i];
                }
            }
            return null;
        }

        public string GetRandomActivePetId()
        {
            sm_tempCandidateList.Clear();
            for (int i = 0; i < this.PETS.Count; i++)
            {
                if (this.PETS[i].Enabled)
                {
                    sm_tempCandidateList.Add(this.PETS[i].Id);
                }
            }
            if (sm_tempCandidateList.Count == 0)
            {
                return "Pet001";
            }
            return LangUtil.GetRandomValueFromList<string>(sm_tempCandidateList);
        }

        public string GetRandomTournamentUpgrade()
        {
            if (this.TOURNAMENT_UPGRADES.Count <= 0)
            {
                return null;
            }
            sm_tempWeightDict.Clear();
            foreach (KeyValuePair<string, TournamentUpgrade> pair in this.TOURNAMENT_UPGRADES)
            {
                if (pair.Value.Weight > 0)
                {
                    sm_tempWeightDict.Add(pair.Key, pair.Value.Weight);
                }
            }
            if (sm_tempWeightDict.Count <= 0)
            {
                return null;
            }
            return LangUtil.GetKeyFromDictionaryWithWeights<string>(sm_tempWeightDict, null);
        }

        public TournamentUpgrade GetTournamentUpgrade(string id)
        {
            if (this.TOURNAMENT_UPGRADES.ContainsKey(id))
            {
                return this.TOURNAMENT_UPGRADES[id];
            }
            return null;
        }

        public float GetTournamentUpgradeModifier(string id, bool isEpic, int numMilestonesCompleted)
        {
            TournamentUpgrade tournamentUpgrade = this.GetTournamentUpgrade(id);
            if (tournamentUpgrade == null)
            {
                return 0f;
            }
            if (isEpic)
            {
                return (tournamentUpgrade.ModifierEpic + ((tournamentUpgrade.ModifierEpic * tournamentUpgrade.MilestoneMultiplier) * numMilestonesCompleted));
            }
            return (tournamentUpgrade.Modifier + ((tournamentUpgrade.Modifier * tournamentUpgrade.MilestoneMultiplier) * numMilestonesCompleted));
        }

        public VendorPriceData GetVendorPriceData(string shopEntryId)
        {
            if ((this.VENDOR_PRICE_DATA != null) && this.VENDOR_PRICE_DATA.ContainsKey(shopEntryId))
            {
                return this.VENDOR_PRICE_DATA[shopEntryId];
            }
            return null;
        }

        public double HighestFloorCoins(Player player)
        {
            CharacterInstance activeCharacter = player.ActiveCharacter;
            double numRankUps = this.HIGHEST_FLOOR_COINS_MULTIPLIER;
            return Math.Floor(Math.Max(player.itemCumulativeUpgradeCost(activeCharacter.HighestLevelItemOwnedAtFloorStart.ItemType, activeCharacter.HighestLevelItemOwnedAtFloorStart.Level, activeCharacter.HighestLevelItemOwnedAtFloorStart.Rank, numRankUps), this.COIN_BUNDLE_MINIMUM_BASE_VALUE * numRankUps));
        }

        public bool IsActivePetId(string petId)
        {
            for (int i = 0; i < this.PETS.Count; i++)
            {
                if (this.PETS[i].Enabled && this.PETS[i].Id.Equals(petId))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsBasicBossChest(ChestType chestType)
        {
            for (int i = 0; i < BOSS_CHESTS_BASIC.Length; i++)
            {
                if (BOSS_CHESTS_BASIC[i] == chestType)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsBossChest(ChestType chestType)
        {
            for (int i = 0; i < BOSS_CHESTS.Count; i++)
            {
                if (((ChestType) BOSS_CHESTS[i]) == chestType)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsEventBossChest(ChestType chestType)
        {
            for (int i = 0; i < BOSS_CHESTS_EVENT.Length; i++)
            {
                if (BOSS_CHESTS_EVENT[i] == chestType)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsMissionIdActive(string missionId)
        {
            return (this.MISSIONS.ContainsKey(missionId) && this.MISSIONS[missionId].Enabled);
        }

        public static bool IsMysteryChest(ChestType chestType)
        {
            for (int i = 0; i < MYSTERY_CHESTS.Length; i++)
            {
                if (MYSTERY_CHESTS[i] == chestType)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsRetirementChest(ChestType chestType)
        {
            for (int i = 0; i < RETIREMENT_CHESTS.Length; i++)
            {
                if (RETIREMENT_CHESTS[i] == chestType)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSpecialBossChest(ChestType chestType)
        {
            for (int i = 0; i < BOSS_CHESTS_SPECIAL.Length; i++)
            {
                if (BOSS_CHESTS_SPECIAL[i] == chestType)
                {
                    return true;
                }
            }
            return false;
        }

        public int ItemEvolveCurve(int evolveRank)
        {
            return ((evolveRank - 1) * 2);
        }

        public double ItemInstantUpgradeCostCurve()
        {
            return this.INSTANT_ITEM_UPGRADE_PRICE;
        }

        public double ItemPowerCurve(ItemType itemType, int level, int rank)
        {
            return Math.Floor(this.ProgressValueCurveLevel(level + rank) * this.GetItemPowerMultiplier(itemType));
        }

        public double ItemRerollDiamondCostCurve(int rarity, int rerollCounter)
        {
            return (this.REROLL_PRICE_BASE_PER_RARITY[rarity] + (this.REROLL_PRICE_INCREMENT_PER_RARITY[rarity] * rerollCounter));
        }

        public void ItemSellCurve(CharacterInstance character, ItemType itemType, int rarity, int level, int rank, out double tokens, out double coins)
        {
            int num = Mathf.Max(rank + ((int) this.ITEM_SELL_COIN_GAIN_OFFSET_BY_RARITY[rarity]), 0);
            coins = Math.Floor(this.ItemUpgradeCostCurve(itemType, level, num) * this.COIN_GAIN_CONTROLLER);
            tokens = 0.0;
        }

        public double ItemTokenValue(CharacterInstance character)
        {
            double baseAmount = this.ITEM_TOKEN_VALUE;
            return CharacterStatModifierUtil.ApplyTokenBonuses(character, baseAmount);
        }

        public int ItemUnlockFloor(int floor)
        {
            int num = UnityEngine.Random.Range(ITEM_UNLOCK_LEVEL_OFFSET_MIN, ITEM_UNLOCK_LEVEL_OFFSET_MAX + 1);
            return (floor + num);
        }

        public double ItemUpgradeCostCurve(ItemType itemType, int level, int rank)
        {
            double num = this.ProgressCostCurveLevel(level + rank) * this.ITEM_BASECOST_MULTIPLIER;
            return Math.Floor(num * ITEM_COST_MULTIPLIERS[itemType]);
        }

        public double LeaderboardCoins(Player player)
        {
            CharacterInstance activeCharacter = player.ActiveCharacter;
            double numRankUps = this.LEADERBOARD_COINS_MULTIPLIER;
            return Math.Floor(Math.Max(player.itemCumulativeUpgradeCost(activeCharacter.HighestLevelItemOwnedAtFloorStart.ItemType, activeCharacter.HighestLevelItemOwnedAtFloorStart.Level, activeCharacter.HighestLevelItemOwnedAtFloorStart.Rank, numRankUps), this.COIN_BUNDLE_MINIMUM_BASE_VALUE * numRankUps));
        }

        public double MinionCoinDropCurve(int floor)
        {
            return Math.Floor(this.ProgressValueCurveFloor((double) floor) * this.MINION_COINDROP_MULTIPLIER);
        }

        public double MinionDamagePerHitCurve(int floor, double progressDifficultyExponent)
        {
            return Math.Floor(this.ProgressDifficultyCurveFloor((double) floor, progressDifficultyExponent) * this.MINION_DAMAGE_MULTIPLIER);
        }

        public double MinionLifeCurve(int floor, double progressDifficultyExponent)
        {
            return Math.Floor(this.ProgressDifficultyCurveFloor((double) floor, progressDifficultyExponent) * this.MINION_HEALTH_MULTIPLIER);
        }

        public double MultikillCoinGainCurve(int killCount, int floorNumber)
        {
            return (this.MinionCoinDropCurve(floorNumber) * killCount);
        }

        public int PassiveMaxMinionCoinDropCount()
        {
            return this.PASSIVE_MAX_MINION_COIN_DROP_COUNT;
        }

        public double PassiveMinionKillFrequencySeconds()
        {
            return this.PASSIVE_MINION_KILL_FREQUENCY_SECONDS;
        }

        public int PetRequiredDuplicatesForLevelUp(int level)
        {
            if (level == 0)
            {
                return 0;
            }
            if (level <= this.PET_LEVELUP_DUPLICATE_REQUIREMENTS.Count)
            {
                return this.PET_LEVELUP_DUPLICATE_REQUIREMENTS[level - 1];
            }
            return this.PET_LEVELUP_DUPLICATE_REQUIREMENTS[this.PET_LEVELUP_DUPLICATE_REQUIREMENTS.Count - 1];
        }

        public double PlayerUpgradeCostCurve(int rank)
        {
            return Math.Ceiling((25.0 * rank) * rank);
        }

        public float PlayerUpgradePowerBonusModifier(int rank)
        {
            if (rank == 0)
            {
                return 0f;
            }
            return (0.125f * Mathf.Pow(1.75f, (float) rank));
        }

        public double ProgressCostCurveFloor(double floor)
        {
            return (this.ProgressValueCurveFloor(floor) * Math.Pow(this.PROGRESS_COST_EXPONENT, floor));
        }

        public double ProgressCostCurveLevel(int level)
        {
            return this.ProgressCostCurveFloor(((double) level) / ((double) this.LEVEL_TO_FLOOR_MULTIPLIER));
        }

        public double ProgressDifficultyCurveFloor(double floor, double progressDifficultyExponent)
        {
            return (this.ProgressValueCurveFloor(floor) * Math.Pow(progressDifficultyExponent, floor));
        }

        public double ProgressValueCurveFloor(double floor)
        {
            return ((this.ProgressValueHelper() * Math.Pow(this.PROGRESS_VALUE_EXPONENT, floor)) - this.ProgressValueHelper());
        }

        public double ProgressValueCurveLevel(int level)
        {
            return this.ProgressValueCurveFloor(((double) level) / ((double) this.LEVEL_TO_FLOOR_MULTIPLIER));
        }

        public double ProgressValueHelper()
        {
            return (1.0 / (this.PROGRESS_VALUE_EXPONENT - 1.0));
        }

        public double RankUpRewardGems(int rank)
        {
            return (double) this.XP_RANK_REWARD_GEMS;
        }

        public double RetirementTokenReward(CharacterInstance character, int floor)
        {
            if (floor < 20)
            {
                return Math.Floor(CharacterStatModifierUtil.ApplyTokenBonuses(character, 2.0));
            }
            double baseAmount = this.RETIREMENT_REWARD_BASE + ((floor - this.RETIREMENT_MIN_FLOOR) * this.RETIREMENT_REWARD_INCREMENT);
            double d = CharacterStatModifierUtil.ApplyTokenBonuses(character, baseAmount);
            if (d < this.RETIREMENT_REWARD_BASE)
            {
                d = this.RETIREMENT_REWARD_BASE;
            }
            return Math.Max(Math.Floor(d), 1.0);
        }

        public int RunestoneEvolveCurve(int targetLevel)
        {
            return ((targetLevel - 1) * 2);
        }

        public double RunestoneTokenGainCurve(int rarity)
        {
            return Math.Floor(rarity * 5.0);
        }

        public float SkillUpgradeDamageBonus(int rank)
        {
            return Mathf.Max((float) ((rank - 1) * 0.5f), (float) 0f);
        }

        public double SkillUpgradeDustCost(int rank)
        {
            return (double) (20 + ((rank - 1) * 5));
        }

        public double TokenBundleSize(Player player, string shopEntryId)
        {
            double num = App.Binder.ConfigMeta.VENDOR_TOKEN_BUNDLES[shopEntryId];
            double num2 = player.getHighestTokenGainWithRetirement() * num;
            return MathUtil.RoundToSignificantDigits(Math.Max(num2, App.Binder.ConfigMeta.VENDOR_TOKEN_BUNDLE_MINIMUM_SIZE[shopEntryId]), 2);
        }

        public double XpFromBossKill(int floor, bool isHighestFloor)
        {
            double num = this.XP_GAIN_BOSS + (floor * this.XP_BOSS_FLOORBONUS);
            if (!isHighestFloor)
            {
                num *= this.XP_BOSS_OLDMULTIPLIER;
            }
            return Math.Floor(num * this.XP_GAIN_CONTROLLER);
        }

        public double XpFromMinionKill(int floor)
        {
            return Math.Floor((double) (this.XP_GAIN_MINION * this.XP_GAIN_CONTROLLER));
        }

        public double XpRequiredForRankUp(int currentRank)
        {
            return Math.Floor((double) (this.XP_RANK_BASE * Mathf.Pow(this.XP_EXP, (float) currentRank)));
        }
    }
}

