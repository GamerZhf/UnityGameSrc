namespace Service
{
    using App;
    using System;

    public static class Binder
    {
        public static Service.AchievementThirdPartyService AchievementThirdPartyService;
        public static IAdsSystem AdsSystem;
        public static Service.ContentService ContentService;
        public static Service.IEventBus EventBus;
        public static Service.FacebookAdapter FacebookAdapter;
        public static Service.InboxProcessor InboxProcessor;
        public static Service.LeaderboardService LeaderboardService;
        public static Service.LeaderboardUpdateService LeaderboardUpdateService;
        public static DummyLogger Logger = new DummyLogger();
        public static Service.LoggingService LoggingService;
        public static Service.NSKRTLManager NSKRTLManager;
        public static Service.PlayerService PlayerService;
        public static IPromotionEventSystem PromotionEventSystem;
        public static Service.PromotionManager PromotionManager = new Service.PromotionManager();
        public static Service.PromotionService PromotionService;
        public static Service.RemoteNotificationSystem RemoteNotificationSystem;
        public static Service.SdkController SdkController;
        public static Service.ServerSelection ServerSelection;
        public static Service.ServerTime ServerTime = new Service.ServerTime();
        public static Service.ServiceContext ServiceContext;
        public static Service.ServiceWatchdog ServiceWatchdog;
        public static ISessionData SessionData;
        public static Service.ShopManager ShopManager;
        public static Service.ShopService ShopService;
        public static Service.TaskManager TaskManager;
        public static Service.TournamentService TournamentService;
        public static Service.TournamentSystem TournamentSystem;
        public static Service.TrackingService TrackingService;
        public static Service.TrackingSystem TrackingSystem;
    }
}

