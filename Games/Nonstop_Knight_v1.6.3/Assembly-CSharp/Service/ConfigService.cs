namespace Service
{
    using System;

    public static class ConfigService
    {
        public static ServerEntry[] DevServerSelectionList = new ServerEntry[] { new ServerEntry("nsk-dev-1", "Development", string.Empty, "https://nsk-dev-1-game.testing.flarecloud.net"), new ServerEntry("nsk-dev-2", "Stable", string.Empty, "https://nsk-dev-2-game.testing.flarecloud.net"), new ServerEntry("nsk-dev-3", "Kopla - Unstable", string.Empty, "https://nsk-dev-3-game.testing.flarecloud.net"), new ServerEntry("nsk-dev-4", "Kopla - Stable", string.Empty, "https://nsk-dev-4-game.testing.flarecloud.net"), new ServerEntry("localhost", "Localhost", string.Empty, "http://localhost:8080"), new ServerEntry("boerje", "Boerje", string.Empty, "http://172.16.2.37:8080"), new ServerEntry("richard", "Richard", string.Empty, "http://172.16.5.14:8080"), new ServerEntry("richard2", "RichardSurface", string.Empty, "http://172.16.5.15:8080"), new ServerEntry("richard3", "RichardSMacHome", string.Empty, "http://192.168.0.13:8080"), new ServerEntry("timo", "Timo", string.Empty, "http://172.16.2.75:8080") };
        public static int LEADERBOARD_LOCAL_REFRESH = 540;
        public static int LEADERBOARD_SOCIAL_REFRESH = 240;
        public static string LiveServerUrl = "https://nsk-us-vir-1-game.flarecloud.net";
        public static ServerEntry OFFLINE_SERVER_ENTRY = new ServerEntry("offline", string.Empty, string.Empty, "http://offline");
        public static bool PROMOTION_EVENTS_ENABLED = true;
        public static int TOURNAMENT_UPSERT_REFRESH = 0x2f;
        public static int TRACKING_INITIAL_WARMUP_DELAY_SECONDS = 30;
        public static int TRACKING_QUEUE_FLUSH_INTERVAL = 3;
        public static int TRACKING_QUEUE_SAVE_INTERVAL = 30;
        public static int TRACKING_QUEUE_SIZE = 100;
        public static bool USE_SERVER_TIME_AS_GAME_TIME = false;
    }
}

