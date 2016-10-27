namespace App
{
    using GameLogic;
    using Service;
    using System;
    using UnityEngine;

    public static class ConfigApp
    {
        public static int AndroidBundleVersionCode = 0x2e;
        public static bool AssetBundlesEnabled;
        public static string BundleIdentifier = "com.koplagames.kopla01";
        public static string BundleVersion = "1.6.3";
        public static bool CHEAT_BOSS_ALWAYS_SUMMONABLE;
        public static bool CHEAT_BYPASS_LOCA;
        public static bool CHEAT_CONSOLE_ENABLED;
        public static bool CHEAT_DISABLE_SHOP_AUTO_REFRESH;
        public static DungeonEventType CHEAT_DUNGEON_EVENT_TYPE;
        public static bool CHEAT_EDITOR_APP_FOCUS_SIMULATES_PAUSING;
        public static bool CHEAT_EDITOR_AUDIO_MUTE;
        public static bool CHEAT_FAST_NOTIFICATIONS;
        public static bool CHEAT_IAP_TEST_MODE;
        public static bool CHEAT_IGNORE_ALL_DAMAGE;
        public static bool CHEAT_MARKETING_MODE_ENABLED;
        public static bool CHEAT_NO_SKILL_COOLDOWNS;
        public static bool CHEAT_POPUP_ENABLED;
        public static bool CHEAT_SHOW_FPS_COUNTER;
        public static DeviceQualityType CHEAT_SIMULATE_DEVICE_QUALITY;
        public static bool CHEAT_SKIP_TUTORIALS;
        public static bool CHEAT_SPEED_BUTTON_VISIBLE;
        public static bool CHEAT_UNLIMITED_TOURNAMENT_DONATES;
        public static bool CHEAT_USE_PRODUCTION_MASTER_REMOTE_CONTENT_AS_DEFAULT;
        public static string CompanyName = "Kopla Games";
        public static string FacebookAppIdDevelopment = "408588599341576";
        public static string FacebookAppIdProduction = "408208149379621";
        public static string GameCenterLeaderboardId = "kopla01_highest_floor";
        public static string GooglePlayLeaderboardId = "CgkI7oWAl_wcEAIQCw";
        public static bool GoogleStoreAutoVerifySignatures = true;
        public static string GoogleStorePublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhRfKZqqCqDwcSo3MfHx/5wCfy1uPnqbS8ngeVbnaNH6WgGSSwuYHHHt+lybThoJoeav0jsJfaCT73SWBxMwEIGXjG2QyiZQ0NNzdEhsdyCrvWpjfPagC+et9Dn1TqekBiUXXdA2yoYRc/4pVYsUI36OpEqsB9+W+aFIrjOqA3Z7olDmUqOevYFCulbS+wp6wFyRMe2l4cQ5ySrePrFSJAYQen/wf/9V91Wk+gx8ZluDosJULnUPeeVZtbpNk457kZWMQkaYj6aPAcmNsbrCzoXnc3dwkCW+RBGEpjbT0+1UjtzWwZqk/qiWxEFXyblp1XUn02vqmxDh3IHsHBCudHwIDAQAB";
        public static int InternalClientVersion = 11;
        public static bool LocalNotificationsEnabled = true;
        public static string LocalPlayerProfileFileBackup = "player.json.bak";
        public static string LocalPlayerProfileFilePrimary = "player.json";
        public static bool MigratePlayerProgressUponDataModelVersionChange = true;
        public static bool PersistentStorageEncryptionEnabled = true;
        public static bool ProductionBuild = true;
        public static int ProductionBuildDefaultRemoteContentVersion = 0x44c;
        public static string ProductName = "Nonstop";
        public static bool SocialSystemEnabled = true;
        public static string SupersonicAppKey = "4691b625";

        public static bool IsStableBuild()
        {
            return (ProductionBuild && false);
        }

        public static MasterRemoteContent LoadDefaultMasterRemoteContent()
        {
            TextAsset asset;
            if ((ProductionBuild || IsStableBuild()) || CHEAT_USE_PRODUCTION_MASTER_REMOTE_CONTENT_AS_DEFAULT)
            {
                asset = ResourceUtil.LoadSafe<TextAsset>("RemoteContent/current_v" + ProductionBuildDefaultRemoteContentVersion, false);
            }
            else
            {
                asset = ResourceUtil.LoadSafe<TextAsset>("RemoteContent/current", false);
            }
            return JsonUtils.Deserialize<MasterRemoteContent>(asset.text, true);
        }
    }
}

