namespace App
{
    using Android;
    using GameLogic;
    using System;
    using System.Text;
    using UnityEngine;

    public class LocalNotificationSystem : MonoBehaviour
    {
        private void clearLocalNotifications()
        {
            if (App.Binder.NotificationRegister.Registered)
            {
                AndroidNotificationServices.CancelAllLocalNotifications();
                AndroidNotificationServices.ClearLocalNotifications();
            }
        }

        private void clearRemoteNotifications()
        {
            AndroidNotificationServices.ClearRemoteNotifications();
        }

        protected void OnApplicationPause(bool paused)
        {
            if (App.Binder.AppContext.systemsShouldReactToApplicationPause())
            {
                if (paused)
                {
                    this.scheduleLocalNotifications();
                }
                else
                {
                    this.clearLocalNotifications();
                }
                this.clearRemoteNotifications();
            }
        }

        protected void OnApplicationQuit()
        {
            this.scheduleLocalNotifications();
            this.clearRemoteNotifications();
        }

        private void printScheduledLocalNotifications()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Scheduled local notifications:\n");
            for (int i = 0; i < AndroidNotificationServices.scheduledLocalNotifications.Length; i++)
            {
                builder.Append(AndroidNotificationServices.scheduledLocalNotifications[i]);
            }
        }

        private void scheduleLocalNotification(string id, string title, string description, long offsetTicks, int badgeNumber)
        {
            if (App.Binder.NotificationRegister.Registered)
            {
                AndroidLocalNotification notification = new AndroidLocalNotification();
                notification.fireDate = DateTime.Now + new TimeSpan(offsetTicks);
                notification.applicationIconBadgeNumber = badgeNumber;
                notification.hasAction = true;
                notification.alertBody = description;
                notification.alertAction = title;
                AndroidNotificationServices.ScheduleLocalNotification(notification);
            }
        }

        private void scheduleLocalNotifications()
        {
            if (App.Binder.NotificationRegister.Registered)
            {
                this.clearLocalNotifications();
                long timeOffsetTicks = ConfigNotifications.RetentionD1Notification.TimeOffsetTicks;
                if (ConfigApp.CHEAT_FAST_NOTIFICATIONS)
                {
                    int seconds = ((int) TimeUtil.TicksToSeconds(timeOffsetTicks)) / 0xe10;
                    timeOffsetTicks = TimeUtil.SecondsToTicks(seconds);
                }
                this.scheduleLocalNotification(ConfigNotifications.RetentionD1Notification.Id, _.L(ConfigNotifications.RetentionD1Notification.Title, null, true), _.L(ConfigNotifications.RetentionD1Notification.Description, null, true), timeOffsetTicks, ConfigNotifications.RetentionD1Notification.BadgeNumber);
                timeOffsetTicks = ConfigNotifications.RetentionD7Notification.TimeOffsetTicks;
                if (ConfigApp.CHEAT_FAST_NOTIFICATIONS)
                {
                    int num4 = ((int) TimeUtil.TicksToSeconds(timeOffsetTicks)) / 0xe10;
                    timeOffsetTicks = TimeUtil.SecondsToTicks(num4);
                }
                this.scheduleLocalNotification(ConfigNotifications.RetentionD7Notification.Id, _.L(ConfigNotifications.RetentionD7Notification.Title, null, true), _.L(ConfigNotifications.RetentionD7Notification.Description, null, true), timeOffsetTicks, ConfigNotifications.RetentionD7Notification.BadgeNumber);
                if ((GameLogic.Binder.GameState != null) && (GameLogic.Binder.GameState.Player != null))
                {
                    Player player = GameLogic.Binder.GameState.Player;
                    Player.PassiveProgress progress = player.getPassiveProgress(player.getLastCompletedFloor(false) + 1, 0x7fffffffffffffffL, true);
                    int b = Mathf.RoundToInt((progress.NumMinionKillsOnBossProgressStop * ((float) App.Binder.ConfigMeta.PassiveMinionKillFrequencySeconds())) + (progress.NumBossKills * App.Binder.ConfigMeta.PASSIVE_BOSS_KILL_FREQUENCY_SECONDS));
                    int num6 = Mathf.Max(App.Binder.ConfigMeta.BOSS_SUMMON_NOTIFICATION_TIMER_MIN_SECONDS, b);
                    if (ConfigApp.CHEAT_FAST_NOTIFICATIONS)
                    {
                        num6 /= 0xe10;
                    }
                    if (num6 > 0)
                    {
                        this.scheduleLocalNotification(ConfigNotifications.BossSummonTimerNotification.Id, _.L(ConfigNotifications.BossSummonTimerNotification.Title, null, true), _.L(ConfigNotifications.BossSummonTimerNotification.Description, null, true), TimeUtil.SecondsToTicks(num6), ConfigNotifications.BossSummonTimerNotification.BadgeNumber);
                    }
                    int num7 = Mathf.RoundToInt((progress.NumMinionKills * ((float) App.Binder.ConfigMeta.PassiveMinionKillFrequencySeconds())) + (progress.NumBossKills * App.Binder.ConfigMeta.PASSIVE_BOSS_KILL_FREQUENCY_SECONDS));
                    if (ConfigApp.CHEAT_FAST_NOTIFICATIONS)
                    {
                        num7 /= 0xe10;
                    }
                    if (num7 > 0)
                    {
                        this.scheduleLocalNotification(ConfigNotifications.CoinCollectedTimerNotification.Id, _.L(ConfigNotifications.CoinCollectedTimerNotification.Title, null, true), _.L(ConfigNotifications.CoinCollectedTimerNotification.Description, null, true), TimeUtil.SecondsToTicks(num7), ConfigNotifications.CoinCollectedTimerNotification.BadgeNumber);
                    }
                    if (App.Binder.ConfigMeta.MISSION_NOTIFICATIONS_ENABLED && player.Missions.hasMissionOnCooldown())
                    {
                        int num8 = Mathf.RoundToInt((float) player.Missions.getMinRemainingCooldownSeconds());
                        if (ConfigApp.CHEAT_FAST_NOTIFICATIONS)
                        {
                            num8 /= 0xe10;
                        }
                        if (num8 > 0)
                        {
                            this.scheduleLocalNotification(ConfigNotifications.NewMissionAvailableNotification.Id, _.L(ConfigNotifications.NewMissionAvailableNotification.Title, null, true), _.L(ConfigNotifications.NewMissionAvailableNotification.Description, null, true), TimeUtil.SecondsToTicks(num8), ConfigNotifications.NewMissionAvailableNotification.BadgeNumber);
                        }
                    }
                }
                this.printScheduledLocalNotifications();
            }
        }
    }
}

