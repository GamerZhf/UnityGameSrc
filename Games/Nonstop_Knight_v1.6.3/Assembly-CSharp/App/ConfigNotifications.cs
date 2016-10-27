namespace App
{
    using System;

    public static class ConfigNotifications
    {
        public static NotificationEntry BossSummonTimerNotification;
        public static NotificationEntry CoinCollectedTimerNotification;
        public static NotificationEntry NewMissionAvailableNotification;
        public static NotificationEntry RetentionD1Notification;
        public static NotificationEntry RetentionD7Notification;

        static ConfigNotifications()
        {
            NotificationEntry entry = new NotificationEntry();
            entry.Id = "BossSummonTimerNotification";
            entry.Title = ConfigLoca.NOTIFICATIONS_PROMPT_PLAY;
            entry.Description = ConfigLoca.NOTIFICATIONS_BOSS_SUMMON_TIMER;
            entry.BadgeNumber = 1;
            BossSummonTimerNotification = entry;
            entry = new NotificationEntry();
            entry.Id = "CoinsCollectedTimerNotification";
            entry.Title = ConfigLoca.NOTIFICATIONS_PROMPT_PLAY;
            entry.Description = ConfigLoca.NOTIFICATIONS_COINS_COLLECTED_TIMER;
            entry.BadgeNumber = 1;
            CoinCollectedTimerNotification = entry;
            entry = new NotificationEntry();
            entry.Id = "RetentionD1Notification";
            entry.Title = ConfigLoca.NOTIFICATIONS_PROMPT_PLAY;
            entry.Description = ConfigLoca.NOTIFICATIONS_RETENTION_D1;
            entry.TimeOffsetTicks = TimeUtil.DaysToTicks(1);
            entry.BadgeNumber = 2;
            RetentionD1Notification = entry;
            entry = new NotificationEntry();
            entry.Id = "RetentionD7Notification";
            entry.Title = ConfigLoca.NOTIFICATIONS_PROMPT_PLAY;
            entry.Description = ConfigLoca.NOTIFICATIONS_RETENTION_D7;
            entry.TimeOffsetTicks = TimeUtil.DaysToTicks(7);
            entry.BadgeNumber = 3;
            RetentionD7Notification = entry;
            entry = new NotificationEntry();
            entry.Id = "NewMissionAvailableNotification";
            entry.Title = ConfigLoca.NOTIFICATIONS_PROMPT_PLAY;
            entry.Description = ConfigLoca.NOTIFICATIONS_NEW_BOUNTY;
            entry.BadgeNumber = 1;
            NewMissionAvailableNotification = entry;
        }

        public class NotificationEntry
        {
            public int BadgeNumber;
            public NotificationCategory Category;
            public string Description;
            public string Id;
            public long TimeOffsetTicks;
            public string Title;
        }
    }
}

